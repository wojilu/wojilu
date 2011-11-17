/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Common;

using wojilu.Members.Users.Service;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Domain;

using wojilu.Web.Controller.Common;
using wojilu.Web.Controller.Users.Admin;
using wojilu.Common.Onlines;
using wojilu.Config;
using wojilu.Common.AppBase;

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


        public void Login() {


            Page.Title = lang( "userLogin" );
            target( CheckLogin );

            set( "returnUrl", getReturnUrl() );

            String lblUnRegisterTip = string.Format( lang( "unRegisterTip" ), to( new RegisterController().Register ) );
            set( "lblUnRegisterTip", lblUnRegisterTip );

            set( "resetPwdLink", to( new ResetPwdController().StepOne ) );

            setLoginValidationCode();
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
                echo( "您有帐号，并且已经登录" );
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
                errors.Add( lang( "exUsere" ) );
                run( Login );
                return;
            }

            if (config.Instance.Site.UserNeedApprove && member.Status == MemberStatus.Approving) {
                errors.Add( "您的账号尚未经过审核，请耐心等候" );
                run( Login );
                return;
            }

            // 需要激活才能登录
            if ( member.IsEmailConfirmed==0 && config.Instance.Site.LoginType == LoginType.ActivationEmail ) {
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

            //echoToParent( lang( "loginok" ), getSavedReturnUrl() );
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
                actionContent( "<div style=\"width:300px;margin:auto;padding:50px;font-size:28px;font-weight:bold;color:red;\">" + lang( "exConfirm" ) + "</div>" );
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

            return returnUrl;
        }

        public void lost() {
            HideLayout( typeof( LayoutController ) );
            actionContent( "wojilu.lostPage" );
        }

        public void LoginScript() {
            HideLayout( typeof( LayoutController ) );

            IBlock welcomeBlock = getBlock( "welcome" );
            IBlock loginBlock = getBlock( "login" );

            if (ctx.viewer.IsLogin) {

                welcomeBlock.Set( "user.Name", ctx.viewer.obj.Name );
                welcomeBlock.Set( "user.LogoutLink", t2( new MainController().Logout ) );
                welcomeBlock.Set( "user.HomeLink", Link.T2( ctx.viewer.obj, new FeedController().My, -1 ) );
                welcomeBlock.Set( "user.SpaceLink", Link.ToMember( ctx.viewer.obj ) );
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

