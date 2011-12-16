using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Shop.Domain;

namespace wojilu.Apps.Shop.Interface
{
    public interface IShopPaymentService
    {
        DataPage<ShopPayment> GetPage();
        DataPage<ShopPayment> GetPage(String key);
        List<ShopPayment> List();
        List<ShopPayment> List(String key);
        void Delete(ShopPayment f);
        ShopPaymentGateway FindGatewayById(int id);
        List<ShopPaymentGateway> FindGatewayAll();
        string FindGatewayName(int id);
        ShopCurrency FindCurrencyById(int id);
        List<ShopCurrency> FindCurrencyAll();
        List<ShopCurrency> FindCurrencyList(string arrayString);
        string FindCurrencyName(int id);
    }
}
