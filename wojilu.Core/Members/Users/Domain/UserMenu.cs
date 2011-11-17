/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.ORM;
using wojilu.Members.Users.Domain;
using wojilu.Common.Menus.Interface;


namespace wojilu.Members.Users.Domain {

    public class UserMenuAdmin {
    }

    [Serializable]
    public class UserMenu : ObjectBase<UserMenu>, IMenu {
                
        public String Name { get; set; }

        public int OrderId { get; set; }

        public User Creator { get; set; }
        public int OwnerId { get; set; }
        public String OwnerUrl { get; set; }

        [NotSave]
        public String OwnerType {
            get { return typeof( User ).FullName; }
            set { }
        }

        public String Style { get; set; }
        public int ParentId { get; set; }
        public String RawUrl { get; set; }
        public String Url { get; set; }
        public DateTime Created { get; set; }
        public int OpenNewWindow { get; set; }

        public void updateOrderId() {
            db.update( this, "OrderId" );
        }


    }
}

