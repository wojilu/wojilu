/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Service;
using wojilu.Apps.Blog.Interface;
using wojilu.Web.Controller.Common;

namespace wojilu.Web.Controller.Blog.Admin {

    [App( typeof( BlogApp ) )]
    public class CategoryController : CategoryBaseController<BlogCategory> {

        private static readonly ILog logger = LogManager.GetLogger( typeof( CategoryController ) );

        public IBlogCategoryService categoryService { get; set; }
        public IBlogPostService postService { get; set; }

        public CategoryController() {
            categoryService = new BlogCategoryService();
            postService = new BlogPostService();
        }

        public void New() {
            target( Insert );
        }

        //--------------------------------------

        [HttpPost, DbTransaction]
        public void Insert() {

            BlogCategory category = BlogValidator.ValidateCategory( null, ctx );
            if (errors.HasErrors) {
                run( New );
                return;
            }

            categoryService.Insert( category );
            List<BlogCategory> categories = categoryService.GetByApp( category.AppId );
            dropList( "CategoryId", categories, "Name=Id", category.Id );
            set( "cid", category.Id );
        }

        [HttpDelete, DbTransaction]
        public override void Delete( int id ) {

            int count = postService.GetCountByCategory( id );
            if (count > 0) {
                String msg = string.Format( alang( "deleteCategoryTip" ), count );
                echo( msg );
            }
            else {
                base.Delete( id );
            }

        }


    }
}
