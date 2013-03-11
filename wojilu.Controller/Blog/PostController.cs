/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Common.Upload;

using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Service;
using wojilu.Apps.Blog.Interface;

namespace wojilu.Web.Controller.Blog {

    [App( typeof( BlogApp ) )]
    public partial class PostController : ControllerBase {

        public IBlogPostService postService { get; set; }
        public UserFileService fileService { get; set; }

        public PostController() {
            postService = new BlogPostService();
            fileService = new UserFileService();
        }

        public void Show( int id ) {

            BlogPost post = postService.GetById( id, ctx.owner.Id );
            if (post == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            String saveStatusInfo = string.Empty;
            if (post.SaveStatus == 1) {
                saveStatusInfo = "<div class='warning border'>" + alang( "blog_draft_info" ) + "</div>";
                if (ctx.viewer.Id != ctx.owner.obj.Id) {
                    echoRedirect( alang( "blog_error_status" ) );
                    return;
                }
            }

            postService.AddHits( post );

            bindBlogPost( post, saveStatusInfo );

            set( "commentUrl", getCommentUrl( post ) );

            bindVisitor( post );
            bindAttachmentPanel( post );
        }


        private void bindAttachmentPanel( BlogPost post ) {
            IBlock attachmentPanel = getBlock( "attachmentPanel" );
            if (post.AttachmentCount > 0) {
                bindAttachments( attachmentPanel, post );
            }
        }

        private void bindAttachments( IBlock attachmentPanel, BlogPost post ) {

            List<UserFile> list = fileService.GetByData( post );

            IBlock block = attachmentPanel.GetBlock( "attachments" );
            foreach (UserFile obj in list) {

                if (obj.IsPic == 1) {
                    block.Set( "obj.PicLink", string.Format( "<div class=\"linePic\"><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\"/></a></div>", obj.PicO, obj.PicM ) );
                    block.Set( "obj.DownloadLink", string.Format( "<a href=\"{0}\" class=\"left10 lnkDown\">查看原图</a>", obj.PicO ) );
                }
                else {
                    block.Set( "obj.PicLink", "" );
                    block.Set( "obj.DownloadLink", string.Format( "<a href=\"{0}\" class=\"left10 lnkDown\">下载附件</a>", to( DownloadAttachment, obj.Id ) ) );
                }

                block.Set( "obj.FileName", obj.FileName );
                block.Set( "obj.FileSizeKB", obj.FileSizeKB );
                block.Set( "obj.DownloadUrl", to( DownloadAttachment, obj.Id ) );
                block.Next();
            }

            attachmentPanel.Next();
        }


        public void DownloadAttachment( int id ) {

            UserFile att = fileService.GetById( id );
            if (att == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            redirectUrl( att.PathFull );
        }


    }
}

