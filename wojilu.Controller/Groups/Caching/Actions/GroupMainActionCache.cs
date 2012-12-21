using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Caching;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Groups.Caching {

    public class GroupMainActionCache : ActionCache {

        public override string GetCacheKey( wojilu.Web.Context.MvcContext ctx, string actionName ) {
            return "__action_group_main_index";
        }

        public override void ObserveActions() {

            observe( new wojilu.Web.Controller.Users.Admin.MyGroupController().StepTwo );

            observe( new wojilu.Web.Controller.Forum.Users.TopicController().Create );
            observe( new wojilu.Web.Controller.Forum.Users.PostController().Create );
            observe( new wojilu.Web.Controller.Forum.Users.PollController().Create );

            observe( new wojilu.Web.Controller.Forum.Edits.TopicController().Update );
            observe( new wojilu.Web.Controller.Forum.Edits.PostController().Update );

            wojilu.Web.Controller.Forum.Moderators.TopicSaveController mt = new wojilu.Web.Controller.Forum.Moderators.TopicSaveController();
            observe( mt.Delete );

            wojilu.Web.Controller.Forum.Moderators.PostSaveController mp = new wojilu.Web.Controller.Forum.Moderators.PostSaveController();
            observe( mp.DeletePost );
            observe( mp.DeleteTopic );


            wojilu.Web.Controller.Admin.Groups.GroupController gc = new wojilu.Web.Controller.Admin.Groups.GroupController();
            observe( gc.SaveHide );
            observe( gc.Delete );

            wojilu.Web.Controller.Groups.Admin.MainController gm = new wojilu.Web.Controller.Groups.Admin.MainController();
            observe( gm.SaveInfo );
        }

        public override void AfterAction( wojilu.Web.Context.MvcContext ctx ) {
            CacheManager.GetApplicationCache().Remove( GetCacheKey( null, null ) );
        }

    }

}
