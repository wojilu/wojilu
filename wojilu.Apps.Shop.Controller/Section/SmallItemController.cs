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
    public class SmallItemController : ControllerBase, IPageSection
    {
        public IShopItemService skuService { get; set; }
        public IShopCategoryService clsService { get; set; }
        public IShopSectionService secService { get; set; }
        public IShopCustomTemplateService ctService { get; set; }

        public SmallItemController()
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

            List<ShopItem> posts = this.skuService.GetTopBySectionAndType(sectionId, ItemMethod.Normal, appId);

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

        private void bindSectionShow(int sectionId, List<ShopItem> skus)
        {

            //set("skuAddUrl", Link.To(new ItemController().Add, sectionId) + "?typeId=" + ItemMethod.Normal);
            //set("skuListUrl", Link.To(new ListController().AdminList, sectionId) + "?typeId=" + ItemMethod.Normal);


            IBlock block = getBlock("list");
            foreach (ShopItem sku in skus)
            {

                block.Set("sku.TitleCss", sku.Style);
                block.Set("sku.TitleFull", sku.Title);

                if (strUtil.HasText(sku.TitleHome))
                    block.Set("sku.Title", sku.TitleHome);
                else
                    block.Set("sku.Title", sku.Title);

                block.Set("sku.GiftWord", (sku.IsGift==1?"礼品":""));
                block.Set("sku.ItemSKU", sku.ItemSKU);
                block.Set("sku.SalePrice", sku.SalePrice);
                block.Set("sku.RetaPrice", sku.RetaPrice);
                block.Set("sku.ImgSrc", sku.GetImgThumb());
                block.Set("sku.Tag", sku.Tag.HtmlString);
                if (sku.Brand != null)
                {
                    block.Set("sku.BrandTitle", sku.Brand.Title);
                    block.Set("sku.BrandPic", "<img src=\"" + sku.Brand.BrandPic + "\" title=\"" + sku.Brand.Title + "\" border=\"0\">");
                }
                block.Set("sku.Url", alink.ToAppData(sku));
                block.Set("sku.Created", sku.Created.ToShortTimeString());
                block.Next();
            }
        }
    }
}
