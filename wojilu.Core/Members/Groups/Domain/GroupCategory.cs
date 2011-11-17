/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Data;
using wojilu.Common.Categories;

namespace wojilu.Members.Groups.Domain {


    [Serializable]
    public class GroupCategory : CategoryBase {

        public GroupCategory() {
        }

        public GroupCategory( int id ) {
            this.Id = id;
        }

        public GroupCategory( int id, int parentId, String name ) {
            this.Id = id;
            this.ParentId = parentId;
            this.Name = name;
        }

        public GroupCategory( String name ) {
            this.Name = name;
        }

        public static List<GroupCategory> GetAll() {
            return db.find<GroupCategory>( "order by OrderId desc,id asc" ).list();
        }

    }
}

