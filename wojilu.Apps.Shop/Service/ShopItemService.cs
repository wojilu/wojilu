/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

//using wojilu.ORM;
using wojilu.Drawing;

using wojilu.Apps.Shop.Domain;
using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Enum;
using wojilu.Common.Jobs;
using wojilu.Log;
using wojilu.Common.AppBase;
using wojilu.Common;
using wojilu.Web.Mvc;
using wojilu.Members.Interface;
using wojilu.Common.Tags;
using System.Collections;

namespace wojilu.Apps.Shop.Service {


    //---------------------------------------- mashup service ----------------------------------------------------------------------

    public class ShopItemService : IShopItemService {

        public virtual ShopItem GetPrevPost( ShopItem post ) {
            return ShopItem.find( "Id<" + post.Id + " and AppId=" + post.AppId + " order by Id desc" ).first();
        }

        public virtual ShopItem GetNextPost( ShopItem post ) {
            return ShopItem.find( "Id>" + post.Id + " and AppId=" + post.AppId + " order by Id asc" ).first();
        }

        public virtual List<DataTagShip> GetRelatedDatas( ShopItem post ) {

            List<Tag> tags = post.Tag.List;

            List<DataTagShip> results = new List<DataTagShip>();
            foreach (Tag t in tags) {

                List<DataTagShip> list = DataTagShip.find( "TagId=" + t.Id + "" ).list();
                mergeTagDatas( results, list, post );
            }

            return results;
        }

        private void mergeTagDatas( List<DataTagShip> results, List<DataTagShip> list, ShopItem post ) {

            foreach (DataTagShip dt in list) {

                if (dt.DataId == post.Id && dt.TypeFullName.Equals( typeof( ShopItem ).FullName )) continue;

                if (containsDataTag( results, dt ) == false) results.Add( dt );

            }

        }

        private bool containsDataTag( List<DataTagShip> results, DataTagShip dt ) {

            foreach (DataTagShip d in results) {

                if (d.DataId == dt.DataId && d.TypeFullName.Equals( dt.TypeFullName )) return true;

            }

            return false;
        }

        public virtual List<ShopItem> GetRelatedPosts( ShopItem post ) {

            List<Tag> tags = post.Tag.List;

            List<int> ids = new List<int>();

            foreach (Tag t in tags) {

                List<DataTagShip> list = DataTagShip.find( "TagId=" + t.Id + " and TypeFullName=:tname" )
                    .set( "tname", typeof( ShopItem ).FullName )
                    .list();

                getItemIds( ids, list, post );
            }

            if (ids.Count == 0) return new List<ShopItem>();

            String strIds = "";
            for (int i = 0; i < ids.Count; i++) {
                strIds += ids[i];
                if (i < ids.Count - 1) strIds += ",";
            }

            List<ShopItem> posts = ShopItem.find( "Id in (" + strIds + ")" ).list();

            return posts;
        }

        private void getItemIds( List<int> ids, List<DataTagShip> list, ShopItem post ) {
            foreach (DataTagShip dt in list) {
                if (ids.Contains( dt.DataId ) == false && dt.DataId != post.Id) ids.Add( dt.DataId );
            }
        }

        public virtual DataPage<ShopItem> GetByCreator( int creatorId, IMember owner, int appId ) {
            String condition = string.Format( "CreatorId={0} and OwnerId={1} and OwnerType='{2}' and AppId={3} and SaveStatus={4}", creatorId, owner.Id, owner.GetType().FullName, appId, SaveStatus.Normal );
            return ShopItem.findPage( condition );
        }

        public virtual int CountByCreator( int creatorId, IMember owner, int appId ) {
            String condition = string.Format( "CreatorId={0} and OwnerId={1} and OwnerType='{2}' and AppId={3} and SaveStatus={4}", creatorId, owner.Id, owner.GetType().FullName, appId, SaveStatus.Normal );
            return ShopItem.count( condition );
        }


