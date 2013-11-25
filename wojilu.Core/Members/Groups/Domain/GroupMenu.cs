/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.ORM;
using wojilu.Members.Users.Domain;
using wojilu.Members.Groups.Domain;
using wojilu.Common.Menus.Interface;


namespace wojilu.Members.Groups.Domain {


    [Serializable]
    public class GroupMenu : ObjectBase<GroupMenu>, IMenu {

        public User Creator { get; set; }

        public String Name { get; set; }

        public int OrderId { get; set; }

        public long OwnerId { get; set; }
        public String OwnerUrl { get; set; }

        [NotSave]
        public String OwnerType {
            get { return typeof( Group ).FullName; }
            set { }
        }

        public long ParentId { get; set; }

        public String RawUrl { get; set; }

        public String Url { get; set; }

        public DateTime Created { get; set; }

        public String Style { get; set; }
        public int OpenNewWindow { get; set; }

        public void updateOrderId() {
            db.update( this, "OrderId" );
        }

    }
}

