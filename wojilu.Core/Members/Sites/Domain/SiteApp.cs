/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Data;
using wojilu.ORM;
using wojilu.Members.Users.Domain;
using wojilu.Members.Sites.Domain;
using wojilu.Common.MemberApp;
using wojilu.Common.AppInstall;
using wojilu.Common.MemberApp.Interface;

namespace wojilu.Members.Sites.Domain {


    [Serializable]
    public class SiteApp : CacheObject, IMemberApp, IComparable {


        public int CreatorId { get; set; }

        [NotSave]
        public User Creator {
            get { return User.findById( this.CreatorId ); }
            set {
                if (value == null) return;
                this.CreatorId = value.Id;
            }
        }

        public String CreatorUrl { get; set; }
        public int AppInfoId { get; set; }
        public int AppOid { get; set; }
        public int IsStop { get; set; }
        public int AccessStatus { get; set; }
        public DateTime Created { get; set; }
        public int OrderId { get; set; }

        [NotSave]
        public int OwnerId {
            get { return Site.Instance.Id; }
            set { }
        }

        [NotSave]
        public String OwnerType {
            get { return typeof( Site ).FullName; }
            set { }
        }

        [NotSave]
        public String OwnerUrl {
            get { return Site.Instance.Url; }
            set { }
        }

        [NotSave]
        public AppInstaller AppInfo {
            get { return new AppInstallerService().GetById( AppInfoId ); }
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

        public int CompareTo( object obj ) {
            SiteApp app = obj as SiteApp;
            if (this.OrderId > app.OrderId) {
                return -1;
            }
            if (this.OrderId < app.OrderId) {
                return 1;
            }
            if (this.Id > app.Id) {
                return 1;
            }
            if (this.Id < app.Id) {
                return -1;
            }
            return 0;
        }

        public void updateOrderId() {
            this.update();
        }

    }
}

