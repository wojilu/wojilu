using System;
using System.Collections.Generic;
using System.Text;

using wojilu.IO;
using wojilu.Drawing;
using wojilu.Web.Utils;

using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;
using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Domain;
using wojilu.Web;

namespace wojilu.Apps.Shop.Service
{
    public class ShopPaymentService:IShopPaymentService
    {
        public virtual DataPage<ShopPayment> GetPage()
        {
            return ShopPayment.findPage(string.Empty);
        }

        public virtual DataPage<ShopPayment> GetPage(String key)
        {
            if (strUtil.IsNullOrEmpty(key))
                return ShopPayment.findPage(string.Empty);
            else
                return ShopPayment.findPage("Title like '%" + key + "%'");
        }
        public virtual List<ShopPayment> List()
        {
            return ShopPayment.find(string.Empty).list();
        }

        public virtual List<ShopPayment> List(String key)
        {
            if (strUtil.IsNullOrEmpty(key))
                return ShopPayment.find(string.Empty).list();
            else
                return ShopPayment.find("Title like '%" + key + "%'").list();
        }

        public virtual void Delete(ShopPayment f)
        {
            //int providerId = f.Id;
            f.delete();
            //ShopItem.deleteBatch("ProviderId=" + providerId);

        }
        public virtual ShopPaymentGateway FindGatewayById(int id)
        {
            return cdb.findById<ShopPaymentGateway>(id);
        }

        public virtual List<ShopPaymentGateway> FindGatewayAll()
        {
            List<ShopPaymentGateway> list = cdb.findAll<ShopPaymentGateway>();
            list.Sort();
            return list;
        }

        public virtual string FindGatewayName(int id)
        {
            return cdb.findById<ShopPaymentGateway>(id).Name;
        }

        public virtual ShopCurrency FindCurrencyById(int id)
        {
            return cdb.findById<ShopCurrency>(id);
        }

        public virtual List<ShopCurrency> FindCurrencyAll()
        {
            List<ShopCurrency> list = cdb.findAll<ShopCurrency>();
            list.Sort();
            return list;
        }
        public virtual List<ShopCurrency> FindCurrencyList(string arrayString)
        {
            var list = new List<ShopCurrency>();
            if (!string.IsNullOrEmpty(arrayString))
            {
                var arrayIds = arrayString.Split(',');
                foreach (var arrayId in arrayIds)
                {
                    list.Add(FindCurrencyById(int.Parse(arrayId)));
                }
                return list;
            }
            else return null;
        }

        public virtual string FindCurrencyName(int id)
        {
            return cdb.findById<ShopCurrency>(id).Name;
        }
    }
}
