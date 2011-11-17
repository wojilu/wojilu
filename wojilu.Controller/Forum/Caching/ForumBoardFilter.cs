using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Controller.Common.Caching;
using wojilu.Web.Context;
using wojilu.Caching;
using wojilu.Web.Mvc.Utils;
using wojilu.Apps.Forum.Domain;
using wojilu.Members.Interface;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Forum.Caching {

    public class ForumBoardCache : IActionCache {

        public string GetCacheKey( MvcContext ctx, string actionName ) {

            IMember owner = ctx.owner.obj;
            int appId = ctx.app.Id;
            int boardId = (int)ctx.GetItem( "boardId" ); // TODO
            int pageId = 1;// TODO 多个分页依次更新

            return owner.GetType().FullName + "_" + owner.Url.Replace( "/", "" ) + "_" + typeof( wojilu.Web.Controller.Forum.BoardController ).FullName + "_app" + appId + "_board" + boardId + "_p" + pageId;
        }

        public Dictionary<string, string> GetRelatedActions() {
            throw new NotImplementedException();
        }

        public void UpdateCache( MvcContext ctx ) {

            if ((ctx.owner.obj is Site) == false) return;

            IMember owner = ctx.owner.obj;
            int appId = ctx.app.Id;
            int boardId = (int)ctx.GetItem( "boardId" ); // TODO
            int pageId = 1;// TODO 多个分页依次更新

            String key = GetCacheKey( ctx, null );

            String content = getIndexCache( owner, appId, boardId, pageId );

            CacheManager.GetApplicationCache().Put( key, content );
        }


        private static string getIndexCache( IMember owner, int appId, int boardId, int pageId ) {
            MvcContext ctx = MockContext.GetOne( owner, typeof( ForumApp ), appId );
            ctx.route.setPage( pageId );
            String content = ControllerRunner.Run( ctx, new wojilu.Web.Controller.Forum.BoardController().Show, boardId );

            return content;
        }

    }


}
