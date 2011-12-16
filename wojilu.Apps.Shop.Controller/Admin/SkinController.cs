using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Shop.Domain;
using wojilu.DI;

namespace wojilu.Web.Controller.Shop.Admin {

    [App( typeof( ShopApp ) )]
    public class SkinController : ControllerBase {

        public void Index() {

            ShopApp app = ctx.app.obj as ShopApp;

            DataPage<ShopSkin> list = ShopSkin.findPage( "order by Id asc" );
            IBlock block = getBlock( "list" );
            foreach (ShopSkin s in list.Results) {

                block.Set( "s.Id", s.Id );
                block.Set( "s.Name", s.Name );
                block.Set( "s.ThumbUrl", strUtil.Join( sys.Path.Skin, s.ThumbUrl ) );
                block.Set( "s.ApplyLink", to( Apply, s.Id ) );

                String currentClass = "";
                String applyLink = string.Format( "<a href=\"{0}\">应用</a>", to( Apply, s.Id ) );
                if (app.SkinId == s.Id) {
                    currentClass = "currentSkin";
                    applyLink = "<span>当前皮肤</span>";
                }

                block.Set( "s.CurrentClass", currentClass );
                block.Set( "s.ApplyLink", applyLink );

                block.Next();

            }

            set( "page", list.PageBar );

        }

        public void Apply( int id ) {

            ShopApp app = ctx.app.obj as ShopApp;
            app.SkinId = id;

            app.update( "SkinId" );

            echoAjaxOk();
            

        }

    }

}
