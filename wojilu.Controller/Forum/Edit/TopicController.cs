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

namespace wojilu.Web.Controller.Forum.Edit {

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


        public void Edit( int id ) {

            ForumTopic topic = topicService.GetById( id, ctx.owner.obj );
            if (topic == null) {
                echoRedirect( alang( "exTopicNotFound" ) );
                return;
            }

            ForumBoard board = getTree().GetById( topic.ForumBoard.Id );

            if (PermissionUtil.IsSelfEdit( ctx, topic ) == false) {
                if (!PermissionUtil.Check( this, board )) return;
            }

            ForumPost post = postService.GetPostByTopic( topic.Id );
            target( Update, id );

            List<ForumBoard> pathboards = getTree().GetPath( board.Id );
            set( "location", ForumLocationUtil.GetTopicEdit( pathboards, topic, ctx ) );

            setCategory( topic, board );
            set( "post.Title", post.Title );
            set( "post.TagList", topic.Tag.TextString );

            editor( "Content", post.Content, "300px" );

            set( "attachmentLink", to( new AttachmentController().Admin, id ) );
        }

        [HttpPost, DbTransaction]
        public void Update( int id ) {

            ForumTopic topic = topicService.GetById( id, ctx.owner.obj );
            if (topic == null) {
                echoRedirect( alang( "exTopicNotFound" ) );
                return;
            }

            ForumBoard board = boardService.GetById( topic.ForumBoard.Id, ctx.owner.obj );

            if (PermissionUtil.IsSelfEdit( ctx, topic ) == false) {
                if (!PermissionUtil.Check( this, board )) return;
            }

            topic = ForumValidator.ValidateTopicEdit( topic, ctx );
            if (ctx.HasErrors) {
                run( Edit, id );
            }
            else {
                topicService.Update( topic, (User)ctx.viewer.obj, ctx.owner.obj );
                new ForumCacheRemove( boardService, topicService, this ).UpdateTopic( topic );
                echoRedirect( lang( "opok" ), alink.ToAppData( topic ) );
            }
        }



        private void setCategory( int id ) {
            List<ForumCategory> categories = categoryService.GetByBoard( id );
            if (categories.Count > 0) {
                categories.Insert( 0, new ForumCategory( 0, alang( "plsSelectCategory" ) ) );
                set( "Category", Html.DropList( categories, "CategoryId", "Name", "Id", ctx.PostInt( "CategoryId" ) ) );
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
