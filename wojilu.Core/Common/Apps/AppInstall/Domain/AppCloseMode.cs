/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Common.AppInstall {

    public class AppCloseMode {

        private static AppCloseMode _closeInstall = new AppCloseMode() { Id = 0, Name = "仅禁止安装" };
        private static AppCloseMode _closeInstallCloseRun = new AppCloseMode() { Id = 1, Name = "除了禁止安装，已安装的app也禁止运行" };

        public static AppCloseMode CloseInstall {
            get { return _closeInstall; }
        }

        public static AppCloseMode CloseInstallCloseRun {
            get { return _closeInstallCloseRun; }
        }

        public static List<AppCloseMode> GetAllMode() {

            List<AppCloseMode> list = new List<AppCloseMode>();
            list.Add( _closeInstall );
            list.Add( _closeInstallCloseRun );

            return list;
        }

        public static AppCloseMode GetById( int id ) {
            if (id == 0) return _closeInstall;
            if (id == 1) return _closeInstallCloseRun;
            return null;
        }

        public static String GetCloseModeName( int id ) {
            AppCloseMode m = GetById( id );
            return m == null ? "" : m.Name;
        }

        public int Id { get; set; }
        public String Name { get; set; }



    }

}
