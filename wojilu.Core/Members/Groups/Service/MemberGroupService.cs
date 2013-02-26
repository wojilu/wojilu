/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;

using wojilu.Common.Feeds.Domain;
using wojilu.Common.Feeds.Interface;
using wojilu.Common.Feeds.Service;
using wojilu.Common.Msg.Interface;
using wojilu.Common.Msg.Service;

using wojilu.Members.Groups.Domain;
using wojilu.Members.Groups.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;

namespace wojilu.Members.Groups.Service {

    public class MemberGroupService : IMemberGroupService {

        public virtual IMessageService msgService { get; set; }
        public virtual IUserService userService { get; set; }
        public virtual IFeedService feedService { get; set; }

        public MemberGroupService() {
            msgService = new MessageService();
            userService = new UserService();
            feedService = new FeedService();
        }

        public virtual Result JoinGroup( User user, Group group, String joinReason, String ip ) {

            GroupUser gu = db.find<GroupUser>( "Member.Id=" + user.Id + " and Group.Id=" + group.Id ).first();

            if (gu == null) {

                gu = new GroupUser();
                gu.Member = user;
                gu.Group = group;
                gu.Msg = joinReason;
                gu.Ip = ip;

                gu.Status = GroupRole.GetInitRoleByGroup( group );

                Result addResult = db.insert( gu );

                if (group.AccessStatus == GroupAccessStatus.Open) {
                    afterJoinDone( user, group, joinReason, gu );
                }
                else {
                    addApprovingMsg( user, group, joinReason ); // 给管理员发消息
                }

                return addResult;
            }
            else {

                Result result = new Result();
                if (gu.Status == GroupRole.Blacklist.Id) {
                    result.Add( lang.get( "exGroupBeBlacklist" ) );
                }
                else if (gu.Status == GroupRole.Approving.Id) {
                    result.Add( lang.get( "exGroupApprovingTip" ) );
                }
                else
                    result.Add( lang.get( "exGroupBeMember" ) );

                return result;

            }
        }

        // 直接加为群组成功，不用审核
        public virtual Result JoinGroupDone( User user, Group group, String joinReason, String ip ) {

            GroupUser gu = db.find<GroupUser>( "Member.Id=" + user.Id + " and Group.Id=" + group.Id ).first();

            Result result;

            if (gu == null) {

                gu = new GroupUser();
                gu.Member = user;
                gu.Group = group;
                gu.Msg = joinReason;
                gu.Ip = ip;

                result = db.insert( gu );
            }
            else {
                gu.Status = GroupRole.Member.Id;
                gu.Msg = joinReason;

                result = gu.update();
            }

            afterJoinDone( user, group, joinReason, gu );

            return result;
        }

        private void afterJoinDone( User user, Group group, String joinReason, GroupUser gu ) {
            recountMembers( group ); // 重新统计成员数量
            addFeedInfo( gu, gu.Ip ); // 将信息加入用户的新鲜事
            sendOfficerMsg( gu, user, joinReason ); // 告知群组管理员
        }

        private void sendOfficerMsg( GroupUser gu, User user, string joinReason ) {

            Group group = gu.Group;

            String msg = string.Format( "用户 {0} 加入了群组 {1}", user.Name, group.Name );
            String body = string.Format( "用户 <a href=\"{1}\" target=\"_blank\">{0}</a> 加入了群组 <a href=\"{3}\" target=\"_blank\">{2}</a>", user.Name, Link.ToMember( user ), group.Name, Link.ToMember( group ) );
            if (strUtil.HasText( joinReason )) {
                body += "<br/>加入信息：" + joinReason;
            }

            List<User> officerList = GetOfficer( group.Id );
            foreach (User officer in officerList) {
                msgService.SiteSend( msg, body, officer );
            }
        }

        private void addApprovingMsg( User user, Group group, String joinReason ) {

            String lnkAdmin = getMemberAdminUrl( group );
            String msg = string.Format( lang.get( "groupApprovingInfo" ), user.Name, group.Name );
            String body = string.Format( lang.get( "groupApprovingBodyInfo" ),
                Link.ToMember( user ), user.Name, lnkAdmin, group.Name );

            if (strUtil.HasText( joinReason )) {
                body += "<br/>申请留言：" + joinReason;
            }

            List<User> officerList = GetOfficer( group.Id );

            foreach (User officer in officerList) {
                msgService.SiteSend( msg, body, officer );
            }
        }

