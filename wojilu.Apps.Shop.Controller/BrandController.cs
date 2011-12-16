/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.DI;
using wojilu.Web.Controller.Shop.Utils;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Shop.Domain;
using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Service;
using wojilu.Web.Controller.Common;
using wojilu.Web.Controller.Shop.Caching;
using wojilu.Web.Context;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Shop
{

    [App(typeof(ShopApp))]
    public class BrandController : ControllerBase
    {
        public IShopItemService itemService;
        public IShopCategoryService classService;
        public IShopBrandService brandService;
        public BrandController()
        {
            itemService = new ShopItemService();
            classService = new ShopCategoryService();
            brandService = new ShopBrandService();
        }

        public void Show(int id)
        {


            ShopBrand c = ShopBrand.findById(id);
            if (c == null)
            {
                echo(lang("exDataNotFound"));
                return;
            }

            WebUtils.pageTitle(this, c.Title);


            load("sidebar", new ShopController().sidebar);

            set("location", "");//Location.GetCategory(ctx, c.Id));

            //set("categories", Location.GetSubCategories(ctx, c));

            DataPage<ShopItem> list = itemService.GetByBrand(ctx.app.Id, c.Id);
            ctx.SetItem("list", list);
            ctx.SetItem("brand", c);
            load("list", List);
        }

        [NonVisit]
        public void List()
        {
            ShopBrand c = ctx.GetItem("brand") as ShopBrand;
            DataPage<ShopItem> list = ctx.GetItem("list") as DataPage<ShopItem>;
            bindList("list", "f", list.Results, bindLink);
            set("page", list.PageBar);
        }

        //private void bindSubCategories( FileCategory c ) {
        //    IBlock subblock = getBlock( "sub" );

        //    int rootId = c.Id;
        //    if (c.ParentId > 0) rootId = c.ParentId;

        //    IBlock block = subblock.GetBlock( "subcats" );
        //    List<FileCategory> subs = FileCategory.GetByParentId( rootId );
        //    foreach (FileCategory sub in subs) {
        //        block.Set( "cat.Name", sub.Name );
        //        block.Set( "cat.Link", to( Show, sub.Id ) );
        //        block.Next();
        //    }
        //    subblock.Next();

        //}

        private void bindLink(IBlock block, int id)
        {
            block.Set("f.ShowLink", to(new ItemController().Show, id));
        }

    }

}
