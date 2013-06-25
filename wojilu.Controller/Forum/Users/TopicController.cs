using System;
using System.Collections.Generic;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Forum.Domain;
using wojilu.Web.Controller.Forum.Utils;
using wojilu.Apps.Forum.Interface;
using wojilu.Apps.Forum.Service;
using wojilu.Common.Money.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Web.Controller.Forum.Users {

    [App( typeof( ForumApp ) )]
    public class TopicController : ControllerBase {

        public IAttachmentService attachService { get; set; }
        public IForumBoardService boardService { get; set; }
        public IForumCategoryService categoryService { get; set; }
        public IForumPostService postService { get; set; }
        public IForumTopicService topicService { get; set; }

        public TopicController() {
            boardService = new ForumBoardService();
            categoryService = new ForumCategoryService();
            topicService = new ForumTopicService();
            postService = new ForumPostService();
            attachService = new AttachmentService();
        }

        private Tree<ForumBoard> _tree;

        private Tree<ForumBoard> getTree() {
            if (_tree == null) _tree = new Tree<ForumBoard>( boardService.GetBoardAll( ctx.app.Id, ctx.viewer.IsLogin ) );
            return _tree;
        }


        public void NewQ() {

            int id = ctx.GetInt( "boardId" );

            ForumBoard board = getTree().GetById( id );
            if (board == null) {
                echo( alang( "exBoardNotFound" ) );
                return;
            }
            if (!SecurityHelper.Check( this, board )) return;

            List<ForumBoard> pathboards = getTree().GetPath( board.Id );
            set( "location", ForumLocationUtil.GetQuestionAdd( pathboards, ctx ) );

            bindAddForm( id );
            set( "uploadLink", to( new UploaderController().UploadForm ) + "?boardId=" + board.Id );

        }

        public void NewTopic() {

            int id = ctx.GetInt( "boardId" );

            ForumBoard board = getTree().GetById( id );
            if (board == null) {
                echo( alang( "exBoardNotFound" ) );
                return;
            }
            if (!SecurityHelper.Check( this, board )) return;

            List<ForumBoard> pathboards = getTree().GetPath( board.Id );
            set( "location", ForumLocationUtil.GetTopicAdd( pathboards, ctx ) );

            bindAddForm( id );

            set( "uploadLink", to( new UploaderController().UploadForm ) + "?boardId=" + board.Id );
            set( "batchUploadLink", to( new UploaderController().SaveFlashUpload ) + "?boardId=" + board.Id );
            set( "authJson", ctx.web.GetAuthJson() );

            // swf上传跨域问题
            set( "jsPath", sys.Path.DiskJs );

        }

        private void bindAddForm( int id ) {
            set( "ActionLink", to( Create ) + "?boardId=" + id );
            setCategory( id );
            set( "Title", ctx.Post( "Title" ) );
            set( "TagList", ctx.Post( "TagList" ) );
            set( "Content", ctx.PostHtml( "Content" ) );

            set( "currency.Unit", KeyCurrency.Instance.Unit );
            int uploadMax = 13;
            set( "uploadMax", uploadMax );
            set( "uploadMaxInfo", string.Format( alang( "uploadMax" ), uploadMax ) );
        }

        [HttpPost, DbTransaction]
        public void Create() {

            if (ForumValidator.IsIntervalShort( ctx )) {
                echoError( "对不起，您发布太快，请稍等一会儿再发布" );
                return;
            }

            int boardId = ctx.GetInt( "boardId" );

            ForumBoard board = boardService.GetById( boardId, ctx.owner.obj );
            if (board == null) {
                echo( alang( "exBoardNotFound" ) );
                return;
            }
            if (!SecurityHelper.Check( this, board )) return;

            ForumTopic topic = ForumValidator.ValidateTopic( ctx );
            if (ctx.HasErrors) {
                echoError();
                return;
            }

            if (ctx.PostIsCheck( "saveContentPic" )==1) {
                topic.Content = wojilu.Net.PageLoader.ProcessPic( topic.Content, null );
            }

            topic.ForumBoard = new ForumBoard( boardId );
            Result result = topicService.CreateTopic( topic, (User)ctx.viewer.obj, ctx.owner.obj, (IApp)ctx.app.obj );
            if (result.HasErrors) {
                errors.Join( result );
                echoError();
                return;
            }

            saveUploadedAttachments( topic );

            if (ctx.HasErrors) {
                echoText( errors.ErrorsHtml );
                return;
            }

            echoRedirect( lang( "opok" ), alink.ToAppData( topic ) );
            ForumValidator.AddCreateTime( ctx );
        }


        //-------------------------------------------------------------------------------

        private void saveUploadedAttachments( ForumTopic topic ) {
            String ids = ctx.PostIdList( "uploadFileIds" );
            int[] arrIds = cvt.ToIntArray( ids );

            attachService.CreateByTemp( ids, topic );
        }

        private void setCategory( int id ) {
            List<ForumCategory> categories = categoryService.GetByBoard( id );
            if (categories.Count > 0) {
                categories.Insert( 0, new ForumCategory( 0, alang( "plsSelectCategory" ) ) );
                set( "Category", "<div id=\"forum-form-cat\">"+ Html.DropList( categories, "CategoryId", "Name", "Id", ctx.PostInt( "CategoryId" ) ) + "</div>" );
            }
            else {
                set( "Category", string.Empty );
            }
        }

        private void setCategory( ForumTopic topic, ForumBoard board ) {
            List<ForumCategory> categories = categoryService.GetByBoard( board.Id );
            if (categories.Count > 0) {
                categories.Insert( 0, new ForumCategory( 0, alang( "plsSelectCategory" ) ) );
                int categoryId = topic.Category == null ? 0 : topic.Category.Id;
                set( "post.Category", Html.DropList( categories, "CategoryId", "Name", "Id", categoryId ) );
            }
            else {
                set( "post.Category", string.Empty );
            }
        }


    }

}
