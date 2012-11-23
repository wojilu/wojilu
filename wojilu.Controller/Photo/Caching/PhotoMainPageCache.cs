using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Controller.Common.Caching;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Photo.Caching {

    public class PhotoMainPageCache : CorePageCache {

        public override void ObserveActionCaches() {
            observe( typeof( SiteLayoutCache ) );
            observe( typeof( PhotoMainActionCache ) );
        }

        public override void UpdateCache( wojilu.Web.Context.MvcContext ctx ) {
            
            String url = Link.To( Site.Instance, new Photo.MainController().Index );
            base.updateAllUrl( url, ctx, Site.Instance );
        }

    }

}
