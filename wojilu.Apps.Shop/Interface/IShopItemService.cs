/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Apps.Shop.Domain;
using wojilu.Members.Interface;
using wojilu.Common.Tags;

namespace wojilu.Apps.Shop.Interface {


    public interface IShopItemService {

        ShopItem GetById( int postId, int ownerId );
        DataPage<ShopItem> GetByCategory(int appId, int categoryId);
        DataPage<ShopItem> GetByCategory(int appId, int categoryId, int brandId);
        DataPage<ShopItem> GetByCategory(int appId, int categoryId, int brandId, int providerId);
        DataPage<ShopItem> GetByProvider(int appId, int providerId);
        DataPage<ShopItem> GetByBrand(int appId, int brandId);

        List<ShopItem> GetRecentByCategory(int appId, int count, int categoryId);
        List<ShopItem> GetRankByCategory(int appId, int count, int categoryId);

        DataPage<ShopItem> GetBySectionAndCategory(int sectionId, int categoryId);
        DataPage<ShopItem> GetBySectionAndCategory(int sectionId, int categoryId, int pageSize);

        List<ShopItem> GetRecentPost(int appId, int count, int typeId);
        List<ShopItem> GetRankPost( int appId, int count, int typeId );

        List<ShopItem> GetBySection( int sectionId );
        List<ShopItem> GetBySection( List<ShopItem> dataAll, int sectionId );
        List<ShopItem> GetBySection( int appId, int sectionId );
        List<ShopItem> GetBySection( int appId, int sectionId, int count );

        DataPage<ShopItem> GetBySectionAndType(int sectionId, int typeId);
        DataPage<ShopItem> GetBySectionAndType(int sectionId, int typeId, int pageSize);

        DataPage<ShopItem> GetPageBySection( int sectionId );
        DataPage<ShopItem> GetPageBySection( int sectionId, int pageSize );
        DataPage<ShopItem> GetPageBySectionAndCategory( int sectionId, int categoryId );
        DataPage<ShopItem> GetPageBySectionAndType(int sectionId, int typeId);

        List<ShopItem> GetTopBySectionAndCategory(int sectionId, int categoryId, int appId);
        List<ShopItem> GetTopBySectionAndType(int sectionId, int typeId, int appId);

        DataPage<ShopItem> GetByApp( int appId, int pageSize );
        DataPage<ShopItem> GetBySearch( int appId, string key, int pageSize );
        DataPage<ShopItem> GetTrashByApp( int appId, int pageSize );

        ShopItem GetFirstPost( int appId, int sectionId );

        DataPage<ShopItem> GetByCreator( int creatorId, IMember owner, int appId );
        int CountByCreator( int creatorId, IMember owner, int appId );

        void AddHits( ShopItem post );

        void Insert( ShopItem post, String tagList );
        void Insert( ShopItem post, String sectionIds, string tagList );

        void Update( ShopItem post, String tagList );
        void Update( ShopItem post, String sectionIds, String tagList );

        void Delete( ShopItem post );
        void Restore( int id );
        void DeleteTrue( int postId );
        
        void UpdateAttachmentPermission( ShopItem post, int ischeck );
        void UpdateSection( ShopItem post, int sectionId );
        void UpdateTitleStyle( ShopItem post, string titleStyle );
        void UpdateSection( string ids, int sectionId );


        List<ShopItem> GetRelatedPosts( ShopItem post );
        List<DataTagShip> GetRelatedDatas( ShopItem post );

        ShopItem GetPrevPost( ShopItem post );
        ShopItem GetNextPost( ShopItem post );


        void DeleteBatch( string ids );
        void SetStatus_Pick( string ids );
        void SetStatus_Normal( string ids );
        void SetStatus_Focus( string ids );
        void SetOnSale(string ids);
        void SetUnSale(string ids);

    }
}

