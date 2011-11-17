/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Service;
using wojilu.Apps.Blog.Interface;

namespace wojilu.Web.Controller.Blog {

    [App( typeof( BlogApp ) )]
    public partial class CategoryController : ControllerBase {

        public IBlogCategoryService categoryService { get; set; }
        public IBlogPostService postService { get; set; }

        public CategoryController() {
            postService = new BlogPostService();
            categoryService = new BlogCategoryService();
        }

        public void Show( int id ) {

            BlogCategory category = categoryService.GetById( id, ctx.owner.Id );
            if (category == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            BlogApp app = ctx.app.obj as BlogApp;
            BlogSetting s = app.GetSettingsObj();


            DataPage<BlogPost> list = postService.GetPageByCategory( ctx.app.Id, id, s.PerPageBlogs );

            bindCategory( category );
            bindPostList( list );

            set( "pager", list.PageBar );
        }


    }
}

