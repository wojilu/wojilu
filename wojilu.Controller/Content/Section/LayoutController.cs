/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;
using wojilu.Apps.Content.Enum;
using wojilu.Web.Controller.Common;
using wojilu.Web.Controller.Content.Caching;
using wojilu.Common;

namespace wojilu.Web.Controller.Content.Section {

    public partial class LayoutController : ControllerBase {

        public IContentPostService postService { get; set; }

        public LayoutController() {
            postService = new ContentPostService();
        }

        public override void Layout() {

            ContentApp app = ctx.app.obj as ContentApp;
            ContentSetting s = app.GetSettingsObj();

            List<ContentPost> posts = postService.GetRankPost( ctx.app.Id, s.RankPosts, PostCategory.Post );
            bindPosts( posts, "list" );

            List<ContentPost> imgs = postService.GetRankPost( ctx.app.Id, s.RankPics, PostCategory.Img );
            bindImgs( imgs, "img" );

            List<ContentPost> videos = postService.GetRankPost( ctx.app.Id, s.RankVideos, PostCategory.Video );
            bindVideos( videos, "video" );

            set( "adSidebarTop", AdItem.GetAdById( AdCategory.ArticleSidebarTop ) );
            set( "adSidebarBottom", AdItem.GetAdById( AdCategory.ArticleSidebarBottom ) );


        }



    }

}
