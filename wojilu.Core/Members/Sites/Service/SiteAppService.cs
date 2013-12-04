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

        public virtual Type GetMemberType() {
            return typeof( Site );
        }

        public virtual IMemberApp New() {
            return new SiteApp();
        }

        public virtual IMemberApp Add(User creator, string name, long appinfoId) {

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


        public virtual IMemberApp Add(User creator, IMember owner, string name, long appinfoId, AccessStatus accessStatus) {

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

        public virtual void Delete( IMemberApp app, String rawAppUrl ) {
            ((SiteApp)app).delete();

            // 删除应用本身
            Type t = ObjectContext.Instance.TypeList[app.AppInfo.TypeFullName];
            ndb.delete( t, app.AppOid );

            menuService.RemoveMenuByApp( app, rawAppUrl );
        }

        public virtual IMemberApp FindById(long userAppId, long userId) {

            IList list = new SiteApp().findAll();
            foreach (IMemberApp app in list) {
                if ((app.OwnerId == userId) && (app.Id == userAppId)) {
                    return app;
                }
            }
            return null;
        }

        public virtual IList GetAppInfos(long memberId) {
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

        public virtual IMemberApp GetByApp( IApp app ) {
            return GetByApp( app.GetType(), app.Id );
        }


        public virtual IMemberApp GetByApp(Type t, long appId) {
            AppInstaller appInfo = appInfoService.GetByType( t );
            return GetByApp( appInfo.Id, appId );
        }

        public virtual IMemberApp GetByApp(long appInfoId, long appId) {
            IList list = new SiteApp().findAll();
            foreach (IMemberApp app in list) {
                if ((app.AppInfoId == appInfoId) && (app.AppOid == appId)) {
                    return app;
                }
            }
            return null;
        }

        public virtual IList GetByMember(long memberId) {
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



        public virtual void Start( IMemberApp app, String rawAppUrl ) {
            UpdateByStart( app );
            menuService.AddMenuByApp( app, app.Name, string.Empty, rawAppUrl );
        }

        public virtual void Stop( IMemberApp app, String rawAppUrl ) {
            UpdateByStop( app );
            menuService.RemoveMenuByApp( app, rawAppUrl );
        }

        public virtual void Update( IMemberApp app, String newName, String rawAppUrl ) {

            app.Name = newName;
            ((SiteApp)app).update();
            menuService.UpdateMenuByApp( app, rawAppUrl );

            // SiteApp 不使用 AccessStatus 控制权限
            //AppFactory.UpdateAccessStatus( app, accessStatus );

        }

        public virtual void UpdateAccessStatus( IMemberApp app, AccessStatus accessStatus ) {

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

        public virtual bool HasInstall(long ownerId, long appInfoId) {
            IList apps = GetByMember( ownerId );
            foreach (IMemberApp app in apps) {
                if (app.AppInfoId == appInfoId) return true;
            }
            return false;
        }

    }

}
