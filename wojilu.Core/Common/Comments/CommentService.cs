/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Serialization;
using wojilu.Common.AppBase;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.Feeds.Domain;
using wojilu.Common.Feeds.Service;
using wojilu.Common.Msg.Enum;
using wojilu.Common.Msg.Service;
using wojilu.Members.Users.Domain;
using wojilu.Common.Msg.Interface;

namespace wojilu.Common.Comments {

    public class CommentService<T> : ICommentService<T> where T : ObjectBase<T>, IComment {

        public INotificationService nfService { get; set; }

        public CommentService() {
            nfService = new NotificationService();
        }

        public T GetById( int commentId, int appId ) {
            return db.find<T>( "Id=" + commentId + " and AppId=" + appId ).first();
        }

        public DataPage<T> GetPageByTarget( int blogId, int pageSize ) {
            return db.findPage<T>( "RootId=" + blogId + " order by Id asc", pageSize );
        }

        public DataPage<T> GetPage( int appId ) {
            return db.findPage<T>( "AppId=" + appId, 40 );
        }

        public Result Insert( IComment c, String lnkTarget ) {

            // 在直接评论模式下，parentId 仅仅用于通知，并不保存到数据库中
            int parentId = c.ParentId;
            c.ParentId = 0;

            Result result = db.insert( c );
            if (result.IsValid) {
                postComment( c, lnkTarget );

                int parentReceiverId = 0;
                if (parentId > 0) {
                    IComment parent = GetById( parentId, c.AppId );
                    parentReceiverId = addNotificationToParent( parent, c, lnkTarget );
                }

                addNotificationToRootAuthor( c, lnkTarget, parentReceiverId );

            }
            return result;
        }

        private static void postComment( IComment c, String lnkTarget ) {
            updateCommentCount( c );
            c.AddFeedInfo( lnkTarget );
            c.AddNotification( lnkTarget );
        }

        public void Reply( IComment parent, IComment c, String lnkTarget ) {
            db.insert( c );
            postComment( c, lnkTarget );
            int parentReceiverId = addNotificationToParent( parent, c, lnkTarget );
            addNotificationToRootAuthor( c, lnkTarget, parentReceiverId );
        }

        // 给上一个评论的作者发通知
        private int addNotificationToParent( IComment parent, IComment c, String lnkTarget ) {

            if (parent.Member == null || parent.Member.Id <= 0) return 0;

            IAppData post = ndb.findById( parent.GetTargetType(), parent.RootId ) as IAppData;

            int receiverId = parent.Member.Id;
            if (c.Member != null && (c.Member.Id == receiverId)) return 0;// 自己的回复不用给自己发通知
            if (receiverId == post.OwnerId) return 0;// parent和target同一作者，也不用重复发通知

            String msg = c.Author + " 回复了你在 <a href=\"" + lnkTarget + "\">" + post.Title + "</a> 的评论";
            nfService.send( receiverId, typeof( User ).FullName, msg, NotificationType.Comment );

            return receiverId;
        }


        // 给主题作者(比如投递人)发通知
        private void addNotificationToRootAuthor( IComment c, String lnkTarget, int parentReceiverId ) {

            IAppData post = ndb.findById( c.GetTargetType(), c.RootId ) as IAppData;

            if (post.Creator == null || post.Creator.Id <= 0) return;
            if (post.Creator.Id == parentReceiverId) return;
            if (post.Creator.Id == post.OwnerId && post.Creator.GetType().FullName == post.OwnerType) return;
            if (c.Member != null && c.Member.Id == post.Creator.Id) return;

            String msgCreator = c.Author + " 评论了你发表的 <a href=\"" + lnkTarget + "\">" + post.Title + "</a>";
            nfService.send( post.Creator.Id, msgCreator, NotificationType.Comment );
        }

        //-----------------------------------------------------------------------------------------------

        public void Delete( IComment c ) {
            db.delete( c );
            updateCommentCount( c );
        }

        private static void updateCommentCount( IComment comment ) {
            IEntity post = ndb.findById( comment.GetTargetType(), comment.RootId );
            if (post == null) return;
            int replies = db.find<T>( "RootId=" + post.Id ).count();
            String propertyName = "Replies";
            post.set( propertyName, replies );
            db.update( post, propertyName );
        }

        public DataPage<T> GetPageAll( String condition ) {
            return db.findPage<T>( condition );
        }

        public void DeleteBatch( String ids ) {

            if (strUtil.IsNullOrEmpty( ids )) return;

            int[] arrId = cvt.ToIntArray( ids );
            foreach (int id in arrId) {

                IEntity c = db.findById<T>( id );
                if (c == null) continue;

                // 删除评论
                db.delete( c );

                // 重新统计父帖的数量
                String typeFullName = ((IComment)c).GetTargetType().FullName;
                Type ttype = ((IComment)c).GetTargetType();
                IEntity parent = ndb.findById( ttype, ((IComment)c).RootId );
                if (parent == null) continue;

                String property = "Replies";
                int replies = (int)parent.get( property );
                parent.set( property, (replies - 1) );
                db.update( parent, property );

            }
        }

    }

}
