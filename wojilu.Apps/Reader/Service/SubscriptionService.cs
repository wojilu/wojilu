/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Reader.Domain;
using wojilu.Apps.Reader.Interface;

namespace wojilu.Apps.Reader.Service {

    public class SubscriptionService : ISubscriptionService {

        public virtual Subscription GetById(long id) {
            return db.findById<Subscription>( id );
        }

        public virtual void Update( Subscription subscription ) {
            db.update( subscription );
        }

        public virtual Subscription GetByFeedAndUser(long feedId, long userId) {
            return db.find<Subscription>( "FeedSource.Id=" + feedId + " and User.Id=" + userId ).first();
        }

        public virtual List<Subscription> GetByCategoryId(long categoryId) {
            return db.find<Subscription>( "Category.Id=" + categoryId ).list();
        }

        public virtual string GetFeedIdsByCategoryId(long categoryId) {
            List<Subscription> list = GetByCategoryId( categoryId );
            return getFeedIds( list );
        }

        private static String getFeedIds( List<Subscription> list ) {
            String result = "";
            if ( result==null || list.Count == 0) return result;
            foreach (Subscription myfeed in list) {
                if (myfeed == null || myfeed.FeedSource == null) continue;
                result += myfeed.FeedSource.Id + ",";
            }
            return result.TrimEnd( ',' );
        }

        public virtual string GetFeedIdsByAppId(long appId) {
            List<Subscription> list = GetByApp( appId );
            return getFeedIds( list );
        }

        public virtual List<Subscription> GetByApp(long appId) {
            return db.find<Subscription>( "AppId=" + appId + " order by OrderId desc, Id asc" ).list();
        }

        public virtual List<FeedSource> GetFeeds( List<Subscription> slist ) {
            List<FeedSource> results = new List<FeedSource>();
            foreach (Subscription subscription in slist) {
                results.Add( subscription.FeedSource );
            }
            return results;
        }

        public virtual List<Subscription> GetByCategoryId(List<Subscription> subscriptionList, long categoryId) {
            List<Subscription> results = new List<Subscription>();
            foreach (Subscription feed in subscriptionList) {
                if (feed.Category.Id == categoryId)
                    results.Add( feed );
            }
            return results;
        }



        public virtual DataPage<Subscription> GetPage(long feedId) {
            return db.findPage<Subscription>( "FeedSource.Id=" + feedId + " order by Id asc" );
        }

        public virtual void Delete( Subscription subscription ) {

            //int feedShipsTotalCount = db.find<SubscribedFeed>( "Feed.Id=" + myfeed.Feed.Id ).count();
            //if (feedShipsTotalCount == 1) {
            //    myfeed.Feed.Delete();
            //    new FeedItem().DeleteBatch( "FeedId=" + myfeed.Feed.Id );
            //}

            db.delete( subscription );

        }

    }
}
