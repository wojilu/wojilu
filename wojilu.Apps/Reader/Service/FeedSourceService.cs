/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web;
using wojilu.Common;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Reader.Domain;
using wojilu.Apps.Reader.Interface;
using wojilu.ORM;

namespace wojilu.Apps.Reader.Service {

    public class FeedSourceService : IFeedSourceService {

        private static readonly ILog logger = LogManager.GetLogger( typeof( FeedSourceService ) );

        public ISubscriptionService subscriptionService { get; set; }
        public IFeedEntryService entryService { get; set; }

        public FeedSourceService() {
            subscriptionService = new SubscriptionService();
            entryService = new FeedEntryService();
        }

        public List<IBinderValue> GetFeed( String url, int count ) {

            if (strUtil.IsNullOrEmpty( url )) return new List<IBinderValue>();

            FeedSource feedsrc = GetByLink( url );
            if (feedsrc == null) return new List<IBinderValue>();

            if (count > 50 || count <= 0) count = 10;

            List<FeedEntry> list = FeedEntry.find( "FeedSource.Id=" + feedsrc.Id + " order by PubDate desc, Id desc" ).list( count );
            return getForList( list );
        }

        private List<IBinderValue> getForList( List<FeedEntry> list ) {

            List<IBinderValue> results = new List<IBinderValue>();
            foreach (FeedEntry post in list) {
                IBinderValue vo = new ItemValue();
                vo.CreatorName = post.Author;
                vo.Title = post.Title;
                vo.Link = post.Link;
                vo.Content = post.Description;
                vo.Created = post.Created;

                results.Add( vo );
            }

            return results;
        }

        public FeedSource CreateRss( String url ) {

            FeedSource fs = GetByLink( url );
            if (fs != null) return fs;

            try {
                FeedSource feed = createNewSource( url );
                saveFeedItems( feed );
                return feed;
            }
            catch (Exception ex) {
                logger.Error( "download rss:" + url );
                logger.Error( ex.ToString() );
                return null;
            }
        }

        public FeedSource CreateRss( String url, String name, FeedSysCategory category ) {
            FeedSource f = CreateRss( url );
            if (f == null) return null;

            if (strUtil.HasText( name )) f.Name = name;
            f.Category = category;
            f.update();

            return f;
        }

        private FeedSource createNewSource( String url ) {

            FeedSource feedsrc = new FeedSource();
            RssChannel rss = RssChannel.Create( url );

            feedsrc.RssChannel = rss;

            //-----------------------------------

            feedsrc.Title = rss.Title;
            feedsrc.FeedLink = url;
            feedsrc.Link = rss.Link;
            feedsrc.Description = rss.Description;
            feedsrc.BlogLanguage = rss.Language;

            feedsrc.LastBuildDate = rss.LastBuildDate;
            if (feedsrc.LastBuildDate == null || feedsrc.LastBuildDate < new DateTime( 1900, 1, 1 ))
                feedsrc.LastBuildDate = DateTime.Now;

            feedsrc.PubDate = rss.PubDate;
            if (feedsrc.PubDate == null || feedsrc.PubDate < new DateTime( 1900, 1, 1 ))
                feedsrc.PubDate = DateTime.Now;

            feedsrc.LastRefreshTime = DateTime.Now;
            feedsrc.Generator = rss.Generator;
            feedsrc.Created = DateTime.Now;

            db.insert( feedsrc );

            return feedsrc;
        }

        public FeedSource GetById( int feedId ) {
            return db.findById<FeedSource>( feedId );
        }

        public FeedSource GetByLink( String link ) {
            return db.find<FeedSource>( "FeedLink=:link" ).set( "link", link ).first();
        }

        public Subscription Create( String url, FeedCategory category, String name, int orderId, User user ) {

            FeedSource feedsrc = GetByLink( url );
            if (feedsrc == null) {
                feedsrc = createNewSource( url );
                saveFeedItems( feedsrc );
            }

            // 检查是否已经订阅
            Subscription s = subscriptionService.GetByFeedAndUser( feedsrc.Id, user.Id );
            if (s != null) return s;

            Subscription subscription = subscribe( category, name, orderId, user, feedsrc );
            return subscription;
        }

        private static Subscription subscribe( FeedCategory category, String name, int orderId, User user, FeedSource feedsrc ) {
            Subscription subscription = new Subscription();
            subscription.FeedSource = feedsrc;
            subscription.Category = category;
            subscription.User = user;
            subscription.Name = strUtil.HasText( name ) ? name : feedsrc.Title;
            subscription.AppId = category.AppId;
            subscription.OrderId = orderId;

            db.insert( subscription );

            return subscription;
        }

        public void DownloadBlogItems( FeedSource f ) {
            downalodFeed( f );
        }

        private void downalodFeed( object arg ) {

            FeedSource f = arg as FeedSource;

            RssItemList articles;
            try {
                logger.Info( "RssChannel.Create=>" + f.FeedLink );
                articles = RssChannel.Create( f.FeedLink ).RssItems;
            }
            catch (Exception ex) {
                logger.Error( "download rss:" + f.FeedLink );
                logger.Error( ex.Message );
                return;
            }

            //最后更新时间
            f.LastRefreshTime = DateTime.Now;
            db.update( f, "LastRefreshTime" );
            logger.Info( "download feed ok" );

            RssChannel c = new RssChannel();
            c.RssItems = articles;
            f.RssChannel = c;

            saveFeedItems( f );
        }


        private void saveFeedItems( FeedSource channel ) {


            RssItemList posts = channel.RssChannel.RssItems;
            for (int i = posts.Count - 1; i >= 0; i--) {

                RssItem post = posts[i];
                FeedEntry entry = new FeedEntry();

                entry.FeedSource = channel;

                entry.Author = post.Author;
                entry.Category = post.Category;

                entry.Description = post.Description;
                entry.Link = post.Link;
                entry.PubDate = post.PubDate;
                if (entry.PubDate == null || entry.PubDate < new DateTime( 1900, 1, 1 ))
                    entry.PubDate = DateTime.Now;
                entry.Title = post.Title;
                entry.Created = DateTime.Now;

                FeedEntry savedEntry = entryService.GetByLink( entry.Link );
                if (savedEntry == null) {
                    db.insert( entry );
                }

            }
        }

        public List<FeedSource> GetUnRefreshedList( int minutes ) {

            DateTime t = DateTime.Now.AddMinutes( -minutes );

            EntityInfo ei = Entity.GetInfo( typeof( FeedSource ) );
            String tq = ei.Dialect.GetTimeQuote();

            return db.find<FeedSource>( "LastRefreshTime<" + tq + t + tq ).list();

        }

        public DataPage<FeedSource> GetPage( int pageSize ) {
            return db.findPage<FeedSource>( "order by OrderId desc, Id asc ", pageSize );
        }

        public List<FeedSource> GetAll() {
            return db.find<FeedSource>( "order by OrderId desc, Id asc " ).list();
        }

        public void Update( FeedSource f ) {
            f.update();
        }

        public Result Delete( FeedSource f ) {
            List<Subscription> list = db.find<Subscription>( "FeedSource.Id=" + f.Id ).list();
            if (list.Count > 0) {
                return new Result( "can not delete this feed : been subscribed by others" );
            }
            else {
                return new Result();
            }
        }



    }
}
