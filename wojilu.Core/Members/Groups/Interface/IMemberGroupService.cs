/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Members.Groups.Domain;
using wojilu.Common.Feeds.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Common.Msg.Interface;

namespace wojilu.Members.Groups.Interface {

    public interface IMemberGroupService {

        IFeedService feedService { get; set; }
        IUserService userService { get; set; }
        IMessageService msgService { get; set; }

        Result JoinGroup( User user, Group group, String joinReason, String ip );
        Result JoinGroupDone( User user, Group group, String joinReason, String ip );
        void JoinCreateGroup( User user, Group group, String ip );

        Result QuitGroup( User user, Group group, string quitReason );

        void AddOfficer( Group group, String userIds );
        void ApproveUser( Group group, String userIds );
        void RemoveOfficer( Group group, String userIds );

        List<Group> FindCreateGroups( User user, int count );
        List<Group> FindJoinedGroups( User user, int count );

        List<Group> GetJoinedGroup( int userId, int count );
        String GetJoinedGroupIds( int userId );

        List<Group> GetGroupByFriends( int userId, int count );
        List<User> GetNewMember( int groupId, int count );
        List<User> GetOfficer( int groupId );
        GroupRole GetUserRole( User user, int groupId );

        DataPage<Group> GetGroupByUser( int userId );
        DataPage<GroupUser> GetMembersAll( int gid );
        DataPage<GroupUser> GetMembersAll( int groupId, int roleId );
        DataPage<GroupUser> GetMembersApproved( int gid );

        Boolean IsGroupFounder( int userId, int groupId );
        Boolean IsGroupMember( int userId, int groupId );
        Boolean IsGroupOfficer( int userId, int groupId );
        Boolean IsGroupApproving( int userId, int groupId );


        int MemberStatus( User user, int groupId );

        void DeleteUser( Group group, String userIds );



    }

}
