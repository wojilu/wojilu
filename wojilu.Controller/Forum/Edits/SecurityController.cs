using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Interface;
using wojilu.Apps.Forum.Service;
using wojilu.Web.Controller.Forum.Utils;

namespace wojilu.Web.Controller.Forum.Edits {

    public class SecurityController : ControllerBase {

        public IForumBoardService boardService { get; set; }
        public IForumTopicService topicService { get; set; }
        public IForumPostService postService { get; set; }

        public SecurityController() {
            boardService = new ForumBoardService();
            topicService = new ForumTopicService();
            postService = new ForumPostService();
        }

        public override void CheckPermission() {

            // 1) login
            if (ctx.viewer.IsLogin == false) {
                redirectLogin();
                return;
            }

            if (ctx.viewer.IsAdministrator()) return;

            // 2) data exist
            ForumTopic topic = null;
            IPost currentData = null;
            if (ctx.controller.GetType() == typeof( PostController )) {
                ForumPost post = postService.GetById( ctx.route.id, ctx.owner.obj );
                if (post == null) {
                    echoRedirect( lang( "exDataNotFound" ) + "(ForumPost)" );
                    return;
                }
                currentData = post;
                topic = topicService.GetById( post.TopicId, ctx.owner.obj );
            }
            else {
                topic = topicService.GetById( ctx.route.id, ctx.owner.obj );
                currentData = topic;
            }


            if (topic == null) {
                echoRedirect( lang( "exDataNotFound" ) + "_ForumTopic" );
                return;
            }

            // 3) self edit
            if (currentData.Creator.Id == ctx.viewer.Id) return;

            // 4) admin permission
            SecurityHelper.Check( this, topic.ForumBoard );
        }


    }

}
