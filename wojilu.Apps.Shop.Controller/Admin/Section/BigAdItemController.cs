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
    public class BigAdItemController : ControllerBase, IPageSection
    {
        public IShopItemService skuService { get; set; }
        public IShopCategoryService clsService { get; set; }

        public BigAdItemController()
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
            ShopItem sku = skuService.GetFirstPost(ctx.app.Id,sectionId);
            bindSectionShow(sectionId, sku);
        }

        private void bindSectionShow(int sectionId, ShopItem sku)
        {

            set("skuAddUrl", Link.To(new ItemController().Add, sectionId) + "?typeId=" + ItemMethod.Normal);
            set("skuListUrl", Link.To(new ListController().AdminList, sectionId) + "?typeId=" + ItemMethod.Normal);

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
            set("sku.Url", Link.To(new ItemController().Edit, sku.Id));
            set("sku.Created", sku.Created.ToShortTimeString());
        }
    }
}
