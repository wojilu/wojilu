using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Shop.Domain;

namespace wojilu.Apps.Shop.Interface
{
    public interface IShopSupplierService
    {
        DataPage<ShopSupplier> GetPage(int appId);
        DataPage<ShopSupplier> GetPage(int appId, String key);
        void AddItemCount(ShopSupplier f);
        void DeleteSupplier(ShopSupplier f);
    }
}
