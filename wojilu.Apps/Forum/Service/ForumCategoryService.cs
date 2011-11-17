/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Apps.Forum.Domain;
using wojilu.Members.Interface;
using wojilu.Apps.Forum.Interface;

namespace wojilu.Apps.Forum.Service {


    public class ForumCategoryService : IForumCategoryService {

        public virtual List<ForumCategory> GetByBoard( int forumBoardId ) {
            return db.find<ForumCategory>( "BoardId=" + forumBoardId + " order by OrderId desc, Id asc" ).list();
        }

        public virtual ForumCategory GetById( int id, IMember owner ) {
            return db.find<ForumCategory>( "Id=:id and OwnerId=:ownerId and OwnerType=:type" )
                .set( "id", id )
                .set( "ownerId", owner.Id )
                .set( "type", owner.GetType().FullName )
                .first();
        }

        public virtual List<ForumCategory> GetDropList( int boardId ) {
            List<ForumCategory> results = this.GetByBoard( boardId );
            ForumCategory category = new ForumCategory();
            category.Name = alang.get( typeof( ForumApp ), "plsSelectCategory" );
            results.Insert( 0, category );
            return results;
        }

        public virtual Result Insert( ForumCategory category ) {
            return db.insert( category );
        }

        public virtual Result Update( ForumCategory category ) {
            return db.update( category );
        }

        public virtual int CountByBoard( int boardId ) {
            return db.count<ForumCategory>( "BoardId=" + boardId );
        }

        public virtual void Delete( ForumCategory category ) {
            db.delete( category );
        }

    }
}

