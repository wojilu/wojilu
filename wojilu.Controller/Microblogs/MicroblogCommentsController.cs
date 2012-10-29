/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;
using wojilu.Members.Users.Domain;
using wojilu.Common.Microblogs.Service;
using wojilu.Common.Microblogs.Domain;
using wojilu.Common.Microblogs.Interface;

namespace wojilu.Web.Controller.Microblogs {

    public partial class MicroblogCommentsController : ControllerBase {

        public IMicroblogService microblogService { get; set; }
        public IFollowerService followService { get; set; }
        public MicroblogCommentService commentService { get; set; }

        public MicroblogCommentsController() {
            microblogService = new MicroblogService();
            followService = new FollowerService();
            commentService = new MicroblogCommentService();

            LayoutControllerType = typeof( MicroblogController );
        }


        [Login]
        public void Show( int id ) {

            commentList( id );
            commentForm( id );

            set( "blog.Id", id );
        }

        public void commentList( int id ) {

            int thisPageCount = 10;
            List<MicroblogComment> comments = commentService.GetTop( id, thisPageCount );

            Microblog blog = microblogService.GetById( id );
            if (blog == null) {
                echoText( "<div class=\"strong red\">本数据已被删除，无法评论</div>" );
                return;
            }

            int restCount = blog.Replies - thisPageCount;


            bindOneBlog( this.utils.getCurrentView(), blog, comments );

            String moreInfo = "";
            if (restCount > 0) {
                String lnk = to( new MicroblogController().Show, id );
                moreInfo = string.Format( "后面还有{0}条评论，<a id=\"moreLink\" href=\"{1}\" to=\"{1}\" target=\"_blank\">点击查看>></a>", restCount, lnk );
            }

            set( "moreLink", moreInfo );

        }

        public void commentForm( int id ) {
            target( SaveReply );
            set( "c.RootId", id );
            set( "viewer.PicSmall", ctx.viewer.obj.PicSmall );

            int parentPanelId = ctx.GetInt( "parentPanelId" );
            if (parentPanelId <= 0) parentPanelId = id;
            set( "parentPanelId", parentPanelId );

            MicroblogComment c = ctx.GetItem( "lastComment" ) as MicroblogComment;
            int parentId = c == null ? 0 : c.Id;
            set( "c.ParentId", parentId );
        }


        private void bindOneBlog( IBlock block, Microblog blog, List<MicroblogComment> comments ) {

            block.Set( "blog.Id", blog.Id );
            block.Set( "blog.Content", blog.Content );
            block.Set( "blog.Created", blog.Created );

            IBlock cblock = block.GetBlock( "comments" );
            bindComments( cblock, comments );
        }

        private void bindComments( IBlock cblock, List<MicroblogComment> clist ) {

            foreach (MicroblogComment c in clist) {

                cblock.Set( "user.Face", c.User.PicSmall );
                cblock.Set( "user.Link", toUser( c.User ) );
                cblock.Set( "user.Name", c.User.Name );


                cblock.Set( "comment.Id", c.Id );
                cblock.Set( "comment.RootId", c.Root.Id );
                cblock.Set( "comment.Content", c.Content );
                cblock.Set( "comment.Created", c.Created );

                cblock.Set( "comment.Indent", 10 );

                cblock.Next();
            }


            if (clist.Count > 0) {
                ctx.SetItem( "lastComment", clist[clist.Count - 1]);
            }

        }

        // 弹窗中的回复窗口
        public void Reply( int id ) {
            int parentId = ctx.GetInt( "parentId" );
            set( "c.ParentId", parentId );
            set( "c.RootId", id );
            target( SaveComment, id );

            MicroblogComment c = commentService.GetById( parentId );
            String content = "//@" + c.User.Name + ":" + c.Content;
            set( "content", content );
        }

        // 保存弹窗中的评论，没有转发
        [HttpPost, DbTransaction]
        public void SaveComment( int id ) {

            if (ctx.viewer.IsLogin == false) {
                echoRedirect( lang( "exPlsLogin" ) );
                return;
            }

            String content = ctx.Post( "content" );
            if (strUtil.IsNullOrEmpty( content )) {
                echoError( lang( "exContent" ) );
                return;
            }
            content = strUtil.CutString( content, MicroblogComment.ContentMax );


            int rootId = saveCommentPrivate( content );

            echoToParent( lang( "opok" ), to( new MicroblogController().Show, id ) );

        }

        // 微博下的直接评论，带转发
        [HttpPost, DbTransaction]
        public void SaveReply( ) {

            if (ctx.viewer.IsLogin == false) {
                echoRedirect( lang( "exPlsLogin" ) );
                return;
            }

            String content = ctx.Post( "content" );
            if (strUtil.IsNullOrEmpty( content )) {
                echoError( lang( "exContent" ) );
                return;
            }
            content = strUtil.CutString( content, MicroblogComment.ContentMax );

            int isRepost = ctx.PostIsCheck( "isRepost" );

            int rootId = saveCommentPrivate( content );

            if (isRepost == 1) {

                Microblog blog = new Microblog();
                blog.Content = content;
                blog.ParentId = rootId; // 转发微博
                blog.User = ctx.viewer.obj as User;
                blog.Ip = ctx.Ip;

                microblogService.Insert( blog );
            }

            Template t = base.utils.getTemplateByAction( "CommentResult" );
            t.Set( "user.Face", ctx.viewer.obj.PicSmall );
            t.Set( "user.Name", ctx.viewer.obj.Name );
            t.Set( "user.Link", toUser( ctx.viewer.obj ) );

            t.Set( "created", cvt.ToTimeString( DateTime.Now ) );
            t.Set( "content", content );

            String msg = t.ToString();

            echoHtmlTo( "blogComments" + rootId, msg );
        }

        private int saveCommentPrivate( String content ) {

            int rootId = ctx.PostInt( "rootId" );
            int parentId = ctx.PostInt( "parentId" );

            Microblog blog = microblogService.GetById( rootId );

            MicroblogComment c = new MicroblogComment();
            c.Root = blog;
            c.ParentId = parentId;
            c.User = (User)ctx.viewer.obj;
            c.Ip = ctx.Ip;
            c.Content = content;

            String microblogLink = Link.To( blog.User, new MicroblogController().Show, blog.Id );
            commentService.InsertComment( c, microblogLink );
            return rootId;
        }




    }
}
