/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Caching;

using wojilu.Web.Mvc;

using wojilu.Members.Interface;
using wojilu.Members.Sites.Domain;

using wojilu.Web.Context;
using wojilu.Web.Controller.Admin;

using wojilu.Apps.Content.Domain;

namespace wojilu.Web.Controller.Content.Caching {

    public class ContentLayoutCache : ActionCache {

        public override string GetCacheKey( MvcContext ctx, string actionName ) {

            if (actionName != "Layout") return null;
            if (ctx.owner.obj.GetType() != typeof( Site )) return null;

            return getCacheKey( ctx );
        }

        private String getCacheKey( MvcContext ctx ) {
            IMember owner = ctx.owner.obj;
            int appId = ctx.app.Id;

            return getCacheKey( owner, appId );
        }

        private static string getCacheKey( IMember owner, int appId ) {
            return owner.GetType().FullName + "_" + owner.Url + "_" + typeof( wojilu.Web.Controller.Content.Section.LayoutController ).FullName + ".Layout" + "_app" + appId;
        }

        public override void ObserveActions() {

            Admin.SettingController sc = new wojilu.Web.Controller.Content.Admin.SettingController();
            observe( sc.Save );

            //---------------------------------------------------------

            Admin.Common.PostController post = new wojilu.Web.Controller.Content.Admin.Common.PostController();
            observe( post.Delete );
            observe( post.DeleteSys );
            observe( post.SaveAdmin );
            observe( post.Restore );

            //---------------------------------------------------------

            Admin.Section.TalkController talk = new wojilu.Web.Controller.Content.Admin.Section.TalkController();
            observe( talk.Create );
            observe( talk.Update );
            observe( talk.Delete );

            Admin.Section.TextController txt = new wojilu.Web.Controller.Content.Admin.Section.TextController();
            observe( txt.Create );
            observe( txt.Update );
            observe( txt.Delete );

            Admin.Section.VideoController video = new wojilu.Web.Controller.Content.Admin.Section.VideoController();
            observe( video.Create );
            observe( video.Update );
            observe( video.Delete );

            Admin.Section.ImgController img = new wojilu.Web.Controller.Content.Admin.Section.ImgController();
            observe( img.CreateListInfo );
            observe( img.CreateImgList );
            observe( img.SetLogo );
            observe( img.UpdateListInfo );
            observe( img.Delete );
            observe( img.DeleteImg );

            Admin.Common.PollController poll = new wojilu.Web.Controller.Content.Admin.Common.PollController();
            observe( poll.Create );
            observe( poll.Delete );

            AdController ad = new AdController();
            observe( ad.Create );
            observe( ad.Update );
            observe( ad.Delete );
            observe( ad.Start );
            observe( ad.Stop );

        }

        public override void AfterAction( MvcContext ctx ) {
            IMember owner = ctx.owner.obj;

            if ((owner is Site) == false) return;

            int appId = ctx.app.Id;

            String key = getCacheKey( ctx );

            //String content = getLayoutCache( appId, owner );
            //CacheManager.GetApplicationCache().Put( key, content );

            if (ctx.app == null || ctx.app.Id <= 0) {

                removeAllCache( owner );

            }
            else {


                CacheManager.GetApplicationCache().Remove( key );

            }
        }

        // 如果不是针对某个特定app的更新，则需要移除所有缓存
        private void removeAllCache( IMember owner ) {
            List<ContentApp> siteApps = ContentApp.find( "OwnerId=" + owner.Id + " and OwnerType=:otype" )
                .set( "otype", owner.GetType().FullName )
                .list();
            foreach (ContentApp app in siteApps) {

                String cacheKey = getCacheKey( owner, app.Id );

                CacheManager.GetApplicationCache().Remove( cacheKey );
            }
        }

        //private static String getLayoutCache( int appId, IMember owner ) {

        //    MvcContext ctx = MockContext.GetOne( owner, typeof( ContentApp ), appId );
        //    String content = ControllerRunner.Run( ctx, new wojilu.Web.Controller.Content.Section.LayoutController().Layout );

        //    return content;
        //}

    }


}
