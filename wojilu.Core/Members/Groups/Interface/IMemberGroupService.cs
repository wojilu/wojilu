/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Members.Groups.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Common.Msg.Interface;
using wojilu.Common.Microblogs.Interface;

namespace wojilu.Members.Groups.Interface {

    public interface IMemberGroupService {

        IMicroblogService microblogService { get; set; }
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

        List<Group> GetJoinedGroup(long userId, int count);
        string GetJoinedGroupIds(long userId);

        List<Group> GetGroupByFriends(long userId, int count);
        List<User> GetNewMember(long groupId, int count);
        List<User> GetOfficer(long groupId);
        GroupRole GetUserRole(User user, long groupId);

        DataPage<Group> GetGroupByUser(long userId);
        DataPage<GroupUser> GetMembersAll(long gid);
        DataPage<GroupUser> GetMembersAll(long groupId, long roleId);
        DataPage<GroupUser> GetMembersApproved(long gid);

        bool IsGroupFounder(long userId, long groupId);
        bool IsGroupMember(long userId, long groupId);
        bool IsGroupOfficer(long userId, long groupId);
        bool IsGroupApproving(long userId, long groupId);


        long MemberStatus( User user, long groupId );

        void DeleteUser( Group group, String userIds );



    }

}
