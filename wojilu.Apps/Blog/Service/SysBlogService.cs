/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Common.AppBase;
using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Interface;
using wojilu.Common;

namespace wojilu.Apps.Blog.Service {

    public class SysBlogService : ISysBlogService {

        public virtual List<BlogPost> GetByCategory( int categoryId, int count ) {
            return db.find<BlogPost>( "SaveStatus=" + SaveStatus.Normal + " and SysCategoryId=" + categoryId ).list( count );
        }

        //------------------------------------------ 系统数据操作 --------------------------------------------------

        public virtual List<BlogPost> GetSysHit( int count ) {
            if (count <= 0) count = 10;
            return db.find<BlogPost>( "SaveStatus=" + SaveStatus.Normal + " order by Hits desc, Id desc" ).list( count );
        }

        public virtual List<BlogPost> GetSysReply( int count ) {
            if (count <= 0) count = 10;
            return db.find<BlogPost>( "SaveStatus=" + SaveStatus.Normal + " order by Replies desc, Hits desc, Id desc" ).list( count );
        }

        public virtual List<IBinderValue> GetPostByHits( int count ) {
            return getResult( GetSysHit( count ) );
        }

        public virtual List<IBinderValue> GetPostByReply( int count ) {
            return getResult( GetSysReply( count ) );
        }

        //----------------------------

        public List<BlogPost> GetSysNew( int categoryId, int count ) {
            return db.find<BlogPost>( this.getCondition( categoryId ) ).list( count );
        }

        private String getCondition( int categoryId ) {
            String str = "SaveStatus=" + SaveStatus.Normal;
            if (categoryId > 0) {
                str = "SysCategoryId=" + categoryId + " and " + str;
            }
            return str;
        }



        public virtual DataPage<BlogPost> GetSysPage( int size ) {
            return db.findPage<BlogPost>( "SaveStatus=" + SaveStatus.Normal + "" );
        }

        public virtual DataPage<BlogPost> GetSysPageByCategory( int categoryId, int size ) {
            if (categoryId <= 0) {
                return db.findPage<BlogPost>( "SaveStatus=" + SaveStatus.Normal );
            }
            return db.findPage<BlogPost>( "SaveStatus=" + SaveStatus.Normal + " and SysCategoryId=" + categoryId );
        }

        public virtual DataPage<BlogPost> GetSysPageBySearch( String condition ) {
            return db.findPage<BlogPost>( "SaveStatus=" + SaveStatus.Normal + " and " + condition );
        }

        public DataPage<BlogPost> GetSysPageTrash() {
            return db.findPage<BlogPost>( "SaveStatus=" + (int)SaveStatus.SysDelete );
        }




        public virtual void SystemDelete( BlogPost post ) {
            if (post == null) return;
            post.SaveStatus = SaveStatus.SysDelete;
            db.update( post, "SaveStatus" );
        }

        public virtual void SystemUnDelete( BlogPost post ) {
            if (post == null) return;
            post.SaveStatus = SaveStatus.Normal;
            db.update( post, "SaveStatus" );
        }

        public virtual int GetSystemDeleteCount() {
            return db.count<BlogPost>( "SaveStatus=" + SaveStatus.SysDelete );
        }

        public void Delete( string ids ) {
            if (strUtil.IsNullOrEmpty( ids )) return;
            BlogPost.updateBatch( "SaveStatus=" + SaveStatus.SysDelete, "Id in (" + ids + ")" );
        }

        public void DeleteTrue( string ids ) {
            if (strUtil.IsNullOrEmpty( ids )) return;
            BlogPost.deleteBatch( "Id in (" + ids + ")" );
        }

        public void UnDelete( string ids ) {
            if (strUtil.IsNullOrEmpty( ids )) return;
            BlogPost.updateBatch( "SaveStatus=" + SaveStatus.Normal, "Id in (" + ids + ")" );
        }

        internal static List<IBinderValue> getResult( List<BlogPost> list ) {

            List<IBinderValue> results = new List<IBinderValue>();

            foreach (BlogPost post in list) {


                IBinderValue vo = new ItemValue();

                vo.CreatorName = post.Creator.Name;
                vo.CreatorLink = Link.ToUser( post.CreatorUrl );
                vo.CreatorPic = post.Creator.PicSmall;

                vo.Title = post.Title;
                vo.Link = alink.ToAppData( post );
                vo.Content = post.Content;
                vo.Created = post.Created;
                vo.Replies = post.Replies;

                results.Add( vo );
            }

            return results;
        }

    }
}
