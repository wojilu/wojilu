using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Interface;
using wojilu.Apps.Forum.Service;
using wojilu.Common.Msg.Service;
using wojilu.Common.Msg.Interface;
using wojilu.Common.Money.Domain;
using wojilu.Web.Controller.Forum.Utils;
using wojilu.Members.Users.Domain;
using wojilu.Web.Controller.Common;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Controller.Forum.Moderators {

    [App( typeof( ForumApp ) )]
    public class AdminSaveController : ControllerBase {

        private String idList;
        private String condition;

        public override void CheckPermission() {

            this.idList = ctx.GetIdList( "ids" );
            this.condition = "Id in (" + idList + ")";
            if (strUtil.IsNullOrEmpty( idList )) echoToParent( lang( "plsSelect" ) );
        }


        [HttpPost, DbTransaction]
        public void Sticky( int id ) {
            topicService.AdminUpdate( "set Status=" + TopicStatus.Sticky, condition );
            topicService.AddAuthorIncome( condition, UserAction.Forum_TopicSticky.Id );
            log( condition, ForumLogAction.Sticky );
        }

        [HttpPost, DbTransaction]
        public void StickyUndo( int id ) {
            topicService.AdminUpdate( "set Status=" + TopicStatus.Normal, condition );
            topicService.SubstractAuthorIncome( condition, UserAction.Forum_TopicSticky.Id );
            log( condition, ForumLogAction.UnSticky );
        }

        [HttpPost, DbTransaction]
        public void GlobalSticky( int id ) {
            topicService.SetGlobalSticky( ctx.app.Id, idList );
            log( idList, ForumLogAction.GlobalSticky );
        }

        [HttpPost, DbTransaction]
        public void GlobalStickyUndo( int id ) {
            topicService.SetGloablStickyUndo( ctx.app.Id, idList );
            log( idList, ForumLogAction.GlobalUnSticky );
        }

        [HttpPost, DbTransaction]
        public void Pick( int id ) {
            topicService.AdminUpdate( "set IsPicked=1", condition );
            topicService.AddAuthorIncome( condition, UserAction.Forum_TopicPicked.Id );
            log( condition, ForumLogAction.Pick );
        }

        [HttpPost, DbTransaction]
        public void PickedUndo( int id ) {
            topicService.AdminUpdate( "set IsPicked=0", condition );
            topicService.SubstractAuthorIncome( condition, UserAction.Forum_TopicPicked.Id );
            log( condition, ForumLogAction.UnPick );
        }


        [HttpPost, DbTransaction]
        public void Highlight( int id ) {
            String action = string.Format( "set TitleStyle='{0}'", strUtil.SqlClean( FormController.GetTitleStyle( ctx ), 150 ) );
            ForumTopic.updateBatch( action, "Id in (" + idList + ")" );
            log( idList, ForumLogAction.Highlight );
        }

        [HttpPost, DbTransaction]
        public void HighlightUndo( int id ) {
            String action = "set TitleStyle=''";
            ForumTopic.updateBatch( action, "Id in (" + idList + ")" );
            log( idList, ForumLogAction.UnHighlight );
            echoToParent( lang( "opok" ) );
        }

        [HttpPost, DbTransaction]
        public void Lock( int id ) {
            topicService.AdminUpdate( "set IsLocked=1", condition );
            topicService.SubstractAuthorIncome( condition, UserAction.Forum_TopicLocked.Id );
            log( condition, ForumLogAction.Lock );
        }

        [HttpPost, DbTransaction]
        public void LockUndo( int id ) {
            topicService.AdminUpdate( "set IsLocked=0", condition );
            topicService.AddAuthorIncome( condition, UserAction.Forum_TopicLocked.Id );
            log( condition, ForumLogAction.UnLock );
        }

        [HttpPost, DbTransaction]
        public void Delete( int id ) {
            topicService.DeleteListToTrash( idList );
            topicService.SubstractAuthorIncome( condition, UserAction.Forum_TopicDeleted.Id );
            log( idList, ForumLogAction.Delete );
        }

        [HttpPost, DbTransaction]
        public void Move( int id ) {
            ForumBoard targetBoard = boardService.GetById( ctx.PostInt( "targetForum" ), ctx.owner.obj );

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
        }


        [HttpPost, DbTransaction]
        public void SaveStickySort( int id ) {

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

            new ForumCacheRemove( this.boardService, this ).SortTopic( bd );
        }


        [HttpPost, DbTransaction]
        public void SaveGlobalStickySort() {

            int topicId = ctx.PostInt( "id" );
            String cmd = ctx.Post( "cmd" );

            ForumApp app = ctx.app.obj as ForumApp;
            PermissionUtil.Check( this, app );

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
        public void Category( int id ) {

            int categoryId = ctx.PostInt( "dropCategories" );
            ForumCategory category = categoryService.GetById( categoryId, ctx.owner.obj );
            if (category == null && categoryId > 0) {
                echoText( "<h1>" + alang( "exCategoryNotFound" ) + "</h4>" );
                return;
            }

            String action = string.Format( "set CategoryId=" + categoryId );
            ForumTopic.updateBatch( action, "Id in (" + idList + ")" );
            log( idList, ForumLogAction.SetCategory );
        }

        [HttpPost, DbTransaction]
        public void Move() {

        }

        //-----------------------------------------------------------------------------------------------------

        public IForumTopicService topicService { get; set; }
        public IForumBoardService boardService { get; set; }
        public IForumCategoryService categoryService { get; set; }
        public IForumLogService logService { get; set; }
        public IMessageService msgService { get; set; }
        public IForumService forumService { get; set; }

        public AdminSaveController() {
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


                String body = string.Format( alang( "adminPostMsgBody" ), "<a href='" + alink.ToAppData( topic ) + "'>" + topic.Title + "</a>",
                    msg, DateTime.Now, reason
                    );

                msgService.SiteSend( title, body, topic.Creator );
            }

            echoToParent( lang( "opok" ) );
        }


        private String getReason() {
            return ctx.PostIsCheck( "chkReason" ) == 1 ? ctx.Post( "reasonText" ) : ctx.Post( "reasonSelect" );
        }

    }

}
