using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Controller.Common.Caching;
using wojilu.Caching;
using wojilu.Web.Context;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Blog.Caching {

    public class BlogMainPageCache : CorePageCache {

        private static readonly ILog logger = LogManager.GetLogger( typeof( BlogMainPageCache ) );

        public override void ObserveActionCaches() {
            observe( typeof( BlogMainCache ) );
            observe( typeof( BlogMainLayoutCache ) );
            observe( typeof( SiteLayoutCache ) );
        }

        public override void UpdateCache( MvcContext ctx ) {

            String url = Link.To( Site.Instance, new Blog.MainController().Index );

            logger.Info( "update blogMain page=" + url );

            base.updateAllUrl( url, ctx, Site.Instance );
        }

    }

}
