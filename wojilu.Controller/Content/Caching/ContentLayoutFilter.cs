using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Controller.Common.Caching;
using wojilu.Members.Interface;
using wojilu.Caching;
using wojilu.Apps.Content.Domain;
using wojilu.Web.Context;
using wojilu.Web.Mvc.Utils;

namespace wojilu.Web.Controller.Content.Caching {

    public class ContentLayoutCache : IActionCache {

        public string GetCacheKey( MvcContext ctx, string actionName ) {

            IMember owner = ctx.owner.obj;
            int appId = ctx.app.Id;

            return owner.GetType().FullName + "_" + owner.Url + "_" + typeof( wojilu.Web.Controller.Content.Section.LayoutController ).FullName + ".Layout" + "_app" + appId;
        }

        public Dictionary<string, string> GetRelatedActions() {
            throw new NotImplementedException();
        }

        public void UpdateCache( MvcContext ctx ) {
            IMember owner = ctx.owner.obj;
            int appId = ctx.app.Id;

            String key = GetCacheKey( ctx, null );

            String content = getLayoutCache( appId, owner );

            CacheManager.GetApplicationCache().Put( key, content );
        }

        private static String getLayoutCache( int appId, IMember owner ) {

            MvcContext ctx = MockContext.GetOne( owner, typeof( ContentApp ), appId );
            String content = ControllerRunner.Run( ctx, new wojilu.Web.Controller.Content.Section.LayoutController().Layout );

            return content;
        }

    }


}
