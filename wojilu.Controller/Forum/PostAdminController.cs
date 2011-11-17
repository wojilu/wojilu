/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Service;
using wojilu.Web.Controller.Forum.Utils;
using wojilu.Members.Users.Service;
using wojilu.Common.Money.Service;
using wojilu.Common.Money.Domain;
using wojilu.Common.Resource;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Forum.Interface;
using wojilu.Common.Money.Interface;
using wojilu.Members.Users.Interface;

namespace wojilu.Web.Controller.Forum {

    [App( typeof( ForumApp ) )]
    public partial class PostAdminController : ControllerBase {

        public IForumBoardService boardService { get; set; }
        public IForumBuyLogService buyService { get; set; }
        public ICurrencyService currencyService { get; set; }
        public IForumPostService postService { get; set; }
        public IForumRateService rateService { get; set; }
        public IForumTopicService topicService { get; set; }
        public IUserService userService { get; set; }
        public IUserIncomeService userIncomeService { get; set; }
        public IModeratorService moderatorService { get; set; }

        public PostAdminController() {
            topicService = new ForumTopicService();
            boardService = new ForumBoardService();
            postService = new ForumPostService();
            currencyService = new CurrencyService();
            userService = new UserService();
            rateService = new ForumRateService();
            buyService = new ForumBuyLogService();
            userIncomeService = new UserIncomeService();
            moderatorService = new ModeratorService();
        }

        public override void CheckPermission() {
            if (ctx.viewer.IsLogin == false) {
                echo( lang( "exPlsLogin" ) );
            }
        }

        //------------------------------------ 悬赏 -----------------------------------------

        public void SetReward( int id ) {

            ForumTopic topic = topicService.GetById( id, ctx.owner.obj );
            if (topic == null) {
                echoRedirect( alang( "exTopicNotFound" ) );
                return;
            }

            if (!checkCreatorPermission( topic )) return;

            Page.Title = alang( "setReward" ) + ":" + topic.Title;
            target( SaveReward, id );

            DataPage<ForumPost> list = postService.GetPageList( id, getPageSize(), 0 );

            bindRewardInfo( topic );
            bindPostList( list );
        }

        public void RewardList( int id ) {

            ForumTopic topic = topicService.GetById( id, ctx.owner.obj );
            if (topic == null) {
                echoRedirect( alang( "exTopicNotFound" ) );
                return;
            }

            target( SaveReward, id );

            DataPage<ForumPost> list = postService.GetPageList( id, getPageSize(), 0 );

            bindRewardInfo( topic );
            bindRewardList( list );
        }

        public void AddReward( int id ) {

            ForumPost post = postService.GetById( id, ctx.owner.obj );
            ForumTopic topic = topicService.GetById( post.TopicId, ctx.owner.obj );
            if (!checkCreatorPermission( topic )) return;

            set( "post.RewardAvailable", topic.RewardAvailable );
            set( "post.Id", id );
            target( SaveReward, id );
        }

        [HttpPost, DbTransaction]
        public void SaveReward( int id ) {

            int rewardValue = ctx.PostInt( "PostReward" );
            if (rewardValue <= 0) {
                errors.Add( alang( "exRewardNotValid" ) );
                run( AddReward, id );
                return;
            }


            ForumPost post = postService.GetById( id, ctx.owner.obj );
            ForumTopic topic = topicService.GetById( post.TopicId, ctx.owner.obj );
            int rewardAvailable = topic.RewardAvailable;

            if (!checkCreatorPermission( topic )) return;

            if (rewardAvailable <= 0) {
                errors.Add( alang( "exNoRewardAvailable" ) );
                run( AddReward, id );
                return;
            }

            if (rewardValue > rewardAvailable) {
                errors.Add( string.Format( alang( "exMaxReward" ), rewardAvailable ) );
                run( AddReward, id );
                return;
            }

            postService.AddReward( post, rewardValue );
            //userIncomeService.AddKeyIncome( post.Creator, rewardValue );
            //topicService.SubstractTopicReward( topic, rewardValue );

            echoToParent( lang( "opok" ) );
        }

        private Boolean checkCreatorPermission( ForumTopic topic ) {
            if (topic.Creator.Id != ctx.viewer.Id) {
                echoText( alang( "exRewardSelfOnly" ) );
                return false;
            }
            return true;
        }

        //-----------------------------------------------------------------------------

        public void Buy( int postId ) {

            ForumPost post = postService.GetById( postId, ctx.owner.obj );
            ForumTopic topic = topicService.GetById( post.TopicId, ctx.owner.obj );
            Result result = buyService.Buy( ctx.viewer.Id, post.Creator.Id, topic );
            if (result.IsValid) {
                set( "content", post.Content.Replace( "'", "&#39;" ) );
            }
            else {
                echoError( result.ErrorsText );
            }
        }

        //------------------------------------ 版主管理：帖子评分 -----------------------------------------


        public void AddCredit( int id ) {

            if (rateService.IsUserRate( (User)ctx.viewer.obj, id )) {
                echoText( alang( "exRewarded" ) );
                return;
            }

            ForumPost post = postService.GetById( id, ctx.owner.obj );
            if (post == null) {
                echoRedirect( alang( "exPostNotFound" ) );
                return;
            }

            if (post.Creator.Id == ctx.viewer.Id) {
                echoText( alang( "exNotAllowSelfCredit" ) );
                return;
            }

            ForumBoard board = boardService.GetById( post.ForumBoardId, ctx.owner.obj );
            if (!PermissionUtil.Check( this, board )) return;

            target( SaveCredit, id );

            IList currencyList = currencyService.GetForumRateCurrency();
            dropList( "CurrencyId", currencyList, "Name=Id", 0 );

            List<PropertyItem> values = getCurrencyValues();
            dropList( "CurrencyValue", values, "Value=Value", 2 );
        }

