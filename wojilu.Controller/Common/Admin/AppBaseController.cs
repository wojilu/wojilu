/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.Web.Url;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Common.AppInstall;
using wojilu.Common.AppBase;
using wojilu.Common.Menus.Interface;
using wojilu.Common.MemberApp.Interface;

using wojilu.Members.Interface;
using wojilu.Members.Users.Domain;

using wojilu.Web.Controller.Security;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.Themes;

namespace wojilu.Web.Controller.Common.Admin {

    public partial class AppBaseController : ControllerBase {

        public IAppInstallerService appinfoService { get; set; }

        public IMemberAppService userAppService { get; set; }
        public IMenuService menuService { get; set; }
        public IThemeService themeService { get; set; }

        public AppBaseController() {
            appinfoService = new AppInstallerService();
            themeService = new ThemeService();
        }

        public virtual void log( String msg, IMemberApp app ) {
        }

        //-----------------------------------------------------------------


        public override void Layout() {
            set( "addAppLink", to( Select ) );
            set( "appListLink", to( Index ) );
        }

        public void Index() {
            IList apps = userAppService.GetByMember( ctx.owner.Id );
            bindAppList( apps );
            set( "sortAction", to( SortMenu ) );
        }

        public void SortMenu() {

            int id = ctx.PostInt( "id" );
            String cmd = ctx.Post( "cmd" );

            IMemberApp app = getApp( id );
            IList apps = userAppService.GetByMember( ctx.owner.Id );

            List<IMemberApp> list = new List<IMemberApp>();
            foreach (IMemberApp c in apps) list.Add( c );

            if (cmd == "up") {

                new SortUtil<IMemberApp>( app, list ).MoveUp();
                echoJsonOk();
            } else if (cmd == "down") {

                new SortUtil<IMemberApp>( app, list ).MoveDown();
                echoJsonOk();
            } else {
                echoError( lang( "exUnknowCmd" ) );
            }

        }


        //----------------------------------------------------------------------------------------------------------------

        public void ViewUrl( int id ) {

            IMemberApp app = getApp( id );

            String url = alink.ToUserAppFull( app );
            url = strUtil.Join( ctx.url.SiteUrl, url );

            set( "app.Name", app.Name );
            set( "app.Url", url );
        }

        public void Edit( int id ) {
            IMemberApp app = getApp( id );
            target( Update, id );
            bindAppEdit( app );
        }

        public void NewApp( int id ) {

            AppInstaller info = getAppInfo( id );

            if (info.IsClose( ctx.owner.obj.GetType() )) {
                echo( "app closed" );
                return;
            }

            if (!checkInstall( info )) return;

            target( Create );
            bindAppInfo( info );

            List<ITheme> themeList = themeService.GetThemeList( info );
            if (themeList.Count > 0) {
                String lnkThemeList = to( ThemeList, info.Id ) + "";
                set( "themeList", "<tr><td class=\"tdL\">主题类型</td><td><iframe id=\"tmplist\" src=\"" + lnkThemeList + "?frm=true\" frameborder=\"0\" scrolling=\"auto\" style=\"width:580px;\"></iframe></td></tr>" );

            } else {
                set( "themeList", "" );
            }

        }

        public void ThemeList( int appInstallerId ) {

            AppInstaller info = getAppInfo( appInstallerId );

            List<ITheme> themeList = themeService.GetThemeList( info );
            bindList( "list", "x", themeList, bindThemePic );
        }

        private void bindThemePic( IBlock block, String lbl, Object obj ) {

            ITheme theme = (ITheme)obj;

            if (strUtil.IsNullOrEmpty( theme.Pic )) {
                block.Set( "x.PicOrDesc", string.Format( "<div class=\"desc\"><div class=\"desc-inner\">{0}</div></div>", theme.Description ) );
            } else {
                block.Set( "x.PicOrDesc", string.Format( "<img src=\"{0}\" title=\"{1}\" />", getPicShow( theme.Pic ), theme.Description ) );
            }

        }

        private String getPicShow( String pic ) {
            if (strUtil.IsNullOrEmpty( pic )) return "";
            if (pic.StartsWith( "http:" )) return pic;
            if (pic.StartsWith( "/" )) return pic;
            return strUtil.Join( sys.Path.Static, "/theme/wojilu.Apps.Content/" ) + pic;

        }

        private Boolean checkInstall( AppInstaller info ) {
            if (isInstalled( info )) {
                //if (info.Singleton && userAppService.HasInstall( ctx.owner.Id, info.Id )) {
                echoRedirect( lang( "exAppInstalled" ) );
                return false;
            }
            return true;
        }


