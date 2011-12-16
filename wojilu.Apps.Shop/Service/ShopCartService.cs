using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

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
    //TODO:注册用户数据库操作，非注册用户Cookie操作
    public class ShopCartService:IShopCartService
    {
        public virtual void Add(ShoppingCart cart) {
            int buyQty = cart.BuyQty;
            //int buyAttr=cart.buy
            if (cart.Id <= 0)
            {
                ShoppingCart ncart = Get(cart.Item.Id, cart.Creator.Id);
                if (ncart != null && ncart.Id >= 0)
                {
                    ncart.BuyQty += buyQty;
                    db.update(ncart);
                }
                else
                {
                    db.insert(cart);
                }
            }
            else db.update(cart);
        }
        public virtual void Clear(int userid) {
            ShoppingCart.deleteBatch(" CreatorId=" + userid + "");
        }
        public virtual ShoppingCart Get(int skuid,int userid)
        {
            return ShoppingCart.find("ItemId=" + skuid + " and CreatorId=" + userid + "").first();
        }
        public virtual List<ShoppingCart> List(int userid) {
            return ShoppingCart.find("CreatorId=" + userid + "").list();
        }
        public virtual int Count(int userid) {
            return ShoppingCart.count("CreatorId=" + userid + "");
        }
        public virtual void Remove(int skuid, int userid) {
            ShoppingCart.deleteBatch("ItemId="+skuid+" and CreatorId=" + userid + "");
        }
        public virtual void Update(int skuid, int qty, int userid) {
            ShoppingCart.updateBatch("set BuyQty=" + qty, "ItemId=" + skuid + " and CreatorId=" + userid + "");
        }
        public virtual void Update(ShoppingCart cart) {
            db.update(cart);
        }
    }
}
