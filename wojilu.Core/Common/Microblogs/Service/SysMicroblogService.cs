/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Microblogs.Domain;
using wojilu.Web.Mvc;
using wojilu.Common.AppBase;

namespace wojilu.Common.Microblogs.Service {

    public class SysMicroblogService {

        private static String showCondition() {
            return " SaveStatus=" + SaveStatus.Normal;
        }
        public List<Microblog> GetRecent( int count ) {
            if (count <= 0) count = 10;
            return Microblog.find( showCondition() + " order by Id desc" ).list( count );
        }

        public List<Microblog> GetByReplies( int count ) {
            if (count <= 0) count = 10;
            return Microblog.find( showCondition() + " order by Replies desc, Id desc" ).list( count );
        }

        public List<IBinderValue> GetRecentMicroblog( int count ) {
            return populatePost( GetRecent( count ) );
        }

        public List<IBinderValue> GetMicroblogByReplies( int count ) {
            return populatePost( GetByReplies( count ) );
        }

        //------------------------------------------------------------------------------------------

        public virtual DataPage<Microblog> GetPageAll( int pageSize ) {

            DataPage<Microblog> list = Microblog.findPage( showCondition(), pageSize );
            return list;
        }

        public virtual DataPage<Microblog> GetPicPageAll( int pageSize ) {

            DataPage<Microblog> list = Microblog.findPage( "Pic <>'' and " + showCondition(), pageSize );
            return list;
        }

        public virtual DataPage<Microblog> GetPageByCondition( String condition ) {
            return GetPageByCondition( condition, 20 );
        }

        public virtual DataPage<Microblog> GetPageByCondition( String condition, int pageSize ) {
            DataPage<Microblog> list;
            if (strUtil.HasText( condition )) {

                String strCondition = showCondition();
                if (condition.Trim().ToLower().StartsWith( "order" )) {
                    strCondition = strCondition + " " + condition;
                }
                else if (condition.Trim().ToLower().StartsWith( "and" )) {
                    strCondition = strCondition + " " + condition;
                }
                else {
                    strCondition = strCondition + " and " + condition;
                }

                list = db.findPage<Microblog>( strCondition, pageSize );
            }
            else {
                list = DataPage<Microblog>.GetEmpty();
            }
            return list;
        }

        //------------------------------------------------------------------------------------------

        public virtual DataPage<Microblog> GetSysTrashPage( int pageSize ) {

            return Microblog.findPage( "SaveStatus=" + SaveStatus.SysDelete, pageSize );

        }


        //------------------------------------------------------------------------------------------

        public virtual void DeleteSys( Microblog blog ) {

            if (blog == null) throw new ArgumentNullException( "blog" );

            blog.SaveStatus = SaveStatus.SysDelete;
            blog.delete();

        }

        public virtual void DeleteSysBatch( string ids ) {

            int[] arrIds = cvt.ToIntArray( ids );
            if (arrIds.Length == 0) return;

            Microblog.updateBatch( "SaveStatus=" + SaveStatus.SysDelete, "id in (" + ids + ")" );
        }


        public void RestoreSysBatch( string ids ) {
            int[] arrIds = cvt.ToIntArray( ids );
            if (arrIds.Length == 0) return;

            Microblog.updateBatch( "SaveStatus=" + SaveStatus.Normal, "id in (" + ids + ")" );
        }

        public virtual void DeleteTrue( Microblog blog ) {
            blog.delete();
        }

        public virtual void DeleteTrueBatch( string ids ) {

            int[] arrIds = cvt.ToIntArray( ids );
            if (arrIds.Length == 0) return;

            Microblog.deleteBatch( "id in (" + ids + ")" );
        }

        //------------------------------------------------------------------------------------------

        internal static List<IBinderValue> populatePost( List<Microblog> list ) {

            List<IBinderValue> results = new List<IBinderValue>();

            foreach (Microblog post in list) {

                IBinderValue vo = new ItemValue();

                vo.CreatorName = post.User.Name;
                vo.CreatorLink = alink.ToUserMicroblog( post.User );
                vo.CreatorPic = post.User.PicSmall;

                vo.Title = strUtil.ParseHtml( post.Content, 500 );
                vo.Link = alink.ToUserMicroblog( post.User );

                vo.Created = post.Created;
                vo.Replies = post.Replies;

                results.Add( vo );
            }

            return results;
        }

    }

}
