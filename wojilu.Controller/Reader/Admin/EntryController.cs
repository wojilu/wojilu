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

namespace wojilu.Web.Controller.Reader.Admin {

    [App( typeof( ReaderApp ) )]
    public class EntryController : ControllerBase {

        public IFeedEntryService entryService { get; set; }

        public EntryController() {
            entryService = new FeedEntryService();
            base.HideLayout( typeof( Reader.LayoutController ) );
        }

        public void Show( int id ) {

            FeedEntry item = entryService.GetById( id );

            if (item == null) {
                echoRedirect( "feed item is not exists" );
            }

            entryService.AddHits( item );
            bindItem( item );
        }

        private void bindItem( FeedEntry item ) {
            set( "item.Title", item.Title );
            set( "item.Hits", item.Hits );
            set( "item.Link", item.Link );
            set( "item.PubDate", item.PubDate );
            set( "item.Description", item.Description );

            set( "item.FeedAuthor", item.FeedSource.Title );
            set( "item.FeedLink", item.FeedSource.Link );
        }

    }

}
