/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Caching;
using wojilu.Web.Context;
using wojilu.Web.Mvc;

using wojilu.Members.Sites.Domain;
using wojilu.Members.Interface;

namespace wojilu.Web.Controller.Content.Caching {


    public class ContentIndexCache : ActionCache {

        public override string GetCacheKey( MvcContext ctx, string actionName ) {

            IMember owner = ctx.owner.obj;

            if ((owner is Site) == false) return null;

            int appId = ctx.app.Id;

            return getCacheKey( owner, appId );

        }

        private String getCacheKey( IMember owner, int appId ) {
            return owner.GetType().FullName + "_" + owner.Url + "_" + typeof( wojilu.Web.Controller.Content.ContentController ).FullName + "_" + appId;
        }

        public override void ObserveActions() {

            Admin.ContentController ac = new wojilu.Web.Controller.Content.Admin.ContentController();
            observe( ac.SaveLayout );
            observe( ac.SaveResize );
            observe( ac.SaveStyle );

            Admin.ContentSectionController cs = new wojilu.Web.Controller.Content.Admin.ContentSectionController();
            observe( cs.Create );
            observe( cs.CreateAuto );
            observe( cs.CreateFeed );
            observe( cs.SaveRowUI );
            observe( cs.SaveUI );
            observe( cs.SaveSectionUI );
            observe( cs.Delete );
            observe( cs.SaveSectionTitleUI );
            observe( cs.SaveSectionContentUI );
            observe( cs.SaveCombine );
            observe( cs.RemoveSection );
            observe( cs.SaveEffect );

            Admin.SectionSettingController ss = new wojilu.Web.Controller.Content.Admin.SectionSettingController();
            observe( ss.Update );
            observe( ss.SaveCount );
            observe( ss.UpdateBinder );

            Admin.SettingController sc = new wojilu.Web.Controller.Content.Admin.SettingController();
            observe( sc.Save );

            Admin.SkinController sk = new wojilu.Web.Controller.Content.Admin.SkinController();
            observe( sk.Apply );

            Admin.TemplateController tpl = new wojilu.Web.Controller.Content.Admin.TemplateController();
            observe( tpl.UpdateTemplate );

            Admin.TemplateCustomController tpc = new wojilu.Web.Controller.Content.Admin.TemplateCustomController();
            observe( tpc.Save );
            observe( tpc.Reset );

            Admin.RowController row = new wojilu.Web.Controller.Content.Admin.RowController();
            observe( row.AddRow );
            observe( row.Move );
            observe( row.DeleteRow );


            //---------------------------------------------------------

            Admin.Common.PostController post = new wojilu.Web.Controller.Content.Admin.Common.PostController();
            observe( post.Create );
            observe( post.Delete );
            observe( post.DeleteSys );
            observe( post.SaveAdmin );
            observe( post.Restore );
            observe( post.Update );
            observe( post.UpdateTitleStyle );

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
        }

        public override void AfterAction( MvcContext ctx ) {


            IMember owner = ctx.owner.obj;
            if ((owner is Site) == false) return;

            if (ctx.app == null) {
            }
            else {
                CacheManager.GetApplicationCache().Remove( getCacheKey( owner, ctx.app.Id ) );
            }

        }

    }

}
