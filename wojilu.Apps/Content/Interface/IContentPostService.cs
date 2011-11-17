/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Apps.Content.Domain;
using wojilu.Members.Interface;
using wojilu.Common.Tags;

namespace wojilu.Apps.Content.Interface {


    public interface IContentPostService {

        ContentPost GetById( int postId, int ownerId );

        List<ContentPost> GetRecentPost( int appId, int count, int typeId );
        List<ContentPost> GetRankPost( int appId, int count, int typeId );

        List<ContentPost> GetBySection( int sectionId );
        List<ContentPost> GetBySection( List<ContentPost> dataAll, int sectionId );
        List<ContentPost> GetBySection( int appId, int sectionId );
        List<ContentPost> GetBySection( int appId, int sectionId, int count );

        DataPage<ContentPost> GetBySectionAndCategory( int sectionId, int categoryId );
        DataPage<ContentPost> GetBySectionAndCategory( int sectionId, int categoryId, int pageSize );

        DataPage<ContentPost> GetPageBySection( int sectionId );
        DataPage<ContentPost> GetPageBySection( int sectionId, int pageSize );
        DataPage<ContentPost> GetPageBySectionAndCategory( int sectionId, int categoryId );

        List<ContentPost> GetTopBySectionAndCategory( int sectionId, int categoryId, int appId );

        DataPage<ContentPost> GetByApp( int appId, int pageSize );
        DataPage<ContentPost> GetBySearch( int appId, string key, int pageSize );
        DataPage<ContentPost> GetTrashByApp( int appId, int pageSize );

        ContentPost GetFirstPost( int appId, int sectionId );

        DataPage<ContentPost> GetByCreator( int creatorId, IMember owner, int appId );
        int CountByCreator( int creatorId, IMember owner, int appId );

        void AddHits( ContentPost post );

        void Insert( ContentPost post, String tagList );
        void Insert( ContentPost post, String sectionIds, string tagList );

        void Update( ContentPost post, String tagList );
        void Update( ContentPost post, String sectionIds, String tagList );

        void Delete( ContentPost post );
        void Restore( int id );
        void DeleteTrue( int postId );
        
        void UpdateAttachmentPermission( ContentPost post, int ischeck );
        void UpdateSection( ContentPost post, int sectionId );
        void UpdateTitleStyle( ContentPost post, string titleStyle );
        void UpdateSection( string ids, int sectionId );


        List<ContentPost> GetRelatedPosts( ContentPost post );
        List<DataTagShip> GetRelatedDatas( ContentPost post );

        ContentPost GetPrevPost( ContentPost post );
        ContentPost GetNextPost( ContentPost post );


        void DeleteBatch( string ids );
        void SetStatus_Pick( string ids );
        void SetStatus_Normal( string ids );
        void SetStatus_Focus( string ids );

    }
}

