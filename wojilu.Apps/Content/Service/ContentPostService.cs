/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.Web.Mvc;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Enum;
using wojilu.Apps.Content.Interface;

using wojilu.Common;
using wojilu.Common.AppBase;
using wojilu.Common.Jobs;
using wojilu.Common.Tags;

using wojilu.Members.Interface;


namespace wojilu.Apps.Content.Service {

    public class ContentPostService : IContentPostService {

        private static readonly ILog logger = LogManager.GetLogger( typeof( ContentPostService ) );

        //--------------------- service ------------------------------------------------------------------------

        public virtual List<IBinderValue> GetByAppIds( String ids, int count ) {
            String sids = checkIds( ids );
            if (strUtil.IsNullOrEmpty( sids )) return new List<IBinderValue>();

            if (count <= 0) count = 10;

            List<ContentPost> list = ContentPost.find( "AppId in (" + sids + ") and SaveStatus=" + SaveStatus.Normal ).list( count );

            return populateBinder( list );
        }

        public virtual List<IBinderValue> GetByTags( String tags, int count ) {

            if (count <= 0) count = 10;

            IList list = TagService.FindByTags( tags, typeof( ContentPost ), count );

            return populateBinder( list );
        }

        public virtual List<IBinderValue> GetHitsRankByAppIds( String ids, int count ) {
            String sids = checkIds( ids );
            if (strUtil.IsNullOrEmpty( sids )) return new List<IBinderValue>();

            if (count <= 0) count = 10;

            String strSort = " order by Hits desc, Id desc";
            List<ContentPost> list = ContentPost.find( "AppId in (" + sids + ")  and SaveStatus=" + SaveStatus.Normal + strSort ).list( count );

            return populateBinder( list );
        }

        public virtual List<IBinderValue> GetRepliesRankByAppIds( String ids, int count ) {
            String sids = checkIds( ids );
            if (strUtil.IsNullOrEmpty( sids )) return new List<IBinderValue>();

            if (count <= 0) count = 10;

            String strSort = " order by Replies desc, Id desc";
            List<ContentPost> list = ContentPost.find( "AppId in (" + sids + ")  and SaveStatus=" + SaveStatus.Normal + strSort ).list( count );

            return populateBinder( list );
        }

        public virtual List<IBinderValue> GetBySectionIds( String ids, int count ) {

            String sids = checkIds( ids );
            if (strUtil.IsNullOrEmpty( sids )) return new List<IBinderValue>();

            if (count <= 0) count = 10;

            List<ContentPostSection> psList = ContentPostSection
                .find( "SectionId in (" + sids + ") and SaveStatus=" + SaveStatus.Normal )
                .list( count );

            List<ContentPost> list = populatePost( psList );

            return populateBinder( list );
        }

        //---------------------------------------------------------------------------------------------

        private ContentPost GetById( int postId ) {
            ContentPost post = db.findById<ContentPost>( postId );
            if (post == null) return null;
            if (post.SaveStatus != SaveStatus.Normal) return null;
            return post;
        }

        public virtual ContentPost GetById( int id, int ownerId ) {
            ContentPost post = GetById( id );
            if (post == null) return null;
            if (post.OwnerId != ownerId) return null;
            if (post.SaveStatus != SaveStatus.Normal) return null;
            return post;
        }

        public virtual List<ContentPost> GetByIds( string ids ) {
            if (strUtil.IsNullOrEmpty( ids )) return new List<ContentPost>();
            List<ContentPost> posts = ContentPost.find( "Id in (" + ids + ") and SaveStatus=" + SaveStatus.Normal ).list();

            return posts;
        }

        //---------------------------------------------------------------------------------------------

        public virtual ContentPost GetPrevPost( ContentPost post ) {
            return ContentPost.find( "Id<" + post.Id + " and AppId=" + post.AppId + " and SaveStatus=" + SaveStatus.Normal + " order by Id desc" ).first();
        }

        public virtual ContentPost GetNextPost( ContentPost post ) {
            return ContentPost.find( "Id>" + post.Id + " and AppId=" + post.AppId + " and SaveStatus=" + SaveStatus.Normal + " order by Id asc" ).first();
        }


        public virtual List<DataTagShip> GetRelatedDatas( ContentPost post, int count ) {

            String tagIds = post.Tag.TagIds;
            if (strUtil.IsNullOrEmpty( tagIds )) return new List<DataTagShip>();

            return DataTagShip.find( "TagId in (" + tagIds + ")" ).list( count );
        }


