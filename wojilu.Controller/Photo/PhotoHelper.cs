using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Photo.Domain;

namespace wojilu.Web.Controller.Photo {

    public class PhotoHelper {

        public static string getCover( PhotoAlbum album ) {

            if (strUtil.HasText( album.Logo )) return sys.Path.GetPhotoThumb( album.Logo, "sx" );

            PhotoPost photo = PhotoPost.find( "AppId=" + album.AppId + " and PhotoAlbum.Id=" + album.Id ).first();
            if (photo != null) {

                album.Logo = photo.DataUrl;
                album.update( "Logo" );

                return photo.ImgThumbUrl;
            }

            return strUtil.Join( sys.Path.Img, "/m/album.jpg" );
        }

        public static int getDataCount( PhotoAlbum album ) {
            if (album.DataCount > 0) return album.DataCount;
            int count = PhotoPost.find( "AppId=" + album.AppId + " and PhotoAlbum.Id=" + album.Id ).count();
            album.DataCount = count;
            album.update( "DataCount" );
            return count;
        }


    }


}