        private String getMemberAdminUrl( Group group ) {

            String groupUrlWithoutExt = strUtil.TrimEnd( Link.ToMember( group ), MvcConfig.Instance.UrlExt );

            return strUtil.Join( groupUrlWithoutExt, "/Groups/Admin/Main/Members" + MvcConfig.Instance.UrlExt );
        }

        private void addFeedInfo( GroupUser relation, String ip ) {
            Feed feed = new Feed();
            feed.Creator = relation.Member;
            feed.DataType = typeof( Group ).FullName;

            feed.TitleTemplate = "{*actor*} " + lang.get( "joinedGroup" ) + " {*group*}";
            feed.TitleData = getTitleData( relation.Group );
            feed.Ip = ip;
            feedService.publishUserAction( feed );
        }

        private String getTitleData( Group g ) {
            String lnk = string.Format( "<a href=\"{0}\">{1}</a>", Link.ToMember( g ), g.Name );
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add( "group", lnk );
            return Json.ToString( dic );
        }

        public virtual void JoinCreateGroup( User user, Group group, String ip ) {

            int users = db.find<GroupUser>( "Member.Id=" + user.Id + " and Group.Id=" + group.Id + " and IsFounder=1" ).count();
            if (users > 0) return;

            GroupUser gu = new GroupUser();

            gu.Member = user;
            gu.Group = group;
            gu.IsFounder = 1;
            gu.Status = GroupRole.Administrator.Id;
            gu.Ip = ip;

            db.insert( gu );

            recountMembers( group );
        }

        public virtual void ApproveUser( Group group, String userIds ) {

            int[] ids = cvt.ToIntArray( userIds );
            foreach (int userId in ids) {

                GroupUser gu = db.find<GroupUser>( getCondition( group.Id, userId ) ).first();
                if (gu == null) continue;

                // 过滤掉已有成员
                if (gu.Status == GroupRole.Administrator.Id || gu.Status == GroupRole.Member.Id) continue;

                gu.Status = GroupRole.Member.Id;
                db.update( gu, "Status" );

                // 给申请者发送消息
                String msg = string.Format( lang.get( "groupMemberJoinedMsg" ) + "“{0}”", group.Name );
                String body = string.Format( lang.get( "groupMemberJoinedMsg" ) + "“<a href=\"{1}\" target=\"_blank\">{0}</a>”",
                    group.Name, Link.ToMember( group ) );

                User receiver = userService.GetById( userId );
                msgService.SiteSend( msg, body, receiver );

                addFeedInfo( gu, gu.Ip );
            }

            recountMembers( group );
        }

        //----------------------------------------------------------------------------------------------------------------------

        public virtual void AddOfficer( Group group, String userIds ) {

            int[] ids = cvt.ToIntArray( userIds );
            foreach (int userId in ids) {

                GroupUser gu = db.find<GroupUser>( getCondition( group.Id, userId ) ).first();
                if (gu == null) continue;

                if (gu.Status == GroupRole.Administrator.Id) continue;

                gu.Status = GroupRole.Administrator.Id;
                db.update( gu, "Status" );

                String msg = string.Format( lang.get( "groupAdminSettedMsg" ) + ":“{0}”", group.Name );
                String body = string.Format( lang.get( "groupAdminSettedMsg" ) + ":“<a href='{1}'>{0}</a>”",
                    group.Name, Link.ToMember( group ) );

                User receiver = userService.GetById( userId );
                msgService.SiteSend( msg, body, receiver );
            }

            // 有可能将待审核的用户直接加为管理员，所以需要统计
            recountMembers( group );
        }

        public virtual void RemoveOfficer( Group group, String userIds ) {

            int[] ids = cvt.ToIntArray( userIds );
            foreach (int userId in ids) {

                GroupUser gu = db.find<GroupUser>( getCondition( group.Id, userId ) ).first();
                if (gu == null) continue;

                if (gu.Status != GroupRole.Administrator.Id) continue;

                gu.Status = GroupRole.Member.Id;
                db.update( gu, "Status" );

                String msg = string.Format( lang.get( "groupAdminCanceledMsg" ) + ":“{0}”", group.Name );
                String body = string.Format( lang.get( "groupAdminCanceledMsg" ) + ":“<a href='{1}'>{0}</a>”",
                    group.Name, Link.ToMember( group ) );

                User receiver = userService.GetById( userId );
                msgService.SiteSend( msg, body, receiver );
            }
        }

