using System;
using System.Collections.Generic;
using System.Text;

using wojilu.ORM;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Service;
using wojilu.Apps.Forum.Interface;

using wojilu.Web.Controller.Forum.Utils;
using wojilu.Common.AppBase.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Web.Controller.Common;
using wojilu.Common.Money.Domain;
using wojilu.Common.Money.Interface;
using wojilu.Common.Money.Service;

namespace wojilu.Web.Controller.Forum.Users {

    [App( typeof( ForumApp ) )]
    public class PostController : ControllerBase {

        public IAttachmentService attachService { get; set; }
        public IForumBoardService boardService { get; set; }
        public IForumPostService postService { get; set; }
        public IForumTopicService topicService { get; set; }
        public IModeratorService moderatorService { get; set; }
        public IForumBuyLogService buyService { get; set; }
        public IUserIncomeService incomeService { get; set; }

        public PostController() {
            boardService = new ForumBoardService();
            topicService = new ForumTopicService();
            postService = new ForumPostService();
            attachService = new AttachmentService();
            moderatorService = new ModeratorService();
            buyService = new ForumBuyLogService();
            incomeService = new UserIncomeService();
        }

        private Tree<ForumBoard> _tree;

        private Tree<ForumBoard> getTree() {
            if (_tree == null) _tree = new Tree<ForumBoard>( boardService.GetBoardAll( ctx.app.Id, ctx.viewer.IsLogin ) );
            return _tree;
        }

        public void ReplyPost( int id ) {

            if (!checkLockByPost( id )) return;

            view( "Quote" );
            setPostView( id );
            set( "Content", "" );
        }

        public void QuotePost( int id ) {
            if (!checkLockByPost( id )) return;

            view( "Quote" );
            ForumPost post = setPostView( id );
            setQuoteContent( post );
        }

        public void ReplyTopic( int id ) {
            if (!checkLock( id )) return;

            view( "Quote" );
            setTopicView( id );
            set( "Content", "" );
        }

        public void QuoteTopic( int id ) {
            if (!checkLock( id )) return;

            view( "Quote" );
            ForumPost post = setTopicView( id );
            setQuoteContent( post );
        }


        [HttpPost, DbTransaction]
        public void Create() {

            if (ForumValidator.IsIntervalShort( ctx )) {
                echoError( "对不起，您发布太快，请稍等一会儿再发布" );
                return;
            }

            ForumPost post = ForumValidator.ValidatePost( ctx );

            if (!checkLock( post.TopicId )) return;

            ForumBoard board = ctx.GetItem( "forumBoard" ) as ForumBoard;
            if (board == null || board.Id != post.ForumBoardId) {
                return;
            }

            ctx.SetItem( "boardId", board.Id );

            if (ctx.HasErrors) {
                echoError();
                return;
            }

            Result result = postService.Insert( post, (User)ctx.viewer.obj, ctx.owner.obj, (IApp)ctx.app.obj );
            if (result.HasErrors) {
                echoError( result );
                return;
            }

            int lastPageNo = getLastPageNo( post );
            if (ctx.PostInt( "__ajaxUpdate" ) > 0) {
                echoAjaxUpdate( post, board, lastPageNo );
            }
            else {
                String lnkTopicLastPage = getTopicLastPage( post, lastPageNo );
                echoRedirect( lang( "opok" ), lnkTopicLastPage );
            }

            ForumValidator.AddCreateTime( ctx );
        }

        private void echoAjaxUpdate( ForumPost post, ForumBoard board, int lastPageNo ) {
            if (ctx.PostInt( "currentPageId" ) == lastPageNo) {
                String postHtml = getReturnHtml( post, board );
                echoJsonMsg( "ajax", true, postHtml );
            }
            else {
                String lnkTopicLastPage = getTopicLastPage( post, lastPageNo );
                echoJsonMsg( "redirect", true, lnkTopicLastPage );
            }
        }

        private int getLastPageNo( ForumPost post ) {
            int pageNo = postService.GetPageCount( post.TopicId, getPageSize( ctx.app.obj ) );
            return pageNo;
        }

        private String getReturnHtml( ForumPost post, ForumBoard board ) {

            ForumTopic topic = topicService.GetById( post.TopicId, ctx.owner.obj );
            List<ForumPost> posts = new List<ForumPost>();
            posts.Add( post );

            ctx.SetItem( "forumBoard", board );
            ctx.SetItem( "forumTopic", topic );
            ctx.SetItem( "posts", posts );
            ctx.SetItem( "attachs", new List<Attachment>() );
            ctx.SetItem( "pageSize", -1 );

            String postHtml = loadHtml( new Forum.TopicController().PostLoop );
            return postHtml;
        }
        //-----------------------------------------------------------------------------

