/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Service;
using wojilu.Members.Users.Interface;

using wojilu.Common.Feeds.Service;
using wojilu.Common.Feeds.Domain;
using wojilu.Common.Feeds.Interface;

using wojilu.Web.Controller.Users.Admin;
using wojilu.Common.MemberApp.Interface;
using wojilu.Members.Sites.Service;
using wojilu.Web.Controller.Common.Feeds;

using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Photo.Domain;


namespace wojilu.Web.Controller.Admin {

    public class FeedAdminController : ControllerBase {

        public IFeedService feedService { get; set; }
        public IUserService userService { get; set; }
        public IMemberAppService siteAppService { get; set; }

        public FeedAdminController() {
            feedService = new FeedService();
            userService = new UserService();
            siteAppService = new SiteAppService();
        }

        public void Index() {
            Home( -1 );
        }

        public void Home( int id ) {

            view( "Home" );

            set( "searchTarget", to( Search ) );
            set( "feedHomeLink", to( Home, -1 ) );

            bindFeedTypes();

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
            return FeedUtils.convertView( results );
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

        //-------------------------------------------------------------------------------

        private void bindFeedTypes() {
            set( "feedListLink", getTypeLink( null ) );
            set( "blogFeedsLink", getTypeLink( typeof( BlogPost ) ) );
            set( "photoFeedsLink", getTypeLink( typeof( PhotoPost ) ) );

            set( "forumFeedsLink", getTypeLink( typeof( ForumPost ) ) );
            set( "shareFeedsLink", getTypeLink( typeof( Share ) ) );
            set( "friendsFeedsLink", getTypeLink( typeof( FriendShip ) ) );
        }

        private String getTypeLink( Type t ) {
            int userId = ctx.GetInt( "userId" );
            if (userId <= 0)
                return to( Home, FeedType.Get( t ) );
            return to( Home, FeedType.Get( t ) ) + "?userId=" + userId;
        }


        private void bindFeedList( DataPage<Feed> feeds ) {

            IBlock dayBlock = getBlock( "days" );
            List<DateTime> dayList = getDayList( feeds.Results );

            foreach (DateTime day in dayList) {

                dayBlock.Set( "feedDay", cvt.ToDayString( day ) );

                IBlock block = dayBlock.GetBlock( "list" );
                List<FeedView> feedList = getFeedByDay( feeds.Results, day );
                bindOneFeed( block, feedList );

                dayBlock.Next();
            }

            set( "page", feeds.PageBar );

        }

        private void bindOneFeed( IBlock block, List<FeedView> feedList ) {
            foreach (FeedView feed in feedList) {

                if (feed.DataType.Equals( typeof( FriendShip ).FullName )) continue;

                if (feed.Creator == null) continue;

                block.Set( "feed.Id", feed.Id );
                block.Set( "feed.DataType", feed.DataType );
                block.Set( "feed.UserFace", feed.Creator.PicSmall );
                block.Set( "feed.UserLink", toUser( feed.Creator ) );
                block.Set( "feed.Ip", feed.Ip );

                String creatorInfo = string.Format( "<a href='{0}'>{1}</a>", toUser( feed.Creator ), feed.Creator.Name );
                String feedTitle = feedService.GetHtmlValue( feed.TitleTemplate, feed.TitleData, creatorInfo );
                block.Set( "feed.Title", feedTitle );

                String feedBody = feedService.GetHtmlValue( feed.BodyTemplate, feed.BodyData, creatorInfo );
                block.Set( "feed.Body", feedBody );
                block.Set( "feed.Created", feed.Created );

                block.Set( "feed.BodyGeneral", getComment( feed.BodyGeneral ) );

                block.Set( "feed.DeleteLink", to( DeleteFeed, feed.Id ) );

                block.Next();
            }
        }

        [HttpDelete]
        public void DeleteFeed( int id ) {
            feedService.DeleteOne( id );
            echoAjaxOk();
        }

        private String getComment( String comment ) {
            if (strUtil.IsNullOrEmpty( comment )) return "";
            return string.Format( "<div class=\"quote\"><span class=\"qSpan\">{0}<span></div>", comment );
        }

    }

}
