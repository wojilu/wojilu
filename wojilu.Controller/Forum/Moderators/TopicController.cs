/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Interface;
using wojilu.Apps.Forum.Service;
using wojilu.Web.Controller.Forum.Utils;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Controller.Forum.Moderators {

    [App( typeof( ForumApp ) )]
    public class TopicController : ControllerBase {

        private String ids;

        public override void CheckPermission() {

            if (ctx.route.action == "SortSticky" ||
                ctx.route.action == "GlobalSortSticky") return;

            this.ids = ctx.GetIdList( "ids" );
            if (strUtil.IsNullOrEmpty( ids )) echoToParent( lang( "plsSelect" ) );
        }

        public void Sticky() {
            int id = ctx.GetInt( "boardId" );
            adminPrivate( id );
            set( "ActionLink", to( new TopicSaveController().Sticky ) + "?boardId=" + id + "&ids=" + ids );
        }

        public void GlobalSticky() {
            int id = ctx.GetInt( "boardId" );
            ForumApp app = ctx.app.obj as ForumApp;
            adminPrivate( id );
            set( "ActionLink", to( new TopicSaveController().GlobalSticky ) + "?boardId=" + id + "&ids=" + ids );
        }

        public void Picked() {
            int id = ctx.GetInt( "boardId" );
            adminPrivate( id );
            set( "ActionLink", to( new TopicSaveController().Pick ) + "?boardId=" + id + "&ids=" + ids );
        }

        public void Lock() {
            int id = ctx.GetInt( "boardId" );
            adminPrivate( id );
            set( "ActionLink", to( new TopicSaveController().Lock ) + "?boardId=" + id + "&ids=" + ids );
        }

        public void Delete() {
            int id = ctx.GetInt( "boardId" );
            adminPrivate( id );
            set( "ActionLink", to( new TopicSaveController().Delete ) + "?boardId=" + id + "&ids=" + ids );
        }

        public void Highlight() {
            int id = ctx.GetInt( "boardId" );
            set( "ActionLink", to( new TopicSaveController().Highlight ) + "?boardId=" + id + "&ids=" + ids );
        }

        //---------------------------------------------------------------------------------

        public void Category() {

            int id = ctx.GetInt( "boardId" );
            List<ForumCategory> categories = categoryService.GetByBoard( id );
            if (categories.Count == 0) {
                echoText( "<h1>" + alang( "exUnCategory" ) + "</h1>" );
                return;
            }

            set( "ActionLink", to( new TopicSaveController().Category ) + "?boardId=" + id + "&ids=" + ids );
            bindList( "categories", "c", categories );
        }

        public void SortSticky() {

            int id = ctx.GetInt( "boardId" );

            this.boardsPath = getTree().GetPath( id );

            List<ForumTopic> globalStickyList = forumService.GetStickyTopics( ctx.app.obj as ForumApp );
            List<ForumTopic> stickyList = topicService.getSubstractStickyList( globalStickyList, id );

            bindList( "list", "t", stickyList );
            set( "reorderLink", to( new TopicSaveController().SaveStickySort ) + "?boardId=" + id );

            String location = ForumLocationUtil.GetTopicSort( this.boardsPath, ctx );
            set( "location", location );
        }

        //---------------------------------------------------------------------------------



        public void GlobalSortSticky() {

            ForumApp app = ctx.app.obj as ForumApp;

            List<ForumTopic> globalStickyList = forumService.GetStickyTopics( app );
            bindList( "list", "t", globalStickyList );
            set( "reorderLink", to( new TopicSaveController().SaveGlobalStickySort ) + "?boardId=" + ctx.GetInt( "boardId" ) );

            String location = ForumLocationUtil.GetGlobalTopicSort( ctx );
            set( "location", location );
        }


        public void Move() {
            int id = ctx.GetInt( "boardId" );
            ForumApp app = ctx.app.obj as ForumApp;

            set( "ActionLink", to( new TopicSaveController().Move ) + "?boardId=" + id + "&ids=" + ids );
            set( "ActionName", getActionName() );
            set( "dropForums", getTree().DropList( "targetForum", 0 ) );
        }

        //----------------------------------------------------------------------------------------------------


        private void adminPrivate( int id ) {
            view( "AdminPost" );
            set( "ActionName", getActionName() );
        }

        private String getActionName() {

            if (ctx.route.action.Equals( "Sticky" )) return alang( "cmdSticky" );
            if (ctx.route.action.Equals( "Picked" )) return alang( "cmdPick" );
            if (ctx.route.action.Equals( "Highlight" )) return alang( "cmdHighlight" );
            if (ctx.route.action.Equals( "Lock" )) return alang( "cmdLockTopic" );
            if (ctx.route.action.Equals( "Move" )) return alang( "cmdMoveTopic" );
            if (ctx.route.action.Equals( "Delete" )) return alang( "cmdDeleteTopic" );

            if (ctx.route.action.Equals( "GlobalSticky" )) return alang( "cmdGlobalSticky" );
            if (ctx.route.action.Equals( "GlobalStickyUndo" )) return alang( "cmdGlobalStickyUndo" );

            if (ctx.route.action.Equals( "Category" )) return alang( "cmdCatetory" );

            if (ctx.route.action.Equals( "StickyUndo" )) return alang( "cmdUnSticky" );
            if (ctx.route.action.Equals( "PickedUndo" )) return alang( "cmdUnPick" );
            if (ctx.route.action.Equals( "HighlightUndo" )) return alang( "cmdUnHighlight" );
            if (ctx.route.action.Equals( "LockUndo" )) return alang( "cmdUnLockTopic" );

            return alang( "cmdPostAdmin" );
        }

        //----------------------------------------------------------------------------------------------------

        public IForumTopicService topicService { get; set; }
        public IForumBoardService boardService { get; set; }
        public IForumCategoryService categoryService { get; set; }
        public IForumLogService logService { get; set; }
        public IForumService forumService { get; set; }

        public TopicController() {
            boardService = new ForumBoardService();
            topicService = new ForumTopicService();
            categoryService = new ForumCategoryService();
            logService = new ForumLogService();
            forumService = new ForumService();
        }


        //private ForumBoard fb;
        private List<ForumBoard> boardsPath;

        private Tree<ForumBoard> _tree;

        private Tree<ForumBoard> getTree() {
            if (_tree == null) _tree = new Tree<ForumBoard>( boardService.GetBoardAll( ctx.app.Id, ctx.viewer.IsLogin ) );
            return _tree;
        }


    }

}
