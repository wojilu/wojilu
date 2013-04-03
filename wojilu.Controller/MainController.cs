/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Config;
using wojilu.Common;
using wojilu.Common.Onlines;
using wojilu.Common.AppBase;

using wojilu.Members.Users.Service;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Members.Sites.Domain;

using wojilu.Web.Controller.Common;
using wojilu.Web.Controller.Users.Admin;

using wojilu.OAuth;

namespace wojilu.Web.Controller {

    public class MainController : ControllerBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( MainController ) );

        public IUserService userService { get; set; }
        public IUserConfirmService confirmService { get; set; }
        public ILoginService loginService { get; set; }
        public IFriendService friendService { get; set; }
        public IInviteService inviteService { get; set; }

        public MainController() {

            confirmService = new UserConfirmService();
            userService = new UserService();
            loginService = new LoginService();
            friendService = new FriendService();
            inviteService = new InviteService();

            HidePermission( typeof( SecurityController ) );

        }

        public void Index() {
            redirect( new SiteInitController().Index );
        }

        public void LoginBox() {

            if (ctx.viewer.IsLogin) {
                echo( "对不起，您已经登录" );
                return;
            }

            set( "loginAction", Link.To( Site.Instance, new MainController().CheckLogin ) );
            set( "regLink", Link.To( Site.Instance, new RegisterController().Register ) );
            set( "resetPwdLink", Link.To( Site.Instance, new wojilu.Web.Controller.Common.ResetPwdController().StepOne ) );

            IBlock block = getBlock( "validBox" );
            if (config.Instance.Site.LoginNeedImgValidation) {
                block.Set( "valideCode", Html.Captcha );
                block.Next();
            }

            set( "returnUrl", ctx.Get( "returnUrl" ) );
        }

        public void Login() {

            if (ctx.viewer.IsLogin) {
                echo( "对不起，您已经登录" );
                return;
            }

            String returnUrl = getReturnUrl();
            if (strUtil.HasText( returnUrl ) &&
                (returnUrl.IndexOf( "frm=true" ) >= 0 || returnUrl.IndexOf( "nolayout=" ) >= 0)
                ) {
                run( LoginBox );
                return;

            }

            Page.Title = lang( "userLogin" );
            target( CheckLogin );

            set( "returnUrl", returnUrl );

            String lblUnRegisterTip = string.Format( lang( "unRegisterTip" ), to( new RegisterController().Register ) );
            set( "lblUnRegisterTip", lblUnRegisterTip );

            set( "resetPwdLink", to( new ResetPwdController().StepOne ) );

            setLoginValidationCode();

            IBlock confirmEmailBlock = getBlock( "sendConfirmEmail" );
            if (config.Instance.Site.EnableEmail) {
                confirmEmailBlock.Set( "resendLink", to( new Common.ActivationController().SendEmailLogin ) );
                confirmEmailBlock.Next();
            }

            load( "connectLogin", connectLogin );
        }

        public void connectLogin() {

            IBlock block = getBlock( "connectWrap" );
            List<AuthConnectConfig> xlist = AuthConnectConfig.GetEnabledList();
            if (xlist.Count == 0) return;

            String lnk = Link.To( Site.Instance, new ConnectController().Login ) + "?connectType=";
            xlist.ForEach( x => x.data.show = lnk + x.TypeFullName );
            block.BindList( "connects", "x", xlist );

            block.Next();
        }

        [HttpPost, DbTransaction]
        public void Logout() {
            ctx.web.UserLogout();
            OnlineStats.Instance.SubtractMemberCount();
            echoRedirect( lang( "logoutok" ), ctx.url.SiteAndAppPath );
        }

        [HttpPost, DbTransaction]
        public void CheckLogin() {

            if (ctx.viewer.IsLogin) {
                echo( "对不起，您已经登录" );
                return;
            }

            if (config.Instance.Site.LoginNeedImgValidation) {

                if (Html.Captcha.CheckError( ctx )) {
                    run( Login );
                    return;
                }
            }

            String user = ctx.Post( "txtUid" );
            String pwd = ctx.Post( "txtPwd" );

            if (strUtil.IsNullOrEmpty( user )) errors.Add( lang( "exUserName" ) );
            if (strUtil.IsNullOrEmpty( pwd )) errors.Add( lang( "exPwd" ) );
            if (errors.HasErrors) { run( Login ); return; }

            User member = userService.IsNameEmailPwdCorrect( user, pwd );
            if (member == null) {
                errors.Add( lang( "exUserNamePwdError" ) );
                run( Login );
                return;
            }

            if (userService.IsUserDeleted( member )) {
                errors.Add( lang( "exUser" ) );
                run( Login );
                return;
            }

            if (config.Instance.Site.UserNeedApprove && member.Status == MemberStatus.Approving) {
                errors.Add( "您的账号尚未经过审核，请耐心等候" );
                run( Login );
                return;
            }

            // 需要激活才能登录
            if (member.IsEmailConfirmed == 0 && config.Instance.Site.LoginType == LoginType.ActivationEmail) {
                ActivationController.AllowSendActivationEmail( ctx, member.Id );
                redirect( new ActivationController().SendEmailButton );
                return;
            }

            LoginTime expiration;
            if (ctx.PostIsCheck( "RememberMe" ) == 1)
                expiration = LoginTime.Forever;
            else
                expiration = LoginTime.Never;

            loginService.Login( member, expiration, ctx.Ip, ctx );

            echoRedirect( lang( "loginok" ), getSavedReturnUrl() );
        }


        public void ConfirmEmail() {

            String code = ctx.Get( "c" );

            User user = confirmService.Valid( code );
            if (user != null) {

                loginService.Login( user, LoginTime.Forever, ctx.Ip, ctx );

                echoRedirect( lang( "confirmok" ), sys.Path.Root );
            }
            else {
                content( "<div style=\"width:300px;margin:auto;padding:50px;font-size:28px;font-weight:bold;color:red;\">" + lang( "exConfirm" ) + "</div>" );
            }
        }

        private void setLoginValidationCode() {
            IBlock cblock = getBlock( "Captcha" );
            if (config.Instance.Site.LoginNeedImgValidation) {
                cblock.Set( "ValidationCode", Html.Captcha );
                cblock.Next();
            }
        }

        private String getReturnUrl() {

            String returnUrl = ctx.Get( "returnUrl" );
            if (strUtil.HasText( returnUrl )) return returnUrl;

            returnUrl = ((ctx.web.PathReferrer == null) ? string.Empty : ctx.web.PathReferrer.ToString());

            return returnUrl;
        }

        private String getSavedReturnUrl() {

            String returnUrl = ctx.Post( "returnUrl" );
            if (strUtil.IsNullOrEmpty( returnUrl )) {
                returnUrl = sys.Path.Root;
            }

            returnUrl = returnUrl.Replace( "&amp;", "&" );

            // 禁止跳转到注册页面
            String regLink = to( new RegisterController().Register );
            if (returnUrl.IndexOf( regLink ) >= 0) return sys.Path.Root;

            return returnUrl;
        }

        public void lost() {
            HideLayout( typeof( LayoutController ) );
            content( "wojilu.lostPage" );
        }

        public void LoginScript() {
            HideLayout( typeof( LayoutController ) );

            IBlock welcomeBlock = getBlock( "welcome" );
            IBlock loginBlock = getBlock( "login" );

            if (ctx.viewer.IsLogin) {

                welcomeBlock.Set( "user.Name", ctx.viewer.obj.Name );
                welcomeBlock.Set( "user.LogoutLink", t2( new MainController().Logout ) );
                welcomeBlock.Set( "user.HomeLink", Link.To( ctx.viewer.obj, new FeedController().My, -1 ) );
                welcomeBlock.Set( "user.SpaceLink", toUser( ctx.viewer.obj ) );
                welcomeBlock.Next();
            }
            else {

                loginBlock.Set( "ActionLink", t2( new MainController().CheckLogin ) );
                loginBlock.Set( "regLink", t2( new RegisterController().Register ) );
                loginBlock.Next();
            }

        }


    }
}

