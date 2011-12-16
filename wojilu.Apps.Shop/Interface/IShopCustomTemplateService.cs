using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Shop.Domain;

namespace wojilu.Apps.Shop.Interface {

    public interface IShopCustomTemplateService {
        ShopCustomTemplate GetById( int id, int ownerId );
        void Insert( ShopCustomTemplate ct );
        void Update( ShopCustomTemplate ct );
    }


}
