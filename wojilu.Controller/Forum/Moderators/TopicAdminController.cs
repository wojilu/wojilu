///*
// * Copyright (c) 2010, www.wojilu.com. All rights reserved.
// */

//using System;
//using System.Collections.Generic;
//using System.Text;

//using wojilu.Web.Mvc;
//using wojilu.Web.Mvc.Attr;
//using wojilu.Web.Controller.Forum.Utils;

//using wojilu.Apps.Forum.Domain;
//using wojilu.Apps.Forum.Service;

//using wojilu.Common.Money.Domain;
//using wojilu.Common.Msg.Service;
//using wojilu.Members.Users.Domain;
//using wojilu.Apps.Forum.Interface;
//using wojilu.Common.Msg.Interface;
//using wojilu.Web.Controller.Common;

//namespace wojilu.Web.Controller.Forum {

//    [App( typeof( ForumApp ) )]
//    public class TopicAdminController : ControllerBase {

//        public IForumTopicService topicService { get; set; }
//        public IForumBoardService boardService { get; set; }
//        public IForumCategoryService categoryService { get; set; }
//        public IForumLogService logService { get; set; }
//        public IMessageService msgService { get; set; }
//        public IForumService forumService { get; set; }

//        public TopicAdminController() {
//            boardService = new ForumBoardService();
//            topicService = new ForumTopicService();
//            categoryService = new ForumCategoryService();
//            logService = new ForumLogService();
//            msgService = new MessageService();
//            forumService = new ForumService();
//        }

//        private ForumBoard fb;
//        private List<ForumBoard> boardsPath;

//        private Tree<ForumBoard> _tree;

//        private Tree<ForumBoard> getTree() {
//            if (_tree == null) _tree = new Tree<ForumBoard>( boardService.GetBoardAll( ctx.app.Id, ctx.viewer.IsLogin ) );
//            return _tree;
//        }

//        private Boolean hasPermission( int id ) {

//            this.boardsPath = getTree().GetPath( id );

//            if (this.boardsPath.Count == 0) {
//                echo( alang( "exBoardNotFound" ) );
//                return false;
//            }


//            ForumBoard board = getTree().GetById( id );

//            this.fb = board;
//            return PermissionUtil.Check( this, fb );
//        }

//        // 版块列表
//        public void AdminSticky( int id ) {
//            adminPrivate( AdminSticky, id );
//        }

//        public void AdminStickyUndo( int id ) {
//            adminPrivate( AdminStickyUndo, id );
//        }

//        // 所有版块
//        public void AdminGlobalSticky( int id ) {

//            ForumApp app = ctx.app.obj as ForumApp;
//            if (PermissionUtil.Check( this, app ) == false) return;

//            adminPrivate( AdminGlobalSticky, id );
//            new ForumCacheRemove( boardService, this ).GlobalSticky();
//        }

//        public void AdminGlobalStickyUndo( int id ) {

//            ForumApp app = ctx.app.obj as ForumApp;
//            if (PermissionUtil.Check( this, app ) == false) return;

//            adminPrivate( AdminGlobalStickyUndo, id );
//            new ForumCacheRemove( boardService, this ).GlobalSticky();
//        }

//        //------------------------------------------------------------------


//        public void SortSticky( int id ) {

//            if (!hasPermission( id )) return;

//            List<ForumTopic> globalStickyList = forumService.GetStickyTopics( ctx.app.obj as ForumApp );
//            List<ForumTopic> stickyList = topicService.getSubstractStickyList( globalStickyList, id );

//            bindList( "list", "t", stickyList );
//            set( "reorderLink", to( SaveStickySort, id ) );

//            String location = ForumLocationUtil.GetTopicSort( boardsPath, ctx );
//            set( "location", location );
//        }

//        [HttpPost, DbTransaction]
//        public void SaveStickySort( int id ) {

//            if (!hasPermission( id )) return;

//            ForumBoard bd = boardService.GetById( id, ctx.owner.obj);

//            int topicId = ctx.PostInt( "id" );
//            String cmd = ctx.Post( "cmd" );
//            if (cmd == "up") {
//                topicService.StickyMoveUp( topicId );
//                echoRedirect( "ok" );
//            }
//            else if (cmd == "down") {
//                topicService.StickyMoveDown( topicId );
//                echoRedirect( "ok" );
//            }
//            else {
//                errors.Add( lang( "exUnknowCmd" ) );
//                echoError();
//            }

//            new ForumCacheRemove( this.boardService, this ).SortTopic( bd );
//        }

//        public void GlobalSortSticky() {

