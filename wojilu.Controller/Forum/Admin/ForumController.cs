/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Web;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.Utils;

using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Service;

using wojilu.Web.Controller.Forum.Utils;
using wojilu.Common.AppBase.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Forum.Interface;
using wojilu.Common.AppBase;
using wojilu.Common.Money.Domain;
using wojilu.ORM;

namespace wojilu.Web.Controller.Forum.Admin {

    [App( typeof( ForumApp ) )]
    public partial class ForumController : ControllerBase {

        public IForumBoardService boardService { get; set; }
        public IForumCategoryService categoryService { get; set; }
        public IForumService forumService { get; set; }
        public IForumLogService logService { get; set; }
        public IModeratorService moderatorService { get; set; }
        public IForumTopicService topicService { get; set; }
        public IForumPostService postService { get; set; }

        public ForumController() {
            forumService = new ForumService();
            boardService = new ForumBoardService();
            categoryService = new ForumCategoryService();
            topicService = new ForumTopicService();
            moderatorService = new ModeratorService();
            logService = new ForumLogService();
            postService = new ForumPostService();
        }

        private Tree<ForumBoard> _tree;

        private Tree<ForumBoard> getTree() {
            if (_tree == null) _tree = new Tree<ForumBoard>( boardService.GetBoardAll( ctx.app.Id, ctx.viewer.IsLogin ) );
            return _tree;
        }

        public void Index() {

            view( "ListBoard" );
            set( "addCategoryUrl", to( AddCategory ) );

            set( "sortAction", to( SaveSort ) );

            List<ForumBoard> orderedList = getTree().GetAllOrdered();

            bindBoards( orderedList );
        }

        public void Notice() {
            redirect( new ForumPickController().Index );
        }

        public void Headline() {

            target( SaveNotice );

            ForumApp forum = ctx.app.obj as ForumApp;

            set( "Notice", forum.Notice );
        }

        [HttpPost, DbTransaction]
        public void SaveNotice() {

            String notice = ctx.PostHtml( "Notice" );

            ForumApp forum = ctx.app.obj as ForumApp;
            forum.Notice = notice;
            forum.update( "Notice" );

            echoRedirect( lang( "opok" ) );

        }


        [HttpPost, DbTransaction]
        public void SaveSort() {

            int id = ctx.PostInt( "id" );
            String cmd = ctx.Post( "cmd" );

            ForumBoard board = getTree().GetById( id );
            if (board == null) echoRedirect( "exBoardNotFound" );

            List<ForumBoard> list = (board.ParentId == 0 ? getTree().GetRoots() : getTree().GetChildren( board.ParentId ));

            if (cmd == "up") {

                new SortUtil<ForumBoard>( board, list ).MoveUp();
                echoJsonOk();
            }
            else if (cmd == "down") {

                new SortUtil<ForumBoard>( board, list ).MoveDown();
                echoJsonOk();
            }
            else {
                errors.Add( lang( "exUnknowCmd" ) );
                echoError();
            }
        }


        //-------------------------------------------------------------------------------


        public void AddCategory() {

            target( SaveCategory );
            bind( "c", new ForumBoard() );

            set( "ViewId", BoardViewStatus.GetDropList( "ViewId", 0 ) );
        }

        public void AddSubBoard( int boardId ) {

            view( "AddBoard" );

            ForumBoard board = boardService.GetById( boardId, ctx.owner.obj );
            if (board == null) {
                echoRedirect( alang( "exCategoryNotFound" ) );
                return;
            }

            target( SaveBoard );
            bindAddSubBoard( boardId, board );
        }

        [HttpPost, DbTransaction]
        public void SaveBoard() {
            ForumBoard fb = ForumValidator.ValidateBoard( ctx );
            if (errors.HasErrors) {
                run( AddSubBoard, fb.Id );
                return;
            }

            Result result = boardService.Insert( fb );
            if (result.HasErrors) {
                errors.Join( result );
                run( AddSubBoard, fb.Id );
                return;
            }

            String str = ((ForumApp)ctx.app.obj).Security;
            boardService.UpdateSecurity( fb, str );

            // 上传图片处理
            if (ctx.HasUploadFiles) {
                HttpFile uploadFile = ctx.GetFileSingle();
                if (uploadFile.ContentLength > 1) {
                    Result uploadResult = Uploader.SaveImg( uploadFile );
                    if (uploadResult.IsValid) {
                        boardService.UpdateLogo( fb, uploadResult.Info.ToString() );
                    }
                }
            }

            logService.Add( (User)ctx.viewer.obj, ctx.app.Id, alang( "addBoard" ) + ":" + fb.Name, ctx.Ip );
            echoToParentPart( lang( "opok" ) );
        }

