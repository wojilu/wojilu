/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Context;
using wojilu.Web.Mvc.Attr;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;

namespace wojilu.Web.Controller.Common {

    public class ActivationController : ControllerBase {


        public IUserService userService { get; set; }
        public IConfirmEmail confirmEmail { get; set; }
        public IUserConfirmService confirmService { get; set; }

        public ActivationController() {
            userService = new UserService();
            confirmEmail = new ConfirmEmail();
            confirmService = new UserConfirmService();
        }

        public override void CheckPermission() {
            if (config.Instance.Site.EnableEmail == false) {
                echo( "对不起，邮件服务尚未开启，无法发送邮件" );
            }
        }

        public static void AllowSendActivationEmail( MvcContext ctx, int userId ) {
            ctx.web.SessionSet( "__ReSendActivationEmail", userId );
        }

        public static int IsAllowSendActivationEmail( MvcContext ctx ) {

            if (ctx.web.SessionGet( "__ReSendActivationEmail" ) == null) return 0;

            return (int)ctx.web.SessionGet( "__ReSendActivationEmail" );
        }

        //-----------------------------------------------------------------------------

        // 重发激活邮件之前的验证：请输入用户名和密码form
        public void SendEmailLogin() {

            if (ctx.viewer.IsLogin) {

                AllowSendActivationEmail( ctx, ctx.viewer.Id );
                redirect( SendEmailButton );
                return;
            }

            target( SendEmailLoginCheck );

            setLoginValidationCode();
        }

        private void setLoginValidationCode() {
            IBlock cblock = getBlock( "Captcha" );
            cblock.Set( "ValidationCode", Html.Captcha );
            cblock.Next();
        }

        [HttpPost]
        public void SendEmailLoginCheck() {

            int userId = userIsValid();
            if (userId <= 0) {
                run( SendEmailLogin );
                return;
            }

            AllowSendActivationEmail( ctx, userId );
            redirect( SendEmailButton );
        }

        private int userIsValid() {


            if (Html.Captcha.CheckError( ctx )) {
                return -1;
            }

            // 检查
            // 1) 用户名密码是否真正确
            String uname = ctx.Post( "regName" );
            String pwd = ctx.Post( "regPwd" );

            if (strUtil.IsNullOrEmpty( uname )) errors.Add( lang( "exUserName" ) );
            if (strUtil.IsNullOrEmpty( pwd )) errors.Add( lang( "exPwd" ) );
            if (ctx.HasErrors) return -1;

            User user = userService.IsNameEmailPwdCorrect( uname, pwd );
            if (user == null) {
                errors.Add( lang( "exUserNamePwdError" ) );
                return -1;
            }

            // 2) 用户是否已经激活
            if (user.IsEmailConfirmed == 1) {
                errors.Add( "您已经激活" );
                return -1;
            }

            return user.Id;
        }

        //-----------------------------------------------------------------------------

        // 重发激活邮件的按钮：
        // 1)必须有验证码
        // 2)同时呈现email文本框，允许及时修改
        public void SendEmailButton() {


            if (hasActivation()) {
                echoError( "您已经激活" );
                return;
            }

            int userId = IsAllowSendActivationEmail( ctx );

            if (userId <= 0) {
                redirect( SendEmailLogin );
                return;
            }

            User user = userService.GetById( userId );
            if (user == null) {
                echoError( "用户不存在" );
                return;
            }

            target( SendEmail );
            set( "Email", user.Email );
            set( "isEmailValidLink", to( CheckEmailExist, userId ) );
        }

        [HttpPost, DbTransaction]
        public void CheckEmailExist( int userId ) {

            String email = ctx.Post( "Email" );

            if (userService.IsEmailExist( userId, email )) {
                echoError( lang( "exEmailFound" ) );
                return;
            }

            echoJsonOk();
        }


        [HttpPost]
        public void SendEmail() {

            if (hasActivation()) {
                echoError( "您已经激活" );
                return;
            }

            int userId = IsAllowSendActivationEmail( ctx );

            if (userId <= 0) {
                redirect( SendEmailLogin );
                return;
            }

            User user = userService.GetById( userId );
            if (user == null) {
                echoError( "用户不存在" );
                return;
            }

            // 检查5分钟之内只能重发一次
            // TODO 配置5分钟
            Result result = confirmService.CanSend( user );
            if (result.HasErrors) {
                echoError( result );
                return;
            }


            String email = strUtil.SubString( ctx.Post( "Email" ), RegPattern.EmailLength );

            if (strUtil.IsNullOrEmpty( email )) {
                echoError( "请填写email" );
                return;
            }

            if (RegPattern.IsMatch( email, RegPattern.Email ) == false) {
                echoError( "email格式不正确" );
                return;
            }

            if (userService.IsEmailExist( userId, email )) {
                echoError( lang( "exEmailFound" ) );
                return;
            }


            if (email.Equals( user.Email ) == false) {
                user.Email = email;
                user.update();
            }

            confirmEmail.SendEmail( user, null, null );

            redirectUrl( to( SendEmailDone ) + "?email=" + email );

        }

        public void SendEmailDone() {

            String email = ctx.Get( "email" );
            set( "email", email );

            String goUrl = WebUtils.getMailLink( email );
            IBlock mblock = getBlock( "mailLink" );
            if (strUtil.HasText( goUrl )) {
                mblock.Set( "goUrl", goUrl );
                mblock.Next();
            }

        }

        private Boolean hasActivation() {

            if (ctx.viewer.IsLogin==false) return false;
            User user = ctx.viewer.obj as User;
            return user.IsEmailConfirmed == 1;
        }


    }

}
