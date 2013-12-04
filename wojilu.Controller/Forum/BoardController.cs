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
using wojilu.Web.Controller.Forum.Utils;
using wojilu.Web.Controller.Common;
using wojilu.Web.Controller.Forum.Caching;

namespace wojilu.Web.Controller.Forum {

    [App( typeof( ForumApp ) )]
    public partial class BoardController : ControllerBase {

        public virtual IForumBoardService boardService { get; set; }
        public virtual IModeratorService moderatorService { get; set; }

        private ForumBoard fb = null;
        private List<ForumBoard> boardsPath = null;
        private Tree<ForumBoard> _tree;

        public BoardController() {
            boardService = new ForumBoardService();
            moderatorService = new ModeratorService();
        }

        public override void CheckPermission() {

            hasPermission( ctx.route.id );

        }

        [CacheAction( typeof( ForumBoardCache ) )]
        public virtual void Show( long id ) {
            showPrivate( id );
        }

        public virtual void Category( long id ) {
            view( "Show" );
            ctx.SetItem( "forumCategory", ctx.GetLong("categoryId") );
            showPrivate( id );
        }

        //------------------------------------------
        public virtual void Views( long id ) {
            view( "Show" );
            ctx.SetItem( "forumSort", "views" );
            showPrivate( id );
        }
        public virtual void Replies( long id ) {
            view( "Show" );
            ctx.SetItem( "forumSort", "replies" );
            showPrivate( id );
        }
        public virtual void Replied( long id ) {
            view( "Show" );
            ctx.SetItem( "forumSort", "replied" );
            showPrivate( id );
        }
        public virtual void Created( long id ) {
            view( "Show" );
            ctx.SetItem( "forumSort", "created" );
            showPrivate( id );
        }
        //------------------------------------------
        public virtual void All( long id ) {
            view( "Show" );
            ctx.SetItem( "forumRecentTime", "all" );
            showPrivate( id );
        }
        public virtual void Day( long id ) {
            view( "Show" );
            ctx.SetItem( "forumRecentTime", "day" );
            showPrivate( id );
        }

        public virtual void DayTwo( long id ) {
            view( "Show" );
            ctx.SetItem( "forumRecentTime", "day2" );
            showPrivate( id );
        }
        public virtual void Week( long id ) {
            view( "Show" );
            ctx.SetItem( "forumRecentTime", "week" );
            showPrivate( id );
        }
        public virtual void Month( long id ) {
            view( "Show" );
            ctx.SetItem( "forumRecentTime", "month" );
            showPrivate( id );
        }
        public virtual void MonthThree( long id ) {
            view( "Show" );
            ctx.SetItem( "forumRecentTime", "month3" );
            showPrivate( id );
        }
        public virtual void MonthSix( long id ) {
            view( "Show" );
            ctx.SetItem( "forumRecentTime", "month6" );
            showPrivate( id );
        }

        //------------------------------------------

        private void showPrivate( long id ) {


            this.boardsPath = getTree().GetPath( id );
            ForumBoard board = getTree().GetById( id );
            this.fb = board;

            if (fb == null) return;

            ctx.Page.Title = this.fb.Name;
            ctx.Page.Description = this.fb.Description;

            // 1、当前路径
            set( "location", ForumLocationUtil.GetBoard( this.boardsPath, ctx ) );

            // 2、子版块
            bindChildBoards();

            // 3、主题列表
            if (this.fb.IsCategory == 0) {
                ctx.SetItem( "forumBoard", this.fb );
                ctx.SetItem( "pathBoards", this.boardsPath );
                load( "topicList", new TopicListController().Index );
            }
            else {
                set( "topicList", "" );
            }

        }

        private Boolean hasPermission( long id ) {

            this.boardsPath = getTree().GetPath( id );

            ForumBoard board = getTree().GetById( id );
            this.fb = board;

            if (this.boardsPath.Count == 0) {
                echo( alang( "exBoardNotFound" ) );
                return false;
            }

            return SecurityHelper.Check( this, fb );
        }


        private Tree<ForumBoard> getTree() {
            if (_tree == null) _tree = new Tree<ForumBoard>( boardService.GetBoardAll( ctx.app.Id, ctx.viewer.IsLogin ) );
            return _tree;
        }

    }
}

