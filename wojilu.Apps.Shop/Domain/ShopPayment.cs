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
    public class ShopPayment : ObjectBase<ShopPayment>, ISort
    {
        public String Title { get; set; }
        public String MerchantCode { get; set; }
        public String PaymentAccount { get; set; }
        public String SecretKey { get; set; }
        public String SecondKey { get; set; }
        public String PayPassword { get; set; }
        public String Partner { get; set; }
        public String Gateway { get; set; }
        [Decimal(Scale = 2,Precision = 4)]
        public Decimal PaymentCharge { get; set; }
        public String Logo { get; set; }
        [LongText, HtmlText]
        public String Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; } // 更新日期
        public int OrderId { get; set; }
        public int GatewayId { get; set; }
        [NotSave]
        public ShopPaymentGateway PaymentGateway
        {
            get { return GatewayId > 0 ? cdb.findById<ShopPaymentGateway>(this.GatewayId) : null; }
        }
        public String SupportedCurrency { get; set; }

        [NotSave]
        public List<ShopCurrency> CurrencyList
        {
            get
            {
                var list = new List<ShopCurrency>();
                if (!string.IsNullOrEmpty(SupportedCurrency))
                {
                    var arrayIds = SupportedCurrency.Split(',');
                    foreach (var arrayId in arrayIds)
                    {
                        list.Add(cdb.findById<ShopCurrency>(int.Parse(arrayId)));
                    }
                    return list;
                }
                else return null;                
            }
        }

        public void updateOrderId()
        {
            this.update();
        }

    }
    /// <summary>
    /// 支付网关配置
    /// </summary>
    public class ShopPaymentGateway : CacheObject, ISort, IComparable
    {
        public int OrderId { get; set; }
        public String DisplayName { get; set; }
        public String Description { get; set; }
        public String Url { get; set; }
        public String Logo { get; set; }
        public int EmailAddress { get; set; }
        public int SellerAccount { get; set; }
        public int PrimaryKey { get; set; }
        public int SecondKey { get; set; }
        public int Password { get; set; }
        public int Partner { get; set; }
        public String RequestType { get; set; }
        public String NotifyType { get; set; }
        public String SupportedCurrency { get; set; }
        public int Enabled { get; set; }
        // name = 支付网关名称，此名称必需唯一
        // description = 支付网关的相关说明
        // url = 支付网关网站
        // logo = 支付网关的logo
        // emailAddress = 是否需要商户电子邮件地址
        // sellerAccount = 是否需要商户号
        // primaryKey = 是否需要第一密钥
        // secondkey = 是否需要第二密钥
        // password = 是否需要商户密码
        // partner = 是否需要填写合作伙伴商户号
        // requestType = 付款请求提供者类的类型
        // notifyType = 网关通知提供者类的类型
        // supportedCurrency = 支付网关所接受的货币类型，使用货币代码，用逗号分开，如"CNY,USD"

        [NotSerialize]
        public String ThumbIcon
        {
            get { return sys.Path.GetPhotoThumb(this.Logo); }
        }

        public void updateOrderId()
        {
            this.update();
        }

        public int CompareTo(object obj)
        {
            ShopPaymentGateway t = obj as ShopPaymentGateway;
            if (this.OrderId > t.OrderId) return -1;
            if (this.OrderId < t.OrderId) return 1;
            if (this.Id > t.Id) return 1;
            if (this.Id < t.Id) return -1;
            return 0;
        }

        //-----------------------------------------------------------------------------------

    }
    /// <summary>
    /// 货币与汇率设置
    /// </summary>
    public class ShopCurrency : CacheObject, ISort, IComparable
    {
        public int OrderId { get; set; }
        public String Code { get; set; }
        public String Symbol { get; set; }
        public Decimal Exchange { get; set; }
        public int Enabled { get; set; }
        public int IsDefault { get; set; }
        public String PreviewPic { get; set; }

        [NotSerialize]
        public String ThumbIcon
        {
            get { return sys.Path.GetPhotoThumb(this.PreviewPic); }
        }

        public void updateOrderId()
        {
            this.update();
        }

        public int CompareTo(object obj)
        {
            ShopCurrency t = obj as ShopCurrency;
            if (this.OrderId > t.OrderId) return -1;
            if (this.OrderId < t.OrderId) return 1;
            if (this.Id > t.Id) return 1;
            if (this.Id < t.Id) return -1;
            return 0;
        }

        //-----------------------------------------------------------------------------------

    }
}
