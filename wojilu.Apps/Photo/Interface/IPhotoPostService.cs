/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Apps.Photo.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Common;
using wojilu.Web.Context;

namespace wojilu.Apps.Photo.Interface {

    public interface IPhotoPostService {

        IFriendService friendService { get; set; }
        IPickedService pickedService { get; set; }

        void AddtHits( PhotoPost post );

        PhotoPost GetById(long id, long ownerId);
        PhotoPost GetById_Admin(long id);

        int GetCountByUser(long userId);
        PhotoPost GetFirst( PhotoPost post );
        List<IBinderValue> GetMyNew(int count, long userId);
        PhotoPost GetNext( PhotoPost post );
        PhotoPost GetPre( PhotoPost post );

        List<PhotoPost> GetByAlbum(long albumId, int count);
        List<PhotoPost> GetNew(long userId, int count);
        List<PhotoPost> GetNew( int count );

        DataPage<PhotoPost> GetFriendsPhoto(long userId, long friendId);
        DataPage<PhotoPost> GetSingle(long ownerId, long id);
        DataPage<PhotoPost> GetPostPage(long ownerId, long appId, int pageSize);
        DataPage<PhotoPost> GetPostPageByAlbum(long ownerId, long appId, long albumId, int pageSize);
        DataPage<PhotoPost> GetByUser(long userId, int pageSize);
        DataPage<PhotoPost> GetFollowing(long userId, int pageSize);

        DataPage<PhotoPost> GetShowByUser(long userId, int pageSize);
        DataPage<PhotoPost> GetShowByUser(long userId, long categoryId, int pageSize);

        Result CreatePost(Result uploadResult, string photoName, long albumId, MvcContext ctx);
        Result CreatePost( PhotoPost post, PhotoApp app );

        Result Update( PhotoPost post );
        void UpdateAlbum(long categoryId, string ids, long ownerId, long appId);


        void DeleteTrue(string ids, long ownerId);
        Boolean CanDeleteImg( PhotoPost post );
        void DeletePosts( String ids, List<PhotoPost> list );

        void CreatePostTemp( PhotoPost post );


        bool IsPin(long userId, PhotoPost x);
        void SavePin( PhotoPost x, PhotoPost postedPhoto, String tagList );

        String GetFeedMsg( List<PhotoPost> imgs );
    }

}
