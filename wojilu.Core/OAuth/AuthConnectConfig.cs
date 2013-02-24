/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wojilu.Data;
using wojilu.ORM;
using wojilu.Common.AppBase;

namespace wojilu.OAuth {

    /*
    [
        {Name:"facebook connect", TypeFullName:"wojilu.OpenAuth.Connects.FacebookConnect", ConsumerKey:"xx1", ConsumerSecret:"xx2"}
    ]
    */

    public class AuthConnectConfig : CacheObject, ISort, IComparable {

        // base.Name

        public String LoginName { get; set; } // 登录名称

        public String TypeFullName { get; set; }

        public String ConsumerKey { get; set; }
        public String ConsumerSecret { get; set; }

        public int IsStop { get; set; }

        // 在顶部登录栏中提前显示，不放在“更多登录”下拉菜单中
        public int IsPick { get; set; }

        [NotSave]
        public String LogoM { get; set; }

        [NotSave]
        public String LogoS { get; set; }


        // 排序编号
        public int OrderId { get; set; }

        public void updateOrderId() {
            this.update();
        }

        public int CompareTo( object obj ) {
            AuthConnectConfig t = obj as AuthConnectConfig;
            if (this.OrderId > t.OrderId) return -1;
            if (this.OrderId < t.OrderId) return 1;
            if (this.Id > t.Id) return 1;
            if (this.Id < t.Id) return -1;
            return 0;
        }


        //---------------------------service 方法--------------------------------------------------------

        public static List<AuthConnectConfig> GetAll() {

            List<AuthConnectConfig> xlist = cdb.findAll<AuthConnectConfig>();
            xlist.Sort();

            foreach (AuthConnectConfig x in xlist) {

                String logoPath = string.Format( "/connect/{0}.png", x.TypeFullName );
                String logoPaths = string.Format( "/connect/{0}_s.png", x.TypeFullName );
                logoPath = strUtil.Join( sys.Path.DiskImg, logoPath );
                logoPaths = strUtil.Join( sys.Path.DiskImg, logoPaths );

                if (file.Exists( PathHelper.Map( logoPath ) )) {

                    x.LogoM = logoPath;
                    x.LogoS = logoPaths;

                }

            }

            return xlist;
        }

        public static AuthConnectConfig GetById( int id ) {

            List<AuthConnectConfig> xlist = GetAll();

            foreach (AuthConnectConfig x in xlist) {
                if (x.Id == id) return x;
            }

            return null;
        }


        public static AuthConnectConfig GetByType( String typeFullName ) {

            List<AuthConnectConfig> xlist = GetAll();

            foreach (AuthConnectConfig x in xlist) {
                if (x.TypeFullName == typeFullName) return x;
            }

            return null;
        }

        public static List<AuthConnectConfig> GetEnabledList() {

            List<AuthConnectConfig> list = new List<AuthConnectConfig>();
            List<AuthConnectConfig> xlist = GetAll();
            foreach (AuthConnectConfig x in xlist) {
                if (x.IsStop == 0) list.Add( x );
            }
            return list;
        }




    }

}
