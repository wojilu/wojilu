using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Common.AppBase.Interface;
using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Service;
using wojilu.Apps.Shop.Enum;
using wojilu.Apps.Shop.Domain;
using wojilu.Web.Controller.Shop.Utils;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Shop.Section
{
    [App(typeof(ShopApp))]
    public class BigAdItemController : ControllerBase, IPageSection
    {
        public IShopItemService skuService { get; set; }
        public IShopCategoryService clsService { get; set; }
        public IShopSectionService secService { get; set; }
        public IShopCustomTemplateService ctService { get; set; }

        public BigAdItemController()
        {
            skuService = new ShopItemService();
            clsService = new ShopCategoryService();
            secService = new ShopSectionService();
            ctService = new ShopCustomTemplateService();
        }

        public List<IPageSettingLink> GetSettingLink(int sectionId)
        {
            return new List<IPageSettingLink>();
        }

        public void SectionShow(int sectionId)
        {

            ShopSection s = secService.GetById(sectionId, ctx.app.Id);
            if (s == null)
            {
                throw new Exception(lang("exDataNotFound") + "=>page section:" + sectionId);
            }

            TemplateUtil.loadTemplate(this, s, ctService);

            int appId = ctx.app.Id;

            ShopItem posts = skuService.GetFirstPost(ctx.app.Id, sectionId);

            bindSectionShow(sectionId, posts);
        }

        public void List(int sectionId)
        {
            run(new ListController().List, sectionId);
        }

        public void Show(int id)
        {
            run(new ListController().Show, id);
        }

        public void AdminSectionShow(int sectionId)
        {
        }

        private void bindSectionShow(int sectionId, ShopItem sku)
        {

            //set("skuAddUrl", Link.To(new ItemController().Add, sectionId) + "?typeId=" + ItemMethod.Normal);
            //set("skuListUrl", Link.To(new ListController().AdminList, sectionId) + "?typeId=" + ItemMethod.Normal);

            set("sku.TitleCss", sku.Style);
            set("sku.TitleFull", sku.Title);
            if (strUtil.HasText(sku.TitleHome))
                set("sku.Title", sku.TitleHome);
            else
                set("sku.Title", sku.Title);

            set("sku.ItemSKU", sku.ItemSKU);
            set("sku.AdImgSrc", sku.GetAdImgUrl());
            set("sku.SalePrice", sku.SalePrice);
            set("sku.RetaPrice", sku.RetaPrice);
            set("sku.Tag", sku.Tag.HtmlString);
            set("sku.Url", alink.ToAppData(sku));
            set("sku.Created", sku.Created.ToShortTimeString());
        }
    }
}