        //----------------------------------------------------------------------------------------------------------------------


        // 管理员是不能直接删除的，必须先降级为普通成员，然后才能删除
        public virtual void DeleteUser( Group group, String userIds ) {

            int[] ids = cvt.ToIntArray( userIds );
            foreach (int userId in ids) {

                GroupUser gu = db.find<GroupUser>( getCondition( group.Id, userId ) ).first();
                if (gu == null || gu.Status == GroupRole.Administrator.Id) continue;

                db.delete( gu );
            }
            recountMembers( group );
        }

        // 如果是最后一个管理员，则不能退出群组
        public virtual Result QuitGroup( User user, Group group, string quitReason ) {

            GroupUser relation = db.find<GroupUser>( "Member.Id=" + user.Id + " and Group.Id=" + group.Id ).first();
            if (relation == null) {
                return new Result( lang.get( "exNotGroupMember" ) );
            }

            if (relation.Status == GroupRole.Administrator.Id) {
                List<User> officers = GetOfficer( group.Id );
                if (officers.Count == 1) {
                    return new Result( lang.get( "exQuitGroup" ) );
                }
            }

            db.delete( relation );
            recountMembers( group );
            sendQuitMsg( user, group, quitReason );

            return new Result();
        }

        private void sendQuitMsg( User user, Group group, string quitReason ) {

            String msg = string.Format( "用户 {0} 退出了群组 {1}", user.Name, group.Name );
            String body = string.Format( "用户 <a href=\"{1}\" target=\"_blank\">{0}</a> 退出了群组 <a href=\"{3}\" target=\"_blank\">{2}</a>", user.Name, Link.ToMember( user ), group.Name, Link.ToMember( group ) );
            if (strUtil.HasText( quitReason )) {
                body += "<br/>退出原因：" + quitReason;
            }

            List<User> officerList = GetOfficer( group.Id );
            foreach (User officer in officerList) {
                msgService.SiteSend( msg, body, officer );
            }

        }

        //----------------------------------------------------------------------------------------------------------------------

        public virtual List<User> GetNewMember( int groupId, int count ) {
            List<GroupUser> list = db.find<GroupUser>( getMembersCondition( groupId ) ).list( count );
            List<User> results = new List<User>();
            foreach (GroupUser mgr in list) {
                results.Add( mgr.Member );
            }
            return results;
        }

        public virtual List<User> GetOfficer( int groupId ) {
            List<GroupUser> mgrList = db.find<GroupUser>( "Group.Id=" + groupId + " and Status=" + GroupRole.Administrator.Id ).list();
            List<User> results = new List<User>();
            foreach (GroupUser mgr in mgrList) results.Add( mgr.Member );
            return results;
        }


        //----------------------------------------------------------------------------------------------------------------------

        public virtual List<Group> FindCreateGroups( User user, int count ) {
            List<GroupUser> list = db.find<GroupUser>( "Member.Id=" + user.Id + " and IsFounder=1" ).list( count );
            return populate( list );
        }

        public virtual List<Group> FindJoinedGroups( User user, int count ) {
            List<GroupUser> list = db.find<GroupUser>( "Member.Id=" + user.Id ).list( count );
            return populate( list );
        }

        public virtual String GetJoinedGroupIds( int userId ) {

            List<GroupUser> lists = GroupUser.find( myGroupCondition( userId ) + " order by LastUpdateTime desc" ).list();
            String ids = "";
            foreach (GroupUser gu in lists) {
                if (gu.Group == null) continue;
                ids += gu.Group.Id + ",";
            }
            return ids.TrimEnd( ',' );
        }

        public virtual List<Group> GetJoinedGroup( int userId, int count ) {

            if (count <= 0) count = 10;

            List<GroupUser> list = GroupUser.find( myGroupCondition( userId ) + " order by LastUpdateTime desc" ).list( count );
            return populate( list );
        }

        public virtual DataPage<Group> GetGroupByUser( int userId ) {

            DataPage<GroupUser> lists = db.findPage<GroupUser>( myGroupCondition( userId ) + " order by LastUpdateTime desc" );
            List<Group> groups = populate( lists.Results );

            return lists.Convert<Group>( groups );
        }

