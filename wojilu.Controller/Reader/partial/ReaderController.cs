/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Reader.Domain;

namespace wojilu.Web.Controller.Reader {

    public partial class ReaderController : ControllerBase {


//        private void bindFeedItem( FeedSource f, DataPage<FeedEntry> list ) {

//            String feedTitle = f.Title;
//            String feedLink = f.Link;
//            String lastRefreshTime = f.LastRefreshTime.ToString( "g" );


//            set( "feed.Title", feedTitle );
//            set( "feed.Link", feedLink );
//            set( "LastRefreshTime", lastRefreshTime );

//            set( "feed.MemberListUrl", to( Users, f.Id ) );
//            set( "feed.UserCount", f.UserCount );

//            IBlock itemBlock = getBlock( "item" );

//            foreach (FeedEntry item in list.Results) {

//                itemBlock.Set( "item.Title", item.Title );
//                itemBlock.Set( "item.Link", to( new EntryController().Show, item.Id ) );
//                itemBlock.Set( "item.PubDate", item.PubDate.ToString() );
//                itemBlock.Set( "item.Description", item.Abstract );
//                itemBlock.Next();
//            }

//            set( "page", list.PageBar );
//        }


        private void bindUserList( FeedSource feed, DataPage<Subscription> list ) {

            set( "feed.Title", feed.Title );
            set( "feed.Link", feed.Link );

            IBlock block = getBlock( "list" );
            foreach (Subscription sf in list.Results) {
                block.Set( "member.Name", sf.User.Name );
                block.Set( "member.Face", sf.User.PicSmall );
                block.Set( "member.Url", toUser( sf.User ) );
                block.Next();
            }

            set( "feed.UserCount", list.RecordCount );
            set( "page", list.PageBar );
        }


//        private void bindSubscribeList( List<Subscription> list ) {
//            IBlock block = getBlock( "list" );
//            foreach (Subscription sf in list) {

//                block.Set( "feed.OrderId", sf.OrderId );
//                block.Set( "feed.Name", sf.Name );
//                block.Set( "feed.Title", sf.FeedSource.Title );
//                block.Next();
//            }
//        }


    }
}
