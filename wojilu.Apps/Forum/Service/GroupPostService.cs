/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Apps.Forum.Domain;
using wojilu.Members.Groups.Domain;
using wojilu.Apps.Forum.Interface;

namespace wojilu.Apps.Forum.Service {

    public class GroupPostService : IGroupPostService {

        public virtual List<ForumPost> GetRecent( int count ) {

            if (count <= 0) count = 10;

            return db.find<ForumPost>( "OwnerType=:otype" ).set("otype", t()).list( count );
        }

        public virtual DataPage<ForumPost> GetPostAll( String condition ) {
            return db.findPage<ForumPost>( "OwnerType='" + t() + "' " + condition );
        }

        public virtual DataPage<ForumTopic> GetTopicAll( String condition ) {

            return db.findPage<ForumTopic>( "OwnerType='" + t() + "' " + condition );
        }

        public virtual List<ForumTopic> GetMyTopic( int userId, String groupIds, int count ) {

            if (count <= 0) count = 10;

            if (strUtil.IsNullOrEmpty( groupIds )) return new List<ForumTopic>();

            String topicTbl = Entity.GetInfo( typeof( ForumTopic ) ).TableName;
            String groupTbl = Entity.GetInfo( typeof( Group ) ).TableName;

            String sql = string.Format( "select top {0} a.* from " + topicTbl + " a, " + groupTbl + " b where a.OwnerId=b.Id and a.OwnerType='{1}' and b.Id in({2}) order by a.Replied desc, a.Id desc", count, t(), groupIds );

            return db.findBySql<ForumTopic>( sql );
        }

        public virtual DataPage<ForumTopic> GetMyTopicPage( int userId, String groupIds, int count ) {

            if (count <= 0) count = 10;

            if (strUtil.IsNullOrEmpty( groupIds )) return DataPage<ForumTopic>.GetEmpty();

            String topicTbl = Entity.GetInfo( typeof( ForumTopic ) ).TableName;
            String groupTbl = Entity.GetInfo( typeof( Group ) ).TableName;

            String sql = string.Format( "select top {0} a.* from " + topicTbl + " a, " + groupTbl + " b where a.OwnerId=b.Id and a.OwnerType='{1}' and b.Id in({2}) order by a.Replied desc, a.Id desc", count, t(), groupIds );

            return db.findPage<ForumTopic>( sql );
        }

        public virtual List<ForumTopic> GetHotTopic( int count ) {
            if (count <= 0) count = 10;
            //return db.find<ForumTopic>( "OwnerType=:otype order by Replies desc, Id desc" ).set( "otype", t() ).list( count );

            String topicTbl = Entity.GetInfo( typeof( ForumTopic ) ).TableName;
            String groupTbl = Entity.GetInfo( typeof( Group ) ).TableName;

            String sql = string.Format( "select top {0} a.* from " + topicTbl + " a, " + groupTbl + " b where a.OwnerId=b.Id and a.OwnerType='{1}' and b.AccessStatus<{2} order by a.Replies desc, a.Id desc", count, t(), GroupAccessStatus.Secret );

            return db.findBySql<ForumTopic>( sql );
        }

        private String t() {
            return typeof( Group ).FullName;
        }

    }

}
