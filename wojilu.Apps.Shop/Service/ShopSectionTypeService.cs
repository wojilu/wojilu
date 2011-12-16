/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Apps.Shop.Domain;
using wojilu.Apps.Shop.Interface;

namespace wojilu.Apps.Shop.Service {


    public class ShopSectionTypeService : IShopSectionTypeService {

        public virtual DataPage<ShopSectionType> GetPage() {
            DataPage<ShopSectionType> list = cdb.findPage<ShopSectionType>( 12 );
            list.Results.Sort( comp );
            return list;
        }

        public virtual List<ShopSectionType> GetAll() {
            List<ShopSectionType> results = cdb.findAll<ShopSectionType>();
            results.Sort( comp );
            return results;
        }

        private static int comp( ShopSectionType x, ShopSectionType y ) {
            if (x.OrderId > y.OrderId) return 1;
            if (x.OrderId < y.OrderId) return -1;
            return 0;
        }

    }
}