        private List<Group> populate( List<GroupUser> lists ) {
            List<Group> results = new List<Group>();
            foreach (GroupUser gu in lists) {
                if (gu.Group == null) continue;
                results.Add( gu.Group );
            }
            return results;
        }

        public virtual List<Group> GetGroupByFriends( int userId, int count ) {

            FriendService fs = new FriendService();
            String fIds = fs.FindFriendsIds( userId );
            if (strUtil.IsNullOrEmpty( fIds )) return new List<Group>();

            List<GroupUser> guList = db.find<GroupUser>( "Member.Id in ( " + fIds + " ) " ).list();
            List<GroupUser> myList = db.find<GroupUser>( myGroupCondition( userId ) ).list();

            List<Group> results = new List<Group>();

            foreach (GroupUser gu in guList) {
                if (gu.Group == null) continue;
                if (isInMyGroups( myList, gu.Group )) continue;
                if (isGroupAdded( results, gu.Group )) continue;
                results.Add( gu.Group );
            }
            return results;
        }



        private Boolean isInMyGroups( List<GroupUser> myList, Group group ) {
            foreach (GroupUser gu in myList) {
                if (gu.Group == null) continue;
                if (gu.Group.Id == group.Id) return true;
            }
            return false;
        }

        private Boolean isGroupAdded( List<Group> results, Group gu ) {
            foreach (Group g in results) {
                if (g.Id == gu.Id) return true;
            }
            return false;
        }

        public virtual DataPage<GroupUser> GetMembersAll( int gid ) {
            return db.findPage<GroupUser>( "Group.Id=" + gid );
        }

        public virtual DataPage<GroupUser> GetMembersAll( int gid, int roleId ) {
            return db.findPage<GroupUser>( "Group.Id=" + gid + " and Status=" + roleId );
        }

        public virtual DataPage<GroupUser> GetMembersApproved( int gid ) {
            return db.findPage<GroupUser>( getMembersCondition( gid ) );
        }

        //--------------------------------------------------------------------------------

        public virtual Boolean IsGroupFounder( int userId, int groupId ) {
            GroupUser relation = db.find<GroupUser>( getCondition( groupId, userId ) ).first();
            return relation != null && relation.IsFounder == 1;
        }

        public virtual Boolean IsGroupMember( int userId, int groupId ) {
            GroupUser relation = db.find<GroupUser>( getCondition( groupId, userId ) ).first();
            return relation != null && (relation.Status == GroupRole.Administrator.Id || relation.Status == GroupRole.Member.Id);
        }

        public virtual Boolean IsGroupApproving( int userId, int groupId ) {
            GroupUser relation = db.find<GroupUser>( getCondition( groupId, userId ) ).first();
            return relation != null && relation.Status == GroupRole.Approving.Id;
        }

        public virtual GroupRole GetUserRole( User user, int groupId ) {
            GroupUser relation = db.find<GroupUser>( getCondition( groupId, user.Id ) ).first();
            if (relation == null) return null;
            return GroupRole.GetById( relation.Status );
        }

        public virtual int MemberStatus( User user, int groupId ) {
            GroupUser relation = db.find<GroupUser>( getCondition( groupId, user.Id ) ).first();
            if (relation == null) return -1;
            return relation.Status;
        }

        public virtual Boolean IsGroupOfficer( int userId, int groupId ) {
            GroupUser relation = db.find<GroupUser>( getCondition( groupId, userId ) ).first();
            return relation != null && relation.Status == GroupRole.Administrator.Id;
        }

        //--------------------------------------------------------------------------------

        private String getCondition( int groupId, int userId ) {
            return "Group.Id=" + groupId + " and Member.Id=" + userId;
        }

        private String getMembersCondition( int groupId ) {
            return "Group.Id=" + groupId + " and (Status=" + GroupRole.Member.Id + " or Status=" + GroupRole.Administrator.Id + ")";
        }

        private String myGroupCondition( int userId ) {
            return "Member.Id=" + userId + " and (Status=" + GroupRole.Member.Id + " or Status=" + GroupRole.Administrator.Id + ")";
        }

        private void recountMembers( Group group ) {
            int newCount = db.count<GroupUser>( getMembersCondition( group.Id ) );
            if (group.MemberCount != newCount) {
                group.MemberCount = newCount;
                db.update( group, "MemberCount" );
            }
        }


    }

}