//            ForumApp app = ctx.app.obj as ForumApp;
//            PermissionUtil.Check( this, app );

//            List<ForumTopic> globalStickyList = forumService.GetStickyTopics( app );
//            bindList( "list", "t", globalStickyList );
//            set( "reorderLink", to( SaveGlobalStickySort ) );

//            String location = ForumLocationUtil.GetGlobalTopicSort( ctx );
//            set( "location", location );
//        }

//        public void SaveGlobalStickySort() {

//            int topicId = ctx.PostInt( "id" );
//            String cmd = ctx.Post( "cmd" );

//            ForumApp app = ctx.app.obj as ForumApp;
//            PermissionUtil.Check( this, app );

//            if (cmd == "up") {

//                forumService.StickyMoveUp( app, topicId );
//                echoRedirect( "ok" );
//            }
//            else if (cmd == "down") {
//                forumService.StickyMoveDown( app, topicId );
//                echoRedirect( "ok" );
//            }
//            else {
//                errors.Add( lang( "exUnknowCmd" ) );
//                echoError();
//            }

//        }

//        //-----------------

//        public void AdminPicked( int id ) {
//            adminPrivate( AdminPicked, id );
//        }

//        public void AdminPickedUndo( int id ) {
//            adminPrivate( AdminPickedUndo, id );
//        }

//        //-----------------

//        public void AdminHighlight( int id ) {

//            if (!hasPermission( id )) return;

//            if ("true".Equals( ctx.Get( "save" ) )) {

//                String idList = ctx.Get( "ids" );
//                if (cvt.IsIdListValid( idList ) == false) {
//                    echoToParent( lang( "plsSelect" ) );
//                    return;
//                }


//                String action = string.Format( "set TitleStyle='{0}'", strUtil.SqlClean( FormController.GetTitleStyle( ctx ), 150 ) );
//                ForumTopic.updateBatch( action, "Id in (" + idList + ")" );

//                String boardUrl = ctx.Post( "boardUrl" );
//                new ForumCacheRemove( this.boardService, this ).HighlightTopic( boardUrl, idList );

//                log( idList, ForumLogAction.Highlight );
//            }
//            else {
//                view( "Highlight" );
//                String idList = ctx.GetIdList( "postSelect" );
//                set( "ActionLink", to( AdminHighlight, id ) + "?save=true&ids=" + idList );
//                set( "boardUrl", ctx.Get( "boardUrl" ) );
//            }
//        }

//        public void AdminHighlightUndo( int id ) {

//            if (!hasPermission( id )) return;

//            String idList = ctx.GetIdList( "postSelect" );
//            String action = "set TitleStyle=''";
//            ForumTopic.updateBatch( action, "Id in (" + idList + ")" );

//            String boardUrl = ctx.Get( "boardUrl" );
//            new ForumCacheRemove( this.boardService, this ).HighlightTopic( boardUrl, idList );

//            log( idList, ForumLogAction.UnHighlight );

//            echoToParent( lang( "opok" ) );
//        }


//        public void AdminCategory( int id ) {

//            List<ForumCategory> categories = categoryService.GetByBoard( id );
//            if (categories.Count == 0) {
//                echoText( "<h1>" + alang( "exUnCategory" ) + "</h1>" );
//                return;
//            }

//            if ("true".Equals( ctx.Get( "save" ) )) {

//                if (!hasPermission( id )) return;

//                String idList = ctx.Get( "ids" );
//                if (cvt.IsIdListValid( idList ) == false) {
//                    echoToParent( lang( "plsSelect" ) );
//                    return;
//                }

//                int categoryId = ctx.PostInt( "dropCategories" );
//                ForumCategory category = categoryService.GetById( categoryId, ctx.owner.obj );
//                if (category == null && categoryId > 0) {
//                    echoText( "<h1>" + alang( "exCategoryNotFound" ) + "</h4>" );
//                    return;
//                }

//                String action = string.Format( "set CategoryId=" + categoryId );
//                ForumTopic.updateBatch( action, "Id in (" + idList + ")" );
//                log( idList, ForumLogAction.SetCategory );
//            }
//            else {
//                String idList = ctx.GetIdList( "postSelect" );
//                set( "ActionLink", to( AdminCategory, id ) + "?save=true&ids=" + idList );
//                bindList( "categories", "c", categories );
//            }
//        }

//        //-----------------

//        public void AdminLock( int id ) {
//            adminPrivate( AdminLock, id );
//        }

//        public void AdminLockUndo( int id ) {
//            adminPrivate( AdminLockUndo, id );
//        }

