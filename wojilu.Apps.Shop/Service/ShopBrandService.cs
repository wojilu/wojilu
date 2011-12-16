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
    public class ShopBrandService:IShopBrandService
    {
        public virtual DataPage<ShopBrand> GetPage(int appId)
        {
            return ShopBrand.findPage("AppId=" + appId);
        }

        public virtual DataPage<ShopBrand> GetPage(int appId, String key)
        {
            if (strUtil.IsNullOrEmpty(key))
                return ShopBrand.findPage("AppId=" + appId);
            else
                return ShopBrand.findPage("AppId=" + appId + " and Title like '%" + key + "%'");
        }

        public virtual void AddItemCount(ShopBrand f)
        {
            f.GoodCounts += 1;
            f.update("GoodCounts");
        }

        public virtual void DeleteBrand(ShopBrand f)
        {
            //int brandId = f.Id;
            f.delete();
            //ShopItem.deleteBatch("BrandId=" + brandId);

        }
    }
}
