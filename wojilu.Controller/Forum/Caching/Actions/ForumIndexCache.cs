using System;
using System.Collections.Generic;

using wojilu.Caching;
using wojilu.Common.AppBase.Interface;
using wojilu.Members.Interface;
using wojilu.Members.Sites.Domain;
using wojilu.Web.Context;
using wojilu.Web.Controller.Common.Caching;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Utils;
using wojilu.Apps.Forum.Domain;
using wojilu.Web.Controller.Admin;
using wojilu.Aop;
using wojilu.Apps.Forum.Service;

namespace wojilu.Web.Controller.Forum.Caching {

    public class ForumIndexCache : ActionCache {

        public override string GetCacheKey( MvcContext ctx, string actionName ) {
            IMember owner = ctx.owner.obj;
            if ((owner is Site) == false) return null;

            int appId = ctx.app.Id;
            return getCacheKey( owner, appId );
        }

        private String getCacheKey( IMember owner, int appId ) {
            return "__action_" + owner.GetType().FullName + "_" + owner.Url.Replace( "/", "" ) + "_" + typeof( wojilu.Web.Controller.Forum.ForumController ).FullName + "_" + appId;
        }

        public override void ObserveActions() {

            observe( new RegisterController().SaveReg );

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

            //-------------------------------------------------------------
            observe( new Admin.SettingController().Save );

            Admin.ForumLinkController lnk = new wojilu.Web.Controller.Forum.Admin.ForumLinkController();
            observe( lnk.Create );
            observe( lnk.Update );
            observe( lnk.Delete );
            observe( lnk.SaveSort );

            Admin.PickedImgController img = new wojilu.Web.Controller.Forum.Admin.PickedImgController();
            observe( img.Create );
            observe( img.Update );
            observe( img.Delete );

            Admin.ModeratorController md = new wojilu.Web.Controller.Forum.Admin.ModeratorController();
            observe( md.Create );
            observe( md.Delete );

            AdController ad = new AdController();
            observe( ad.Create );
            observe( ad.Update );
            observe( ad.Delete );
            observe( ad.Start );
            observe( ad.Stop );

            Admin.ForumPickController pick = new Admin.ForumPickController();
            observe( pick.SavePinAd );
            observe( pick.SavePinTopic );
            observe( pick.UpdatePin );
            observe( pick.UpdateTopic );
            observe( pick.DeletePin );
            observe( pick.DeleteTopic );
            observe( pick.Restore );
        }

        public override void AfterAction( MvcContext ctx ) {

            if ((ctx.owner.obj is Site) == false) return;


            IMember owner = ctx.owner.obj;
            String key = GetCacheKey( ctx, null );

            if (ctx.app == null || ctx.app.Id <= 0) { // 比如新用户注册成功，那种情况下ctx.app是null

                removeAllForumCache( owner );

            }
            else {

                CacheManager.GetApplicationCache().Remove( key );
            }
        }

        // 如果不是针对某个特定app的更新，则需要移除所有缓存
        private void removeAllForumCache( IMember owner ) {

            List<ForumApp> siteForums = ForumApp.find( "OwnerId=" + owner.Id+ " and OwnerType=:otype" )
                .set( "otype", owner.GetType().FullName )
                .list();
            foreach (ForumApp app in siteForums) {

                String cacheKey = getCacheKey( owner, app.Id );

                CacheManager.GetApplicationCache().Remove( cacheKey );
            }
        }

    }



}
