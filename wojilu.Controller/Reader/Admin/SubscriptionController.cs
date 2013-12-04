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
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Reader.Admin {

    [App( typeof( ReaderApp ) )]
    public class SubscriptionController : ControllerBase {


        public virtual IFeedCategoryService categoryService { get; set; }
        public virtual IFeedSourceService feedService { get; set; }
        public virtual ISubscriptionService subscriptionService { get; set; }
        public virtual IFeedEntryService entryService { get; set; }

        public SubscriptionController() {
            categoryService = new FeedCategoryService();
            feedService = new FeedSourceService();
            subscriptionService = new SubscriptionService();
            entryService = new FeedEntryService();
            base.HideLayout( typeof( Reader.LayoutController ) );
        }

        public override void Layout() {
        }

        public virtual void Index() {

            List<Subscription> list =subscriptionService.GetByApp( ctx.app.Id );
            bindSubscribeList( list );
            set( "sortAction", to( SaveSort ) );

            set( "addSubscriptionLink", to( Add ) );
        }


        [HttpPost, DbTransaction]
        public virtual void SaveSort() {

            long id = ctx.PostLong( "id" );
            String cmd = ctx.Post( "cmd" );

            Subscription subscription = subscriptionService.GetById( id );
            List<Subscription> list = subscriptionService.GetByApp( ctx.app.Id );

            if (cmd == "up") {

                new SortUtil<Subscription>( subscription, list ).MoveUp();
                echoJsonOk();
            }
            else if (cmd == "down") {

                new SortUtil<Subscription>( subscription, list ).MoveDown();
                echoJsonOk();
            }
            else {
                echoError( lang( "exUnknowCmd" ) );
            }

        }


        public virtual void Add() {

            List<FeedCategory> categories = categoryService.GetByApp( ctx.app.Id );
            if (categories.Count == 0) {
                echoRedirect( "please add a category first", new FeedCategoryController().Add );
                return;
            }

            target( Create );
            dropList( "CategoryId", categories, "Name=Id", categories[0].Id );
        }

        [HttpPost, DbTransaction]
        public virtual void Create() {

            String rssLink = ctx.Post( "Link" );
            if (strUtil.IsNullOrEmpty( rssLink )) {
                echoError( "rss url can not be empty" );
                return;
            }

            long categoryId = ctx.PostLong( "CategoryId" );
            FeedCategory category = categoryService.GetById( categoryId );
            String name = ctx.Post( "Name" );

            Subscription s= feedService.Create( rssLink, category, name, 0, (User)ctx.owner.obj );

            redirect( Show, s.Id );
        }

        public virtual void Edit( long id ) {

            Subscription sf = subscriptionService.GetById( id );
            if (sf == null) {
                echoError( "feed is not exists" );
                return;
            }

            List<FeedCategory> categories = categoryService.GetByApp( ctx.app.Id );
            bindEdit( sf, categories );
            target( Update, id );
        }

        [HttpPost, DbTransaction]
        public virtual void Update( long id ) {

            Subscription sf = subscriptionService.GetById( id );
            if (sf == null) {
                echoError( "feed is not exists" );
                return;
            }

            sf.Name = ctx.Post( "Name" );
            //sf.OrderId = ctx.PostInt( "OrderId" );

            long categoryId = ctx.PostLong( "CategoryId" );
            sf.Category = new FeedCategory( categoryId );

            subscriptionService.Update( sf );

            redirect( Index );
        }

        [HttpDelete, DbTransaction]
        public virtual void Delete( long id ) {

            Subscription sf = subscriptionService.GetById( id );
            if (sf == null) {
                echoError( "feed is not exists" );
                return;
            }

            subscriptionService.Delete( sf );
            echoRedirect( lang( "opok" ) );
        }


        private void bindSubscribeList( List<Subscription> list ) {
            IBlock block = getBlock( "list" );
            foreach (Subscription sf in list) {

                if (sf == null || sf.FeedSource == null) continue;

                block.Set( "feed.Id", sf.Id );
                block.Set( "feed.OrderId", sf.OrderId );
                block.Set( "feed.Name", sf.Name );
                block.Set( "feed.Title", sf.FeedSource.Title );
                block.Set( "feed.LinkEdit", to( Edit, sf.Id ) );
                block.Set( "feed.LinkDelete", to( Delete, sf.Id ) );

                block.Next();
            }
        }

        private void bindEdit( Subscription sf, List<FeedCategory> categories ) {

            set( "feed.Title", sf.FeedSource.Title );
            set( "feed.Name", sf.Name );
            set( "feed.RssLink", sf.FeedSource.FeedLink );
            set( "feed.OrderId", sf.OrderId.ToString() );
            dropList( "CategoryId", categories, "Name=Id", sf.Category.Id );
        }



        public virtual void Show( long id ) {

            Subscription s = subscriptionService.GetById( id );
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
