using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;

namespace wojilu.Apps.Shop.Domain {

    public class ShopDigg : ObjectBase<ShopDigg> {

        public int UserId { get; set; }
        public int ItemId { get; set; }

        public DateTime Created { get; set; }
        public String Ip { get; set; }

        public int TypeId { get; set; } // 0=up, 1=down


    }

}
