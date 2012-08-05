/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Service;
using wojilu.Web.Controller.Forum.Utils;
using wojilu.Common.AppBase;
using wojilu.Common.AppBase.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Forum.Interface;

namespace wojilu.Web.Controller.Forum.Admin {


    [App( typeof( ForumApp ) )]
    public class CategoryController : ControllerBase {

        public IForumBoardService boardService { get; set; }
        public IForumCategoryService categoryService { get; set; }
        public IForumService forumService { get; set; }
        public IForumLogService logService { get; set; }

        public CategoryController() {
            forumService = new ForumService();
            boardService = new ForumBoardService();
            categoryService = new ForumCategoryService();
            logService = new ForumLogService();
        }

        public void Admin( int boardId ) {

            ForumBoard board = boardService.GetById( boardId, ctx.owner.obj );
            if (board == null) {
                echoRedirect( alang( "exBoardNotFound" ) );
                return;
            }

            target( Create );
            set( "sortAction", to( SaveSort, boardId ) );

            bindCategories( categoryService.GetByBoard( boardId ), getBlock( "list" ) );
            set( "BoardId", boardId );
            set( "forumBoard.Name", board.Name );
        }

        private void bindCategories( List<ForumCategory> list, IBlock block ) {
            foreach (ForumCategory category in list) {
                block.Set( "category.Id", category.Id );
                block.Set( "category.Name", category.Name );
                block.Set( "category.NameColor", category.NameColor );
                block.Set( "category.TopicCount", category.TopicCount );
                block.Set( "category.EditUrl", to( Edit, category.Id ) );
                block.Set( "category.DeleteUrl", to( Delete, category.Id ) );
                block.Next();
            }
        }

        [HttpPost, DbTransaction]
        public void Create() {

            ForumCategory category = ForumValidator.ValidateCategory( ctx );
            if (errors.HasErrors) {
                run( Admin, category.BoardId );
                return;
            }

            ForumBoard board = boardService.GetById( category.BoardId, ctx.owner.obj );
            if (board == null) {
                run( Admin, category.BoardId );
                return;
            }

            category.AppId = board.AppId;
            Result result = categoryService.Insert( category );
            if (result.HasErrors) {
                errors.Join( result );
                run( Admin, category.BoardId );
                return;
            }

            logService.Add( (User)ctx.viewer.obj, ctx.app.Id, string.Format( alang( "logAddCategory" ), category.Name, board.Name ), ctx.Ip );
            redirect( Admin, board.Id );
        }

        public void Edit( int id ) {
            ForumCategory category = categoryService.GetById( id, ctx.owner.obj );
            if (category == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }
            set( "Name", category.Name );
            set( "NameColor", category.NameColor );
            set( "BoardId", category.BoardId );
            target( Update, id );
        }

        [HttpPost, DbTransaction]
        public void Update( int id ) {
            ForumCategory category = categoryService.GetById( id, ctx.owner.obj );
            if (category == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }
            category = ForumValidator.ValidateCategory( category, ctx );
            Result result = categoryService.Update( category );
            if (result.HasErrors) {
                errors.Join( result );
                run( Edit, id );
            }
            else {
                logService.Add( (User)ctx.viewer.obj, ctx.app.Id, alang( "logEditCategory" ) + ":" + category.Name, ctx.Ip );
                redirect( Admin, category.BoardId );
            }
        }

        [HttpDelete, DbTransaction]
        public void Delete( int id ) {
            ForumCategory category = categoryService.GetById( id, ctx.owner.obj );
            if (category == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }
            categoryService.Delete( category );
            redirect( Admin, category.BoardId );
        }

        [HttpPost, DbTransaction]
        public void SaveSort( int boardId ) {

            int id = ctx.PostInt( "id" );
            String cmd = ctx.Post( "cmd" );

            ForumCategory category = categoryService.GetById( id, ctx.owner.obj );
            if (category == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            List<ForumCategory> list = categoryService.GetByBoard( boardId );

            if (cmd == "up") {
                new SortUtil<ForumCategory>( category, list ).MoveUp();
                echoJsonOk();
            }
            else if (cmd == "down") {
                new SortUtil<ForumCategory>( category, list ).MoveDown();
                echoJsonOk();
            }
            else {
                echoError( lang( "exUnknowCmd" ) );
            }
        }



    }
}

