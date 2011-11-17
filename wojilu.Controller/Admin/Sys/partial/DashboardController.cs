/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Collections;

using wojilu.Web.Mvc;

using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Photo.Domain;

using wojilu.Members.Users.Domain;

using wojilu.Common.Feeds.Domain;

using wojilu.Web.Controller.Users.Admin;
using wojilu.Common.MemberApp.Interface;
using wojilu.Members.Sites.Domain;
using wojilu.Web.Controller.Security;
using wojilu.Web.Controller.Common.Feeds;

namespace wojilu.Web.Controller.Admin.Sys {

    public partial class DashboardController : ControllerBase {




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

        private void bindAppList( IList apps, IBlock block ) {
            foreach (IMemberApp app in apps) {
                block.Set( "app.Name", app.Name );
                String lnk = lnkFull( alink.ToUserAppFull( app ) );
                block.Set( "app.Link", lnk );
                block.Next();
            }
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


                block.Set( "feed.DataType", feed.DataType );
                block.Set( "feed.UserFace", feed.Creator.PicSmall );
                block.Set( "feed.UserLink", Link.ToMember( feed.Creator ) );

                String creatorInfo = string.Format( "<a href='{0}'>{1}</a>", Link.ToMember( feed.Creator ), feed.Creator.Name );
                String feedTitle = feedService.GetHtmlValue( feed.TitleTemplate, feed.TitleData, creatorInfo );
                block.Set( "feed.Title", feedTitle );

                String feedBody = feedService.GetHtmlValue( feed.BodyTemplate, feed.BodyData, creatorInfo );
                block.Set( "feed.Body", feedBody );
                block.Set( "feed.Created", feed.Created );

                block.Set( "feed.BodyGeneral", getComment( feed.BodyGeneral ) );

                block.Next();
            }
        }

        private String getComment( String comment ) {
            if (strUtil.IsNullOrEmpty( comment )) return "";
            return string.Format( "<div class=\"quote\"><span class=\"qSpan\">{0}<span></div>", comment );
        }


        //------------------------------------------------------------------------

        private void bindUsers() {
            List<User> newUsers = userService.GetNewList( 8 );
            bindUserList( "newUsers", newUsers );

            List<User> newLoginUsers = userService.GetNewLoginList( 8 );
            bindUserList( "visitors", newLoginUsers );
        }

        private void bindUserList( String blockName, List<User> users ) {

            IBlock block = getBlock( blockName );
            foreach (User user in users) {
                block.Set( "user.Name", user.Name );
                block.Set( "user.Face", user.PicSmall );
                block.Set( "user.Created", cvt.ToTimeString( user.Created ) );
                block.Set( "user.LastLoginTime", cvt.ToTimeString( user.LastLoginTime ) );
                block.Set( "user.Link", Link.ToMember( user ) );
                block.Next();
            }
        }

    }

}
