/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

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

namespace wojilu.Web.Controller.Forum {

    [App( typeof( ForumApp ) )]
    public partial class PostController : ControllerBase {

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

        public override void CheckPermission() {

            ForumPost post = postService.GetById( ctx.route.id, ctx.owner.obj );
            if (post == null) {
                echo( alang( "exPostNotFound" ) );
                return;
            }

            ForumBoard board = getTree().GetById( post.ForumBoardId );
            if (board == null) {
                echo( alang( "exBoardNotFound" ) );
                return;
            }

            SecurityHelper.Check( this, board );

        }

        private Tree<ForumBoard> _tree;

        private Tree<ForumBoard> getTree() {
            if (_tree == null) _tree = new Tree<ForumBoard>( boardService.GetBoardAll( ctx.app.Id, ctx.viewer.IsLogin ) );
            return _tree;
        }

        public void Show( int id ) {

            ForumPost post = postService.GetById( id, ctx.owner.obj );
            ForumBoard board = getTree().GetById( post.ForumBoardId );

            postService.AddHits( post );

            ForumTopic topic = topicService.GetById( post.TopicId, ctx.owner.obj );
            if (topic == null) {
                echo( alang( "exTopicNotFound" ) );
                return;
            }

            topicService.AddHits( topic );

            ctx.Page.SetTitle( post.Title, board.Name );

            List<ForumBoard> pathboards = getTree().GetPath( post.ForumBoardId );
            set( "location", ForumLocationUtil.GetPost( pathboards, post, ctx ) );

            List<ForumPost> posts = new List<ForumPost>();
            posts.Add( post );
            List<Attachment> attachments = attachService.GetByPost( post.Id );

            ctx.SetItem( "forumBoard", board );
            ctx.SetItem( "forumTopic", topic );
            ctx.SetItem( "posts", posts );
            ctx.SetItem( "attachs", attachments );
            ctx.SetItem( "pageSize", -1 );
            load( "currentPost", new TopicController().PostLoop );

            set( "topic.Url", to( new TopicController().Show, post.TopicId ) );

            set( "moderatorJson", moderatorService.GetModeratorJson( board ) );
            set( "creatorId", topic.Creator.Id );
            set( "tagAction", to( new Edits.TagController().SaveTag, topic.Id ) );

            DataPage<ForumPost> replyList = postService.GetPageList( post.TopicId, 200, 0 );

            bindReplyList( replyList, post.Id );
        }


        private void bindReplyList( DataPage<ForumPost> results, int currentPostId ) {

            IBlock block = getBlock( "replypost" );
            List<ForumPost> replyList = results.Results;
            foreach (ForumPost post in replyList) {

                if (post.Creator == null) continue;
                block.Set( "p.MemberUrl", toUser( post.Creator ) );
                block.Set( "p.MemberName", post.Creator.Name );

                block.Set( "p.Title", post.Title );
                block.Set( "p.Length", post.Content.Length );
                block.Set( "p.CreateTime", post.Created );
                block.Set( "p.Hits", post.Hits );

                String plink = post.ParentId == 0 ? to( new TopicController().Show, post.TopicId ) : alink.ToAppData( post );
                block.Set( "p.Url", plink );

                String pclass = post.Id == currentPostId ? "red strong" : "";
                block.Set( "p.Class", pclass );


                block.Next();
            }

            String page = results.PageCount > 1 ? results.PageBar : "";
            set( "replyPage", page );
        }



    }
}

