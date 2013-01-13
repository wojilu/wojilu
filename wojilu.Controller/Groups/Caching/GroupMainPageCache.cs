using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Controller.Common.Caching;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Groups.Caching {

    public class GroupMainPageCache : CorePageCache {

        public override void ObserveActionCaches() {
            observe( typeof( SiteLayoutCache ) );
            observe( typeof( GroupMainActionCache ) );
            observe( typeof( GroupMainLayoutCache ) );
        }

        public override void UpdateCache( wojilu.Web.Context.MvcContext ctx ) {

            String url = Link.To( Site.Instance, new Groups.MainController().Index );
            base.updateAllUrl( url, ctx, Site.Instance );
        }

    }

}
