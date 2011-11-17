using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Controller.Common.Caching;
using wojilu.Caching;
using wojilu.Web.Context;
using wojilu.Members.Interface;
using wojilu.Web.Mvc.Utils;
using wojilu.Members.Sites.Domain;
using wojilu.Apps.Forum.Domain;

namespace wojilu.Web.Controller.Forum.Caching {

    public class ForumRecentCache : IActionCache {

        public string GetCacheKey( MvcContext ctx, string actionName ) {

            IMember owner = ctx.owner.obj;
            int appId = ctx.app.Id;


            return owner.GetType().FullName + "_" + owner.Url.Replace( "/", "" ) + "_" + typeof( wojilu.Web.Controller.Forum.RecentController ).FullName + "_" + actionName + "_app" + appId;
        }

        private string getActionName( MvcContext ctx ) {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> GetRelatedActions() {
            throw new NotImplementedException();
        }

        public void UpdateCache( MvcContext ctx ) {
            if ((ctx.owner.obj is Site) == false) return;

            IMember owner = ctx.owner.obj;
            int appId = ctx.app.Id;

            String viewName = getActionName( ctx );


            String key = GetCacheKey( ctx, viewName );

            String content = getIndexCache( owner, appId, viewName );

            CacheManager.GetApplicationCache().Put( key, content );
        }

        private static string getIndexCache( IMember owner, int appId, String viewName ) {
            MvcContext ctx = MockContext.GetOne( owner, typeof( ForumApp ), appId );
            String content = ControllerRunner.Run( ctx, typeof( wojilu.Web.Controller.Forum.RecentController ).FullName, viewName );

            return content;
        }


    }


}
