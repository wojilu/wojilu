/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.IO;

using wojilu.Web.Context;
using wojilu.Web.Mvc;

using wojilu.Common;
using wojilu.Common.AppBase;
using wojilu.Common.Jobs;
using wojilu.Common.Money.Domain;
using wojilu.Common.Money.Interface;
using wojilu.Common.Money.Service;

using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;

using wojilu.Apps.Photo.Domain;
using wojilu.Apps.Photo.Interface;
using wojilu.Drawing;
using System.Drawing;
using wojilu.Apps.Photo.Helper;

namespace wojilu.Apps.Photo.Service {

    public class PhotoPostService : IPhotoPostService {

        public virtual IFriendService friendService { get; set; }
        public virtual IPickedService pickedService { get; set; }
        public virtual IUserIncomeService incomeService { get; set; }
        public virtual IFollowerService followerService { get; set; }

        public PhotoPostService() {
            friendService = new FriendService();
            pickedService = new PickedService();
            incomeService = new UserIncomeService();
            followerService = new FollowerService();
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
            if (post == null || post.PhotoAlbum == null) return null;
            return db.find<PhotoPost>( "OwnerId=" + post.OwnerId + " and CategoryId=" + post.PhotoAlbum.Id + " and Id<" + post.Id ).first();
        }

        public virtual PhotoPost GetPre( PhotoPost post ) {
            if (post == null || post.PhotoAlbum == null) return null;
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
            return SysPhotoService.populatePhoto( GetNew( userId, count ) );
        }

        public virtual DataPage<PhotoPost> GetByUser( int userId, int pageSize ) {
            return PhotoPost.findPage( "OwnerId=" + userId, pageSize );
        }

        public virtual DataPage<PhotoPost> GetShowByUser( int userId, int pageSize ) {
            return PhotoPost.findPage( "SysCategoryId>0 and  SaveStatus=" + SaveStatus.Normal + " and OwnerId=" + userId, pageSize );
        }

        public virtual DataPage<PhotoPost> GetShowByUser( int userId, int categoryId, int pageSize ) {
            return PhotoPost.findPage( "SysCategoryId>0 and CategoryId=" + categoryId + " and SaveStatus=" + SaveStatus.Normal + " and OwnerId=" + userId, pageSize );
        }

        public virtual void UpdateAlbum( int categoryId, String ids, int ownerId, int appId ) {

            String condition = string.Format( "Id in ({0}) and OwnerId={1}", ids, ownerId );
            db.updateBatch<PhotoPost>( "set CategoryId=" + categoryId + ", AppId=" + appId, condition );
        }

        public virtual void DeleteTrue( String ids, int ownerId ) {
            if (cvt.IsIdListValid( ids ) == false) return;
            String condition = string.Format( "Id in ({0}) and OwnerId={1}", ids, ownerId );
            List<PhotoPost> list = db.find<PhotoPost>( condition ).list();

            DeletePosts( ids, list );

            // 统计
            User user = User.findById( ownerId );
            if (user != null) {
                user.Pins = PhotoPost.count( "OwnerId=" + ownerId );
                user.update( "Pins" );
            }
        }

        public void DeletePosts( String ids, List<PhotoPost> list ) {
            foreach (PhotoPost post in list) {
                db.delete( post );
                if (CanDeleteImg( post )) {
                    wojilu.Drawing.Img.DeleteImgAndThumb( strUtil.Join( sys.Path.DiskPhoto, post.DataUrl ) );
                }
            }

            pickedService.DeletePhoto( ids );
        }

        public Boolean CanDeleteImg( PhotoPost post ) {
            // 原始图片
            if (post.RootId == 0) {
                return noRepins( post ); // 是否有其他人收集
            }
            // 只是转发: 原始图片存在
            else if (isRootExits( post.RootId )) {
                return false;
            }
            // 只是转发
            else {
                return isLastRepin( post );  //看是否是最后一个转发 
            }
        }

        private bool isRootExits( int rootId ) {
            return this.GetById_Admin( rootId ) != null;
        }

        private bool isLastRepin( PhotoPost post ) {
            return PhotoPost.find( "RootId=" + post.RootId + " and Id<>" + post.Id ).first() == null;
        }

