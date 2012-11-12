using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Photo.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Photo.Interface;

namespace wojilu.Apps.Photo.Service {

    public class PhotoLikeService : IPhotoLikeService {

        public bool IsLiked( int userId, int id ) {
            return this.GetOne( userId, id ) != null;
        }

        public PhotoLike GetOne( int userId, int postId ) {

            return PhotoLike.find( "PostId=:pid and UserId=:uid" )
                .set( "pid", postId )
                .set( "uid", userId )
                .first();

        }

        public void Like( User user, PhotoPost post ) {

            PhotoLike x = new PhotoLike();
            x.Post = post;
            x.User = user;

            x.insert();


            post.Likes = PhotoLike.count( "PostId=" + post.Id );
            post.update();


            user.Likes = PhotoLike.count( "UserId=" + user.Id );
            user.update( "Likes" );

        }

        public void UnLike( User user, PhotoPost post ) {

            PhotoLike p = this.GetOne( user.Id, post.Id );

            p.delete();

            post.Likes = PhotoLike.count( "PostId=" + post.Id );
            post.update();

            user.Likes = PhotoLike.count( "UserId=" + user.Id );
            user.update( "Likes" );

        }

        public DataPage<PhotoPost> GetByUser( int userId, int pageSize ) {

            DataPage<PhotoLike> list = PhotoLike.findPage( "UserId=" + userId, 12 );

            return getPostPage( list );

        }

        private DataPage<PhotoPost> getPostPage( DataPage<PhotoLike> list ) {

            DataPage<PhotoPost> results = new DataPage<PhotoPost>( list );
            List<PhotoPost> xlist = new List<PhotoPost>();
            foreach (PhotoLike x in list.Results) {
                xlist.Add( x.Post );
            }
            results.Results = xlist;
            return results;
        }


    }

}
