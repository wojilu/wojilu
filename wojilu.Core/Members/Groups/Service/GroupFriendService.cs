/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Members.Groups.Domain;
using wojilu.Members.Interface;
using wojilu.Members.Groups.Interface;

namespace wojilu.Members.Groups.Service {

    public class GroupFriendService : IGroupFriendService {

        public virtual List<Group> GetFriends( int groupId, int count ) {
            List<GroupFriends> list = db.find<GroupFriends>( "Group.Id=" + groupId ).list( count );
            return populate( list );
        }

        private List<Group> populate( List<GroupFriends> list ) {
            List<Group> results = new List<Group>();
            foreach (GroupFriends gf in list) {
                if (gf.Friend == null || gf.Friend.Id <= 0) continue;
                results.Add( gf.Friend );
            }
            return results;
        }

        public virtual DataPage<Group> GetPage( int groupId, int pageSize ) {

            DataPage<GroupFriends> list = db.findPage<GroupFriends>( "Group.Id=" + groupId, pageSize );
            DataPage<Group> page = new DataPage<Group>( list );
            page.Results = populate( list.Results );
            return page;
        }


        public virtual Result AddFriend( IMember group, String name ) {

            Group friend = db.find<Group>( "Name=:name" )
                .set( "name", name )
                .first() as Group;

            if (friend == null) throw new Exception( lang.get( "exFriendGroupNotFound" ) );

            GroupFriends gf = GetFriend( group.Id, friend.Id );
            if (gf != null) {
                return new Result( lang.get( "exFriendGroupAdded" ) );
            }

            GroupFriends friends = new GroupFriends();
            friends.Group = group as Group;
            friends.Friend = friend;

            return db.insert( friends );
        }

        public virtual GroupFriends GetFriend( int groupId, int friendId ) {
            return db.find<GroupFriends>( "Group.Id=" + groupId + " and Friend.Id=" + friendId ).first();
        }


        public virtual void Delete( GroupFriends gf ) {
            db.delete( gf );
        }
    }

}
