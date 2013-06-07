/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Data;
using wojilu.ORM;

namespace wojilu.Common.AppInstall {

    // app 与 IMember 的多对多映射表
    // 如果没有配置，则使用CatId中的配置
    // 如果 AppInstaller 的 Status==0 ，则表示正常可用(默认)
    // 如果 AppInstaller 的 Status==1 ，则表示全部停止
    // 如果 AppInstaller 的 Status==2 ，则表示使用本表自定义
    public class AppMemberShip : CacheObject {

        public int AppInstallerId { get; set; }
        public String MemberTypeName { get; set; }

        [NotSave]
        public String MemberName {

            get {
                return AppCategory.GetNameByType( this.MemberTypeName );
            }
        }

        //--------------------------------------------

        public static String GetStatusName( int installerId ) {

            List<AppMemberShip> list = cdb.findBy<AppMemberShip>( "AppInstallerId", installerId );

            String str = "";
            foreach (AppMemberShip am in list) {
                str += am.MemberName + ",";
            }

            return str.TrimEnd( ',' );

        }

        public static String GetStatusTypeValue( int installerId ) {

            List<AppMemberShip> list = cdb.findBy<AppMemberShip>( "AppInstallerId", installerId );

            String str = "";
            foreach (AppMemberShip am in list) {
                str += am.MemberTypeName + ",";
            }

            return str.TrimEnd( ',' );

        }
        //--------------------------------------------

        public static Boolean IsAppStop( int installId, Type ownerType ) {

            List<AppMemberShip> list = cdb.findAll<AppMemberShip>();

            foreach (AppMemberShip am in list) {
                if (am.AppInstallerId == installId && am.MemberTypeName.Equals( ownerType.FullName )) return false;
            }

            return true;
        }


    }


}
