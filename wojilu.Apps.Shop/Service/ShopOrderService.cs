using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.AppBase;
using wojilu.IO;
using wojilu.Drawing;
using wojilu.Web.Utils;

using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;
using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Domain;
using wojilu.Apps.Shop.Enum;
using wojilu.Web;

namespace wojilu.Apps.Shop.Service
{
    public class ShopOrderService:IShopOrderService
    {
        public virtual ShopOrder GetById(int id)
        {
            ShopOrder post = db.findById<ShopOrder>(id);
            if (post.SaveStatus != SaveStatus.Normal) return null;
            return post;
        }

        public virtual ShopOrder GetById(int id, int ownerId)
        {
            ShopOrder post = GetById(id);
            if (post == null) return null;
            if (post.OwnerId != ownerId) return null;
            if (post.SaveStatus != SaveStatus.Normal) return null;
            return post;
        }

        public virtual ShopOrder GetByOrder(string orderNum)
        {

            ShopOrder gorder = ShopOrder.find("OrderNum='" + orderNum + "'").first();
            if (gorder != null)
            {
                if (gorder.SaveStatus != SaveStatus.Normal) return null;
                return gorder;
            }
            else return null;
        }

        public virtual ShopOrder GetByOrder(string orderNum, int ownerId)
        {
            ShopOrder post = GetByOrder(orderNum);
            if (post == null) return null;
            if (post.OwnerId != ownerId) return null;
            if (post.SaveStatus != SaveStatus.Normal) return null;
            return post;
        }

        public virtual DataPage<ShopOrder> FindOrder(int appId, int pageSize)
        {
            return ShopOrder.findPage("AppId=" + appId, pageSize);
        }

        public virtual DataPage<ShopOrder> FindOrder(int appId, int userid, int pageSize)
        {
            return ShopOrder.findPage("AppId=" + appId + " and CreatorId=" + userid + "", pageSize);
        }

        public virtual DataPage<ShopOrder> FindOrder(int appId, string key, int pageSize)
        {
            return ShopOrder.findPage("AppId=" + appId + " and Title like '%" + key + "%'",pageSize);
        }

        public virtual DataPage<ShopOrder> FindOrder(int appId, OrderStatus orderstatus, OrderActStatus actStatus, PaymentStatus paymentstatus)
        {
            return
                ShopOrder.findPage("AppId=" + appId + " and orderstatus=" + (int)orderstatus +
                                   (actStatus != OrderActStatus.All ? " and Activitystatus=" + (int)actStatus : "") +
                                   (paymentstatus != PaymentStatus.All
                                        ? " and Paymentstatus=" + (int)paymentstatus
                                        : ""));
        }
        public virtual DataPage<ShopOrder> FindOrder(int appId, OrderFilterMethod method, int pageSize)
        {
            switch (method) {
                case OrderFilterMethod.All:
                    return
                        ShopOrder.findPage("AppId=" + appId, pageSize);
                    break;
                case OrderFilterMethod.UnPay:
                    return
                        ShopOrder.findPage("AppId=" + appId + " and Paymentstatus=" + (int)PaymentStatus.NotYet, pageSize);
                    break;
                case OrderFilterMethod.Paid:
                    return
                        ShopOrder.findPage("AppId=" + appId + " and Paymentstatus=" + (int)PaymentStatus.Prepaid, pageSize);
                    break;
                case OrderFilterMethod.UnDeliver:
                    return
                        ShopOrder.findPage("AppId=" + appId + " and orderstatus<" + (int)OrderStatus.WAIT_BUYER_CONFIRM_GOODS, pageSize);
                    break;
                case OrderFilterMethod.Delivered:
                    return
                        ShopOrder.findPage("AppId=" + appId + " and orderstatus>=" + (int)OrderStatus.WAIT_BUYER_CONFIRM_GOODS, pageSize);
                    break;
                case OrderFilterMethod.UnDone:
                    return
                        ShopOrder.findPage("AppId=" + appId + " and Activitystatus<" + (int)OrderActStatus.Successed, pageSize);
                    break;
                case OrderFilterMethod.Done:
                    return
                        ShopOrder.findPage("AppId=" + appId + " and Activitystatus=" + (int)OrderActStatus.Successed, pageSize);
                    break;
            }
            return null;
        }
        // TODO: 1,购特车加入订单流程函数 2,订单流程函数

        public virtual DataPage<ShopOrderItem> FindOrderItemByApp(int appId)
        {
            return ShopOrderItem.findPage("AppId=" + appId);
        }

        public virtual DataPage<ShopOrderItem> FindOrderItemByOrder(int appId, int orderid)
        {
            return ShopOrderItem.findPage("AppId=" + appId + " and orderid=" + orderid + "");
        }

        public virtual DataPage<ShopOrderItem> FindOrderItemByItem(int appId, int itemid)
        {
            return ShopOrderItem.findPage("AppId=" + appId + " and itemid=" + itemid + "");
        }

        public virtual void DeleteItemByOrder(int orderid)
        {
            ShopOrderItem.deleteBatch("orderid=" + orderid);
        }

        public virtual void DeleteItemByItem(int itemid)
        {
            ShopOrderItem.deleteBatch("itemid=" + itemid);
        }

        public virtual void Delete(ShopOrder order)
        {
            order.SaveStatus = SaveStatus.Delete;
            order.update();
        }

        public virtual void Restore(int id)
        {
            ShopOrder order = ShopOrder.findById(id);
            if (order == null) return;
            order.SaveStatus = SaveStatus.Normal;
            order.update();
        }
        public virtual void DeleteTrue(int id)
        {
            ShopOrder order = ShopOrder.findById(id);
            if (order == null) return;
            order.SaveStatus = SaveStatus.SysDelete;
            order.update();
        }

        public virtual void DeleteBatch(string ids)
        {
            ShopOrderItem.deleteBatch("orderid in (" + ids + ")");
            ShopOrder.deleteBatch("Id in (" + ids + ")");
        }

        public virtual void InsertOrderItemBatch(int orderid,List<ShoppingCart> carts) { 
            foreach(ShoppingCart cart in carts){
                ShopOrderItem item = new ShopOrderItem();
                item.OrderId = orderid;
                item.Item = cart.Item;
                item.BuyQty = cart.BuyQty;
                item.CostPrice = cart.Item.CostPrice;
                item.SalePrice = cart.SalePrice;
                item.Points = cvt.ToInt(cart.SalePrice);
                item.ItemWeight = cart.ItemWeight;
                item.ItemAttribute = cart.ItemAttribute;
                item.Created = DateTime.Now;
                item.Updated = DateTime.Now;
                item.insert();
            }
        }
        public virtual void UpdateOrderItemBatch(List<ShopOrderItem> items) {
            foreach (ShopOrderItem item in items)
            {
                if (item.Id > 0)
                    item.update();
                else
                    item.insert();
            }
        }
    }
}
