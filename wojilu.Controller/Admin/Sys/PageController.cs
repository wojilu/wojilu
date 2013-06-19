/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Common.Pages.Service;
using wojilu.Common.Pages.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Common.Pages.Interface;
using wojilu.Members.Sites.Interface;
using wojilu.Members.Sites.Service;
using wojilu.Web.Controller.Security;
using wojilu.Members.Interface;
using wojilu.Members.Sites.Domain;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Admin.Sys {

    public partial class PageController : ControllerBase {

        public IPageService pageService { get; set; }
        public IAdminLogService<SiteLog> logService { get; set; }

        public PageController() {
            pageService = new PageService();
            logService = new SiteLogService();
        }

        public override void Layout() {

            List<PageCategory> categories = pageService.GetCategories( ctx.owner.obj );
            bindList( "lists", "data", categories, bindCategoryLink );
            set( "categoryLink", to( new PageCategoryController().List ) );
        }

        public void List( int categoryId ) {

            set( "addLink", to( Add, categoryId ) );
            set( "sortAction", to( SaveSort, categoryId ) );

            PageCategory category = pageService.GetCategoryById( categoryId, ctx.owner.obj );
            set( "category.Name", category.Name );

            bindTransDrop( category );

            List<Page> list = pageService.GetPosts( ctx.owner.obj, categoryId );

            bindPages( list );
        }

        [HttpPost]
        public void TransPage( int categoryId ) {
            String ids = ctx.PostIdList( "ids" );
            if (strUtil.IsNullOrEmpty( ids )) {
                echoText( "请先选择" );
                return;
            }

            pageService.UpdateCategory( ids, categoryId );

            echoAjaxOk();
        }

        private void bindTransDrop( PageCategory category ) {

            List<PageCategory> xlist = getTransList( category );
            xlist.ForEach( x => x.data["TransLink"] = to( TransPage, x.Id ) );
            bindList( "transList", "x", xlist );

        }

        private List<PageCategory> getTransList( PageCategory category ) {
            List<PageCategory> list = pageService.GetCategories( ctx.owner.obj );
            List<PageCategory> xlist = new List<PageCategory>();
            foreach (PageCategory x in list) {
                if (x.Id != category.Id) {
                    xlist.Add( x );
                }
            }
            return xlist;
        }


        [HttpPost]
        public virtual void SaveSort( int categoryId ) {

            int id = ctx.PostInt( "id" );
            String cmd = ctx.Post( "cmd" );

            Page data = pageService.GetPostById( id, ctx.owner.obj );

            List<Page> list = pageService.GetPosts( ctx.owner.obj, categoryId );
            // 在同一个层级排序
            List<Page> xlist = filterByParent( list, data.ParentId );

            if (cmd == "up") {
                new SortUtil<Page>( data, xlist ).MoveUp();
                echoJsonOk();
            }
            else if (cmd == "down") {

                new SortUtil<Page>( data, xlist ).MoveDown();
                echoJsonOk();
            }
            else {
                echoError( lang( "exUnknowCmd" ) );
            }
        }

        private List<Page> filterByParent( List<Page> list, int parentId ) {

            List<Page> xlist = new List<Page>();
            foreach (Page x in list) {
                if (x.ParentId == parentId) {
                    xlist.Add( x );
                }
            }
            return xlist;
        }


        public void ViewUrl( int id ) {
            Page p = pageService.GetPostById( id, ctx.owner.obj );
            set( "page.Title", p.Title );
            set( "page.Url", plink( id ) );
        }


        //-----------------------------------------------------------------------------

        public void Add( int id ) {
            target( Create, id );
            PageCategory category = pageService.GetCategoryById( id, ctx.owner.obj );
            set( "category", category.Name );
        }

        [HttpPost, DbTransaction]
        public void Create( int id ) {

            Page data = validate( new Page() );
            if (ctx.HasErrors) { run( Add, id ); return; }

            data.Category = new PageCategory { Id = id };

            populateOwner( data );

            pageService.Insert( data );

            if (ctx.HasErrors) { run( Add, id ); return; }


            log( SiteLogString.AddPage(), data );

            golist( data.Category.Id );
        }

        private void populateOwner( Page data ) {
            data.OwnerId = ctx.owner.Id;
            data.OwnerType = ctx.owner.obj.GetType().FullName;
            data.OwnerUrl = ctx.owner.obj.Url;
            data.Creator = (User)ctx.viewer.obj;
        }

        //-----------------------------------------------------------------------------

        public void AddSubPage( int id ) {
            Page page = pageService.GetPostById( id, ctx.owner.obj );
            if (page == null) {
                echoRedirect( lang( "exPageNotFound" ) );
                return;
            }

            target( CreateSubPage, id );
            set( "pageName", page.Name );
        }

        [HttpPost, DbTransaction]
        public void CreateSubPage( int id ) {

            Page page = pageService.GetPostById( id, ctx.owner.obj );
            if (page == null) {
                echoRedirect( lang( "exParentPageNotFound" ) );
                return;
            }

            Page data = validate( new Page() );
            if (ctx.HasErrors) { run( AddSubPage, id ); return; }

            data.ParentId = id;
            data.Category = new PageCategory { Id = page.Category.Id };

            populateOwner( data );

            pageService.Insert( data );

            log( SiteLogString.AddPage(), data );

            golist( data.Category.Id );
        }

        //-----------------------------------------------------------------------------

        public void Edit( int id ) {

            target( Update, id );

            Page data = pageService.GetPostById( id, ctx.owner.obj );
            if (data == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            Tree<Page> tree = new Tree<Page>( pageService.GetPosts( ctx.owner.obj, data.Category.Id ) );

            bindForm( data, tree );
        }

        [HttpPost, DbTransaction]
        public void Update( int id ) {

            Page data = pageService.GetPostById( id, ctx.owner.obj );
            if (data == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            data = validate( data );
            if (ctx.HasErrors) { run( Edit, id ); return; }

            int ParentId = ctx.PostInt( "ParentId" );
            data.ParentId = ParentId;

            pageService.Update( data );

            log( SiteLogString.UpdatePage(), data );

            golist( data.Category.Id );
        }

        //-----------------------------------------------------------------------------

        [HttpDelete, DbTransaction]
        public void Delete( int id ) {

            Page data = pageService.GetPostById( id, ctx.owner.obj );
            if (data == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            pageService.Delete( data );
            log( SiteLogString.DeletePage(), data );

            golist( data.Category.Id );
        }

        private void log( String msg, Page data ) {
            String dataInfo = "{Id:" + data.Id + ", Title:'" + data.Title + "'}";
            logService.Add( (User)ctx.viewer.obj, msg, dataInfo, typeof( Page ).FullName, ctx.Ip );
        }

    }

}