        [HttpPost, DbTransaction]
        public void Create() {

            int appInfoId = cvt.ToInt( ctx.Post( "appInfoId" ) );
            AppInstaller info = getAppInfo( appInfoId );

            if (info.IsClose( ctx.owner.obj.GetType() )) {
                echo( "app closed" );
                return;
            }

            if (!checkInstall( info )) return;

            String name = ctx.Post( "Name" );
            AccessStatus accs = AccessStatus.Public;

            if (strUtil.IsNullOrEmpty( name )) {
                echoError( "请填写名称" );
                return;
            }

            // 自定义安装
            Type appType = ObjectContext.Instance.TypeList[info.TypeFullName];
            if (rft.IsInterface( appType, typeof( IAppInstaller ) )) {

                // 主题ID
                String themeId = ctx.Post( "themeId" );

                IAppInstaller customInstaller = ObjectContext.CreateObject( appType ) as IAppInstaller;
                IMemberApp capp = customInstaller.Install( ctx, ctx.owner.obj, name, accs, themeId, "" );
                intiAppPermission( capp );


                echoToParentPart( lang( "opok" ), to( Index ), 1 );
                return;
            }

            // 主题安装
            if (strUtil.HasText( info.InstallerType )) {

                // 主题ID
                String themeId = ctx.Post( "themeId" );

                Type installerType = ObjectContext.GetType( info.InstallerType );

                IAppInstaller customInstaller = ObjectContext.CreateObject( installerType ) as IAppInstaller;
                IMemberApp capp = customInstaller.Install( ctx, ctx.owner.obj, name, accs, themeId, "" );
                intiAppPermission( capp );

                echoToParentPart( lang( "opok" ), to( Index ), 1 );
                return;

            }

            // 默认安装
            IMember owner = ctx.owner.obj;
            User creator = (User)ctx.viewer.obj;

            // 1、添加一条 IMemberApp
            IMemberApp app = userAppService.Add( creator, owner, name, info.Id, accs );

            if (app != null) {

                // 2、添加菜单
                String appUrl = UrlConverter.clearUrl( app, ctx );
                menuService.AddMenuByApp( app, name, "", appUrl );

                // 3、初始化权限
                intiAppPermission( app );

                log( SiteLogString.InsertApp(), app );


                echoToParentPart( lang( "opok" ), to( Index ), 1 );
            } else {
                errors.Add( lang( "exop" ) );

                run( NewApp, info.Id );
            }
        }

        private static void intiAppPermission( IMemberApp app ) {
            AppRole.InitSiteFront( app.Id );
            AppAdminRole.InitSiteAdmin( app.Id );
        }

        public void Select() {


            bindHomePage();

            IList apps = appinfoService.GetByOwnerType( ctx.owner.obj.GetType() );
            bindAppSelectList( apps );
        }

        [HttpPut, DbTransaction]
        public void Start( int id ) {
            IMemberApp app = getApp( id );
            String appUrl = UrlConverter.clearUrl( app, ctx );

            userAppService.Start( app, appUrl );
            log( SiteLogString.StartApp(), app );

            echoRedirectPart( lang( "opok" ), to( Index ), 0 );
        }

        private static readonly ILog logger = LogManager.GetLogger( typeof( AppBaseController ) );

        [HttpPut, DbTransaction]
        public void Stop( int id ) {
            IMemberApp app = getApp( id );
            String appUrl = UrlConverter.clearUrl( app, ctx );

            logger.Info( "stoped app url : " + appUrl );

            userAppService.Stop( app, appUrl );
            log( SiteLogString.StopApp(), app );

            //redirect( Index );
            //echoRedirect( lang( "opok" ), Index );
            echoRedirectPart( lang( "opok" ), to( Index ), 0 );
        }

        [HttpPost, DbTransaction]
        public void Update( int id ) {

            IMemberApp app = getApp( id );
            String appUrl = UrlConverter.clearUrl( app, ctx );

            String name = ctx.Post( "Name" );

            userAppService.Update( app, name, appUrl );

            log( SiteLogString.UpdateApp(), app );

            echoToParentPart( lang( "opok" ), to( Index ), 0 );
        }


        [HttpDelete, DbTransaction]
        public void Delete( int id ) {

            IMemberApp app = getApp( id );
            String appUrl = UrlConverter.clearUrl( app, ctx );

            userAppService.Delete( app, appUrl );

            log( SiteLogString.DeleteApp(), app );

            echoRedirect( lang( "opok" ), Index );
        }

        //-------------------------------------------------------------------------------------------

        private IMemberApp getApp( int id ) {
            IMemberApp app = userAppService.FindById( id, ctx.owner.Id );
            if (app == null) {
                throw new Exception( lang( "exAppNotFound" ) );
            }
            return app;
        }

        private AppInstaller getAppInfo( int appInfoId ) {
            AppInstaller appinfo = appinfoService.GetById( appInfoId );
            if (appinfo == null) {
                throw new Exception( lang( "exAppNotFound" ) );
            }
            return appinfo;
        }


    }
}

