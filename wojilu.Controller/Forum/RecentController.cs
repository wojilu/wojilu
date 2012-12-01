/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Service;
using wojilu.Common.Money.Domain;
using wojilu.Web.Controller.Forum.Utils;
using wojilu.Apps.Forum.Interface;
using wojilu.Web.Controller.Common;
using wojilu.Web.Controller.Forum.Caching;

namespace wojilu.Web.Controller.Forum {

    [App( typeof( ForumApp ) )]
    public class RecentController : ControllerBase {

        public IForumTopicService topicService { get; set; }
        public IForumBoardService boardService { get; set; }
        public IForumPostService postService { get; set; }

        public RecentController() {
            topicService = new ForumTopicService();
            boardService = new ForumBoardService();
            postService = new ForumPostService();
        }

        [CacheAction( typeof( ForumRecentLayoutCache ) )]
        public override void Layout() {

            List<ForumBoard> boards = getTree().GetPath( -1 );
            set( "location", ForumLocationUtil.GetRecent( boards, ctx ) );

            set( "lnkMyTopic", to( new RecentController().MyTopic ) );
            set( "lnkMyPost", to( new RecentController().MyPost ) );

            set( "lnkPost", to( new RecentController().Post ) );
            set( "lnkTopic", to( new RecentController().Topic ) );
            set( "lnkPicked", to( new RecentController().Picked ) );
            set( "lnkReplies", to( new RecentController().Replies ) );
            set( "lnkViews", to( new RecentController().Views ) );

            set( "lnkPickedImg", to( new RecentController().ImgTopic ) );
        }

        private Tree<ForumBoard> _tree;

        private Tree<ForumBoard> getTree() {
            if (_tree == null) _tree = new Tree<ForumBoard>( boardService.GetBoardAll( ctx.app.Id, ctx.viewer.IsLogin ) );
            return _tree;
        }

        [CacheAction( typeof( ForumRecentTopicCache ) )]
        public void Topic() {

            ctx.Page.Title = alang( "newTopic" );

            DataPage<ForumTopic> plist = topicService.GetPageByApp( ctx.app.Id, 50 );
            bintTopics( plist.Results, plist.PageBar );
        }


        [CacheAction( typeof( ForumRecentPostCache ) )]
        public void Post() {

            ctx.Page.Title = alang( "newPosts" );

            DataPage<ForumPost> results = postService.GetPageByApp( ctx.app.Id, 50 );
            bindPosts( results );
        }

        public void ImgTopic() {

            DataPage<ForumPickedImg> list = db.findPage<ForumPickedImg>( "AppId=" + ctx.app.Id );

            IBlock block = getBlock( "list" );
            foreach (ForumPickedImg f in list.Results) {
                block.Set( "f.Id", f.Id );
                block.Set( "f.Title", f.Title );
                block.Set( "f.Url", f.Url );
                block.Set( "f.ImgUrl", f.ImgUrl );
                block.Set( "f.Created", f.Created );
                block.Next();
            }
            set( "page", list.PageBar );

            
        }

        public void MyTopic() {
            view( "Topic" );
            DataPage<ForumTopic> plist = topicService.GetByUserAndApp( ctx.app.Id, ctx.viewer.Id, 50 );
            bintTopics( plist.Results, plist.PageBar );
        }

        public void Picked() {
            view( "Topic" );


            ctx.Page.Title = alang( "picked" );

            DataPage<ForumTopic> plist = topicService.GetPickedByApp( ctx.app.Id, 50 );
            bintTopics( plist.Results, plist.PageBar );
        }

        public void Replies() {
            view( "Topic" );

            ctx.Page.Title = alang( "rankByReplies" );

            List<ForumTopic> list = topicService.GetByAppAndReplies( ctx.app.Id, 50 );
            bintTopics( list, "" );
        }

        public void Views() {
            view( "Topic" );

            ctx.Page.Title = alang( "rankByViews" );

            List<ForumTopic> list = topicService.GetByAppAndViews( ctx.app.Id, 50 );
            bintTopics( list, "" );
        }



        public void MyPost() {
            view( "Post" );
            DataPage<ForumPost> results = postService.GetByAppAndUser( ctx.app.Id, ctx.viewer.Id, 50 );
            bindPosts( results );
        }


        private void bindPosts( DataPage<ForumPost> results ) {
            IBlock block = getBlock( "list" );
            foreach (ForumPost t in results.Results) {
                bindPostOne( block, t );
            }
            set( "page", results.PageBar );
        }


        private void bintTopics( List<ForumTopic> results, String pageBar ) {
            IBlock block = getBlock( "list" );
            foreach (ForumTopic t in results) {
                bindTopicOne( block, t );
            }
            set( "page", pageBar );
        }

