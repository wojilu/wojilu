/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.ORM;
using wojilu.Members.Users.Domain;
using wojilu.Common.MemberApp;
using wojilu.Common.AppInstall;
using wojilu.Common.MemberApp.Interface;

namespace wojilu.Members.Users.Domain {

    public class UserAppAdmin {
    }

    [Serializable]
    public class UserApp : ObjectBase<UserApp>, IMemberApp {

        public int OwnerId { get; set; }
        public String OwnerUrl { get; set; }

        public int AppInfoId { get; set; }
        public int AppOid { get; set; }
        public User Creator { get; set; }
        public String CreatorUrl { get; set; }

        public String Name { get; set; }
        public int OrderId { get; set; }
        public int IsStop { get; set; }
        public int AccessStatus { get; set; }
        public DateTime Created { get; set; }

        [NotSave]
        public String Status {
            get {
                if (IsStop == 1) {
                    return lang.get( "stopped" );
                }
                return "";
            }
        }

        [NotSave]
        public AppInstaller AppInfo {
            get { return new AppInstallerService().GetById( AppInfoId ); }
        }

        [NotSave]
        public String OwnerType {
            get { return typeof( User ).FullName; }
            set { }
        }

        public void updateOrderId() {
            db.update( this, "OrderId" );
        }

    }
}

