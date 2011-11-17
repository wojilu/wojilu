/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.ORM;
using wojilu.Common.Security;

namespace wojilu.Members.Groups.Domain {

    [Serializable]
    public class GroupRole : IRole {

        public GroupRole( int id, String name ) {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public String Name { get; set; }

        [NotSave, NotSerialize]
        public IRole Role {
            get { return this; }
            set { }
        }

        private static IList _all = getall();

        private static IList getall() {
            IList results = new ArrayList();
            results.Add( new GroupRole( 0, lang.get( "groupMember" ) ) );
            results.Add( new GroupRole( 1, lang.get( "groupToApprove" ) ) );
            results.Add( new GroupRole( 2, lang.get( "groupAdmin" ) ) );
            return results;
        }

        public static IList GetAll() {
            return _all;
        }        

        public static GroupRole GetById( int id ) {
            return GetAll()[id] as GroupRole;
        }

        public static readonly GroupRole Member = GetById(0);
        public static readonly GroupRole Approving = GetById(1);
        public static readonly GroupRole Administrator = GetById(2);
        public static readonly GroupRole Blacklist = getBlackList();

        private static GroupRole getBlackList() {
            return new GroupRole( 3, lang.get( "blacklist" ) );
        }

        public static readonly int NotFound = -1;


        // 用户加入群组之后的初始角色
        public static int GetInitRoleByGroup( Group g ) {

            if (g.AccessStatus == GroupAccessStatus.Open) return GroupRole.Member.Id;
            if (g.AccessStatus == GroupAccessStatus.Closed) return GroupRole.Approving.Id;
            if (g.AccessStatus == GroupAccessStatus.Secret) return GroupRole.Approving.Id;

            return GroupRole.Member.Id;
        }

    }


}

