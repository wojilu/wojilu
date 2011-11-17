/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;


using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;
using wojilu.Members.Sites.Domain;
using wojilu.Common.AppInstall;
using wojilu.Common.AppBase;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.Menus.Interface;
using wojilu.Common.MemberApp.Interface;
using System.Collections.Generic;
using wojilu.DI;

namespace wojilu.Members.Sites.Service {

    public class SiteAppService : IMemberAppService {

        private AppInstallerService appInfoService;
        public IMenuService menuService { get; set; }

        public SiteAppService() {
            menuService = new SiteMenuService();
            appInfoService = new AppInstallerService();
        }

        public Type GetMemberType() {
            return typeof( Site );
        }

        public IMemberApp New() {
            return new SiteApp();
        }

        public IMemberApp Add( User creator, String name, int appinfoId ) {

            //创建应用
            IApp app = AppFactory.Create( appinfoId, creator, AccessStatus.Public );

            // 创建控制面板中的 程序
            IMemberApp userApp = New(); 
            userApp.AppInfoId = appinfoId;
            userApp.AppOid = app.Id;
            userApp.OwnerId = creator.Id;
            userApp.OwnerUrl = creator.Url;
            userApp.OwnerType = creator.GetType().FullName;
            userApp.Creator = creator;
            userApp.CreatorUrl = creator.Url;
            userApp.Name = name;
            userApp.AccessStatus = (int)AccessStatus.Public;

            Insert( userApp );

            return userApp;
        }

        private Result Insert( IMemberApp app ) {
            app.Created = DateTime.Now;
            ((SiteApp)app).insert();
            return new Result();
        }


        public IMemberApp Add( User creator, IMember owner, String name, int appinfoId, AccessStatus accessStatus ) {

            // 创建应用实例
            IApp app = AppFactory.Create( appinfoId, owner, accessStatus );

            // 创建控制面板中的 程序
            IMemberApp userApp = New(); 
            userApp.AppInfoId = appinfoId;
            userApp.AppOid = app.Id;
            userApp.OwnerId = owner.Id;
            userApp.OwnerUrl = owner.Url;
            userApp.OwnerType = owner.GetType().FullName;
            userApp.Creator = creator;
            userApp.CreatorUrl = creator.Url;
            userApp.Name = name;
            userApp.AccessStatus = (int)accessStatus;

            Insert( userApp );

            return userApp;

        }

        public void Delete( IMemberApp app, String rawAppUrl ) {
            ((SiteApp)app).delete();

            // 删除应用本身
            Type t = ObjectContext.Instance.TypeList[app.AppInfo.TypeFullName];
            ndb.delete( t, app.AppOid );

            menuService.RemoveMenuByApp( app, rawAppUrl );
        }

        public IMemberApp FindById( int userAppId, int userId ) {

            IList list = new SiteApp().findAll();
            foreach (IMemberApp app in list) {
                if ((app.OwnerId == userId) && (app.Id == userAppId)) {
                    return app;
                }
            }
            return null;
        }

        public IList GetAppInfos( int memberId ) {
            IList byMember = this.GetByMember( memberId );
            IList addedList = new ArrayList();
            foreach (SiteApp app in byMember) {
                if (!this.isAdded( app, addedList )) {
                    addedList.Add( app.AppInfo );
                }
            }
            return addedList;
        }

        private Boolean isAdded( SiteApp app, IList addedList ) {
            foreach (AppInstaller info in addedList) {
                if (app.AppInfo.Id == info.Id) {
                    return true;
                }
            }
            return false;
        }

        public IMemberApp GetByApp( IApp app ) {
            return GetByApp( app.GetType(), app.Id );
        }


        public IMemberApp GetByApp( Type t, int appId ) {
            AppInstaller appInfo = appInfoService.GetByType( t );
            return GetByApp( appInfo.Id, appId );
        }

        public IMemberApp GetByApp( int appInfoId, int appId ) {
            IList list = new SiteApp().findAll();
            foreach (IMemberApp app in list) {
                if ((app.AppInfoId == appInfoId) && (app.AppOid == appId)) {
                    return app;
                }
            }
            return null;
        }

        public IList GetByMember( int memberId ) {
            if (memberId < 0) {
                return new ArrayList();
            }
            List<SiteApp> list = cdb.findAll<SiteApp>();
            ArrayList results = new ArrayList();
            foreach (IMemberApp app in list) {
                if (app.OwnerId == memberId) {
                    results.Add( app );
                }
            }
            results.Sort();
            return results;
        }



        public void Start( IMemberApp app, String rawAppUrl ) {
            UpdateByStart( app );
            menuService.AddMenuByApp( app, app.Name, string.Empty, rawAppUrl );
        }

        public void Stop( IMemberApp app, String rawAppUrl ) {
            UpdateByStop( app );
            menuService.RemoveMenuByApp( app, rawAppUrl );
        }

        public void Update( IMemberApp app, String newName, String rawAppUrl ) {

            app.Name = newName;
            ((SiteApp)app).update();
            menuService.UpdateMenuByApp( app, rawAppUrl );

            // SiteApp 不使用 AccessStatus 控制权限
            //AppFactory.UpdateAccessStatus( app, accessStatus );

        }

        public void UpdateAccessStatus( IMemberApp app, AccessStatus accessStatus ) {

            app.AccessStatus = (int)accessStatus;
            ((SiteApp)app).update();
            //AppFactory.UpdateAccessStatus( app, accessStatus );
        }

        private void UpdateByStart( IMemberApp app ) {
            app.IsStop = 0;
            ((SiteApp)app).update();
        }

        private void UpdateByStop( IMemberApp app ) {
            app.IsStop = 1;
            ((SiteApp)app).update();
        }

        public Boolean HasInstall( int ownerId, int appInfoId ) {
            IList apps = GetByMember( ownerId );
            foreach (IMemberApp app in apps) {
                if (app.AppInfoId == appInfoId) return true;
            }
            return false;
        }

    }

}
