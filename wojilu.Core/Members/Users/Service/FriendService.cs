/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;

using wojilu.Common.Feeds.Domain;
using wojilu.Common.Feeds.Service;
using wojilu.Common.Msg.Enum;
using wojilu.Common.Msg.Interface;
using wojilu.Common.Msg.Service;

using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Enum;
using wojilu.Members.Users.Interface;

namespace wojilu.Members.Users.Service {

    public class FriendService : IFriendService {

        public virtual INotificationService notificationService { get; set; }
        public virtual IUserService userService { get; set; }

        public FriendService() {
            notificationService = new NotificationService();
            userService = new UserService();
        }


        public virtual void AddInviteFriend( User newRegUser, int friendId ) {

            FriendShip ship = new FriendShip();
            ship.User = newRegUser;
            ship.Friend = userService.GetById( friendId );
            ship.Status = FriendStatus.Approved;

            ship.insert();

            recountFriends( newRegUser.Id );
            recountFriends( friendId );

            addFeedInfo( ship, ship.Ip );

            addNotificationWhenApproved( newRegUser, friendId );
        }

        private void addNotificationWhenApproved( User friend, int receiverId ) {
            String msg = string.Format( "<a href=\"{0}\">{1}</a> 接受了您的好友邀请</a>", Link.ToMember( friend ), friend.Name );
            notificationService.send( receiverId, msg, NotificationType.FriendApprove );
        }

        public virtual Result AddFriend( int userId, int friendId, String msg, String ip ) {

            Result result = CanAddFriend( userId, friendId );
            if (result.HasErrors) return result;

            FriendShip fs = new FriendShip();

            User user = userService.GetById( userId );
            User friend = userService.GetById( friendId );

            fs.User = user;
            fs.Friend = friend;
            fs.Msg = msg;

            fs.Status = FriendStatus.Waiting;
            fs.Ip = ip;

            result = db.insert( fs );
            if (result.IsValid) {
                string userLink = Link.ToMember( user );
                String note = string.Format( "<a href=\"{0}\" target=\"_blank\" class=\"requestUser\">{1}</a> " + lang.get( "requestFriend" ), userLink, user.Name );
                if (strUtil.HasText( msg )) {
                    note += "<br/><span class=\"quote\">" + msg + "</span>";
                }
                notificationService.sendFriendRequest( userId, friendId, note );

                // 顺带添加关注
                FollowerService followService = new FollowerService();
                if (followService.IsFollowing( userId, friendId ) == false)
                    followService.Follow( userId, friendId );
            }
            return result;
        }

        public Result CanAddFriend( int userId, int targetId ) {

            Result result = new Result();

            if (userId <= 0) {
                result.Add( lang.get( "exPlsLogin" ) );
                return result;
            }

            if (userId == targetId) {
                result.Add( lang.get( "exFriendSelf" ) );
                return result;
            }

            User f = userService.GetById( targetId );
            if (f == null) {
                result.Add( lang.get( "exUser" ) );
                return result;
            }

            BlacklistService blacklistService = new BlacklistService();

            if (blacklistService.IsBlack( targetId, userId )) {
                result.Add( lang.get( "blackFriend" ) );
            }
            else if (IsFriend( userId, targetId )) {
                result.Add( lang.get( "exFriendBeen" ) );
            }
            else if (IsWaitingFriendApproving( userId, targetId )) {
                result.Add( lang.get( "inApproveFriend" ) );
            }

            return result;
        }


        public void CancelAddFriend( int userId, int fid ) {

            String condition = "User.Id=" + userId + " and Friend.Id=" + fid;
            Boolean cancelOk = cancelFriendSingle( userId, fid, condition );
            if (cancelOk) return;

            condition = "User.Id=" + fid + " and Friend.Id=" + userId;
            cancelOk = cancelFriendSingle( fid, userId, condition );

            if (cancelOk == false) throw new Exception( "friendship not exist" );
        }

