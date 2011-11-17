using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Interface;
using wojilu.Apps.Forum.Service;
using wojilu.Web.Controller.Forum.Utils;

namespace wojilu.Web.Controller.Forum.Users {

    public class SecurityController : ControllerBase {

        public IForumBoardService boardService { get; set; }

        public SecurityController() {
            boardService = new ForumBoardService();
        }

        public override void CheckPermission() {

            if (ctx.viewer.IsLogin == false) {
                if (ctx.utils.isAjax) {
                    echoText( lang("exPlsLogin") );
                }
                else {
                    redirectLogin();
                }
                return;
            }

            int boardId = ctx.GetInt( "boardId" );
            ForumBoard board = boardService.GetById( boardId, ctx.owner.obj );
            if (board == null) {
                echo( "版块不存在" );
                return;
            }
            ctx.SetItem( "forumBoard", board );

            if (ctx.viewer.IsAdministrator()) return;

            SecurityHelper.Check( this, board );


        }

    }

}
