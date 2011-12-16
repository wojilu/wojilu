using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Shop.Domain;
using wojilu.Apps.Shop.Enum;

namespace wojilu.Apps.Shop.Interface
{
    public interface IShopOrderService
    {
        ShopOrder GetById(int id);
        ShopOrder GetById(int id, int ownerId);
        ShopOrder GetByOrder(string orderNum);
        ShopOrder GetByOrder(string orderNum, int ownerId);

        DataPage<ShopOrder> FindOrder(int appId, int pageSize);
        DataPage<ShopOrder> FindOrder(int appId, int userid,int pageSize);
        DataPage<ShopOrder> FindOrder(int appId, string key, int pageSize);
        DataPage<ShopOrder> FindOrder(int appId, OrderStatus orderstatus, OrderActStatus actStatus,
                                      PaymentStatus paymentstatus);
        DataPage<ShopOrder> FindOrder(int appId, OrderFilterMethod method, int pageSize);

        DataPage<ShopOrderItem> FindOrderItemByApp(int appId);
        DataPage<ShopOrderItem> FindOrderItemByOrder(int appId, int orderid);
        DataPage<ShopOrderItem> FindOrderItemByItem(int appId, int itemid);
        void DeleteItemByOrder(int orderid);
        void DeleteItemByItem(int itemid);

        void Delete(ShopOrder order);
        void Restore(int id);
        void DeleteTrue(int id);
        void DeleteBatch(string ids);
        void InsertOrderItemBatch(int orderid, List<ShoppingCart> carts);
        void UpdateOrderItemBatch(List<ShopOrderItem> items);
    }
}