        private List<PropertyItem> getCurrencyValues() {
            List<PropertyItem> values = new List<PropertyItem>();
            int rateMaxValue = ((ForumApp)ctx.app.obj).MaxRateValue;
            for (int i = rateMaxValue / 2; i > 0; i--) {
                values.Add( new PropertyItem( "CurencyValue", -i * 2 ) );
            }
            for (int i = 1; i <= (rateMaxValue / 2); i++) {
                values.Add( new PropertyItem( "CurencyValue", i * 2 ) );
            }
            return values;
        }

        public void SaveCredit( int id ) {

            ForumPost post = postService.GetById( id, ctx.owner.obj );
            if (post == null) {
                echoRedirect( alang( "exPostNotFound" ) );
                return;
            }

            ForumBoard board = boardService.GetById( post.ForumBoardId, ctx.owner.obj );
            if (!PermissionUtil.Check( this, board )) return;

            int rateMaxValue = ((ForumApp)ctx.app.obj).MaxRateValue;

            int currencyValue = ctx.PostInt( "CurrencyValue" );
            if (((currencyValue != 0) && (currencyValue >= -rateMaxValue)) && (currencyValue <= rateMaxValue)) {
                postService.SetPostCredit( post, ctx.PostInt( "CurrencyId" ), currencyValue, ctx.Post( "Reason" ), (User)ctx.viewer.obj );
                userIncomeService.AddIncome( post.Creator, ctx.PostInt( "CurrencyId" ), currencyValue );
                echoRedirect( lang( "opok" ) );
            }
            else {
                errors.Add( alang( "exCreditNotValid" ) );
                run( AddCredit, id );
            }
        }

        //------------------------------------ admin -----------------------------------------

        [HttpPut, DbTransaction]
        public void Ban( int id ) {

            ForumPost post = postService.GetById( id, ctx.owner.obj );
            if (post == null) {
                actionContent( alang( "exPostNotFound" ) );
                return;
            }

            ForumBoard board = boardService.GetById( post.ForumBoardId, ctx.owner.obj );
            if (!PermissionUtil.Check( this, board )) return;

            postService.BanPost( post, ctx.Post( "Reason" ), ctx.PostIsCheck( "IsSendMsg" ), (User)ctx.viewer.obj, ctx.app.Id, ctx.Ip );

            new ForumCacheRemove( this.boardService, topicService, this ).BanPost( post );
            echoRedirect( lang( "opok" ) );
        }

        [HttpPut, DbTransaction]
        public void UnBan( int id ) {

            ForumPost post = postService.GetById( id, ctx.owner.obj );
            if (post == null) {
                actionContent( alang( "exPostNotFound" ) );
                return;
            }

            ForumBoard board = boardService.GetById( post.ForumBoardId, ctx.owner.obj );
            if (!PermissionUtil.Check( this, board )) return;

            postService.UnBanPost( post, (User)ctx.viewer.obj, ctx.app.Id, ctx.Ip );
            new ForumCacheRemove( this.boardService, topicService, this ).BanPost( post );
            echoRedirect( lang( "opok" ) );
        }

        [HttpPut, DbTransaction]
        public void Lock( int id ) {

            ForumTopic topic = topicService.GetById( id, ctx.owner.obj );
            if (topic == null) {
                echoRedirect( alang( "exPostNotFound" ) );
                return;
            }

            ForumBoard board = boardService.GetById( topic.ForumBoard.Id, ctx.owner.obj );
            if (!PermissionUtil.Check( this, board )) return;

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

            ForumBoard board = boardService.GetById( topic.ForumBoard.Id, ctx.owner.obj );
            if (!PermissionUtil.Check( this, board )) return;

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

            ForumBoard board = boardService.GetById( post.ForumBoardId, ctx.owner.obj );
            if (!PermissionUtil.Check( this, board )) return;

            postService.DeleteToTrash( post, (User)ctx.viewer.obj, ctx.Ip );
            echoRedirect( lang( "opok" ) );
        }

        public void DeleteTopic( int id ) {

            ForumTopic topic = topicService.GetById( id, ctx.owner.obj );
            if (topic == null) {
                echo( alang( "exPostNotFound" ) );
                return;
            }

            ForumBoard board = boardService.GetById( topic.ForumBoard.Id, ctx.owner.obj );
            if (!PermissionUtil.Check( this, board )) return;

            topicService.DeleteToTrash( topic, (User)ctx.viewer.obj, ctx.Ip );
            echoRedirect( lang( "opok" ), alink.ToAppData( board ) );
        }

        public void Detail( int id ) {

            ForumPost post = postService.GetById( id, ctx.owner.obj );

            String postContent = "<div style='width:600px;'><div>" +
                lang( "author" ) + ": {0} " +
                lang( "title" ) + ": {1} <span class='note'>({2})</span></div>" +
                "<hr/><div>{3}</div></div>";

            actionContent( string.Format( postContent,
                post.Creator.Name, post.Title, post.Created, post.Content ) );
        }

        private Tree<ForumBoard> _tree;

        private Tree<ForumBoard> getTree() {
            if (_tree == null) _tree = new Tree<ForumBoard>( boardService.GetBoardAll( ctx.app.Id, ctx.viewer.IsLogin ) );
            return _tree;
        }

        private int getPageSize() { return 100; }


    }
}

