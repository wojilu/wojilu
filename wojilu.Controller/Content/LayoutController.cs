/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Content.Domain;

namespace wojilu.Web.Controller.Content {

    public class LayoutController : ControllerBase {


        public override void Layout() {

            ContentApp app = ctx.app.obj as ContentApp;
            ContentSkin s = ContentSkin.findById( app.SkinId );
            set( "skinPath", strUtil.Join( sys.Path.Skin, s.StylePath ) );


            set( "adminUrl", to( new Admin.ContentController().Index ) );
            set( "appUrl", to( new ContentController().Index ) );

            set( "adminCheckUrl", t2( new SecurityController().CanAppAdmin, app.Id ) + "?appType=" + typeof( ContentApp ).FullName );



        }
    }

}
