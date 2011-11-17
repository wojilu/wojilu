/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.Data;
using wojilu.ORM;
using wojilu.Members.Users.Domain;
using wojilu.Common.Security;

namespace wojilu.Members.Sites.Domain {


    [Serializable]
    public class SiteRole : CacheObject, IRole {

        public SiteRole() { }

        public SiteRole( int id, String name ) {
            this.Id = id;
            this.Name = name;
        }

        private int _groupId;        

        public int GroupId {
            get { return _groupId; }
            set { _groupId = value; }
        }


        [NotSave, NotSerialize]
        public IRole Role {
            get { return this; }
            set { }
        }

        public static SiteRole Administrator = getAdmin();
        public static SiteRole NormalMember = getNormalMember();
        public static SiteRole Guest = getGuest();
        public static SiteRole DeletedUser = getDeletedUser();


        private static SiteRole getAdmin() {
            return cdb.findById<SiteRole>( 1 );
        }

        private static SiteRole getNormalMember() {
            return cdb.findById<SiteRole>( 2 );
        }

        private static SiteRole getGuest() {
            return cdb.findById<SiteRole>( 0 );
        }

        private static SiteRole getDeletedUser() {
            return new SiteRole( DeletedUserId, lang.get( "deletedUserName" ) );
        }

        public static Boolean IsInAdminGroup( int roleId ) {
            SiteRole role = cdb.findById<SiteRole>( roleId );
            if (role == null) return false;

            if (role.GroupId == RoleGroup.Admin) return true;
            return false;
        }

        public static Boolean IsAdministrator( IRole role ) {
            if (role.Role.GetType() == typeof( SiteRole ) && role.Role.Id == Administrator.Id) return true;
            return false;
        }

        public static Boolean IsNormalMember( IRole role ) {
            if (role.Role.GetType() == typeof( SiteRole ) && role.Role.Id == NormalMember.Id) return true;
            return false;
        }

        public static SiteRole GetById( int userId, int roleId ) {

            if (userId == GuestId) return Guest;
            if (userId == DeletedUserId) return DeletedUser;

            SiteRole role = cdb.findById<SiteRole>( roleId );
            if (role == null) return NormalMember;

            return role;
        }

        public static SiteRole GetById( int roleId ) {
            if (roleId == DeletedUserId) return DeletedUser;
            return cdb.findById<SiteRole>( roleId );
        }

        public static int GuestId = 0;
        public static int DeletedUserId = -1;



    }
}

