using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Controller.Layouts;
using wojilu.Members.Sites.Service;
using wojilu.Members.Sites.Interface;

namespace wojilu.Web.Controller.Microblogs {

    public class LayoutController : ControllerBase {

        public ISiteSkinService siteSkinService { get; set; }

        public LayoutController() {
            siteSkinService = new SiteSkinService();
        }

        public override void Layout() {

            ctx.controller.HideLayout( typeof( wojilu.Web.Controller.LayoutController ) );

            set( "siteName", config.Instance.Site.SiteName );
            set( "microblogHomeLink", getFullUrl( alink.ToMicroblog() ) );

            utils.renderPageMetaToView();

            String skinContent = siteSkinService.GetSkin();
            set( "siteSkinContent", skinContent );

            //loadHtml( "topNav", to( new wojilu.Web.Controller.Layouts.TopNavController().Index ) );
            load( "topNav", new TopNavController().Index );

            IBlock block = getBlock( "myNav" );

            if (ctx.viewer.IsLogin==false) return;

            block.Set( "myHomeLink", Link.To( ctx.viewer.obj, new Microblogs.My.MicroblogController().Home ) );
            block.Set( "myPageLink", alink.ToUserMicroblog( ctx.viewer.obj ) );
            block.Set( "atmeLink", to( new My.MicroblogController().Atme ) );
            block.Set( "msgLink", to( new Users.Admin.MsgController().Index ) );


            block.Set( "searchAction", to( new My.MicroblogController().Search ) );
            String q = strUtil.SqlClean( ctx.Get( "q" ), 10 );
            block.Set( "searchKey", q );

            block.Next();
        }

        private String getFullUrl( String url ) {
            if (url == null) return "";
            if (url.StartsWith( "http" )) return url;
            return strUtil.Join( ctx.url.SiteAndAppPath, url );
        }

    }

}
