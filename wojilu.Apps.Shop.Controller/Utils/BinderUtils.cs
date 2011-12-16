/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Domain;
using wojilu.Apps;
using wojilu.Web.Context;
using wojilu.Common.AppBase;
using wojilu.Common.AppBase.Interface;
using wojilu.Common;
using wojilu.DI;
using System.Collections.Generic;
using wojilu.Members.Sites.Domain;
using wojilu.Apps.Shop.Enum;

namespace wojilu.Web.Controller.Shop.Utils {

    public class BinderUtils {

        private static readonly ILog logger = LogManager.GetLogger( typeof( BinderUtils ) );

        //public static readonly String iconPic = string.Format( "<img src=\"{0}\"/> ", strUtil.Join( sys.Path.Img, "img.gif" ) );
        //public static readonly String iconVideo = string.Format( "<img src=\"{0}\"/> ", strUtil.Join( sys.Path.Img, "video.gif" ) );
        //public static readonly String iconAttachment = string.Format( "<img src=\"{0}\"/>", strUtil.Join( sys.Path.Img, "attachment.gif" ) );

        //public static readonly String iconPicked = string.Format( "<img src=\"{0}star.gif\" />", sys.Path.Img );
        //public static readonly String iconFocus = string.Format( "<img src=\"{0}sticky2.gif\" />", sys.Path.Img );

        //public static String getTypeIcon( ShopItem post ) {

        //    //if (post.TypeName == typeof( ContentVideo ).FullName) return iconVideo;
        //    if (post.TypeName == typeof( ShopItemImg ).FullName) return iconPic;
        //    if (post.HasImg()) return iconPic;

        //    return "";
        //}

        //public static String getPickedIcon( ShopItem post ) {

        //    if (post.PickStatus == PickStatus.Picked) return iconPicked;
        //    if (post.PickStatus == PickStatus.Focus) return iconFocus;
        //    return "";

        //}

        public static void bindListItem(IBlock block, ShopItem sku, MvcContext ctx)
        {
            block.Set("sku.SectionName", sku.PageSection.Title);
            block.Set("sku.SectionUrl", ctx.to(new SectionController().Show, sku.PageSection.Id));

            //String typeIcon = BinderUtils.getTypeIcon(sku);
            //block.Set("sku.ImgIcon", typeIcon);

            String att = sku.Attachments > 0 ? "<img src=\"" + strUtil.Join(sys.Path.Img, "attachment.gif") + "\"/>" : "";
            block.Set("sku.AttachmentIcon", att);

            block.Set("sku.TitleCss", sku.Style);
            block.Set("sku.TitleFull", sku.Title);
            block.Set("sku.Title", strUtil.SubString(sku.GetTitle(), 50));
            block.Set("sku.GiftWord", (sku.IsGift == 1 ? "礼品" : ""));
            block.Set("sku.ItemSKU", sku.ItemSKU);
            block.Set("sku.SalePrice", sku.SalePrice);
            block.Set("sku.RetaPrice", sku.RetaPrice);
            if (sku.HasImg())
            {
                block.Set("sku.ImgSrc", sku.GetImgThumb());
                block.Set("sku.ImgBigSrc", sku.GetImgMedium());
            }
            block.Set("sku.Tag", sku.Tag.HtmlString);
            block.Set("sku.Created", sku.Created);
            block.Set("sku.Hits", sku.Hits);
            block.Set("sku.Url", alink.ToAppData(sku));

            if (sku.Creator != null)
            {
                block.Set("sku.Submitter", string.Format("<a href=\"{0}\" target=\"_blank\">{1}</a>", Link.ToMember(sku.Creator), sku.Creator.Name));
            }
            else {
                block.Set("sku.Submitter", "");
            }
            block.Bind("sku", sku);

        }

        public static ISectionBinder GetBinder( ShopSectionTemplate template, MvcContext ctx, Template currentView ) {

            String binderName = string.Format( "wojilu.Web.Controller.Shop.Binder.{0}BinderController", template.TemplateName );

            ControllerBase controller = ControllerFactory.FindController( binderName, ctx ) as ControllerBase;
            if (controller == null) throw new Exception( "ISectionBinder not found:" + binderName );
            controller.utils.setCurrentView( currentView );
            return controller as ISectionBinder;
        }

        public static IPageSection GetPageSection( ShopSection articleSection, MvcContext ctx, String currentView ) {

            ControllerBase controller = ControllerFactory.FindController( articleSection.SectionType, ctx ) as ControllerBase;
            controller.view( currentView );
            return controller as IPageSection;
        }

