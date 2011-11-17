using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Caching;
using wojilu.Members.Interface;
using wojilu.Web.Context;
using wojilu.Web.Controller.Common.Caching;
using wojilu.Web.Mvc.Utils;
using wojilu.Apps.Forum.Domain;

namespace wojilu.Web.Controller.Forum.Caching {

    public class BoardViewCacher {


        public static void Update( IMember owner, int appId, int boardId, int pageId ) {

            // TODO 多个分页依次更新

            String key = GetCacheKey( owner, appId, boardId, pageId );

            String content = getIndexCache( owner, appId, boardId, pageId );

            CacheManager.GetApplicationCache().Put( key, content );

        }

        public static String GetCacheKey( IMember owner, int appId, int boardId, int pageId ) {
            return owner.GetType().FullName + "_" + owner.Url.Replace( "/", "" ) + "_" + typeof( wojilu.Web.Controller.Forum.BoardController ).FullName + "_app" + appId + "_board" + boardId + "_p" + pageId;
        }

        private static string getIndexCache( IMember owner, int appId, int boardId, int pageId ) {
            MvcContext ctx = MockContext.GetOne( owner, typeof( ForumApp ), appId );
            ctx.route.setPage( pageId );
            String content = ControllerRunner.Run( ctx, new wojilu.Web.Controller.Forum.BoardController().Show, boardId );

            return content;
        }

    }

}
