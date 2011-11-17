using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Controller.Common.Caching;
using wojilu.Members.Interface;
using wojilu.Caching;
using wojilu.Apps.Content.Domain;
using wojilu.Web.Context;
using wojilu.Web.Mvc.Utils;
using wojilu.Web.Mvc;
using wojilu.Common.AppBase.Interface;
using wojilu.Members.Sites.Domain;

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

            Admin.PostController post = new wojilu.Web.Controller.Content.Admin.PostController();
            observe( post.Create );
            observe( post.Delete );
            observe( post.DeleteTrue );
            observe( post.SaveAdmin );
            observe( post.Restore );
            observe( post.Update );
            observe( post.UpdateTitleStyle );

            //---------------------------------------------------------

            Admin.Section.ListController list = new wojilu.Web.Controller.Content.Admin.Section.ListController();
            //observe( list.Create );
            //observe( list.Update );
            observe( list.Delete );

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

            Admin.Section.VideoShowController vshow = new wojilu.Web.Controller.Content.Admin.Section.VideoShowController();
            observe( vshow.Create );
            observe( vshow.Update );
            observe( vshow.Delete );

            Admin.Section.ImgController img = new wojilu.Web.Controller.Content.Admin.Section.ImgController();
            observe( img.CreateListInfo );
            observe( img.CreateImgList );
            observe( img.SetLogo );
            observe( img.UpdateListInfo );
            observe( img.Delete );
            observe( img.DeleteImg );

            Admin.Section.PollController poll = new wojilu.Web.Controller.Content.Admin.Section.PollController();
            observe( poll.Create );
            observe( poll.Delete );
        }

        public override void UpdateCache( MvcContext ctx ) {


            IMember owner = ctx.owner.obj;
            if ((owner is Site) == false) return;

            if (ctx.app == null) {
            }
            else {
                CacheManager.GetApplicationCache().Remove( getCacheKey( owner, ctx.app.Id ) );
            }

            //int appId = ctx.app.Id;
            //String key = GetCacheKey( ctx, null );
            //String content = getIndexCache( appId, owner );
            //CacheManager.GetApplicationCache().Put( key, content );
        }

        //private static String getIndexCache( int appId, IMember owner ) {
        //    MvcContext ctx = MockContext.GetOne( owner, typeof( ContentApp ), appId );
        //    String content = ControllerRunner.Run( ctx, new wojilu.Web.Controller.Content.ContentController().Index );
        //    return content;
        //}

    }



}
