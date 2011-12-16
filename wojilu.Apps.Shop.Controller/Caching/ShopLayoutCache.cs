using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Controller.Common.Caching;
using wojilu.Members.Interface;
using wojilu.Caching;
using wojilu.Apps.Shop.Domain;
using wojilu.Web.Context;
using wojilu.Web.Mvc.Utils;
using wojilu.Members.Sites.Domain;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Shop.Caching {

    public class ShopLayoutCache : ActionCache {

        public override string GetCacheKey( MvcContext ctx, string actionName ) {

            if (actionName != "Layout") return null;
            if (ctx.owner.obj.GetType() != typeof( Site )) return null;

            return getCacheKey( ctx );
        }

        private String getCacheKey( MvcContext ctx ) {
            IMember owner = ctx.owner.obj;
            int appId = ctx.app.Id;

            return owner.GetType().FullName + "_" + owner.Url + "_" + typeof( wojilu.Web.Controller.Shop.Section.LayoutController ).FullName + ".Layout" + "_app" + appId;
        }

        public override Dictionary<Type, String> GetRelatedActions() {

            Dictionary<Type, String> dic = new Dictionary<Type, String>();

            dic.Add( typeof( Admin.SettingController ), "Save" );

            Dictionary<Type, String> postDic = ShopIndexCache.getPostAdminActions();
            foreach (KeyValuePair<Type, String> kv in postDic) dic.Add( kv.Key, kv.Value );


            return dic;
        }

        public override void UpdateCache( MvcContext ctx ) {
            IMember owner = ctx.owner.obj;
            int appId = ctx.app.Id;

            String key = getCacheKey( ctx );

            String content = getLayoutCache( appId, owner );

            CacheManager.GetApplicationCache().Put( key, content );
        }

        private static String getLayoutCache( int appId, IMember owner ) {

            MvcContext ctx = MockContext.GetOne( owner, typeof( ShopApp ), appId );
            String content = ControllerRunner.Run( ctx, new wojilu.Web.Controller.Shop.Section.LayoutController().Layout );

            return content;
        }

    }


}
