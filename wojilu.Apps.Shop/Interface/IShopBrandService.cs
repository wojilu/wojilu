using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Shop.Domain;

namespace wojilu.Apps.Shop.Interface
{
    public interface IShopBrandService
    {
        DataPage<ShopBrand> GetPage(int appId);
        DataPage<ShopBrand> GetPage(int appId, String key);
        void AddItemCount(ShopBrand f);
        void DeleteBrand(ShopBrand f);
    }
}
