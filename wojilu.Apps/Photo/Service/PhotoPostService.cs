/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.IO;

using wojilu.Web.Mvc;
using wojilu.Web.Context;
using wojilu.Common.AppBase;
using wojilu.Members.Users.Service;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Photo.Domain;
using wojilu.Common.Jobs;
using wojilu.Members.Users.Interface;
using wojilu.Apps.Photo.Interface;
using wojilu.Common;
using wojilu.Drawing;

namespace wojilu.Apps.Photo.Service {

    public class PhotoPostService : IPhotoPostService {

        public virtual IFriendService friendService { get; set; }
        public virtual IPickedService pickedService { get; set; }

        public PhotoPostService() {
            friendService = new FriendService();
            pickedService = new PickedService();
        }

        public virtual DataPage<PhotoPost> GetPostPage( int ownerId, int appId, int pageSize ) {
            return db.findPage<PhotoPost>( "OwnerId=" + ownerId + " and AppId=" + appId, pageSize );
        }

        public virtual DataPage<PhotoPost> GetPostPageByAlbum( int ownerId, int appId, int albumId, int pageSize ) {
            if (albumId == 0) {
                return db.findPage<PhotoPost>( "OwnerId=" + ownerId + " and PhotoAlbum.Id=" + albumId, pageSize );
            }
            return db.findPage<PhotoPost>( "OwnerId=" + ownerId + " and AppId=" + appId + " and PhotoAlbum.Id=" + albumId, pageSize );
        }

        private PhotoPost GetById( int id ) {
            return db.findById<PhotoPost>( id );
        }

        public virtual PhotoPost GetById_Admin( int id ) {
            return db.findById<PhotoPost>( id );
        }

        public virtual PhotoPost GetById( int id, int ownerId ) {
            PhotoPost post = GetById( id );
            if (post == null) return null;
            if (post.OwnerId != ownerId) return null;
            PhotoAlbum album = new PhotoAlbum();
            album.Id = 0;
            album.Name = alang.get( typeof( PhotoApp ), "defaultAlbum" );
            if (post.PhotoAlbum == null) post.PhotoAlbum = album;
            return post;
        }

        public virtual DataPage<PhotoPost> GetSingle( int ownerId, int id ) {


            DataPage<PhotoPost> list = new DataPage<PhotoPost>();

            PhotoPost post = GetById( id );

            List<PhotoPost> results = new List<PhotoPost>();
            results.Add( post );

            list.Results = results;

            int count = GetPostCount( ownerId, post.PhotoAlbum.Id );
            list.RecordCount = count;

            int preCount = getPreCount( ownerId, post.PhotoAlbum.Id, id );
            list.Current = preCount + 1;

            return list;
        }

        private int getPreCount( int ownerId, int albumId, int id ) {
            return db.count<PhotoPost>( "OwnerId=" + ownerId + " and CategoryId=" + albumId + " and Id>" + id );
        }

        public virtual int GetCountByUser( int userId ) {
            return db.count<PhotoPost>( "OwnerId=" + userId + " and SaveStatus=" + SaveStatus.Normal );
        }

        private int GetPostCount( int ownerId, int albumId ) {
            return db.count<PhotoPost>( "OwnerId=" + ownerId + " and CategoryId=" + albumId );
        }

        public virtual PhotoPost GetNext( PhotoPost post ) {
            return db.find<PhotoPost>( "OwnerId=" + post.OwnerId + " and CategoryId=" + post.PhotoAlbum.Id + " and Id<" + post.Id ).first();
        }

        public virtual PhotoPost GetPre( PhotoPost post ) {
            return db.find<PhotoPost>( "OwnerId=" + post.OwnerId + " and CategoryId=" + post.PhotoAlbum.Id + " and Id>" + post.Id + " order by Id" ).first();
        }

        public virtual PhotoPost GetFirst( PhotoPost post ) {
            PhotoPost result = db.find<PhotoPost>( "OwnerId=" + post.OwnerId + " and CategoryId=" + post.PhotoAlbum.Id + " and Id>" + post.Id ).first();
            if (result == null) return post;
            return result;
        }

        public virtual void AddtHits( PhotoPost post ) {
            HitsJob.Add( post );
        }

        public virtual DataPage<PhotoPost> GetFriendsPhoto( int userId, int friendId ) {

            String condition;
            if (friendId > 0)
                condition = "CreatorId=" + friendId;
            else {
                String ids = friendService.FindFriendsIds( userId );
                if (strUtil.IsNullOrEmpty( ids )) return DataPage<PhotoPost>.GetEmpty();
                condition = "CreatorId in (" + ids + ")";
            }

            condition = condition + " and AppId>0 and SaveStatus=" + SaveStatus.Normal;

            return db.findPage<PhotoPost>( condition );
        }

        public virtual List<IBinderValue> GetMyNew( int count, int userId ) {
            if (count < 0) count = 10;
            List<PhotoPost> posts = db.find<PhotoPost>( "Creator.Id=" + userId + " and AppId>0 and SaveStatus=" + SaveStatus.Normal ).list( count );
            return SysPhotoService.populatePhoto( posts );
        }

