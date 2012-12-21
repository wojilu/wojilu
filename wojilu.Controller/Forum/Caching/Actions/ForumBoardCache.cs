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
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Forum.Caching {

    public class ForumBoardCache : ActionCache {

        private static readonly ILog logger = LogManager.GetLogger( typeof( ForumBoardCache ) );

        private static readonly int cacheCount = 5;

        public override string GetCacheKey( MvcContext ctx, string actionName ) {

            IMember owner = ctx.owner.obj;
            if ((owner is Site) == false) return null;


            if (ctx.route.page > cacheCount) return null;

            return getCacheKey( ctx, getBoardId( ctx ) ) + ctx.route.page;
        }

        private String getCacheKey( MvcContext ctx, int boardId ) {

            IMember owner = ctx.owner.obj;
            int appId = ctx.app.Id;

            return owner.GetType().FullName + "_" + owner.Url.Replace( "/", "" ) + "_" + typeof( wojilu.Web.Controller.Forum.BoardController ).FullName + "_app" + appId + "_board" + boardId + "_p";
        }

        public override void ObserveActions() {

            observe( new Users.TopicController().Create );
            observe( new Users.PostController().Create );
            observe( new Users.PostController().SaveReward );
            observe( new Users.PostController().SaveBuy );

            observe( new Users.PollController().Create );

            observe( new Edits.TopicController().Update );
            observe( new Edits.PostController().Update );

            Moderators.TopicSaveController mt = new wojilu.Web.Controller.Forum.Moderators.TopicSaveController();
            observe( mt.Sticky );
            observe( mt.StickyUndo );
            observe( mt.GlobalSticky );
            observe( mt.GlobalStickyUndo );
            observe( mt.Pick );
            observe( mt.PickedUndo );
            observe( mt.Highlight );
            observe( mt.HighlightUndo );
            observe( mt.Lock );
            observe( mt.LockUndo );
            observe( mt.Delete );
            observe( mt.Move );
            observe( mt.SaveStickySort );
            observe( mt.SaveGlobalStickySort );
            observe( mt.Category );

            Moderators.PostSaveController mp = new wojilu.Web.Controller.Forum.Moderators.PostSaveController();
            observe( mp.SaveCredit );
            observe( mp.Ban );
            observe( mp.UnBan );
            observe( mp.Lock );
            observe( mp.UnLock );
            observe( mp.DeletePost );
            observe( mp.DeleteTopic );

            Admin.ForumController f = new wojilu.Web.Controller.Forum.Admin.ForumController();
            observe( f.SaveNotice );
            observe( f.SaveBoard );
            observe( f.SaveCategory );
            observe( f.UpdateBoard );
            observe( f.UpdateCategory );
            observe( f.DeleteBoard );
            observe( f.DeleteCategory );
            observe( f.AdminTopicTrashList );
            observe( f.AdminPostTrashList );
            observe( f.SaveCombine );

            Admin.CategoryController c = new wojilu.Web.Controller.Forum.Admin.CategoryController();
            observe( c.Create );
            observe( c.Update );
            observe( c.Delete );
            observe( c.SaveSort );

            Admin.ModeratorController m = new wojilu.Web.Controller.Forum.Admin.ModeratorController();
            observe( m.Create );
            observe( m.Delete );

            observe( new Admin.SettingController().Save );

            Admin.ModeratorController md = new wojilu.Web.Controller.Forum.Admin.ModeratorController();
            observe( md.Create );
            observe( md.Delete );


        }

        public override void AfterAction( MvcContext ctx ) {

            if ((ctx.owner.obj is Site) == false) return;

            // 1) board
            int boardId = getBoardId( ctx );
            updateOneBoard( ctx, boardId );

            // 2) 如果有多个board，比如移动主题的时候(move topic)
            int targetForumId = cvt.ToInt( ctx.GetItem( "targetForumId" ) );
            if (targetForumId >0) {
                updateOneBoard( ctx, targetForumId );
            }

        }

        private void updateOneBoard( MvcContext ctx, int boardId ) {

            if (boardId <= 0) {
                logger.Error( "updateOneBoard, boardId=" + boardId );
                return;
            }


            IMember owner = ctx.owner.obj;
            int appId = ctx.app.Id;

            logger.Info( "updateOneBoard, boardId=" + boardId );

            for (int pageId = 1; pageId < cacheCount+1; pageId++) {
                String key = getCacheKey( ctx, boardId ) + pageId;

                CacheManager.GetApplicationCache().Remove( key );

            }
        }

        private static int getBoardId( MvcContext ctx ) {

            int bid = cvt.ToInt( ctx.GetItem( "boardId" ) );
            if (bid > 0) return bid;

            int boardId = ctx.GetInt( "boardId" );
            if (boardId > 0) return boardId;

            if (ctx.route.id > 0) return ctx.route.id;

            return -1;
        }


        //private static string getIndexCache( IMember owner, int appId, int boardId, int pageId ) {
        //    MvcContext ctx = MockContext.GetOne( owner, typeof( ForumApp ), appId );
        //    ctx.route.setPage( pageId );
        //    String content = ControllerRunner.Run( ctx, new wojilu.Web.Controller.Forum.BoardController().Show, boardId );

        //    return content;
        //}

    }


}
