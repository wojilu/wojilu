/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web;

using wojilu.DI;
using wojilu.Data;

using wojilu.ORM;
using wojilu.ORM.Caching;

using wojilu.Common.Feeds.Domain;
using wojilu.Common.Feeds.Interface;
using wojilu.Common.Msg.Interface;
using wojilu.Common.Msg.Service;

using wojilu.Members.Users.Service;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;

namespace wojilu.Common.Feeds.Service {

    public class FeedService : IFeedService {

        public virtual IFriendService friendService { get; set; }
        public virtual IFollowerService followerService { get; set; }
        public virtual INotificationService nfService { get; set; }

        public FeedService() {
            friendService = new FriendService();
            followerService = new FollowerService();
            nfService = new NotificationService();
        }


        public Feed GetById( int feedId ) {
            return Feed.findById( feedId );
        }

        public IEntity GetData( int id ) {

            Feed feed = GetById( id );
            IEntity result = ndb.findById( ObjectContext.Instance.TypeList[feed.DataType], feed.DataId );

            return result;
        }

        //----------------------------------------------- 开放方法 --------------------------------------------------------------------

        public virtual TemplateBundle registerTemplateBundle( List<OneLineStoryTemplate> oneLineStoryTemplates, List<ShortStoryTemplate> shortStoryTemplates, List<ActionLink> actionLinks ) {

            TemplateBundle t = new TemplateBundle();
            t.OneLineStoryTemplatesStr = Json.ToStringList( oneLineStoryTemplates );
            t.ShortStoryTemplatesStr = Json.ToStringList( shortStoryTemplates );
            t.ActionLinksStr = Json.ToStringList( actionLinks );
            db.insert( t );

            return t;
        }


        public virtual TemplateBundle getRegisteredTemplateBundleByID( int id ) {
            return db.findById<TemplateBundle>( id );
        }

        public virtual void publishUserAction( Feed data ) {

            Feed feed = new Feed();
            // 区别在这里
            feed.DataId = data.DataId; 

            populateData( data, feed );
            db.insert( feed );
        }

        public virtual void publishUserAction( IFeed data ) {

            Feed feed = new Feed();
            // 区别在这里
            feed.DataId = data.Id; 

            populateData( data, feed );
            db.insert( feed );
        }

        private static void populateData( IFeed data, Feed feed ) {
            feed.Creator = data.Creator;
            feed.DataType = data.DataType;

            feed.TitleTemplate = data.TitleTemplate;
            feed.TitleData = data.TitleData;

            feed.BodyTemplate = data.BodyTemplate;
            feed.BodyData = data.BodyData;
            feed.BodyGeneral = data.BodyGeneral;
        }

        public virtual void publishUserAction( User creator, String dataType, int templateBundleId, String templateData, String bodyGeneral, String ip ) {

            // 模板数据是 json 字符串类型；也就是 key-value 对
            // 除了自定义的键外，比如 {*gift*}, {*mood*}, {*score*}
            // 还有系统保留的4个键：images, flash, mp3, video

            Feed feed = new Feed();
            feed.Creator = creator;
            feed.DataType = dataType;
            feed.BodyGeneral = bodyGeneral;

            TemplateBundle t = getRegisteredTemplateBundleByID( templateBundleId );

            // 判断是单行模式还是多行模式
            List<OneLineStoryTemplate> onelineTemplates = t.GetOneLineStoryTemplates(  );
            List<ShortStoryTemplate> shortStoryTemplates = t.GetShortStoryTemplates(  );

            // 如果是单行模式，只解析出标题的模板与数据
            if (onelineTemplates.Count > 0) {

                OneLineStoryTemplate tpl = onelineTemplates[0];
                //feed.TitleTemplate = getTitleTemplate( onelineTemplates, templateData, dataKeyCount );
                feed.TitleTemplate = tpl.Title;
                feed.TitleData = templateData;
            }
            // 如果是多行模式，除了标题，还要解析出主干部分的模板和数据，以及评论
            else if (shortStoryTemplates.Count > 0) {

                ShortStoryTemplate tpl = shortStoryTemplates[0];
                feed.TitleTemplate = tpl.Title;
                feed.TitleData = PlatformTemplate.GetVarData( tpl.Title, templateData );

                feed.BodyTemplate = tpl.Body;
                feed.BodyData = PlatformTemplate.GetVarData( tpl.Body, templateData );

            }
            else
                throw new NotImplementedException( "TemplateBundle(id:" + templateBundleId + ")" + lang.get( "exTemplateNotFound" ) );

            db.insert( feed );


        }

