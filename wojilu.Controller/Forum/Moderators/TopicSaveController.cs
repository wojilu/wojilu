/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Interface;
using wojilu.Apps.Forum.Service;
using wojilu.Web.Controller.Common;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Forum.Moderators {

    [App( typeof( ForumApp ) )]
    public class TopicSaveController : ControllerBase {

        public virtual IForumTopicService topicService { get; set; }
        public virtual IForumBoardService boardService { get; set; }
        public virtual IForumCategoryService categoryService { get; set; }
        public virtual IForumLogService logService { get; set; }
        public virtual IForumService forumService { get; set; }

        public TopicSaveController() {
            boardService = new ForumBoardService();
            topicService = new ForumTopicService();
            categoryService = new ForumCategoryService();
            logService = new ForumLogService();
            forumService = new ForumService();
        }

        private String idList;
        private String condition;

        public override void CheckPermission() {

            this.idList = ctx.GetIdList( "ids" );
            this.condition = "Id in (" + idList + ")";

            if ("up".Equals( ctx.Post( "cmd" ) ) || "down".Equals( ctx.Post( "cmd" ) )) return;

            if (strUtil.IsNullOrEmpty( idList )) echoToParent( lang( "plsSelect" ) );
        }

        [HttpPost, DbTransaction]
        public virtual void Sticky() {
            topicService.MakeSticky( av() );
            echoToParent( lang( "opok" ) );
        }

        [HttpPost, DbTransaction]
        public virtual void StickyUndo() {
            topicService.MakeStickyUndo( av() );
            echoAjaxOk();
        }

        [HttpPost, DbTransaction]
        public virtual void GlobalSticky() {
            topicService.MakeGlobalSticky( av() );
            echoToParent( lang( "opok" ) );
        }

        [HttpPost, DbTransaction]
        public virtual void GlobalStickyUndo() {
            topicService.MakeGloablStickyUndo( av() );
            echoAjaxOk();
        }

        [HttpPost, DbTransaction]
        public virtual void Pick() {
            topicService.MakePick( av() );
            echoToParent( lang( "opok" ) );
        }

        [HttpPost, DbTransaction]
        public virtual void PickedUndo() {
            topicService.MakePickUndo( av() );
            echoAjaxOk();
        }

        [HttpPost, DbTransaction]
        public virtual void Highlight() {
            topicService.MakeHighlight( strUtil.SqlClean( FormController.GetTitleStyle( ctx ), 150 ), av() );
            echoToParent( lang( "opok" ) );
        }

        [HttpPost, DbTransaction]
        public virtual void HighlightUndo() {
            topicService.MakeHighlightUndo( av() );
            echoAjaxOk();
        }

        [HttpPost, DbTransaction]
        public virtual void Lock() {
            topicService.MakeLock( av() );
            echoToParent( lang( "opok" ) );
        }

        [HttpPost, DbTransaction]
        public virtual void LockUndo() {
            topicService.MakeLockUndo( av() );
            echoAjaxOk();
        }

        [HttpPost, DbTransaction]
        public virtual void Delete() {
            topicService.DeleteList( av() );
            echoToParent( lang( "opok" ) );
        }

        [HttpPost, DbTransaction]
        public virtual void Move() {
            long targetForumId = ctx.PostLong( "targetForum" );
            ForumBoard targetBoard = boardService.GetById( targetForumId, ctx.owner.obj );
            ctx.SetItem( "targetForumId", targetForumId );

            if (targetBoard == null) {
                errors.Add( alang( "exBoardNotFound" ) );
                return;
            }

            if (targetBoard.IsCategory == 1) {
                errors.Add( alang( "exTargetCantCategory" ) );
                return;
            }

            topicService.MakeMove( targetBoard.Id, av() );
            echoToParent( lang( "opok" ) );
        }

        [HttpPost, DbTransaction]
        public virtual void SaveStickySort() {

            long topicId = ctx.PostLong( "id" );
            String cmd = ctx.Post( "cmd" );
            if (cmd == "up") {
                topicService.StickyMoveUp( topicId );
                echoJsonOk();
            }
            else if (cmd == "down") {
                topicService.StickyMoveDown( topicId );
                echoJsonOk();
            }
            else {
                errors.Add( lang( "exUnknowCmd" ) );
                echoError();
            }
        }


        [HttpPost, DbTransaction]
        public virtual void SaveGlobalStickySort() {

            long topicId = ctx.PostLong( "id" );
            String cmd = ctx.Post( "cmd" );
            ForumApp app = ctx.app.obj as ForumApp;

            if (cmd == "up") {

                forumService.StickyMoveUp( app, topicId );
                echoJsonOk();
            }
            else if (cmd == "down") {
                forumService.StickyMoveDown( app, topicId );
                echoJsonOk();
            }
            else {
                errors.Add( lang( "exUnknowCmd" ) );
                echoError();
            }
        }

        [HttpPost, DbTransaction]
        public virtual void Category() {

            long categoryId = ctx.PostLong( "dropCategories" );
            ForumCategory category = categoryService.GetById( categoryId, ctx.owner.obj );
            if (category == null && categoryId > 0) {
                echoText( "<h1>" + alang( "exCategoryNotFound" ) + "</h4>" );
                return;
            }

            topicService.MakeCategory( categoryId, av() );
            echoToParent( lang( "opok" ) );
        }

        //-----------------------------------------------------------------------------------------------------


        private AdminValue av() {

            AdminValue v = new AdminValue();

            v.Ids = idList;
            v.AppId = ctx.app.Id;

            v.User = (User)ctx.viewer.obj;
            v.Reason = ctx.PostIsCheck( "chkReason" ) == 1 ? ctx.Post( "reasonText" ) : ctx.Post( "reasonSelect" );

            v.Ip = ctx.Ip;
            v.IsSendMsg = ctx.PostIsCheck( "IsSendMsg" ) == 1;

            return v;
        }

    }

}
