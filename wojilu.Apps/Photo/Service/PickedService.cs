/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Apps.Photo.Domain;
using wojilu.Apps.Photo.Interface;
using wojilu.Common.AppBase;

namespace wojilu.Apps.Photo.Service {

    public class PickedService : IPickedService {

        public virtual Boolean IsPicked( PhotoPost post ) {

            PhotoPostPicked pickedPost = GetByPost( post );
            return (pickedPost != null);
        }

        public virtual PhotoPostPicked GetByPost( PhotoPost post ) {
             return db.find<PhotoPostPicked>( "Post.Id=" + post.Id ).first();
        }


        public virtual void PickPost( String ids ) {

            List<PhotoPost> posts = db.find<PhotoPost>( "Id in (" + ids + ")" ).list();
            foreach (PhotoPost post in posts) {
                pickOnePost( post );
            }

        }

        private void pickOnePost( PhotoPost post ) {

            PhotoPostPicked pickedPost = GetByPost( post );
            if (pickedPost == null) {
                pickedPost = new PhotoPostPicked();
                pickedPost.Post = post;
                pickedPost.Status = SystemPickStatus.Picked;
                db.insert( pickedPost );
            }

            else {
                pickedPost.Status = SystemPickStatus.Picked;
                db.update( pickedPost );
            }
        }

        public virtual void UnPickPost( String ids ) {
            List<PhotoPost> posts = db.find<PhotoPost>( "Id in (" + ids + ")" ).list();
            foreach (PhotoPost post in posts) {
                unPickOnePost( post );
            }
        }

        private void unPickOnePost( PhotoPost post ) {
            PhotoPostPicked pickedPost = GetByPost( post );
            if (pickedPost != null) {
                db.delete( pickedPost );
            }
        }

        public virtual DataPage<PhotoPost> GetAll() {
            return this.GetAll( 20 ) ;
        }

        public virtual DataPage<PhotoPost> GetAll( int pageSize ) {

            DataPage<PhotoPostPicked> list = db.findPage<PhotoPostPicked>( "", pageSize );
            DataPage<PhotoPost> r = new DataPage<PhotoPost>();
            r.Results = populatePosts( list.Results );
            r.PageCount = list.PageCount;
            r.RecordCount = list.RecordCount;
            r.Size = list.Size;
            r.Current = list.Current;
            r.PageBar = list.PageBar;
            return r;
        }

        public virtual DataPage<PhotoPost> GetShowAll( int pageSize ) {
            DataPage<PhotoPost> list = this.GetAll( pageSize );
            list.Results = filterBySysCategory( list.Results );
            return list;
        }

        private List<PhotoPost> filterBySysCategory( List<PhotoPost> list ) {
            List<PhotoPost> xlist = new List<PhotoPost>();
            foreach (PhotoPost x in list) {
                if (x.SysCategoryId > 0) xlist.Add( x );
            }
            return xlist;
        }

        public virtual List<PhotoPost> GetTop( int count ) {

            List<PhotoPostPicked> picked = db.find<PhotoPostPicked>( "order by Id desc" ).list( count );

            return populatePosts( picked );
        }

        private List<PhotoPost> populatePosts( List<PhotoPostPicked> picked ) {
            List<PhotoPost> results = new List<PhotoPost>();
            foreach (PhotoPostPicked pick in picked) {
                if (pick.Post == null) continue;
                if (strUtil.IsNullOrEmpty( pick.Post.ImgUrl )) continue;
                results.Add( pick.Post );
            }
            return results;
        }



        public virtual void DeletePhoto( String ids ) {
            db.deleteBatch<PhotoPostPicked>( " PostId in (" + ids + ")" );
        }

    }

}