        public virtual List<ContentPost> GetRelatedPosts( ContentPost post, int count ) {

            String tagIds = post.Tag.TagIds;
            if (strUtil.IsNullOrEmpty( tagIds )) return new List<ContentPost>();

            List<DataTagShip> list = DataTagShip.find( "TagId in (" + tagIds + ") and TypeFullName=:tname" )
                .set( "tname", typeof( ContentPost ).FullName )
                .list( count );

            List<int> ids = new List<int>();
            foreach (DataTagShip x in list) {
                if (ids.Contains( x.DataId )) continue;
                ids.Add( x.DataId );
            }

            if (ids.Count == 0) return new List<ContentPost>();

            String strIds = "";
            for (int i = 0; i < ids.Count; i++) {
                strIds += ids[i];
                if (i < ids.Count - 1) strIds += ",";
            }

            return GetByIds( strIds );
        }

        private void getPostIds( List<int> ids, List<DataTagShip> list, ContentPost post ) {
            foreach (DataTagShip dt in list) {
                if (ids.Contains( dt.DataId ) == false && dt.DataId != post.Id) ids.Add( dt.DataId );
            }
        }

        //---------------------------------------------------------------------------------------------


        public virtual List<ContentPost> GetBySection( int sectionId ) {
            ContentSection s = ContentSection.findById( sectionId );
            return this.GetBySection( sectionId, s.ListCount );
        }

        public virtual List<ContentPost> GetBySection( int sectionId, int count ) {

            List<ContentPostSection> psList = ContentPostSection
                .find( "SectionId=" + sectionId + " and SaveStatus=" + SaveStatus.Normal + " order by PostId desc" )
                .list( count );

            if (psList.Count == 0) {
                // 兼容旧版
                return ContentPost.find( "SectionId=" + sectionId + " and SaveStatus=" + SaveStatus.Normal ).list( count );
            }

            return populatePost( psList );
        }

        public virtual List<ContentPost> GetTopBySectionAndCategory( int sectionId, int categoryId, int count ) {

            List<ContentPost> xlist = GetBySection( sectionId, count );

            List<ContentPost> list = new List<ContentPost>();
            foreach (ContentPost x in xlist) {
                if (x.CategoryId == categoryId) {
                    list.Add( x );
                }
            }

            return list;
        }

        public virtual int CountBySection( int sectionId ) {
            int count = ContentPostSection.count( "SectionId=" + sectionId + " and SaveStatus=" + SaveStatus.Normal );
            // 兼容旧版
            if (count == 0) {
                return ContentPost.count( "SectionId=" + sectionId + " and SaveStatus=" + SaveStatus.Normal );
            }
            else {
                return count;
            }
        }

        public virtual List<ContentPost> GetAllBySection( int sectionId ) {
            List<ContentPost> xResult = ContentPostSection
                .find( "SectionId=" + sectionId + " and SaveStatus=" + SaveStatus.Normal + " order by PostId desc" )
                .listChildren<ContentPost>( "Post" );

            if (xResult.Count == 0) {
                // 兼容旧版
                return ContentPost.find( "SectionId=" + sectionId + " and SaveStatus=" + SaveStatus.Normal ).list();
            }
            else {
                return xResult;
            }
        }

        public virtual ContentPost GetFirstPost( int appId, int sectionId ) {
            ContentPostSection xResult = ContentPostSection
                .find( "SectionId=" + sectionId + " and SaveStatus=" + SaveStatus.Normal + " order by PostId desc" )
                .first();

            if (xResult == null) {
                // 兼容旧版
                return ContentPost.find( "SectionId=" + sectionId + " and SaveStatus=" + SaveStatus.Normal ).first();
            }
            if (xResult.Post.AppId != appId) return null;
            return xResult.Post;
        }

        public virtual List<ContentPost> GetTopBySectionAndCategory( int sectionId, int categoryId ) {
            ContentSection s = ContentSection.findById( sectionId );

            return GetTopBySectionAndCategory( sectionId, categoryId, s.ListCount );
        }

        //--------------------------------------------------------------------------------------------------------------

        public virtual DataPage<ContentPost> GetPageByCreator( int creatorId, IMember owner, int appId ) {
            String condition = string.Format( "CreatorId={0} and OwnerId={1} and OwnerType='{2}' and AppId={3} and SaveStatus={4}", creatorId, owner.Id, owner.GetType().FullName, appId, SaveStatus.Normal );
            return ContentPost.findPage( condition );
        }

        public virtual int CountByCreator( int creatorId, IMember owner, int appId ) {
            String condition = string.Format( "CreatorId={0} and OwnerId={1} and OwnerType='{2}' and AppId={3} and SaveStatus={4}", creatorId, owner.Id, owner.GetType().FullName, appId, SaveStatus.Normal );
            return ContentPost.count( condition );
        }

        public virtual int CountByApp( int appId ) {
            return ContentPost.count( "AppId=" + appId + " and SaveStatus=" + SaveStatus.Normal );
        }

        public virtual List<ContentPost> GetRecentPost( int appId, int count, int typeId ) {
            return ContentPost.find( "AppId=" + appId + " and CategoryId=" + typeId + " and SaveStatus=" + SaveStatus.Normal ).list( count );
        }

