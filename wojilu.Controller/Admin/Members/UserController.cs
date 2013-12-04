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

        public String FriendlyUrl { get; set; }
    }

    public partial class UserController : ControllerBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( UserController ) );

        public virtual IUserService userService { get; set; }
        public virtual ISiteRoleService roleService { get; set; }
        public virtual IMessageService msgService { get; set; }
        public virtual IAdminLogService<SiteLog> logService { get; set; }
        public virtual IConfirmEmail confirmEmail { get; set; }
        public virtual IUserErrorPicService errorPicService { get; set; }

        public UserController() {
            userService = new UserService();
            roleService = new SiteRoleService();
            msgService = new MessageService();
            logService = new SiteLogService();
            confirmEmail = new ConfirmEmail();
            errorPicService = new UserErrorPicService();
        }

        public virtual void Index() {

            set( "userListLink", to( Index ) );

            set( "SearchAction", to( Index ) );
            set( "OperationUrl", to( Operation ) );

            set( "sendMsgLink", to( SendMsg ) );
            set( "sendEmailLink", to( SendEmail ) );
            set( "sendConfirmEmailLink", to( SendConfirmMail ) );

            set( "resetPwdLink", to( ResetPwd ) );
            set( "errorPicLink", to( ApproveUserPic ) );

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

        [HttpPost, DbTransaction]
        public virtual void Operation() {
            String userIds = ctx.PostIdList( "choice" );

            if (strUtil.IsNullOrEmpty( userIds )) {
                redirect( Index );
                return;
            }

            String condition = "Id in (" + userIds + ") ";

            String cmd = ctx.Post( "action" );
            String action = "";
            if ("pick" == cmd) {
                action = "set Status=" + MemberStatus.Pick;
            }
            else if ("unpick" == cmd) {
                action = "set Status=" + MemberStatus.Normal;
            }
            else if ("approve" == cmd) {
                action = "set Status=" + MemberStatus.Normal;
            }
            else if ("delete" == cmd) {
                action = "set Status=" + MemberStatus.Deleted;
            }
            else if ("undelete" == cmd) {
                action = "set Status=" + MemberStatus.Normal;
            }
            else if ("deletetrue" == cmd) {
                User.deleteBatch( condition );
                redirect( Index );
                return;
            }
            else if ("category" == cmd) {
                long roleId = ctx.PostLong( "categoryId" );
                action = "set RoleId=" + roleId;
            }

            User.updateBatch( action, condition );
            logUser( SiteLogString.AdminUser( cmd ), userIds );
            content( "ok" );
        }

        //----------------------------------------------------------------------------


        public virtual void Edit( long id ) {

            target( UpdateProfile, id );
            User m = User.findById( id );
            bindProfile( m );
            set( "lnkEditName", to( EditName, id ) );
            set( "lnkEditUrl", to( EditUrl, id ) );
            set( "lnkEditEmail", to( EditEmail, id ) );
        }

        [HttpPost, DbTransaction]
        public virtual void UpdateProfile( long id ) {
            User m = User.findById( id );
            UserProfileController.SaveProfile( m, ctx );
            db.update( m );
            db.update( m.Profile );
            echoRedirectPart( lang( "opok" ) );
        }

        //--------------------------------------

        public virtual void EditName( long id ) {
            target( UpdateName, id );
            User m = User.findById( id );
            set( "userName", m.Name );
        }

        [HttpPost,DbTransaction]
        public virtual void UpdateName( long id ) {

            String newName = strUtil.SubString( ctx.Post( "userName" ), 20 );
            if (strUtil.IsNullOrEmpty( newName )) {
                echoError( "请填写用户名" );
                return;
            }

            User m = User.findById( id );
            m.Name = newName;
            m.update();
            echoToParentPart( lang( "opok" ) );
        }

        //--------------------------------------

        public virtual void EditUrl( long id ) {
            target( UpdateUrl, id );
            User m = User.findById( id );
            set( "userUrl", m.Url );
        }

        [HttpPost, DbTransaction]
        public virtual void UpdateUrl( long id ) {

            String newUrl = strUtil.SubString( ctx.Post( "userUrl" ), 50 );
            if (strUtil.IsNullOrEmpty( newUrl )) {
                echoError( "请填写个性网址" );
                return;
            }

            User m = User.findById( id );
            m.Url = newUrl;
            m.update();

            // UserApp
            UserApp.updateBatch( "OwnerUrl='" + newUrl + "'", "OwnerId=" + id );
            UserApp.updateBatch( "CreatorUrl='" + newUrl + "'", "OwnerId=" + id );
            UserMenu.updateBatch( "OwnerUrl='" + newUrl + "'", "OwnerId=" + id );

            echoToParentPart( lang( "opok" ) );
        }

        //--------------------------------------

        public virtual void EditEmail( long id ) {
            target( UpdateEmail, id );
            User m = User.findById( id );
            set( "userEmail", m.Email );
        }

        [HttpPost, DbTransaction]
        public virtual void UpdateEmail( long id ) {

            String userEmail = strUtil.SubString( ctx.Post( "userEmail" ), RegPattern.EmailLength );

            if (strUtil.IsNullOrEmpty( userEmail ) || RegPattern.IsMatch( userEmail, RegPattern.Email ) == false) {
                echoError( lang( "exUserMail" ) );
                return;
            }

            User m = User.findById( id );
            m.Email = userEmail;
            m.update();
            echoToParentPart( lang( "opok" ) );
        }

        //----------------------------------------------------------------------------

        public virtual void ResetPwd() {
            set( "ActionLink", to( SavePwd ) + "?id=" + ctx.GetIdList( "id" ) );
            String idsStr = ctx.GetIdList( "id" );
            List<User> users = userService.GetByIds( idsStr );
            bindReceiverEmailList( users, idsStr );
        }

        [HttpPost, DbTransaction]
        public virtual void SavePwd() {

            // 1、验证密码是否正确
            String pwd = ctx.Post( "Pwd" );

            // 2、修改密码
            String idsStr = ctx.PostIdList( "UserIds" );
            List<User> users = userService.GetByIds( idsStr );
            if (users.Count == 0) {
                echoRedirectPart( lang( "exUserNotFound" ) );
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
            else {
                echoRedirectPart( lang( "pwdUpdated" ), to( Index ) );
            }

        }

        private void sendPwdToEmail( List<User> users, String pwd ) {

            MailClient mail = MailClient.Init();

            String msgTitle = string.Format( lang( "newPwdInfo" ), config.Instance.Site.SiteName );
            String msgBody = "{0} : <br/>" + string.Format( lang( "newPwdBody" ), config.Instance.Site.SiteName, pwd ) + config.Instance.Site.SiteName;

            int sendCount = 0;
            foreach (User user in users) {
                if (isEmailValid( user ) == false) continue;
                mail.Send( user.Email, msgTitle, string.Format( msgBody, user.Name ) );
                sendCount++;
            }

            if (sendCount > 0)
                echoRedirectPart( lang( "pwdUpdatedAndSent" ), to( Index ) );
            else
                echoRedirectPart( lang( "pwdUpdatedAndSentError" ), to( Index ) );
        }

        //----------------------------------------------------------------------------

        public virtual void SendMsg() {
            set( "ActionLink", to( SaveMsg ) + "?id=" + ctx.GetIdList( "id" ) );

            String idsStr = ctx.GetIdList( "id" );
            List<User> users = userService.GetByIds( idsStr );
            bindReceiverList( users, idsStr );
        }

        [HttpPost, DbTransaction]
        public virtual void SaveMsg() {

            MsgInfo msgInfo = validateMsg( true );

            if (ctx.HasErrors) {
                run( SendMsg );
                return;
            }

            msgService.SiteSend( msgInfo.Title, msgInfo.Body, msgInfo.Users );
            echoRedirectPart( lang( "sentok" ), to( Index ) );
        }

        public virtual void SendEmail() {

            if (config.Instance.Site.EnableEmail == false) {
                echo( "对不起，邮件服务尚未开启，无法发送邮件" );
                return;
            }

            String ids = ctx.GetIdList( "id" );

            set( "ActionLink", to( SaveEmail ) + "?id=" + ids );

            List<User> users = userService.GetByIds( ids );
            bindReceiverEmailList( users, ids );
        }

        public virtual void SendConfirmMail() {

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
        public virtual void SaveEmail() {

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
                Result sent = sendEmail( user, msgInfo.Title, msgInfo.Body );
                if (sent.IsValid) {
                    logUser( SiteLogString.SendUserEmail(), user );
                    sendCount++;
                }
            }

            if (sendCount > 0) {
                echoRedirectPart( lang( "sentok" ), to( Index ) );
            }
            else {
                echoRedirectPart( lang( "exSentError" ), to( Index ) );
            }
        }

        private Result sendEmail( User user, string title, string msg ) {
            return MailClient.Init().Send( user.Email, title, msg );
        }

        public virtual void ApproveUserPic() {
            set( "ActionLink", to( SaveApprovePic ) + "?id=" + ctx.GetIdList( "id" ) );
            String idsStr = ctx.GetIdList( "id" );
            List<User> users = userService.GetByIds( idsStr );
            bindReceiverList( users, idsStr );
        }

        public virtual void SaveApprovePic() {
            String ids = ctx.PostIdList( "UserIds" );
            String reviewMsg = validPicMsg();

            if (ctx.HasErrors) {
                run( ApproveUserPic );
                return;
            }

            int isPass = ctx.PostInt( "IsPass" );
            int isDelete = ctx.PostIsCheck( "IsDelete" );
            if (isPass == 0) {
                errorPicService.ApproveError( ids, reviewMsg, ctx.PostIsCheck( "IsNextAutoPass" ), isDelete );
            }
            else {
                errorPicService.ApproveOk( ids, reviewMsg );
            }

            echoRedirectPart( "审核成功", to( Index ) );
        }

        private string validPicMsg() {
            String idsStr = ctx.PostIdList( "UserIds" );
            List<User> users = userService.GetByIds( idsStr );
            if (users.Count == 0) {
                errors.Add( lang( "exNoReceiver" ) );
                return null;
            }

            String msg = strUtil.CutString( ctx.Post( "Msg" ), 200 );
            if (strUtil.IsNullOrEmpty( msg )) errors.Add( "请填写审核原因" );

            return msg;
        }


    }
}