        public void Buy( int postId ) {

            ForumPost post = postService.GetById( postId, ctx.owner.obj );
            ForumTopic topic = topicService.GetById( post.TopicId, ctx.owner.obj );
            if (boardError( topic )) return;

            if (topic.Price <= 0) {
                echo( "topic.price <=0" );
                return;
            }

            if (incomeService.HasEnoughKeyIncome( ctx.viewer.Id, topic.Price ) == false) {
                echo( String.Format( alang( "exIncome" ), KeyCurrency.Instance.Name ) );
                return;
            }

            set( "ActionLink", to( SaveBuy, postId ) + "?boardId=" + ctx.GetInt( "boardId" ) );
        }

        [HttpPost]
        public void SaveBuy( int postId ) {
            ForumPost post = postService.GetById( postId, ctx.owner.obj );
            ForumTopic topic = topicService.GetById( post.TopicId, ctx.owner.obj );
            if (boardError( topic )) return;

            Result result = buyService.Buy( ctx.viewer.Id, post.Creator.Id, topic );

            if (result.IsValid) {
                echoToParent( alang( "buyok" ) );
            }
            else {
                echoError( result );
            }
        }

        //------------------------------------ 悬赏 -----------------------------------------

        private int getPageSize() { return 100; }

        private Boolean checkCreatorPermission( ForumTopic topic ) {
            if (topic.Creator.Id != ctx.viewer.Id) {
                echoText( alang( "exRewardSelfOnly" ) );
                return false;
            }
            return true;
        }

        private Boolean boardError( ForumTopic topic ) {
            if (ctx.GetInt( "boardId" ) != topic.ForumBoard.Id) {
                echoRedirect( lang( "exNoPermission" ) + ": borad id error" );
                return true;
            }
            return false;
        }

        private Boolean boardError( ForumPost post ) {
            if (ctx.GetInt( "boardId" ) != post.ForumBoardId) {
                echoRedirect( lang( "exNoPermission" ) + ": borad id error" );
                return true;
            }
            return false;
        }

        public void SetReward( int id ) {

            ForumTopic topic = topicService.GetById( id, ctx.owner.obj );
            if (topic == null) {
                echoRedirect( alang( "exTopicNotFound" ) );
                return;
            }

            if (boardError( topic )) return;


            if (!checkCreatorPermission( topic )) return;


            Page.Title = alang( "setReward" ) + ":" + topic.Title;
            set( "ActionLink", to( SaveReward, id ) + "?boardId=" + topic.ForumBoard.Id );

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

            set( "ActionLink", to( SaveReward, id ) + "?boardId=" + topic.ForumBoard.Id );

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
            set( "ActionLink", to( SaveReward, id ) + "?boardId=" + topic.ForumBoard.Id );
        }

        [HttpPost, DbTransaction]
        public void SaveReward( int id ) {

            int rewardValue = ctx.PostInt( "PostReward" );
            if (rewardValue <= 0) {
                errors.Add( alang( "exRewardNotValid" ) );
                echoError();
                return;
            }

            ForumPost post = postService.GetById( id, ctx.owner.obj );
            ForumTopic topic = topicService.GetById( post.TopicId, ctx.owner.obj );
            int rewardAvailable = topic.RewardAvailable;

            if (boardError( topic )) return;

            if (!checkCreatorPermission( topic )) return;

            if (rewardAvailable <= 0) {
                errors.Add( alang( "exNoRewardAvailable" ) );
                echoError();
                return;
            }

            if (rewardValue > rewardAvailable) {
                errors.Add( string.Format( alang( "exMaxReward" ), rewardAvailable ) );
                echoError();
                return;
            }

            postService.AddReward( post, rewardValue );

            echoToParent( lang( "opok" ) );
        }


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


        private void bindPostList( DataPage<ForumPost> list ) {

            IBlock block = getBlock( "list" );
            foreach (ForumPost post in list.Results) {

                if (post.ParentId == 0) {
                    block.Set( "p.Reward", "--" );
                }
                else if (post.Reward > 0) {
                    block.Set( "p.Reward", cvt.ToInt( post.Reward ) );
                }
                else {
                    block.Set( "p.Reward", string.Format( "<a href='{0}' class='frmBox btn btn-mini'><i class=\"icon-plus-sign\"></i> " + alang( "setReward" ) + "</a>", to( AddReward, post.Id ) + "?boardId=" + post.ForumBoardId ) );
                }

                block.Set( "p.User", post.Creator.Name );

                String content = strUtil.ParseHtml( post.Content, 70 );

                String lnk;
                if (post.ParentId == 0) {
                    ForumTopic topic = topicService.GetById( post.TopicId, ctx.owner.obj );
                    lnk = alink.ToAppData( topic );
                }
                else {
                    lnk = alink.ToAppData( post );
                }

                block.Set( "p.Content", content + string.Format( " <a href=\"{0}\">{1}</a>", lnk, "原帖" ) );

                block.Set( "p.Created", post.Created );
                block.Next();
            }

            set( "page", list.PageBar );
        }