        public virtual List<IBinderValue> GetByAppIds( String ids, int count ) {
            String sids = checkIds( ids );
            if (strUtil.IsNullOrEmpty( sids )) return new List<IBinderValue>();

            if (count <= 0) count = 10;

            List<ShopItem> list = ShopItem.find( "AppId in (" + sids + ") and SaveStatus=" + SaveStatus.Normal ).list( count );

            return populatePosts( list );
        }

        public virtual List<IBinderValue> GetByTags( String tags, int count ) {

            if (count <= 0) count = 10;

            IList list = TagService.FindByTags( tags, typeof( ShopItem ), count );

            return populatePosts( list );
        }

        public virtual List<IBinderValue> GetHitsRankByAppIds( String ids, int count ) {
            String sids = checkIds( ids );
            if (strUtil.IsNullOrEmpty( sids )) return new List<IBinderValue>();

            if (count <= 0) count = 10;

            String strSort = " order by Hits desc, Id desc";
            List<ShopItem> list = ShopItem.find( "AppId in (" + sids + ")  and SaveStatus=" + SaveStatus.Normal + strSort ).list( count );

            return populatePosts( list );
        }

        public virtual List<IBinderValue> GetRepliesRankByAppIds( String ids, int count ) {
            String sids = checkIds( ids );
            if (strUtil.IsNullOrEmpty( sids )) return new List<IBinderValue>();

            if (count <= 0) count = 10;

            String strSort = " order by Replies desc, Id desc";
            List<ShopItem> list = ShopItem.find( "AppId in (" + sids + ")  and SaveStatus=" + SaveStatus.Normal + strSort ).list( count );

            return populatePosts( list );
        }

        public virtual List<IBinderValue> GetBySectionIds( String ids, int count ) {

            String sids = checkIds( ids );
            if (strUtil.IsNullOrEmpty( sids )) return new List<IBinderValue>();

            if (count <= 0) count = 10;

            List<ShopItem> list = ShopItem.find( "SectionId in (" + sids + ")  and SaveStatus=" + SaveStatus.Normal ).list( count );

            return populatePosts( list );
        }

        public virtual List<IBinderValue> GetByCategoryIds(String ids, int count)
        {

            String sids = checkIds(ids);
            if (strUtil.IsNullOrEmpty(sids)) return new List<IBinderValue>();

            if (count <= 0) count = 10;

            List<ShopItem> list = ShopItem.find("CategoryId in (" + sids + ")  and SaveStatus=" + SaveStatus.Normal).list(count);

            return populatePosts(list);
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
            foreach (ShopItem post in list) {
                IBinderValue vo = new ItemValue();

                vo.Title = post.Title;
                vo.Created = post.Created;
                vo.CreatorName = post.Creator.Name;
                vo.Link = alink.ToAppData( post );
                vo.Content = post.Content;
                vo.Replies = post.Replies;
                vo.Category = post.Category.Name;

                results.Add( vo );
            }

            return results;
        }

        //--------------------------------------------------------------------------------------------------------------


        public virtual List<ShopItem> GetRecentByCategory(int appId, int count, int categoryId)
        {
            return db.find<ShopItem>("AppId=" + appId + " and CategoryId=" + categoryId + " and SaveStatus=" + SaveStatus.Normal).list(count);
        }

        public virtual List<ShopItem> GetRankByCategory(int appId, int count, int categoryId)
        {
            return db.find<ShopItem>("AppId=" + appId + " and CategoryId=" + categoryId + " and SaveStatus=" + SaveStatus.Normal + " order by Hits desc, Replies desc, Id desc").list(count);
        }

        public virtual List<ShopItem> GetRecentPost(int appId, int count, int typeId)
        {
            return db.find<ShopItem>("AppId=" + appId + " and MethodId=" + typeId + " and SaveStatus=" + SaveStatus.Normal).list(count);
        }

        public virtual List<ShopItem> GetRankPost(int appId, int count, int typeId)
        {
            return db.find<ShopItem>("AppId=" + appId + " and MethodId=" + typeId + " and SaveStatus=" + SaveStatus.Normal + " order by Hits desc, Replies desc, Id desc").list(count);
        }

        private ShopItem GetById( int ItemId ) {
            ShopItem post = db.findById<ShopItem>( ItemId );
            if (post.SaveStatus != SaveStatus.Normal) return null;
            return post;
        }

