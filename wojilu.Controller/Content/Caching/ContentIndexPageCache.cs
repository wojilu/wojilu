using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Controller.Common.Caching;
using wojilu.Common.AppBase.Interface;
using wojilu.Members.Sites.Domain;
using wojilu.Apps.Content.Domain;
using wojilu.Web.Context;
using wojilu.Members.Interface;

namespace wojilu.Web.Controller.Content.Caching {

    public class ContentIndexPageCache : PageCache {

        public override bool IsCache( MvcContext ctx ) {

            IMember owner = ctx.owner.obj;

            if ((owner is Site)) return true;

            return false;
        }

        public override void ObserveActionCaches() {
            observe( typeof( ContentIndexCache ) );
            observe( typeof( SiteLayoutCache ) );
        }

        public override void UpdateCache( MvcContext ctx ) {

            IApp app = ctx.app.obj as IApp;

            if (app != null) {
                base.updateAllUrl( alink.ToApp( app, ctx ), ctx );
            }
            else {

                List<ContentApp> apps = ContentApp.find( "OwnerId=" + Site.Instance.Id + " and OwnerType=:otype" )
                    .set( "otype", typeof( Site ).FullName )
                    .list();

                foreach (ContentApp a in apps) {
                    base.updateAllUrl( alink.ToApp( a, ctx ), ctx );
                }

            }
        }
    }

}
