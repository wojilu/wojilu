/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Feeds.Domain;
using wojilu.Common.Feeds.Interface;
using wojilu.Members.Users.Service;
using wojilu.Members.Users.Interface;
using wojilu.Common.Msg.Interface;
using wojilu.Common.Msg.Service;
using wojilu.Common.Msg.Enum;
using wojilu.Members.Users.Domain;

namespace wojilu.Common.Feeds.Service {

    public class ShareService : IShareService {

        public virtual IFriendService fs { get; set; }
        public virtual IFeedService feedService { get; set; }
        public virtual INotificationService nfService { get; set; }

        public ShareService() {
            fs = new FriendService();
            feedService = new FeedService();
            nfService = new NotificationService();
        }

        public virtual Share GetById(long id) {
            return Share.findById( id );
        }

        public virtual Share GetByIdWithComments(long id) {
            Share share = Share.findById( id );
            if (share != null) {
                List<ShareComment> list = ShareComment.find( "RootId=" + id + " order by Id" ).list();
                share.setComments( list );
            }
            return share;
        }

        public virtual Result Delete(long id) {
            Share share = GetById( id );
            Result result = new Result();
            if (share == null) {
                result.Add( lang.get( "exDataNotFound" ) );
                return result;
            }
            else {
                share.delete();
                deleteFeedByShare( id );
                return result;
            }
        }

        private void deleteFeedByShare(long id) {

            Feed feed = Feed.find( "DataType=:type and DataId=:id" )
                .set( "type", typeof( Share ).FullName )
                .set( "id", id )
                .first();

            if (feed != null) feed.delete();
        }

        public virtual DataPage<Share> GetPageAll() {
            return Share.findPage( "" );
        }

        public virtual ShareComment GetCommentById(long id) {
            return ShareComment.findById( id );
        }

        public virtual Result Create( Share share ) {
            if (IsShared( share )) return new Result( lang.get( "exHaveShared" ) );
            return db.insert( share );
        }

        public virtual Result CreateUrl( User user, String shareLink, String shareDescription ) {
            Share share = new Share();

            share.Creator = user;
            share.DataType = typeof( Share ).FullName;

            String titleTemplate = string.Format( lang.get( "shareInfo" ), "{*actor*}" );
            //String titleTemplate = "{*actor*} 分享了一个网址";
            String titleData = "";

            share.TitleTemplate = titleTemplate;
            share.TitleData = titleData;

            String bodyTemplate = @"<div><a href=""{*postLink*}"">{*postLink*}</a></div><div class=""note"">{*body*}</div>";
            String bodyData = "{postLink:\"" + shareLink + "\", body:\"" + shareDescription + "\"}";

            share.BodyTemplate = bodyTemplate;
            share.BodyData = bodyData;

            return share.insert();
        }

        public virtual List<Share> GetByUser(int count, long userId) {
            if (count <= 0) count = 5;
            return Share.find( "Creator.Id=" + userId ).list( count );
        }

        public virtual DataPage<Share> GetPageByUser(long userId, int pageSize) {
            DataPage<Share> list = Share.findPage( "CreatorId=" + userId, pageSize );
            mergeCommentsPrivate( list.Results );
            return list;
        }

        public virtual DataPage<Share> GetFriendsPage(long userId, int pageSize) {

            String friendIds = fs.FindFriendsIds( userId );
            if (strUtil.IsNullOrEmpty( friendIds )) return DataPage<Share>.GetEmpty();

            DataPage<Share> list = Share.findPage( "CreatorId in (" + friendIds+")", pageSize );
            mergeCommentsPrivate( list.Results );
            return list;
        }

        public virtual Boolean IsShared( Share share ) {


            List<Share> list = Share.find( "Creator.Id=:cid and HashData=:data" )
                .set( "cid", share.Creator.Id )
                .set( "data", share.HashData )
                .list();

            return list.Count > 0;
        }


        //--------------------------------------

        private void mergeCommentsPrivate( List<Share> list ) {
            String ids = getShareIds( list );
            if (strUtil.IsNullOrEmpty( ids )) return;
            List<ShareComment> comments = ShareComment.find( "RootId in (" + ids + ") order by Id" ).list();

            mergeComments( list, comments );
        }


        private void mergeComments( List<Share> list, List<ShareComment> comments ) {
            foreach (Share blog in list) {
                List<ShareComment> clist = getCommentsByShare( comments, blog );
                if (clist.Count == 0) continue;
                blog.setComments( clist );
            }
        }

        private List<ShareComment> getCommentsByShare( List<ShareComment> comments, Share share ) {
            List<ShareComment> results = new List<ShareComment>();
            foreach (ShareComment c in comments) {
                if (c.Root.Id == share.Id) results.Add( c );
            }
            return results;
        }

        private String getShareIds( List<Share> list ) {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++) {
                sb.Append( list[i].Id );
                if (i < list.Count - 1) sb.Append( "," );
            }
            return sb.ToString();
        }

        //--------------------------------------

        public virtual void InsertComment( ShareComment c, String shareLink, String parentShareLink ) {

            saveComment( c );
            copyCommentCountToFeed( c );

            addNotificationToRoot( c, shareLink );
            addNotificationToParent( c, parentShareLink );
        }

        private void saveComment( ShareComment c ) {
            c.insert();
        }

        private void copyCommentCountToFeed( ShareComment c ) {
            feedService.SetCommentCount( c.Root );

        }

        private void addNotificationToRoot( ShareComment c, String shareLink ) {
            Share root = c.Root;

            long receiverId = root.Creator.Id;
            if (c.User.Id == receiverId) return;

            String msg = c.User.Name + " " + lang.get( "commentYour" ) + " <a href=\"" + shareLink + "\">" + lang.get( "share" ) + "</a>";
            nfService.send( receiverId, typeof(User).FullName, msg, NotificationType.Comment );
        } 
        
        private void addNotificationToParent( ShareComment c, String shareLink ) {
            if (c.ParentId == 0) return;

            ShareComment parent = ShareComment.findById( c.ParentId );

            long receiverId = parent.User.Id;
            if (c.User.Id == receiverId) return;

            //String msg = c.User.Name + " 回复了你的分享 <a href=\"" + shareLink + "\">评论</a>";
            String msg = c.User.Name + " " + lang.get( "commentYour" ) + " <a href=\"" + shareLink + "\">" + lang.get( "share" ) + "</a>";

            nfService.send( receiverId, typeof( User ).FullName, msg, NotificationType.Comment );
        }



    }

}
