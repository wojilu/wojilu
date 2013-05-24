/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.Web.Mvc;

using wojilu.Members.Users.Domain;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Sites.Service;

using wojilu.Common.AppInstall;
using wojilu.Common.Menus;
using wojilu.Common.Onlines;

using wojilu.Web.Controller.Admin.Sys;
using wojilu.Web.Controller.Security;
using wojilu.Common.Menus.Interface;
using wojilu.Common.MemberApp.Interface;
using wojilu.Members.Sites.Interface;
using wojilu.Common;
using wojilu.Web.Controller.Common;

namespace wojilu.Web.Controller.Layouts {

    public partial class SiteLayoutController : ControllerBase {

        public IMemberAppService siteAppService { get; set; }
        public IAppInstallerService appInfoService { get; set; }
        public ISiteSkinService siteSkinService { get; set; }
        private IMenuService menuService { get; set; }

        public SiteLayoutController() {

            siteAppService = new SiteAppService();
            siteAppService.menuService = new SiteMenuService();
            appInfoService = new AppInstallerService();
            menuService = new SiteMenuService();
            siteSkinService = new SiteSkinService();
        }

        public override void Layout() {

            load( "topNav", new TopNavController().IndexNew );
            load( "header", new TopNavController().Header );
            set( "statsJs", config.Instance.Site.GetStatsJs() );

            set( "adFooter", AdItem.GetAdById( AdCategory.Footer ) );

            set( "adLoadLink", to( new AdLoaderController().Index ) );


            bindCommon();
            bindSiteSkin();

            set( "siteBeiAn", config.Instance.Site.BeiAn );
            set( "copyright", lang( "siteCopyright" ) );
            set( "ramsize", lang( "memoryUse" ) + ": " + (((Environment.WorkingSet / 1024) / 1024)) + " MB" );

            List<FooterMenu> menus = FooterMenu.GetAll();
            bindFooterMenus( menus );


            set( "customSkinLink", to( new Admin.SiteSkinController().CustomBg ) );
        }

        public void AdminLayout() {

            if (strUtil.IsNullOrEmpty( Page.Title )) {
                Page.Title = "wojilu " + lang( "adminTitle" );
            }

            bindCommon();

            set( "sitename", config.Instance.Site.SiteName );
            set( "copyright", config.Instance.Site.Copyright );
            set( "viewer.Name", ctx.viewer.obj.Name );

            set( "siteLink", "<a href='" + sys.Path.Root + "'>" + lang( "siteHome" ) + "</a>" );
            set( "site.OnlineCount", OnlineStats.Instance.Count );

            set( "site.AdminLink", Link.To( new DashboardController().Index ) );
            set( "site.LogoutLink", to( new Admin.MainController().Logout ) );
            set( "lostPage", Link.To( Site.Instance, new MainController().lost ) );

            List<SiteAdminMenu> menus = OperationDB.GetInstance().SiteAdminMenus;
            List<SiteAdminOperation> userActions = SiteAdminOperationConfig.Instance.GetActionsByUser( (User)ctx.viewer.obj );
            IList apps = siteAppService.GetByMember( Site.Instance.Id );
            List<AppInstaller> userDataApps = appInfoService.GetUserDataAdmin();

            bindAdminMenus( menus, userActions, apps, userDataApps );

            bindLeftNav( apps );
        }



        private void bindSiteSkin() {
            String skinContent = siteSkinService.GetSkin( ctx.GetInt( "skinId" ), MvcConfig.Instance.CssVersion );
            set( "siteSkinContent", skinContent );
        }

        private void bindCommon() {

            utils.renderPageMetaToView();
            set( "langStr", wojilu.lang.getLangString() );
        }



    }

}
