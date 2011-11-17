/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Interface;
using wojilu.Apps.Forum.Service;
using wojilu.Common.Msg.Service;
using wojilu.Common.Msg.Interface;
using wojilu.Common.Money.Domain;
using wojilu.Web.Controller.Forum.Utils;
using wojilu.Web.Controller.Common;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Forum.Moderators {

    [App( typeof( ForumApp ) )]
    public class TopicSaveController : ControllerBase {

        private String idList;
        private String condition;

        public override void CheckPermission() {

            this.idList = ctx.GetIdList( "ids" );
            this.condition = "Id in (" + idList + ")";

            if ("up".Equals( ctx.Post( "cmd" ) ) || "down".Equals( ctx.Post( "cmd" ) )) return;

            if (strUtil.IsNullOrEmpty( idList )) echoToParent( lang( "plsSelect" ) );
        }


        [HttpPost, DbTransaction]
        public void Sticky() {
            int id = ctx.GetInt( "boardId" );
            topicService.AdminUpdate( "set Status=" + TopicStatus.Sticky, condition );
            topicService.AddAuthorIncome( condition, UserAction.Forum_TopicSticky.Id );
            log( condition, ForumLogAction.Sticky );
            echoToParent( lang( "opok" ) );
        }

        [HttpPost, DbTransaction]
        public void StickyUndo() {
            int id = ctx.GetInt( "boardId" );
            topicService.AdminUpdate( "set Status=" + TopicStatus.Normal, condition );
            topicService.SubstractAuthorIncome( condition, UserAction.Forum_TopicSticky.Id );
            log( condition, ForumLogAction.UnSticky );
            echoAjaxOk();
        }

        [HttpPost, DbTransaction]
        public void GlobalSticky() {
            int id = ctx.GetInt( "boardId" );
            topicService.SetGlobalSticky( ctx.app.Id, idList );
            log( idList, ForumLogAction.GlobalSticky );
            echoToParent( lang( "opok" ) );
        }

        [HttpPost, DbTransaction]
        public void GlobalStickyUndo() {
            int id = ctx.GetInt( "boardId" );
            topicService.SetGloablStickyUndo( ctx.app.Id, idList );
            log( idList, ForumLogAction.GlobalUnSticky );
            echoAjaxOk();
        }

        [HttpPost, DbTransaction]
        public void Pick() {
            int id = ctx.GetInt( "boardId" );
            topicService.AdminUpdate( "set IsPicked=1", condition );
            topicService.AddAuthorIncome( condition, UserAction.Forum_TopicPicked.Id );
            log( condition, ForumLogAction.Pick );
            echoToParent( lang( "opok" ) );
        }

        [HttpPost, DbTransaction]
        public void PickedUndo() {
            int id = ctx.GetInt( "boardId" );
            topicService.AdminUpdate( "set IsPicked=0", condition );
            topicService.SubstractAuthorIncome( condition, UserAction.Forum_TopicPicked.Id );
            log( condition, ForumLogAction.UnPick );
            echoAjaxOk();
        }

        [HttpPost, DbTransaction]
        public void Highlight() {
            int id = ctx.GetInt( "boardId" );
            String action = string.Format( "set TitleStyle='{0}'", strUtil.SqlClean( FormController.GetTitleStyle( ctx ), 150 ) );
            ForumTopic.updateBatch( action, "Id in (" + idList + ")" );
            log( idList, ForumLogAction.Highlight );
            echoToParent( lang( "opok" ) );
        }

        [HttpPost, DbTransaction]
        public void HighlightUndo() {
            int id = ctx.GetInt( "boardId" );
            String action = "set TitleStyle=''";
            ForumTopic.updateBatch( action, "Id in (" + idList + ")" );
            log( idList, ForumLogAction.UnHighlight );
            echoAjaxOk();
        }


        [HttpPost, DbTransaction]
        public void Lock() {
            int id = ctx.GetInt( "boardId" );
            topicService.AdminUpdate( "set IsLocked=1", condition );
            topicService.SubstractAuthorIncome( condition, UserAction.Forum_TopicLocked.Id );
            log( condition, ForumLogAction.Lock );
            echoToParent( lang( "opok" ) );
        }

        [HttpPost, DbTransaction]
        public void LockUndo() {
            int id = ctx.GetInt( "boardId" );
            topicService.AdminUpdate( "set IsLocked=0", condition );
            topicService.AddAuthorIncome( condition, UserAction.Forum_TopicLocked.Id );
            log( condition, ForumLogAction.UnLock );
            echoAjaxOk();
        }

        [HttpPost, DbTransaction]
        public void Delete() {
            int id = ctx.GetInt( "boardId" );
            topicService.DeleteListToTrash( idList );
            topicService.SubstractAuthorIncome( condition, UserAction.Forum_TopicDeleted.Id );
            log( idList, ForumLogAction.Delete );
            echoToParent( lang( "opok" ) );
        }

        [HttpPost, DbTransaction]
        public void Move() {
            int id = ctx.GetInt( "boardId" );
            int targetForumId = ctx.PostInt( "targetForum" );
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

            topicService.Move( targetBoard.Id, idList );
            log( idList, ForumLogAction.MoveTopic );
            echoToParent( lang( "opok" ) );
        }


        [HttpPost, DbTransaction]
        public void SaveStickySort() {

            int id = ctx.GetInt( "boardId" );
            ForumBoard bd = boardService.GetById( id, ctx.owner.obj );

            int topicId = ctx.PostInt( "id" );
            String cmd = ctx.Post( "cmd" );
            if (cmd == "up") {
                topicService.StickyMoveUp( topicId );
                echoRedirect( "ok" );
            }
            else if (cmd == "down") {
                topicService.StickyMoveDown( topicId );
                echoRedirect( "ok" );
            }
            else {
                errors.Add( lang( "exUnknowCmd" ) );
                echoError();
            }
        }


        [HttpPost, DbTransaction]
        public void SaveGlobalStickySort() {

            int topicId = ctx.PostInt( "id" );
            String cmd = ctx.Post( "cmd" );

            ForumApp app = ctx.app.obj as ForumApp;

            if (cmd == "up") {

                forumService.StickyMoveUp( app, topicId );
                echoRedirect( "ok" );
            }
            else if (cmd == "down") {
                forumService.StickyMoveDown( app, topicId );
                echoRedirect( "ok" );
            }
            else {
                errors.Add( lang( "exUnknowCmd" ) );
                echoError();
            }
        }

        [HttpPost, DbTransaction]
        public void Category() {

            int id = ctx.GetInt( "boardId" );
            int categoryId = ctx.PostInt( "dropCategories" );
            ForumCategory category = categoryService.GetById( categoryId, ctx.owner.obj );
            if (category == null && categoryId > 0) {
                echoText( "<h1>" + alang( "exCategoryNotFound" ) + "</h4>" );
                return;
            }

            String action = string.Format( "set CategoryId=" + categoryId );
            ForumTopic.updateBatch( action, "Id in (" + idList + ")" );
            log( idList, ForumLogAction.SetCategory );
            echoToParent( lang( "opok" ) );
        }



        //-----------------------------------------------------------------------------------------------------

        public IForumTopicService topicService { get; set; }
        public IForumBoardService boardService { get; set; }
        public IForumCategoryService categoryService { get; set; }
        public IForumLogService logService { get; set; }
        public IMessageService msgService { get; set; }
        public IForumService forumService { get; set; }

        public TopicSaveController() {
            boardService = new ForumBoardService();
            topicService = new ForumTopicService();
            categoryService = new ForumCategoryService();
            logService = new ForumLogService();
            msgService = new MessageService();
            forumService = new ForumService();
        }

        private void log( String idList, int actionId ) {

            String reason = getReason();

            List<ForumTopic> topics = topicService.GetByIds( idList );

            foreach (ForumTopic topic in topics) {

                logService.AddTopic( (User)ctx.viewer.obj, ctx.app.Id, topic.Id, actionId, reason, ctx.Ip );

                // 发送短信通知
                if (ctx.PostIsCheck( "IsSendMsg" ) != 1) continue;

                String msg = ForumLogAction.GetLable( actionId );
                String title = string.Format( alang( "adminPostMsgTitle" ), topic.Title, msg );

                String topicInfo = "<a href='" + alink.ToAppData( topic ) + "'>" + topic.Title + "</a>";
                if (actionId == ForumLogAction.Delete) topicInfo = topic.Title;

                String body = string.Format( alang( "adminPostMsgBody" ), topicInfo, msg, DateTime.Now, reason );

                msgService.SiteSend( title, body, topic.Creator );
            }

        }


        private String getReason() {
            return ctx.PostIsCheck( "chkReason" ) == 1 ? ctx.Post( "reasonText" ) : ctx.Post( "reasonSelect" );
        }

    }

}
