/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Service;

using wojilu.Members.Users.Domain;
using wojilu.Common.Security;
using wojilu.Common.Security.Utils;
using wojilu.Apps.Forum.Interface;
using wojilu.Web.Controller.Forum.Utils;

namespace wojilu.Web.Controller.Forum.Admin {

    [App( typeof( ForumApp ) )]
    public class SecurityController : ControllerBase {

        public IForumBoardService boardService { get; set; }
        public IForumLogService logService { get; set; }

        public SecurityController() {
            boardService = new ForumBoardService();
            logService = new ForumLogService();
        }

        public void Setting() {
            target( SavePermissionAll );
            set( "lnkApplyAll", to( ApplyAll ) );
            ForumApp f = ctx.app.obj as ForumApp;
            bindRoleActions( f );
        }

        public void BoardSetting( int id ) {
            target( SaveBoardPermissionAll, id );
            ForumBoard board = boardService.GetById( id, ctx.owner.obj );
            if (board == null) { echoRedirect( alang( "exBoardNotFound" ) ); return; }

            set( "forumBoard.Name", board.Name );
            set( "resetLink", to( BoardReset, id ) );
            bindRoleActions( board );
        }

        private void bindRoleActions( ISecurity f ) {

            IBlock sblock = getBlock( "sysroles" );

            SecurityTool tool = SecurityHelper.GetSecurityTool( f, ctx );

            IList actionList = tool.GetActionAll();
            bindList( "actions", "a", actionList );

            IList allRoles = tool.GetRoles();
            String lastRoleType = null;

            foreach (IRole role in allRoles) {

                sblock.Set( "role.Name", role.Name );


                bindCheckBoxList( tool, sblock, role );

                if (role.Role.GetType().FullName.Equals( lastRoleType ) == false) {
                    sblock.Set( "seperator", "<tr><td colspan=" + (actionList.Count + 1) + ">&nbsp;</td></tr>" );
                    lastRoleType = role.Role.GetType().FullName;
                }
                else {
                    sblock.Set( "seperator", "" );
                }

                sblock.Next();
            }
        }

        private static void bindCheckBoxList( SecurityTool tool, IBlock sblock, IRole role ) {
            IBlock cbBlock = sblock.GetBlock( "checkboxs" );
            IList checkValues = tool.GetCheckBoxList( role );
            foreach (ActionVo av in checkValues) {
                cbBlock.Set( "a.Name", av.Name );
                cbBlock.Set( "a.Value", av.Value );
                cbBlock.Set( "a.Checked", av.Checked );
                cbBlock.Next();
            }
        }

        [HttpPost, DbTransaction]
        public void SavePermissionAll() {

            string[] actionIds = ctx.web.postValuesByKey( typeof( SecurityAction ).Name );
            ForumApp f = ctx.app.obj as ForumApp;

            SecurityTool tool = SecurityHelper.GetSecurityTool( f, ctx );

            tool.SaveActionAll( actionIds );

            if ("true".Equals( ctx.Get( "applyAll" ) )) {
                boardService.UpdateSecurityAll( ctx.app.obj as ForumApp );
            }

            echoRedirectPart( lang( "opok" ) );
        }

        [HttpPost, DbTransaction]
        public void SaveBoardPermissionAll( int id ) {

            ForumBoard board = boardService.GetById( id, ctx.owner.obj );
            if (board == null) {
                echoRedirect( alang( "exBoardNotFound" ) );
                return;
            }

            string[] actionIds = ctx.web.postValuesByKey( typeof( SecurityAction ).Name );


            SecurityTool tool = SecurityHelper.GetSecurityTool( board, ctx );
            tool.SaveActionAll( actionIds );

            echoRedirectPart( lang( "opok" ) );
        }

        [HttpPost, DbTransaction]
        public void ApplyAll() {

            boardService.UpdateSecurityAll( ctx.app.obj as ForumApp );

            echoRedirectPart( lang( "opok" ) );
        }

        [HttpPost, DbTransaction]
        public void BoardReset( int id ) {
            ForumBoard board = boardService.GetById( id, ctx.owner.obj );
            if (board == null) { echoRedirect( alang( "exBoardNotFound" ) ); return; }

            ForumApp f = ctx.app.obj as ForumApp;
            board.Security = f.Security;
            db.update( board, "Security" );
            echoRedirectPart( lang( "opok" ) );
        }

        public void Log() {
            DataPage<ForumLog> list = logService.FindPage( ctx.app.Id );
            bindList( "list", "forumlog", list.Results );
            set( "page", list.PageBar );
        }

    }
}

