/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

//using wojilu.ORM;
using wojilu.Drawing;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Enum;
using wojilu.Common.Jobs;
using wojilu.Log;
using wojilu.Common.AppBase;
using wojilu.Common;
using wojilu.Web.Mvc;
using wojilu.Members.Interface;
using wojilu.Common.Tags;
using System.Collections;

namespace wojilu.Apps.Content.Service {


    //---------------------------------------- mashup service ----------------------------------------------------------------------

    public class ContentPostService : IContentPostService {

        private static readonly ILog logger = LogManager.GetLogger( typeof( ContentPostService ) );

        public virtual ContentPost GetPrevPost( ContentPost post ) {
            return ContentPost.find( "Id<" + post.Id + " and AppId=" + post.AppId + " and SaveStatus=" + SaveStatus.Normal + " order by Id desc" ).first();
        }

        public virtual ContentPost GetNextPost( ContentPost post ) {
            return ContentPost.find( "Id>" + post.Id + " and AppId=" + post.AppId + " and SaveStatus=" + SaveStatus.Normal + " order by Id asc" ).first();
        }

        public virtual List<DataTagShip> GetRelatedDatas( ContentPost post ) {

            List<Tag> tags = post.Tag.List;

            List<DataTagShip> results = new List<DataTagShip>();
            foreach (Tag t in tags) {

                List<DataTagShip> list = DataTagShip.find( "TagId=" + t.Id + "" ).list();
                mergeTagDatas( results, list, post );
            }

            return results;
        }

        private void mergeTagDatas( List<DataTagShip> results, List<DataTagShip> list, ContentPost post ) {

            foreach (DataTagShip dt in list) {

                if (dt.DataId == post.Id && dt.TypeFullName.Equals( typeof( ContentPost ).FullName )) continue;

                if (containsDataTag( results, dt ) == false) results.Add( dt );

            }

        }

        private bool containsDataTag( List<DataTagShip> results, DataTagShip dt ) {

            foreach (DataTagShip d in results) {

                if (d.DataId == dt.DataId && d.TypeFullName.Equals( dt.TypeFullName )) return true;

            }

            return false;
        }

        public virtual List<ContentPost> GetRelatedPosts( ContentPost post ) {

            List<Tag> tags = post.Tag.List;

            List<int> ids = new List<int>();

            foreach (Tag t in tags) {

                List<DataTagShip> list = DataTagShip.find( "TagId=" + t.Id + " and TypeFullName=:tname" )
                    .set( "tname", typeof( ContentPost ).FullName )
                    .list();

                getPostIds( ids, list, post );
            }

            if (ids.Count == 0) return new List<ContentPost>();

            String strIds = "";
            for (int i = 0; i < ids.Count; i++) {
                strIds += ids[i];
                if (i < ids.Count - 1) strIds += ",";
            }

            List<ContentPost> posts = ContentPost.find( "Id in (" + strIds + ") and SaveStatus=" + SaveStatus.Normal ).list();

            return posts;
        }

        private void getPostIds( List<int> ids, List<DataTagShip> list, ContentPost post ) {
            foreach (DataTagShip dt in list) {
                if (ids.Contains( dt.DataId ) == false && dt.DataId != post.Id) ids.Add( dt.DataId );
            }
        }

        public virtual DataPage<ContentPost> GetByCreator( int creatorId, IMember owner, int appId ) {
            String condition = string.Format( "CreatorId={0} and OwnerId={1} and OwnerType='{2}' and AppId={3} and SaveStatus={4}", creatorId, owner.Id, owner.GetType().FullName, appId, SaveStatus.Normal );
            return ContentPost.findPage( condition );
        }

        public virtual int CountByCreator( int creatorId, IMember owner, int appId ) {
            String condition = string.Format( "CreatorId={0} and OwnerId={1} and OwnerType='{2}' and AppId={3} and SaveStatus={4}", creatorId, owner.Id, owner.GetType().FullName, appId, SaveStatus.Normal );
            return ContentPost.count( condition );
        }


