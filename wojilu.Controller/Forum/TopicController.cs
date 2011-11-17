/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Members.Users.Domain;

using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Service;
using wojilu.Apps.Forum.Interface;

using wojilu.Common.Security;
using wojilu.Common.Money.Domain;
using wojilu.Common.Money.Service;
using wojilu.Common.Money.Interface;

using wojilu.Web.Controller.Forum.Utils;
using wojilu.Web.Controller.Common;

namespace wojilu.Web.Controller.Forum {


    [App( typeof( ForumApp ) )]
    public class TopicController : ControllerBase {

        public IForumBoardService boardService { get; set; }
        public IForumTopicService topicService { get; set; }
        public IForumPostService postService { get; set; }
        public IUserIncomeService incomeService { get; set; }
        public IAttachmentService attachService { get; set; }

        public TopicController() {
            boardService = new ForumBoardService();
            topicService = new ForumTopicService();
            postService = new ForumPostService();
            incomeService = new UserIncomeService();
            attachService = new AttachmentService();
        }

        public override void CheckPermission() {

            ForumTopic topic = topicService.GetById( ctx.route.id, ctx.owner.obj );
            if (topic == null) {
                echo( alang( "exTopicNotFound" ) );
                return;
            }

            // 判断阅读权限
            if (topic.ReadPermission > 0 && haveReadPermission( topic ) == false) {
                echo( alang( "exReadPermission" ) );
                return;
            }

            ForumBoard board = topic.ForumBoard;

            SecurityHelper.Check( this, board );
        }

        private Tree<ForumBoard> _tree;

        private Tree<ForumBoard> getTree() {
            if (_tree == null) _tree = new Tree<ForumBoard>( boardService.GetBoardAll( ctx.app.Id, ctx.viewer.IsLogin ) );
            return _tree;
        }

        public void Show( int id ) {


            ForumTopic topic = topicService.GetById( id, ctx.owner.obj );
            ForumBoard board = topic.ForumBoard;

            set( "isTopicLocked", topic.IsLocked == 1 ? "true" : "false" );


            topicService.AddHits( topic );

            int userId = ctx.GetInt( "userId" );
            bindPosts( id, topic, board, userId );
        }

        private void bindPosts( int id, ForumTopic topic, ForumBoard board, int userId ) {
            DataPage<ForumPost> list = postService.GetPageList( id, getPageSize( ctx.app.obj ), userId );

            ForumPost lastPost = getLastPost( list, topic.Id );

            List<Attachment> attachments = attachService.GetByTopic( list.Results );
            UserIncomeUtil.AddIncomeListToUser( list.Results );

            forwardPreNext( topic );

            WebUtils.pageTitle( this, topic.Title, board.Name );
            Page.Keywords = topic.Tag.TextString;

            List<ForumBoard> pathboards = getTree().GetPath( board.Id );
            set( "location", ForumLocationUtil.GetTopic( pathboards, topic, ctx ) );

            set( "newReplyUrl", Link.To( new Users.PostController().ReplyTopic, id ) + "?boardId=" + topic.ForumBoard.Id );
            set( "newTopicUrl", Link.To( new Users.TopicController().NewTopic ) + "?boardId=" + topic.ForumBoard.Id );

            ctx.SetItem( "forumTopic", topic );
            ctx.SetItem( "forumBoard", board );
            ctx.SetItem( "posts", list.Results );
            ctx.SetItem( "attachs", attachments );
            ctx.SetItem( "pageSize", getPageSize( ctx.app.obj ) );

            load( "postBlock", new PostBlockController().Show );

            bindForm( topic, board, lastPost );

            list.RecordCount++;

            String pager = list.PageCount > 1 ? list.PageBar : "";
            set( "page", pager );
        }

        private ForumPost getLastPost( DataPage<ForumPost> list, int topicId ) {
            ForumPost lastPost;
            if (list.Current == list.PageCount) {
                lastPost = list.Results[list.Results.Count - 1];
            }
            else {
                lastPost = postService.GetLastPostByTopic( topicId );
            }
            return lastPost;
        }

        private Boolean haveReadPermission( ForumTopic topic ) {

            if (ctx.viewer.IsLogin == false) return false;
            if (ctx.viewer.Id == topic.Creator.Id) return true;
            int permission = incomeService.GetUserIncome( ctx.viewer.Id, Currency.ReadPermission().Id ).Income;
            return permission >= topic.ReadPermission;
        }

        //-----------------------------------------------------------------------------------------------





        private void bindForm( ForumTopic topic, ISecurity board, ForumPost lastPost ) {
            IBlock formBlock = getBlock( "form" );
            //if (topic.IsLocked == 1) return;
            //ISecurityAction replyAction = ForumAction.Get( new PostController().ReplyPost, ctx.route.getRootNamespace() );
            //if (PermissionUtil.HasAction( (User)ctx.viewer.obj, board, replyAction, ctx ))
            bindFormNew( topic, lastPost, formBlock );
        }

        private void bindFormNew( ForumTopic topic, ForumPost lastPost, IBlock formBlock ) {


            User user = ctx.viewer.obj as User;
            if (strUtil.HasText( user.Pic )) {
                formBlock.Set( "currentUser", "<img src=\"" + user.PicMedium + "\"/>" );
            }
            else {
                formBlock.Set( "currentUser", user.Name );
            }

            formBlock.Set( "post.ReplyActionUrl", Link.To( new Users.PostController().Create ) + "?boardId=" + topic.ForumBoard.Id );
            formBlock.Set( "post.ReplyTitle", "re:" + topic.Title );
            formBlock.Set( "post.TopicId", topic.Id );
            formBlock.Set( "post.ParentId", lastPost.Id );
            formBlock.Set( "forumBoard.Id", topic.ForumBoard.Id );

            Editor ed = Editor.NewOne( "Content", "", "150px", sys.Path.Editor, MvcConfig.Instance.JsVersion, Editor.ToolbarType.Basic );
            ed.AddUploadUrl( ctx );

            formBlock.Set( "Editor", ed );
            formBlock.Next();
        }

        private void forwardPreNext( ForumTopic topic ) {
            if (ctx.url.Query.Equals( "?next" )) {
                forwardToPre( topic );
            }
            else if (ctx.url.Query.Equals( "?pre" )) {
                forwardToNext( topic );
            }
        }

        private void forwardToNext( ForumTopic topic ) {
            ForumTopic pre = topicService.GetPre( topic );
            if (pre == null) {
                echoRedirect( alang( "exFirstTopic" ) );
            }
            else {
                redirect( Show, pre.Id );
            }
        }

        private void forwardToPre( ForumTopic topic ) {
            ForumTopic next = topicService.GetNext( topic );
            if (next == null) {
                echoRedirect( alang( "exLastTopic" ) );
            }
            else {
                redirect( Show, next.Id );
            }
        }

        private int getPageSize( object app ) {
            return ((ForumApp)app).GetSettingsObj().ReplySize;
        }


    }
}

