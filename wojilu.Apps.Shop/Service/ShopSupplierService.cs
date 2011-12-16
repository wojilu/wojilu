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
    public class ShopSupplierService:IShopSupplierService
    {
        public virtual DataPage<ShopSupplier> GetPage(int appId)
        {
            return ShopSupplier.findPage("AppId=" + appId);
        }

        public virtual DataPage<ShopSupplier> GetPage(int appId, String key)
        {
            if (strUtil.IsNullOrEmpty(key))
                return ShopSupplier.findPage("AppId=" + appId);
            else
                return ShopSupplier.findPage("AppId=" + appId + " and Title like '%" + key + "%'");
        }

        public virtual void AddItemCount(ShopSupplier f)
        {
            f.GoodCounts += 1;
            f.update("GoodCounts");
        }

        public virtual void DeleteSupplier(ShopSupplier f)
        {
            //int providerId = f.Id;
            f.delete();
            //ShopItem.deleteBatch("ProviderId=" + providerId);

        }
    }
}
