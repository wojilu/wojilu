/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Apps.Shop.Domain;
using wojilu.Apps.Shop.Interface;
using System.Text;
using wojilu.Common.AppBase;

namespace wojilu.Apps.Shop.Service {

    public class ShopSectionService : IShopSectionService {

        public virtual List<ShopSection> GetByApp( int appId ) {
            return db.find<ShopSection>( "AppId=" + appId + " order by OrderId asc, Id asc" ).list();
        }

        public virtual List<ShopSection> GetInputSectionsByApp( int appId ) {
            return db.find<ShopSection>( "AppId=" + appId + " and ServiceId=0 order by OrderId asc, Id asc" ).list();
        }


        private ShopSection GetById( int id ) {
            return db.findById<ShopSection>( id );
        }

        public virtual ShopSection GetById( int id, int appId ) {
            ShopSection s = GetById( id );
            if (s.AppId != appId) return null;
            return s;
        }

        public virtual List<ShopSection> GetByRowColumn( List<ShopSection> list, int rowId, int columnId ) {
            List<ShopSection> results = new List<ShopSection>();
            foreach (ShopSection section in list) {
                if ((section.RowId == rowId) && (section.ColumnId == columnId)) {
                    results.Add( section );
                }
            }
            return results;
        }

        public virtual String GetSectionIdsByPost( int ItemId ) {

            List<ShopItemSection> list = ShopItemSection.find( "ItemId=" + ItemId ).list();

            if (list.Count == 0) {
                ShopItem post = ShopItem.findById( ItemId );
                return post == null ? "" : post.PageSection.Id.ToString();
            }

            String ids = "";
            foreach (ShopItemSection ps in list) {
                ids += ps.Section.Id + ",";
            }

            return ids.TrimEnd( ',' );
        }

        public virtual void Insert( ShopSection section ) {
            db.insert( section );
        }

        public virtual void Update( ShopSection section ) {
            db.update( section );
        }

        public virtual void Delete( ShopSection section ) {
            db.delete( section );
            ShopItem.updateBatch( "SaveStatus=" + SaveStatus.Delete + ", SectionId=0", "SectionId=" + section.Id );
        }

        public virtual int Count( int appId, int rowId ) {
            return db.count<ShopSection>( "AppId=" + appId + " and RowId=" + rowId );
        }

        public virtual void CombineSections( int sectionId, int targetSectionId ) {

            ShopSection target = GetById( targetSectionId );
            if (strUtil.HasText( target.CombineIds ) && isInIds( target, sectionId ) == false) {
                target.CombineIds += "," + sectionId;
            }
            else
                target.CombineIds = sectionId.ToString();

            target.update( "CombineIds" );
        }

        private Boolean isInIds( ShopSection target, int sectionId ) {

            int[] arrIds = cvt.ToIntArray( target.CombineIds );
            for (int i = 0; i < arrIds.Length; i++) {
                if (arrIds[i] == sectionId) return true;
            }
            return false;
        }

        public virtual void RemoveSection( int sectionId, int fromSectionId ) {
            ShopSection from = GetById( fromSectionId );
            if (strUtil.IsNullOrEmpty( from.CombineIds )) return;
            if (isInIds( from, sectionId ) == false) return;

            int[] arrIds = cvt.ToIntArray( from.CombineIds );
            String result = "";
            for (int i = 0; i < arrIds.Length; i++) {
                if (arrIds[i] != sectionId) result += arrIds[i] + ",";
            }

            from.CombineIds = result.TrimEnd( ',' );
            from.update( "CombineIds" );
        }

        public virtual List<ShopSection> GetForCombine( ShopSection section ) {
            List<ShopSection> sections = GetByApp( section.AppId );
            List<ShopSection> list = new List<ShopSection>();
            foreach (ShopSection s in sections) {
                if (s.Id != section.Id && notCombined( sections, s )) list.Add( s );
            }
            return list;
        }

        private Boolean notCombined( List<ShopSection> sections, ShopSection section ) {
            foreach (ShopSection s in sections) {
                int[] arrIds = cvt.ToIntArray( s.CombineIds );
                if (arrIds.Length == 0) continue;

                foreach (int id in arrIds) {
                    if (id == section.Id) return false;
                }

            }
            return true;
        }

    }
}

