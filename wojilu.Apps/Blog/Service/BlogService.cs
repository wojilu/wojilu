/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Interface;
using wojilu.Common.Jobs;
using wojilu.Common.AppBase.Interface;
using wojilu.Members.Users.Domain;

namespace wojilu.Apps.Blog.Service {


    public class BlogService : IBlogService {

        public virtual BlogApp GetFirstByUser(long ownerId) {
            return BlogApp.find( "OwnerId=" + ownerId + " and OwnerType=:OwnerType" )
                .set( "OwnerType", typeof( User ).FullName )
                .first();
        }

        public virtual List<BlogApp> GetBlogAppAll() {
            return BlogApp.findAll();
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