//        //-----------------

//        public void AdminMove( int id ) {


//            ForumApp app = ctx.app.obj as ForumApp;
//            PermissionUtil.Check( this, app );

//            if (!hasPermission( id )) return;

//            String idList = ctx.GetIdList( "postSelect" );
//            set( "ActionLink", to( AdminMoveSave, id ) + "?ids=" + idList );
//            set( "ActionName", getActionName() );
//            set( "dropForums", getTree().DropList( "targetForum", 0 ) );
//        }

//        public void AdminMoveSave( int id ) {

//            ForumApp app = ctx.app.obj as ForumApp;
//            PermissionUtil.Check( this, app );

//            if (!hasPermission( id )) return;

//            run( AdminSave, id );
//        }


//        //-----------------

//        public void AdminDelete( int id ) {
//            adminPrivate( AdminDelete, id );
//        }

//        private void adminPrivate( aActionWithId method, int id ) {

//            if (!hasPermission( id )) return;

//            if ("true".Equals( ctx.Get( "save" ) )) {
//                run( AdminSave, id );
//            }
//            else {
//                view( "AdminPost" );

//                String idList = ctx.GetIdList( "postSelect" );
//                set( "ActionLink", to( method, id ) + "?save=true&ids=" + idList );
//                set( "ActionName", getActionName() );
//                set( "boardUrl", ctx.Get( "boardUrl" ) );
//            }
//        }

//        private String getActionName() {

//            if (ctx.route.action.Equals( "AdminSticky" )) return alang( "cmdSticky" );
//            if (ctx.route.action.Equals( "AdminPicked" )) return alang( "cmdPick" );
//            if (ctx.route.action.Equals( "AdminHighlight" )) return alang( "cmdHighlight" );
//            if (ctx.route.action.Equals( "AdminLock" )) return alang( "cmdLockTopic" );
//            if (ctx.route.action.Equals( "AdminMove" )) return alang( "cmdMoveTopic" );
//            if (ctx.route.action.Equals( "AdminDelete" )) return alang( "cmdDeleteTopic" );

//            if (ctx.route.action.Equals( "AdminGlobalSticky" )) return alang( "cmdGlobalSticky" );
//            if (ctx.route.action.Equals( "AdminGlobalStickyUndo" )) return alang( "cmdGlobalStickyUndo" );

//            if (ctx.route.action.Equals( "AdminCategory" )) return alang( "cmdCatetory" );


//            if (ctx.route.action.Equals( "AdminStickyUndo" )) return alang( "cmdUnSticky" );
//            if (ctx.route.action.Equals( "AdminPickedUndo" )) return alang( "cmdUnPick" );
//            if (ctx.route.action.Equals( "AdminHighlightUndo" )) return alang( "cmdUnHighlight" );
//            if (ctx.route.action.Equals( "AdminLockUndo" )) return alang( "cmdUnLockTopic" );

//            return alang( "cmdPostAdmin" );
//        }

//        [NonVisit]
//        public void AdminSave( int id ) {

//            if (!hasPermission( id )) return;

//            String idList = ctx.GetIdList( "ids" );
//            if (strUtil.IsNullOrEmpty( idList )) {
//                echoToParent( lang( "plsSelect" ) );
//                return;
//            }

//            String boardUrl = ctx.Post( "boardUrl" );

//            String condition = "Id in (" + idList + ")";

//            if (ctx.route.action.Equals( "AdminSticky" )) {
//                topicService.AdminUpdate( "set Status=" + TopicStatus.Sticky, condition );
//                topicService.AddAuthorIncome( condition, UserAction.Forum_TopicSticky.Id );
//                log( idList, ForumLogAction.Sticky );
//                new ForumCacheRemove( boardService, this ).Sticky( this.fb );
//            }
//            else if (ctx.route.action.Equals( "AdminStickyUndo" )) {
//                topicService.AdminUpdate( "set Status=" + TopicStatus.Normal, condition );
//                topicService.SubstractAuthorIncome( condition, UserAction.Forum_TopicSticky.Id );
//                log( idList, ForumLogAction.UnSticky );
//                new ForumCacheRemove( boardService, this ).Sticky( this.fb );
//            }
//            else if (ctx.route.action.Equals( "AdminPicked" )) {
//                topicService.AdminUpdate( "set IsPicked=1", condition );
//                topicService.AddAuthorIncome( condition, UserAction.Forum_TopicPicked.Id );
//                log( idList, ForumLogAction.Pick );
//                new ForumCacheRemove( boardService, this ).PickTopics( boardUrl );
//            }
//            else if (ctx.route.action.Equals( "AdminPickedUndo" )) {
//                topicService.AdminUpdate( "set IsPicked=0", condition );
//                topicService.SubstractAuthorIncome( condition, UserAction.Forum_TopicPicked.Id );
//                log( idList, ForumLogAction.UnPick );
//                new ForumCacheRemove( boardService, this ).PickTopics( boardUrl );
//            }
//            else if (ctx.route.action.Equals( "AdminLock" )) {
//                topicService.AdminUpdate( "set IsLocked=1", condition );
//                topicService.SubstractAuthorIncome( condition, UserAction.Forum_TopicLocked.Id );
//                log( idList, ForumLogAction.Lock );
//                new ForumCacheRemove( boardService, this ).LockTopics( boardUrl, idList ); // 需要分页

