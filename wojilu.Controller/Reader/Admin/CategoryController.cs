/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Reader.Interface;
using wojilu.Web.Controller.Common;
using wojilu.Apps.Reader.Service;
using wojilu.Apps.Reader.Domain;

namespace wojilu.Web.Controller.Reader.Admin {

    [App( typeof( ReaderApp ) )]
    public class CategoryController : ControllerBase {

        public virtual IFeedCategoryService categoryService { get; set; }
        public virtual ISubscriptionService subscriptionService { get; set; }
        public virtual IFeedEntryService entryService { get; set; }

        public CategoryController() {
            HideLayout( typeof( Reader.LayoutController ) );
            categoryService = new FeedCategoryService();
            subscriptionService = new SubscriptionService();
            entryService = new FeedEntryService();
        }

        public override void Layout() {
        }

        public virtual void Show( long id ) {

            FeedCategory category = categoryService.GetById( id );
            bindCategory( category );

            String feedIds = subscriptionService.GetFeedIdsByCategoryId( id );

            if (strUtil.IsNullOrEmpty( feedIds )) {
                content( "<div style=\"margin:30px;\" class=\"warning\">no items</div>" );
                return;
            }

            DataPage<FeedEntry> list = entryService.GetPage( feedIds );

            bindItemList( id, category, list );
        }

        private void bindCategory( FeedCategory category ) {
            bind( "c", category );
        }

        private void bindItemList( long id, FeedCategory category, DataPage<FeedEntry> list ) {
            set( "feed.Title", category.Name );
            set( "feed.Link", to( Show, id ) );
            set( "LastRefreshTime", "" );

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

    }

}
