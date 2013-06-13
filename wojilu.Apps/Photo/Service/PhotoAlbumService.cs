/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Apps.Photo.Domain;
using wojilu.Common.AppBase;
using wojilu.Apps.Photo.Interface;

namespace wojilu.Apps.Photo.Service {

    public class PhotoAlbumService : IPhotoAlbumService {


        public virtual PhotoAlbum GetById( int albumId ) {
            return db.findById<PhotoAlbum>( albumId );
        }

        public virtual PhotoAlbum GetById( int id, int ownerId ) {
            PhotoAlbum album = GetById( id );
            if (album == null) return null;
            if (album.OwnerId != ownerId) return null;
            return album;
        }

        public virtual PhotoAlbum GetByIdWithDefault( int id, int ownerId ) {
            PhotoAlbum album = GetById( id );
            if (album == null) {
                return getDefaultAlbum( ownerId );
            }

            if (album.OwnerId != ownerId) return null;
            return album;
        }

        private PhotoAlbum getDefaultAlbum( int ownerId ) {
            PhotoAlbum album = new PhotoAlbum();
            album.Name = alang.get( typeof(PhotoApp), "defaultAlbum" );
            album.OwnerId = ownerId;
            return album;
        }

        public virtual List<PhotoAlbum> GetListByApp( int appId ) {
            return db.find<PhotoAlbum>( "AppId=" + appId + " order by OrderId desc, Id asc" ).list();
        }

        public virtual List<PhotoAlbum> GetListByUser( int ownerId ) {
            return db.find<PhotoAlbum>( "OwnerId=" + ownerId + " order by OrderId desc, Id asc" ).list();
        }

        //private PhotoAlbum GetDefaultAlbum( int userId ) {
        //    return db.find<PhotoAlbum>( "IsDefault=1 and OwnerId=" + userId ).first();
        //}

        public virtual Result Create( PhotoAlbum album ) {
            return db.insert( album );
        }

        public virtual void Delete( PhotoAlbum album ) {
            db.updateBatch<PhotoPost>( "SaveStatus=" + SaveStatus.Delete, "CategoryId=" + album.Id );
            db.delete( album );
        }


        public virtual Result Update( PhotoAlbum album ) {
            return db.update( album );
        }


    }

}
