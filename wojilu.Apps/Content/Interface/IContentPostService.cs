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

        ContentPost GetById(long postId, long ownerId);

        List<ContentPost> GetRelatedPosts( ContentPost post, int count );
        List<DataTagShip> GetRelatedDatas( ContentPost post, int count );

        ContentPost GetPrevPost( ContentPost post );
        ContentPost GetNextPost( ContentPost post );

        List<ContentPost> GetRecentPost(long appId, int count, int typeId);
        List<ContentPost> GetRankPost(long appId, int count, int typeId);

        int CountBySection(long sectionId);
        List<ContentPost> GetAllBySection(long sectionId);
        List<ContentPost> GetBySection(long sectionId);
        List<ContentPost> GetBySection(long sectionId, int count);
        List<ContentPost> GetTopBySectionAndCategory(long sectionId, long categoryId);

        DataPage<ContentPost> GetPageBySectionAndCategory(long sectionId, long categoryId);
        DataPage<ContentPost> GetPageBySectionAndCategory(long sectionId, long categoryId, int pageSize);

        DataPage<ContentPost> GetPageBySection(long sectionId);
        DataPage<ContentPost> GetPageBySection(long sectionId, int pageSize);

        List<ContentPost> GetByIds( string ids );
        List<ContentPost> GetByApp(long appId);

        DataPage<ContentPost> GetByApp(long appId, int pageSize);

        DataPage<ContentPost> GetBySearch(long appId, string key, int pageSize);
        DataPage<ContentPost> GetTrashByApp(long appId, int pageSize);

        ContentPost GetFirstPost(long appId, long sectionId);

        DataPage<ContentPost> GetPageByCreator(long creatorId, IMember owner, long appId);

        int CountByCreator(long creatorId, IMember owner, long appId);
        int CountByApp(long appId);


        void AddHits( ContentPost post );

        void Insert( ContentPost post, String tagList );
        void Insert( ContentPost post, String sectionIds, string tagList );

        void Update( ContentPost post, String tagList );
        void Update( ContentPost post, String sectionIds, String tagList );

        void Delete( ContentPost post );
        void DeleteBatch( string ids );
        void Restore(long id);
        void DeleteSys(long postId);
        void DeleteTrueBatch( string ids );

        void UpdateAttachmentPermission( ContentPost post, int ischeck );
        void UpdateTitleStyle( ContentPost post, string titleStyle );

        void SetStatus_Pick( string ids );
        void SetStatus_Normal( string ids );
        void SetStatus_Focus( string ids );

        void Trans( string postIds, string targetSectionIds );
    }
}

