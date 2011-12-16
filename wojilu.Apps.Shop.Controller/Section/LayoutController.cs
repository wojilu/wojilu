/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;

using wojilu.Apps.Shop.Domain;
using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Service;
using wojilu.Apps.Shop.Enum;
using wojilu.Web.Controller.Common;
using wojilu.Web.Controller.Shop.Caching;

namespace wojilu.Web.Controller.Shop.Section {

    public partial class LayoutController : ControllerBase {

        public IShopItemService postService { get; set; }

        public LayoutController() {
            postService = new ShopItemService();
        }

        public override void Layout() {

            ShopApp app = ctx.app.obj as ShopApp;
            ShopSetting s = app.GetSettingsObj();

            List<ShopItem> posts = postService.GetRankPost( ctx.app.Id, s.RankPosts, ItemMethod.Normal );
            bindPosts( posts, "list" );


        }

        private void bindPosts(List<ShopItem> posts, String blockName)
        {
            IBlock panel = getBlock(blockName + "Panel");
            if (posts.Count == 0) return;
            IBlock block = panel.GetBlock(blockName);
            foreach (ShopItem post in posts)
            {

                if (post.PageSection == null) continue;

                block.Set("sku.TitleFull", post.Title);
                block.Set("sku.Title", strUtil.SubString(post.Title, 18));

                block.Set("sku.Link", alink.ToAppData(post));
                block.Next();
            }
            panel.Next();
        }
    }

}
