/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Apps.Shop.Domain;

namespace wojilu.Apps.Shop.Interface {

    public interface ISysItemService {

        ShopItem GetById_ForAdmin( int id );
        DataPage<ShopItem> GetPage();
        DataPage<ShopItem> GetPageTrash();
        int GetDeleteCount();

        void Delete( String ids );
        void DeleteTrue( String ids );
        void UnDelete( ShopItem post );

    }

}
