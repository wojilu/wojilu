/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Shop.Domain;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Shop.Admin {

    public class LayoutController : ControllerBase {

        public override void Layout() {

            ShopApp app = ctx.app.obj as ShopApp;

            set("allgoodLink", to(new ItemController().List));
            set( "trashgoodLink", to( new ItemController().Trash ) );
            set( "settingLink", to( new SettingController().Index ) );
            set( "defaultLink", to( new ShopController().Home ) );
            set( "commentLink", to( new CommentController().AdminList ) );

            set("viewByCategoryLink", to(new SubCategoryController().Items));
            set("categoryLink", to(new CategoryController().List));
            set("subCategoryLink", to(new SubCategoryController().List));
            set("brandLink", to(new BrandController().List));
            set("supplierLink", to(new SupplierController().List));
            set("allorderLink", to(new OrderController().List));
            //set("unpayorderLink", "");
            //set("paidorderLink", "");
            //set("unshiporderLink", "");
            //set("shipedorderLink", "");
            //set("stockLink", "");
            set("paymenttypeLink", to(new PaymentController().List));
            set("paygatewayLink", to(new PaymentController().ListGateway));
            set("paycurrenctyLink", to(new PaymentController().ListCurrency));
            set("delivertypeLink", to(new DeliverController().List));
            set("deliverContactLink", to(new DeliverController().ListContact));
            set("delivercompanyLink", to(new DeliverController().ListProvider));

            if (app.SkinId > 0)
            {
                ShopSkin skinv = ShopSkin.findById(app.SkinId);
                set("skinPath", skinv.StylePath);
            }
            //if (app.GetSettingsObj().EnableSubmit == 1) {
            //    String slnk = string.Format( "<td class=\"otherTab\" style=\"width:15%\"><a href=\"{0}\" class=\"frmLink\" loadTo=\"adminPortalContainer\" nolayout=3><img src=\"{1}\" /> {2}</a></td>",
            //        to( new SubmitSettingController().List ),
            //        strUtil.Join( sys.Path.Img, "user.gif" ),
            //        "投递员管理" );
            //    set( "submitterLink", slnk );
            //}
            //else {
            //    set( "submitterLink", "" );
            //}

            String appStyle = app.Style == null ? "" : app.Style.Replace( "display:none;", "" );
            String skinStyle = app.SkinStyle == null ? "" : app.SkinStyle.Replace( "display:none;", "" );

            set( "app.Style", appStyle );
            set( "app.SkinStyle", skinStyle );

            StringBuilder sb = new StringBuilder();
            if (ctx.owner.obj is Site) {
                //sb.AppendLine( "#adminPortalContainer {width: 1000px;}" );
                //sb.AppendLine( "#portalAdminNav,#portalAdminNavWrap,.tabMain {width:1030px;}" );
                //sb.AppendLine( "#toggleSidebar { display:none;}" );
                sb.AppendLine( "#adminPortalContainer {width: 100%;}" );
                sb.AppendLine( "#portalAdminNav,#portalAdminNavWrap,.tabMain {width:100%;}" );
                sb.AppendLine( "#toggleSidebar { display:none;}" );
            }
            set( "portalWrapCss", sb );

        }
    }

}
