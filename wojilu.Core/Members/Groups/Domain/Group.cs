/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.ORM;
using wojilu.Members.Users.Domain;
using wojilu.Members.Groups.Service;
using wojilu.Members.Interface;
using wojilu.Common.Security;
using wojilu.Common;

namespace wojilu.Members.Groups.Domain {


    [Serializable]
    [Table( "Groups" )]
    public class Group : ObjectBase<Group>, IMember, IHits {

        public GroupCategory Category { get; set; }

        public User Creator { get; set; }
        public int TemplateId { get; set; }

        public String Name { get; set; }
        public String Url { get; set; }
        public String Subject { get; set; }
        public String Logo { get; set; }

        [LongText]
        public String Notice { get; set; }

        [LongText]
        public String Description { get; set; }

        public int MemberCount { get; set; }
        public int FriendCount { get; set; }
        public int PostCount { get; set; }
        public int TopicCount { get; set; }

        public int Hits { get; set; }

        public int Status { get; set; }
        public int AccessStatus { get; set; } // GroupAccessStatus

        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }

        public int IsLock { get; set; }
        public int IsSystemHide { get; set; }

        public int IsCloseJoinCmd { get; set; } // ÊÇ·ñÒþ²ØÉêÇë¼ÓÈëÃüÁî¡£

        [LongText]
        public String Security { get; set; }

        //-----------------------------------------------------------------------------------

        [NotSave]
        public String LogoFull {
            get { return sys.Path.GetGroupLogoOriginal( this.Logo ); }
        }

        [NotSave]
        public String LogoSmall {
            get { return sys.Path.GetGroupLogoThumb( this.Logo ); }
        }

        public Boolean IsSecret() {
            return this.AccessStatus == GroupAccessStatus.Secret || this.IsSystemHide == 1;
        }

        public IList GetRoles() {
            return GroupRole.GetAll();
        }

        public IRole GetAdminRole() {
            return GroupRole.Administrator;
        }

        public IRole GetUserRole( IMember user ) {
            return new MemberGroupService().GetUserRole( (User)user, this.Id );
        }

        public String GetAccessString() {
            String result = "";
            if (this.AccessStatus > 0)
                result = GroupAccessStatus.GetById( this.AccessStatus ).Name;
            if (this.IsLock == 1) result += " (" + lang.get( "locked" ) + ")";
            if (this.IsSystemHide == 1) result += " (" + lang.get( "sysHidden" ) + ")";
            return result;
        }

        public String GetUrl() {
            return "group";
        }

    }
}

