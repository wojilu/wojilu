/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Members.Interface;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.MemberApp.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Data;

namespace wojilu.Common.AppInstall {

    public class AppInstallerService : IAppInstallerService {

        /// <summary>
        /// 安装某个app
        /// </summary>
        /// <param name="appType">必须事先IApp的类型，比如ForumApp</param>
        /// <param name="owner"></param>
        /// <param name="creator"></param>
        /// <param name="appName">app名称</param>
        /// <returns></returns>
        public virtual IMemberApp Install( Type appType, IMember owner, User creator, String appName ) {

            IApp objApp = ObjectContext.Create<IApp>( appType );

            objApp.OwnerId = owner.Id;
            objApp.OwnerType = owner.GetType().FullName;
            objApp.OwnerUrl = owner.Url;
            db.insert( objApp );

            AppInstaller info = GetByType( appType );

            IMemberApp mApp = ObjectContext.Create<IMemberApp>( getMemberAppType( owner ) );
            mApp.Creator = creator;
            mApp.CreatorUrl = creator.Url;
            mApp.AppInfoId = info.Id;
            mApp.AppOid = objApp.Id;
            mApp.Name = appName;

            if (mApp.GetType().IsSubclassOf( typeof( CacheObject ) )) {
                cdb.insert( (CacheObject)mApp );
            }
            else {
                db.insert( mApp );
            }

            return mApp;
        }

        private Type getMemberAppType( IMember owner ) {

            foreach (KeyValuePair<String, Type> kv in ObjectContext.Instance.TypeList) {

                if (rft.IsInterface( kv.Value, typeof( IMemberApp ) )) {

                    IMemberApp obj = ObjectContext.Create<IMemberApp>( kv.Value );
                    if (obj.OwnerType == owner.GetType().FullName) return kv.Value;

                }

            }

            return null;
        }


        public virtual List<AppInstaller> GetByOwnerType( Type ownerType ) {
            List<AppInstaller> all = GetAll();
            List<AppInstaller> results = new List<AppInstaller>();
            foreach (AppInstaller app in all) {

                if (app.IsClose( ownerType ) == false) results.Add( app );
            }
            return results;
        }

        public virtual List<AppInstaller> GetAll() {
            List<AppInstaller> results = new List<AppInstaller>();
            List<AppInstaller> list = cdb.findAll<AppInstaller>();
            foreach (AppInstaller a in list) {
                if (a.Status == AppInstallerStatus.Run.Id) {
                    results.Add( a );
                }
                // bug fixed by: http://www.wojilu.com/space/robin_qu/Blog2588/Post/355 
                else if (a.Status == AppInstallerStatus.Custom.Id) {
                    results.Add( a );
                }
            }
            return results;
        }

        public virtual List<AppInstaller> GetByCategory(long categoryId) {
            List<AppInstaller> all = this.GetAll();
            List<AppInstaller> results = new List<AppInstaller>();
            foreach (AppInstaller info in all) {
                if (info.CatId == categoryId) results.Add( info );
            }
            return results;
        }

        public virtual List<AppInstaller> GetUserDataAdmin() {
            List<AppInstaller> all = this.GetAll();
            List<AppInstaller> results = new List<AppInstaller>();
            foreach (AppInstaller info in all) {
                if (info.HasUserData) results.Add( info );
            }
            return results;
        }

        public virtual AppInstaller GetApprovedById(long id, Type ownerType) {
            AppInstaller a = cdb.findById<AppInstaller>( id );
            if (a != null && a.Status != AppInstallerStatus.Stop.Id) return null;
            return a;
        }

        public virtual AppInstaller GetById(long id) {
            return cdb.findById<AppInstaller>( id );
        }

        public virtual AppInstaller GetByType( Type appType ) {
            List<AppInstaller> all = cdb.findAll<AppInstaller>();
            foreach (AppInstaller info in all) {
                if (info.TypeFullName == appType.FullName) return info;
            }
            return null;

        }

        public virtual AppInstaller GetByTypeFullName( String typeFullName ) {
            List<AppInstaller> all = cdb.findAll<AppInstaller>();
            foreach (AppInstaller info in all) {
                if (info.TypeFullName == typeFullName) return info;
            }
            return null;
        }

        /// <summary>
        /// typeName必须包括app后缀
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public virtual AppInstaller GetByTypeName( String typeName ) {
            List<AppInstaller> all = this.GetAll();
            foreach (AppInstaller info in all) {
                if (info.TypeName == typeName) return info;
            }
            return null;
        }

        //------------------------------------------


        public virtual void UpdateStatus( AppInstaller installer, string postValues ) {

            // 如果没有值(什么都不选)
            if (strUtil.IsNullOrEmpty( postValues )) {
                clearAppMemberShip( installer );
                installer.Status = AppInstallerStatus.Stop.Id;
            }

            // 如果和默认值相同
            else if (valIsDefault( installer, postValues )) {
                clearAppMemberShip( installer );
                installer.Status = AppInstallerStatus.Run.Id;
            }

            // 自定义
            else {
                addAppMemberShip( installer, postValues );
                installer.Status = AppInstallerStatus.Custom.Id;
            }

            installer.update();
        }

        private void clearAppMemberShip( AppInstaller installer ) {

            List<AppMemberShip> list = cdb.findAll<AppMemberShip>();
            for (int i = 0; i < list.Count; i++) {
                if (list[i].AppInstallerId == installer.Id) list[i].delete();
            }

        }

        private bool valIsDefault( AppInstaller installer, string postValues ) {

            String[] arrValues = postValues.Split( ',' );

            // 通用的程序
            if (installer.CatId == AppCategory.General) {
                return matchAllMembers( arrValues );
            }
            else {
                AppCategory ac = AppCategory.GetByCatId( installer.CatId );
                return ac.TypeFullName.Equals( postValues.Trim() );
            }
        }

        private bool matchAllMembers( string[] arrValues ) {
            List<AppCategory> list = AppCategory.GetAllWithoutGeneral();
            foreach (AppCategory ac in list) {
                if (notInMemberType( ac, arrValues )) return false;
            }
            return true;
        }

        private bool notInMemberType( AppCategory ac, string[] arrValues ) {
            foreach (String val in arrValues) {
                if (strUtil.IsNullOrEmpty( val )) continue;
                if (val.Trim().Equals( ac.TypeFullName )) return false;
            }
            return true;
        }



        private void addAppMemberShip( AppInstaller installer, string postValues ) {

            // 先删掉已有的
            clearAppMemberShip( installer );

            // 增加现在的
            String[] arrValues = postValues.Split( ',' );
            foreach (String val in arrValues) {

                if (strUtil.IsNullOrEmpty( val )) continue;
                AppMemberShip am = new AppMemberShip();
                am.AppInstallerId = installer.Id;
                am.MemberTypeName = val.Trim();
                am.insert();
            }

        }

    }
}

