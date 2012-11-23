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
using wojilu.Web.Controller.Common;

namespace wojilu.Web.Controller.Reader {

    [App( typeof( ReaderApp ) )]
    public class EntryController : ControllerBase {

        public IFeedEntryService entryService { get; set; }

        public EntryController() {
            entryService = new FeedEntryService();
        }

        public void Show( int id ) {

            FeedEntry item = entryService.GetById( id );

            if (item == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            ctx.Page.Title = item.Title;

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
