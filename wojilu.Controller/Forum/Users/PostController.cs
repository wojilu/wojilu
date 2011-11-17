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

namespace wojilu.Web.Controller.Forum.Users {

    [App( typeof( ForumApp ) )]
    public class PostController : ControllerBase {

        public IAttachmentService attachService { get; set; }
        public IForumBoardService boardService { get; set; }
        public IForumPostService postService { get; set; }
        public IForumTopicService topicService { get; set; }
        public IModeratorService moderatorService { get; set; }


        public PostController() {
            boardService = new ForumBoardService();
            topicService = new ForumTopicService();
            postService = new ForumPostService();
            attachService = new AttachmentService();
            moderatorService = new ModeratorService();
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
            editor( "Content", "", "250px" );
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
            editor( "Content", "", "250px" );
        }

        public void QuoteTopic( int id ) {
            if (!checkLock( id )) return;

            view( "Quote" );
            ForumPost post = setTopicView( id );
            setQuoteContent( post );
        }

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

        [HttpPost, DbTransaction]
        public void Create() {

            ForumPost post = ForumValidator.ValidatePost( ctx );

            if (!checkLock( post.TopicId )) return;

            ForumBoard board = ctx.GetItem( "forumBoard" ) as ForumBoard;
            if (board == null || board.Id != post.ForumBoardId) {
                return;
            }

            ctx.SetItem( "boardId", board.Id );

            if (ctx.HasErrors) {
                echoError();
            }
            else {
                Result result = postService.Insert( post, (User)ctx.viewer.obj, ctx.owner.obj, (IApp)ctx.app.obj );
                if (result.IsValid) {

                    new ForumCacheRemove( boardService, topicService, this ).CreatePost( post );

                    String lnkTopicLastPage = getTopicLastPage( post );
                    echoRedirect( lang( "opok" ), lnkTopicLastPage );
                }
                else {
                    echoRedirect( result.ErrorsHtml );
                }
            }
        }


        private string getTopicLastPage( ForumPost post ) {
            String lnk = Link.To( new wojilu.Web.Controller.Forum.TopicController().Show, post.TopicId );
            int pageNo = postService.GetPageCount( post.TopicId, getPageSize( ctx.app.obj ) );
            lnk = Link.AppendPage( lnk, pageNo );

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
                dataLink = Link.To( new wojilu.Web.Controller.Forum.TopicController().Show, post.TopicId );
            }
            else {
                dataLink = Link.To( new wojilu.Web.Controller.Forum.PostController().Show, post.Id );
            }

            String lnk = string.Format( "<a href=\"{0}\" class=\"qOriginal\"><img src=\"{1}back.gif\"/></a>", dataLink, sys.Path.Img );

            String signature = string.Format( " <a href=\"{0}\">{1}</a> at {2} {3}", Link.ToMember( post.Creator ), post.Creator.Name, post.Created.ToString( "g" ), lnk );


            String content = string.Format( "<div class=\"quoteContainer\"><div class=\"quote\"><div class=\"qSpan\">{0}<div class=\"quoteAuthor\">{1}</div></div></div></div>", post.Content, signature );
            content += "<p>&nbsp;</p>";

            editor( "Content", content, "350px" );
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
