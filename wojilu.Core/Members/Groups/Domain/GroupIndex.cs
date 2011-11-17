/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;


using wojilu.ORM;
using wojilu.Common.AppInstall;
using wojilu.Common.AppBase.Interface;


namespace wojilu.Members.Groups.Domain {

    [Serializable]
    public class GroupIndex : IApp {
        private int _appInfoId;

        public int AccessStatus {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        [NotSave]
        public wojilu.Common.AppInstall.AppInstaller AppInfo {
            get {
                return new AppInstallerService().GetById( this.AppInfoId );
            }
        }

        public int AppInfoId {
            get {
                return this._appInfoId;
            }
            set {
                this._appInfoId = value;
            }
        }

        public DateTime Created {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public int Id {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public int OwnerId {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public String OwnerUrl {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }



        public String OwnerType {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

    }
}

