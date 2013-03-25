/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Web.Mvc;
using wojilu.Web.Utils;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Interface;
using wojilu.Apps.Forum.Service;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Forum.Users {


    [App( typeof( ForumApp ) )]
    public class UploaderController : ControllerBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( UploaderController ) );

        public IAttachmentService attachService { get; set; }
        public IForumTopicService topicService { get; set; }

        public UploaderController() {
            topicService = new ForumTopicService();
            attachService = new AttachmentService();
        }

        public void UploadForm() {

            int boardId = ctx.GetInt( "boardId" );

            set( "ActionLink", to( SaveUpload ) + "?boardId=" + boardId );
        }

        public void SaveUpload() {

            int boardId = ctx.GetInt( "boardId" );
            HttpFile postedFile = ctx.GetFileSingle();

            Result result = Uploader.SaveFileOrImage( postedFile );

            if (result.HasErrors) {
                errors.Join( result );
                run( UploadForm );
                return;
            }

            AttachmentTemp uploadFile = savePostData( postedFile, result );

            // 返回数据给主页面
            set( "objFile", Json.ToString( uploadFile.GetJsonObject() ) );
            set( "deleteLink", to( DeleteTempAttachment ) + "?boardId=" + boardId );

        }

        private AttachmentTemp savePostData( HttpFile postedFile, Result result ) {
            // 将附件存入数据库
            AttachmentTemp uploadFile = new AttachmentTemp();
            uploadFile.FileSize = postedFile.ContentLength;
            uploadFile.Type = postedFile.ContentType;
            uploadFile.Name = result.Info.ToString();
            uploadFile.Description = ctx.Post( "FileDescription" );
            uploadFile.ReadPermission = ctx.PostInt( "FileReadPermission" );
            uploadFile.Price = ctx.PostInt( "FilePrice" );
            uploadFile.AppId = ctx.app.Id;

            attachService.CreateTemp( uploadFile, (User)ctx.viewer.obj, ctx.owner.obj );
            return uploadFile;
        }

        public void SaveFlashUpload() {

            int boardId = ctx.GetInt( "boardId" );
            HttpFile postedFile = ctx.GetFileSingle();

            Result result = Uploader.SaveFileOrImage( postedFile );

            if (result.HasErrors) {
                logger.Error( result.ErrorsText );
                errors.Join( result );
                echoError();
                return;
            }

            AttachmentTemp uploadFile = savePostData( postedFile, result );

            // 返回json给主页面
            String photoJson = Json.ToString( uploadFile.GetJsonObject() );
            String json = "{\"deleteLink\":\"" + to( DeleteTempAttachment ) + "?boardId=" + boardId + "\", \"photo\":" + photoJson + "}";
            echoText( json );

        }

        public void DeleteTempAttachment() {
            int id = ctx.PostInt( "Id" );
            attachService.DeleteTempAttachment( id );
            echoAjaxOk();
        }


    }

}