        public virtual DataPage<PhotoPost> GetByUser( int userId, int pageSize ) {
            return PhotoPost.findPage( "OwnerId=" + userId, pageSize );
        }

        public virtual void UpdateAlbum( int categoryId, String ids, int ownerId, int appId ) {

            String condition = string.Format( "Id in ({0}) and OwnerId={1}", ids, ownerId );
            db.updateBatch<PhotoPost>( "set CategoryId=" + categoryId + ", AppId=" + appId, condition );
        }

        public virtual void DeleteTrue( String ids, int ownerId ) {
            if (cvt.IsIdListValid( ids ) == false) return;
            String condition = string.Format( "Id in ({0}) and OwnerId={1}", ids, ownerId );
            List<PhotoPost> list = db.find<PhotoPost>( condition ).list();

            foreach (PhotoPost post in list) {
                db.delete( post );
                wojilu.Drawing.Img.DeleteImgAndThumb( strUtil.Join( sys.Path.DiskPhoto, post.DataUrl ) );
            }

            pickedService.DeletePhoto( ids );
        }

        public Result Update( PhotoPost post ) {
            return db.update( post );
        }

        //------------------------------------------------------------------------------------------------------

        public virtual void CreatePostTemp( PhotoPost post ) {
            db.insert( post );
        }

        public virtual Result CreatePost( PhotoPost post, PhotoApp app ) {

            if (strUtil.IsNullOrEmpty( post.Title )) {
                post.Title = Path.GetFileNameWithoutExtension( post.DataUrl );
            }
            post.Title = strUtil.CutString( post.Title, 30 );

            Result result = db.insert( post );
            if (result.IsValid) {
                this.updatePostCount( app );
            }
            return result;

        }

        public virtual Result CreatePost( Result uploadResult, String photoName, int albumId, MvcContext ctx ) {

            String path = uploadResult.Info.ToString();
            if (strUtil.IsNullOrEmpty( photoName )) {
                photoName = Path.GetFileNameWithoutExtension( path );
            }
            photoName = strUtil.CutString( photoName, 30 );
            PhotoAlbum album = getAlbum( albumId, ctx.viewer.obj as User );

            PhotoPost photo = new PhotoPost();
            photo.AppId = album.AppId;
            photo.Creator = (User)ctx.viewer.obj;
            photo.CreatorUrl = ctx.viewer.obj.Url;
            photo.OwnerId = ctx.owner.Id;
            photo.OwnerUrl = ctx.owner.obj.Url;
            photo.OwnerType = ctx.owner.obj.GetType().FullName;
            photo.Title = photoName;
            photo.DataUrl = path;
            photo.PhotoAlbum = album;
            photo.Ip = ctx.Ip;

            Result result = db.insert( photo );
            if (result.IsValid) {
                this.updatePostCount( ctx.app.obj as PhotoApp );
            }
            return result;
        }

        private PhotoAlbum getAlbum( int albumId, User user ) {
            PhotoAlbum album;
            if (albumId > 0) {
                album = getAlbumById( albumId );
            }
            else {
                album = getDefaultPhotoAlbum( user.Id );
            }
            if (album == null) {
                album = initDefaultAlbum( user );
            }
            return album;
        }

        private PhotoAlbum getAlbumById( int albumId ) {
            return db.findById<PhotoAlbum>( albumId );
        }

        private PhotoAlbum getDefaultPhotoAlbum( int userId ) {
            return db.find<PhotoAlbum>( "IsDefault=1 and OwnerId=" + userId ).first();
        }

        private PhotoAlbum initDefaultAlbum( User user ) {

            PhotoAlbum album = db.find<PhotoAlbum>( "OwnerId=" + user.Id ).first();
            if (album == null) {
                return createDefaultAlbum( user, getDefaultPhotoAppId( user.Id ) );
            }
            album.IsDefault = 1;
            db.update( album, "IsDefault" );
            return album;
        }

        private PhotoAlbum createDefaultAlbum( User user, int appId ) {
            PhotoAlbum album = new PhotoAlbum();
            album.Name = alang.get( typeof( PhotoApp ), "defaultAlbum" );
            album.IsDefault = 1;
            album.OwnerId = user.Id;
            album.OwnerUrl = user.Url;
            album.AppId = appId;
            db.insert( album );
            return album;
        }

        private int getDefaultPhotoAppId( int userId ) {
            PhotoApp photo = db.find<PhotoApp>( "OwnerId=" + userId ).first();
            if (photo == null) {
                throw new Exception( "the photo app is not found" );
            }
            return photo.Id;
        }

        private void updatePostCount( PhotoApp app ) {
            int count = db.count<PhotoPost>( "AppId=" + app.Id );
            app.PhotoCount = count;
            db.update( app, "PhotoCount" );
        }

    }

}
