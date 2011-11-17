/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Reader.Domain;
using wojilu.Apps.Reader.Interface;
using wojilu.Apps.Reader.Service;

namespace wojilu.Web.Controller.Reader.Admin {

    [App( typeof( ReaderApp ) )]
    public class ReaderController : ControllerBase {

        public IFeedCategoryService categoryService { get; set; }
        public IFeedSourceService feedService { get; set; }
        public ISubscriptionService subscriptionService { get; set; }
        public IFeedEntryService entryService { get; set; }


        public ReaderController() {
            HideLayout( typeof( Reader.LayoutController ) );

            categoryService = new FeedCategoryService();
            feedService = new FeedSourceService();
            subscriptionService = new SubscriptionService();
            entryService = new FeedEntryService();

        }

        public override void Layout() {
        }

        public void Index() {
            String feedIds = subscriptionService.GetFeedIdsByAppId( ctx.app.Id );
            DataPage<FeedEntry> list = entryService.GetPage( feedIds );
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
            set( "LastRefreshTime", f.LastRefreshTime );

            //set( "feed.MemberListUrl", to( Users, f.Id ) );
            set( "feed.UserCount", f.UserCount );

        }

    }

}