        public virtual ShopItem GetById( int id, int ownerId ) {
            ShopItem post = GetById( id );
            if (post == null) return null;
            if (post.OwnerId != ownerId) return null;
            if (post.SaveStatus != SaveStatus.Normal) return null;
            return post;
        }

        public virtual DataPage<ShopItem> GetByCategory(int appId, int categoryId)
        {

            ShopCategory c = new ShopCategoryService().GetById(categoryId);
            if (c.ParentId == 0)
            {

                List<ShopCategory> list = new ShopCategoryService().GetByParentId(c.Id);
                String ids = "";
                for (int i = 0; i < list.Count; i++)
                {
                    ids += list[i].Id;
                    if (i < list.Count - 1) ids += ",";
                }

                return ShopItem.findPage("AppId=" + appId + " and CategoryId in (" + ids + ")");

            }
            else
            {

                return ShopItem.findPage("AppId=" + appId + " and CategoryId=" + categoryId);
            }
        }

        public virtual DataPage<ShopItem> GetByCategory(int appId, int categoryId, int brandId)
        {

            ShopCategory c = new ShopCategoryService().GetById(categoryId);
            if (c.ParentId == 0)
            {

                List<ShopCategory> list = new ShopCategoryService().GetByParentId(c.Id);
                String ids = "";
                for (int i = 0; i < list.Count; i++)
                {
                    ids += list[i].Id;
                    if (i < list.Count - 1) ids += ",";
                }

                return ShopItem.findPage("AppId=" + appId + " and CategoryId in (" + ids + ")" + " and BrandId=" + brandId);

            }
            else
            {

                return ShopItem.findPage("AppId=" + appId + " and CategoryId=" + categoryId + " and BrandId=" + brandId);
            }
        }

        public virtual DataPage<ShopItem> GetByCategory(int appId, int categoryId, int brandId, int providerId)
        {

            ShopCategory c = new ShopCategoryService().GetById(categoryId);
            if (c.ParentId == 0)
            {

                List<ShopCategory> list = new ShopCategoryService().GetByParentId(c.Id);
                String ids = "";
                for (int i = 0; i < list.Count; i++)
                {
                    ids += list[i].Id;
                    if (i < list.Count - 1) ids += ",";
                }

                return ShopItem.findPage("AppId=" + appId + " and CategoryId in (" + ids + ")" + " and BrandId=" + brandId + " and ProviderId=" + providerId);

            }
            else
            {

                return ShopItem.findPage("AppId=" + appId + " and CategoryId=" + categoryId + " and BrandId=" + brandId + " and ProviderId=" + providerId);
            }
        }

        public virtual DataPage<ShopItem> GetByProvider(int appId, int providerId)
        {
            return ShopItem.findPage("AppId=" + appId + " and ProviderId=" + providerId);
        }

        public virtual DataPage<ShopItem> GetByBrand(int appId, int brandId)
        {
            return ShopItem.findPage("AppId=" + appId + " and BrandId=" + brandId);
        }


        public virtual List<ShopItem> GetBySection( int sectionId ) {
            return db.find<ShopItem>( "PageSection.Id=" + sectionId + " and SaveStatus=" + SaveStatus.Normal + " order by Id desc" ).list();
        }

        public virtual DataPage<ShopItem> GetByApp( int appId, int pageSize ) {
            return ShopItem.findPage( "AppId=" + appId + " and SaveStatus=" + SaveStatus.Normal + " order by Id desc", pageSize );
        }



        public virtual DataPage<ShopItem> GetTrashByApp( int appId, int pageSize ) {
            return ShopItem.findPage( "AppId=" + appId + " and SaveStatus=" + SaveStatus.Delete + " order by Id desc", pageSize );
        }

        public virtual DataPage<ShopItem> GetBySearch( int appId, String key, int pageSize ) {
            return ShopItem.findPage( "AppId=" + appId + " and Title like '%" + key + "%' and SaveStatus=" + SaveStatus.Normal + " order by Id desc", pageSize );
        }

