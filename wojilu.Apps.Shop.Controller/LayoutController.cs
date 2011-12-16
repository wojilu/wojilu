/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Shop.Domain;

namespace wojilu.Web.Controller.Shop {

    public class LayoutController : ControllerBase {

        public override void Layout() {

            ShopApp app = ctx.app.obj as ShopApp;
            ShopSkin s = ShopSkin.findById( app.SkinId );
            set( "skinPath", strUtil.Join( sys.Path.Skin, s.StylePath ) );


            set( "adminUrl", to( new Admin.ShopController().Index ) );
            set( "appUrl", to( new ShopController().Index ) );



        }
    }

}
