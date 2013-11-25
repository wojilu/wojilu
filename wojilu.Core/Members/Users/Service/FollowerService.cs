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
using wojilu.Common.Microblogs;
using wojilu.Common.Microblogs.Interface;
using wojilu.Common.Microblogs.Service;

namespace wojilu.Members.Users.Service {

    public class FollowerService : IFollowerService {

        public virtual IFriendService friendService { get; set; }
        public virtual IUserService userService { get; set; }

        public FollowerService() {
            friendService = new FriendService();
            userService = new UserService();
        }

        public virtual bool IsFollowing(long userId, long targetId) {

            Follower f = db.find<Follower>( "User.Id=:userId and Target.Id=:targetId" )
             .set( "userId", userId )
             .set( "targetId", targetId )
             .first();
            return f != null;
        }

        public virtual Follower Follow(long userId, long targetId, string ip) {

            if (userId <= 0) return null;
            if (targetId <= 0) return null;

            if (IsFollowing( userId, targetId )) return null;
            if (friendService.IsFriend( userId, targetId )) return null;

            User user = userService.GetById( userId );
            User target = userService.GetById( targetId );

            Follower f = new Follower();
            f.User = user;
            f.Target = target;
            f.Ip = ip;
            db.insert( f );

            recountUsers( userId );
            recountUsers( targetId );

            return f;
        }

        public virtual void FollowWithFeedNotification(long userId, long targetId, string ip) {

            Follower f = this.Follow( userId, targetId, ip );
            if (f != null) {
                addFeedInfo( f );
                addNotification( f );
            }
        }

        private void addFeedInfo( Follower f ) {
            String msg = MbTemplate.GetFeed( "¹Ø×¢ÁË", f.Target.Name, Link.ToMember( f.Target ), "", f.Target.PicSX );
            IMicroblogService microblogService = ObjectContext.Create<IMicroblogService>( typeof( MicroblogService ) );
            microblogService.AddSimplePrivate( f.User, msg, typeof( Follower ).FullName, f.Target.Id, f.Ip );
        }

        private void addNotification( Follower f ) {
            long receiverId = f.Target.Id;
            String msg = "<a href='" + Link.ToMember( f.User ) + "'>" + f.User.Name + "</a> " + lang.get( "followedYou" );
            new NotificationService().send( receiverId, typeof( User ).FullName, msg, NotificationType.Follow );
        }

        //----------------------------------------------------------------------------------------------------------

        public virtual Result DeleteFollow(long userId, long targetId) {
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

        public virtual List<User> GetRecentFollowing(long userId, int count) {
            List<Follower> followers = db.find<Follower>( "User.Id=" + userId ).list( count );
            return populateTarget( followers );
        }

        public virtual List<User> GetRecentFollowers(long targetId, int count) {
            if (count == 0) count = 10;
            List<Follower> followers = db.find<Follower>( "Target.Id=" + targetId ).list( count );
            return populateUser( followers );
        }

        public virtual List<User> GetRecentFriendsAndFollowers(long targetId, int count) {
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


        public virtual DataPage<User> GetFollowingPage(long userId) {
            return GetFollowingPage( userId, 20 );
        }

        public virtual DataPage<User> GetFollowingPage(long userId, int pageSize) {
            DataPage<Follower> followers = db.findPage<Follower>( "User.Id=" + userId, pageSize );
            return followers.Convert<User>( populateTarget( followers.Results ) );
        }

        public virtual DataPage<User> GetFollowersPage(long targetId) {
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


        public virtual string GetFollowingIds(long userId) {
            List<User> fs = GetRecentFollowing( userId, -1 );
            String ids = "";
            foreach (User user in fs) {
                ids += user.Id + ",";
            }
            return ids.TrimEnd( ',' );

        }

        //-------------------------------------------------------------------------------------

        private void recountUsers(long userId) {

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
