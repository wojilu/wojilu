/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.Web.Mvc;

using wojilu.Web.Controller.Admin.Credits;
using wojilu.Web.Controller.Admin.Sys;
using wojilu.Web.Controller.Admin.Members;
using wojilu.Web.Controller.Admin.Security;
using wojilu.Web.Controller.Admin.Groups;
using wojilu.Web.Controller.Admin;
using wojilu.Common.MemberApp;
using wojilu.Common.MemberApp.Interface;
using wojilu.Web.Controller.Admin.Mb;
using wojilu.Web.Controller.Admin.Spiders;

namespace wojilu.Web.Controller.Security {

    /// <summary>
    /// 后台导航分成三个部分：由2个导航菜单(横向菜单+首页左下角) + 1个app列表(首页左侧)
    /// 其中2个导航菜单在这里直接初始化到内存中；app列表则存储在磁盘里
    /// 
    /// 如果需要在后台加入操作，需要完成两个步骤：
    /// 1）确定它所在的菜单，你可以放在横向菜单或首页左下角
    /// 2）将操作纳入权限控制，即 SiteAdminOperation 中
    /// </summary>
    public class OperationDB {

        private static readonly String rootNamespace = "wojilu.Web.Controller";

        public List<SiteAdminOperation> SiteAdminOperations { get; set; }
        public List<SiteAdminMenu> SiteAdminMenus { get; set; }
        public List<SiteDataAdminMenu> SiteDataAdminMenus { get; set; }

        private OperationDB() {
            initSiteAdminMenus(); // 初始化菜单1: 后台横向菜单
            initSiteDataAdminMenu(); // 初始化菜单2: 后台首页左下角菜单
            initSiteAdminOperations(); // 初始化权限操作
        }

        private static readonly OperationDB _instance = new OperationDB();

        public static OperationDB GetInstance() {
            return _instance;
        }

        // 菜单1: 后台横向导航菜单
        private void initSiteAdminMenus() {

            List<SiteAdminMenu> results = new List<SiteAdminMenu>();

            // 第二个参数是图片，第三个参数是名称
            results.Add( new SiteAdminMenu( 1, "home", lang.get( "adminHome" ), new DashboardController().Index, rootNamespace ) );
            //results.Add( new SiteAdminMenu( 2, "apps", lang.get( "adminApp" ), "", SiteAdminMenu.AppTag ) );
            results.Add( new SiteAdminMenu( 3, "userdata", lang.get( "adminUserData" ), "", SiteAdminMenu.UserDataAdminTag ) );
            results.Add( new SiteAdminMenu( 4, "users", lang.get( "userAdmin" ), new UserController().Index, rootNamespace ) );
            results.Add( new SiteAdminMenu( 7, "talk", lang.get( "adminGroup" ), new GroupController().Index, rootNamespace ) );
            results.Add( new SiteAdminMenu( 5, "security", lang.get( "adminSecurity" ), new Admin.Security.SecurityController().Index, rootNamespace ) );
            results.Add( new SiteAdminMenu( 6, "stats", lang.get( "adminCurrencyCredit" ), new CurrencyController().Index, rootNamespace ) );
            results.Add( new SiteAdminMenu( 8, "setting", lang.get( "adminSiteSettings" ), new SiteConfigController().Base, rootNamespace ) );
            results.Add( new SiteAdminMenu( 9, "skin", lang.get( "adminSiteSkin" ), new Admin.SiteSkinController().List, rootNamespace ) );
            results.Add( new SiteAdminMenu( 10, "calendar", lang.get( "adminSiteLog" ), new Admin.Sys.SiteLogController().Index, rootNamespace ) );

            this.SiteAdminMenus = results;
        }

        // 菜单2: 后台首页左下角菜单
        private void initSiteDataAdminMenu() {
            List<SiteDataAdminMenu> results = new List<SiteDataAdminMenu>();
            // 第二个参数是图片，第三个参数是名称
            //results.Add( new SiteDataAdminMenu( 1, "settings", lang.get( "adminSiteApp" ), new AppController().Index, rootNamespace ) );
            //results.Add( new SiteDataAdminMenu( 2, "menus", lang.get( "menuAdmin" ), new MenuController().Index, rootNamespace ) );
            results.Add( new SiteDataAdminMenu( 3, "link", lang.get( "pageFooterLink" ), new FooterMenuController().List, rootNamespace ) );
            results.Add( new SiteDataAdminMenu( 4, "url", lang.get( "adminSiteUrl" ), new DashboardController().Links, rootNamespace ) );
            results.Add( new SiteDataAdminMenu( 5, "doc", lang.get( "commonPage" ), new PageCategoryController().List, rootNamespace ) );
            results.Add( new SiteDataAdminMenu( 6, "microblog", "微博管理", new MicroblogController().List, rootNamespace ) );
            results.Add( new SiteDataAdminMenu( 7, "tag", "tag 管理", new TagAdminController().Index, rootNamespace ) );
            results.Add( new SiteDataAdminMenu( 8, "cache", "缓存管理", new CacheController().Index, rootNamespace ) );
            results.Add( new SiteDataAdminMenu( 9, "ad", "广告设置", new AdController().Index, rootNamespace ) );

            results.Add( new SiteDataAdminMenu( 10, "spider", "采集管理", new TemplateController().List, rootNamespace ) );
            results.Add( new SiteDataAdminMenu( 11, "comment", "评论管理", new CommentController().List, rootNamespace ) );
            results.Add( new SiteDataAdminMenu( 12, "connect", "第三方集成", new ConnectAdminController().Index, rootNamespace ) );

            this.SiteDataAdminMenus = results;
        }

