using System;
using wojilu.Apps.Photo.Domain;
using wojilu.Members.Users.Domain;

namespace wojilu.Apps.Photo.Interface {

    public interface IPhotoLikeService {

        bool IsLiked(long userId, long id);

        PhotoLike GetOne(long userId, long postId);

        DataPage<PhotoPost> GetByUser(long userId, int pageSize);

        void Like( User user, PhotoPost post );
        void UnLike( User user, PhotoPost post );

    }

}
