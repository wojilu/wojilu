using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Members.Interface;
using wojilu.Members.Sites.Domain;
using wojilu.Web.Context;
using wojilu.Apps.Content.Domain;
using wojilu.Web.Mvc.Utils;
using System.IO;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Web.Controller.Content.Caching {

    /*
    public class HtmlCacheAction : ActionCache {

        public override void ObserveActions() {


            Admin.ContentController ac = new wojilu.Web.Controller.Content.Admin.ContentController();
            observe( ac.AddRow );
            observe( ac.DeleteRow );
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

        public override void UpdateCache( wojilu.Web.Context.MvcContext ctx ) {

            IMember owner = ctx.owner.obj;
            if ((owner is Site) == false) return;

            
            //makeHtml( ctx );
        }

        // 生成静态 html 页面
        private static void makeHtml( wojilu.Web.Context.MvcContext ctx ) {

            String addr = strUtil.Join( ctx.url.SiteAndAppPath, alink.ToApp( (IApp)ctx.app.obj, ctx ) );
            String html = makeHtml( addr );

            file.Write( PathHelper.Map( "/html/site_content_" + ctx.app.Id + ".html" ), html );
        }

        private static String makeHtml( String addr ) {
            StringWriter sw = new StringWriter();
            IWebContext webContext = MockWebContext.New( 0, addr, sw );
            new CoreHandler().ProcessRequest( webContext );
            return sw.ToString();
        }


    }
     * 
     * */

}
