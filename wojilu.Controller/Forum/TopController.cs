using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Interface;
using wojilu.Apps.Forum.Service;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Controller.Forum {

    public class TopController : ControllerBase {

        public IForumTopicService topicService { get; set; }
        public IForumPostService postService { get; set; }

        public TopController() {
            topicService = new ForumTopicService();
            postService = new ForumPostService();
        }

        public void SimpleList() {

            List<ForumTopic> newPosts = topicService.GetByApp( ctx.app.Id, 12 );
            bindRecentSimplePosts( newPosts );
        }

        [NonVisit]
        public void List() {

            ForumApp app = ctx.app.obj as ForumApp;

            set( "recentTopicLink", to( new RecentController().Topic ) );
            set( "recentPostLink", to( new RecentController().Post ) );
            set( "recentHotLink", to( new RecentController().Replies ) );
            set( "recentPickedImgLink", to( new RecentController().ImgTopic ) );

            ForumSetting s = app.GetSettingsObj();

            List<ForumPickedImg> pickedImg = ForumPickedImg.find( "AppId=" + ctx.app.Id ).list( s.HomeImgCount );
            bindImgs( pickedImg );

            List<ForumTopic> newPosts = topicService.GetByApp( ctx.app.Id, s.HomeListCount );
            bindTopics( newPosts, "topic" );

            List<ForumTopic> hots = topicService.GetByAppAndReplies( ctx.app.Id, s.HomeListCount, s.HomeHotDays );
            bindTopics( hots, "hot" );

            List<ForumPost> posts = postService.GetRecentByApp( ctx.app.Id, s.HomeListCount );
            bindPosts( posts, "post" );

        }

        private void bindImgs( List<ForumPickedImg> list ) {
            IBlock block = getBlock( "pickedImg" );
            foreach (ForumPickedImg f in list) {
                block.Set( "f.Title", f.Title );
                block.Set( "f.Url", f.Url );
                block.Set( "f.ImgUrl", f.ImgUrl );
                block.Next();
            }
        }

        private void bindTopics( List<ForumTopic> newPosts, String blockName ) {
            IBlock block = getBlock( blockName );
            int i = 1;
            foreach (ForumTopic t in newPosts) {
                block.Set( "topic.Index", i );
                block.Set( "topic.Title", t.Title );
                block.Set( "topic.Link", to( new TopicController().Show, t.Id ) );
                block.Next();
                i++;
            }
        }

        private void bindPosts( List<ForumPost> newPosts, String blockName ) {
            IBlock block = getBlock( blockName );
            int i = 1;
            foreach (ForumPost t in newPosts) {
                block.Set( "post.Index", i );
                block.Set( "post.Title", t.Title );
                block.Set( "post.Link", to( new PostController().Show, t.Id ) );
                block.Next();
                i++;
            }
        }

        private void bindRecentSimplePosts( List<ForumTopic> newPosts ) {
            String lblValue = getRecentPostsHtml( newPosts );
            set( "forumNewPosts", lblValue );
        }

        private String getRecentPostsHtml( List<ForumTopic> newPosts ) {
            StringBuilder builder = new StringBuilder();
            int onelineCount = 4;
            builder.Append( "<table><tr>" );
            for (int i = 0; i < newPosts.Count; i++) {
                if (((i % onelineCount) == 0) && (i > 0)) {
                    builder.Append( "</tr><tr>" );
                }
                ForumTopic data = newPosts[i];
                builder.AppendFormat( "<td><a href=\"{0}\" target=\"_blank\">{1}</a></td>", alink.ToAppData( data ), strUtil.SubString( data.Title, 17 ) );
            }
            builder.Append( "</tr></table>" );
            return builder.ToString();
        }

    }

}
