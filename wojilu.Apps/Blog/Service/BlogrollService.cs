/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Interface;

namespace wojilu.Apps.Blog.Service {


    public class BlogrollService : IBlogrollService {


        public virtual List<Blogroll> GetByApp( int appId, int ownerId ) {
            return db.find<Blogroll>( "OwnerId=" + ownerId + " and AppId=" + appId + " order by OrderId desc" ).list();
        }

        public virtual Blogroll GetById( int id, int appId ) {
            Blogroll blogroll = db.findById<Blogroll>( id );
            if ((blogroll != null) && (blogroll.AppId != appId)) return null;
            return blogroll;
        }

        public virtual void Insert( Blogroll roll, int ownerId, int appId ) {
            roll.AppId = appId;
            roll.OwnerId = ownerId;
            db.insert( roll );
        }

        public virtual void Update( Blogroll roll ) {
            db.update( roll );
        }

        public virtual void Delete( Blogroll roll ) {
            db.delete( roll );
        }

    }
}