        private Boolean cancelFriendSingle( int userId, int fid, String condition ) {
            FriendShip ship = db.find<FriendShip>( condition ).first();
            if (ship == null) return false;
            db.delete( ship );

            notificationService.cancelFriendRequest( userId, fid );
            return true;
        }

        public virtual void Approve( int userId, int friendId ) {

            // friendId 是邀请方

            String condition = "User.Id=" + friendId + " and Friend.Id=" + userId;
            FriendShip ship = db.find<FriendShip>( condition ).first();
            if (ship == null) throw new Exception( "user(id:" + friendId + ") have not request you friend(id:" + userId + ")" );
            ship.Status = FriendStatus.Approved;
            db.update( ship, "Status" );

            recountFriends( userId );
            recountFriends( friendId );

            new FollowerService().DeleteFollow( userId, friendId );
            new FollowerService().DeleteFollow( friendId, userId );

            addFeedInfo( ship, ship.Ip );

            User user = userService.GetById( userId );

            addNotificationWhenApproved( user, friendId );
        }


        private void addFeedInfo( FriendShip ship, String ip ) {

            addFrinedFeedInfo( ship.User, ship.Friend, ip );
            addFrinedFeedInfo( ship.Friend, ship.User, ip );
        }

        private static void addFrinedFeedInfo( User user, User friend, String ip ) {
            String userLink = Link.ToMember( friend );

            String lnkInfo = string.Format( "<a href=\"{0}\">{1}</a>", userLink, friend.Name );

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add( "friend", lnkInfo );
            dic.Add( "friendId", friend.Id );
            String templateData = Json.ToString( dic );

            TemplateBundle tplBundle = TemplateBundle.GetFriendsTemplateBundle();
            new FeedService().publishUserAction( user, typeof( FriendShip ).FullName, tplBundle.Id, templateData, "", ip );
        }

        public virtual void Refuse( int userId, int friendId ) {
            String condition = "User.Id=" + friendId + " and Friend.Id=" + userId;
            FriendShip ship = db.find<FriendShip>( condition ).first() as FriendShip;
            if (ship == null) throw new Exception( "user(id:" + friendId + ") have not request you friend(id:" + userId + ")" );
            db.delete( ship );
        }

        //-----------------------------------------------------------------------------------------------------------------

        public virtual Boolean IsFriend( int userId, int fid ) {

            if (userId <= 0 || fid <= 0) return false;

            String condition = getCondition( userId, fid );
            return db.find<FriendShip>( condition ).first() != null;
        }

        public virtual Boolean IsWaitingFriendApproving( int userId, int fid ) {
            String condition = "( (User.Id=" + userId + " and Friend.Id=" + fid + ") or ( User.Id=" + fid + " and Friend.Id=" + userId + " ) ) and Status=" + FriendStatus.Waiting;
            return db.find<FriendShip>( condition ).first() != null;
        }

        //-----------------------------------------------------------------------------------------------------------------

        public virtual List<User> FindFriends( int userId, int count ) {
            if (count == 0) count = 10;
            String condition = getCondition( userId );
            List<FriendShip> list = db.find<FriendShip>( condition ).list( count );
            return populateUser( list, userId );
        }

        public virtual List<FriendShip> GetFriendsAll( int userId ) {

            String condition = getCondition( userId );
            return db.find<FriendShip>( condition ).list();
        }

        public virtual DataPage<User> GetFriendsPage( int userId ) {
            return GetFriendsPage( userId, 20 );
        }

        public virtual DataPage<User> GetFriendsPage( int userId, int pageSize ) {

            String condition = getCondition( userId );
            return populateUsers( userId, pageSize, condition );
        }

        public virtual DataPage<FriendShip> GetPageByCategory( int userId, int categoryId, int pageSize ) {

            String condition = getConditionByCategory( userId, categoryId );
            return db.findPage<FriendShip>( condition, pageSize );
        }

