using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Forum.Interface;
using wojilu.Apps.Forum.Service;
using wojilu.Apps.Forum.Domain;
using wojilu.Web.Controller.Forum.Utils;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Forum.Users {

    [App( typeof( ForumApp ) )]
    public class TagController : ControllerBase {

        public IForumTopicService topicService { get; set; }
        public IForumBoardService boardService { get; set; }

        public TagController() {
            boardService = new ForumBoardService();
            topicService = new ForumTopicService();
        }


        [HttpPost, DbTransaction]
        public void SaveTag() {

            int postId = ctx.PostInt( "postId" );
            String tagValue = ctx.Post( "tagValue" );

            if (strUtil.IsNullOrEmpty( tagValue )) {
                echoText( "请输入内容" );
                return;
            }

            ForumTopic topic = topicService.GetById( postId, ctx.owner.obj );
            if (topic == null) { echo( lang( "exDataNotFound" ) ); return; }
            ForumPost post = topicService.GetPostByTopic( topic.Id );


            if (canAdminTag( post ) == false) {
                echoText( lang( "exNoPermission" ) );
                return;
            }

            topic.Tag.Save( tagValue );

            echoAjaxOk();
        }

        private Boolean canAdminTag( ForumPost post ) {
            if (ctx.viewer.IsLogin == false) return false;
            return post.Creator.Id == ctx.viewer.Id || hasAdminPermission( post );
        }

        private Boolean hasAdminPermission( ForumPost post ) {

            ForumBoard board = boardService.GetById( post.ForumBoardId, ctx.owner.obj );

            IList adminCmds = SecurityHelper.GetTopicAdminCmds( (User)ctx.viewer.obj, board, ctx );

            return adminCmds.Count > 0;
        }


    }

}
