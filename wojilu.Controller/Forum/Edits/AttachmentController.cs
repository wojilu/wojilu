/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Service;
using wojilu.Apps.Forum.Interface;
using wojilu.Web.Utils;
using wojilu.Members.Users.Domain;
using wojilu.Common.AppBase.Interface;
using wojilu.Web.Controller.Forum.Utils;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Forum.Edits {

    [App( typeof( ForumApp ) )]
    public class AttachmentController : ControllerBase {

        public virtual IAttachmentService attachmentService { get; set; }
        public virtual IForumTopicService topicService { get; set; }
        public virtual IAttachmentService attachService { get; set; }
        public virtual IForumBoardService boardService { get; set; }

        public AttachmentController() {
            boardService = new ForumBoardService();
            attachmentService = new AttachmentService();
            topicService = new ForumTopicService();
            attachService = new AttachmentService();
        }

        public virtual void Admin( long topicId ) {


            set( "addLink", to( Add, topicId ) );

            set( "uploadLink", to( SaveAdd, topicId ) );
            set( "jsPath", sys.Path.DiskJs );
            set( "authJson", ctx.web.GetAuthJson() );


            set( "sortAction", to( SaveSort, topicId ) );

            ForumTopic topic = topicService.GetById( topicId, ctx.owner.obj );
            ForumPost data = topicService.GetPostByTopic( topicId );
            List<Attachment> attachList = attachService.GetByPost( data.Id );


            List<ForumBoard> pathboards = getTree().GetPath( data.ForumBoardId );
            set( "location", ForumLocationUtil.GetTopicAttachment( pathboards, topic, ctx ) );

            String cmd;
            if (topic.IsAttachmentLogin == 1) {
                String lockImg = strUtil.Join( sys.Path.Img, "edit.gif" );
                cmd = alang( "currentDownloadPermission" );
                cmd += string.Format( "<img src=\"{1}\" /> <a href=\"{0}\" class=\"frmBox\">", to( SetPermission, topicId ), lockImg );
                cmd += lang( "edit" ) + "</a>";
                //cmd = string.Format( "当前权限：查看附件需要登录 <img src=\"{1}\" /> <a href=\"{0}\" class=\"frmBox\">修改</a>", to( SetPermission, topicId ), lockImg );
            }
            else {
                String lockImg = strUtil.Join( sys.Path.Img, "lock.gif" );
                cmd = string.Format( "<img src=\"{1}\" /> <a href=\"{0}\" class=\"frmBox\">" + alang( "setDownloadPermission" ) + "</a>", to( SetPermission, topicId ), lockImg );
            }
            set( "cmd", cmd );


            bindAttachments( attachList );

        }

        public virtual void SetPermission( long topicId ) {

            ForumTopic topic = topicService.GetById( topicId, ctx.owner.obj );

            String chk = topic.IsAttachmentLogin == 1 ? "checked=\"checked\"" : "";
            set( "checked", chk );

            target( SavePermission, topicId );
        }

        [HttpPost, DbTransaction]
        public virtual void SavePermission( long topicId ) {

            ForumTopic topic = topicService.GetById( topicId, ctx.owner.obj );
            int ischeck = ctx.PostIsCheck( "IsAttachmentLogin" );
            topicService.UpdateAttachmentPermission( topic, ischeck );
            echoToParent( lang( "opok" ) );
        }

        private void bindAttachments( List<Attachment> attachList ) {
            IBlock block = getBlock( "list" );

            foreach (Attachment attachment in attachList) {

                block.Set( "a.Id", attachment.Id );

                block.Set( "a.Name", attachment.GetFileShowName() );

                String str;

                if ( attachment.IsImage ) {
                    str = string.Format( "<div><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" /></a></div>",
                        attachment.FileUrl, attachment.FileThumbUrl );
                    block.Set( "a.DownloadLink", attachment.FileUrl );

                }
                else {
                    str = alang( "noThumb" );
                    block.Set( "a.DownloadLink", to( new Forum.AttachmentController().Show, attachment.Id ) + "?id=" + attachment.Guid );
                }

                block.Set( "a.Info", str );
                block.Set( "a.Size", attachment.FileSizeKB );
                block.Set( "a.Downloads", attachment.Downloads );

                block.Set( "a.RenameLink", to( Rename, attachment.TopicId ) + "?aid=" + attachment.Id );

                block.Set( "a.UploadLink", to( Upload, attachment.TopicId ) + "?aid=" + attachment.Id );
                block.Set( "a.DeleteLink", to( Delete, attachment.TopicId ) + "?aid=" + attachment.Id );

                block.Next();
            }
        }

        [HttpPost, DbTransaction]
        public virtual void SaveSort( long topicId ) {

            long id = ctx.PostLong( "id" );
            String cmd = ctx.Post( "cmd" );

            Attachment data = attachmentService.GetById( id );
            String condition = (ctx.app == null ? "" : "AppId=" + ctx.app.Id);

            ForumTopic topic = topicService.GetById( topicId, ctx.owner.obj );
            ForumPost post = topicService.GetPostByTopic( topicId );
            List<Attachment> list = attachService.GetByPost( post.Id );

            if (cmd == "up") {

                new SortUtil<Attachment>( data, list ).MoveUp();
                echoJsonOk();
            }
            else if (cmd == "down") {

                new SortUtil<Attachment>( data, list ).MoveDown();
                echoJsonOk();
            }
            else {
                echoError( lang( "exUnknowCmd" ) );
            }

        }


        public virtual void Add( long topicId ) {

            target( SaveAdd, topicId );
        }


        [HttpPost, DbTransaction]
        public virtual void SaveAdd( long topicId ) {

            HttpFile postedFile = ctx.GetFileSingle();

            Result result = Uploader.SaveFileOrImage( postedFile );

            if (result.HasErrors) {
                errors.Join( result );
                run( Add, topicId );
                return;
            }

            Attachment uploadFile = new Attachment();
            uploadFile.FileSize = postedFile.ContentLength;
            uploadFile.Type = postedFile.ContentType;
            uploadFile.Name = result.Info.ToString();
            uploadFile.Description = ctx.Post( "Name" );
            uploadFile.AppId = ctx.app.Id;
            uploadFile.TopicId = topicId;

            attachmentService.Create( uploadFile, (User)ctx.viewer.obj, ctx.owner.obj );

            echoToParent( lang( "opok" ) );
        }

        //--------------------------------------------------------------------------------------------------------------

        public virtual void Rename( long topicId ) {

            long id = ctx.GetLong( "aid" );
            set( "ActionLink", to( SaveRename, topicId ) + "?aid=" + id );

            Attachment attachment = attachmentService.GetById( id );
            if (attachment == null) {
                echoRedirect( alang( "exAttrachment" ) );
                return;
            }

            set( "name", attachment.GetFileShowName() );
        }

        [HttpPost, DbTransaction]
        public virtual void SaveRename( long topicId ) {

            long id = ctx.GetLong( "aid" );

            String name = ctx.Post( "Name" );
            if (strUtil.IsNullOrEmpty( name )) {
                errors.Add( lang( "exName" ) );
                run( Rename, id );
                return;
            }

            Attachment attachment = attachmentService.GetById( id );
            if (attachment == null) {
                echoRedirect( alang( "exAttrachment" ) );
                return;
            }

            attachmentService.UpdateName( attachment, name );
            echoToParent( lang( "opok" ) );
        }

        public virtual void Upload( long topicId ) {

            long id = ctx.GetLong( "aid" );

            Attachment attachment = attachmentService.GetById( id );
            if (attachment == null) {
                echoRedirect( alang( "exAttrachment" ) );
                return;
            }

            set( "ActionLink", to( SaveUpload, topicId ) + "?aid=" + id );
        }

        [HttpPost, DbTransaction]
        public virtual void SaveUpload( long topicId ) {

            long id = ctx.GetLong( "aid" );

            Attachment attachment = attachmentService.GetById( id );
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

            User user = ctx.viewer.obj as User;
            attachmentService.UpdateFile( user, attachment, toDeleteFile );

            echoToParent( lang( "opok" ) );
        }

        [HttpDelete, DbTransaction]
        public virtual void Delete( long topicId ) {

            long id = ctx.GetLong( "aid" );

            Attachment attachment = attachmentService.GetById( id );
            if (attachment == null) {
                echoRedirect( alang( "exAttrachment" ) );
                return;
            }

            attachmentService.Delete( id );
            echoRedirect( lang( "opok" ) );
        }

        private Tree<ForumBoard> _tree;

        private Tree<ForumBoard> getTree() {
            if (_tree == null) _tree = new Tree<ForumBoard>( boardService.GetBoardAll( ctx.app.Id, ctx.viewer.IsLogin ) );
            return _tree;
        }

    }

}
