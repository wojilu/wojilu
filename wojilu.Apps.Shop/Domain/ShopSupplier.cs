/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Common.AppBase;
using wojilu.Drawing;
using wojilu.ORM;
using wojilu.Members.Users.Domain;
using wojilu.Common.Tags;
using wojilu.Common.Jobs;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.Feeds.Interface;
using wojilu.Common;

namespace wojilu.Apps.Shop.Domain {

    [Serializable]
    public class ShopSupplier : ObjectBase<ShopSupplier>, IAppData, ISort
    {
        public int OwnerId { get; set; }
        public String OwnerType { get; set; }
        [Column(Length = 50)]
        public String OwnerUrl { get; set; }
        public int AppId { get; set; }
        public int AccessStatus { get; set; }
        public User Creator { get; set; }
        [Column(Length = 50)]
        public String CreatorUrl { get; set; }
        [Column(Length = 40)]
        public String Ip { get; set; }
        public String Title { get; set; }
        public String Contact { get; set; }
        public String Mobile { get; set; }
        public String Telphone { get; set; }
        public String Faxphone { get; set; }
        public String Address { get; set; }
        public String Postcode { get; set; }
        [LongText, HtmlText]
        public String Description { get; set; }

        public int GoodCounts { get; set; }//产品数量
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; } // 更新日期
        public int OrderId { get; set; }


        public void updateOrderId()
        {
            this.update();
        }
    }
}

