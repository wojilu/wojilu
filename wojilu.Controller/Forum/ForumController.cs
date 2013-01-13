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
using wojilu.Common.Picks;

namespace wojilu.Web.Controller.Forum {

    [App( typeof( ForumApp ) )]
    public partial class ForumController : ControllerBase {

        public IForumBoardService boardService { get; set; }
        public IForumService forumService { get; set; }
        public IForumLinkService linkService { get; set; }
        public IForumTopicService topicService { get; set; }
        public IUserService userService { get; set; }
        public IForumPostService postService { get; set; }
        public IForumPickService pickService { get; set; }

        public ForumController() {
            forumService = new ForumService();
            boardService = new ForumBoardService();
            linkService = new ForumLinkService();
            topicService = new ForumTopicService();
            userService = new UserService();
            postService = new ForumPostService();
            pickService = new ForumPickService();
        }

        [CachePage( typeof( ForumIndexPageCache ) )]
        [CacheAction( typeof( ForumIndexCache ) )]
        public void Index() {

            List<ForumBoard> categories = getTree().GetRoots();
            List<ForumLink> linkList = linkService.GetByApp( ctx.app.Id, ctx.owner.Id );

            ForumApp forum = ctx.app.obj as ForumApp;
            ForumSetting setting = forum.GetSettingsObj();

            ctx.Page.Title = ctx.app.Name;
            ctx.Page.Keywords = setting.MetaKeywords;
            ctx.Page.Description = setting.MetaDescription;

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

            List<ForumPickedImg> pickedImg = db.find<ForumPickedImg>( "AppId=" + ctx.app.Id ).list( s.HomeImgCount );
            bindImgs( pickedImg );

            //List<ForumTopic> newPosts = topicService.GetByApp( ctx.app.Id, s.HomeListCount );
            //bindTopics( newPosts, "topic" );

            //List<ForumTopic> hots = topicService.GetByAppAndReplies( ctx.app.Id, s.HomeListCount, s.HomeHotDays );
            //bindTopics( hots, "hot" );

            //List<ForumPost> posts = postService.GetRecentByApp( ctx.app.Id, s.HomeListCount );
            //bindPosts( posts, "post" );

            List<ForumTopic> newPosts = topicService.GetByApp( ctx.app.Id, 30 );
            List<MergedData> results = pickService.GetAll( newPosts, ctx.app.Id );

            bindCustomList( results );
        }


        private void bindCustomList( List<MergedData> list ) {

            IBlock hBlock = getBlock( "hotPick" );
            IBlock pBlock = getBlock( "pickList" );

            // 绑定第一个
            if (list.Count == 0) return;
            bindPick( list[0], hBlock, 1 );

            // 绑定列表
            if (list.Count == 1) return;
            for (int i = 1; i < list.Count; i++) {
                bindPick( list[i], pBlock, i + 1 );
            }
        }

        private void bindPick( MergedData x, IBlock block, int index ) {

            block.Set( "x.Title", x.Title );
            block.Set( "x.Summary", x.Summary );

            String lnk = x.Topic == null ? x.Link : alink.ToAppData( x.Topic );
            block.Set( "x.LinkShow", lnk );

            block.Next();
        }


        private Tree<ForumBoard> _tree;

        private Tree<ForumBoard> getTree() {
            if (_tree == null) _tree = new Tree<ForumBoard>( boardService.GetBoardAll( ctx.app.Id, ctx.viewer.IsLogin ) );
            return _tree;
        }

    }
}

