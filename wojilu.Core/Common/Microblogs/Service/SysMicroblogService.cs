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
    public interface ISysMicroblogService {
        List<Microblog> GetRecent( int count );
        List<Microblog> GetByReplies( int count );
        List<IBinderValue> GetRecentMicroblog( int count );
        List<IBinderValue> GetMicroblogByReplies( int count );
        DataPage<Microblog> GetPageAllByUser( long userId, int pageSize );
        DataPage<Microblog> GetPageAll( int pageSize );
        DataPage<Microblog> GetPicPageAll( int pageSize );
        DataPage<Microblog> GetPageByCondition( String condition );
        DataPage<Microblog> GetPageByCondition( String condition, int pageSize );
        DataPage<Microblog> GetSysTrashPage( int pageSize );
        void DeleteSys( Microblog blog );
        void DeleteSysBatch( string ids );
        void RestoreSysBatch( string ids );
        void DeleteTrue( Microblog blog );
        void DeleteTrueBatch( string ids );
    }

    public class SysMicroblogService : ISysMicroblogService {

        private static String showCondition() {
            return " (SaveStatus=" + SaveStatus.Normal + " or SaveStatus=" + SaveStatus.Private + ")";
        }
        public virtual List<Microblog> GetRecent( int count ) {
            if (count <= 0) count = 10;
            return Microblog.find( showCondition() + " order by Id desc" ).list( count );
        }

        public virtual List<Microblog> GetByReplies( int count ) {
            if (count <= 0) count = 10;
            return Microblog.find( showCondition() + " order by Replies desc, Id desc" ).list( count );
        }

        public virtual List<IBinderValue> GetRecentMicroblog( int count ) {
            return populatePost( GetRecent( count ) );
        }

        public virtual List<IBinderValue> GetMicroblogByReplies( int count ) {
            return populatePost( GetByReplies( count ) );
        }

        //------------------------------------------------------------------------------------------

        public virtual DataPage<Microblog> GetPageAllByUser( long userId, int pageSize ) {

            if (userId <= 0) return GetPageAll( pageSize );

            DataPage<Microblog> list = Microblog.findPage( "UserId=" + userId + " and " + showCondition(), pageSize );
            return list;
        }

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

            long[] arrIds = cvt.ToLongArray( ids );
            if (arrIds.Length == 0) return;

            Microblog.updateBatch( "SaveStatus=" + SaveStatus.SysDelete, "id in (" + ids + ")" );
        }


        public virtual void RestoreSysBatch( string ids ) {
            long[] arrIds = cvt.ToLongArray( ids );
            if (arrIds.Length == 0) return;

            Microblog.updateBatch( "SaveStatus=" + SaveStatus.Normal, "id in (" + ids + ")" );
        }

        public virtual void DeleteTrue( Microblog blog ) {
            blog.delete();
        }

        public virtual void DeleteTrueBatch( string ids ) {

            long[] arrIds = cvt.ToLongArray( ids );
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