        public virtual List<ShopItem> GetBySection( List<ShopItem> dataAll, int sectionId ) {
            List<ShopItem> result = new List<ShopItem>();
            foreach (ShopItem post in dataAll) {
                if (post.PageSection.Id == sectionId) {
                    result.Add( post );
                }
            }

            result.Sort();
            return result;
        }


        public virtual DataPage<ShopItem> GetBySectionAndType(int sectionId, int typeId)
        {
            return GetBySectionAndType(sectionId, typeId, 20);
        }

        public virtual DataPage<ShopItem> GetBySectionAndType(int sectionId, int typeId, int pageSize)
        {
            if (typeId > 0)
            {
                return db.findPage<ShopItem>("PageSection.Id=" + sectionId + " and MethodId=" + typeId + " and SaveStatus=" + SaveStatus.Normal, pageSize);
            }
            return db.findPage<ShopItem>("PageSection.Id=" + sectionId + " and SaveStatus=" + SaveStatus.Normal, pageSize);
        }

        public virtual DataPage<ShopItem> GetBySectionAndCategory( int sectionId, int categoryId ) {
            return GetBySectionAndCategory( sectionId, categoryId, 20 );
        }

        public virtual DataPage<ShopItem> GetBySectionAndCategory( int sectionId, int categoryId, int pageSize ) {
            if (categoryId > 0) {
                return db.findPage<ShopItem>( "PageSection.Id=" + sectionId + " and CategoryId=" + categoryId + " and SaveStatus=" + SaveStatus.Normal, pageSize );
            }
            return db.findPage<ShopItem>( "PageSection.Id=" + sectionId + " and SaveStatus=" + SaveStatus.Normal, pageSize );
        }

        public virtual DataPage<ShopItem> GetPageBySection( int sectionId ) {
            return db.findPage<ShopItem>( "PageSection.Id=" + sectionId + " and SaveStatus=" + SaveStatus.Normal );
        }

        public virtual DataPage<ShopItem> GetPageBySection( int sectionId, int pageSize ) {
            return db.findPage<ShopItem>( "PageSection.Id=" + sectionId + " and SaveStatus=" + SaveStatus.Normal, pageSize );
        }

        public virtual DataPage<ShopItem> GetPageBySectionAndCategory(int sectionId, int categoryId)
        {
            return db.findPage<ShopItem>("PageSection.Id=" + sectionId + " and CategoryId=" + categoryId + " and SaveStatus=" + SaveStatus.Normal);
        }

        public virtual DataPage<ShopItem> GetPageBySectionAndType(int sectionId, int typeId)
        {
            return db.findPage<ShopItem>("PageSection.Id=" + sectionId + " and MethodId=" + typeId+ " and SaveStatus=" + SaveStatus.Normal);
        }

        public virtual ShopItem GetFirstPost( int appId, int sectionId ) {
            ShopItem result = db.find<ShopItem>( "PageSection.Id=" + sectionId + " and SaveStatus=" + SaveStatus.Normal + " order by Id desc" ).first();
            if (result == null) return null;
            if (result.AppId != appId) return null;
            return result;
        }

        public virtual List<ShopItem> GetTopBySectionAndType(int sectionId, int typeId, int appId)
        {
            ShopSection s = ShopSection.findById(sectionId);

            return GetTopBySectionAndType(sectionId, typeId, appId, s.ListCount);
        }

        public virtual List<ShopItem> GetTopBySectionAndCategory(int sectionId, int categoryId, int appId)
        {
            ShopSection s = ShopSection.findById( sectionId );

            return GetTopBySectionAndCategory( sectionId, categoryId, appId, s.ListCount );
        }

        public virtual List<ShopItem> GetBySection( int appId, int sectionId ) {
            ShopSection s = ShopSection.findById( sectionId );
            return this.GetBySection( appId, sectionId, s.ListCount );
        }

        public virtual List<ShopItem> GetBySection( int appId, int sectionId, int count ) {

            List<ShopItem> list;

            // 兼容旧版
            ShopSection s = ShopSection.findById( sectionId );
            if (strUtil.IsNullOrEmpty( s.CacheIds )) {

                list = ShopItem.find( "AppId=" + appId + " and PageSection.Id=" + sectionId + " and SaveStatus=" + SaveStatus.Normal + " order by Id desc" ).list( count );
            }
            else {
                list = ShopItem.find( "Id in (" + s.CacheIds + ")" ).list();
            }

            list.Sort();
            return list;
        }

