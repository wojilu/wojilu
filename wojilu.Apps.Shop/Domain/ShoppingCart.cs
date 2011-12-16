using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;
using wojilu.Common.AppBase.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Common.Tags;
using System.IO;
using wojilu.Data;
using wojilu.Common.AppBase;

namespace wojilu.Apps.Shop.Domain
{

    public class ShoppingCart : ObjectBase<ShoppingCart>
    {
        [NotSerialize]
        public User Creator { get; set; }

        [Column(Length = 40)]
        public String Ip { get; set; }

        public String Title { get; set; }

        public ShopItem Item { get; set; }

        public int BuyQty { get; set; }

        public int ItemWeight { get; set; }

        [Money]
        public Decimal SalePrice { get; set; }

        [NotSave]
        public Decimal TotalSaleAmount
        {
            get { return SalePrice * BuyQty; }
        }

        [NotSave]
        public int TotalWeight
        {
            get { return ItemWeight * BuyQty; }
        }

        public String ItemAttribute { get; set; }

        public int Points { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; } // 更新日期

    }
}
