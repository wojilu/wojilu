/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.Utils;

using wojilu.Members.Users.Domain;

using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Service;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Content.Admin.Common {

    [App( typeof( ContentApp ) )]
    public class AttachmentController : ControllerBase {

        public IContentPostService postService { get; set; }
        public IAttachmentService attachService { get; set; }

        public AttachmentController() {
            postService = new ContentPostService();
            attachService = new AttachmentService();

            HideLayout( typeof( wojilu.Web.Controller.Content.LayoutController ) );
        }

        public void AdminList( int postId ) {
            ContentPost post = postService.GetById( postId, ctx.owner.Id );
            if (post == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            bindAttachments( post );
        }


        private void bindAttachments( ContentPost post ) {

            set( "addLink", to( new AttachmentController().Add, post.Id ) );
            set( "sortAction", to( new AttachmentController().SaveSort, post.Id ) );

            ContentPost topic = postService.GetById( post.Id, ctx.owner.Id );
            List<ContentAttachment> attachList = attachService.GetAttachmentsByPost( post.Id );

            String cmd;
            if (topic.IsAttachmentLogin == 1) {
                String lockImg = strUtil.Join( sys.Path.Img, "edit.gif" );
                cmd = alang( "currentDownloadPermission" );
                cmd += string.Format( "<img src=\"{1}\" /> <a href=\"{0}\" class=\"frmBox\">", to( new AttachmentController().SetPermission, post.Id ), lockImg );
                cmd += lang( "edit" ) + "</a>";
            }
            else {
                String lockImg = strUtil.Join( sys.Path.Img, "lock.gif" );
                cmd = string.Format( "<img src=\"{1}\" /> <a href=\"{0}\" class=\"frmBox\">" + alang( "setDownloadPermission" ) + "</a>", to( new AttachmentController().SetPermission, post.Id ), lockImg );
            }
            set( "cmd", cmd );


            bindAttachments( attachList );

        }

        private void bindAttachments( List<ContentAttachment> attachList ) {
            IBlock block = getBlock( "list" );

            foreach (ContentAttachment attachment in attachList) {

                block.Set( "a.Id", attachment.Id );

                block.Set( "a.Name", attachment.GetFileShowName() );

                String str;

                if (isImage( attachment )) {
                    str = string.Format( "<div><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" /></a></div>",
                        attachment.FileUrl, attachment.FileThumbUrl );
                }
                else {
                    str = alang( "noThumb" );
                }

                block.Set( "a.Info", str );
                block.Set( "a.Size", attachment.FileSizeKB );
                block.Set( "a.Downloads", attachment.Downloads );

                block.Set( "a.RenameLink", to( new AttachmentController().Rename, attachment.PostId ) + "?aid=" + attachment.Id );

                block.Set( "a.UploadLink", to( new AttachmentController().Upload, attachment.PostId ) + "?aid=" + attachment.Id );
                block.Set( "a.DeleteLink", to( new AttachmentController().Delete, attachment.PostId ) + "?aid=" + attachment.Id );

                block.Next();
            }
        }

        private Boolean isImage( ContentAttachment attachment ) {
            return Uploader.IsImage( attachment.Type );
        }

        public void SetPermission( int postId ) {

            ContentPost post = postService.GetById( postId, ctx.owner.Id );

            String chk = post.IsAttachmentLogin == 1 ? "checked=\"checked\"" : "";
            set( "checked", chk );

            target( SavePermission, postId );
        }

        [HttpPost, DbTransaction]
        public void SavePermission( int postId ) {

            ContentPost post = postService.GetById( postId, ctx.owner.Id );
            int ischeck = ctx.PostIsCheck( "IsAttachmentLogin" );
            postService.UpdateAttachmentPermission( post, ischeck );
            echoToParentPart( lang( "opok" ) );
        }


        [HttpPost, DbTransaction]
        public virtual void SaveSort( int postId ) {

            int id = ctx.PostInt( "id" );
            String cmd = ctx.Post( "cmd" );

            ContentAttachment data = attachService.GetById( id );

            ContentPost post = postService.GetById( postId, ctx.owner.Id );
            List<ContentAttachment> list = attachService.GetAttachmentsByPost( postId );

            if (cmd == "up") {

                new SortUtil<ContentAttachment>( data, list ).MoveUp();
                echoJsonOk();
            }
            else if (cmd == "down") {

                new SortUtil<ContentAttachment>( data, list ).MoveDown();
                echoJsonOk();
            }
            else {
                echoError( lang( "exUnknowCmd" ) );
            }

        }


        public void Add( int postId ) {

            target( SaveAdd, postId );
        }


        [HttpPost, DbTransaction]
        public void SaveAdd( int postId ) {

            ContentPost post = postService.GetById( postId, ctx.owner.Id );
            HttpFile postedFile = ctx.GetFileSingle();

            Result result = Uploader.SaveFileOrImage( postedFile );

            if (result.HasErrors) {
                errors.Join( result );
                run( Add, postId );
                return;
            }

            ContentAttachment uploadFile = new ContentAttachment();
            uploadFile.FileSize = postedFile.ContentLength;
            uploadFile.Type = postedFile.ContentType;
            uploadFile.Name = result.Info.ToString();
            uploadFile.Description = ctx.Post( "Name" );
            uploadFile.AppId = ctx.app.Id;
            uploadFile.PostId = postId;

            attachService.Create( uploadFile, (User)ctx.viewer.obj, ctx.owner.obj );

            echoToParentPart( lang( "opok" ) );
        }
        //--------------------------------------------------------------------------------------------------------------

        public void SaveFlashFile() {

            HttpFile postedFile = ctx.GetFileSingle();

            Result result = attachService.SaveFile( postedFile );

            Dictionary<String, String> dic = new Dictionary<String, String>();

            if (result.HasErrors) {

                dic.Add( "FileName", "" );
                dic.Add( "DeleteLink", "" );
                dic.Add( "Msg", result.ErrorsText );

                echoText( Json.ToString( dic ) );
            }
            else {

                ContentAttachment att = result.Info as ContentAttachment;
                String deleteLink = to( DeleteAttachment, att.Id );

                dic.Add( "FileName", att.Description );
                dic.Add( "DeleteLink", deleteLink );
                dic.Add( "Id", att.Id.ToString() );

                echoText( Json.ToString( dic ) );
            }

        }

        [HttpPost, DbTransaction]
        public void DeleteAttachment( int id ) {

            attachService.Delete( id ); // 删除文件，并且删除附件在数据库中的临时记录
            echoAjaxOk();
        }


        //--------------------------------------------------------------------------------------------------------------

        public void Rename( int postId ) {

            int id = ctx.GetInt( "aid" );
            set( "ActionLink", to( SaveRename, postId ) + "?aid=" + id );

            ContentAttachment attachment = attachService.GetById( id );
            if (attachment == null) {
                echoRedirect( alang( "exAttrachment" ) );
                return;
            }

            set( "name", attachment.GetFileShowName() );
        }

        [HttpPost, DbTransaction]
        public void SaveRename( int postId ) {

            ContentPost post = postService.GetById( postId, ctx.owner.Id );
            int id = ctx.GetInt( "aid" );

            String name = ctx.Post( "Name" );
            if (strUtil.IsNullOrEmpty( name )) {
                errors.Add( lang( "exName" ) );
                run( Rename, id );
                return;
            }

            ContentAttachment attachment = attachService.GetById( id );
            if (attachment == null) {
                echoRedirect( alang( "exAttrachment" ) );
                return;
            }

            attachService.UpdateName( attachment, name );
            echoToParentPart( lang( "opok" ) );
        }

        public void Upload( int postId ) {

            int id = ctx.GetInt( "aid" );

            ContentAttachment attachment = attachService.GetById( id );
            if (attachment == null) {
                echoRedirect( alang( "exAttrachment" ) );
                return;
            }

            set( "ActionLink", to( SaveUpload, postId ) + "?aid=" + id );
        }

        [HttpPost, DbTransaction]
        public void SaveUpload( int postId ) {

            ContentPost post = postService.GetById( postId, ctx.owner.Id );
            int id = ctx.GetInt( "aid" );

            ContentAttachment attachment = attachService.GetById( id );
            if (attachment == null) {
                echoRedirect( alang( "exAttrachment" ) );
                return;
            }

            HttpFile postedFile = ctx.GetFileSingle();

            Result result = Uploader.SaveFileOrImage( postedFile );

            if (result.HasErrors) {
                errors.Join( result );
                run( Upload, id );
                return;
            }

            String toDeleteFile = attachment.FileUrl;

            attachment.FileSize = postedFile.ContentLength;
            attachment.Type = postedFile.ContentType;
            attachment.Name = result.Info.ToString();

            attachService.UpdateFile( attachment, toDeleteFile );

            echoToParentPart( lang( "opok" ) );
        }

        [HttpDelete, DbTransaction]
        public void Delete( int postId ) {

            ContentPost post = postService.GetById( postId, ctx.owner.Id );
            int id = ctx.GetInt( "aid" );

            ContentAttachment attachment = attachService.GetById( id );
            if (attachment == null) {
                echoRedirect( alang( "exAttrachment" ) );
                return;
            }

            attachService.Delete( id );

            echoRedirectPart( lang( "opok" ) );
        }




    }
}
