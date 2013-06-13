/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Apps.Photo.Domain;
using wojilu.Common.AppBase;
using wojilu.Apps.Photo.Interface;
using wojilu.Common;
using System;

namespace wojilu.Apps.Photo.Service {

    public class SysPhotoService : ISysPhotoService {

        public virtual IPickedService pickedService { get; set; }
        public virtual IPhotoPostService postService { get; set; }

        public SysPhotoService() {
            pickedService = new PickedService();
            postService = new PhotoPostService();
        }
        public virtual DataPage<PhotoPost> GetSysPostPage( int categoryId, int pageSize ) {

            if (categoryId <= 0) {
                return db.findPage<PhotoPost>( "SaveStatus=" + SaveStatus.Normal, pageSize );
            }
            else {
                return db.findPage<PhotoPost>( "SaveStatus=" + SaveStatus.Normal + " and SysCategoryId=" + categoryId, pageSize );
            }
        }

        public virtual DataPage<PhotoPost> GetSysPostTrashPage( int pageSize ) {
            return db.findPage<PhotoPost>( "SaveStatus=" + SaveStatus.SysDelete, pageSize );
        }

        public virtual void SystemDelete( PhotoPost post ) {
            if (post == null) return;
            post.SaveStatus = (int)SaveStatus.SysDelete;
            db.update( post, "SaveStatus" );
        }


        public virtual void SystemUnDelete( PhotoPost post ) {
            if (post == null) return;
            post.SaveStatus = (int)SaveStatus.Normal;
            db.update( post, "SaveStatus" );
        }


        public virtual void SystemDeleteList( string ids ) {
            int[] arrIds = cvt.ToIntArray( ids );
            if (arrIds.Length == 0) return;
            String condition = string.Format( "Id in ({0}) ", ids );
            PhotoPost.updateBatch( "set SaveStatus=" + SaveStatus.SysDelete, condition );
        }

        public virtual void SystemUnDeleteList( string ids ) {
            int[] arrIds = cvt.ToIntArray( ids );
            if (arrIds.Length == 0) return;
            String condition = string.Format( "Id in ({0}) ", ids );
            PhotoPost.updateBatch( "set SaveStatus=" + SaveStatus.Normal, condition );
        }

        public virtual void SystemDeleteListTrue( string ids ) {

            int[] arrIds = cvt.ToIntArray( ids );
            if (arrIds.Length == 0) return;

            String condition = string.Format( "Id in ({0})", ids );
            List<PhotoPost> list = db.find<PhotoPost>( condition ).list();

            postService.DeletePosts( ids, list );
        }

        private bool noRepins( PhotoPost post ) {
            return PhotoPost.find( "RootId=" + post.Id ).first() == null;
        }

        public virtual int GetSystemDeleteCount() {
            return db.count<PhotoPost>( "SaveStatus=" + SaveStatus.SysDelete );
        }

        public virtual List<PhotoPost> GetSysTop( int categoryId, int count ) {
            if (count <= 0) count = 10;
            return db.find<PhotoPost>( "SaveStatus=" + SaveStatus.Normal + " and SysCategoryId=" + categoryId ).list( count );
        }

        public virtual List<PhotoPost> GetSysHits( int count ) {
            if (count <= 0) count = 10;
            return db.find<PhotoPost>( "SaveStatus=" + SaveStatus.Normal + " order by Hits desc, Id desc" ).list( count );
        }

        public virtual List<PhotoPost> GetSysReply( int count ) {
            if (count <= 0) count = 10;
            return db.find<PhotoPost>( "SaveStatus=" + SaveStatus.Normal + " order by Replies desc, Hits desc, Id desc" ).list( count );
        }


        // service

        public virtual List<IBinderValue> GetByReply( int count ) {

            return populatePhoto( GetSysReply( count ) );
        }

        public virtual List<IBinderValue> GetByHits( int count ) {
            return populatePhoto( GetSysHits( count ) );
        }

        public virtual List<IBinderValue> GetNewAll( int count ) {

            if (count <= 0) count = 10;
            List<PhotoPost> posts = db.find<PhotoPost>( "SaveStatus=" + SaveStatus.Normal + " order by Id desc" ).list( count );

            return populatePhoto( posts );
        }

        internal static List<IBinderValue> populatePhoto( List<PhotoPost> posts ) {

            List<IBinderValue> results = new List<IBinderValue>();

            foreach (PhotoPost post in posts) {

                IBinderValue vo = new ItemValue();

                vo.Title = post.Title;
                vo.PicUrl = post.ImgThumbUrl;
                vo.Link = alink.ToAppData( post );

                vo.CreatorName = post.Creator.Name;
                vo.CreatorLink = Link.ToMember( post.Creator );
                vo.CreatorPic = post.Creator.PicSmall;

                vo.Created = post.Created;
                vo.Replies = post.Replies;

                vo.Summary = post.Description;


                results.Add( vo );
            }
            return results;
        }




        public DataPage<PhotoPost> GetShowRecent( int pageSize ) {
            return PhotoPost.findPage( "SysCategoryId>0 and SaveStatus=" + SaveStatus.Normal, pageSize );
        }

        public DataPage<PhotoPost> GetShowByCategory( int categoryId, int pageSize ) {

            return db.findPage<PhotoPost>( "SaveStatus=" + SaveStatus.Normal + " and SysCategoryId=" + categoryId, pageSize );
        }

        public DataPage<PhotoPost> GetShowHot( int pageSize ) {
            return PhotoPost.findPage( "SysCategoryId>0 and SaveStatus=" + SaveStatus.Normal + " order by Likes desc, Pins desc, Hits desc, Replies desc", pageSize );
        }


    }
}

