using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.AppBase.Interface;
using wojilu.Apps.Shop.Domain;
using wojilu.Apps.Shop.Interface;

namespace wojilu.Apps.Shop.Service {


    public class ShopCustomTemplateService : IShopCustomTemplateService
    {



        public ShopCustomTemplate GetById(int id, int ownerId)
        {

            ShopCustomTemplate ct = ShopCustomTemplate.findById( id );
            if (ct == null) return null;
            if (ct.OwnerId != ownerId) return null;
            return ct;
        }


        public void Insert(ShopCustomTemplate ct)
        {
            ct.insert();
        }

        public void Update(ShopCustomTemplate ct)
        {
            ct.update();
        }
    }

}
