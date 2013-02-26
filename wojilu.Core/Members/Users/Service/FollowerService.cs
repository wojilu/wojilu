/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;

using wojilu.Common.Msg.Enum;
using wojilu.Common.Msg.Service;
using wojilu.Common.Feeds.Domain;
using wojilu.Common.Feeds.Service;

using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;

namespace wojilu.Members.Users.Service {

    public class FollowerService : IFollowerService {

        public virtual IFriendService friendService { get; set; }
        public virtual IUserService userService { get; set; }

        public FollowerService() {
            friendService = new FriendService();
            userService = new UserService();
        }

        public virtual Boolean IsFollowing( int userId, int targetId ) {

            Follower f = db.find<Follower>( "User.Id=:userId and Target.Id=:targetId" )
             .set( "userId", userId )
             .set( "targetId", targetId )
             .first();
            return f != null;
        }

        public virtual Follower Follow( int userId, int targetId ) {

            if (userId <= 0) return null;
            if (targetId <= 0) return null;

            if (IsFollowing( userId, targetId )) return null;
            if (friendService.IsFriend( userId, targetId )) return null;

            User user = userService.GetById( userId );
            User target = userService.GetById( targetId );

            Follower f = new Follower();
            f.User = user;
            f.Target = target;
            db.insert( f );

            recountUsers( userId );
            recountUsers( targetId );

            return f;
        }

        public virtual void FollowWithFeedNotification( int userId, int targetId ) {

            Follower f = this.Follow( userId, targetId );
            if (f != null) {
                addFeedInfo( f );
                addNotification( f );
            }
        }

        private void addNotification( Follower f ) {
            int receiverId = f.Target.Id;
            String msg = "<a href='" + Link.ToMember( f.User ) + "'>" + f.User.Name + "</a> " + lang.get( "followedYou" );
            new NotificationService().send( receiverId, typeof( User ).FullName, msg, NotificationType.Follow );
        }

        private void addFeedInfo( Follower f ) {
            Feed feed = new Feed();
            feed.Creator = f.User;
            feed.DataType = typeof( Follower ).FullName;
            feed.TitleTemplate = "{*actor*} " + lang.get( "follow" ) + " {*friend*}";

            String userLink = Link.ToMember( f.Target );

            Dictionary<string, object> dic = new Dictionary<string, object>();
            String flnk = string.Format( "<a href=\"{0}\">{1}</a>", userLink, f.Target.Name );
            dic.Add( "friend", flnk );
            dic.Add( "friendId", f.Target.Id );
            String templateData = Json.ToString( dic );

            feed.TitleData = templateData;

            new FeedService().publishUserAction( feed );
        }

        //----------------------------------------------------------------------------------------------------------

        public virtual Result DeleteFollow( int userId, int targetId ) {
            Follower f = db.find<Follower>( "User.Id=:userId and Target.Id=:targetId" )
                .set( "userId", userId )
                .set( "targetId", targetId )
                .first();

            if (f != null) {

                db.delete( f );

                recountUsers( userId );
                recountUsers( targetId );

                return new Result();
            }
            else
                return new Result( lang.get( "followshipNotFound" ) );
        }


        //----------------------------------------------------------------------------------------------------------

        public virtual List<User> GetRecentFollowing( int userId, int count ) {
            List<Follower> followers = db.find<Follower>( "User.Id=" + userId ).list( count );
            return populateTarget( followers );
        }

        public virtual List<User> GetRecentFollowers( int targetId, int count ) {
            if (count == 0) count = 10;
            List<Follower> followers = db.find<Follower>( "Target.Id=" + targetId ).list( count );
            return populateUser( followers );
        }

        public virtual List<User> GetRecentFriendsAndFollowers( int targetId, int count ) {
            List<User> friends = friendService.FindFriends( targetId, count );
            List<User> followers = GetRecentFollowers( targetId, count );
            List<User> results = new List<User>();
            foreach (User u in followers) results.Add( u );
            int iend = count - results.Count;
            for (int i = 0; i < iend; i++) {
                if (i >= friends.Count) break;
                results.Add( friends[i] );
            }
            return results;
        }


        public virtual DataPage<User> GetFollowingPage( int userId ) {
            return GetFollowingPage( userId, 20 );
        }

        public virtual DataPage<User> GetFollowingPage( int userId, int pageSize ) {
            DataPage<Follower> followers = db.findPage<Follower>( "User.Id=" + userId, pageSize );
            return followers.Convert<User>( populateTarget( followers.Results ) );
        }

        public virtual DataPage<User> GetFollowersPage( int targetId ) {
            DataPage<Follower> followers = db.findPage<Follower>( "Target.Id=" + targetId );
            return followers.Convert<User>( populateUser( followers.Results ) );
        }

        private List<User> populateTarget( List<Follower> followers ) {
            List<User> results = new List<User>();
            foreach (Follower f in followers) {
                if (f.Target == null) {
                    db.delete( f );
                    recountUsers( f.User.Id );
                    continue;
                }
                results.Add( f.Target );
            }
            return results;
        }

        private List<User> populateUser( List<Follower> followers ) {
            List<User> results = new List<User>();
            foreach (Follower f in followers) {
                if (f.User == null) {
                    db.delete( f );
                    recountUsers( f.Target.Id );
                }
                results.Add( f.User );
            }
            return results;
        }


        public virtual String GetFollowingIds( int userId ) {
            List<User> fs = GetRecentFollowing( userId, -1 );
            String ids = "";
            foreach (User user in fs) {
                ids += user.Id + ",";
            }
            return ids.TrimEnd( ',' );

        }

        //-------------------------------------------------------------------------------------

        private void recountUsers( int userId ) {

            User user = userService.GetById( userId );
            if (user == null) return;

            int followersCount = db.count<Follower>( "TargetId=" + userId );
            int followingCount = db.count<Follower>( "UserId=" + userId );

            List<string> p = new List<string>();

            if( user.FollowersCount != followersCount ) {
                user.FollowersCount = followersCount;
                p.Add( "FollowersCount");
            }

            if (user.FollowingCount != followingCount) {
                user.FollowingCount = followingCount;
                p.Add( "FollowingCount" );
            }

            if (p.Count > 0) {
                db.update( user, p.ToArray() );
            }
        }

    }

}
