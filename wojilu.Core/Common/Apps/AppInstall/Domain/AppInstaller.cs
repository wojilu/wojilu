/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Data;
using wojilu.ORM;
using wojilu.Members.Interface;

namespace wojilu.Common.AppInstall {


    public class AppInstaller : CacheObject {

        public AppInstaller() {
            this.Singleton = true;
        }

        public AppInstaller( int id ) {
            this.Id = id;
            this.Singleton = true;
        }

        /// <summary>
        /// 所属分类
        /// </summary>
        public int CatId { get; set; }

        /// <summary>
        /// app的作者或创建公司
        /// </summary>
        public String Creator { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// logo
        /// </summary>
        public String Logo { get; set; }

        /// <summary>
        /// 对应app的完整的type名称
        /// </summary>
        public String TypeFullName { get; set; }

        /// <summary>
        /// app状态(AppInstallerStatus)，默认0表示启用；1表示禁用；2表示自定义。
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 部署时间
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// 是否由用户生成内容(UGC)。如果有，应该提供对用户生成内容的后台管理界面。
        /// </summary>
        public Boolean HasUserData { get; set; }

        /// <summary>
        /// 是否单例(安装的时候是否只能安装一个运行，还是可以安装多个运行)
        /// </summary>
        public Boolean Singleton { get; set; }

        /// <summary>
        /// 父程序的ID(某些程序内核相同，但界面和初始化数据不一样，通过制定父程序ID，可以归类到同一组)
        /// </summary>
        public int ParentId { get; set; }


        /// <summary>
        /// 关闭方式。默认0表示禁止用户安装，但如果已安装了，则可以运行；1表示不但禁止安装，已经安装的也禁止运行。
        /// </summary>
        public int CloseMode { get; set; }

        /// <summary>
        /// 本App的主题类型，供安装时选择。
        /// </summary>
        public String ThemeType { get; set; }

        /// <summary>
        /// 自定义的主题安装器，可以根据主题进行安装
        /// </summary>
        public String InstallerType { get; set; }

        //-------------------------------------------------------------------------------------

        [NotSave]
        public String TypeName {
            get { return strUtil.GetTypeName( this.TypeFullName ); }
        }

        [NotSave]
        public String CatName {
            get { return AppCategory.GetNameById( this.CatId ); }
        }

        [NotSave]
        public String LogoImg {
            get { return this.Logo.Replace( "~img/", sys.Path.Img ); }
        }

        [NotSave]
        public String StatusName {

            get {

                if (this.Status == AppInstallerStatus.Stop.Id) return AppInstallerStatus.Stop.Name;

                //if (this.Status == AppInstallerStatus.Run.Id) return AppInstallerStatus.Run.Name;

                if (this.Status == AppInstallerStatus.Run.Id) {

                    if (this.CatId == AppCategory.General) {
                        return AppCategory.GetAllNameWithoutGeneral();
                    }
                    else {
                        return AppCategory.GetByCatId( this.CatId ).Name;
                    }

                }

                return AppMemberShip.GetStatusName( this.Id );

            }
        }

        [NotSave]
        public String StatusValue {

            get {

                if (this.Status == AppInstallerStatus.Stop.Id) return null;

                if (this.Status == AppInstallerStatus.Run.Id) {

                    if (this.CatId == AppCategory.General) {
                        return AppCategory.GetAllTypeNameWithoutGeneral();
                    }
                    else {
                        return AppCategory.GetByCatId( this.CatId ).TypeFullName;
                    }

                }

                return AppMemberShip.GetStatusTypeValue( this.Id );

            }

        }

        [NotSave]
        public String CloseModeName {
            get { return AppCloseMode.GetCloseModeName( this.CloseMode ); }
        }

        //-------------------------------------------------------------------------------------------------

        public Boolean IsInstanceClose( Type ownerType ) {

            if (this.CloseMode == AppCloseMode.CloseInstall.Id) return false; // 仅仅禁止安装，则对app实例不起作用

            return this.IsClose( ownerType );
        }

        public Boolean IsClose( Type ownerType ) {


            if (this.Status == AppInstallerStatus.Stop.Id) return true;

            if (this.Status == AppInstallerStatus.Run.Id) { // 默认状态
                return !(this.CatId == AppCategory.General || AppCategory.GetByCatId( this.CatId ).TypeFullName.Equals( ownerType.FullName ));
            }

            // 自定义
            return AppMemberShip.IsAppStop( this.Id, ownerType );
        }


    }


}

