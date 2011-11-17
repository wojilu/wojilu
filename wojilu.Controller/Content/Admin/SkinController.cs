using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Content.Domain;
using wojilu.DI;

namespace wojilu.Web.Controller.Content.Admin {

    [App( typeof( ContentApp ) )]
    public class SkinController : ControllerBase {

        public void Index() {

            ContentApp app = ctx.app.obj as ContentApp;

            DataPage<ContentSkin> list = ContentSkin.findPage( "order by Id asc" );
            IBlock block = getBlock( "list" );
            foreach (ContentSkin s in list.Results) {

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

            ContentApp app = ctx.app.obj as ContentApp;
            app.SkinId = id;

            app.update( "SkinId" );

            echoAjaxOk();
            

        }

    }

}
