/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Apps.Shop.Domain;

namespace wojilu.Apps.Shop.Interface {

    public interface IShopSectionService {

        ShopSection GetById( int id, int appId );

        List<ShopSection> GetByApp( int appId );
        List<ShopSection> GetInputSectionsByApp( int appId );
        List<ShopSection> GetByRowColumn( List<ShopSection> list, int rowId, int columnId );

        void Insert( ShopSection section );
        void Update( ShopSection section );
        void Delete( ShopSection section );

        int Count( int appId, int rowId );

        List<ShopSection> GetForCombine( ShopSection section );
        void CombineSections( int sectionId, int targetSectionId );
        void RemoveSection( int sectionId, int fromSectionId );


        String GetSectionIdsByPost( int postId );
    }

}

