/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Config;
using wojilu.Common;
using wojilu.Common.AppBase;
using wojilu.Common.MemberApp.Interface;
using wojilu.Common.Menus.Interface;
using wojilu.Common.Resource;

using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;

using wojilu.Web.Controller.Common;
using wojilu.Web.Controller.Helpers;

namespace wojilu.Web.Controller {

    public class RegisterController : ControllerBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( RegisterController ) );

        public IUserService userService { get; set; }
        public IUserConfirmService confirmService { get; set; }
        public IConfirmEmail confirmEmail { get; set; }
        public ILoginService loginService { get; set; }
        public IInviteService inviteService { get; set; }

        public virtual IMemberAppService appService { get; set; }
        public virtual IMenuService menuService { get; set; }


        public RegisterController() {
            userService = new UserService();
            confirmService = new UserConfirmService();
            confirmEmail = new ConfirmEmail();
            loginService = new LoginService();

            appService = new UserAppService();
            menuService = new UserMenuService();

            inviteService = new InviteService();

            HidePermission( typeof( SecurityController ) );

        }

        public override void CheckPermission() {
            if (config.Instance.Site.RegisterType == RegisterType.Close) {
                echo( "对不起，网站已经关闭注册" );
            }
        }

        public void Invite( int userId ) {

            if (ctx.viewer.IsLogin) {
                echo( "对不起，您已经登录" );
                return;
            }

            String inviteCode = ctx.Get( "code" );
            Result result = inviteService.Validate( userId, inviteCode );
            if (result.HasErrors) {
                echo( result.ErrorsHtml );
                return;
            }

            User user = userService.GetById( userId );

            target( RegisterFriend, userId );
            set( "inviteCode", inviteCode );

            set( "userName", user.Name );
            set( "userPic", user.PicM );

            set( "loginLink", to( new MainController().Login ) );

        }

        [HttpPost]
        public void RegisterFriend( int friendId ) {

            if (ctx.viewer.IsLogin) {
                echo( "对不起，您已经登录" );
                return;
            }

            String inviteCode = ctx.Post( "inviteCode" );

            Result result = inviteService.Validate( friendId, inviteCode );
            if (result.HasErrors) {
                echo( result.ErrorsHtml );
                return;
            }

            User friend = userService.GetById( friendId );

            set( "friend.Name", friend.Name );
            set( "friend.Pic", friend.PicM );
            set( "friend.Id", friend.Id );
            set( "friend.Code", inviteCode );

            bindRegister();
        }

        public void Register() {

            if (ctx.viewer.IsLogin) {
                echo( "对不起，您已经登录" );
                return;
            }

            if (config.Instance.Site.RegisterType == RegisterType.CloseUnlessInvite) {
                echo( "对不起，您必须接受邀请才能注册" );
                return;
            }

            bindRegister();
            load( "connectLogin", new MainController().connectLogin );
        }


        private void bindRegister() {

            target( SaveReg );


            Page.Title = lang( "userReg" );

            String loginTip = string.Format( lang( "loginTip" ), to( new MainController().Login ) );
            set( "loginTip", loginTip );

            String userNameNote = string.Format( lang( "userNameNote" ),
                config.Instance.Site.UserNameLengthMin,
                config.Instance.Site.UserNameLengthMax );

            set( "userNameNote", userNameNote );

            set( "m.Name", ctx.Post( "Name" ) );

            set( "m.Password1", ctx.Post( "Password1" ) );
            set( "m.Password2", ctx.Post( "Password2" ) );
            set( "m.Email", ctx.Post( "Email" ) );
            set( "m.FriendUrl", ctx.Post( "FriendUrl" ) );

            int gender = ctx.PostInt( "Gender" );
            if (gender <= 0) gender = 1;

            String genderStr = Html.RadioList( AppResource.Gender, "Gender", "Name", "Value", gender );

            set( "m.Gender", genderStr );

            IBlock urlBlock = getBlock( "friendlyUrl" );
            IBlock subdomainBlock = getBlock( "subDomainFriendlyUrl" );


            if (Component.IsEnableUserSpace()) {

                if (MvcConfig.Instance.CheckDomainMap()) {
                    subdomainBlock.Set( "userUrlPrefix", SystemInfo.HostNoSubdomain );
                    subdomainBlock.Set( "isUrlValidLink", to( CheckUrlExist ) );
                    subdomainBlock.Next();
                }
                else {
                    urlBlock.Set( "userUrlPrefix", strUtil.TrimStart( strUtil.Append( ctx.url.SiteAndAppPath, "/" ), "http://" ) );
                    urlBlock.Set( "urlExt", MvcConfig.Instance.UrlExt );
                    urlBlock.Set( "isUrlValidLink", to( CheckUrlExist ) );
                    urlBlock.Next();

                }
            }

            // validation
            set( "isEmailValidLink", to( CheckEmailExist ) );
            set( "isNameValidLink", to( CheckUserExist ) );

            IBlock confirmEmailBlock = getBlock( "sendConfirmEmail" );
            if (config.Instance.Site.EnableEmail) {
                confirmEmailBlock.Set( "resendLink", to( new Common.ActivationController().SendEmailLogin ) );
                confirmEmailBlock.Next();
            }

            bindCaptcha();
        }

        private void bindCaptcha() {

            IBlock cblock = getBlock( "Captcha" );
            if (config.Instance.Site.RegisterNeedImgValidateion) {
                cblock.Set( "ValidationCode", Html.Captcha );
                cblock.Next();
            }
        }

        [HttpPost, DbTransaction]
        public void CheckEmailExist() {

            String email = ctx.Post( "Email" );

            if (userService.IsEmailExist( email )) {
                echoError( lang( "exEmailFound" ) );
                return;
            }

            echoJsonOk();
        }

        [HttpPost, DbTransaction]
        public void CheckUserExist() {

            String name = ctx.Post( "Name" );

            if (userService.IsNameReservedOrExist( name )) {
                echoError( lang( "exNameFound" ) );
                return;
            }

            echoJsonOk();
        }

        [HttpPost, DbTransaction]
        public void CheckUrlExist() {

            String url = ctx.Post( "FriendUrl" );
            if (userService.IsUrlReservedOrExist( url )) {
                echoJsonMsg( lang( "exUrlFound" ), false, "" );
            }
            else {
                echoJsonOk();
            }

        }

        //--------------------------------------------------------------------------------

        public void Done() {

            set( "email", ctx.Get( "email" ) );
            set( "domainName", config.Instance.Site.GetSmtpUserDomain() );

            String goUrl = WebUtils.getMailLink( ctx.Get( "email" ) );
            IBlock mblock = getBlock( "mailLink" );
            if (strUtil.HasText( goUrl )) {
                mblock.Set( "goUrl", goUrl );
                mblock.Next();
            }

            set( "resendLink", to( new Common.ActivationController().SendEmailLogin ) );
        }

        [HttpPost, DbTransaction]
        public void SaveReg() {

            if (ctx.viewer.IsLogin) {
                echo( "您有帐号，并且已经登录" );
                return;
            }

            if (config.Instance.Site.RegisterType == RegisterType.CloseUnlessInvite) {

                int friendId = ctx.PostInt( "friendId" );
                String friendCode = ctx.Post( "friendCode" );
                Result result = inviteService.Validate( friendId, friendCode );
                if (result.HasErrors) {
                    echo( result.ErrorsHtml );
                    return;
                }
            }

            // 验证
            User user = validateUser();
            if (errors.HasErrors) {
                run( Register );
                return;
            }

            // 用户注册
            user = userService.Register( user, ctx );
            if ((user == null) || errors.HasErrors) {
                run( Register );
                return;
            }

            // 是否开启空间
            RegHelper.CheckUserSpace( user, ctx );

            // 好友处理
            RegHelper.ProcessFriend( user, ctx );

            // 是否需要审核、激活
            if (config.Instance.Site.UserNeedApprove) {

                user.Status = MemberStatus.Approving;
                user.update( "Status" );

                view( "needApproveMsg" );
                set( "siteName", config.Instance.Site.SiteName );
            }
            else if (config.Instance.Site.EnableEmail) {

                if (config.Instance.Site.LoginType == LoginType.Open) {
                    loginService.Login( user, LoginTime.Forever, ctx.Ip, ctx );
                }

                redirectUrl( to( Done ) + "?email=" + user.Email );

            }
            else {
                loginService.Login( user, LoginTime.Forever, ctx.Ip, ctx );
                echoRedirect( lang( "registerok" ), getSavedReturnUrl() );
            }

        }

        public void needApproveMsg() {
            set( "siteName", config.Instance.Site.SiteName );
        }

        public void SendConfirmEmail( int userId ) {

            User user = userService.GetById( userId );
            Result sent = confirmEmail.SendEmail( user, null, null );

            if (sent.IsValid) {
                echoAjaxOk();
            }
            else
                echoText( "对不起，发送失败，请稍后再试" );
        }

        //--------------------------------------------------------------------------------

        private String getSavedReturnUrl() {
            String returnUrl = ctx.Post( "returnUrl" );
            if (strUtil.IsNullOrEmpty( returnUrl )) {
                returnUrl = sys.Path.Root;
            }
            return returnUrl;
        }

        public User validateUser() {

            if (config.Instance.Site.RegisterNeedImgValidateion) Html.Captcha.CheckError( ctx );

            String name = ctx.Post( "Name" );
            String pwd = ctx.Post( "Password1" );
            String pageUrl = ctx.Post( "FriendUrl" );
            String email = strUtil.SubString( ctx.Post( "Email" ), RegPattern.EmailLength );


            if (strUtil.IsNullOrEmpty( name )) {
                errors.Add( lang( "exUserName" ) );
            }
            else if (name.Length < config.Instance.Site.UserNameLengthMin) {
                errors.Add( string.Format( lang( "exUserNameLength" ), config.Instance.Site.UserNameLengthMin ) );
            }
            else {
                name = strUtil.SubString( name, config.Instance.Site.UserNameLengthMax );
            }
            if (strUtil.IsAbcNumberAndChineseLetter( name ) == false) errors.Add( lang( "exUserNameError" ) );


            if (strUtil.IsNullOrEmpty( pwd ))
                errors.Add( lang( "exPwd" ) );
            else
                pwd = strUtil.CutString( pwd, 20 );


            if (Component.IsEnableUserSpace()) {
                if (strUtil.IsNullOrEmpty( pageUrl )) {
                    errors.Add( lang( "exUrl" ) );
                }
                else if (pageUrl.IndexOf( "http:" ) >= 0) {
                    errors.Add( lang( "exUserUrlHttpError" ) );
                }
                else {
                    pageUrl = strUtil.SubString( pageUrl, config.Instance.Site.UserNameLengthMax );
                    pageUrl = pageUrl.ToLower();
                }

                if (strUtil.IsUrlItem( pageUrl ) == false) errors.Add( lang( "exUserUrlError" ) );

            }

            if (strUtil.IsNullOrEmpty( email ))
                errors.Add( lang( "exEmail" ) );
            else {
                if (RegPattern.IsMatch( email, RegPattern.Email ) == false) errors.Add( lang( "exUserMail" ) );
            }

            User user = new User();
            user.Name = name;
            user.Pwd = pwd;
            user.Url = pageUrl;
            user.Email = email;
            user.Gender = ctx.PostInt( "Gender" );
            return user;
        }


    }

}
