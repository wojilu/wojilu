using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Msg.Interface;
using wojilu.Common.Msg.Service;
using wojilu.Members.Users.Domain;
using wojilu.Common.Msg.Enum;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.Microblogs.Domain;

namespace wojilu.Common.Comments {


    public class OpenCommentService : IOpenCommentService {

        public INotificationService nfService { get; set; }

        public OpenCommentService() {
            nfService = new NotificationService();
        }

        public virtual OpenComment GetById(long id) {
            return db.findById<OpenComment>( id );
        }

        public virtual DataPage<OpenComment> GetPageAll( String condition ) {
            return OpenComment.findPage( condition );
        }


        public virtual void Delete( OpenComment c ) {
            if (c == null) return;
            db.delete( c );
            deleteSubComments( c );
            updateRootTargetReplies( c );
        }

        private void deleteSubComments( OpenComment c ) {
            if (c.Replies == 0) return;
            db.deleteBatch<OpenComment>( "ParentId=" + c.Id );
        }

        public virtual void DeleteBatch( String ids ) {
            if (strUtil.IsNullOrEmpty( ids )) return;
            if (cvt.IsIdListValid( ids ) == false) throw new ArgumentException( "id list" );
            db.deleteBatch<OpenComment>( "Id in (" + ids + ")" );
        }

        public virtual void DeleteAll(string url, long dataId, string dataType) {

            if (dataId > 0 && strUtil.HasText( dataType )) {
                deleteAllByData( dataId, dataType );
            }
            else {
                deleteAllByUrl( url );
            }
        }

        private void deleteAllByData(long dataId, string dataType) {
            db.deleteBatch<OpenComment>( "TargetDataType='" + strUtil.SqlClean( dataType, 50 ) + "' and TargetDataId=" + dataId );
            clearRootTargetRepliesByData( dataId, dataType );
        }

        private void deleteAllByUrl( string url ) {
            db.deleteBatch<OpenComment>( "TargetUrl='" + strUtil.SqlClean( url, 50 ) + "'" );
            clearRootTargetRepliesByUrl( url );
        }

        public virtual DataPage<OpenComment> GetByMicroblogOwnerId(long ownerId) {

            int pageSize = 20;

            DataPage<OpenComment> datas = OpenComment.findPage( "OwnerId=" + ownerId + " and FeedId>0 and ParentId=0", pageSize );

            datas.Results = addSubList( datas.Results, true );

            return datas;
        }

        public virtual DataPage<OpenComment> GetByDataAndOwnerId(string dataType, long ownerId) {

            int pageSize = 20;

            DataPage<OpenComment> datas = OpenComment.findPage( "TargetDataType='" + strUtil.SqlClean( dataType, 50 ) + "' and OwnerId=" + ownerId + " and ParentId=0", pageSize );

            datas.Results = addSubList( datas.Results, true );

            return datas;
        }

        public virtual DataPage<OpenComment> GetByDataDesc(string dataType, long dataId) {
            return GetByDataDesc( dataType, dataId, -1 );
        }

        public virtual DataPage<OpenComment> GetByDataDesc(string dataType, long dataId, int pageSize) {

            if (pageSize <= 0 || pageSize > 500) pageSize = 20;

            DataPage<OpenComment> datas = OpenComment.findPage( "TargetDataType='" + strUtil.SqlClean( dataType, 50 ) + "' and TargetDataId=" + dataId + " and ParentId=0", pageSize );

            datas.Results = addSubList( datas.Results, true );

            return datas;
        }

        public virtual DataPage<OpenComment> GetByDataAsc(string dataType, long dataId) {

            DataPage<OpenComment> datas = OpenComment.findPage( "TargetDataType='" + strUtil.SqlClean( dataType, 50 ) + "' and TargetDataId=" + dataId + " and ParentId=0 order by Id asc" );

            datas.Results = addSubList( datas.Results, false );

            return datas;
        }

        public virtual DataPage<OpenComment> GetByUrlDesc( String url ) {

            DataPage<OpenComment> datas = OpenComment.findPage( "TargetUrl='" + strUtil.SqlClean( url, 50 ) + "' and ParentId=0" );

            datas.Results = addSubList( datas.Results, true );

            return datas;
        }

        public virtual DataPage<OpenComment> GetByUrlAsc( String url ) {

            DataPage<OpenComment> datas = OpenComment.findPage( "TargetUrl='" + strUtil.SqlClean( url, 50 ) + "' and ParentId=0 order by Id asc" );

            datas.Results = addSubList( datas.Results, false );

            return datas;
        }


        public virtual List<OpenComment> GetByApp(Type type, long appId, int listCount) {

            if (listCount <= 0) listCount = 7;

            String condition = "TargetDataType='" + type + "'";
            if (appId > 0) condition = condition + " and AppId=" + appId;

            return OpenComment.find( condition ).list( listCount );
        }

