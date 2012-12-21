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
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Forum.Caching {

    public class ForumRecentLayoutCache : ActionCache {

        public override string GetCacheKey( MvcContext ctx, string actionName ) {
            IMember owner = ctx.owner.obj;
            if ((owner is Site) == false) return null;
            return "forum_recent_" + ctx.app.Id;
        }
    }

    //---------------------------------------------------------------------------------

    public class ForumRecentTopicCache : ForumRecentCache {

        public override string getActionName() { return "Topic"; }

        public override void ObserveActions() {

            observe( new Users.TopicController().Create );
            observe( new Users.PollController().Create );

            observe( new Edits.TopicController().Update );

            Moderators.TopicSaveController mt = new wojilu.Web.Controller.Forum.Moderators.TopicSaveController();
            observe( mt.Delete );

            Moderators.PostSaveController mp = new wojilu.Web.Controller.Forum.Moderators.PostSaveController();
            observe( mp.DeleteTopic );
        }
    }

    public class ForumRecentPostCache : ForumRecentCache {

        public override string getActionName() { return "Post"; }

        public override void ObserveActions() {

            observe( new Users.TopicController().Create );
            observe( new Users.PostController().Create );
            observe( new Users.PollController().Create );

            observe( new Edits.TopicController().Update );
            observe( new Edits.PostController().Update );

            Moderators.TopicSaveController mt = new wojilu.Web.Controller.Forum.Moderators.TopicSaveController();
            observe( mt.Delete );

            Moderators.PostSaveController mp = new wojilu.Web.Controller.Forum.Moderators.PostSaveController();
            observe( mp.DeletePost );
            observe( mp.DeleteTopic );
        }
    }


    public abstract class ForumRecentCache : ActionCache {

        public abstract String getActionName();

        public override string GetCacheKey( MvcContext ctx, string actionName ) {

            if (ctx.route.page > 1) return null;

            return getCacheKey( ctx );
        }

        private String getCacheKey( MvcContext ctx ) {

            IMember owner = ctx.owner.obj;
            int appId = ctx.app.Id;
            return getCacheKey( owner, appId );
        }

        private String getCacheKey( IMember owner, int appId ) {
            return "__action_" + owner.GetType().FullName + "_" + owner.Url.Replace( "/", "" ) + "_" + typeof( wojilu.Web.Controller.Forum.RecentController ).FullName + "_" + getActionName() + "_app" + appId;
        }

        public override void AfterAction( MvcContext ctx ) {

            if ((ctx.owner.obj is Site) == false) return;

            IMember owner = ctx.owner.obj;
            int appId = ctx.app.Id;

            //String viewName = this.getActionName();
            //String key = getCacheKey( ctx );
            //String content = getIndexCache( owner, appId, viewName );
            //CacheManager.GetApplicationCache().Put( key, content );

            CacheManager.GetApplicationCache().Remove( getCacheKey( owner, appId ) );
        }

        //private static string getIndexCache( IMember owner, int appId, String viewName ) {
        //    MvcContext ctx = MockContext.GetOne( owner, typeof( ForumApp ), appId );
        //    String content = ControllerRunner.Run( ctx, typeof( wojilu.Web.Controller.Forum.RecentController ).FullName, viewName );

        //    return content;
        //}


    }


}
