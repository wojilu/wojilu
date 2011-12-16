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

namespace wojilu.Web.Controller.Shop.Admin.Section
{
    [App(typeof(ShopApp))]
    public class TextItemController : ControllerBase, IPageSection
    {
        public IShopItemService skuService { get; set; }
        public IShopCategoryService clsService { get; set; }

        public TextItemController()
        {
            skuService = new ShopItemService();
            clsService = new ShopCategoryService();
        }

        public List<IPageSettingLink> GetSettingLink(int sectionId)
        {
            List<IPageSettingLink> links = new List<IPageSettingLink>();

            PageSettingLink lnk = new PageSettingLink();
            lnk.Name = lang("editSetting");
            lnk.Url = to(new SectionSettingController().EditCount, sectionId);
            links.Add(lnk);

            PageSettingLink lnktmp = new PageSettingLink();
            lnktmp.Name = alang("editTemplate");
            lnktmp.Url = to(new TemplateCustomController().Edit, sectionId);
            links.Add(lnktmp);

            return links;
        }

        public void SectionShow(int sectionId)
        {
        }

        public void AdminSectionShow(int sectionId)
        {
            List<ShopItem> skus = skuService.GetTopBySectionAndType(sectionId, ItemMethod.Normal, ctx.app.Id);
            bindSectionShow(sectionId, skus);
        }

        private void bindSectionShow(int sectionId, List<ShopItem> skus)
        {

            set("skuAddUrl", Link.To(new ItemController().Add, sectionId) + "?typeId=" + ItemMethod.Normal);
            set("skuListUrl", Link.To(new ListController().AdminList, sectionId) + "?typeId=" + ItemMethod.Normal);


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
                block.Set("sku.Tag", sku.Tag.HtmlString);
                if (sku.Brand != null)
                {
                    block.Set("sku.BrandTitle", sku.Brand.Title);
                    block.Set("sku.BrandPic", "<img src=\"" + sku.Brand.BrandPic + "\" title=\"" + sku.Brand.Title + "\" border=\"0\">");
                }
                block.Set("sku.Url", Link.To(new ItemController().Edit, sku.Id));
                block.Set("sku.Created", sku.Created.ToShortTimeString());
                block.Next();
            }
        }
    }
}
