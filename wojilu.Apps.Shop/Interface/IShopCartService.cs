using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Shop.Domain;

namespace wojilu.Apps.Shop.Interface
{
    public interface IShopCartService
    {
        void Add(ShoppingCart cart);
        //string CheckItem(ShoppingCart cart);
        void Clear(int userid);
        ShoppingCart Get(int skuid, int userid);
        List<ShoppingCart> List(int userid);
        int Count(int userid);
        void Remove(int skuid, int userid);
        void Update(int skuid, int qty, int userid);
        void Update(ShoppingCart cart);

        //DataPage<ShoppingCart> GetPage(int appId);
        //DataPage<ShoppingCart> GetPage(int appId, int itemid);
        //DataPage<ShoppingCart> GetUserPage(int appId, int userid);
        //void DeleteItem(int itemid);
    }
}
