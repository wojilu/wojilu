/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

using wojilu.Serialization;
using wojilu.Web.Mvc;
using wojilu.Web.Utils;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Interface;
using wojilu.Apps.Forum.Service;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Forum {

    [App( typeof( ForumApp ) )]
    public class UploaderController : ControllerBase {

        public IAttachmentService attachService { get; set; }
        public IForumTopicService topicService { get; set; }

        public UploaderController() {
            topicService = new ForumTopicService();
            attachService = new AttachmentService();
        }

        public void UploadForm() {
            target( SaveUpload );

            set( "uploadLink", to( SaveUpload ) );
            set( "authCookieName", ctx.web.CookieAuthName() );
            set( "authCookieValue", ctx.web.CookieAuthValue() );
        }

        public void SaveUpload() {

            HttpFile postedFile = ctx.GetFileSingle();

            Result result = Uploader.SaveFileOrImage( postedFile );

            if (result.HasErrors) {
                errors.Join( result );
                run( UploadForm );
                return;
            }

            AttachmentTemp uploadFile = savePostData( postedFile, result );

            // 返回数据给主页面
            set( "objFile", SimpleJsonString.ConvertObject( uploadFile ) );
            set( "deleteLink", to( DeleteTempAttachment ) );

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

            HttpFile postedFile = ctx.GetFileSingle();

            Result result = Uploader.SaveFileOrImage( postedFile );

            if (result.HasErrors) {
                errors.Join( result );
                echoError();
                return;
            }

            AttachmentTemp uploadFile = savePostData( postedFile, result );

            // 返回json给主页面
            String json = "{deleteLink:'" + to( DeleteTempAttachment ) + "', photo:" + SimpleJsonString.ConvertObject( uploadFile ) + "}";
            echoText( json );


        }

        public void DeleteTempAttachment() {
            int id = ctx.PostInt( "Id" );
            attachService.DeleteTempAttachment( id );
            echoAjaxOk();
        }

        //--------------------------




        //private void saveAttachment( ForumTopic topic ) {
        //    ForumPost post = topicService.GetPostByTopic( topic.Id );
        //    int attachmentCount = 0;

        //    string[] des = ctx.context.getFormValues( "FileDescription" );
        //    string[] rdPer = ctx.context.getFormValues( "FileReadPermission" );
        //    string[] prices = ctx.context.getFormValues( "FilePrice" );

        //    int uploadMaxUnit = 3;
        //    int uploadMax = 1024 * 1024 * uploadMaxUnit;

        //    for (int i = 0; i < ctx.getFiles().Count; i++) {

        //        if (des == null || rdPer == null || prices == null) continue;

        //        HttpPostedFile postedFile = ctx.getFiles()[i];

        //        // 检查文件大小
        //        if (postedFile.ContentLength <= 1 || postedFile.ContentLength > uploadMax) continue;

        //        // 检查文件格式
        //        if (Uploader.isAllowType( postedFile ) == false) {
        //            errors.Add( "文件类型不允许:" + postedFile.FileName + "/" + postedFile.ContentType );
        //            continue;
        //        }

        //        Result result;
        //        if (Uploader.IsImage( postedFile ))
        //            result = Uploader.SaveImg( postedFile );
        //        else
        //            result = Uploader.SaveFile( postedFile );
        //        if (result.HasErrors) continue;

        //        Attachment uploadFile = new Attachment();
        //        uploadFile.FileSize = postedFile.ContentLength;
        //        uploadFile.Type = postedFile.ContentType;
        //        uploadFile.Name = result.Info.ToString();

        //        uploadFile.Description = des[i];
        //        uploadFile.ReadPermission = cvt.ToInt( rdPer[i] );
        //        uploadFile.Price = cvt.ToInt( prices[i] );


        //        uploadFile.AppId = ctx.app.Id;
        //        uploadFile.TopicId = topic.Id;
        //        uploadFile.PostId = post.Id;
        //        attachService.Create( uploadFile, (User)ctx.viewer.obj, ctx.owner.obj );
        //        attachmentCount++;
        //    }
        //    topicService.UpdateAttachments( topic, attachmentCount );
        //}



    }

}
