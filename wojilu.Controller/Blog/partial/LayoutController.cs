/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Common.Comments;

using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Interface;
using wojilu.Apps.Blog.Service;

namespace wojilu.Web.Controller.Blog {

    public partial class LayoutController : ControllerBase {


        private void bindAppInfo( BlogApp blog ) {
            set( "blog.Count", blog.BlogCount );
            set( "blog.Hits", blog.Hits );
            set( "blog.CommentCount", getBlogComments( blog ) );
            set( "blog.RssUrl", Link.To( new BlogController().Rss ) );

            Page.RssLink = Link.To( new BlogController().Rss );

        }

        private object getBlogComments( BlogApp blog ) {

            return BlogPostComment.count( "AppId=" + blog.Id );

            //return blog.CommentCount;
        }

        private void bindCategories( List<BlogCategory> categories ) {
            IBlock catblock = getBlock( "category" );
            foreach (BlogCategory category in categories) {
                catblock.Set( "category.Title", category.Name );
                catblock.Set( "category.Url", Link.To( new CategoryController().Show, category.Id ) );
                catblock.Next();
            }
        }

        private void bindPostList( List<BlogPost> newBlogs ) {
            IBlock postblock = getBlock( "newpost" );
            foreach (BlogPost post in newBlogs) {
                postblock.Set( "post.Title", strUtil.SubString( post.Title, 14 ) );
                postblock.Set( "post.Url", alink.ToAppData( post ) );
                postblock.Next();
            }
        }

        private void bindBlogroll( List<Blogroll> blogrolls ) {
            IBlock rollblock = getBlock( "myBlogroll" );
            foreach (Blogroll blogroll in blogrolls) {
                rollblock.Set( "roll.Name", strUtil.SubString( blogroll.Name, 10 ) );
                rollblock.Set( "roll.Link", blogroll.Link );
                rollblock.Next();
            }

        }

        private void bindComments( List<BlogPostComment> newComments ) {
            IBlock commentblock = getBlock( "comment" );
            foreach (BlogPostComment comment in newComments) {
                commentblock.Set( "comment.Title", strUtil.SubString( comment.Content, 14 ) );
                commentblock.Set( "comment.Url", Link.To( new PostController().Show, comment.RootId ) + "#comments" );
                commentblock.Next();
            }

            String commentMoreLink = BlogCommentController.GetCommentMoreLink( newComments.Count, ctx );
            set( "commentMoreLink", commentMoreLink );
        }

    }

}