        public virtual List<ContentPost> GetRankPost( int appId, int count, int typeId ) {
            return ContentPost.find( "AppId=" + appId + " and CategoryId=" + typeId + " and SaveStatus=" + SaveStatus.Normal + " order by Hits desc, Replies desc, Id desc" ).list( count );
        }


        public virtual List<ContentPost> GetByApp( int appId ) {
            return ContentPost.find( "AppId=" + appId + " and SaveStatus=" + SaveStatus.Normal + " order by Id desc" ).list();
        }

        public virtual DataPage<ContentPost> GetByApp( int appId, int pageSize ) {
            return ContentPost.findPage( "AppId=" + appId + " and SaveStatus=" + SaveStatus.Normal + " order by Id desc", pageSize );
        }

        public virtual DataPage<ContentPost> GetTrashByApp( int appId, int pageSize ) {
            return ContentPost.findPage( "AppId=" + appId + " and SaveStatus=" + SaveStatus.Delete + " order by Id desc", pageSize );
        }


        public virtual DataPage<ContentPost> GetBySearch( int appId, String key, int pageSize ) {
            return ContentPost.findPage( "AppId=" + appId + " and Title like '%" + key + "%' and SaveStatus=" + SaveStatus.Normal + " order by Id desc", pageSize );
        }

        //--------------------------------------------------------------------------------------------------

        public virtual DataPage<ContentPost> GetPageBySection( int sectionId ) {
            return GetPageBySection( sectionId, 20 );
        }

        public virtual DataPage<ContentPost> GetPageBySection( int sectionId, int pageSize ) {
            DataPage<ContentPostSection> list = ContentPostSection.findPage( "SectionId=" + sectionId + " and SaveStatus=" + SaveStatus.Normal + " order by PostId desc", pageSize );
            DataPage<ContentPost> xResult = list.Convert<ContentPost>( populatePost( list.Results ) );

            // 兼容旧版
            if (xResult.RecordCount == 0) {
                return ContentPost.findPage( "SectionId=" + sectionId + " and SaveStatus=" + SaveStatus.Normal, pageSize );
            }
            else {
                return xResult;
            }
        }

        // 用在 AdminList 中
        public virtual DataPage<ContentPost> GetPageBySectionAndCategory( int sectionId, int categoryId ) {
            return GetPageBySectionAndCategory( sectionId, categoryId, 20 );
        }

        // 用在 AdminList 中
        public virtual DataPage<ContentPost> GetPageBySectionAndCategory( int sectionId, int categoryId, int pageSize ) {
            // TODO
            //if (categoryId > 0) {
            //    return db.findPage<ContentPost>( "PageSection.Id=" + sectionId + " and CategoryId=" + categoryId + " and SaveStatus=" + SaveStatus.Normal, pageSize );
            //}
            return GetPageBySection( sectionId, pageSize );
        }

        //--------------------------------------------------------------------------------------------------


        public virtual void Insert( ContentPost post, String tagList ) {

            if (post.PageSection == null) {
                logger.Error( "post.PageSection can not be null" );
                return;
            }
            else if (post.PageSection.Id <= 0) {
                logger.Error( "post.PageSection.Id==" + post.PageSection.Id );
                return;
            }

            String sectionIds = post.PageSection.Id.ToString();
            Insert( post, sectionIds, tagList );
        }

        public virtual void Insert( ContentPost post, string sectionIds, string tagList ) {

            int[] ids = cvt.ToIntArray( sectionIds );
            if (ids.Length == 0) throw new ArgumentException( "sectionIds" );

            // 保存
            post.PageSection = new ContentSection { Id = ids[0] }; // 缓存Section
            post.insert();

            // 多对多处理
            ContentPostSection ps = new ContentPostSection();
            foreach (int sectionId in ids) {
                ContentSection section = new ContentSection();
                section.Id = sectionId;
                ps.Post = post;
                ps.Section = section;
                ps.insert();
            }

            // tag
            post.Tag.Save( tagList );
        }

        //----------------------------------------------------------------------------------------------------------

        public virtual void Update( ContentPost post, String tagList ) {
            if (db.update( post ).IsValid) {
                post.Tag.Save( tagList );
            }
        }

        public virtual void Update( ContentPost post, String sectionIds, String tagList ) {

            // 保存
            if (db.update( post ).IsValid) {
                post.Tag.Save( tagList );
            }

            // 多对多处理
            ContentPostSection.deleteBatch( "PostId=" + post.Id );
            ContentPostSection ps = new ContentPostSection();
            int[] ids = cvt.ToIntArray( sectionIds );
            foreach (int sectionId in ids) {
                ContentSection section = new ContentSection();
                section.Id = sectionId;
                ps.Post = post;
                ps.Section = section;
                ps.insert();
            }
        }