        private static List<SiteDataAdminMenu> GetAppAndMenu() {
            List<SiteDataAdminMenu> results = new List<SiteDataAdminMenu>();
            results.Add( new SiteDataAdminMenu( new AppController().Index, rootNamespace ) );
            results.Add( new SiteDataAdminMenu( new AppController().Select, rootNamespace ) );
            results.Add( new SiteDataAdminMenu( new MenuController().Index, rootNamespace ) );
            results.Add( new SiteDataAdminMenu( new MenuController().AddMenu, rootNamespace ) );
            results.Add( new SiteDataAdminMenu( new AppConfigController().App, rootNamespace ) );

            return results;
        }

        public static SiteDataAdminMenu GetMenu( int index ) {
            return GetAppAndMenu()[index];
        }

        // 所有需要纳入细分权限的后台操作列表
        private void initSiteAdminOperations() {

            List<SiteAdminOperation> results = new List<SiteAdminOperation>();

            // 第三个参数 menuId 即 SiteAdminMenus 中对应的 menu 的ID

            results.Add( new SiteAdminOperation( 1, lang.get( "adminHome" ), 1, new DashboardController().Index, rootNamespace ) );

            results.Add( new SiteAdminOperation( 2, lang.get( "userAdmin" ), 4, typeof( UserController ) ) );

            results.Add( new SiteAdminOperation( 3, lang.get( "adminRole" ), 5, typeof( Admin.Security.SecurityController ) ) );
            results.Add( new SiteAdminOperation( 4, lang.get( "adminFrontSecurity" ), 5, typeof( PermissionFrontController ) ) );
            results.Add( new SiteAdminOperation( 5, lang.get( "adminBackSecurity" ), 5, typeof( PermissionBackController ) ) );
            results.Add( new SiteAdminOperation( 6, lang.get( "adminCurrency" ), 6, typeof( CurrencyController ) ) );
            results.Add( new SiteAdminOperation( 7, lang.get( "adminCredit" ), 6, typeof( CreditController ) ) );
            results.Add( new SiteAdminOperation( 8, lang.get( "adminGroup" ), 7, typeof( GroupController ) ) );
            results.Add( new SiteAdminOperation( 9, lang.get( "adminSiteSettings" ), 8, typeof( SiteConfigController ) ) );
            results.Add( new SiteAdminOperation( 10, lang.get( "adminSiteSkin" ), 9, typeof( SiteSkinController ) ) );
            results.Add( new SiteAdminOperation( 11, lang.get( "adminSiteLog" ), 10, typeof( SiteLogController ) ) );

            results.Add( new SiteAdminOperation( 12, lang.get( "adminSiteApp" ), 1, typeof( AppController ) ) );
            results.Add( new SiteAdminOperation( 13, lang.get( "adminNav" ), 1, typeof( MenuController ) ) );
            results.Add( new SiteAdminOperation( 14, lang.get( "pageFooterLink" ), 1, typeof( FooterMenuController ) ) );
            results.Add( new SiteAdminOperation( 15, lang.get( "adminSiteUrl" ), 1, new DashboardController().Links, rootNamespace ) );
            Type[] ptypes = new Type[] { typeof( PageController ), typeof( PageCategoryController ) };
            results.Add( new SiteAdminOperation( 16, lang.get( "commonPage" ), 1, ptypes ) );

            results.Add( new SiteAdminOperation( 17, "微博管理", 1, typeof( MicroblogController ) ) );
            results.Add( new SiteAdminOperation( 18, "tag 管理", 1, typeof( TagAdminController ) ) );
            results.Add( new SiteAdminOperation( 19, "缓存管理", 1, typeof( CacheController ) ) );
            results.Add( new SiteAdminOperation( 20, "广告管理", 1, typeof( AdController ) ) );
            results.Add( new SiteAdminOperation( 21, "采集管理", 1, typeof( TemplateController ) ) );
            results.Add( new SiteAdminOperation( 22, "评论管理", 1, typeof( CommentController ) ) );
            results.Add( new SiteAdminOperation( 23, "第三方集成", 1, typeof( ConnectAdminController ) ) );
            results.Add( new SiteAdminOperation( 24, "App和组件配置", 1, typeof( AppConfigController ) ) );

            this.SiteAdminOperations = results;
        }


        public static SiteAdminOperation GetForbiddenAdminOperations( String checkedPath ) {

            foreach (SiteAdminOperation op in GetInstance().SiteAdminOperations) {

                if (op == null) continue;

                foreach (String forbiddenUrl in op.GetUrlList()) {
                    if (checkedPath.StartsWith( forbiddenUrl )) return op;
                }

            }

            return null;
        }



    }

}
