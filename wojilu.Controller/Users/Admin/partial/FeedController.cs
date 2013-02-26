/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;

using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Photo.Domain;
using wojilu.Apps.Forum.Domain;

using wojilu.Members.Users.Domain;
using wojilu.Common.Feeds.Domain;
using wojilu.Web.Controller.Common.Feeds;

namespace wojilu.Web.Controller.Users.Admin {

    public partial class FeedController : ControllerBase {


        private void bindFeedTypes() {
            set( "feedListLink", getTypeLink( null ) );
            set( "blogFeedsLink", getTypeLink( typeof( BlogPost ) ) );
            set( "photoFeedsLink", getTypeLink( typeof( PhotoPost ) ) );

            set( "forumFeedsLink", getTypeLink( typeof( ForumPost ) ) );
            set( "shareFeedsLink", getTypeLink( typeof( Share ) ) );
            set( "friendsFeedsLink", getTypeLink( typeof( FriendShip ) ) );
        }

        private String getTypeLink( Type t ) {
            int userId = ctx.GetInt( "uid" );
            if (userId <= 0)
                return to( My, FeedType.Get( t ) );
            return to( My, FeedType.Get( t ) ) + "?uid=" + userId;
        }

        //--------------------------------------------------------------------------------------------------------------------

        private void loopBindFeeds( List<FeedView> feedList, IBlock block ) {

            foreach (FeedView feed in feedList) {

                if (feed.DataType.Equals( typeof( FriendShip ).FullName ) && isMyInfo( feed.TitleData )) continue;
                if (feed.DataType.Equals( typeof( Follower ).FullName ) && isMyInfo( feed.TitleData )) continue;

                bindOneFeed( block, feed );

                block.Next();
            }
        }

        private void bindOneFeed( IBlock block, FeedView feed ) {
            block.Set( "feed.Id", feed.Id );
            block.Set( "feed.DataType", feed.DataType );
            block.Set( "feed.UserFace", feed.Creator.PicSmall );
            block.Set( "feed.UserLink", toUser( feed.Creator ) );

            String creatorInfo = getCreatorInfos( feed.CreatorList );
            String feedTitle = feedService.GetHtmlValue( feed.TitleTemplate, feed.TitleData, creatorInfo );
            block.Set( "feed.Title", feedTitle );

            String commentCmd = getCommentCmd( feed );
            block.Set( "feed.CommentCmd", commentCmd );

            String feedBody = feedService.GetHtmlValue( feed.BodyTemplate, feed.BodyData, creatorInfo );
            block.Set( "feed.Body", feedBody );
            block.Set( "feed.BodyGeneral", getComment( feed.BodyGeneral ) );

            String created = "";
            if (cvt.IsDayEqual( feed.Created, DateTime.Now )) created = cvt.ToTimeString( feed.Created );
            block.Set( "feed.Created", created );
        }

        private String getCommentCmd( FeedView feed ) {
            return "";
            //if (!feed.CanComment) return "";

            //ControllerBase controller = ControllerFactory.FindFeedCommentsController( feed.DataType, ctx );
            //if (controller == null) return "";

            //String commentsLink = "";
            //if (feed.DataType.Equals( typeof( Share ).FullName )) {
            //    commentsLink = Link.To( ctx.owner.obj, controller.GetType(), "Show", feed.DataId );
            //}
            //else {
            //    commentsLink = Link.To( feed.Creator, controller.GetType(), "Show", feed.DataId );
            //}
            //commentsLink += "?parentPanelId=" + feed.Id;

            //String lbl = lang( "comment" );
            //String c = feed.Replies > 0 ? lbl + "(" + feed.Replies + ")" : lbl;
            //return string.Format( "<span class=\"frmUpdate link\" href=\"{0}\" loadTo=\"commentList{1}\" txtHidden=\"{2}\">{3}</span>", commentsLink, feed.Id, lang( "hideComment" ), c );
        }

        //-----------------------------------------------

        private String getCreatorInfos( List<User> creatorList ) {
            String result = "";
            foreach (User user in creatorList) {
                result += string.Format( "<a href='{0}'>{1}</a>", toUser( user ), user.Name );
                result += "、";
            }
            return result.TrimEnd( '、' );
        }


        private String getComment( String comment ) {
            if (strUtil.IsNullOrEmpty( comment )) return "";
            return string.Format( "<div class=\"quote\"><span class=\"qSpan\">{0}<span></div>", comment );
        }




        private Boolean isMyInfo( String bodyData ) {
            JsonObject dic = Json.ParseJson( bodyData );
            if (dic.ContainsKey( "friendId" ) == false) return false;
            if (cvt.ToInt( dic["friendId"] ) == ctx.owner.obj.Id) return true;
            return false;
        }


        //--------------------------------------------------------------------------------------------------------------------


        private void bindVisitorList() {

            IBlock block = getBlock( "visitors" );
            List<User> visitorList = visitorService.GetRecent( 9, ctx.owner.obj.Id );
            foreach (User user in visitorList) {
                block.Set( "user.Name", user.Name );
                block.Set( "user.Face", user.PicSmall );
                block.Set( "user.Link", toUser( user ) );
                block.Next();
            }
        }

        private void bindFriendList() {
            List<User> friends = friendService.GetRecentActive( 9, ctx.owner.obj.Id );
            IBlock block = getBlock( "myfriendList" );
            foreach (User user in friends) {
                block.Set( "user.Name", user.Name );
                block.Set( "user.Face", user.PicSmall );
                block.Set( "user.Link", toUser( user ) );
                block.Set( "user.LastActiveTime", getLastActiveTime( user ) );
                block.Next();
            }
        }

        private void bindFollowingList() {
            List<User> friends = followService.GetRecentFollowing( ctx.owner.obj.Id, 9 );
            IBlock block = getBlock( "following" );
            foreach (User user in friends) {
                block.Set( "user.Name", user.Name );
                block.Set( "user.Face", user.PicSmall );
                block.Set( "user.Link", toUser( user ) );
                block.Set( "user.LastActiveTime", getLastActiveTime( user ) );
                block.Next();
            }
        }

        private void bindShareFriends( DataPage<User> friends ) {
            List<User> list = friends.Results;
            IBlock block = getBlock( "shareFriends" );
            foreach (User user in list) {
                block.Set( "user.Name", user.Name );
                block.Set( "user.Face", user.PicSmall );
                block.Set( "user.ShareLink", to( My, -1 ) + "?uid=" + user.Id );
                block.Next();
            }
            set( "page", friends.PageBar );
        }

        private void bindShareFollowing( DataPage<User> friends ) {
            List<User> list = friends.Results;
            IBlock block = getBlock( "shareFollowing" );
            foreach (User user in list) {
                block.Set( "user.Name", user.Name );
                block.Set( "user.Face", user.PicSmall );
                block.Set( "user.ShareLink", to( My, -1 ) + "?uid=" + user.Id );
                block.Next();
            }
            set( "page", friends.PageBar );
        }


        // TODO 得到好友最近活动时间
        private String getLastActiveTime( User user ) {
            if (cvt.IsDayEqual( user.LastUpdateTime, DateTime.Now ))
                return user.LastUpdateTime.ToShortTimeString();
            return user.LastUpdateTime.ToShortDateString();
        }


    }

}