        public virtual DataPage<FriendShip> GetPageBySearch( int userId, String friendName, int pageSize ) {

            String condition = getConditionByCategory( userId, 0 );
            List<FriendShip> xlist = db.find<FriendShip>( condition ).list();
            List<FriendShip> list = new List<FriendShip>();
            foreach (FriendShip x in xlist) {
                if (x.User.Id == userId) {
                    if (x.Friend.Name.StartsWith( friendName )) list.Add( x );
                }
                else if (x.Friend.Id == userId) {
                    if (x.User.Name.StartsWith( friendName )) list.Add( x );
                }
            }

            return DataPage<FriendShip>.GetPage( list, pageSize );
        }

        private DataPage<User> populateUsers( int userId, int pageSize, String condition ) {
            DataPage<FriendShip> list = db.findPage<FriendShip>( condition, pageSize );

            return list.Convert<User>( populateUser( list.Results, userId ) );
        }

        public virtual void DeleteFriendByBlacklist( int userId, int fid ) {

            FollowerService followService = new FollowerService();

            String condition = "User.Id=" + userId + " and Friend.Id=" + fid + " and Status=" + FriendStatus.Approved;
            FriendShip ship = db.find<FriendShip>( condition ).first();
            if (ship != null) {
                db.delete( ship );
                if (followService.IsFollowing( userId, fid )) followService.DeleteFollow( userId, fid );
                recountFriends( userId );
                recountFriends( fid );

                return;
            }

            condition = "User.Id=" + fid + " and Friend.Id=" + userId + " and Status=" + FriendStatus.Approved;
            ship = db.find<FriendShip>( condition ).first();
            if (ship != null) {
                db.delete( ship );
                if (followService.IsFollowing( fid, userId )) followService.DeleteFollow( fid, userId );
                recountFriends( userId );
                recountFriends( fid );

            }

        }

        public virtual void DeleteFriend( int userId, int fid ) {

            FollowerService followService = new FollowerService();

            String condition = "User.Id=" + userId + " and Friend.Id=" + fid + " and Status=" + FriendStatus.Approved;
            FriendShip ship = db.find<FriendShip>( condition ).first();
            if (ship != null) {
                db.delete( ship );
                if (followService.IsFollowing( userId, fid ) == false) followService.Follow( userId, fid );
                recountFriends( userId );
                recountFriends( fid );

                return;
            }

            condition = "User.Id=" + fid + " and Friend.Id=" + userId + " and Status=" + FriendStatus.Approved;
            ship = db.find<FriendShip>( condition ).first();
            if (ship != null) {
                db.delete( ship );
                if (followService.IsFollowing( fid, userId ) == false) followService.Follow( fid, userId );
                recountFriends( userId );
                recountFriends( fid );

            }
        }



        public virtual String FindFriendsIds( int userId ) {
            List<User> fs = FindFriends( userId, -1 );
            String ids = "";
            foreach (User user in fs) {
                ids += user.Id + ",";
            }
            return ids.TrimEnd( ',' );
        }

        public virtual List<int> FindFriendsIdList( int userId ) {
            List<User> fs = FindFriends( userId, -1 );

            List<int> ids = new List<int>();
            foreach (User user in fs) {
                ids.Add( user.Id );
            }
            return ids;
        }