//            }
//            else if (ctx.route.action.Equals( "AdminLockUndo" )) {
//                topicService.AdminUpdate( "set IsLocked=0", condition );
//                topicService.AddAuthorIncome( condition, UserAction.Forum_TopicLocked.Id );
//                log( idList, ForumLogAction.UnLock );
//                new ForumCacheRemove( boardService, this ).LockTopics( boardUrl, idList ); // 需要分页
//            }
//            else if (ctx.route.action.Equals( "AdminDelete" )) {
//                topicService.DeleteListToTrash( idList );
//                topicService.SubstractAuthorIncome( condition, UserAction.Forum_TopicDeleted.Id );
//                log( idList, ForumLogAction.Delete );
//                new ForumCacheRemove( boardService, this ).DeleteTopic( this.fb, idList ); // 需要分页
//            }

//            else if (ctx.route.action.Equals( "AdminGlobalSticky" )) {
//                topicService.SetGlobalSticky( ctx.app.Id, idList );
//                log( idList, ForumLogAction.GlobalSticky );
//                new ForumCacheRemove( boardService, this ).GlobalSticky();

//            }
//            else if (ctx.route.action.Equals( "AdminGlobalStickyUndo" )) {
//                topicService.SetGloablStickyUndo( ctx.app.Id, idList );
//                log( idList, ForumLogAction.GlobalUnSticky );
//                new ForumCacheRemove( boardService, this ).GlobalSticky();
//            }

//            else if (ctx.route.action.Equals( "AdminMoveSave" )) {

//                ForumBoard targetBoard = getTree().GetById( ctx.PostInt( "targetForum" ) );

//                if (targetBoard == null) {
//                    errors.Add( alang( "exBoardNotFound" ) );
//                    run( AdminMove, id );
//                    return;
//                }

//                if (targetBoard.IsCategory == 1) {
//                    errors.Add( alang( "exTargetCantCategory" ) );
//                    run( AdminMove, id );
//                    return;
//                }

//                topicService.Move( targetBoard.Id, idList );
//                log( idList, ForumLogAction.MoveTopic );

//                new ForumCacheRemove( boardService, this ).MoveTopic( this.fb, targetBoard, idList );
//            }
//        }

//        private void log( String idList, int actionId ) {

//            String reason = getReason();

//            List<ForumTopic> topics = topicService.GetByIds( idList );

//            foreach (ForumTopic topic in topics) {

//                logService.AddTopic( (User)ctx.viewer.obj, ctx.app.Id, topic.Id, actionId, reason, ctx.Ip );

//                // 发送短信通知
//                if (ctx.PostIsCheck( "IsSendMsg" ) != 1) continue;

//                String msg = ForumLogAction.GetLable( actionId );

//                //String title = "你的帖子《" + topic.Title + "》被" + msg;
//                String title = string.Format( alang( "adminPostMsgTitle" ), topic.Title, msg );

//                //String body = "[事件]：你的帖子《<a href='" + alink.ToAppData( topic ) + "'>" + topic.Title + "</a>》被版主" + msg +
//                //    "<br/>[发生时间]：" + DateTime.Now +
//                //    "<br/>[操作原因]：" + reason;

//                String body = string.Format( alang( "adminPostMsgBody" ), "<a href='" + alink.ToAppData( topic ) + "'>" + topic.Title + "</a>",
//                    msg, DateTime.Now, reason
//                    );

//                msgService.SiteSend( title, body, topic.Creator );
//            }

//            echoToParent( lang( "opok" ) );
//        }


//        private String getReason() {
//            return ctx.PostIsCheck( "chkReason" ) == 1 ? ctx.Post( "reasonText" ) : ctx.Post( "reasonSelect" );
//        }

//    }

//}