        [HttpPost, DbTransaction]
        public void SaveCategory() {

            ForumBoard fb = ForumValidator.ValidateBoard( ctx );
            if (errors.HasErrors) {
                run( AddCategory );
                return;
            }

            // 确保只是作为分类
            fb.IsCategory = 1;

            Result result = boardService.Insert( fb );
            if (result.HasErrors) {
                errors.Join( result );
                run( AddCategory );
                return;
            }

            String str = ((ForumApp)ctx.app.obj).Security;
            boardService.UpdateSecurity( fb, str );


            logService.Add( (User)ctx.viewer.obj, ctx.app.Id, alang( "addBoardCategory" ) + ":" + fb.Name, ctx.Ip );
            echoToParentPart( lang( "opok" ) );
        }

        //-------------------------------------------------------------------------------                

        public void EditBoard( int id ) {

            view( "AddBoard" );
            ForumBoard board = boardService.GetById( id, ctx.owner.obj );
            if (board == null) {
                echoRedirect( alang( "exBoardNotFound" ) );
                return;
            }

            set( "lblForumAction", alang( "editBoard" ) );


            target( UpdateBoard, board.Id );

            bindBoard( board );
        }

        [HttpPost, DbTransaction]
        public void DeleteLogo( int id ) {

            ForumBoard board = boardService.GetById( id, ctx.owner.obj );
            if (board == null) {
                echoRedirect( alang( "exBoardNotFound" ) );
                return;
            }

            boardService.DeleteLogo( board );
            echoAjaxOk();
        }

        public void EditCategory( int id ) {

            view( "AddCategory" );
            ForumBoard board = boardService.GetById( id, ctx.owner.obj );

            if (board == null) {
                echoRedirect( alang( "exCategoryNotFound" ) );
                return;
            }
            target( UpdateCategory, id );
            bind( "c", board );
        }

        [HttpPost, DbTransaction]
        public void UpdateBoard( int id ) {

            ForumBoard board = boardService.GetById( id, ctx.owner.obj );
            if (board == null) {
                echoRedirect( alang( "exBoardNotFound" ) );
                return;
            }

            board = ForumValidator.ValidateBoard( board, ctx );
            if (errors.HasErrors) {
                run( EditBoard, id );
                return;
            }

            Result result = boardService.Update( board );
            if (result.HasErrors) {
                errors.Join( result );
                run( EditBoard, id );
                return;
            }

            // 上传图片处理
            if (ctx.HasUploadFiles) {
                HttpFile uploadFile = ctx.GetFileSingle();
                if (uploadFile.ContentLength > 1) {
                    Result uploadResult = Uploader.SaveImg( uploadFile );
                    if (uploadResult.IsValid) {
                        boardService.UpdateLogo( board, uploadResult.Info.ToString() );
                    }
                }
            }

            String logmsg = string.Format( alang( "editBoard" ) + ":{0}(id:{1})", board.Name, id );
            logService.Add( (User)ctx.viewer.obj, ctx.app.Id, logmsg, ctx.Ip );
            echoToParentPart( lang( "opok" ), to( Index ) );
        }

        [HttpPost, DbTransaction]
        public void UpdateCategory( int id ) {
            ForumBoard board = boardService.GetById( id, ctx.owner.obj );
            if (board == null) {
                echoRedirect( alang( "exCategoryNotFound" ) );
                return;
            }

            board = ForumValidator.ValidateBoard( board, ctx );
            // 确保只是作为分类
            board.IsCategory = 1;

            if (errors.HasErrors) {
                run( EditCategory, board.Id );
                return;
            }

            Result result = boardService.Update( board );
            if (result.HasErrors) {
                errors.Join( result );
                run( EditCategory, board.Id );
                return;
            }

            String logmsg = string.Format( alang( "editBoardCategory" ) + ":{0}(id:{1})", board.Name, id );

            logService.Add( (User)ctx.viewer.obj, ctx.app.Id, logmsg, ctx.Ip );
            echoRedirect( lang( "opok" ), Index );

        }

        //-------------------------------------------------------------------------------

        [HttpDelete, DbTransaction]
        public void DeleteBoard( int id ) {

            ForumBoard board = boardService.GetById( id, ctx.owner.obj );
            if (board == null) {
                echoRedirect( alang( "exBoardNotFound" ) );
                return;
            }

            Boolean hasChildren = boardService.HasChildren( board.Id );
            if (hasChildren) {
                echoRedirect( "请先删除子论坛" );
                return;
            }

            boardService.Delete( board );
            echoRedirect( lang( "opok" ), Index );
        }

