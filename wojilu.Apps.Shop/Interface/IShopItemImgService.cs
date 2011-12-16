/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Shop.Domain;

namespace wojilu.Apps.Shop.Interface {

    public interface IShopItemImgService {

        ShopItemImg GetImgById( int imgId );
        ShopItem GetTopImg(int sectionId, int typeId, int appId);
        ShopItem GetTopImgByCategory(int sectionId, int categoryId, int appId);
        List<ShopItemImg> GetImgList(int postId);
        DataPage<ShopItemImg> GetImgPage( int postId );
        DataPage<ShopItemImg> GetImgPage( int postId, int currentPage );

        List<ShopItem> GetByType(int sectionId, int typeid, int appId);
        List<ShopItem> GetByType(int sectionId, int typeid, int appId, int count);

        List<ShopItem> GetByCategory(int sectionId, int categoryId, int appId);
        List<ShopItem> GetByCategory(int sectionId, int categoryId, int appId, int count);

        int GetImgCount( int postId );

        void CreateImg( ShopItemImg img );
        void DeleteImg( ShopItem post );
        void DeleteImgOne( ShopItemImg articleImg );
        void UpdateImgLogo( ShopItem post );


    }

}