        private bool noRepins( PhotoPost post ) {
            return PhotoPost.find( "RootId=" + post.Id ).first() == null;
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

                updatePhotoSize( post );

                this.updateCountApp( app );
                this.updateCountAlbum( post.PhotoAlbum );

                String msg = string.Format( "上传图片 <a href=\"{0}\">{1}</a>，得到奖励", alink.ToAppData( post ), post.Title );
                incomeService.AddIncome( post.Creator, UserAction.Photo_CreatePost.Id, msg );

            }
            return result;
        }

        // 保存图片大小等信息
        private void updatePhotoSize( PhotoPost post ) {

            String photoPath = post.DataUrl;
            if (strUtil.IsNullOrEmpty( photoPath )) return;
            if (photoPath.ToLower().StartsWith( "http://" )) return;
            if (photoPath.StartsWith( "/" )) return;

            Dictionary<String, PhotoInfo> dic = new Dictionary<String, PhotoInfo>();
            foreach (KeyValuePair<String, ThumbInfo> kv in ThumbConfig.GetPhotoConfig()) {

                String xpath = Img.GetThumbPath( strUtil.Join( sys.Path.DiskPhoto, photoPath ), kv.Key );
                String thumbPath = PathHelper.Map( xpath );

                Size size = Img.GetPhotoSize( thumbPath );

                dic.Add( kv.Key, new PhotoInfo { Width=size.Width, Height=size.Height } );
            }

            String str = ObjectContext.Create<PhotoInfoHelper>().ConvertString( dic );
            if (strUtil.IsNullOrEmpty( str )) return;

            post.SizeInfo = str;
            post.update();
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
                this.updateCountApp( ctx.app.obj as PhotoApp );
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

        // app count
        private void updateCountApp( PhotoApp app ) {

            int count = db.count<PhotoPost>( "AppId=" + app.Id );
            app.PhotoCount = count;
            db.update( app, "PhotoCount" );

        }

        // album count
        private void updateCountAlbum( PhotoAlbum album ) {

            int count = db.count<PhotoPost>( "CategoryId=" + album.Id );
            album.DataCount = count;
            db.update( album, "DataCount" );
        }

        public List<PhotoPost> GetByAlbum( int albumId, int count ) {
            return PhotoPost.find( "CategoryId=" + albumId + " and SaveStatus=" + SaveStatus.Normal ).list( count );
        }

        public List<PhotoPost> GetNew( int userId, int count ) {
            if (count <= 0) count = 10;
            return db.find<PhotoPost>( "Creator.Id=" + userId + " and SysCategoryId>0 and AppId>0 and SaveStatus=" + SaveStatus.Normal ).list( count );
        }

        public List<PhotoPost> GetNew( int count ) {
            if (count <= 0) count = 10;
            return db.find<PhotoPost>( "SysCategoryId>0 and SaveStatus=" + SaveStatus.Normal ).list( count );
        }

        public DataPage<PhotoPost> GetFollowing( int userId, int pageSize ) {
            String ids = followerService.GetFollowingIds( userId );
            ids = strUtil.HasText( ids ) ? ids + "," + userId : userId.ToString();
            return PhotoPost.findPage( "OwnerId in (" + ids + ") and SaveStatus=" + SaveStatus.Normal, pageSize );
        }

        public bool IsPin( int userId, PhotoPost x ) {

            String condition = "OwnerId=" + userId + " and ";
            condition += x.RootId > 0
                ? "(RootId=" + x.Id + " or RootId=" + x.RootId + ")"
                : "RootId=" + x.Id;

            return (x.OwnerId == userId || PhotoPost.find( condition ).first() != null);

        }




        public void SavePin( PhotoPost x, PhotoPost photo, String tagList ) {


            populatePostInfo( photo, x );


            photo.insert();
            photo.Tag.Save( tagList );
            // TODO 动态消息

            x.Pins = PhotoPost.count( "RootId=" + x.Id + " or ParentId=" + x.Id );
            x.update( "Pins" );

            User user = photo.Creator;
            user.Pins = PhotoPost.count( "OwnerId=" + user.Id );
            user.update( "Pins" );
        }

        private void populatePostInfo( PhotoPost photo, PhotoPost x ) {

            photo.ParentId = x.Id;
            photo.RootId = x.RootId > 0 ? x.RootId : x.Id;

            photo.SysCategoryId = x.SysCategoryId;

            photo.Title = x.Title;
            photo.DataUrl = x.DataUrl;
        }


    }

}
