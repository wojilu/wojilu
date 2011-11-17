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
        public ICommentService<BlogPostComment> commentService { get; set; }
        public IBlogPostService postService { get; set; }
        public IBlogrollService rollService { get; set; }

        public LayoutController() {

            blogService = new BlogService();
            postService = new BlogPostService();
            categoryService = new BlogCategoryService();
            rollService = new BlogrollService();
            commentService = new CommentService<BlogPostComment>();
        }

        [CacheAction( typeof( BlogLayoutCache ) )]
        public override void Layout() {

            BlogApp blog = ctx.app.obj as BlogApp;
            blogService.AddHits( blog );

            set( "adminUrl", to( new Admin.MyListController().Index ) );


            bindAppInfo( blog );

            load( "blog.UserMenu", new Users.ProfileController().UserMenu );

            BlogSetting s = blog.GetSettingsObj();

            List<BlogCategory> categories = categoryService.GetByApp( ctx.app.Id );
            List<BlogPost> newBlogs = postService.GetNewBlog( ctx.app.Id, s.NewBlogCount );

            //List<BlogPostComment> newComments = commentService.GetNew( ctx.owner.Id, ctx.app.Id, s.NewCommentCount );
            List<BlogPostComment> newComments = BlogPostComment.find( "AppId=" + ctx.app.Id ).list( s.NewCommentCount );

            List<Blogroll> blogrolls = rollService.GetByApp( ctx.app.Id, ctx.owner.obj.Id );

            bindBlogroll( blogrolls );
            bindCategories( categories );
            bindPostList( newBlogs );
            bindComments( newComments );
        }


    }



}
