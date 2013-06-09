/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Common.Comments;

using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Interface;
using wojilu.Apps.Blog.Service;
using wojilu.Caching;
using wojilu.Web.Context;
using wojilu.Members.Users.Domain;
using wojilu.Web.Controller.Blog.Caching;
using wojilu.Web.Controller.Common.Caching;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Controller.Blog {

    public partial class LayoutController : ControllerBase {

        public IBlogService blogService { get; set; }
        public IBlogCategoryService categoryService { get; set; }
        public IBlogPostService postService { get; set; }
        public IBlogrollService rollService { get; set; }
        public IOpenCommentService commentService { get; set; }

        public LayoutController() {

            blogService = new BlogService();
            postService = new BlogPostService();
            categoryService = new BlogCategoryService();
            rollService = new BlogrollService();
            commentService = new OpenCommentService();
        }

        [CacheAction( typeof( BlogLayoutCache ) )]
        public override void Layout() {

            BlogApp blog = ctx.app.obj as BlogApp;
            if (blog == null) throw new NullReferenceException( "BlogApp" );
            blogService.AddHits( blog );

            set( "adminUrl", to( new Admin.MyListController().Index ) );
            bindAdminLink();


            bindAppInfo( blog );

            load( "blog.UserMenu", new Users.ProfileController().UserMenu );

            BlogSetting s = blog.GetSettingsObj();

            List<BlogCategory> categories = categoryService.GetByApp( ctx.app.Id );
            List<BlogPost> newBlogs = postService.GetNewBlog( ctx.app.Id, s.NewBlogCount );

            List<OpenComment> comments = commentService.GetByApp( typeof( BlogPost ), ctx.app.Id, s.NewCommentCount );

            List<Blogroll> blogrolls = rollService.GetByApp( ctx.app.Id, ctx.owner.obj.Id );

            bindBlogroll( blogrolls );
            bindCategories( categories );
            bindPostList( newBlogs );
            bindComments( comments );
        }

        private void bindAdminLink() {

            set( "friendsBlogLink", to( new Admin.BlogController().Friends, -1 ) );
            set( "myBlogLink", to( new Admin.MyListController().My ) );
            set( "addBlogLink", to( new Admin.PostController().Add ) );
            set( "categoryLink", to( new Admin.CategoryController().List ) );
            set( "blogrollLink", to( new Admin.BlogrollController().AdminList ) );

            set( "draftLink", to( new Admin.DraftController().Draft ) );
            set( "trashLink", to( new Admin.TrashController().Trash ) );

            set( "settingLink", to( new Admin.SettingController().Index ) );

        }


    }



}