        //----------------------------------------------------------------------------------------------


        private List<OpenComment> addSubList( List<OpenComment> list, Boolean isDesc ) {

            String subIds = "";
            foreach (OpenComment c in list) {
                if (isDesc) {
                    subIds = strUtil.Join( subIds, c.LastReplyIds, "," );
                }
                else {
                    subIds = strUtil.Join( subIds, c.FirstReplyIds, "," );
                }
            }

            subIds = subIds.Trim().TrimStart( ',' ).TrimEnd( ',' );
            if (strUtil.IsNullOrEmpty( subIds )) return list;

            List<OpenComment> totalSubList = OpenComment.find( "Id in (" + subIds + ")" ).list();
            foreach (OpenComment c in list) {
                c.SetReplyList( getSubListFromTotal( c, totalSubList ) );
            }

            return list;
        }

        private List<OpenComment> getSubListFromTotal( OpenComment parent, List<OpenComment> totalSubList ) {

            List<OpenComment> results = new List<OpenComment>();
            int iCount = 0;
            foreach (OpenComment c in totalSubList) {

                if (iCount >= OpenComment.subCacheSize) break;

                if (c.ParentId == parent.Id) {
                    results.Add( c );
                    iCount = iCount + 1;
                }
            }

            return results;
        }

        //----------------------------------------------------------------------------------------------

        public virtual Result Create( OpenComment c ) {

            Result result = c.insert();
            if (result.IsValid) {
                updateParentReplies( c );
                updateRootTargetReplies( c );
                sendNotifications( c );
                return result;
            }
            else {
                return result;
            }

        }

        public virtual Result CreateNoNotification( OpenComment c ) {

            Result result = c.insert();
            if (result.IsValid) {
                updateParentReplies( c );
                updateRootTargetReplies( c );
                return result;
            }
            else {
                return result;
            }

        }

        // 只是导入，并不发送通知
        public virtual Result Import( OpenComment c ) {

            Result result = c.insert();
            if (result.IsValid) {
                updateParentReplies( c );
                updateRootTargetReplies( c );
                return result;
            }
            else {
                return result;
            }

        }

        private void sendNotifications( OpenComment c ) {

            List<long> sentIds = new List<long>();

            if (c.ParentId > 0) {
                OpenComment p = OpenComment.findById( c.ParentId );
                if (p != null && p.Member != null) {
                    sendNotificationsTo( sentIds, p, c );
                }
            }

            if (c.AtId > 0) {
                OpenComment at = OpenComment.findById( c.AtId );
                if (at != null && at.Member != null) {
                    sendNotificationsTo( sentIds, at, c );
                }
            }

            if (c.TargetUserId > 0) {
                sendNotificationToRoot( sentIds, c );
            }
        }

        private void sendNotificationToRoot(List<long> sentIds, OpenComment c) {

            if (c.Member != null && c.Member.Id == c.TargetUserId) return; // 不用给自己发通知
            long receiverId = c.TargetUserId;
            if (sentIds.Contains( receiverId )) return; // 已经发过，不用重发

            String msg = c.Author + " 回复了你的 <a href=\"" + c.TargetUrl + "\">" + c.TargetTitle + "</a> ";

            nfService.send( receiverId, typeof( User ).FullName, msg, NotificationType.Comment );
            sentIds.Add( receiverId );
        }

        private void sendNotificationsTo(List<long> sentIds, OpenComment comment, OpenComment c) {

            long receiverId = comment.Member.Id;
            if (c.Member != null && c.Member.Id == receiverId) return; // 不用给自己发通知
            if (sentIds.Contains( receiverId )) return; // 已经发过，不用重发

            String msg = c.Author + " 回复了你在 <a href=\"" + c.TargetUrl + "\">" + comment.TargetTitle + "</a> 的评论";
            nfService.send( receiverId, typeof( User ).FullName, msg, NotificationType.Comment );
            sentIds.Add( receiverId );
        }

        private static void updateParentReplies( OpenComment c ) {

            if (c.ParentId == 0) return;

            OpenComment p = OpenComment.findById( c.ParentId );
            if (p == null) {
                c.ParentId = 0;
                c.update();
                return;
            }

            //------------------------------------------------
            p.Replies = OpenComment.count( "ParentId=" + p.Id );

            //-------------------------------------------------
            List<OpenComment> subFirst = OpenComment.find( "ParentId=" + p.Id + " order by Id asc" ).list( OpenComment.subCacheSize );
            List<OpenComment> subLast = OpenComment.find( "ParentId=" + p.Id + " order by Id desc" ).list( OpenComment.subCacheSize );

            p.FirstReplyIds = strUtil.GetIds( subFirst );
            p.LastReplyIds = strUtil.GetIds( subLast );

            p.update();

        }


