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


namespace wojilu.Web.Controller.Forum.Edits {

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

        public void Edit( int id ) {

            target( Update, id );

            ForumTopic topic = topicService.GetById( id, ctx.owner.obj );
            ForumPost post = postService.GetPostByTopic( topic.Id );
            ForumBoard board = getTree().GetById( topic.ForumBoard.Id );

            List<ForumBoard> pathboards = getTree().GetPath( board.Id );
            set( "location", ForumLocationUtil.GetTopicEdit( pathboards, topic, ctx ) );

            setCategory( topic, board );
            set( "post.Title", post.Title );
            set( "post.TagList", topic.Tag.TextString );

            editor( "Content", post.Content, "300px" );

            set( "attachmentLink", to( new Edits.AttachmentController().Admin, id ) );
        }

        [HttpPost, DbTransaction]
        public void Update( int id ) {

            ForumTopic topic = topicService.GetById( id, ctx.owner.obj );
            ForumBoard board = getTree().GetById( topic.ForumBoard.Id );


            topic = ForumValidator.ValidateTopicEdit( topic, ctx );
            if (ctx.HasErrors) {
                run( Edit, id );
            }
            else {

                if (ctx.PostIsCheck( "saveContentPic" ) == 1) {
                    topic.Content = wojilu.Net.PageLoader.ProcessPic( topic.Content, null );
                }

                topicService.Update( topic, (User)ctx.viewer.obj, ctx.owner.obj );
                new ForumCacheRemove( boardService, topicService, this ).UpdateTopic( topic );
                echoRedirect( lang( "opok" ), alink.ToAppData( topic ) );
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


        private Tree<ForumBoard> _tree;

        private Tree<ForumBoard> getTree() {
            if (_tree == null) _tree = new Tree<ForumBoard>( boardService.GetBoardAll( ctx.app.Id, ctx.viewer.IsLogin ) );
            return _tree;
        }
    }

}