        private void bindTopicOne( IBlock block, ForumTopic topic ) {

            if (topic.ForumBoard == null) return;
            if (topic.Creator == null) return;

            String rewardInfo = string.Empty;
            if (topic.Reward > 0) rewardInfo = getRewardInfo( topic, rewardInfo );

            String lblCategory = string.Empty;
            if (topic.Category != null && topic.Category.Id > 0) {
                String lnkCategory = to( new BoardController().Category, topic.ForumBoard.Id ) + "?categoryId=" + topic.Category.Id;
                lblCategory = string.Format( "<a href='{0}'>[<span style=\"color:{2}\">{1}]</span></a>&nbsp;", lnkCategory, topic.Category.Name, topic.Category.NameColor );
            }

            String typeImg = string.Empty;
            if (strUtil.HasText( topic.TypeName )) {
                typeImg = string.Format( "<img src='{0}apps/forum/{1}.gif'>", sys.Path.Skin, topic.TypeName );
            }

            String priceInfo = string.Empty;
            if (topic.Price > 0) {
                priceInfo = alang( "price" ) + " :" + topic.Price + " ";
            }

            String permissionInfo = string.Empty;
            if (topic.ReadPermission > 0) {
                permissionInfo = alang( "readPermission" ) + ":" + topic.ReadPermission + "";
            }

            block.Set( "p.Id", topic.Id );
            block.Set( "p.Category", lblCategory );
            block.Set( "p.TypeImg", typeImg );
            block.Set( "p.Reward", rewardInfo );
            block.Set( "p.Price", priceInfo );
            block.Set( "p.ReadPermission", permissionInfo );
            block.Set( "p.TitleStyle", topic.TitleStyle );
            block.Set( "p.Titile", strUtil.CutString( topic.Title, 30 ) );
            block.Set( "p.Url", to( new TopicController().Show, topic.Id ) );

            block.Set( "p.BoardName", topic.ForumBoard.Name );
            block.Set( "p.BoardLink", alink.ToAppData(topic.ForumBoard) );


            block.Set( "p.MemberName", topic.Creator.Name );
            block.Set( "p.MemberUrl", toUser( topic.Creator ) );
            block.Set( "p.CreateTime", topic.Created );
            block.Set( "p.ReplyCount", topic.Replies );
            block.Set( "p.Hits", topic.Hits.ToString() );
            block.Set( "p.LastUpdate", topic.Replied.GetDateTimeFormats( 'g' )[0] );
            block.Set( "p.LastReplyUrl", toUser( topic.RepliedUserFriendUrl ) );
            block.Set( "p.LastReplyName", topic.RepliedUserName );

            String attachments = topic.Attachments > 0 ? "<img src='" + sys.Path.Img + "attachment.gif'/>" : "";
            block.Set( "p.Attachments", attachments );

            String statusImg;

            if (topic.IsLocked == 1)
                statusImg = sys.Path.Skin + "apps/forum/lock.gif";
            else if (topic.IsPicked == 1)
                statusImg = sys.Path.Skin + "apps/forum/pick.gif";
            else
                statusImg = sys.Path.Skin + "apps/forum/topic.gif";

            block.Set( "postStatusImage", statusImg );
            block.Next();
        }

        private String getRewardInfo( ForumTopic topic, String rewardInfo ) {
            if (topic.RewardAvailable == 0) {
                rewardInfo = "(" + alang( "resolved" ) + ")";
            }
            else {
                rewardInfo = string.Format( "({0}{1})", topic.Reward, KeyCurrency.Instance.Unit );
            }
            if (DateTime.Now.Subtract( topic.Created ).Days >= ForumConfig.Instance.QuestionExpiryDay) {
            }
            return rewardInfo;
        }


        private void bindPostOne( IBlock block, ForumPost post ) {

            if (post.Creator == null) return;

            String title = post.Title;
            if (strUtil.IsNullOrEmpty( title )) {
                ForumTopic topic = topicService.GetById( post.TopicId, ctx.owner.obj );
                if (topic == null) return;
                title = "re:" + topic.Title;
            }

            block.Set( "p.Titile", strUtil.CutString( title, 38 ) );
            block.Set( "p.Url", to( new PostController().Show, post.Id ) );

            ForumBoard board = getTree().GetById( post.ForumBoardId );
            if (board == null) return;

            block.Set( "p.BoardName", board.Name );
            block.Set( "p.BoardLink", alink.ToAppData( board ) );
            block.Set( "p.MemberName", post.Creator.Name );
            block.Set( "p.MemberUrl", toUser( post.Creator ) );
            block.Set( "p.CreateTime", post.Created );

            block.Next();

        }
    }

}
