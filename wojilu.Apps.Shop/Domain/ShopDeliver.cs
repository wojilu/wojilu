using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Data;
using wojilu.Members.Users.Domain;
using wojilu.ORM;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;

namespace wojilu.Apps.Shop.Domain
{
    public class ShopDeliverContacts : ObjectBase<ShopDeliverContacts>, ISort
    {
        [NotSerialize]
        public User Creator { get; set; }
        public String Title { get; set; }
        public String Contact { get; set; }
        public int CountryId{get;set;}
        public int StateId{get;set;}
        public int CityId { get; set; }
        public int PlaceId { get; set; }
        public String AddressFirst { get; set; }
        public String AddressSecond { get; set; }
        public String PostCode { get; set; }
        public String Telphone { get; set; }
        public String Mobilephone { get; set; }
        public String Faxphone { get; set; }
        public String LocalBuilding { get; set; }
        public String AgreeShipTime { get; set; }
        [TinyInt]
        public int Enabled { get; set; }
        [TinyInt]
        public int IsDefault { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; } // 更新日期
        public int OrderId { get; set; }
        public void updateOrderId()
        {
            this.update();
        }

    }


    public class ShopDeliver : ObjectBase<ShopDeliver>, ISort
    {
        public String Title { get; set; }
        public int Weight { get; set; }//首重
        [Money]
        public Decimal Price { get; set; }//首价
        public int AddWeight { get; set; }//续重
        [Money]
        public Decimal AddPrice { get; set; }//续价

        public ShopDeliverProvider Provider { get; set; }
        public String SupportedPayment { get; set; }
        [NotSave]
        public List<ShopPayment> PaymentList
        {
            get
            {
                var list = new List<ShopPayment>();
                if (!string.IsNullOrEmpty(SupportedPayment))
                {
                    var arrayIds = SupportedPayment.Split(',');
                    foreach (var arrayId in arrayIds)
                    {
                        list.Add(ShopPayment.findById(int.Parse(arrayId)));
                    }
                    return list;
                }
                else return null;
            }
        }

        [LongText, HtmlText]
        public String Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; } // 更新日期
        public int OrderId { get; set; }


        public void updateOrderId()
        {
            this.update();
        }

    }


    public class ShopDeliverProvider : ObjectBase<ShopDeliverProvider>, ISort
    {
        public String Title { get; set; }
        public String Contact { get; set; }
        public String Telphone { get; set; }
        public String Faxphone { get; set; }
        public String Mobile { get; set; }
        public String Address { get; set; }
        public String Postcode { get; set; }
        [LongText, HtmlText]
        public String Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; } // 更新日期
        public int OrderId { get; set; }


        public void updateOrderId()
        {
            this.update();
        }
    }
}