        // 根据数据，确定使用单行模板数组中的哪个模板？
        private String getTitleTemplate( List<OneLineStoryTemplate> onelineTemplates, String templateData, int dataKeyCount ) {

            foreach (OneLineStoryTemplate tpl in onelineTemplates) {
                int varCount = getVarCount( tpl.Title );
                if (dataKeyCount == varCount) return tpl.Title;
            }

            return null;
        }

        private static int getVarCount( String str ) {

            char[] arrChar = str.ToCharArray();
            int scount = 0;

            for (int i = 0; i < arrChar.Length; i++) {
                if (i == 0) continue;
                if (arrChar[i - 1] == '{' && arrChar[i] == '*') scount++;
            }
            return scount;
        }

        //----------------------------------------------- 内部方法 --------------------------------------------------------------------

        public virtual List<Feed> GetUserFeeds( int count, int userId ) {
            if (count <= 0) count = 10;
            List<Feed> list = db.find<Feed>( "Creator.Id=" + userId + " " ).list( count );
            //mergeCommentsPrivate( list );
            return list;
        }

        public virtual DataPage<Feed> GetByUser( int userId, String dataType ) {
            return GetByUser( userId, dataType, 20 );
        }

        private static readonly ILog logger = LogManager.GetLogger( typeof( FeedService ) );

        public virtual DataPage<Feed> GetByUser( int userId, String dataType, int pageSize ) {

            String ids = getFriendAndFollowingIds( userId );

            if (strUtil.IsNullOrEmpty( ids )) return DataPage<Feed>.GetEmpty();

            DataPage<Feed> list;

            if (strUtil.IsNullOrEmpty( dataType )) {
                list = db.findPage<Feed>( "CreatorId in (" + ids + ")", pageSize );
            }
            else {
                list = db.findPage<Feed>( "CreatorId in (" + ids + ") and DataType='" + dataType + "'", pageSize );
            }

            //mergeCommentsPrivate( list.Results );
            return list;
        }

        //--------------------------------------

        //private void mergeCommentsPrivate( List<Feed> list ) {
        //    String feedIds = getFeedIds( list );
        //    if (strUtil.IsNullOrEmpty( feedIds )) return;
        //    List<FeedComment> comments = FeedComment.find( "RootId in (" + feedIds + ") order by Id" ).list();
        //    mergeComments( list, comments );
        //}

        //private void mergeComments( List<Feed> list, List<FeedComment> comments ) {
        //    foreach (Feed feed in list) {
        //        List<FeedComment> clist = getCommentsByFeed( comments, feed );
        //        feed.setComments( clist );
        //    }
        //}

        //private List<FeedComment> getCommentsByFeed( List<FeedComment> comments, Feed feed ) {
        //    List<FeedComment> results = new List<FeedComment>();
        //    foreach (FeedComment c in comments) {
        //        if (c.RootId == feed.Id) results.Add( c );
        //    }
        //    return results;
        //}

        //private String getFeedIds( List<Feed> list ) {
        //    StringBuilder sb = new StringBuilder();
        //    for (int i = 0; i < list.Count; i++) {
        //        sb.Append( list[i].Id );
        //        if (i < list.Count - 1) sb.Append( "," );
        //    }
        //    return sb.ToString();
        //}
        //--------------------------------------


        private String getFriendAndFollowingIds( int userId ) {
            String friendIds = friendService.FindFriendsIds( userId );
            String followingIds = followerService.GetFollowingIds( userId );

            String ids = strUtil.Join( friendIds, followingIds, "," );
            ids = strUtil.Join( ids, userId.ToString(), "," );
            ids = ids.Trim().TrimEnd( ',' ).TrimStart( ',' );
            return ids;
        }