        [HttpDelete, DbTransaction]
        public void DeleteCategory( int id ) {

            ForumBoard board = getTree().GetById( id );
            if (board == null) {
                echoRedirect( alang( "exCategoryNotFound" ) );
                return;
            }

            if (getTree().GetChildren( board.Id ).Count > 0) {
                echoRedirect( alang( "exDeleteSubFirstError" ) );
                return;
            }

            boardService.DeleteCategoryOnly( board );
            echoRedirect( lang( "opok" ), Index );
        }

        /*------------------------------------------------------------------------------------------------------------*/

        public void TopicTrash() {

            target( AdminTopicTrashList );
            set( "topicTrashLink", to( TopicTrash ) );
            set( "postTrashLink", to( PostTrash ) );

            DataPage<ForumTopic> deletedPage = topicService.GetDeletedPage( ctx.app.Id );

            bindTrashTopic( deletedPage );
        }

        public void ViewDeletedTopic( int id ) {
            ForumTopic topic = topicService.GetById_ForAdmin( id );
            bind( "t", topic );

            ForumLog log = logService.GetByDeletedTopicId( topic.Id );
            bind( "log", log );

            DataPage<ForumPost> list = postService.GetPageList_ForAdmin( id, 50 );
            bindList( "list", "p", list.Results );
            set( "page", list.PageBar );
        }

        public void PostTrash() {

            target( AdminPostTrashList );
            set( "topicTrashLink", to( TopicTrash ) );
            set( "postTrashLink", to( PostTrash ) );

            DataPage<ForumPost> deletedPage = postService.GetDeletedPage( ctx.app.Id );
            bindTrashPost( deletedPage );
        }

        public void ViewDeletedPost( int id ) {

            ForumPost post = postService.GetById_ForAdmin( id );
            ForumTopic topic = topicService.GetById_ForAdmin( post.TopicId );

            ForumLog log = logService.GetByDeletedPostId( post.Id );
            if (log == null)
                log = logService.GetByDeletedTopicId( topic.Id );

            bind( "log", log );

            set( "t.Link", alink.ToAppData( topic ) );
            set( "p.CreatorLink", toUser( post.Creator ) );

            bind( "t", topic );
            bind( "p", post );

        }

        [HttpPost, DbTransaction]
        public void AdminTopicTrashList() {

            String ids = ctx.PostIdList( "choice" );
            String cmd = ctx.Post( "action" );

            if (cmd.Equals( "restore" )) {
                topicService.Restore( ids );
                echoAjaxOk();
                return;
            }
            if (cmd.Equals( "deletetrue" )) {
                topicService.DeleteListTrue( ids, (User)ctx.viewer.obj, ctx.Ip );
                echoAjaxOk();
                return;
            }
        }

        [HttpPost, DbTransaction]
        public void AdminPostTrashList() {

            String ids = ctx.PostIdList( "choice" );
            String cmd = ctx.Post( "action" );

            if (cmd.Equals( "restore" )) {

                postService.Restore( ids );
                echoAjaxOk();
                return;
            }
            if (cmd.Equals( "deletetrue" )) {
                postService.DeleteListTrue( ids, (User)ctx.viewer.obj, ctx.Ip );
                echoAjaxOk();
                return;
            }

        }

        /*------------------------------------------------------------------------------------------------------------*/

        public void DataCombine() {

            target( SaveCombine );
            set( "ForumDropDown1", getTree().DropList( "ForumSource", 0 ) );
            set( "ForumDropDown2", getTree().DropList( "ForumTarget", 0 ) );
        }

        [HttpPost, DbTransaction]
        public void SaveCombine() {

            int srcId = ctx.PostInt( "ForumSource" );
            int targetId = ctx.PostInt( "ForumTarget" );

            ForumBoard srcBoard = boardService.GetById( srcId, ctx.owner.obj );
            ForumBoard targetBoard = boardService.GetById( targetId, ctx.owner.obj );

            if (srcBoard == null) { echoRedirect( alang( "exSrcBoardNotFound" ) ); return; }
            if (targetBoard == null) { echoRedirect( alang( "exTargetBoardNotFound" ) ); return; }
            if (targetBoard.IsCategory == 1) { echoRedirect( alang( "exTargetCantCategory" ) ); return; }

            boardService.Combine( srcBoard, targetBoard );

            String msg = string.Format( alang( "logCombine" ), srcBoard.Name, srcBoard.Id, targetBoard.Name, targetBoard.Id );
            logService.Add( (User)ctx.viewer.obj, ctx.app.Id, msg, ctx.Ip );
            echoRedirect( lang( "opok" ), Index );
        }


    }
}

