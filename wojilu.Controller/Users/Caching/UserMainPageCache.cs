using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Controller.Common.Caching;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Users.Caching {

    public class UserMainPageCache : CorePageCache {

        public override void ObserveActionCaches() {
            observe( typeof( SiteLayoutCache ) );
            observe( typeof( UserMainIndexCache ) );
        }

        public override void UpdateCache( wojilu.Web.Context.MvcContext ctx ) {

            String url = Link.To( Site.Instance, new Users.MainController().Index );

            base.updateAllUrl( url, ctx, Site.Instance );

        }

    }

}
