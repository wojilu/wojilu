/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Common.AppBase;
using wojilu.Apps.Reader.Domain;
using wojilu.Apps.Reader.Interface;
using wojilu.Apps.Reader.Service;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Web.Controller.Admin.Apps.Reader {

    [App( typeof( ReaderApp ) )]
    public partial class MainController : ControllerBase {

        IFeedSourceService srcService { get; set; }
        IFeedSysCategoryService categoryService { get; set; }
        public IFeedSourceService feedService { get; set; }

        public MainController() {
            srcService = new FeedSourceService();
            categoryService = new FeedSysCategoryService();
            feedService = new FeedSourceService();
        }

        public void Index() {

            set( "addLink", to( Add ) );

            set( "sortAction", to( SaveSort ) );

            DataPage<FeedSource> list = srcService.GetPage( 500 );

            IBlock block = getBlock( "list" );
            foreach (FeedSource f in list.Results) {

                block.Set( "feed.Id", f.Id );
                block.Set( "feed.OrderId", f.OrderId );
                block.Set( "feed.Name", f.Name );
                block.Set( "feed.Title", f.Title );

                String categoryName = f.Category == null ? "" : f.Category.Name;
                block.Set( "feed.CategoryName", categoryName );

                block.Set( "feed.LinkEdit", to( Edit, f.Id ) );
                block.Set( "feed.LinkDelete", to( Delete, f.Id ) );

                block.Next();
            }

            set( "page", list.PageBar );
        }

        [HttpPost, DbTransaction]
        public virtual void SaveSort() {

            int id = ctx.PostInt( "id" );
            String cmd = ctx.Post( "cmd" );

            FeedSource target = srcService.GetById( id );
            List<FeedSource> list = srcService.GetAll();

            if (cmd == "up") {

                new SortUtil<FeedSource>( target, list ).MoveUp();
                echoJsonOk();
            }
            else if (cmd == "down") {

                new SortUtil<FeedSource>( target, list ).MoveDown();
                echoJsonOk();
            }
            else {
                echoError( lang( "exUnknowCmd" ) );
            }

        }

        public void Add() {

            List<FeedSysCategory> categories = categoryService.GetAll();
            if (categories.Count == 0) {
                echoRedirect( "you need add category first", new SysCategoryController().Add );
                return;
            }

            target( Create );
            dropList( "CategoryId", categories, "Name=Id", categories[0].Id );
        }

        [HttpPost, DbTransaction]
        public void Create() {

            String rssLink = ctx.Post( "Link" );
            if (strUtil.IsNullOrEmpty( rssLink )) {
                echoError( "rss url can not be empty" );
                return;
            }

            int categoryId = ctx.PostInt( "CategoryId" );
            FeedSysCategory category = categoryService.GetById( categoryId );
            String name = ctx.Post( "Name" );

            FeedSource f = feedService.CreateRss( rssLink, name, category );
            if (f == null) {
                echoError( "create rss error，please try again later" );
                return;
            }
            else
                redirect( Index );
        }

        public void Edit( int id ) {

            FeedSource f = feedService.GetById( id );
            if (f == null) {
                echoError( "feed is not exists" );
                return;
            }

            List<FeedSysCategory> categories = categoryService.GetAll();
            bindEdit( f, categories );
            target( Update, id );
        }

        private void bindEdit( FeedSource f, List<FeedSysCategory> categories ) {

            set( "feed.Title", f.Title );
            set( "feed.Name", f.Name );
            set( "feed.RssLink", f.FeedLink );
            int selected = f.Category == null ? 0 : f.Category.Id;
            dropList( "CategoryId", categories, "Name=Id", selected );
        }

        [HttpPost, DbTransaction]
        public void Update( int id ) {

            FeedSource f = feedService.GetById( id );
            if (f == null) {
                echoError( "feed is not exists" );
                return;
            }

            f.Name = ctx.Post( "Name" );
            int categoryId = ctx.PostInt( "CategoryId" );
            f.Category = new FeedSysCategory( categoryId );

            feedService.Update( f );

            redirect( Index );
        }

        [HttpDelete, DbTransaction]
        public void Delete( int id ) {

            FeedSource f = feedService.GetById( id );
            if (f == null) {
                echoError( "feed is not exists" );
                return;
            }

            Result result = feedService.Delete( f );
            if (result.IsValid)
                echoRedirect( lang( "opok" ) );
            else
                echoError( result );
        }



    }

}
