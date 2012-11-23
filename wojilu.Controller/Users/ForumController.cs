using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Interface;
using wojilu.Apps.Forum.Service;
using wojilu.Common.Money.Domain;
using wojilu.Web.Controller.Forum;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Web.Controller.Common;

namespace wojilu.Web.Controller.Users {

    public class ForumController : ControllerBase {

        public IForumTopicService topicService { get; set; }
        public IForumPostService postService { get; set; }
        public IForumBoardService boardService { get; set; }

        public ForumController() {
            topicService = new ForumTopicService();
            postService = new ForumPostService();
            boardService = new ForumBoardService();
        }

        public override void Layout() {
        }


        public void Topic() {

            if (ctx.viewer.HasPrivacyPermission( ctx.owner.obj, UserPermission.ForumTopic.ToString() ) == false) {
                echo( lang( "exVisitNoPermission" ) );
                return;
            }

            ctx.Page.Title = "论坛主题";

            DataPage<ForumTopic> plist = topicService.GetByUser( ctx.owner.Id, 50 );
            bintTopics( plist.Results, plist.PageBar );
            set( "lnkPost", to( Post ) );
        }

        public void Post() {

            if (ctx.viewer.HasPrivacyPermission( ctx.owner.obj, UserPermission.ForumTopic.ToString() ) == false) {
                echo( lang( "exVisitNoPermission" ) );
                return;
            }

            ctx.Page.Title = "论坛帖子";

            DataPage<ForumPost> results = postService.GetByUser( ctx.owner.Id, 50 );
            bindPosts( results );
            set( "lnkTopic", to( Topic ) );
        }


        private void bintTopics( List<ForumTopic> results, String pageBar ) {
            IBlock block = getBlock( "list" );
            foreach (ForumTopic t in results) {
                bindTopicOne( block, t );
            }
            set( "page", pageBar );
        }

        private void bindTopicOne( IBlock block, ForumTopic topic ) {            

            String typeImg = string.Empty;
            if (strUtil.HasText( topic.TypeName )) {
                typeImg = string.Format( "<img src='{0}apps/forum/{1}.gif'>", sys.Path.Skin, topic.TypeName );
            }

            block.Set( "p.Id", topic.Id );
            block.Set( "p.TypeImg", typeImg );
            block.Set( "p.TitleStyle", topic.TitleStyle );
            block.Set( "p.Titile", strUtil.CutString( topic.Title, 30 ) );
            block.Set( "p.Url", Link.To( topic.OwnerType, topic.OwnerUrl, new TopicController().Show, topic.Id, topic.AppId ) );

            block.Set( "p.BoardName", strUtil.SubString( topic.ForumBoard.Name, 10 ) );
            block.Set( "p.BoardUrl", Link.To( topic.OwnerType, topic.OwnerUrl, new BoardController().Show, topic.ForumBoard.Id, topic.AppId ) );


            block.Set( "p.CreateTime", topic.Created );
            block.Set( "p.Replied", topic.Replied );
            block.Set( "p.RepliedUserName", topic.RepliedUserName );

            block.Set( "p.ReplyCount", topic.Replies );
            block.Set( "p.Hits", topic.Hits.ToString() );

            String attachments = topic.Attachments > 0 ? "<img src='" + sys.Path.Img + "attachment.gif'/>" : "";
            block.Set( "p.Attachments", attachments );

            block.Next();
        }

        private void bindPosts( DataPage<ForumPost> results ) {
            IBlock block = getBlock( "list" );
            foreach (ForumPost t in results.Results) {
                bindPostOne( block, t );
            }
            set( "page", results.PageBar );
        }

        private void bindPostOne( IBlock block, ForumPost post ) {

            String title = post.Title;


            if (strUtil.IsNullOrEmpty( title )) {

                ForumTopic topic = topicService.GetByPost( post.Id );
                if (topic == null) return;

                title = "re:" + topic.Title;
            }

            block.Set( "p.Titile", title );
            block.Set( "p.Url", Link.To( post.OwnerType, post.OwnerUrl, new PostController().Show, post.Id, post.AppId ) );

            block.Set( "p.CreateTime", post.Created );

            block.Next();
        }


    }
}
