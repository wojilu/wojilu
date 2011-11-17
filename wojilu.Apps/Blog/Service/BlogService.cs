/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Interface;
using wojilu.Common.Jobs;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Apps.Blog.Service {


    public class BlogService : IBlogService {

        public virtual List<BlogApp> GetBlogAppAll() {
            return BlogApp.findAll();
        }

        public virtual void UpdateCommentCount( IApp blogApp ) {

            BlogApp blog = blogApp as BlogApp;
            int count = BlogPostComment.find( "AppId=" + blog.Id ).count();
            blog.CommentCount = count;
            blog.update( "CommentCount" );
        }

        public virtual void UpdateCommentCount( int appId ) {

            BlogApp blogApp = BlogApp.findById( appId );
            this.UpdateCommentCount( blogApp );
        }

        public virtual void UpdateCount( BlogApp blog ) {

            int count = BlogPost.count( "AppId=" + blog.Id );
            blog.BlogCount = count;
            blog.update( "BlogCount" );
        }

        public virtual void AddHits( BlogApp app ) {
            HitsJob.Add( app );
        }



    }
}

