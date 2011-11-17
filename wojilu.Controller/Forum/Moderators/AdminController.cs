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
    public class AdminController : ControllerBase {

        private String ids;

        public override void CheckPermission() {

            this.ids = ctx.GetIdList( "postSelect" );
            if (strUtil.IsNullOrEmpty( ids )) echoToParent( lang( "plsSelect" ) );            
        }

        public void Sticky( int id ) {
            adminPrivate( id );
            set( "ActionLink", to( new AdminSaveController().Sticky, id ) + "?ids=" + ids );
        }

        public void Picked( int id ) {
            adminPrivate( id );
            set( "ActionLink", to( new AdminSaveController().Pick, id ) + "?ids=" + ids );
        }

        public void Lock( int id ) {
            adminPrivate( id );
            set( "ActionLink", to( new AdminSaveController().Lock, id ) + "?ids=" + ids );
        }

        public void Delete( int id ) {
            adminPrivate( id );
            set( "ActionLink", to( new AdminSaveController().Delete, id ) + "?ids=" + ids );
        }

        public void Highlight( int id ) {
            set( "ActionLink", to( new AdminSaveController().Highlight, id ) + "?ids=" + ids );
        }

        //---------------------------------------------------------------------------------

        public void Category( int id ) {

            List<ForumCategory> categories = categoryService.GetByBoard( id );
            if (categories.Count == 0) {
                echoText( "<h1>" + alang( "exUnCategory" ) + "</h1>" );
                return;
            }

            set( "ActionLink", to( new AdminSaveController().Category, id ) + "?ids=" + ids );
            bindList( "categories", "c", categories );
        }

        public void SortSticky( int id ) {

            List<ForumTopic> globalStickyList = forumService.GetStickyTopics( ctx.app.obj as ForumApp );
            List<ForumTopic> stickyList = topicService.getSubstractStickyList( globalStickyList, id );

            bindList( "list", "t", stickyList );
            set( "reorderLink", to( new AdminSaveController().SaveStickySort, id ) );

            String location = ForumLocationUtil.GetTopicSort( boardsPath, ctx );
            set( "location", location );
        }

        //---------------------------------------------------------------------------------

        public void GlobalSortSticky() {

            ForumApp app = ctx.app.obj as ForumApp;
            PermissionUtil.Check( this, app );

            List<ForumTopic> globalStickyList = forumService.GetStickyTopics( app );
            bindList( "list", "t", globalStickyList );
            set( "reorderLink", to( new AdminSaveController().SaveGlobalStickySort ) );

            String location = ForumLocationUtil.GetGlobalTopicSort( ctx );
            set( "location", location );
        }

        public void GlobalSticky( int id ) {
            ForumApp app = ctx.app.obj as ForumApp;
            if (PermissionUtil.Check( this, app ) == false) return;

            adminPrivate( id );
            set( "ActionLink", to( new AdminSaveController().GlobalSticky, id ) + "?ids=" + ids );
        }

        public void Move( int id ) {
            ForumApp app = ctx.app.obj as ForumApp;
            PermissionUtil.Check( this, app );

            set( "ActionLink", to( new AdminSaveController().Move, id ) + "?ids=" + ids );
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

        public AdminController() {
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
