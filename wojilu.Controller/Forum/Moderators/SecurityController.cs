using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Controller.Forum.Utils;
using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Interface;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Forum.Service;

using System.Collections;
using System.Reflection;

using wojilu.Web.Mvc;
using wojilu.Web.Context;

using wojilu.Members.Users.Domain;
using wojilu.Members.Sites.Domain;
using wojilu.Common.Security;
using wojilu.Common.AppBase;
using wojilu.Common.Security.Utils;
using wojilu.Common.AppBase.Interface;


namespace wojilu.Web.Controller.Forum.Moderators {

    /// <summary>
    /// 本目录下权限受配置决定，不再硬编码限制。
    /// </summary>
    [App( typeof( ForumApp ) )]
    public class SecurityController : ControllerBase {

        public IForumBoardService boardService { get; set; }
        public IModeratorService moderatorService { get; set; }

        public SecurityController() {
            boardService = new ForumBoardService();
            moderatorService = new ModeratorService();
        }

        public override void CheckPermission() {

            // 1) login
            if (ctx.viewer.IsLogin == false) {
                redirectLogin();
                return;
            }

            int boardId = ctx.GetInt( "boardId" );
            ForumBoard board = boardService.GetById( boardId, ctx.owner.obj );

            if (board == null) {
                echoRedirect( lang( "exDataNotFound" ) + "(ForumBoard)" );
                return;
            }

            SecurityHelper.Check( this, board );


        }



    }

}
