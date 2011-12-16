using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Shop.Domain;

namespace wojilu.Apps.Shop.Interface
{
    public interface IShopDeliverService
    {
        DataPage<ShopDeliverContacts> FindContactPage(int userid);
        DataPage<ShopDeliverContacts> FindContactPage(int userid, String key);
        List<ShopDeliverContacts> FindContact(int userid);
        List<ShopDeliverContacts> FindContact(int userid, String key);
        ShopDeliverContacts LoadContact(int attid);
        ShopDeliverContacts LoadContactDef(int userid);
        ShopDeliverContacts LoadContact(int userid, string name);
        void UpdateContact(ShopDeliverContacts att);
        void InsertContact(ShopDeliverContacts att);

        DataPage<ShopDeliver> FindDeliverPage();
        DataPage<ShopDeliver> FindDeliverPage(String key);
        List<ShopDeliver> ListDeliver();
        List<ShopDeliver> ListDeliver(String key);
        void DeleteDeliver(ShopDeliver f);
        DataPage<ShopDeliverProvider> FindProviderPage();
        DataPage<ShopDeliverProvider> FindProviderPage(String key);
        void DeleteProvider(ShopDeliverProvider f);
    }
}
