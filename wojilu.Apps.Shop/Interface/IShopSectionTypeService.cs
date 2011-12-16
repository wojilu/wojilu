/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System.Collections.Generic;
using wojilu.Apps.Shop.Domain;

namespace wojilu.Apps.Shop.Interface {

    public interface IShopSectionTypeService {

        DataPage<ShopSectionType> GetPage();
        List<ShopSectionType> GetAll();
    }

}

