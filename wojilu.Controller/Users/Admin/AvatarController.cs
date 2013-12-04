/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Members.Users.Domain;
using wojilu.Web.Utils;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;

namespace wojilu.Web.Controller.Users.Admin {

    public class AvatarController : ControllerBase {

        public virtual IUserErrorPicService errorPicService { get; set; }
        public virtual IUserService userService { get; set; }

        public AvatarController() {
            userService = new UserService();
            errorPicService = new UserErrorPicService();
        }

        public override void Layout() {
        }

        public virtual void NeedUserPic() {

            HideLayout( typeof( wojilu.Web.Controller.LayoutController ) );
            HideLayout( typeof( wojilu.Web.Controller.Users.Admin.LayoutController ) );
            HideLayout( typeof( wojilu.Web.Controller.Users.Admin.UserProfileController ) );

            int uploadStatus = errorPicService.GetStatus( ctx.viewer.obj as User );

            if (uploadStatus == UserErrorPic.StatusFirstUpload) {
                firstUpload();
            }
            else if (uploadStatus == UserErrorPic.StatusWaitingUpload) {
                waitingUpload();
            }
            else if (uploadStatus == UserErrorPic.StatusWaitingApprove) {
                waitingApprove();
            }
            else if (uploadStatus == UserErrorPic.StatusOk) {
                redirectDirect( ctx.url.SiteAndAppPath );
            }

        }

        private void firstUpload() {
            view( "uploadFirst" );
            target( SaveUserPic );

            User user = ctx.owner.obj as User;
            bindFace( user );
            set( "redirectUrl", ctx.web.PathReferrer );
        }


        private void waitingUpload() {
            view( "uploadWaiting" );
            target( SaveUserPic );

            User user = ctx.owner.obj as User;
            bindFace( user );
            set( "redirectUrl", ctx.web.PathReferrer );

            int errorCount = errorPicService.CheckErrorCount( user );
            String errorMsg = errorPicService.GetLastReviewMsg( user );
            set( "errorWraning", string.Format( "您当前头像不符合要求，已经 {0} 次。<br/>审核原因：<strong>{1}</strong>", errorCount, errorMsg ) );
        }

        private void waitingApprove() {
            view( "uploadWaitingApprove" );
            target( SaveUserPic );

            User user = ctx.owner.obj as User;
            bindFace( user );
            set( "redirectUrl", ctx.web.PathReferrer );

            int errorCount = errorPicService.CheckErrorCount( user );
            String errorMsg = errorPicService.GetLastReviewMsg( user );
            set( "errorWraning", string.Format( "您上次头像不符合要求，已经 {0} 次。<br/>上次审核原因：<strong>{1}</strong>", errorCount, errorMsg ) );
        }

        //----------------------------------------------------------

        public virtual void SaveUserPic() {


            int uploadStatus = errorPicService.GetStatus( ctx.viewer.obj as User );

            if (uploadStatus == UserErrorPic.StatusFirstUpload) {
                saveFirstUpload();
            }
            else if (uploadStatus == UserErrorPic.StatusWaitingUpload) {
                saveWaitingUpload();
            }
            else if (uploadStatus == UserErrorPic.StatusWaitingApprove) {
                saveWaitingApprove();
            }

        }

        private void saveFirstUpload() {

            User user = ctx.owner.obj as User;

            Result result = AvatarUploader.Save( ctx.GetFileSingle(), user.Id );
            if (result.HasErrors) {
                echoError( result );
                return;
            }

            if (user.Pic != UserFactory.Guest.Pic) {
                AvatarUploader.Delete( user.Pic );
            }

            userService.UpdateAvatar( user, result.Info.ToString() );
            String msg = "感谢上传！";
            echoRedirect( msg, sys.Url.SiteUrl );

        }

        private void saveWaitingUpload() {

            User user = ctx.owner.obj as User;

            Result result = AvatarUploader.Save( ctx.GetFileSingle(), user.Id );
            if (result.HasErrors) {
                echoError( result );
                return;
            }

            if (user.Pic != UserFactory.Guest.Pic) {
                AvatarUploader.Delete( user.Pic );
            }

            // 增加日志
            UserErrorPic lastLog = errorPicService.GetLastLog( user );
            if (lastLog.IsNextAutoPass == 1) {
                errorPicService.AddLogAndPass( user, ctx.Ip );
            }
            else {
                errorPicService.AddLog( user, ctx.Ip );
            }

            // 2) 保存图像、不会增加积分、不会发送邮件鼓励；给管理员发通知
            userService.UpdateAvatarWhenError( user, result.Info.ToString() );

            String msg = "感谢上传！";
            echoRedirect( msg );

        }

        private void saveWaitingApprove() {
            User user = ctx.owner.obj as User;

            Result result = AvatarUploader.Save( ctx.GetFileSingle(), user.Id );
            if (result.HasErrors) {
                echoError( result );
                return;
            }

            if (user.Pic != UserFactory.Guest.Pic) {
                AvatarUploader.Delete( user.Pic );
            }

            // 更新最后一个日志
            errorPicService.UpdateLastUpload( user, ctx.Ip );

            // 2) 仅仅保存图像、不会增加积分、不会发送邮件鼓励、不给管理员发通知
            userService.UpdateAvatarOnly( user, result.Info.ToString() );

            String msg = "感谢上传！";
            echoRedirect( msg );
        }

        private void bindFace( User user ) {
            if (strUtil.IsNullOrEmpty( user.Pic )) {
                set( "memberFace", "<span class=\"warning\">" + lang( "exNotUploadFace" ) + "</span>" );
            }
            else {
                set( "memberFace", string.Format( "<img src=\"{0}\">", user.PicM ) );
            }
        }
    }

}
