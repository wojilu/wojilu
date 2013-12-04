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


        public virtual IForumBoardService boardService { get; set; }
        public virtual IForumPostService postService { get; set; }
        public virtual IForumTopicService topicService { get; set; }
        public virtual IUserIncomeService userIncomeService { get; set; }

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

        public virtual void SaveCredit( long id ) {

            ForumPost post = postService.GetById( id, ctx.owner.obj );
            if (post == null) {
                echoRedirect( alang( "exPostNotFound" ) );
                return;
            }

            if (boardError( post )) return;

            int rateMaxValue = ((ForumApp)ctx.app.obj).MaxRateValue;

            long currencyId = ctx.PostLong( "CurrencyId" );
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
        public virtual void Ban( long id ) {

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
        public virtual void UnBan( long id ) {

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
        public virtual void Lock( long id ) {

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
        public virtual void UnLock( long id ) {

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
        public virtual void DeletePost( long id ) {

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

        public virtual void DeleteTopic( long id ) {

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
            if (ctx.GetLong( "boardId" ) != topic.ForumBoard.Id) {
                echoRedirect( lang( "exNoPermission" ) );
                return true;
            }
            return false;
        }

        private Boolean boardError( ForumPost post ) {
            if (ctx.GetLong( "boardId" ) != post.ForumBoardId) {
                echoRedirect( lang( "exNoPermission" ) );
                return true;
            }
            return false;
        }

    }
}

