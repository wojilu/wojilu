using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Controller.Common.Caching;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Users.Caching {

    public class UserMainPageCache : PageCache {

        public override void ObserveActionCaches() {
            observe( typeof( SiteLayoutCache ) );
            observe( typeof( UserMainIndexCache ) );
        }

        public override void UpdateCache( wojilu.Web.Context.MvcContext ctx ) {

            String url = new Link( ctx ).T2( Site.Instance, new Users.MainController().Index );

            base.updateAllUrl( url, ctx, Site.Instance );

        }

    }

}
