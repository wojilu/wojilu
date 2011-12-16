using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;

namespace wojilu.Apps.Shop.Domain {

    [Serializable]
    public class ShopItemSection : ObjectBase<ShopItemSection> {

        [Column( Name = "ItemId" )]
        public ShopItem Item { get; set; }

        [Column( Name = "SectionId" )]
        public ShopSection Section { get; set; }


    }

}
