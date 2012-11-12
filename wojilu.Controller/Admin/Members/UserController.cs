/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Net;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.Utils;
using wojilu.Web.Mvc;

using wojilu.Common.Msg.Service;
using wojilu.Common.AppBase;
using wojilu.Common.Msg.Interface;

using wojilu.Members.Users.Service;
using wojilu.Members.Users.Domain;
using wojilu.Members.Sites.Service;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Users.Interface;

using wojilu.Web.Controller.Users.Admin;
using wojilu.Web.Controller.Security;
using wojilu.Members.Interface;
using wojilu.Web.Mvc.Utils;
using wojilu.Web.Controller.Common;
using wojilu.Common.Resource;

namespace wojilu.Web.Controller.Admin.Members {

    internal class UserVo {

        public String Name { get; set; }
        public String Pwd { get; set; }
        public String Email { get; set; }
    }

    public partial class UserController : ControllerBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( UserController ) );

        public IUserService userService { get; set; }
        public ISiteRoleService roleService { get; set; }
        public IMessageService msgService { get; set; }
        public IAdminLogService<SiteLog> logService { get; set; }
        public IConfirmEmail confirmEmail { get; set; }

        public UserController() {
            userService = new UserService();
            roleService = new SiteRoleService();
            msgService = new MessageService();
            logService = new SiteLogService();
            confirmEmail = new ConfirmEmail();
        }

        public void Index() {

            set( "userListLink", to( Index ) );
            set( "addUserLink", to( AddUser ) );

            set( "SearchAction", to( Index ) );
            set( "OperationUrl", to( Operation ) );

            set( "sendMsgLink", to( SendMsg ) );
            set( "sendEmailLink", to( SendEmail ) );
            set( "sendConfirmEmailLink", to( SendConfirmMail ) );

            set( "resetPwdLink", to( ResetPwd ) );

            List<SiteRole> roles = roleService.GetAllRoles();
            List<SiteRole> roles2 = roleService.GetRolesWithotGuest();
            List<SiteRank> ranks = roleService.GetRankAll();
            bindList( "roles", "role", roles );
            bindList( "roles2", "r", roles2 );
            bindList( "ranks", "rank", ranks );

            String condition = getCondition();
            DataPage<User> list = db.findPage<User>( condition );

            bindUserList( list );
        }

        public void AddUser() {
            target( CreateUser );
        }

        [HttpPost, DbTransaction]
        public void CreateUser() {

            List<UserVo> users = getUserInfoList();
            if (users.Count == 0) {
                echoError( "请填写用户信息" );
                return;
            }

            logger.Info( "user count=" + users.Count );

            try {
                int okCount = 0;
                foreach (UserVo u in users) {
                    Result result = registerUser( u ); // 逐个导入用户(导入的过程就是注册的过程)
                    if (result.IsValid) {
                        logger.Info( "register user=" + u.Name );
                        okCount += 1;
                    }
                    else {
                        logger.Error( "register user error=" + result.ErrorsText );
                        errors.Join( result );
                    }
                }

                if (okCount > 0) {
                    logger.Info( "register done" );
                    echoToParentPart( "注册成功" );
                }
                else {
                    echoError();
                }
            }
            catch (Exception ex) {
                logger.Info( "" + ex.Message );
                logger.Info( "" + ex.StackTrace );

                echoError( "对不起，注册出错，请查看日志" );
            }
        }

        private Result registerUser( UserVo user ) {
            // 调用 OpenService 进行 wojilu 注册
            return new wojilu.Open.OpenService().UserRegister( user.Name, user.Pwd, user.Email );
        }


        private List<UserVo> getUserInfoList() {

            String txtUsers = ctx.Post( "txtUsers" );

            if (strUtil.IsNullOrEmpty( txtUsers )) return new List<UserVo>();

            List<UserVo> users = new List<UserVo>();

            String[] arrLines = txtUsers.Trim().Split( new char[] { '\n', '\r' } );

            foreach (String line in arrLines) {

                if (strUtil.IsNullOrEmpty( line )) continue;

                String[] arrItems = line.Split( '/' );
                if (arrItems.Length != 3) continue;

                UserVo user = new UserVo();
                user.Name = arrItems[0];
                user.Pwd = arrItems[1];
                user.Email = arrItems[2];

                if (hasError( user )) continue;

                users.Add( user );
            }

            return users;
        }

        private bool hasError( UserVo user ) {
            return string.IsNullOrEmpty( user.Name ) ||
                string.IsNullOrEmpty( user.Pwd ) ||
                string.IsNullOrEmpty( user.Email );
        }



        [HttpPost, DbTransaction]
        public void Operation() {
            String userIds = ctx.PostIdList( "choice" );

            if (strUtil.IsNullOrEmpty( userIds )) {
                redirect( Index );
                return;
            }

            String condition = "Id in (" + userIds + ") ";

            String cmd = ctx.Post( "action" );
            String action = "";
            if ("pick" == cmd)
                action = "set Status=" + MemberStatus.Pick;
            else if ("unpick" == cmd)
                action = "set Status=" + MemberStatus.Normal;
            else if ("approve" == cmd)
                action = "set Status=" + MemberStatus.Normal;
            else if ("delete" == cmd)
                action = "set Status=" + MemberStatus.Deleted;
            else if ("undelete" == cmd)
                action = "set Status=" + MemberStatus.Normal;
            else if ("deletetrue" == cmd) {
                User.deleteBatch( condition );
                redirect( Index );
                return;
            }
            else if ("category" == cmd) {
                int roleId = ctx.PostInt( "categoryId" );
                action = "set RoleId=" + roleId;
            }

            User.updateBatch( action, condition );
            logUser( SiteLogString.AdminUser( cmd ), userIds );
            actionContent( "ok" );
        }

        //-----------------------------------------------------------------------------------------------------


        public void Edit( int id ) {

            target( UpdateProfile, id );
            User m = User.findById( id );
            bindProfile( m );

        }

        public void UpdateProfile( int id ) {
            User m = User.findById( id );
            UserProfileController.SaveProfile( m, ctx );
            db.update( m );
            db.update( m.Profile );
            echoRedirect( lang( "opok" ) );
        }


        //-----------------------------------------------------------------------------------------------------

        public void ResetPwd() {
            set( "ActionLink", to( SavePwd ) + "?id=" + ctx.GetIdList( "id" ) );
            String idsStr = ctx.GetIdList( "id" );
            List<User> users = userService.GetByIds( idsStr );
            bindReceiverEmailList( users, idsStr );
        }

        [HttpPost, DbTransaction]
        public void SavePwd() {

            // 1、验证密码是否正确
            String pwd = ctx.Post( "Pwd" );

            // 2、修改密码
            String idsStr = ctx.PostIdList( "UserIds" );
            List<User> users = userService.GetByIds( idsStr );
            if (users.Count == 0) {
                echoRedirect( lang( "exUserNotFound" ) );
                return;
            }

            foreach (User user in users) {
                userService.UpdatePwd( user, pwd );
                logUser( SiteLogString.UpdateUserPwd(), user );
            }

            // 3、发送到邮箱
            Boolean isSendMail = (ctx.PostIsCheck( "chkSendEmail" ) == 1);
            if (isSendMail) {
                sendPwdToEmail( users, pwd );
            }
            else
                echoRedirect( lang( "pwdUpdated" ), Index );

        }

        private void sendPwdToEmail( List<User> users, String pwd ) {

            MailService mail = MailUtil.getMailService();

            String msgTitle = string.Format( lang( "newPwdInfo" ), config.Instance.Site.SiteName );
            String msgBody = "{0} : <br/>" + string.Format( lang( "newPwdBody" ), config.Instance.Site.SiteName, pwd ) + config.Instance.Site.SiteName;

            int sendCount = 0;
            foreach (User user in users) {
                if (isEmailValid( user ) == false) continue;
                mail.send( user.Email, msgTitle, string.Format( msgBody, user.Name ) );
                sendCount++;
            }

            if (sendCount > 0)
                echoRedirect( lang( "pwdUpdatedAndSent" ), Index );
            else
                echoRedirect( lang( "pwdUpdatedAndSentError" ), Index );
        }

        //-----------------------------------------------------------------------------------------------------

        public void SendMsg() {
            set( "ActionLink", to( SaveMsg ) + "?id=" + ctx.GetIdList( "id" ) );

            String idsStr = ctx.GetIdList( "id" );
            List<User> users = userService.GetByIds( idsStr );
            bindReceiverList( users, idsStr );
            editor( "MsgBody", "", "350px" );
        }

        [HttpPost, DbTransaction]
        public void SaveMsg() {

            MsgInfo msgInfo = validateMsg();

            if (ctx.HasErrors) {
                run( SendMsg );
                return;
            }

            msgService.SiteSend( msgInfo.Title, msgInfo.Body, msgInfo.Users );
            echoRedirect( lang( "sentok" ), Index );
        }

        public void SendEmail() {

            if (config.Instance.Site.EnableEmail == false) {
                echo( "对不起，邮件服务尚未开启，无法发送邮件" );
                return;
            }

            String ids = ctx.GetIdList( "id" );

            set( "ActionLink", to( SaveEmail ) + "?id=" + ids );

            List<User> users = userService.GetByIds( ids );
            bindReceiverEmailList( users, ids );
            editor( "MsgBody", "", "350px" );
        }

        public void SendConfirmMail() {

            if (config.Instance.Site.EnableEmail == false) {
                echo( "对不起，邮件服务尚未开启，无法发送邮件" );
                return;
            }

            String ids = ctx.GetIdList( "id" );
            set( "ActionLink", to( new EmailConfirmController().SendConfirmMail ) + "?id=" + ids );

            List<User> users = userService.GetByIds( ids );
            bindReceiverEmailList( users, ids );
            set( "title", config.Instance.Site.SiteName + " " + lang( "accountConfirm" ) );

            String emailBody = confirmEmail.GetEmailBody( users[0] );
            set( "emailBody", emailBody );

            String confirmTemplateTip = string.Format( lang( "confirmTemplateTip" ), to( new EmailConfirmController().EditTemplate ) );
            set( "confirmTemplateTip", confirmTemplateTip );
        }

        [HttpPost, DbTransaction]
        public void SaveEmail() {

            if (config.Instance.Site.EnableEmail == false) {
                echo( "对不起，邮件服务尚未开启，无法发送邮件" );
                return;
            }

            MsgInfo msgInfo = validateMsg( true );


            if (ctx.HasErrors) {
                run( SendMsg );
                return;
            }

            int sendCount = 0;
            foreach (User user in msgInfo.Users) {
                if (isEmailValid( user ) == false) continue;
                Boolean sent = confirmEmail.SendEmail( user, msgInfo.Title, msgInfo.Body );
                if (sent) {
                    logUser( SiteLogString.SendUserEmail(), user );
                    sendCount++;
                }
            }

            if (sendCount > 0)
                echoRedirect( lang( "sentok" ), Index );
            else
                echoRedirect( lang( "exSentError" ), Index );
        }




    }
}