        public static IPageSection GetPageSectionAdmin( ShopSection articleSection, MvcContext ctx, String currentView ) {
            String adminSectionControllerName = getAdminControllerName( articleSection );
            ControllerBase controller = ControllerFactory.FindController( adminSectionControllerName, ctx ) as ControllerBase;
            controller.view( currentView );
            return controller as IPageSection;
        }

        private static String getAdminControllerName( ShopSection articleSection ) {
            string[] typeItem = articleSection.SectionType.Split( '.' );
            String controllerName = typeItem[typeItem.Length - 1];
            String namespaceStr = strUtil.TrimEnd( articleSection.SectionType, "." + controllerName );
            String adminNamespace = getAdminNamespace( namespaceStr );
            return adminNamespace + "." + controllerName;
        }

        // wojilu.Web.Controller.Shop.Section =>
        // wojilu.Web.Controller.Shop.Admin.Section
        private static String getAdminNamespace( String namespaceStr ) {
            string[] item = namespaceStr.Split( '.' );
            String lastItem = item[item.Length - 1];
            String namespaceStart = strUtil.TrimEnd( namespaceStr, "." + lastItem );
            return namespaceStart + ".Admin." + lastItem;
        }

        // 得到视图view的模板文件
        public static String GetBinderTemplatePath( ShopSectionTemplate sectionTemplate ) {
            return "Shop/Binder/" + sectionTemplate.TemplateName;
        }

        // 得到缩略图示例
        public static String GetBinderTemplateThumbPath() {
            return strUtil.Join( sys.Path.Img, "app/shop/Binder/" );
        }

        // 得到缩略图示例
        public static String GetSectionTemplateThumbPath() {
            return strUtil.Join(sys.Path.Img, "app/shop/section/");
        }

        public static void bindPostSingle(IBlock block, ShopItem sku)
        {

            block.Set("sku.TitleCss", sku.Style);
            block.Set("sku.TitleFull", sku.Title);

            if (strUtil.HasText(sku.TitleHome))
                block.Set("sku.Title", sku.TitleHome);
            else
                block.Set("sku.Title", sku.Title);

            block.Set("sku.GiftWord", (sku.IsGift == 1 ? "礼品" : ""));
            block.Set("sku.ItemSKU", sku.ItemSKU);
            block.Set("sku.SalePrice", sku.SalePrice);
            block.Set("sku.RetaPrice", sku.RetaPrice);
            if (sku.HasImg())
            {
                block.Set("sku.ImgSrc", sku.GetImgThumb());
                block.Set("sku.ImgBigSrc", sku.GetImgMedium());
            }
            block.Set("sku.Tag", sku.Tag.HtmlString);
            if (sku.Brand != null)
            {
                block.Set("sku.BrandTitle", sku.Brand.Title);
                block.Set("sku.BrandPic", "<img src=\"" + sku.Brand.BrandPic + "\" title=\"" + sku.Brand.Title + "\" border=\"0\">");
            }

            block.Set("sku.Url", alink.ToAppData(sku));

            block.Set("sku.Description", sku.Content);

            block.Set("sku.Created", sku.Created);
            block.Set("sku.CreatedDay", sku.Created.ToShortDateString());
            block.Set("sku.CreatedTime", sku.Created.ToShortTimeString());

            block.Set("sku.CreatorName", sku.Creator == null ? "" : sku.Creator.Name);
            block.Set("sku.CreatorLink", Link.ToMember(sku.Creator));

            block.Bind("sku", sku);

        }

        public static void bindMashupData( IBlock block, IBinderValue item, int itemIndex ) {

            block.Set("sku.ItemIndex", itemIndex);
            block.Set("sku.Title", item.Title);
            block.Set("sku.Url", item.Link);

            block.Set("sku.Created", item.Created.Day);
            block.Set("sku.CreatedDay", item.Created.ToShortDateString());
            block.Set("sku.CreatedTime", item.Created.ToShortTimeString());

            block.Set("sku.CreatorName", item.CreatorName);
            block.Set("sku.CreatorLink", item.CreatorLink);
            block.Set("sku.CreatorPic", item.CreatorPic);

            block.Set("sku.Content", item.Content);
            block.Set("sku.Summary", strUtil.CutString(item.Content, 200));
            block.Set("sku.PicUrl", item.PicUrl);
            block.Set("sku.Replies", item.Replies);

            block.Bind("sku", item);

        }

        public static void BindSkuCart(IBlock block, ShopItem sku)
        {
            block.Set("sku.buySKU", sku.ItemSKU);
            block.Set("sku.buyQty", sku.MinOrderQty > 0 ? sku.MinOrderQty : 1);
            block.Set("sku.buyAttr", "");//TODO:产品属性
            block.Set("sku.buyId", sku.Id);
        }

    }

}
