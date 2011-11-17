/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Apps.Forum.Domain;
using wojilu.Members.Interface;
using wojilu.Apps.Forum.Interface;

namespace wojilu.Apps.Forum.Service {


    public class ForumLinkService : IForumLinkService {

        public virtual List<ForumLink> GetByApp( int appId, int ownerId ) {
            return db.find<ForumLink>( "AppId=:appId and OwnerId=:ownerId order by OrderId desc, Id asc" )
                .set( "appId", appId )
                .set( "ownerId", ownerId ).list();
        }

        public virtual ForumLink GetById( int id, IMember owner ) {
            return db.find<ForumLink>( "Id=:id and OwnerId=:ownerId and OwnerType=:type" )
                .set( "id", id )
                .set( "ownerId", owner.Id )
                .set( "type", owner.GetType().FullName )
                .first();
        }

        public virtual Result Insert( ForumLink link ) {
            return db.insert( link );
        }

        public virtual Result Update( ForumLink link ) {
            return db.update( link );
        }

        public virtual void Delete( ForumLink link ) {
            db.delete( link );
        }
    }
}

