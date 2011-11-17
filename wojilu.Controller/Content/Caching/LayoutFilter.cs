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

    public class LayoutFilter : ICacheFilter {



        public Dictionary<string, string> Actions {
            get { throw new NotImplementedException(); }
        }

        public void Update( wojilu.Web.Context.MvcContext ctx ) {


            IMember owner = ctx.owner.obj;
            int appId = ctx.app.Id;

            String key = GetCacheKey( owner, appId );

            String content = getLayoutCache( appId, owner );

            CacheManager.GetApplicationCache().Put( key, content );

        }


        public static String GetCacheKey( IMember owner, int appId ) {
            return owner.GetType().FullName + "_" + owner.Url + "_" + typeof( wojilu.Web.Controller.Content.Section.LayoutController ).FullName + ".Layout" + "_app" + appId;
        }

        private static String getLayoutCache( int appId, IMember owner ) {

            MvcContext ctx = MockContext.GetOne( owner, typeof( ContentApp ), appId );
            String content = ControllerRunner.Run( ctx, new wojilu.Web.Controller.Content.Section.LayoutController().Layout );

            return content;
        }



    }

}
