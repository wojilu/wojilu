/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Common.AppBase;
using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Interface;

namespace wojilu.Apps.Blog.Service {

    public class PickedService : IPickedService {

        public virtual Boolean IsPicked( BlogPost post ) {

            BlogPostPicked pickedPost = GetByPost( post );
            return (pickedPost != null);
        }

        public virtual BlogPostPicked GetByPost( BlogPost post ) {
            return db.find<BlogPostPicked>( "Post.Id=" + post.Id ).first();
        }


        public virtual void PickPost( String ids ) {

            List<BlogPost> posts = db.find<BlogPost>( "Id in (" + ids + ")" ).list();
            foreach (BlogPost post in posts) {
                pickOnePost( post );
            }

        }

        private void pickOnePost( BlogPost post ) {

            BlogPostPicked pickedPost = GetByPost( post );
            if (pickedPost == null) {
                pickedPost = new BlogPostPicked();
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
            List<BlogPost> posts = db.find<BlogPost>( "Id in (" + ids + ")" ).list();
            foreach (BlogPost post in posts) {
                unPickOnePost( post );
            }
        }

        private void unPickOnePost( BlogPost post ) {
            BlogPostPicked pickedPost = GetByPost( post );
            if (pickedPost != null) {
                db.delete( pickedPost );
            }
        }

        public virtual DataPage<BlogPost> GetAll() {

            DataPage<BlogPostPicked> list = db.findPage<BlogPostPicked>( "" );

            DataPage<BlogPost> results = new DataPage<BlogPost>();
            results.Results = populatePosts( list.Results );

            results.Current = list.Current;
            results.Size = list.Size;
            results.PageCount = list.PageCount;
            results.PageBar = list.PageBar;
            results.RecordCount = list.RecordCount;

            return results;
        }

        public virtual List<BlogPost> GetTop( int count ) {

            List<BlogPostPicked> picked = db.find<BlogPostPicked>( "" ).list( count );
            return populatePosts( picked );
        }

        private static List<BlogPost> populatePosts( List<BlogPostPicked> picked ) {
            List<BlogPost> results = new List<BlogPost>();
            foreach (BlogPostPicked pick in picked) {
                if (pick.Post == null || strUtil.IsNullOrEmpty( pick.Post.Title )) continue;
                results.Add( pick.Post );
            }
            return results;
        }


    }

}
