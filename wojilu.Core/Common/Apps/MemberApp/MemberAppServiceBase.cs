/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.DI;
using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;
using wojilu.Common.AppInstall;
using wojilu.Common.AppBase;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.Menus.Interface;
using wojilu.Common.MemberApp.Interface;

namespace wojilu.Common.MemberApp {


    public abstract class MemberAppServiceBase : IMemberAppService {

        private AppInstallerService appInfoService = new AppInstallerService();

        public IMenuService menuService { get; set; }

        public abstract IMemberApp New();
        public abstract Object getObj();
        public abstract Type GetMemberType();

        private Type thisType() {
            return getObj().GetType();
        }

        public IMemberApp Add( User creator, String name, int appinfoId ) {
            return Add( creator, creator, name, appinfoId, AccessStatus.Public );
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

            db.insert( (IEntity)userApp );

            return userApp;
        }

        public void Delete( IMemberApp app, String rawAppUrl ) {
            db.delete( (IEntity)app );

            // 删除应用本身
            Type t = ObjectContext.Instance.TypeList[app.AppInfo.TypeFullName];
            ndb.delete( t, app.AppOid );

            menuService.RemoveMenuByApp( app, rawAppUrl );
        }

        public IMemberApp FindById( int userAppId, int userId ) {
            String c = "OwnerId=" + userId + " and Id=" + userAppId;
            return ndb.find( thisType(), c ).first() as IMemberApp;
            //return (getObj().find( string.Concat( new object[] { "OwnerId=", userId, " and Id=", userAppId } ) ).first() as IUserApp);
        }

        public IList GetAppInfos( int memberId ) {

            IList byMember = this.GetByMember( memberId );
            IList addedList = new ArrayList();
            foreach (IMemberApp app in byMember) {
                if (!this.isAdded( app, addedList )) {
                    addedList.Add( app.AppInfo );
                }
            }
            return addedList;
        }

        private Boolean isAdded( IMemberApp app, IList addedList ) {
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
            String c = "AppInfoId=" + appInfo.Id + " and AppOid=" + appId;
            return ndb.find( thisType(), c ).first() as IMemberApp;
            //return getObj().find( string.Concat( new object[] { "AppInfoId=", appInfo.Id, " and AppOid=", appId } ) ).first() as IUserApp;
        }

        public IList GetByMember( int memberId ) {

            if (memberId < 0) {
                return new ArrayList();
            }
            return ndb.find( thisType(), "OwnerId=" + memberId + " order by OrderId desc, Id asc" ).list();
            //return getObj().find( "OwnerId=" + memberId + " order by OrderId desc, Id asc" ).list();
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

            db.update( (IEntity)app );
            menuService.UpdateMenuByApp( app,rawAppUrl );

            //AppFactory.UpdateAccessStatus( app, accessStatus );

        }

        public void UpdateAccessStatus( IMemberApp app, AccessStatus accessStatus ) {

            app.AccessStatus = (int)accessStatus;

            db.update( (IEntity)app, "AccessStatus" );
            AppFactory.UpdateAccessStatus( app, accessStatus );
        }

        private void UpdateByStart( IMemberApp app ) {
            app.IsStop = 0;
            db.update( (IEntity)app, "IsStop" );
        }

        private void UpdateByStop( IMemberApp app ) {
            app.IsStop = 1;
            db.update( (IEntity)app, "IsStop" );
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

