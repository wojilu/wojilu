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


namespace wojilu.Web.Controller.Forum.Edits {

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


        public void Edit( int id ) {

            ForumPost post = postService.GetById( id, ctx.owner.obj );
            ForumBoard board = getTree().GetById( post.ForumBoardId );

            target( Update, post.Id );

            List<ForumBoard> pathboards = getTree().GetPath( board.Id );
            set( "location", ForumLocationUtil.GetPostEdit( pathboards, post, ctx ) );

            set( "post.Title", post.Title );
            set( "Content", post.Content );
        }

        [HttpPost, DbTransaction]
        public void Update( int id ) {

            ForumPost post = postService.GetById( id, ctx.owner.obj );
            ForumBoard board = boardService.GetById( post.ForumBoardId, ctx.owner.obj );

            post = ForumValidator.ValidatePostEdit( post, ctx );
            if (errors.HasErrors) {
                run( Edit, id );
            }
            else {
                postService.Update( post, (User)ctx.viewer.obj );

                String parentUrl = getTopicLastPage( post );
                echoToParent( lang( "opok" ), parentUrl );
            }
        }

        private static Random rd = new Random();

        private string getTopicLastPage( ForumPost post ) {
            String lnk = to( new wojilu.Web.Controller.Forum.TopicController().Show, post.TopicId );
            int pageNo = postService.GetPageCount( post.TopicId, getPageSize( ctx.app.obj ) );
            lnk = PageHelper.AppendNo( lnk, pageNo );

            lnk = lnk + "?rd=" + getRandomStr() + "#post" + post.Id;

            return lnk;
        }

        private String getRandomStr() {
            String strRandom = rd.Next( 10000, 20000 ).ToString();
            DateTime now = DateTime.Now;
            return now.Hour.ToString() + now.Minute.ToString() + now.Second.ToString() + now.Millisecond.ToString() + strRandom;
        }

        private int getPageSize( object app ) {
            return ((ForumApp)app).GetSettingsObj().ReplySize;
        }



        private Tree<ForumBoard> _tree;

        private Tree<ForumBoard> getTree() {
            if (_tree == null) _tree = new Tree<ForumBoard>( boardService.GetBoardAll( ctx.app.Id, ctx.viewer.IsLogin ) );
            return _tree;
        }

    }

}
