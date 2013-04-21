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
            set( "blog.RssUrl", to( new BlogController().Rss ) );

            Page.RssLink = to( new BlogController().Rss );

        }

        private int getBlogComments( BlogApp blog ) {
            return blog.CommentCount;
        }

        private void bindCategories( List<BlogCategory> categories ) {
            IBlock catblock = getBlock( "category" );
            foreach (BlogCategory category in categories) {
                catblock.Set( "category.Title", category.Name );
                catblock.Set( "category.Url", to( new CategoryController().Show, category.Id ) );
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

        private void bindComments( List<OpenComment> comments ) {
            IBlock block = getBlock( "comment" );
            foreach (OpenComment x in comments) {
                block.Set( "comment.Title", strUtil.SubString( x.Content, 14 ) );
                block.Set( "comment.Url", to( new PostController().Show, x.TargetDataId ) + "#comments" );
                block.Next();
            }

            String lnkMore = BlogCommentController.GetMoreLink( comments.Count, ctx );
            set( "commentMoreLink", lnkMore );
        }

    }

}