        public virtual List<IBinderValue> GetByAppIds( String ids, int count ) {
            String sids = checkIds( ids );
            if (strUtil.IsNullOrEmpty( sids )) return new List<IBinderValue>();

            if (count <= 0) count = 10;

            List<ContentPost> list = ContentPost.find( "AppId in (" + sids + ") and SaveStatus=" + SaveStatus.Normal ).list( count );

            return populatePosts( list );
        }

        public virtual List<IBinderValue> GetByTags( String tags, int count ) {

            if (count <= 0) count = 10;

            IList list = TagService.FindByTags( tags, typeof( ContentPost ), count );

            return populatePosts( list );
        }

        public virtual List<IBinderValue> GetHitsRankByAppIds( String ids, int count ) {
            String sids = checkIds( ids );
            if (strUtil.IsNullOrEmpty( sids )) return new List<IBinderValue>();

            if (count <= 0) count = 10;

            String strSort = " order by Hits desc, Id desc";
            List<ContentPost> list = ContentPost.find( "AppId in (" + sids + ")  and SaveStatus=" + SaveStatus.Normal + strSort ).list( count );

            return populatePosts( list );
        }

        public virtual List<IBinderValue> GetRepliesRankByAppIds( String ids, int count ) {
            String sids = checkIds( ids );
            if (strUtil.IsNullOrEmpty( sids )) return new List<IBinderValue>();

            if (count <= 0) count = 10;

            String strSort = " order by Replies desc, Id desc";
            List<ContentPost> list = ContentPost.find( "AppId in (" + sids + ")  and SaveStatus=" + SaveStatus.Normal + strSort ).list( count );

            return populatePosts( list );
        }

        public virtual List<IBinderValue> GetBySectionIds( String ids, int count ) {

            String sids = checkIds( ids );
            if (strUtil.IsNullOrEmpty( sids )) return new List<IBinderValue>();

            if (count <= 0) count = 10;

            List<ContentPost> list = ContentPost.find( "SectionId in (" + sids + ")  and SaveStatus=" + SaveStatus.Normal ).list( count );

            return populatePosts( list );
        }

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

        private static List<IBinderValue> populatePosts( IList list ) {

            List<IBinderValue> results = new List<IBinderValue>();
            foreach (ContentPost post in list) {
                IBinderValue vo = new ItemValue();

                vo.Title = post.Title;
                vo.Created = post.Created;
                vo.CreatorName = post.Creator.Name;
                vo.Link = alink.ToAppData( post );
                vo.Content = post.Content;
                vo.Replies = post.Replies;
                vo.Category = post.PageSection.Title;

                results.Add( vo );
            }

            return results;
        }

        //--------------------------------------------------------------------------------------------------------------


        public virtual List<ContentPost> GetRecentPost( int appId, int count, int typeId ) {
            return db.find<ContentPost>( "AppId=" + appId + " and CategoryId=" + typeId + " and SaveStatus=" + SaveStatus.Normal ).list( count );
        }

        public virtual List<ContentPost> GetRankPost( int appId, int count, int typeId ) {
            return db.find<ContentPost>( "AppId=" + appId + " and CategoryId=" + typeId + " and SaveStatus=" + SaveStatus.Normal + " order by Hits desc, Replies desc, Id desc" ).list( count );
        }

