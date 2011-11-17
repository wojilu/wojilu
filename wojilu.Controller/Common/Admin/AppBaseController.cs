/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.Web.Url;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.DI;
using wojilu.Common.AppInstall;
using wojilu.Common.AppBase;
using wojilu.Common.Menus.Interface;
using wojilu.Common.MemberApp.Interface;

using wojilu.Members.Interface;
using wojilu.Members.Users.Domain;

using wojilu.Web.Controller.Security;
using wojilu.Common.AppBase.Interface;
using System.Collections.Generic;

namespace wojilu.Web.Controller.Common.Admin {

    public partial class AppBaseController : ControllerBase {

        public IAppInstallerService appinfoService { get; set; }

        public IMemberAppService userAppService { get; set; }
        public IMenuService menuService { get; set; }

        public AppBaseController() {
            appinfoService = new AppInstallerService();
        }

        public virtual void log( String msg, IMemberApp app ) {
        }

        //-----------------------------------------------------------------


        public override void Layout() {
            set( "addAppLink", to( Select ) );
            set( "appListLink", to( Index ) );
        }

        public void Index() {
            //set( "addAppLink", to( Select ) );
            //set( "appListLink", to( List ) );
            //load( "list", List );
            //}

            //public void List() {
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
                echoRedirect( "ok" );
            }
            else if (cmd == "down") {

                new SortUtil<IMemberApp>( app, list ).MoveDown();
                echoRedirect( "ok" );
            }
            else {
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
            //AccessStatus accs = AccessStatusUtil.GetPostValue( ctx.PostInt( "AccessStatus" ) );
            AccessStatus accs = AccessStatus.Public;


            Type appType = ObjectContext.Instance.TypeList[info.TypeFullName];
            if (rft.IsInterface( appType, typeof( IAppInstaller ) )) {
                IAppInstaller customInstaller = ObjectContext.CreateObject( appType ) as IAppInstaller;
                IMemberApp capp = customInstaller.Install( ctx, ctx.owner.obj, name, accs );
                intiAppPermission( capp );

                echoToParentPart( lang( "opok" ), to( Index ), 1 );
                return;
            }


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
            }
            else {
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

            //echoRedirect( lang( "opok" ), Index );
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