        public virtual void UpdateTitleStyle( ContentPost post, string titleStyle ) {
            post.Style = titleStyle;
            post.update( "Style" );
        }

        public virtual void Delete( ContentPost post ) {
            post.SaveStatus = SaveStatus.Delete;
            post.update();
            ContentPostSection.updateBatch( "SaveStatus=" + SaveStatus.Delete, "PostId=" + post.Id );
        }

        public virtual void Restore( int id ) {
            ContentPost post = ContentPost.findById( id );
            if (post == null) return;
            post.SaveStatus = SaveStatus.Normal;
            post.update();

            ContentPostSection.updateBatch( "SaveStatus=" + SaveStatus.Normal, "PostId=" + post.Id );
        }

        public virtual void DeleteSys( int postId ) {
            ContentPost post = ContentPost.findById( postId );
            if (post == null) return;
            post.SaveStatus = SaveStatus.SysDelete;
            post.update();
            ContentPostSection.updateBatch( "SaveStatus=" + SaveStatus.SysDelete, "PostId=" + post.Id );
        }

        public virtual void AddHits( ContentPost post ) {
            HitsJob.Add( post );
        }

        public virtual void UpdateAttachmentPermission( ContentPost post, int ischeck ) {
            post.IsAttachmentLogin = ischeck;
            post.update( "IsAttachmentLogin" );
        }

        public virtual void DeleteTrueBatch( string ids ) {
            ContentPost.deleteBatch( "Id in (" + ids + ")" );
            ContentPostSection.deleteBatch( "PostId in  in (" + ids + ")" );
        }

        public virtual void DeleteBatch( string ids ) {
            ContentPost.updateBatch( "set SaveStatus=" + SaveStatus.Delete, "Id in (" + ids + ")" );
            ContentPostSection.updateBatch( "SaveStatus=" + SaveStatus.Delete, "PostId in(" + ids + ")" );
        }

        public virtual void SetStatus_Pick( string ids ) {
            ContentPost.updateBatch( "set PickStatus=" + PickStatus.Picked, "Id in (" + ids + ")" );
        }
        public virtual void SetStatus_Normal( string ids ) {
            ContentPost.updateBatch( "set PickStatus=" + PickStatus.Normal, "Id in (" + ids + ")" );
        }
        public virtual void SetStatus_Focus( string ids ) {
            ContentPost.updateBatch( "set PickStatus=" + PickStatus.Focus, "Id in (" + ids + ")" );
        }

        //----------------------------------------------------------------------------------------------------------

        public virtual void Trans( String postIds, String targetSectionIds ) {

            List<ContentPost> posts = ContentPost.find( "Id in (" + postIds + ") " ).list();

            List<ContentSection> sections = ContentSection.find( "Id in (" + targetSectionIds + ") " ).list();

            foreach (ContentPost x in posts) {
                transOne( x, sections );
            }
        }

        private void transOne( ContentPost x, List<ContentSection> sections ) {

            if (sections.Count == 0) return;

            // 0. 先更新post
            x.AppId = sections[0].AppId;
            x.PageSection = sections[0];
            x.update();

            // 1. 删除旧有关系
            ContentPostSection.deleteBatch( "PostId=" + x.Id );

            // 2. 挪到新section中
            foreach (ContentSection section in sections) {

                // 多对多处理
                ContentPostSection ps = new ContentPostSection();
                ps.Post = x;
                ps.Section = section;
                ps.insert();

            }
        }

        //-----------------------------------------------------------------------------------------

        private String checkIds( String ids ) {

            int[] arrIds = cvt.ToIntArray( ids );
            if (arrIds.Length == 0) return null;

            String sids = "";
            for (int i = 0; i < arrIds.Length; i++) {
                if (arrIds[i] == 0) continue;
                sids += arrIds[i];
                if (i < arrIds.Length - 1) sids += ",";
            }

            return sids;
        }

        private static List<IBinderValue> populateBinder( IList list ) {

            List<IBinderValue> results = new List<IBinderValue>();
            foreach (ContentPost post in list) {
                IBinderValue vo = new ItemValue();

                vo.Title = post.Title;
                vo.Created = post.Created;
                vo.CreatorName = post.Creator.Name;
                vo.Link = alink.ToAppData( post );
                vo.Content = post.Content;
                vo.Replies = post.Replies;
                vo.Category = post.SectionName;

                vo.obj = post;

                results.Add( vo );
            }

            return results;
        }

        private List<ContentPost> populatePost( List<ContentPostSection> psList ) {

            List<ContentPost> list = new List<ContentPost>();
            foreach (ContentPostSection x in psList) {
                if (x.Post != null) {
                    list.Add( x.Post );
                }
            }

            list.Sort();

            return list;
        }

    }


}

