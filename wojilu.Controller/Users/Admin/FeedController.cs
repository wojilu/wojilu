/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;

using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Service;
using wojilu.Members.Users.Interface;
using wojilu.Common.Feeds.Service;
using wojilu.Common.Feeds.Domain;
using wojilu.Common.Feeds.Interface;
using wojilu.Web.Controller.Common.Feeds;
using wojilu.Common;
using wojilu.Common.Microblogs.Domain;

namespace wojilu.Web.Controller.Users.Admin {

    public partial class FeedController : ControllerBase {

        public IFeedService feedService { get; set; }
        public IFriendService friendService { get; set; }
        public IVisitorService visitorService { get; set; }
        public IFollowerService followService { get; set; }

        public FeedController() {
            feedService = new FeedService();
            friendService = new FriendService();
            visitorService = new VisitorService();
            followService = new FollowerService();
        }

        public override void CheckPermission() {

            Boolean isFeedClose = Component.IsClose( typeof( FeedApp ) );
            if (isFeedClose) {
                echo( "对不起，本功能已经停用" );
            }
        }

        public override void Layout() {
        }

        public virtual void My( int id ) {

            feedService.ClearFeeds();

            User user = ctx.owner.obj as User;
            set( "user.PicSmall", user.PicSmall );
            set( "user.Link", to( new UserProfileController().Face ) );

            set( "notification.ListLink", to( new NotificationController().List ) );
            set( "notification.NewCount", getNewNotificationCount() );
            load( "notification.NewList", new NotificationController().NewList );

            set( "friendFeedLink", to( My, -1 ) );
            set( "user.MyFeedLink", to( My, -1 ) + "?uid=" + ctx.owner.Id );

            //load( "publish", new Microblogs.My.MbSaveController().Publish );

            if (Component.IsClose( typeof( MicroblogApp ) )) {
                set( "publish", "" );
            }
            else {

                load( "publish", new Microblogs.My.MicroblogController().Publisher );

            }

            bindFeedTypes();

            set( "friendsFrame", to( Friends ) );
            set( "followingFrame", to( Following ) );

            String dataType = FeedType.GetByInt( id );
            bindFeeds( dataType );

            bindVisitorList();
            bindFriendList();
            bindFollowingList();
        }
        
        private object getNewNotificationCount() {
            int count = ((User)ctx.viewer.obj).NewNotificationCount;
            return count > 0 ? "<span class=\"newNFcount\">(" + count + ")</span>" : "";
        }

        private void bindFeeds( String dataType ) {

            DataPage<Feed> feeds = getFeedList( dataType );

            IBlock dayBlock = getBlock( "days" );
            List<DateTime> dayList = getDayList( feeds.Results );
            foreach (DateTime day in dayList) {

                dayBlock.Set( "feedDay", cvt.ToDayString( day ) );

                IBlock block = dayBlock.GetBlock( "list" );
                List<FeedView> feedList = getFeedByDay( feeds.Results, day );

                loopBindFeeds( feedList, block );

                dayBlock.Next();
            }

            set( "page", feeds.PageBar );
        }

        private DataPage<Feed> getFeedList( String dataType ) {

            int userId = ctx.GetInt( "uid" );
            if (feedService.IsUserIdValid( userId, ctx.owner.Id )) {
                return feedService.GetUserSelf( userId, dataType, 50 );
            }

            return feedService.GetByUser( ctx.owner.Id, dataType, 50 );
        }


        private List<DateTime> getDayList( List<Feed> feeds ) {
            List<DateTime> results = new List<DateTime>();
            foreach (Feed feed in feeds) {
                if (isDayAdded( results, feed.Created ) == false) results.Add( feed.Created );
            }
            return results;
        }


        private Boolean isDayAdded( List<DateTime> days, DateTime created ) {
            foreach (DateTime day in days) {
                if (cvt.IsDayEqual( day, created )) return true;
            }
            return false;
        }

        private List<FeedView> getFeedByDay( List<Feed> feeds, DateTime day ) {
            List<Feed> results = new List<Feed>();
            foreach (Feed feed in feeds) {
                if (cvt.IsDayEqual( feed.Created, day )) results.Add( feed );
            }
            return FeedUtils.mergeFeed( results );
        }

        public void Friends() {
            DataPage<User> friends = friendService.GetFriendsPage( ctx.owner.obj.Id, 6 );
            bindShareFriends( friends );
        }

        public void Following() {
            DataPage<User> friends = followService.GetFollowingPage( ctx.owner.obj.Id, 6 );
            bindShareFollowing( friends );
        }

    }

}
