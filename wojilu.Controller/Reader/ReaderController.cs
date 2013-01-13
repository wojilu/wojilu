/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Reader.Interface;
using wojilu.Apps.Reader.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Reader.Service;
using wojilu.Web.Controller.Common;

namespace wojilu.Web.Controller.Reader {

    [App( typeof( ReaderApp ) )]
    public partial class ReaderController : ControllerBase {

        public IFeedCategoryService categoryService { get; set; }
        public IFeedSourceService feedService { get; set; }
        public ISubscriptionService subscriptionService { get; set; }
        public IFeedEntryService entryService { get; set; }

        public ReaderController() {
            categoryService = new FeedCategoryService();
            feedService = new FeedSourceService();
            subscriptionService = new SubscriptionService();
            entryService = new FeedEntryService();
        }

        public void Index() {

            ctx.Page.Title = "feed 订阅";


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


        public void Users( int feedId ) {

            FeedSource feed = feedService.GetById( feedId );
            DataPage<Subscription> list = subscriptionService.GetPage( feedId );
            bindUserList( feed, list );
        }



    }

}
