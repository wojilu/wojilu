/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Members.Interface;
using wojilu.Common.Pages.Domain;
using wojilu.Common.Pages.Interface;
using wojilu.Common.Msg.Service;
using wojilu.Common.Msg.Interface;
using wojilu.Common.Msg.Enum;
using wojilu.Web.Mvc;
using wojilu.Members.Users.Domain;

namespace wojilu.Common.Pages.Service {

    public class PageService : IPageService {

        public virtual List<PageCategory> GetCategories( IMember owner ) {
            return db.find<PageCategory>( "OwnerId=" + owner.Id + " and OwnerType=:otype order by OrderId desc, Id asc" )
                .set( "otype", owner.GetType().FullName )
                .list();
        }

        public virtual PageCategory GetCategoryById(long categoryId, IMember owner) {
            PageCategory category = db.findById<PageCategory>( categoryId );
            if (category != null) {
                if (category.OwnerType != owner.GetType().FullName || category.OwnerId != owner.Id) return null;
            }
            return category;
        }

        public virtual Page GetPostById(long id, IMember owner) {
            Page page = db.findById<Page>( id );
            if (page != null) {
                if (page.OwnerType != owner.GetType().FullName || page.OwnerId != owner.Id) return null;
            }
            return page;
        }

        public virtual List<Page> GetPosts(IMember owner, long categoryId) {

            if (categoryId <= 0) {
                return db.find<Page>( "OwnerId=:oid and OwnerType=:otype order by OrderId desc, Id asc" )
                 .set( "oid", owner.Id )
                 .set( "otype", owner.GetType().FullName )
                 .list();
            }


            return db.find<Page>( "OwnerId=:oid and OwnerType=:otype and Category.Id=:cid order by OrderId desc, Id asc" )
                .set( "oid", owner.Id )
                .set( "otype", owner.GetType().FullName )
                .set( "cid", categoryId )
                .list();
        }


        public virtual void AddHits( Page data ) {
            data.Hits = data.Hits + 1;
            db.update( data, "Hits" );
        }

        public virtual int GetPagesCount( IMember owner ) {

            return db.find<PageCategory>( "OwnerId=" + owner.Id + " and OwnerType=:otype" )
                .set( "otype", owner.GetType().FullName )
                .count();

        }

        public virtual DataPage<PageHistory> GetHistoryPage(long pageId, IMember owner, int pageSize) {
            Page p = GetPostById( pageId, owner );
            if (p == null) return DataPage<PageHistory>.GetEmpty();
            return PageHistory.findPage( "PageId=" + pageId, pageSize );
        }

        public virtual List<long> GetEditorIds(long pageId) {

            List<PageHistory> list = PageHistory.find( "PageId=" + pageId ).list();

            List<long> users = new List<long>();
            foreach (PageHistory ph in list) {
                if (users.Contains( ph.EditUser.Id )) continue;
                users.Add( ph.EditUser.Id );
            }

            return users;
        }


        public virtual PageHistory GetHistory(long id) {

            PageHistory ph = PageHistory.findById( id );
            return ph;
        }

        public virtual Result Insert( Page p ) {

            p.EditCount = 1;
            p.EditUser = p.Creator;
            Result result = p.insert();

            saveHistory( p );



            return result;
        }

        public virtual Result Update( Page p ) {
            p.Updated = DateTime.Now;
            Result result = p.update();
            if (result.HasErrors) return result;

            saveHistory( p );

            int count = PageHistory.count( "PageId=" + p.Id );
            p.EditCount = count;
            p.update();

            return result;
        }

        private static void saveHistory( Page p ) {
            PageHistory ph = new PageHistory();
            ph.PageId = p.Id;
            ph.EditReason = p.EditReason;
            ph.EditUser = p.EditUser;
            ph.Content = p.Content;
            ph.insert();
        }

        public virtual void Delete( Page p ) {
            PageHistory.deleteBatch( "PageId=" + p.Id );
            p.delete();
        }

        public virtual void UpdateCategory(string ids, long categoryId) {

            db.updateBatch<Page>( "CategoryId=" + categoryId, "Id in (" + ids + ")" );
        }



    }

}
