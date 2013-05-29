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

        List<ContentPost> GetRelatedPosts( ContentPost post, int count );
        List<DataTagShip> GetRelatedDatas( ContentPost post, int count );

        ContentPost GetPrevPost( ContentPost post );
        ContentPost GetNextPost( ContentPost post );

        List<ContentPost> GetRecentPost( int appId, int count, int typeId );
        List<ContentPost> GetRankPost( int appId, int count, int typeId );

        int CountBySection( int sectionId );
        List<ContentPost> GetAllBySection( int sectionId );
        List<ContentPost> GetBySection( int sectionId );
        List<ContentPost> GetBySection( int sectionId, int count );
        List<ContentPost> GetTopBySectionAndCategory( int sectionId, int categoryId );

        DataPage<ContentPost> GetPageBySectionAndCategory( int sectionId, int categoryId );
        DataPage<ContentPost> GetPageBySectionAndCategory( int sectionId, int categoryId, int pageSize );

        DataPage<ContentPost> GetPageBySection( int sectionId );
        DataPage<ContentPost> GetPageBySection( int sectionId, int pageSize );

        List<ContentPost> GetByIds( string ids );
        List<ContentPost> GetByApp( int appId );

        DataPage<ContentPost> GetByApp( int appId, int pageSize );

        DataPage<ContentPost> GetBySearch( int appId, string key, int pageSize );
        DataPage<ContentPost> GetTrashByApp( int appId, int pageSize );

        ContentPost GetFirstPost( int appId, int sectionId );

        DataPage<ContentPost> GetPageByCreator( int creatorId, IMember owner, int appId );

        int CountByCreator( int creatorId, IMember owner, int appId );
        int CountByApp( int appId );


        void AddHits( ContentPost post );

        void Insert( ContentPost post, String tagList );
        void Insert( ContentPost post, String sectionIds, string tagList );

        void Update( ContentPost post, String tagList );
        void Update( ContentPost post, String sectionIds, String tagList );

        void Delete( ContentPost post );
        void DeleteBatch( string ids );
        void Restore( int id );
        void DeleteSys( int postId );
        void DeleteTrueBatch( string ids );

        void UpdateAttachmentPermission( ContentPost post, int ischeck );
        void UpdateTitleStyle( ContentPost post, string titleStyle );

        void SetStatus_Pick( string ids );
        void SetStatus_Normal( string ids );
        void SetStatus_Focus( string ids );

        void Trans( string postIds, string targetSectionIds );
    }
}