        public virtual List<OpenComment> GetMore(long parentId, long startId, int replyPageSize, string sort) {

            String condition = "";

            if (sort == "asc") {
                condition = "ParentId=" + parentId + " and Id>" + startId + " order by Id asc";
            }
            else {
                condition = "ParentId=" + parentId + " and Id<" + startId + " order by Id desc";
            }

            return OpenComment.find( condition ).list( replyPageSize );
        }

        //------------------------------------------------------------------------------------------------------------

        public virtual int GetReplies(long dataId, string dataType, string url) {

            if (dataId > 0 && strUtil.HasText( dataType )) {
                return GetRepliesByData( dataId, dataType );
            }
            else {
                return GetRepliesByUrl( url );
            }
        }

        public virtual int GetRepliesByUrl( String url ) {
            OpenCommentCount objCount = OpenCommentCount.find( "TargetUrl=:url" )
                .set( "url", url )
                .first();
            return objCount == null ? 0 : objCount.Replies;
        }

        public virtual int GetRepliesByData(long dataId, string dataType) {
            OpenCommentCount objCount = OpenCommentCount.find( "DataType=:dtype and DataId=" + dataId )
                .set( "dtype", dataType )
                .first();
            return objCount == null ? 0 : objCount.Replies;
        }


        private void updateRootTargetReplies( OpenComment c ) {

            int replies;
            OpenCommentCount objCount;

            if (c.TargetDataId > 0 && strUtil.HasText( c.TargetDataType )) {
                replies = OpenComment.find( "TargetDataType=:dtype and TargetDataId=" + c.TargetDataId )
                    .set( "dtype", c.TargetDataType )
                    .count();

                objCount = OpenCommentCount.find( "DataType=:dtype and DataId=" + c.TargetDataId )
                    .set( "dtype", c.TargetDataType )
                    .first();
            }
            else {

                if (c.TargetUrl == null) {
                    replies = 0;
                    objCount = null;
                }
                else {

                    replies = OpenComment.find( "TargetUrl=:url" )
                        .set( "url", c.TargetUrl )
                        .count();

                    objCount = OpenCommentCount.find( "TargetUrl=:url" )
                        .set( "url", c.TargetUrl )
                        .first();

                }
            }


            if (objCount == null) {
                insertCommentCount( c, replies );
            }
            else {
                updateCommentCount( objCount, replies );
            }

            updateTargetReplies( c, replies );
        }

        private static void updateCommentCount( OpenCommentCount objCount, int replies ) {
            objCount.Replies = replies;
            objCount.update();
        }

        private static void insertCommentCount( OpenComment c, int replies ) {
            OpenCommentCount objCount = new OpenCommentCount();
            objCount.TargetUrl = c.TargetUrl;
            objCount.DataType = c.TargetDataType;
            objCount.DataId = c.TargetDataId;
            objCount.Replies = replies;

            objCount.insert();
        }

        public virtual IEntity GetTarget( OpenComment c ) {

            if (strUtil.IsNullOrEmpty( c.TargetDataType )) return null;
            if (c.TargetDataId <= 0) return null;

            Type targetType = Entity.GetType( c.TargetDataType );
            if (targetType == null) return null;
            return ndb.findById( targetType, c.TargetDataId );
        }

        private void updateTargetReplies( OpenComment c, int replies ) {
            ICommentTarget target = GetTarget( c ) as ICommentTarget;
            if (target != null) {
                target.Replies = replies;
                db.update( target );
            }

            // feed replies
            Microblog mblog = getFeed( c );
            if (mblog != null) {
                mblog.Replies = replies;
                mblog.update();
            }

            if (c.AppId > 0 && target != null) {
                Type appType = target.GetAppType();
                if (appType != null) {

                    ICommentApp app = ndb.findById( appType, c.AppId ) as ICommentApp;
                    if (app != null) {
                        int appCount = OpenComment.count( "AppId=" + c.AppId + " and TargetDataType='" + c.TargetDataType + "'" );
                        app.CommentCount = appCount;
                        db.update( app );
                    }
                }
            }
        }

        private static Microblog getFeed( OpenComment c ) {
            if (c.FeedId > 0) {
                return Microblog.findById( c.FeedId );
            }
            else {
                return Microblog.find( "DataId=:id and DataType=:dtype" )
                    .set( "id", c.TargetDataId )
                    .set( "dtype", c.TargetDataType )
                    .first();
            }
        }

        private static void clearRootTargetRepliesByData(long dataId, string dataType) {

            OpenCommentCount objCount = OpenCommentCount.find( "DataType=:dtype and DataId=" + dataId )
                .set( "dtype", dataType )
                .first();

            if (objCount == null) return;

            objCount.Replies = 0;
            objCount.update();
        }

        private static void clearRootTargetRepliesByUrl( String url ) {

            OpenCommentCount objCount = OpenCommentCount.find( "TargetUrl=:url" )
                .set( "url", url )
                .first();

            if (objCount == null) return;

            objCount.Replies = 0;
            objCount.update();
        }


    }
}
