/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;
using System.Text.RegularExpressions;

using wojilu.Net;
using wojilu.Web.Mvc;
using wojilu.Web.Utils;
using wojilu.Web.Mvc.Attr;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Service;
using wojilu.Members.Users.Interface;

namespace wojilu.Web.Controller.Common {

    public class ResetPwdController : ControllerBase {

        public IUserService userService { get; set; }
        public IUserResetPwdSerevice resetService { get; set; }

        public ResetPwdController() {
            userService = new UserService();
            resetService = new UserResetPwdSerevice();

            HidePermission( typeof( SecurityController ) );
        }

        public override void CheckPermission() {
            if (ctx.viewer.IsLogin) {
                echoRedirect( lang( "exLogged" ), sys.Url.SiteUrl );
            }
        }

        public override void Layout() {
        }

        public void StepOne() {
            target( StepTwo );
            set( "siteName", config.Instance.Site.SiteName );
            set( "Captcha", Html.Captcha );
        }

        [HttpPost, DbTransaction]
        public void StepTwo() {

            // 0、验证码
            Html.Captcha.CheckError( ctx );

            // 1、得到email地址和相应的用户
            String email = ctx.Post( "Email" );
            if (strUtil.IsNullOrEmpty( email )) {
                errors.Add( lang( "exEmail" ) );
            }
            else if (new Regex( RegPattern.Email ).IsMatch( email ) == false) {
                errors.Add( lang( "exEmailFormat" ) );
            }

            if (ctx.HasErrors) {
                showError();
                return;
            }

            User user = userService.GetByMail( email );
            if (user == null) {
                errors.Add( lang( "exUserNotFoundByEmail" ) );
                showError();
                return;
            }
            ctx.SetItem( "User", user );

            // 2、产生唯一code并加入数据库
            UserResetPwd userReset = new UserResetPwd();
            userReset.User = user;
            userReset.Code = Guid.NewGuid().ToString().Replace( "-", "" ).ToLower();
            userReset.Ip = ctx.Ip;
            String resetLink = getResetLink( userReset );
            ctx.SetItem( "ResetLink", resetLink );

            // 3、给此email发送一封重置pwd的邮件
            MailClient mail = MailClient.Init();
            String title = string.Format( lang( "exResetMsgTitle" ), config.Instance.Site.SiteName );
            String body = loadHtml( emailBody );
            Result sentResult = mail.Send( email, title, body );
            if (sentResult.HasErrors) {
                errors.Add( lang( "exResetSend" ) );
                showError();
            }
            else {
                resetService.Insert( userReset );
                showJson( lang( "resetSendok" ) );
            }

        }

        private void showError() {
            echoJson( errors.ErrorsJson );
        }

        private void showJson( String msg ) {

            StringBuilder builder = new StringBuilder();
            builder.Append( "{\"IsValid\":true, Msg:\"" );
            builder.Append( msg );
            builder.Append( "\"}" );

            echoJson( builder.ToString() );
        }

        [NonVisit]
        public void emailBody() {

            User user = ctx.GetItem( "User" ) as User;
            String resetLink = ctx.GetItem( "ResetLink" ).ToString();

            set( "siteName", config.Instance.Site.SiteName );
            set( "siteLink", ctx.url.SiteUrl );
            set( "userName", user.Name );
            set( "created", DateTime.Now.ToShortDateString() );

            set( "resetLink", resetLink );
        }

        private String getResetLink( UserResetPwd resetInfo ) {
            String codeFull = resetInfo.User.Id + "_" + resetInfo.Code;
            codeFull = Convert.ToBase64String( System.Text.Encoding.UTF8.GetBytes( codeFull ) );
            return strUtil.Join( ctx.url.SiteUrl, to( ResetPwd ) + "?c=" + codeFull );
        }

        public void ResetPwd() {

            // 1、根据get的code，查询数据库，是否有此重置密码请求
            UserResetPwd resetInfo = validateCode();
            if (resetInfo == null) {
                echoRedirect( ctx.errors.ErrorsHtml , sys.Path.Root );
                return;
            }

            // 2、渲染表单
            set( "ActionLink", to( SavePwd ) + "?c=" + ctx.Get( "c" ) );

        }

        [HttpPost, DbTransaction]
        public void SavePwd() {

            UserResetPwd resetInfo = validateCode();
            if (resetInfo == null) {
                echoRedirect( ctx.errors.ErrorsHtml );
                return;
            }

            // 1、重设用户的密码
            String pwd = ctx.Post( "Pwd" );
            userService.UpdatePwd( resetInfo.User, pwd );

            // 2、将重设记录改成已设
            resetService.UpdateResetSuccess( resetInfo );

            echoRedirect( lang( "opok" ), sys.Path.Root );
        }

        private static readonly ILog logger = LogManager.GetLogger( typeof( ResetPwdController ) );

        private UserResetPwd validateCode() {
            String code = ctx.Get( "c" );
            if (strUtil.IsNullOrEmpty( code )) {
                errors.Add( "code error" );
                return null;
            }

            String codestr =null;
            try {
                codestr = System.Text.Encoding.UTF8.GetString( Convert.FromBase64String( code ) );
            }
            catch (Exception ex) {
                logger.Error( lang( "exResetError" ) + ex.Message );
                errors.Add( "code " + lang( "error" ) );
                return null;
            }

            string[] arrCode = codestr.Split( '_' );
            if (arrCode.Length != 2) {
                errors.Add( "code " + lang( "error" ) );
                return null;
            }

            int userId = cvt.ToInt( arrCode[0] );
            if (userId <= 0) {
                errors.Add( "code " + lang( "error" ) );
                return null;
            }

            String guid = arrCode[1];

            UserResetPwd resetInfo = resetService.GetByUserAndCode( userId, guid );
            if (resetInfo == null) {
                errors.Add( lang( "exResetNotFound" ) );
                return null;
            }

            return resetInfo;
        }


    }





}