        public virtual List<ShopItem> GetTopBySectionAndCategory( int sectionId, int categoryId, int appId, int count ) {

            List<ShopItem> list;

            // 兼容旧版
            ShopSection s = ShopSection.findById( sectionId );
            if (strUtil.IsNullOrEmpty( s.CacheIds )) {

                list = ShopItem.find( "AppId=" + appId + " and PageSection.Id=" + sectionId + " and CategoryId=" + categoryId + " and SaveStatus=" + SaveStatus.Normal + " order by Id desc" ).list( count );

            }
            else {

                //list = ShopItem.findBySql( "select top " + count + " from ShopItem a, ShopSection b where a.AppId=" + appId + " and a.SectionId=" + sectionId + " and a.CategoryId=" + categoryId + " and a.SaveStatus=" + SaveStatus.Normal + " and b.SectionId=" + sectionId );

                list = new List<ShopItem>();
                List<ShopItem> posts = ShopItem.find( "Id in (" + s.CacheIds + ")" ).list();
                foreach (ShopItem p in posts) {
                    if (p.CategoryId == categoryId) list.Add( p );
                }

            }

            list.Sort();

            return list;
        }

        public virtual List<ShopItem> GetTopBySectionAndType(int sectionId, int typeId, int appId, int count)
        {

            List<ShopItem> list;

            // 兼容旧版
            ShopSection s = ShopSection.findById(sectionId);
            if (strUtil.IsNullOrEmpty(s.CacheIds))
            {

                list = ShopItem.find("AppId=" + appId + " and PageSection.Id=" + sectionId + " and MethodId=" + typeId + " and SaveStatus=" + SaveStatus.Normal + " order by Id desc").list(count);
            }
            else
            {

                //list = ShopItem.findBySql( "select top " + count + " from ShopItem a, ShopSection b where a.AppId=" + appId + " and a.SectionId=" + sectionId + " and a.MethodId=" + typeId + " and a.SaveStatus=" + SaveStatus.Normal + " and b.SectionId=" + sectionId );

                list = new List<ShopItem>();
                List<ShopItem> posts = ShopItem.find("Id in (" + s.CacheIds + ")").list();
                foreach (ShopItem p in posts)
                {
                    if (p.MethodId == typeId) list.Add(p);
                }

            }

            list.Sort();

            return list;
        }

        //--------------------------------------------------------------------------------------------------

        public virtual void Insert( ShopItem post, String tagList ) {

            if (db.insert( post ).IsValid) post.Tag.Save( tagList );
        }

        public virtual void Insert( ShopItem post, string sectionIds, string tagList ) {

            int[] ids = cvt.ToIntArray( sectionIds );

            post.PageSection = new ShopSection(); // 不再使用旧版一对多关系
            post.PageSection.Id = ids[0];
            post.insert();


            // 多对多处理
            ShopItemSection ps = new ShopItemSection();
            foreach (int sectionId in ids) {

                ShopSection section = new ShopSection();
                section.Id = sectionId;

                ps.Item = post;
                ps.Section = section;
                ps.insert();
            }

            // 旧版一对多兼容处理
            foreach (int sectionId in ids) {
                updateCacheIds( sectionId );
            }

            post.Tag.Save( tagList );
        }