        private ContentPost GetById( int postId ) {
            ContentPost post = db.findById<ContentPost>( postId );
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

        public virtual List<ContentPost> GetBySection( int sectionId ) {
            return db.find<ContentPost>( "PageSection.Id=" + sectionId + " and SaveStatus=" + SaveStatus.Normal + " order by Id desc" ).list();
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

        public virtual List<ContentPost> GetBySection( List<ContentPost> dataAll, int sectionId ) {
            List<ContentPost> result = new List<ContentPost>();
            foreach (ContentPost post in dataAll) {
                if (post.PageSection.Id == sectionId) {
                    result.Add( post );
                }
            }

            result.Sort();
            return result;
        }



        public virtual DataPage<ContentPost> GetBySectionAndCategory( int sectionId, int categoryId ) {
            return GetBySectionAndCategory( sectionId, categoryId, 20 );
        }

        public virtual DataPage<ContentPost> GetBySectionAndCategory( int sectionId, int categoryId, int pageSize ) {
            if (categoryId > 0) {
                return db.findPage<ContentPost>( "PageSection.Id=" + sectionId + " and CategoryId=" + categoryId + " and SaveStatus=" + SaveStatus.Normal, pageSize );
            }
            return db.findPage<ContentPost>( "PageSection.Id=" + sectionId + " and SaveStatus=" + SaveStatus.Normal, pageSize );
        }

        public virtual DataPage<ContentPost> GetPageBySection( int sectionId ) {
            return db.findPage<ContentPost>( "PageSection.Id=" + sectionId + " and SaveStatus=" + SaveStatus.Normal );
        }

        public virtual DataPage<ContentPost> GetPageBySection( int sectionId, int pageSize ) {
            return db.findPage<ContentPost>( "PageSection.Id=" + sectionId + " and SaveStatus=" + SaveStatus.Normal, pageSize );
        }

        public virtual DataPage<ContentPost> GetPageBySectionAndCategory( int sectionId, int categoryId ) {
            return db.findPage<ContentPost>( "PageSection.Id=" + sectionId + " and CategoryId=" + categoryId + " and SaveStatus=" + SaveStatus.Normal );
        }

        public virtual ContentPost GetFirstPost( int appId, int sectionId ) {
            ContentPost result = db.find<ContentPost>( "PageSection.Id=" + sectionId + " and SaveStatus=" + SaveStatus.Normal + " order by Id desc" ).first();
            if (result == null) return null;
            if (result.AppId != appId) return null;
            return result;
        }

        public virtual List<ContentPost> GetTopBySectionAndCategory( int sectionId, int categoryId, int appId ) {
            ContentSection s = ContentSection.findById( sectionId );

            return GetTopBySectionAndCategory( sectionId, categoryId, appId, s.ListCount );
        }

        public virtual List<ContentPost> GetBySection( int appId, int sectionId ) {
            ContentSection s = ContentSection.findById( sectionId );
            return this.GetBySection( appId, sectionId, s.ListCount );
        }

        public virtual List<ContentPost> GetBySection( int appId, int sectionId, int count ) {

            List<ContentPost> list;

            // 兼容旧版
            ContentSection s = ContentSection.findById( sectionId );
            if (strUtil.IsNullOrEmpty( s.CachePostIds )) {

                list = ContentPost.find( "AppId=" + appId + " and PageSection.Id=" + sectionId + " and SaveStatus=" + SaveStatus.Normal + " order by Id desc" ).list( count );
            }
            else {
                list = ContentPost.find( "Id in (" + s.CachePostIds + ") and SaveStatus=" + SaveStatus.Normal ).list( count );
            }

            list.Sort();
            return list;
        }

        public virtual List<ContentPost> GetTopBySectionAndCategory( int sectionId, int categoryId, int appId, int count ) {

            List<ContentPost> list;

            // 兼容旧版
            ContentSection s = ContentSection.findById( sectionId );
            if (strUtil.IsNullOrEmpty( s.CachePostIds )) {

                list = ContentPost.find( "AppId=" + appId + " and PageSection.Id=" + sectionId + " and CategoryId=" + categoryId + " and SaveStatus=" + SaveStatus.Normal + " order by Id desc" ).list( count );

            }
            else {

                //list = ContentPost.findBySql( "select top " + count + " from ContentPost a, ContentSection b where a.AppId=" + appId + " and a.SectionId=" + sectionId + " and a.CategoryId=" + categoryId + " and a.SaveStatus=" + SaveStatus.Normal + " and b.SectionId=" + sectionId );

                list = new List<ContentPost>();
                List<ContentPost> posts = ContentPost.find( "Id in (" + s.CachePostIds + ") and SaveStatus=" + SaveStatus.Normal ).list();
                foreach (ContentPost p in posts) {
                    if (p.CategoryId == categoryId) list.Add( p );
                }

            }

            list.Sort();

            return list;
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

            post.PageSection = new ContentSection(); // 不再使用旧版一对多关系
            post.PageSection.Id = ids[0];
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

            // 旧版一对多兼容处理
            foreach (int sectionId in ids) {
                updateCachePostIds( sectionId );
            }

            post.Tag.Save( tagList );
        }

        private void updateCachePostIds( int sectionId ) {

            ContentSection section = ContentSection.findById( sectionId );
            if (section == null) throw new NullReferenceException();

            // 为了兼容旧版，section和post一对多关系下的数据也要抓取
            List<ContentPostSection> list = ContentPostSection.find( "SectionId=" + sectionId ).list( section.ListCount );
            List<ContentPost> posts = ContentPost.find( "SectionId=" + sectionId ).list( section.ListCount );
            List<int> idList = getRecentIds( list, posts );

            section.CachePostIds = getCacheIds( idList, section.ListCount );
            section.update();
        }

        private static String getCacheIds( List<int> idList, int listCount ) {
            String ids = "";
            int icount = 0;
            foreach (int id in idList) {
                ids += id + ",";
                icount++;

                if (icount > listCount) break;
            }
            ids = ids.TrimEnd( ',' );
            return ids;
        }

        private List<int> getRecentIds( List<ContentPostSection> list, List<ContentPost> posts ) {

            List<int> idList = new List<int>();
            foreach (ContentPostSection ps in list) {
                if (ps.Post == null) continue;
                if (idList.Contains( ps.Post.Id )) continue;
                idList.Add( ps.Post.Id );
            }

            foreach (ContentPost post in posts) {
                if (idList.Contains( post.Id )) continue;
                idList.Add( post.Id );
            }

            idList.Sort();
            idList.Reverse();

            return idList;
        }

        //----------------------------------------------------------------------------------------------------------

        public virtual void Update( ContentPost post, String tagList ) {
            if (db.update( post ).IsValid) {
                post.Tag.Save( tagList );
            }
        }

        public virtual void Update( ContentPost post, String sectionIds, String tagList ) {

            if (db.update( post ).IsValid) {
                post.Tag.Save( tagList );
            }

            List<ContentPostSection> oldPsList = ContentPostSection.find( "PostId=" + post.Id ).list();

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

            post.PageSection = new ContentSection(); // 不再使用旧版一对多关系
            post.PageSection.Id = ids[0];
            post.update();


            // 更新旧的section关系
            foreach (ContentPostSection p in oldPsList) {
                updateCachePostIds( p.Section.Id );
            }
            // 更新新的section关系
            foreach (int sectionId in ids) {
                updateCachePostIds( sectionId );
            }

        }

        public virtual void UpdateSection( ContentPost post, int sectionId ) {
            ContentSection section = new ContentSection();
            section.Id = sectionId;
            post.PageSection = section;
            post.update();
        }

        public virtual void UpdateSection( String ids, int sectionId ) {

            ContentPost.updateBatch( "set SectionId=" + sectionId, "Id in (" + ids + ")" );
        }

        public virtual void UpdateTitleStyle( ContentPost post, string titleStyle ) {
            post.Style = titleStyle;
            post.update( "Style" );
        }

        public virtual void Delete( ContentPost post ) {
            post.SaveStatus = SaveStatus.Delete;
            post.update();
        }

        public virtual void Restore( int id ) {
            ContentPost post = ContentPost.findById( id );
            if (post == null) return;
            post.SaveStatus = SaveStatus.Normal;
            post.update();
        }
        public virtual void DeleteTrue( int postId ) {
            ContentPost post = ContentPost.findById( postId );
            if (post == null) return;
            post.SaveStatus = SaveStatus.SysDelete;
            post.update();
        }

        public virtual void AddHits( ContentPost post ) {
            HitsJob.Add( post );
        }

        public virtual void UpdateAttachmentPermission( ContentPost post, int ischeck ) {
            post.IsAttachmentLogin = ischeck;
            post.update( "IsAttachmentLogin" );
        }

        public virtual void DeleteBatch( string ids ) {
            ContentPost.deleteBatch( "Id in (" + ids + ")" );
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


    }


}

