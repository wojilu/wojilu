/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Service;
using wojilu.Apps.Forum.Interface;
using wojilu.Common.Onlines;
using wojilu.Members.Users.Service;
using wojilu.Members.Users.Interface;
using wojilu.Web.Controller.Common;
using wojilu.Web.Controller.Forum.Caching;

namespace wojilu.Web.Controller.Forum {

    [App( typeof( ForumApp ) )]
    public partial class ForumController : ControllerBase {

        public IForumBoardService boardService { get; set; }
        public IForumService forumService { get; set; }
        public IForumLinkService linkService { get; set; }
        public IForumTopicService topicService { get; set; }
        public IUserService userService { get; set; }
        public IForumPostService postService { get; set; }

        public ForumController() {
            forumService = new ForumService();
            boardService = new ForumBoardService();
            linkService = new ForumLinkService();
            topicService = new ForumTopicService();
            userService = new UserService();
            postService = new ForumPostService();
        }

        [CachePage( typeof( ForumIndexPageCache ) )]
        [CacheAction( typeof( ForumIndexCache ) )]
        public void Index() {

            WebUtils.pageTitle( this, ctx.app.Name );

            List<ForumBoard> categories = getTree().GetRoots();
            List<ForumLink> linkList = linkService.GetByApp( ctx.app.Id, ctx.owner.Id );

            ForumApp forum = ctx.app.obj as ForumApp;
            String notice = strUtil.HasText( forum.Notice ) ? "<div class=\"forumPanel\" id=\"forumNotice\">" + forum.Notice + "</div>" : "";
            set( "forumNotice", notice );

            bindAll( categories, linkList );
        }


        [NonVisit]
        public void TopList() {

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

        private Tree<ForumBoard> _tree;

        private Tree<ForumBoard> getTree() {
            if (_tree == null) _tree = new Tree<ForumBoard>( boardService.GetBoardAll( ctx.app.Id, ctx.viewer.IsLogin ) );
            return _tree;
        }

    }
}

