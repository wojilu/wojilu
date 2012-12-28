/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using System.Text;
using wojilu.Common.AppBase;

namespace wojilu.Apps.Content.Service {

    public class ContentSectionService : IContentSectionService {

        public virtual List<ContentSection> GetByApp( int appId ) {
            return db.find<ContentSection>( "AppId=" + appId + " order by OrderId asc, Id asc" ).list();
        }

        public virtual List<ContentSection> GetInputSectionsByApp( int appId ) {
            return db.find<ContentSection>( "AppId=" + appId + " and ServiceId=0 order by OrderId asc, Id asc" ).list();
        }


        public virtual ContentSection GetById( int id ) {
            return db.findById<ContentSection>( id );
        }

        public virtual ContentSection GetById( int id, int appId ) {
            ContentSection s = GetById( id );
            if (s.AppId != appId) return null;
            return s;
        }

        public virtual List<ContentSection> GetByRowColumn( List<ContentSection> list, int rowId, int columnId ) {
            List<ContentSection> results = new List<ContentSection>();
            foreach (ContentSection section in list) {
                if ((section.RowId == rowId) && (section.ColumnId == columnId)) {
                    results.Add( section );
                }
            }
            return results;
        }

        public virtual String GetSectionIdsByPost( int postId ) {

            List<ContentPostSection> list = ContentPostSection.find( "PostId=" + postId ).list();

            if (list.Count == 0) {
                ContentPost post = ContentPost.findById( postId );
                if (post == null) return "";
                if (post.PageSection == null) return "";
                return post.PageSection.Id.ToString();
            }

            String ids = "";
            foreach (ContentPostSection ps in list) {
                ids += ps.Section.Id + ",";
            }

            return ids.TrimEnd( ',' );
        }

        public virtual void Insert( ContentSection section ) {
            db.insert( section );
        }

        public virtual void Update( ContentSection section ) {
            db.update( section );
        }

        public virtual void Delete( ContentSection section ) {
            db.delete( section );
            ContentPost.updateBatch( "SaveStatus=" + SaveStatus.Delete + ", SectionId=0", "SectionId=" + section.Id );
        }

        public virtual int Count( int appId, int rowId ) {
            return db.count<ContentSection>( "AppId=" + appId + " and RowId=" + rowId );
        }

        public virtual void CombineSections( int sectionId, int targetSectionId ) {

            ContentSection target = GetById( targetSectionId );
            if (strUtil.HasText( target.CombineIds ) && isInIds( target, sectionId ) == false) {
                target.CombineIds += "," + sectionId;
            }
            else
                target.CombineIds = sectionId.ToString();

            target.update( "CombineIds" );
        }

        private Boolean isInIds( ContentSection target, int sectionId ) {

            int[] arrIds = cvt.ToIntArray( target.CombineIds );
            for (int i = 0; i < arrIds.Length; i++) {
                if (arrIds[i] == sectionId) return true;
            }
            return false;
        }

        public virtual void RemoveSection( int sectionId, int fromSectionId ) {
            ContentSection from = GetById( fromSectionId );
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

        public virtual List<ContentSection> GetForCombine( ContentSection section ) {
            List<ContentSection> sections = GetByApp( section.AppId );
            List<ContentSection> list = new List<ContentSection>();
            foreach (ContentSection s in sections) {
                if (s.Id != section.Id && notCombined( sections, s )) list.Add( s );
            }
            return list;
        }

        private Boolean notCombined( List<ContentSection> sections, ContentSection section ) {
            foreach (ContentSection s in sections) {
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

