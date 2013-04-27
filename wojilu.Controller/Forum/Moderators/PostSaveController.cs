/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Service;
using wojilu.Web.Controller.Forum.Utils;
using wojilu.Common.Money.Service;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Forum.Interface;
using wojilu.Common.Money.Interface;
using wojilu.Common.Money.Domain;

namespace wojilu.Web.Controller.Forum.Moderators {

    [App( typeof( ForumApp ) )]
    public partial class PostSaveController : ControllerBase {


        public IForumBoardService boardService { get; set; }
        public IForumPostService postService { get; set; }
        public IForumTopicService topicService { get; set; }
        public IUserIncomeService userIncomeService { get; set; }

        public PostSaveController() {
            topicService = new ForumTopicService();
            boardService = new ForumBoardService();
            postService = new ForumPostService();
            userIncomeService = new UserIncomeService();
        }

        private Boolean checkCreatorPermission( ForumTopic topic ) {
            if (topic.Creator.Id != ctx.viewer.Id) {
                echoText( alang( "exRewardSelfOnly" ) );
                return false;
            }
            return true;
        }


        //------------------------------------ 版主管理：帖子评分 -----------------------------------------

        public void SaveCredit( int id ) {

            ForumPost post = postService.GetById( id, ctx.owner.obj );
            if (post == null) {
                echoRedirect( alang( "exPostNotFound" ) );
                return;
            }

            if (boardError( post )) return;

            int rateMaxValue = ((ForumApp)ctx.app.obj).MaxRateValue;

            int currencyId = ctx.PostInt( "CurrencyId" );
            int currencyValue = ctx.PostInt( "CurrencyValue" );
            String reason = ctx.Post( "Reason" );
            User user = (User)ctx.viewer.obj;

            if (currencyValue != 0 && currencyValue >= -rateMaxValue && currencyValue <= rateMaxValue) {
                postService.SetPostCredit( post, currencyId, currencyValue, reason, user );
                echoToParent( lang( "opok" ) );
            }
            else {
                errors.Add( alang( "exCreditNotValid" ) );
                echoError();
            }
        }

        //------------------------------------ admin -----------------------------------------

        [HttpPut, DbTransaction]
        public void Ban( int id ) {

            ForumPost post = postService.GetById( id, ctx.owner.obj );
            if (post == null) {
                content( alang( "exPostNotFound" ) );
                return;
            }
            if (boardError( post )) return;

            postService.BanPost( post, ctx.Post( "Reason" ), ctx.PostIsCheck( "IsSendMsg" ), (User)ctx.viewer.obj, ctx.app.Id, ctx.Ip );
            echoRedirect( lang( "opok" ) );
        }

        [HttpPut, DbTransaction]
        public void UnBan( int id ) {

            ForumPost post = postService.GetById( id, ctx.owner.obj );
            if (post == null) {
                content( alang( "exPostNotFound" ) );
                return;
            }
            if (boardError( post )) return;

            postService.UnBanPost( post, (User)ctx.viewer.obj, ctx.app.Id, ctx.Ip );
            echoRedirect( lang( "opok" ) );
        }

        [HttpPut, DbTransaction]
        public void Lock( int id ) {

            ForumTopic topic = topicService.GetById( id, ctx.owner.obj );
            if (topic == null) {
                echoRedirect( alang( "exPostNotFound" ) );
                return;
            }
            if (boardError( topic )) return;

            topicService.Lock( topic, (User)ctx.viewer.obj, ctx.Ip );
            echoRedirect( lang( "opok" ), alink.ToAppData( topic ) );
        }

        [HttpPut, DbTransaction]
        public void UnLock( int id ) {

            ForumTopic topic = topicService.GetById( id, ctx.owner.obj );
            if (topic == null) {
                echo( alang( "exPostNotFound" ) );
                return;
            }
            if (boardError( topic )) return;

            topicService.UnLock( topic, (User)ctx.viewer.obj, ctx.Ip );
            echoRedirect( lang( "opok" ), alink.ToAppData( topic ) );
        }

        [HttpDelete, DbTransaction]
        public void DeletePost( int id ) {

            ForumPost post = postService.GetById( id, ctx.owner.obj );
            if (post == null) {
                echo( alang( "exPostNotFound" ) );
                return;
            }
            if (boardError( post )) return;

            postService.DeleteToTrash( post, (User)ctx.viewer.obj, ctx.Ip );

            ForumTopic topic = topicService.GetById( post.TopicId, ctx.owner.obj );

            echoRedirect( lang( "opok" ), alink.ToAppData( topic ) );
        }

        public void DeleteTopic( int id ) {

            ForumTopic topic = topicService.GetById( id, ctx.owner.obj );
            if (topic == null) {
                echo( alang( "exPostNotFound" ) );
                return;
            }
            if (boardError( topic )) return;

            topicService.DeleteToTrash( topic, (User)ctx.viewer.obj, ctx.Ip );

            echoRedirect( lang( "opok" ), alink.ToAppData( topic.ForumBoard ) );
        }

        private Tree<ForumBoard> _tree;

        private Tree<ForumBoard> getTree() {
            if (_tree == null) _tree = new Tree<ForumBoard>( boardService.GetBoardAll( ctx.app.Id, ctx.viewer.IsLogin ) );
            return _tree;
        }

        private int getPageSize() { return 100; }

        private Boolean boardError( ForumTopic topic ) {
            if (ctx.GetInt( "boardId" ) != topic.ForumBoard.Id) {
                echoRedirect( lang( "exNoPermission" ) );
                return true;
            }
            return false;
        }

        private Boolean boardError( ForumPost post ) {
            if (ctx.GetInt( "boardId" ) != post.ForumBoardId) {
                echoRedirect( lang( "exNoPermission" ) );
                return true;
            }
            return false;
        }

    }
}

