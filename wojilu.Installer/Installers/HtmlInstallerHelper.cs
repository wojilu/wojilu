using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.MemberApp.Interface;
using wojilu.Apps.Content.Domain;
using wojilu.Members.Sites.Domain;
using wojilu.Web.Controller.Content.Admin;
using wojilu.Web.Mvc;
using wojilu.Web.Context;
using wojilu.Web.Jobs;

namespace wojilu.Web.Controller {

    public class HtmlInstallerHelper {

        public void MakeHtml( MvcContext ctx, IMemberApp portalApp, IMemberApp newsApp ) {

            // 1) 修改News的菜单
            updateStaticMenu( "news" );

            // 2) 生成静态页面
            makeHtml( ctx, portalApp, "index.htm" );
            makeHtml( ctx, newsApp, "news/index.htm" );

            // 3) 启动生成静态页面的job
            startJob( "wojilu.Web.Controller.Content.Htmls.HtmlJob" );
            startJob( "wojilu.Web.Controller.Content.Htmls.HomeHtmlJob" );
        }

        // 将静态频道的url，指向实际静态目录
        private void updateStaticMenu( String fUrl ) {
            SiteMenu menu = getMenuByUrl( fUrl );
            menu.Url = "";
            menu.RawUrl = fUrl + "/";
            menu.update();
        }

        private SiteMenu getMenuByUrl( string furl ) {
            List<SiteMenu> menus = cdb.findAll<SiteMenu>();
            foreach (SiteMenu x in menus) {
                if (x.Url == furl) return x;
            }
            return null;
        }

        // 生成频道的全部html
        private void makeHtml( MvcContext ctx, IMemberApp mApp, String staticPath ) {
            ContentApp app = initApp( ctx, mApp );
            setStaticPath( app, staticPath );

            HtmlController controller = ControllerFactory.FindController( typeof( HtmlController ), ctx ) as HtmlController;
            controller.MakeAll();
        }

        // 静态配置
        private void setStaticPath( ContentApp app, string staticPath ) {

            ContentSetting setting = app.GetSettingsObj();

            setting.IsAutoHtml = 1;
            setting.StaticPath = staticPath;
            setting.ArticleListMode = 1; // 摘要列表

            app.Settings = Json.ToString( setting );

            app.update();
        }

        // 初始化当前app
        private ContentApp initApp( MvcContext ctx, IMemberApp mapp ) {
            IAppContext context = new AppContext();
            int appId = mapp.AppOid;
            context.Id = appId;

            ContentApp app = ContentApp.findById( appId );
            context.obj = app;
            context.setAppType( app.GetType() );
            ctx.utils.setAppContext( context );
            return app;
        }

        private void startJob( String jobType ) {
            List<WebJob> jobs = cdb.findBy<WebJob>( "Type", jobType );
            WebJob job = jobs[0];
            job.IsRunning = true;
            job.update();
        }

    }
}