        private void updateCacheIds( int sectionId ) {

            ShopSection section = ShopSection.findById( sectionId );
            if (section == null) throw new NullReferenceException();

            // 为了兼容旧版，section和post一对多关系下的数据也要抓取
            List<ShopItemSection> list = ShopItemSection.find( "SectionId=" + sectionId ).list( section.ListCount );
            List<ShopItem> posts = ShopItem.find( "SectionId=" + sectionId ).list( section.ListCount );
            List<int> idList = getRecentIds( list, posts );

            section.CacheIds = getCacheIds( idList, section.ListCount );
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

        private List<int> getRecentIds( List<ShopItemSection> list, List<ShopItem> posts ) {

            List<int> idList = new List<int>();
            foreach (ShopItemSection ps in list) {
                if (idList.Contains(ps.Item.Id)) continue;
                idList.Add(ps.Item.Id);
            }

            foreach (ShopItem post in posts) {
                if (idList.Contains( post.Id )) continue;
                idList.Add( post.Id );
            }

            idList.Sort();
            idList.Reverse();

            return idList;
        }

        //----------------------------------------------------------------------------------------------------------

        public virtual void Update( ShopItem post, String tagList ) {
            if (db.update( post ).IsValid) {
                post.Tag.Save( tagList );
            }
        }

        public virtual void Update( ShopItem post, String sectionIds, String tagList ) {

            if (db.update( post ).IsValid) {
                post.Tag.Save( tagList );
            }

            List<ShopItemSection> oldPsList = ShopItemSection.find( "ItemId=" + post.Id ).list();

            // 多对多处理
            ShopItemSection.deleteBatch( "ItemId=" + post.Id );

            ShopItemSection ps = new ShopItemSection();
            int[] ids = cvt.ToIntArray( sectionIds );
            foreach (int sectionId in ids) {

                ShopSection section = new ShopSection();
                section.Id = sectionId;

                ps.Item = post;
                ps.Section = section;
                ps.insert();
            }

            post.PageSection = new ShopSection(); // 不再使用旧版一对多关系
            post.PageSection.Id = ids[0];
            post.update();


            // 更新旧的section关系
            foreach (ShopItemSection p in oldPsList) {
                updateCacheIds( p.Section.Id );
            }
            // 更新新的section关系
            foreach (int sectionId in ids) {
                updateCacheIds( sectionId );
            }

        }

        public virtual void UpdateSection( ShopItem post, int sectionId ) {
            ShopSection section = new ShopSection();
            section.Id = sectionId;
            post.PageSection = section;
            post.update();
        }

        public virtual void UpdateSection( String ids, int sectionId ) {

            ShopItem.updateBatch( "set SectionId=" + sectionId, "Id in (" + ids + ")" );
        }

        public virtual void UpdateTitleStyle( ShopItem post, string titleStyle ) {
            post.Style = titleStyle;
            post.update( "Style" );
        }

        public virtual void Delete( ShopItem post ) {
            post.SaveStatus = SaveStatus.Delete;
            post.update();
        }

        public virtual void Restore( int id ) {
            ShopItem post = ShopItem.findById( id );
            if (post == null) return;
            post.SaveStatus = SaveStatus.Normal;
            post.update();
        }
        public virtual void DeleteTrue( int ItemId ) {
            ShopItem post = ShopItem.findById( ItemId );
            if (post == null) return;
            post.SaveStatus = SaveStatus.SysDelete;
            post.update();
        }

        public virtual void AddHits( ShopItem post ) {
            HitsJob.Add( post );
        }

        public virtual void UpdateAttachmentPermission( ShopItem post, int ischeck ) {
            post.IsAttachmentLogin = ischeck;
            post.update( "IsAttachmentLogin" );
        }

        public virtual void DeleteBatch( string ids ) {
            ShopItem.deleteBatch( "Id in (" + ids + ")" );
        }

        public virtual void SetStatus_Pick( string ids ) {
            ShopItem.updateBatch( "set PickStatus=" + PickStatus.Picked, "Id in (" + ids + ")" );
        }
        public virtual void SetStatus_Normal( string ids ) {
            ShopItem.updateBatch( "set PickStatus=" + PickStatus.Normal, "Id in (" + ids + ")" );
        }
        public virtual void SetStatus_Focus( string ids ) {
            ShopItem.updateBatch( "set PickStatus=" + PickStatus.Focus, "Id in (" + ids + ")" );
        }
        public virtual void SetOnSale(string ids)
        {
            ShopItem.updateBatch("set IsSale=1", "Id in (" + ids + ")");
        }
        public virtual void SetUnSale(string ids)
        {
            ShopItem.updateBatch("set IsSale=0", "Id in (" + ids + ")");
        }


    }


}

