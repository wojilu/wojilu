/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Service;
using wojilu.Apps.Forum.Interface;
using wojilu.Web.Controller.Forum.Utils;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Forum {

    [App( typeof( ForumApp ) )]
    public partial class TopicListController : ControllerBase {

        public IForumService forumService { get; set; }
        public IForumTopicService topicService { get; set; }
        public IForumBoardService boardService { get; set; }
        public IForumCategoryService categoryService { get; set; }
        public IModeratorService moderatorService { get; set; }

        private ForumBoard fb;
        private List<ForumBoard> boardsPath;
        private Tree<ForumBoard> _tree;

        public TopicListController() {
            forumService = new ForumService();
            boardService = new ForumBoardService();
            topicService = new ForumTopicService();
            categoryService = new ForumCategoryService();
            moderatorService = new ModeratorService();
        }

        public override void CheckPermission() {

            this.boardsPath = getTree().GetPath( ctx.route.id );

            if (this.boardsPath.Count == 0) {
                echoRedirect( alang( "exBoardNotFound" ) );
                return;
            }

            ForumBoard board = getTree().GetById( ctx.route.id );

            this.fb = board;
            SecurityHelper.Check( this, fb );
        }


        public void Picked( int id ) {

            view( "Index" );

            this.boardsPath = getTree().GetPath( ctx.route.id );
            ForumBoard board = getTree().GetById( ctx.route.id );
            this.fb = board;


            set( "topicTypeName", alang( "pickedTopic" ) );
            set( "forumPath", ForumLocationUtil.GetBoard( this.boardsPath, ctx ) );

            Boolean isAdmin = false;
            List<ForumTopic> stickyList = null;
            DataPage<ForumTopic> topicList = topicService.FindPickedPage( id, getPageSize( ctx.app.obj ) );
            List<ForumCategory> categories = categoryService.GetByBoard( id );

            bindAll( id, stickyList, topicList, categories, isAdmin );
        }

        public void Polls( int id ) {

            view( "Index" );

            set( "topicTypeName", alang( "pollTopic" ) );
            set( "forumPath", ForumLocationUtil.GetBoard( this.boardsPath, ctx ) );

            Boolean isAdmin = false;
            List<ForumTopic> stickyList = null;
            DataPage<ForumTopic> topicList = topicService.FindPollPage( id, getPageSize( ctx.app.obj ) );
            List<ForumCategory> categories = categoryService.GetByBoard( id );

            bindAll( id, stickyList, topicList, categories, isAdmin );
        }


        [NonVisit]
        public void Index() {

            set( "forumPath", "" );
            set( "topicTypeName", alang( "normalTopic" ) );

            this.fb = ctx.GetItem( "forumBoard" ) as ForumBoard;
            this.boardsPath = ctx.GetItem( "pathBoards" ) as List<ForumBoard>;
            int id = fb.Id;

            IList adminCmds = SecurityHelper.GetTopicAdminCmds( (User)ctx.viewer.obj, fb, ctx );
            Boolean isAdmin = (adminCmds.Count > 0);

            ForumApp app = ctx.app.obj as ForumApp;
            int page = ctx.route.page;
            List<ForumTopic> globalStickyList = page <= 1 ? forumService.GetStickyTopics( app ) : null;
            List<ForumTopic> stickyList = topicService.getMergedStickyList( globalStickyList, id, page );

            int categoryId = cvt.ToInt( ctx.GetItem( "forumCategory" ) );
            String sort = ctx.GetItem( "forumSort" ) as String;
            String time = ctx.GetItem( "forumRecentTime" ) as String;
            DataPage<ForumTopic> topicList = topicService.FindTopicPage( id, getPageSize( ctx.app.obj ), categoryId, sort, time );
            List<ForumCategory> categories = categoryService.GetByBoard( id );

            bindAll( id, stickyList, topicList, categories, isAdmin );
        }

        private int getPageSize( object app ) {
            return ((ForumApp)app).GetSettingsObj().PageSize;
        }

        private Tree<ForumBoard> getTree() {
            if (_tree == null) _tree = new Tree<ForumBoard>( boardService.GetBoardAll( ctx.app.Id, ctx.viewer.IsLogin ) );
            return _tree;
        }



    }

}
