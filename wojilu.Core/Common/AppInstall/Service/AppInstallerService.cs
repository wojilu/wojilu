/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

namespace wojilu.Common.AppInstall {

    public class AppInstallerService : IAppInstallerService {

        //public virtual List<AppInstaller> GetByOwnerType( Type ownerType ) {
        //    int catId = AppCategory.GetIdByOwnerType( ownerType.FullName );
        //    List<AppInstaller> all = GetAll();
        //    List<AppInstaller> results = new List<AppInstaller>();
        //    foreach (AppInstaller app in all) {
        //        if (app.CatId == catId || app.CatId== AppCategory.General) results.Add( app );
        //    }
        //    return results;
        //}

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
                if (a.Status == AppInstallerStatus.Run.Id) results.Add( a );
            }
            return results;
        }

        public virtual List<AppInstaller> GetByCategory( int categoryId ) {
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

        public virtual AppInstaller GetApprovedById( int id, Type ownerType ) {
            AppInstaller a = cdb.findById<AppInstaller>( id );
            if (a != null && a.Status != AppInstallerStatus.Stop.Id) return null;
            return a;
        }

        public virtual AppInstaller GetById( int id ) {
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


        public void UpdateStatus( AppInstaller installer, string postValues ) {

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

