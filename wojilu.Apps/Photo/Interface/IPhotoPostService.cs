/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Apps.Photo.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Common;

namespace wojilu.Apps.Photo.Interface {

    public interface IPhotoPostService {

        IFriendService friendService { get; set; }
        IPickedService pickedService { get; set; }

        void AddtHits( PhotoPost post );

        PhotoPost GetById( int id, int ownerId );
        PhotoPost GetById_Admin( int id );

        int GetCountByUser( int userId );
        PhotoPost GetFirst( PhotoPost post );
        List<IBinderValue> GetMyNew( int count, int userId );
        PhotoPost GetNext( PhotoPost post );
        PhotoPost GetPre( PhotoPost post );

        List<PhotoPost> GetByAlbum( int albumId, int count );
        List<PhotoPost> GetNew( int userId, int count );
        List<PhotoPost> GetNew( int count );

        DataPage<PhotoPost> GetFriendsPhoto( int userId, int friendId );
        DataPage<PhotoPost> GetSingle( int ownerId, int id );
        DataPage<PhotoPost> GetPostPage( int ownerId, int appId, int pageSize );
        DataPage<PhotoPost> GetPostPageByAlbum( int ownerId, int appId, int albumId, int pageSize );
        DataPage<PhotoPost> GetByUser( int userId, int pageSize );
        DataPage<PhotoPost> GetFollowing( int userId, int pageSize );

        DataPage<PhotoPost> GetShowByUser( int userId, int pageSize );
        DataPage<PhotoPost> GetShowByUser( int userId, int categoryId, int pageSize );

        Result CreatePost( Result uploadResult, String photoName, int albumId, wojilu.Web.Context.MvcContext ctx );
        Result CreatePost( PhotoPost post, PhotoApp app );

        Result Update( PhotoPost post );
        void UpdateAlbum( int categoryId, String ids, int ownerId, int appId );


        void DeleteTrue( String ids, int ownerId );
        Boolean CanDeleteImg( PhotoPost post );
        void DeletePosts( String ids, List<PhotoPost> list );

        void CreatePostTemp( PhotoPost post );


        Boolean IsPin( int userId, PhotoPost x );
        void SavePin( PhotoPost x, PhotoPost postedPhoto, String tagList );
    }

}
