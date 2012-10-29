/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Service;

using wojilu.Members.Users.Service;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Forum.Interface;
using wojilu.Members.Users.Interface;

namespace wojilu.Web.Controller.Forum.Admin {


    [App( typeof( ForumApp ) )]
    public class ModeratorController : ControllerBase {

        public IForumBoardService boardService { get; set; }
        public IForumService forumService { get; set; }
        public IModeratorService moderatorService { get; set; }
        public IUserService userService { get; set; }

        public ModeratorController() {
            forumService = new ForumService();
            boardService = new ForumBoardService();
            userService = new UserService();
            moderatorService = new ModeratorService();
        }

        public void List( int boardId ) {

            ForumBoard board = boardService.GetById( boardId, ctx.owner.obj );
            if (board == null) {
                echoRedirect( alang( "exBoardNotFound" ) );
                return;
            }
            IList moderatorList = moderatorService.GetModeratorList( board );
            String deleteUrl = to( Delete, board.Id );
            bindModerators( moderatorList, deleteUrl );
            target( Create  );
            set( "BoardId", boardId );
            set( "forumBoard.Name", board.Name );
        }

        private void bindModerators( IList moderatorList, String deleteUrl ) {
            IBlock block = getBlock( "list" );
            foreach (User member in moderatorList) {
                block.Set( "m.Name", member.Name );
                block.Set( "m.Url", toUser( member) );
                block.Set( "m.DeleteUrl", deleteUrl + "?m=" + member.Id );
                block.Next();
            }
        }

        [HttpPost, DbTransaction]
        public void Create() {

            int id = ctx.PostInt( "BoardId" );
            ForumBoard board = boardService.GetById( id, ctx.owner.obj );
            if (board == null) {
                echoRedirect( alang( "exBoardNotFound" ) );
                return;
            }

            ctx.SetItem( "boardId", id );

            String target = ctx.Post("Name");
            if (strUtil.IsNullOrEmpty( target )) {
                errors.Add( lang( "exUserName" ) );
                run( List, id );
                return;
            }

            User user = userService.GetByName( target.Trim() );
            if (user == null) {
                errors.Add( lang( "exUser" ) );
                run( List, id );
                return;
            }

            moderatorService.AddModerator( board, user.Name );
            redirect( List, id );
        }

        [HttpDelete, DbTransaction]
        public void Delete( int id ) {

            ForumBoard board = boardService.GetById( id, ctx.owner.obj );
            if (board == null) {
                echoRedirect( alang( "exBoardNotFound" ) );
                return;
            }

            ctx.SetItem( "boardId", id );

            int userId = ctx.GetInt( "m" );
            if (userId <= 0) {
                echoRedirect( lang( "exUser" ) );
                return;
            }

            User user = userService.GetById( userId );
            if (user == null) {
                echoRedirect( lang( "exUser" ) );
                return;
            }

            if (!moderatorService.IsModerator( board, user.Name )) {
                echoRedirect( alang( "exUserNotModerator" ) );
                return;
            }

            moderatorService.DeleteModerator( board, user.Name );

            redirect( List, id );
        }




    }
}

