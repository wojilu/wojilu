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

namespace wojilu.Web.Controller.Forum {

    [App( typeof( ForumApp ) )]
    public class AttachmentController : ControllerBase {

        public IAttachmentService attachmentService { get; set; }
        public IForumTopicService topicService { get; set; }
        public IAttachmentService attachService { get; set; }
        public IForumBoardService boardService { get; set; }

        public AttachmentController() {
            boardService = new ForumBoardService();
            attachmentService = new AttachmentService();
            topicService = new ForumTopicService();
            attachService = new AttachmentService();
        }

        private Tree<ForumBoard> _tree;

        private Tree<ForumBoard> getTree() {
            if (_tree == null) _tree = new Tree<ForumBoard>( boardService.GetBoardAll( ctx.app.Id, ctx.viewer.IsLogin ) );
            return _tree;
        }

        public void Show( int id ) {

            String guid = ctx.Get( "id" );

            Attachment attachment = attachmentService.GetById( id, guid );
            if (attachment == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            // 权限控制
            ForumTopic topic = topicService.GetById( attachment.TopicId, ctx.owner.obj );
            if (SecurityHelper.Check( this, topic.ForumBoard ) == false) return;

            attachmentService.AddHits( attachment );

            // 检查盗链
            if (isDownloadValid() == false) {
                echoRedirect( alang( "exDownload" ) );
                return;
            }

            // 转发
            redirectUrl( attachment.FileUrl );

        }

        private Boolean isDownloadValid() {

            if (ctx.web.PathReferrer == null) return false;

            return ctx.web.PathReferrerHost.Equals( ctx.url.Host );
        }




    }

}
