using System;
using System.Collections.Generic;
using wojilu.Apps.Shop.Enum;
using wojilu.ORM;
using wojilu.Common.AppBase.Interface;
using wojilu.Members.Users.Domain;

namespace wojilu.Apps.Shop.Domain
{
    public class ShopOrder:ObjectBase<ShopOrder>,IAppData,IComparable
    {
        public int OwnerId { get; set; }
        public String OwnerType { get; set; }
        [Column(Length = 50)]
        public String OwnerUrl { get; set; }

        public int AppId { get; set; }
        public User Creator { get; set; }
        [Column(Length = 50)]
        public String CreatorUrl { get; set; }

        [TinyInt]
        public int SaveStatus { get; set; }
        public int AccessStatus { get; set; }

        [Column(Length = 40)]
        public String Ip { get; set; }

        public String Title { get; set; }

        public String OrderNum { get; set; }

        public String Description { get; set; }

        public String OrderRemark { get; set; }

        public int CurrencyId { get; set; }
        [NotSave]
        public ShopCurrency Currency
        {
            get { return CurrencyId > 0 ? cdb.findById<ShopCurrency>(this.CurrencyId) : null; }
        }

        public ShopPayment Payment { get; set; }
        public ShopDeliver Deliver { get; set; }
        public ShopDeliverContacts DeliverContact { get; set; }

        public String ShipTo { get; set; }//收货人
        public String ShipAddress { get; set; }//收货地址
        public String ShipArea { get; set; }//收货省、市
        public String ShipZip { get; set; }//收货邮编
        public String ShipEmail { get; set; }//收货人邮箱
        public String ShipBuilding { get; set; }//收货地附近建筑
        public String ShipTelPhone { get; set; }//收货人联系电话
        public String ShipMobile { get; set; }//收货人移动电话
        public String ShipTaxName { get; set; }//收货人发票台头

        [Money]
        public Decimal FreightAmount { get; set; }//总运费

        [Money]
        public Decimal TaxAmount { get; set; }//总税收

        [Money]
        public Decimal TotalAmount { get; set; }//总金额

        [NotSave]
        public Decimal TotalCostAmount
        {
            get
            {
                Decimal allcost = 0;
                foreach (var shopOrderItem in ListItem)
                {
                    allcost += shopOrderItem.CostPrice * shopOrderItem.BuyQty;
                }
                return allcost;
            }
        }//总成本
        [NotSave]
        public Decimal TotalSaleAmount
        {
            get
            {
                Decimal allcost = 0;
                foreach (var shopOrderItem in ListItem)
                {
                    allcost += shopOrderItem.SalePrice * shopOrderItem.BuyQty;
                }
                return allcost;
            }
        }//总产品售价

        [NotSave]
        public List<ShopOrderItem> ListItem
        {
            get { return Id > 0 ? ShopOrderItem.find("orderid=" + Id).list() : null; }
        }

        public int TotalPoint { get; set; }//总积分数

        public OrderStatus Orderstatus { get; set; }//交易状态

        public PaymentStatus Paymentstatus { get; set; }//付款状态

        public OrderActStatus Activitystatus { get; set; }//订单状态

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; } // 更新日期
    }


    public class ShopOrderItem : ObjectBase<ShopOrderItem>
    {
        public int OrderId { get; set; }
        //[NotSerialize]
        public ShopItem Item { get; set; }

        public int BuyQty { get; set; }

        public int ItemWeight { get; set; }

        [Money]
        public Decimal CostPrice { get; set; }

        [Money]
        public Decimal SalePrice { get; set; }

        [NotSave]
        public Decimal TotalCostAmount
        {
            get { return CostPrice*BuyQty; }
        }

        [NotSave]
        public Decimal TotalSaleAmount
        {
            get { return SalePrice*BuyQty; }
        }

        [NotSave]
        public int TotalWeight
        {
            get { return ItemWeight*BuyQty; }
        }

        public String ItemAttribute { get; set; }

        public int Points { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; } // 更新日期
    }
}
