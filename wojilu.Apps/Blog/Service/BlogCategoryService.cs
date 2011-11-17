/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Interface;
using wojilu.Common.AppBase;

namespace wojilu.Apps.Blog.Service {

    public class BlogCategoryService : IBlogCategoryService {

        public virtual List<BlogCategory> GetByApp( int appId ) {
            return db.find<BlogCategory>( "AppId=" + appId + " order by OrderId desc, Id asc" ).list();
        }

        public virtual BlogCategory GetById( int id, int ownerId ) {
            BlogCategory result = db.findById<BlogCategory>( id );
            if (result.OwnerId != ownerId ) return null;
            return result;
        }

        public virtual void Insert( BlogCategory category ) {
            db.insert( category );
        }

        public virtual void Delete( BlogCategory category ) {

            db.updateBatch<BlogPost>( "SaveStatus=" + SaveStatus.Delete, "CategoryId=" + category.Id );
            db.delete( category );
        }

        public virtual void RefreshCache(BlogCategory category) {
        }

    }
}

