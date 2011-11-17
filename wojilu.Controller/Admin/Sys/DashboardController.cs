/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Collections;

using wojilu.Web.Mvc;

using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Service;
using wojilu.Members.Users.Interface;

using wojilu.Common.Feeds.Service;
using wojilu.Common.Feeds.Domain;
using wojilu.Common.Feeds.Interface;

using wojilu.Web.Controller.Users.Admin;
using wojilu.Common.MemberApp.Interface;
using wojilu.Members.Sites.Service;
using wojilu.Members.Sites.Domain;
using wojilu.Web.Controller.Security;
using wojilu.Web.Controller.Common.Feeds;

namespace wojilu.Web.Controller.Admin.Sys {

    public partial class DashboardController : ControllerBase {

        public IFeedService feedService { get; set; }
        public IUserService userService { get; set; }
        public IMemberAppService siteAppService { get; set; }

        public DashboardController() {
            feedService = new FeedService();
            userService = new UserService();
            siteAppService = new SiteAppService();
        }


        public void Links() {

            set( "addMenu", to( new Admin.MenuController().AddMenu ) );
            set( "addFooterMenuLink", to( new FooterMenuController().Add ) );

            set( "groupLink", lnkFull( to( new Web.Controller.Groups.MainController().Index ) ) );
            set( "userLink", lnkFull( to( new Users.MainController().Index ) ) );
            set( "blogLink", lnkFull( to( new Blog.MainController().Index ) ) );
            set( "photoLink", lnkFull( to( new Photo.MainController().Index ) ) );
            set( "microblogLink", lnkFull( alink.ToMicroblog() ) );
            set( "tagLink", lnkFull( to( new TagController().Index ) ) );


            set( "myHomeLink", lnkFull( to( new LinkController().MyHome ) ) );
            set( "mySpaceLink", lnkFull( to( new LinkController().MySpace ) ) );
            set( "myMicroblogLink", lnkFull( to( new LinkController().MyMicroblog ) ) );
            set( "myFriendsLink", lnkFull( to( new LinkController().MyFriends ) ) );
            set( "myMsgLink", lnkFull( to( new LinkController().MyMsg ) ) );
            set( "myAppLink", lnkFull( to( new LinkController().MyApp ) ) );
            set( "myMenuLink", lnkFull( to( new LinkController().MyMenu ) ) );

            set( "myProfileEditLink", lnkFull( to( new LinkController().MyProfile ) ) );
            set( "myPicLink", lnkFull( to( new LinkController().MyPic ) ) );
            set( "myPwdLink", lnkFull( to( new LinkController().MyPwd ) ) );
            set( "myContactLink", lnkFull( to( new LinkController().MyContact ) ) );
            set( "myInterestLink", lnkFull( to( new LinkController().MyInterest ) ) );
            set( "myTagLink", lnkFull( to( new LinkController().MyTag ) ) );

            set( "myPrivacyLink", lnkFull( to( new LinkController().MyPrivacy ) ) );
            set( "myCreditLink", lnkFull( to( new LinkController().MyCredit ) ) );
            set( "myInviteLink", lnkFull( to( new LinkController().MyInvite ) ) );

            set( "myGroupLink", lnkFull( to( new LinkController().MyGroup ) ) );
            set( "myNotificationLink", lnkFull( to( new LinkController().MyNotification ) ) );
            set( "mySkinLink", lnkFull( to( new LinkController().MySkin ) ) );


            IList apps = siteAppService.GetByMember( Site.Instance.Id );
            IBlock block = getBlock( "list" );
            bindAppList( apps, block );
        }


        private String lnkFull( String link ) {
            if (link.StartsWith( "http" )) return link;
            return strUtil.Join( ctx.url.SiteAndAppPath, link );
        }

        public void Index() {
            Home( -1 );
        }

        public void Home( int id ) {

            view( "Home" );

            set( "searchTarget", to( Search ) );
            set( "feedHomeLink", to( Home, -1 ) );

            bindFeedTypes();

            bindUsers();

            DataPage<Feed> feeds = getFeeds( id );
            bindFeedList( feeds );

        }

        private DataPage<Feed> getFeeds( int id ) {

            String dataType = FeedType.GetByInt( id );
            int userId = ctx.GetInt( "userId" );
            User user = userService.GetById( userId );

            if (user != null) {
                return feedService.GetAll( userId, dataType, 50 );
            }
            else {
                return feedService.GetAll( dataType, 50 );
            }
        }

        public void Search() {

            String userName = ctx.Post( "UserName" );

            if (strUtil.IsNullOrEmpty( userName )) {
                echoRedirect( lang( "exUserName" ) );
                return;
            }

            User user = userService.GetByName( userName );
            if (user == null) {
                echoRedirect( lang( "exUserNotFound" ) );
                return;
            }

            String lnk = to( Home, -1 ) + "?userId=" + user.Id;
            redirectUrl( lnk );
        }

        //-------------------------------------------------------------------------------


        private List<FeedView> getFeedByDay( List<Feed> feeds, DateTime day ) {
            List<Feed> results = new List<Feed>();
            foreach (Feed feed in feeds) {
                if (cvt.IsDayEqual( feed.Created, day )) results.Add( feed );
            }
            //return results;
            return FeedUtils.mergeFeed( results );
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


    }

}
