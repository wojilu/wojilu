/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps.Blog.Service;
using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Interface;
using wojilu.Apps.Blog.Interface;
using wojilu.Apps.Blog.Service;

namespace wojilu.Web.Controller.Admin.Apps.Blog {

    [App( typeof( BlogApp ) )]
    public class LayoutController : ControllerBase {

        public ISysBlogService sysblogService { get; set; }
        public IBlogSysCategoryService categoryService { get; set; }

        public LayoutController() {
            sysblogService = new SysBlogService();
            categoryService = new BlogSysCategoryService();
        }

        public override void Layout() {
            set( "listLink", to( new MainController().Index, 0 ) );
            set( "pickedLink", to( new MainController().Picked ) );
            set( "trashLink", to( new MainController().Trash ) );
            set( "commentLink", to( new CommentController().List )+"?type=" + typeof(BlogPostComment).FullName );

            int trashCount = sysblogService.GetSystemDeleteCount();
            set( "trashCount", trashCount );
            set( "categoryLink", to( new SysCategoryController().List ) );

            IList categories = categoryService.GetAll();
            bindList( "categories", "c", categories, bindLink );

        }

        private void bindLink( IBlock tpl, int id ) {
            tpl.Set( "c.LinkCategory", to( new MainController().Index, id ) );
        }


    }

}
