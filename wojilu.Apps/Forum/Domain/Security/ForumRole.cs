/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.ORM;
using wojilu.Members.Sites.Service;
using wojilu.Common.Security;

namespace wojilu.Apps.Forum.Domain {

    [Serializable]
    public class ForumRole : ObjectBase<ForumRole>, IRole {

        public int ForumId { get; set; }
        public String Name { get; set; }
        public String Title { get; set; }

        public ForumRole() { }

        public ForumRole( int id, String name ) {
            Id = id;
            Name = name;
        }

        [NotSave, NotSerialize]
        public IRole Role {
            get { return this; }
            set { }
        }

        [NotSave]
        public Boolean IsAdmin { get; set; }

        public static ForumRole Moderator = GetById( 1 );


        public static List<ForumRole> GetAll() {
            return db.findAll<ForumRole>();
        }

        public static List<IRole> GetAllWithSystem() {
            List<ForumRole> forumRoles = GetAll();
            return new SiteRoleService().GetRoleAndRank( forumRoles );
        }

        public static ForumRole GetById( int roleId ) {
            return db.findById<ForumRole>( roleId );
        }

    }
}



