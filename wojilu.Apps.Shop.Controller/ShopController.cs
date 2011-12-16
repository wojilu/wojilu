/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Caching;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps.Shop.Domain;
using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Service;
using wojilu.Web.Controller.Common;
using wojilu.Web.Controller.Shop.Caching;

namespace wojilu.Web.Controller.Shop
{

    [App( typeof( ShopApp ) )]
    public partial class ShopController : ControllerBase {

        private static readonly ILog logger = LogManager.GetLogger(typeof(ShopController));

        public IShopSectionService SectionService { get; set; }
        public IShopCategoryService classService { get; set; }
        public IShopSectionTemplateService TplService { get; set; }
        public IShopCustomTemplateService ctService { get; set; }

        public ShopController()
        {
            classService = new ShopCategoryService();
            SectionService = new ShopSectionService();
            TplService = new ShopSectionTemplateService();
            ctService = new ShopCustomTemplateService();
        }

        [CachePage( typeof( ContentIndexPageCache ) )]
        [CacheAction( typeof( ShopIndexCache ) )]
        public void Index() {

            ShopApp app = ctx.app.obj as ShopApp;
            ShopSetting setting = app.GetSettingsObj();
            if (setting.CacheSeconds > 0) {
                String content = loadFromCache();
                if (strUtil.IsNullOrEmpty( content )) {
                    content = loadHtml( IndexPage );
                    SysCache.Put( getKey(), content, setting.CacheSeconds );
                }
                actionContent( content );
            }
            else {
                run( IndexPage );
            }
        }

        private string getKey() {
            return typeof( ShopApp ).FullName + "_" + ctx.app.Id;
        }

        private string loadFromCache() {
            Object objCache = SysCache.Get( getKey() );
            if (objCache == null) return null;
            return objCache.ToString();
        }

        [NonVisit]
        public void IndexPage() {

            WebUtils.pageTitle( this, ctx.app.Name );

            ShopApp app = ctx.app.obj as ShopApp;

            set( "app.Style", app.Style );
            set( "app.SkinStyle", app.SkinStyle );

            List<ShopSection> sections = SectionService.GetByApp( ctx.app.Id );
            bindRows( app, sections );

        }

        [NonVisit]
        public void sidebar()
        {

            List<ShopCategory> cats = classService.GetRootList();
            bindCats(cats);

        }

        private void bindCats(List<ShopCategory> cats)
        {
            IBlock block = getBlock("cat");
            foreach (ShopCategory cat in cats)
            {

                block.Set("cat.Name", cat.Name);
                block.Set("cat.Link", to(new CategoryController().Show, cat.Id));

                bindSubCats(block, cat);

                block.Next();
            }
        }

        private void bindSubCats(IBlock block, ShopCategory cat)
        {
            IBlock subBlock = block.GetBlock("subcat");
            List<ShopCategory> subcats = classService.GetByParentId(cat.Id);
            foreach (ShopCategory subcat in subcats)
            {

                subBlock.Set("subcat.Name", subcat.Name);
                subBlock.Set("subcat.Link", to(new CategoryController().Show, subcat.Id));
                subBlock.Next();

            }
        }

    }
}