        public virtual Boolean IsUserIdValid( int userId, int ownerId ) {
            if (userId == ownerId) return true;
            String ids = getFriendAndFollowingIds( ownerId );
            if (strUtil.IsNullOrEmpty( ids )) return false;
            int[] arrIds = cvt.ToIntArray( ids );
            foreach (int id in arrIds) {
                if (userId == id) return true;
            }
            return false;
        }

        public virtual DataPage<Feed> GetUserSelf( int userId, String dataType ) {
            return GetUserSelf( userId, dataType, 20 );
        }

        public virtual DataPage<Feed> GetUserSelf( int userId, String dataType, int pageSize ) {
            DataPage<Feed> list;
            if (strUtil.IsNullOrEmpty( dataType )) 
                list = db.findPage<Feed>( "CreatorId=" + userId, pageSize );
            else
                list = db.findPage<Feed>( "CreatorId=" + userId + " and DataType='" + dataType + "'", pageSize );
            //mergeCommentsPrivate( list.Results );
            return list;
        }

        public virtual String GetHtmlValue( String template, String templateData, String actorInfo ) {

            templateData = templateData.Trim().Replace( "\r", "" ).Replace( "\n", "" );

            JsonObject data = Json.ParseJson( templateData );

            String result = template;
            String creatorKey = "{*actor*}";
            if (strUtil.HasText( actorInfo ))
                result = result.Replace( creatorKey, actorInfo );

            foreach (String key in data.Keys) {
                String rkey = "{*" + key + "*}";
                result = result.Replace( rkey, data[key].ToString() );
            }

            return result;
        }

        public virtual int GetTemplateBundleCount() {
            return db.count<TemplateBundle>();
        }

        public virtual void DeleteOne( int feedId ) {
            Feed feed = Feed.findById( feedId );
            if (feed == null) return;
            feed.delete();
        }

        // 每日清除过期feed
        public virtual void ClearFeeds() {

            Boolean isClearFeeds = config.Instance.Site.FeedKeepDay > 0;

            if (isClearFeeds) {

                DateTime lastClearTime = config.Instance.Site.LastFeedClearTime;
                if (cvt.IsDayEqual( lastClearTime, DateTime.Now )) return;

                Feed feed = new Feed();
                EntityInfo ei = Entity.GetInfo( feed );
                String table = ei.TableName;

                // TODO 支持其他数据库类型，
                int dayCount = config.Instance.Site.FeedKeepDay;
                String sql = "";
                DatabaseType dbtype = ei.DbType;
                if (dbtype == DatabaseType.SqlServer)
                    sql = "delete from " + table + " where datediff(day, created, getdate())>" + dayCount;
                else if (dbtype == DatabaseType.Access)
                    sql = "delete from " + table + " where datediff('d', created, now())>" + dayCount;
                else if (dbtype == DatabaseType.MySql)
                    sql = "delete from " + table + " where datediff(created, now())>" + dayCount;
                else
                    throw new NotImplementedException( "not implemented database function : datediff" );

                db.RunSql<Feed>( sql );

                config.Instance.Site.Update( "LastFeedClearTime", DateTime.Now );

            }
        }


        public virtual DataPage<Feed> GetAll( String dataType, int pageSize ) {
            if (strUtil.HasText( dataType ))
                return db.findPage<Feed>( "DataType='" + dataType + "'", pageSize );
            else
                return db.findPage<Feed>( "", pageSize );
        }

        public virtual DataPage<Feed> GetAll( int userId, String dataType, int pageSize ) {
            if (strUtil.HasText( dataType ))
                return db.findPage<Feed>( "CreatorId=" + userId + " and  DataType='" + dataType + "'", pageSize );
            else
                return db.findPage<Feed>( "CreatorId=" + userId, pageSize );
        }



        public void SetCommentCount( IEntity target ) {
            Feed feed = Feed.find( "DataType=:dtype and DataId=:did" )
               .set( "dtype", target.GetType().FullName )
               .set( "did", target.Id )
               .first();
            if (feed == null) {
                return;
            }

            ContextCache.Remove( target.GetType().FullName, target.Id );
            IEntity obj = ndb.findById( target.GetType(), target.Id );
            feed.Replies = (int)obj.get( "Replies" );
            feed.update( "Replies" );
        }

    }


}