        public virtual List<User> FindFriendsByFriends( int userId, int count ) {

            List<int> idList = new List<int>();
            idList.Add( userId );

            // 1、好友的ID
            String condition = getCondition( userId );
            List<FriendShip> list = db.find<FriendShip>( condition ).list();
            String ids = "";
            foreach (FriendShip ship in list) {

                if (ship.User == null || ship.Friend == null) continue;

                if (ship.User.Id == userId) {
                    int fid = ship.Friend.Id;
                    if (idList.Contains( fid )) continue;
                    idList.Add( fid );
                    ids += fid + ",";
                }
                else if (ship.Friend.Id == userId) {
                    int fid = ship.User.Id;
                    if (idList.Contains( fid )) continue;
                    idList.Add( fid );
                    ids += fid + ",";
                }

            }

            ids = ids.TrimEnd( ',' ).TrimStart( ',' );

            // 2、好友的好友的ID
            String c1 = "MemberId in (" + ids + ") and Status=" + FriendStatus.Approved;
            String c2 = "FriendId in (" + ids + " ) and Status=" + FriendStatus.Approved;

            String userIds = "";

            List<FriendShip> fs1 = strUtil.HasText( ids ) ? db.find<FriendShip>( c1 ).list() : new List<FriendShip>();
            foreach (FriendShip ship in fs1) {
                if (ship.Friend == null) continue;
                int fid = ship.Friend.Id;
                if (idList.Contains( fid ) == false) {
                    idList.Add( fid );
                    userIds += fid + ",";
                }
            }

            List<FriendShip> fs2 = strUtil.HasText( ids ) ? db.find<FriendShip>( c2 ).list() : new List<FriendShip>();
            foreach (FriendShip ship in fs2) {
                if (ship.User == null) continue;
                int fid = ship.User.Id;
                if (idList.Contains( fid ) == false) {
                    idList.Add( fid );
                    userIds += fid + ",";
                }
            }

            userIds = userIds.TrimEnd( ',' ).TrimStart( ',' );
            if (strUtil.IsNullOrEmpty( userIds )) return new List<User>();

            return db.find<User>( "Id in (" + userIds + ")" ).list( count );
        }

        //---------------------------------------------------------------------------------------------------------------

        private String getCondition( int userId ) {
            return "( User.Id=" + userId + " or Friend.Id=" + userId + ") and Status=" + FriendStatus.Approved;
        }

        private String getCondition( int userId, int fid ) {
            return "( (User.Id=" + userId + " and Friend.Id=" + fid + ") or ( User.Id=" + fid + " and Friend.Id=" + userId + " ) ) and Status=" + FriendStatus.Approved;
        }

        private String getConditionByCategory( int userId, int categoryId ) {
            if (categoryId <= 0) return "( User.Id=" + userId + " or Friend.Id=" + userId + ") and Status=" + FriendStatus.Approved;
            return "(( User.Id=" + userId + " and CategoryId=" + categoryId + " ) or (Friend.Id=" + userId + " and CategoryIdFriend=" + categoryId + " )) and Status=" + FriendStatus.Approved;
        }

        //private String getConditionBySearch( int userId, String friendName ) {
        //    if (categoryId <= 0) return "( User.Id=" + userId + " or Friend.Id=" + userId + ") and Status=" + FriendStatus.Approved;
        //}

        private List<User> populateUser( List<FriendShip> list, int userId ) {
            List<User> results = new List<User>();
            foreach (FriendShip ship in list) {

                if (ship.User == null || ship.Friend == null) continue;

                if (ship.User.Id == userId)
                    results.Add( ship.Friend );
                else
                    results.Add( ship.User );
            }
            return results;
        }

        // TODO : 按照最新活动时间排序
        public virtual List<User> GetRecentActive( int count, int userId ) {
            String condition = getCondition( userId );
            List<FriendShip> list = db.find<FriendShip>( condition ).list( count );
            return populateUser( list, userId );
        }

        //---------------------------------------------------------------------------------------------------------------

        private void recountFriends( int userId ) {

            User user = userService.GetById( userId );
            if (user == null) return;

            int newCount = db.count<FriendShip>( getCondition( userId ) );
            if (newCount != user.FriendCount) {
                user.FriendCount = newCount;
                db.update( user, "FriendCount" );
            }
        }

        public virtual void UpdateCategory( int ownerId, int friendId, int categoryId, string friendDescription ) {
            String condition = getCondition( ownerId, friendId );
            FriendShip fs = db.find<FriendShip>( condition ).first();
            if (fs == null) return;

            if (fs.User.Id == ownerId) {
                fs.CategoryId = categoryId;
                fs.Description = friendDescription;
            }
            else {
                fs.CategoryIdFriend = categoryId;
                fs.DescriptionFriend = friendDescription;
            }

            fs.update();
        }

    }

}
