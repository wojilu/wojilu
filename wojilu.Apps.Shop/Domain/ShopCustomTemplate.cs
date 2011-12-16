using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;
using wojilu.Members.Users.Domain;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Apps.Shop.Domain {

    [Serializable]
    public class ShopCustomTemplate : ObjectBase<ShopCustomTemplate> {

        public String Name { get; set; }
        public String Description { get; set; }

        public User Creator { get; set; }
        public int OwnerId { get; set; }
        public String OwnerType { get; set; }
        public String OwnerUrl { get; set; }

        [LongText]
        public String Content { get; set; }
        public DateTime Created { get; set; }


    }

}
