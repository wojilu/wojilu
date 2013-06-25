/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Reader.Interface;
using wojilu.Apps.Reader.Service;
using wojilu.Apps.Reader.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Web.Controller.Common;

namespace wojilu.Web.Controller.Reader {

    [App( typeof( ReaderApp ) )]
    public class SubscriptionController : ControllerBase {


        public IFeedCategoryService categoryService { get; set; }
        public IFeedSourceService feedService { get; set; }
        public ISubscriptionService subscriptionService { get; set; }
        public IFeedEntryService entryService { get; set; }

        public SubscriptionController() {
            categoryService = new FeedCategoryService();
            feedService = new FeedSourceService();
            subscriptionService = new SubscriptionService();
            entryService = new FeedEntryService();
        }



        public void Show( int id ) {

            Subscription s = subscriptionService.GetById( id );
            if (s == null || s.FeedSource == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            ctx.Page.Title = s.Name;

            DataPage<FeedEntry> list = entryService.GetPage( s.FeedSource.Id );
            bindFeedInfo( s.FeedSource );
            bindFeedItem( list );
        }


        private void bindFeedItem( DataPage<FeedEntry> list ) {

            IBlock itemBlock = getBlock( "item" );

            foreach (FeedEntry item in list.Results) {

                itemBlock.Set( "item.Title", item.Title );
                itemBlock.Set( "item.Link", to( new EntryController().Show, item.Id ) );
                itemBlock.Set( "item.PubDate", item.PubDate );
                itemBlock.Set( "item.Description", item.Abstract );
                itemBlock.Next();
            }

            set( "page", list.PageBar );
        }

        private void bindFeedInfo( FeedSource f ) {
            set( "feed.Title", f.Title );
            set( "feed.Link", f.Link );
            set( "feed.RssLink", f.FeedLink );
            set( "feed.LastRefreshTime", f.LastRefreshTime );

            //set( "feed.MemberListUrl", to( Users, f.Id ) );
            set( "feed.UserCount", f.UserCount );

        }
    }
}