        //---------------------------------------------------------------------------


        private Boolean checkLock( int topicId ) {
            ForumTopic topic = topicService.GetById( topicId, ctx.owner.obj );
            return checkIsLockPrivate( topic );
        }

        private Boolean checkLockByPost( int postId ) {
            ForumPost post = postService.GetById( postId, ctx.owner.obj );
            ForumTopic topic = topicService.GetById( post.TopicId, ctx.owner.obj );
            return checkIsLockPrivate( topic );
        }

        private Boolean checkIsLockPrivate( ForumTopic topic ) {
            if (topic.IsLocked == 1) {
                echoRedirect( alang( "exLockTip" ) );
                return false;
            }
            return true;
        }

        private string getTopicLastPage( ForumPost post, int pageNo ) {
            String lnk = to( new wojilu.Web.Controller.Forum.TopicController().Show, post.TopicId );
            lnk = PageHelper.AppendNo( lnk, pageNo );

            if (ctx.web.PathReferrer.IndexOf( "reload=true" ) < 0) {
                lnk = lnk + "?reload=true#post" + post.Id;
            }
            else {
                lnk = lnk + "#post" + post.Id;
            }

            return lnk;
        }

        private int getPageSize( object app ) {
            return ((ForumApp)app).GetSettingsObj().ReplySize;
        }


        private void setQuoteContent( ForumPost post ) {

            String dataLink;
            if (post.ParentId == 0) {
                dataLink = to( new wojilu.Web.Controller.Forum.TopicController().Show, post.TopicId );
            }
            else {
                dataLink = to( new wojilu.Web.Controller.Forum.PostController().Show, post.Id );
            }

            String lnk = string.Format( "<a href=\"{0}\" class=\"qOriginal\"><img src=\"{1}back.gif\"/></a>", dataLink, sys.Path.Img );

            String signature = string.Format( " <a href=\"{0}\">{1}</a> at {2} {3}", toUser( post.Creator ), post.Creator.Name, post.Created.ToString( "g" ), lnk );

            String content = string.Format( "<blockquote>{0}<p class=\"quote-user\">{1}</p></blockquote>", post.Content, signature );
            content += "<p>&nbsp;</p>";

            set( "Content", content );
        }

        private ForumPost setPostView( int id ) {

            ForumPost post = postService.GetById( id, ctx.owner.obj );
            ForumTopic topic = topicService.GetById( post.TopicId, ctx.owner.obj );

            ForumBoard board = ctx.GetItem( "forumBoard" ) as ForumBoard;
            if (board == null || board.Id != post.ForumBoardId) {
                return null;
            }

            List<ForumBoard> pathboards = getTree().GetPath( board.Id );
            set( "location", ForumLocationUtil.GetReply( pathboards, topic, ctx ) );

            set( "post.ReplyTitle", "re:" + post.Title );
            set( "post.ForumBoardId", post.ForumBoardId );
            set( "post.TopicId", post.TopicId );
            set( "post.ParentId", post.Id );
            set( "post.ReplyActionUrl", to( new Users.PostController().Create ) + "?boardId=" + topic.ForumBoard.Id );
            return post;
        }

        private ForumPost setTopicView( int id ) {

            ForumPost post = postService.GetPostByTopic( id );
            ForumTopic topic = topicService.GetById( post.TopicId, ctx.owner.obj );

            ForumBoard board = ctx.GetItem( "forumBoard" ) as ForumBoard;
            if (board == null || board.Id != post.ForumBoardId) {
                return null;
            }

            List<ForumBoard> pathboards = getTree().GetPath( board.Id );
            set( "location", ForumLocationUtil.GetReply( pathboards, topic, ctx ) );

            set( "post.ReplyTitle", "re:" + post.Title );
            set( "post.ForumBoardId", post.ForumBoardId );
            set( "post.TopicId", post.TopicId );
            set( "post.ParentId", post.Id );

            set( "post.ReplyActionUrl", to( Create ) + "?boardId=" + topic.ForumBoard.Id );

            return post;
        }


    }

}
