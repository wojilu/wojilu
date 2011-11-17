/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Interface;
using wojilu.Apps.Forum.Service;
using wojilu.Common.Money.Domain;
using wojilu.Common.Money.Interface;
using wojilu.Common.Money.Service;
using wojilu.Common.Resource;
using wojilu.Members.Users.Domain;
using wojilu.Web.Controller.Forum.Utils;

namespace wojilu.Web.Controller.Forum.Moderators {

    [App( typeof( ForumApp ) )]
    public class PostController : ControllerBase {

        public IForumBoardService boardService { get; set; }
        public ICurrencyService currencyService { get; set; }
        public IForumTopicService topicService { get; set; }
        public IForumPostService postService { get; set; }
        public IForumRateService rateService { get; set; }

        public PostController() {
            topicService = new ForumTopicService();
            boardService = new ForumBoardService();
            postService = new ForumPostService();
            currencyService = new CurrencyService();
            rateService = new ForumRateService();
        }

        private Boolean checkCreatorPermission( ForumTopic topic ) {
            if (topic.Creator.Id != ctx.viewer.Id) {
                echoText( alang( "exRewardSelfOnly" ) );
                return false;
            }
            return true;
        }

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

        //------------------------------------ 悬赏 -----------------------------------------

        public void SetReward( int id ) {

            ForumTopic topic = topicService.GetById( id, ctx.owner.obj );
            if (topic == null) {
                echoRedirect( alang( "exTopicNotFound" ) );
                return;
            }

            if (boardError( topic )) return;


            if (!checkCreatorPermission( topic )) return;


            Page.Title = alang( "setReward" ) + ":" + topic.Title;
            set( "ActionLink", to( new PostSaveController().SaveReward, id ) + "?boardId=" + topic.ForumBoard.Id );

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

            if (boardError( topic )) return;

            set( "ActionLink", to( new PostSaveController().SaveReward, id ) + "?boardId=" + topic.ForumBoard.Id );

            DataPage<ForumPost> list = postService.GetPageList( id, getPageSize(), 0 );

            bindRewardInfo( topic );
            bindRewardList( list );
        }

        public void AddReward( int id ) {

            ForumPost post = postService.GetById( id, ctx.owner.obj );
            ForumTopic topic = topicService.GetById( post.TopicId, ctx.owner.obj );

            if (boardError( topic )) return;

            if (!checkCreatorPermission( topic )) return;

            set( "post.RewardAvailable", topic.RewardAvailable );
            set( "post.Id", id );
            set( "ActionLink", to( new PostSaveController().SaveReward, id ) + "?boardId=" + topic.ForumBoard.Id );
        }

        //--------------------------------------------------------------------------


        public void AddCredit( int id ) {

            String msg = "<div style=\"font-size:22px;color:red;font-weight:bold;margin-top:30px; text-align:center;\">{0}</div>";

            if (rateService.IsUserRate( (User)ctx.viewer.obj, id )) {
                actionContent( string.Format( msg, alang( "exRewarded" ) ) );
                return;
            }

            ForumPost post = postService.GetById( id, ctx.owner.obj );
            if (post == null) {
                actionContent( string.Format( msg, alang( "exPostNotFound" ) ) );
                return;
            }

            if (post.Creator.Id == ctx.viewer.Id) {
                actionContent( string.Format( msg, alang( "exNotAllowSelfCredit" ) ) );
                return;
            }

            if (boardError( post )) return;


            ForumBoard board = boardService.GetById( post.ForumBoardId, ctx.owner.obj );

            set( "ActionLink", to( new PostSaveController().SaveCredit, id ) + "?boardId=" + board.Id );


            IList currencyList = currencyService.GetForumRateCurrency();
            dropList( "CurrencyId", currencyList, "Name=Id", 0 );

            List<PropertyItem> values = getCurrencyValues();
            dropList( "CurrencyValue", values, "Value=Value", 2 );
        }

        public void Detail( int id ) {

            ForumPost post = postService.GetById( id, ctx.owner.obj );
            if (boardError( post )) return;

            String postContent = "<div style='width:600px;'><div>" +
                lang( "author" ) + ": {0} " +
                lang( "title" ) + ": {1} <span class='note'>({2})</span></div>" +
                "<hr/><div>{3}</div></div>";

            actionContent( string.Format( postContent,
                post.Creator.Name, post.Title, post.Created, post.Content ) );
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

        private int getPageSize() { return 100; }


        private void bindRewardInfo( ForumTopic topic ) {

            List<ForumBoard> pathboards = getTree().GetPath( topic.ForumBoard.Id );
            set( "location", ForumLocationUtil.GetSetReward( pathboards, topic, ctx ) );

            int rewardAvailable = topic.RewardAvailable;

            set( "currency.Name", KeyCurrency.Instance.Name );
            set( "post.Reward", topic.Reward );
            set( "post.RewardSetted", topic.Reward - rewardAvailable );
            set( "post.RewardAvailable", rewardAvailable );

            String rewardInfo = string.Format( alang( "rewardInfo" ), (topic.Reward - rewardAvailable), rewardAvailable );
            set( "rewardInfo", rewardInfo );
        }


        private void bindPostList( DataPage<ForumPost> list ) {

            IBlock block = getBlock( "list" );
            foreach (ForumPost post in list.Results) {

                if (post.ParentId == 0)
                    block.Set( "p.Reward", "--" );
                else if (post.Reward > 0)
                    block.Set( "p.Reward", cvt.ToInt( post.Reward ) );
                else
                    block.Set( "p.Reward", string.Format( "<a href='{0}' class='frmBox'>+ " + alang( "setReward" ) + "</a>", to( AddReward, post.Id ) + "?boardId=" + post.ForumBoardId ) );

                block.Set( "p.User", post.Creator.Name );

                String content = strUtil.ParseHtml( post.Content, 70 );
                if (content.EndsWith( "..." )) {
                    String lnkDetail = string.Format( "<a href='{0}' class='frmBox left10'>" + lang( "more" ) + ForumLocationUtil.separator + "</a>", to( Detail, post.Id ) + "?boardId=" + post.ForumBoardId, sys.Path.Skin );
                    block.Set( "p.Content", content + lnkDetail );
                }
                else {
                    block.Set( "p.Content", content );
                }

                block.Set( "p.Created", post.Created );
                block.Next();
            }

            set( "page", list.PageBar );
        }


        private void bindRewardList( DataPage<ForumPost> list ) {
            IBlock block = getBlock( "list" );
            foreach (ForumPost post in list.Results) {

                if ((post.ParentId == 0) || (post.Reward == 0))
                    block.Set( "p.Reward", "--" );
                else
                    block.Set( "p.Reward", cvt.ToInt( post.Reward ) );

                block.Set( "p.User", post.Creator.Name );
                block.Set( "p.Content", strUtil.ParseHtml( post.Content, 70 ) );
                block.Set( "p.Created", post.Created );
                block.Next();
            }
            set( "page", list.PageBar );
        }

        private Tree<ForumBoard> _tree;

        private Tree<ForumBoard> getTree() {
            if (_tree == null) _tree = new Tree<ForumBoard>( boardService.GetBoardAll( ctx.app.Id, ctx.viewer.IsLogin ) );
            return _tree;
        }
    }

}
