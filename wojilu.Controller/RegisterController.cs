using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Common;
using wojilu.Common.Resource;

using wojilu.Web.Controller.Common;
using wojilu.Web.Controller.Common.Installers;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;
using wojilu.Config;
using wojilu.Common.MemberApp.Interface;
using wojilu.Common.Menus.Interface;
using wojilu.Web.Url;
using wojilu.Common.Onlines;
using System.Threading;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller {

    public class RegisterController : ControllerBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( RegisterController ) );

        public IUserService userService { get; set; }
        public IUserConfirmService confirmService { get; set; }
        public IConfirmEmail confirmEmail { get; set; }
        public IInviteService inviteService { get; set; }
        public ILoginService loginService { get; set; }
        public IFriendService friendService { get; set; }

        public virtual IMemberAppService appService { get; set; }
        public virtual IMenuService menuService { get; set; }


        public RegisterController() {
            userService = new UserService();
            confirmService = new UserConfirmService();
            confirmEmail = new ConfirmEmail();
            inviteService = new InviteService();
            loginService = new LoginService();
            friendService = new FriendService();

            appService = new UserAppService();
            menuService = new UserMenuService();


            HidePermission( typeof( SecurityController ) );

        }

        public override void CheckPermission() {
            if (config.Instance.Site.RegisterType == RegisterType.Close) {
                echo( "对不起，网站已经关闭注册" );
            }
        }

        public void Invite( int userId ) {

            if (ctx.viewer.IsLogin) {
                echo( "您有帐号，并且已经登录" );
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
            set( "userPic", user.PicMedium );

            set( "loginLink", to( new MainController().Login ) );

        }

        [HttpPost]
        public void RegisterFriend( int friendId ) {

            String inviteCode = ctx.Post( "inviteCode" );

            Result result = inviteService.Validate( friendId, inviteCode );
            if (result.HasErrors) {
                echo( result.ErrorsHtml );
                return;
            }

            User friend = userService.GetById( friendId );

            set( "friend.Name", friend.Name );
            set( "friend.Pic", friend.PicMedium );
            set( "friend.Id", friend.Id );
            set( "friend.Code", inviteCode );

            bindRegister();
        }

        public void Register() {



            if (config.Instance.Site.RegisterType == RegisterType.CloseUnlessInvite) {
                echo( "对不起，您必须接受邀请才能注册" );
                return;
            }

            bindRegister();
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
                    subdomainBlock.Set( "userUrlPrefix", SystemInfo.HostNoSubdomain);
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
            //set( "isUrlValidLink", to( CheckUrlExist ) );

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

            echoRedirect( "ok" );
        }

        [HttpPost, DbTransaction]
        public void CheckUserExist() {

            String name = ctx.Post( "Name" );

            if (userService.IsNameReservedOrExist( name )) {
                echoError( lang( "exNameFound" ) );
                return;
            }

            echoRedirect( "ok" );
        }

        [HttpPost, DbTransaction]
        public void CheckUrlExist() {

            String url = ctx.Post( "FriendUrl" );
            if (userService.IsUrlReservedOrExist( url )) {
                //echoError( lang( "exUrlFound" ) );
                //return;
                echoJsonMsg( lang( "exUrlFound" ), false, "" );
            }
            else {

                echoJsonMsg( "ok", true, "" );

            }

            //echoRedirect( "ok" );
        }

        public void Done() {

            set( "email", ctx.Get( "email" ) );

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


            User user = validateUser();
            if (errors.HasErrors) {
                run( Register );
                return;
            }

            user = userService.Register( user, ctx );
            if ((user == null) || errors.HasErrors) {
                run( Register );
                return;
            }

            if (Component.IsEnableUserSpace()) {

                addUserAppAndMenus( user );
            }

            processFriend( user );

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



        // 根据邀请码注册，需要加为好友
        private void processFriend( User newRegUser ) {

            int friendId = ctx.PostInt( "friendId" );
            if (friendId <= 0) return;

            String friendCode = ctx.Post( "friendCode" );

            Result result = inviteService.Validate( friendId, friendCode );
            if (result.HasErrors) return;

            friendService.AddInviteFriend( newRegUser, friendId );
        }

        public void SendConfirmEmail( int userId ) {

            User user = userService.GetById( userId );
            Boolean sent = confirmEmail.SendEmail( user, null, null );

            if (sent) {
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
            String email = ctx.Post( "Email" );


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

        public void TestMenu() {

            addUserAppAndMenus( User.findById( 2 ) );
        }


        private void addUserAppAndMenus( User user ) {

            if (strUtil.IsNullOrEmpty( config.Instance.Site.UserInitApp )) return;

            List<String> menus = new List<string>();

            String[] arr = config.Instance.Site.UserInitApp.Split( ',' );
            foreach (String app in arr) {
                if (strUtil.IsNullOrEmpty( app )) continue;
                menus.Add( app.Trim() );
            }

            if (menus.Contains( "home" )) {
                new UserHomeInstaller().Install( ctx, user, lang( "homepage" ), wojilu.Common.AppBase.AccessStatus.Public );
            }

            if (menus.Contains( "blog" )) {
                IMemberApp blogApp = appService.Add( user, "博客", 2 );
                // 添加菜单：此处需要明确传入MemberType，否则将会使用ctx.Owner，也就是Site的值，导致bug
                String blogUrl = UrlConverter.clearUrl( alink.ToUserAppFull( blogApp ), ctx, typeof( User ).FullName, user.Url );
                menuService.AddMenuByApp( blogApp, blogApp.Name, "", blogUrl );
            }

            if (menus.Contains( "photo" )) {
                IMemberApp photoApp = appService.Add( user, "相册", 3 );
                String photoUrl = UrlConverter.clearUrl( alink.ToUserAppFull( photoApp ), ctx, typeof( User ).FullName, user.Url );
                menuService.AddMenuByApp( photoApp, photoApp.Name, "", photoUrl );
            }

            if (menus.Contains( "microblog" )) {
                IMenu menu = getMenu( user, "微博", alink.ToUserMicroblog( user ) );
                menuService.Insert( menu, user, user );
            }

            if (menus.Contains( "share" )) {
                IMenu menu = getMenu( user, "转帖", t2( new Users.ShareController().Index ) );
                menuService.Insert( menu, user, user );
            }

            if (menus.Contains( "friend" )) {
                IMenu menu = getMenu( user, "好友", t2( new Users.FriendController().FriendList ) );
                menuService.Insert( menu, user, user );
            }

            if (menus.Contains( "visitor" )) {
                IMenu menu = getMenu( user, "访客", t2( new Users.VisitorController().Index ) );
                menuService.Insert( menu, user, user );
            }

            if (menus.Contains( "forumpost" )) {
                IMenu menu = getMenu( user, "论坛帖子", t2( new Users.ForumController().Topic ) );
                menuService.Insert( menu, user, user );
            }

            if (menus.Contains( "about" )) {
                IMenu menu = getMenu( user, "关于我", t2( new Users.ProfileController().Main ) );
                menuService.Insert( menu, user, user );
            }

            if (menus.Contains( "feedback" )) {
                IMenu menu = getMenu( user, "留言", t2( new Users.FeedbackController().List ) );
                menuService.Insert( menu, user, user );
            }

        }

        private IMenu getMenu( User user, string name, string url ) {
            IMenu menu = new UserMenu();
            menu.Name = name;
            menu.RawUrl = UrlConverter.clearUrl( url, ctx, typeof( User ).FullName, user.Url ).TrimStart( '/' );

            return menu;
        }




    }

}
