/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.ORM;
using wojilu.Members.Users.Domain;
using wojilu.Common.AppInstall;
using wojilu.Common.MemberApp.Interface;


namespace wojilu.Members.Groups.Domain {

    [Serializable]
    public class GroupApp : ObjectBase<GroupApp>, IMemberApp {


        private int _OwnerId;
        private String _OwnerUrl;

        private int _AppInfoId;
        private int _AppOid;

        private User _Creator;
        private String _CreatorUrl;

        private String _Name;
        private int _OrderId;
        private int _IsStop;
        private int _AccessStatus;
        private DateTime _Created;

        public int OwnerId {
            get { return _OwnerId; }
            set { _OwnerId = value; }
        }

        [NotSave]
        public String OwnerType {
            get { return typeof( Group ).FullName; }
            set { }
        }

        public String OwnerUrl {
            get { return _OwnerUrl; }
            set { _OwnerUrl = value; }
        }


        public int AppInfoId {
            get { return _AppInfoId; }
            set { _AppInfoId = value; }
        }

        [NotSave]
        public AppInstaller AppInfo {
            get { return new AppInstallerService().GetById( AppInfoId ); }
        }

        public int AppOid {
            get { return _AppOid; }
            set { _AppOid = value; }
        }



        public User Creator {
            get { return _Creator; }
            set { _Creator = value; }
        }

        public String CreatorUrl {
            get { return _CreatorUrl; }
            set { _CreatorUrl = value; }
        }



        public String Name {
            get { return _Name; }
            set { _Name = value; }
        }

        public int OrderId {
            get { return _OrderId; }
            set { _OrderId = value; }
        }

        public int IsStop {
            get { return _IsStop; }
            set { _IsStop = value; }
        }



        [NotSave]
        public String Status {
            get {
                if (IsStop == 1) {
                    return lang.get( "stopped" );
                }
                return "";
            }
        }

        public int AccessStatus {
            get { return _AccessStatus; }
            set { _AccessStatus = value; }
        }

        public DateTime Created {
            get { return _Created; }
            set { _Created = value; }
        }

        public void updateOrderId() {
            db.update( this, "OrderId" );
        }

    }
}
