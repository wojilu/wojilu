using System;
using wojilu.Apps.Photo.Domain;
using wojilu.Members.Users.Domain;

namespace wojilu.Apps.Photo.Interface {

    public interface IPhotoLikeService {

        bool IsLiked( int userId, int id );

        PhotoLike GetOne( int userId, int postId );

        DataPage<PhotoPost> GetByUser( int userId, int pageSize );

        void Like( User user, PhotoPost post );
        void UnLike( User user, PhotoPost post );

    }

}
