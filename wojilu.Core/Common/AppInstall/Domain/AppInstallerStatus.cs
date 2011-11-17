/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Common.AppInstall {

    public class AppInstallerStatus {

        private static AppInstallerStatus _run = new AppInstallerStatus() { Id = 0, Name = "已经启用" };
        private static AppInstallerStatus _stop = new AppInstallerStatus() { Id = 1, Name = "已禁用" };
        private static AppInstallerStatus _custom = new AppInstallerStatus() { Id = 2, Name = "自定义" };

        public static AppInstallerStatus Run {
            get { return _run; }
        }

        public static AppInstallerStatus Stop {
            get { return _stop; }
        }

        public static AppInstallerStatus Custom {
            get { return _custom; }
        }

        public static List<AppInstallerStatus> GetStatus() {

            List<AppInstallerStatus> list = new List<AppInstallerStatus>();
            list.Add( _run );
            list.Add( _stop );
            list.Add( _custom );

            return list;
        }

        public static AppInstallerStatus GetById( int id ) {
            if (id == 0) return _run;
            if (id == 1) return _stop;
            if (id == 2) return _custom;
            return null;
        }

        public static String GetNameById( int id ) {
            AppInstallerStatus s = GetById( id );
            return s == null ? "" : s.Name;
        }

        public int Id { get; set; }
        public String Name { get; set; }

    }

}
