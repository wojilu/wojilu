using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Msg.Interface;
using wojilu.Common.Msg.Service;
using wojilu.Members.Users.Domain;
using wojilu.Common.Msg.Enum;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Common.Comments {


    public class OpenCommentService : IOpenCommentService {

        public INotificationService nfService { get; set; }

        public OpenCommentService() {
            nfService = new NotificationService();
        }

        public OpenComment GetById( int id ) {
            return db.findById<OpenComment>( id );
        }

        public DataPage<OpenComment> GetPageAll( String condition ) {
            return OpenComment.findPage( condition );
        }


        public void Delete( OpenComment c ) {
            if (c == null) return;
            db.delete( c );
            deleteSubComments( c );
            updateRootTargetReplies( c );
        }

        private void deleteSubComments( OpenComment c ) {
            if (c.Replies == 0) return;
            db.deleteBatch<OpenComment>( "ParentId=" + c.Id );
        }

        public void DeleteBatch( String ids ) {
            if (strUtil.IsNullOrEmpty( ids )) return;
            if (cvt.IsIdListValid( ids ) == false) throw new ArgumentException( "id list" );
            db.deleteBatch<OpenComment>( "Id in (" + ids + ")" );
        }

        public void DeleteAll( string url, int dataId, string dataType ) {

            if (dataId > 0 && strUtil.HasText( dataType )) {
                deleteAllByData( dataId, dataType );
            }
            else {
                deleteAllByUrl( url );
            }
        }

        private void deleteAllByData( int dataId, string dataType ) {
            db.deleteBatch<OpenComment>( "TargetDataType='" + strUtil.SqlClean( dataType, 50 ) + "' and TargetDataId=" + dataId );
            clearRootTargetRepliesByData( dataId, dataType );
        }

        private void deleteAllByUrl( string url ) {
            db.deleteBatch<OpenComment>( "TargetUrl='" + strUtil.SqlClean( url, 50 ) + "'" );
            clearRootTargetRepliesByUrl( url );
        }

        public DataPage<OpenComment> GetByDataDesc( String dataType, int dataId ) {
            return GetByDataDesc( dataType, dataId, -1 );
        }

        public DataPage<OpenComment> GetByDataDesc( String dataType, int dataId, int pageSize ) {

            if (pageSize <= 0 || pageSize > 500) pageSize = 20;

            DataPage<OpenComment> datas = OpenComment.findPage( "TargetDataType='" + strUtil.SqlClean( dataType, 50 ) + "' and TargetDataId=" + dataId + " and ParentId=0", pageSize );

            datas.Results = addSubList( datas.Results, true );

            return datas;
        }

        public DataPage<OpenComment> GetByDataAsc( String dataType, int dataId ) {

            DataPage<OpenComment> datas = OpenComment.findPage( "TargetDataType='" + strUtil.SqlClean( dataType, 50 ) + "' and TargetDataId=" + dataId + " and ParentId=0 order by Id asc" );

            datas.Results = addSubList( datas.Results, false );

            return datas;
        }

        public DataPage<OpenComment> GetByUrlDesc( String url ) {

            DataPage<OpenComment> datas = OpenComment.findPage( "TargetUrl='" + strUtil.SqlClean( url, 50 ) + "' and ParentId=0" );

            datas.Results = addSubList( datas.Results, true );

            return datas;
        }

        public DataPage<OpenComment> GetByUrlAsc( String url ) {

            DataPage<OpenComment> datas = OpenComment.findPage( "TargetUrl='" + strUtil.SqlClean( url, 50 ) + "' and ParentId=0 order by Id asc" );

            datas.Results = addSubList( datas.Results, false );

            return datas;
        }


        public List<OpenComment> GetByApp( Type type, int appId, int listCount ) {

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

        public Result Create( OpenComment c ) {

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

        // 只是导入，并不发送通知
        public Result Import( OpenComment c ) {

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

            List<int> sentIds = new List<int>();

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

        private void sendNotificationToRoot( List<int> sentIds, OpenComment c ) {

            if (c.Member != null && c.Member.Id == c.TargetUserId) return; // 不用给自己发通知
            int receiverId = c.TargetUserId;
            if (sentIds.Contains( receiverId )) return; // 已经发过，不用重发

            String msg = c.Author + " 回复了你的 <a href=\"" + c.TargetUrl + "\">" + c.TargetTitle + "</a> ";

            nfService.send( receiverId, typeof( User ).FullName, msg, NotificationType.Comment );
            sentIds.Add( receiverId );
        }

        private void sendNotificationsTo( List<int> sentIds, OpenComment comment, OpenComment c ) {

            int receiverId = comment.Member.Id;
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


        public List<OpenComment> GetMore( int parentId, int startId, int replyPageSize, string sort ) {

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

        public int GetReplies( int dataId, String dataType, String url ) {

            if (dataId > 0 && strUtil.HasText( dataType )) {
                return GetRepliesByData( dataId, dataType );
            }
            else {
                return GetRepliesByUrl( url );
            }
        }

        public int GetRepliesByUrl( String url ) {
            OpenCommentCount objCount = OpenCommentCount.find( "TargetUrl=:url" )
                .set( "url", url )
                .first();
            return objCount == null ? 0 : objCount.Replies;
        }

        public int GetRepliesByData( int dataId, String dataType ) {
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

        private static void updateTargetReplies( OpenComment c, int replies ) {
            Type targetType = Entity.GetType( c.TargetDataType );
            ICommentTarget target = ndb.findById( targetType, c.TargetDataId ) as ICommentTarget;
            if (target == null) return;
            target.Replies = replies;
            db.update( target );

            if (c.AppId <= 0) return;
            Type appType = target.GetAppType();
            if (appType == null) return;

            ICommentApp app = ndb.findById( appType, c.AppId ) as ICommentApp;
            if (app == null) return;
            int appCount = OpenComment.count( "AppId=" + c.AppId + " and TargetDataType='" + c.TargetDataType + "'" );
            app.CommentCount = appCount;
            db.update( app );
        }

        private static void clearRootTargetRepliesByData( int dataId, string dataType ) {

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
