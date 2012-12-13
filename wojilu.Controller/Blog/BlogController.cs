/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Service;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Blog.Interface;
using wojilu.Web.Controller.Common;
using wojilu.Web.Controller.Blog.Caching;

namespace wojilu.Web.Controller.Blog {

    [App( typeof( BlogApp ) )]
    public partial class BlogController : ControllerBase {

        public IBlogPostService postService { get; set; }

        public BlogController() {
            postService = new BlogPostService();
        }
        
        [CacheAction( typeof( BlogIndexCache ) )]
        public void Index() {

            ctx.Page.Title = lang( "blog" );

            BlogApp app = ctx.app.obj as BlogApp;
            if (app == null) {
                echoError( "app不存在" );
                return;
            }

            BlogSetting s = app.GetSettingsObj();

            DataPage<BlogPost> results = postService.GetPage( ctx.app.Id, s.PerPageBlogs );

            IBlock block = getBlock( "topblog" );
            if (ctx.route.page == 1) {
                bindTopPosts( s, block );
            }

            bindPosts( results, s );

            set( "pager", results.PageBar );

        }

        public void Rss() {

            BlogApp app = ctx.app.obj as BlogApp;
            BlogSetting s = app.GetSettingsObj();

            RssChannel c = postService.GetRssByAppId( ctx.app.Id, s.RssCount );
            c.Generator = ctx.url.SiteUrl;

            c.Link = strUtil.Join( ctx.url.SiteUrl, c.Link );
            foreach (RssItem i in c.RssItems) {
                i.Link = strUtil.Join( ctx.url.SiteUrl, i.Link );
            }

            ctx.RenderXml( c.GetRenderContent() );
        }


    }
}

