/*
 * Copyright 2010 www.wojilu.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */


using System;
using System.IO;
using System.Web;
using System.Drawing;
using wojilu.Drawing;

namespace wojilu.Web.Utils {

    /// <summary>
    /// 头像上传工具
    /// </summary>
    public class AvatarUploader {

        private static readonly ILog logger = LogManager.GetLogger( typeof( AvatarUploader ) );

        /// <summary>
        /// 保存用户上传的头像
        /// </summary>
        /// <param name="postedFile"></param>
        /// <param name="viewerUrl"></param>
        /// <returns></returns>
        public static Result Save( HttpFile postedFile, String viewerUrl ) {
            return upload_private( sys.Path.DiskAvatar, postedFile, viewerUrl );
        }

        private static Result upload_private( String uploadPath, HttpFile postedFile, String picName ) {

            logger.Info( "uploadPath:" + uploadPath + ", picName:" + picName );

            Result result = new Result();

            Uploader.checkUploadPic( postedFile, result );
            if (result.HasErrors) return result;

            String str = PathHelper.Map( uploadPath );
            String str2 = picName + "." + Img.GetImageExt( postedFile.ContentType );
            String srcPath = Path.Combine( str, str2 );
            try {
                postedFile.SaveAs( srcPath );
                saveAvatarThumb( srcPath );
            }
            catch (Exception exception) {
                logger.Error( lang.get( "exPhotoUploadError" ) + ":" + exception.Message );
                result.Add( lang.get( "exPhotoUploadErrorTip" ) );
                return result;
            }



            // 返回的信息是缩略图
            String thumbPath = Img.GetThumbPath( srcPath );
            result.Info = "face/" + Path.GetFileName( thumbPath );
            return result;
        }

        private static void saveAvatarThumb( String srcPath ) {

            Boolean saveSmallThumb = true;
            if (saveSmallThumb) {
                int x = config.Instance.Site.AvatarThumbWidth;
                int y = config.Instance.Site.AvatarThumbHeight;
                saveAvatarPrivate( x, y, srcPath, ThumbnailType.Small );
            }

            if (config.Instance.Site.IsSaveAvatarMedium) {
                int x = config.Instance.Site.AvatarThumbWidthMedium;
                int y = config.Instance.Site.AvatarThumbHeightMedium;
                saveAvatarPrivate( x, y, srcPath, ThumbnailType.Medium );
            }

            if (config.Instance.Site.IsSaveAvatarBig) {
                int x = config.Instance.Site.AvatarThumbWidthBig;
                int y = config.Instance.Site.AvatarThumbHeightBig;
                saveAvatarPrivate( x, y, srcPath, ThumbnailType.Big );
            }

        }

        private static void saveAvatarPrivate( int x, int y, String srcPath, ThumbnailType ttype ) {

            String thumbPath = Img.GetThumbPath( srcPath, ttype );

            using (Image img = Image.FromFile( srcPath )) {
                if (img.Size.Width <= x && img.Size.Height <= y) {
                    File.Copy( srcPath, thumbPath );
                }
                else {
                    logger.Info( "SaveThumbnail..." );
                    Img.SaveThumbnail( srcPath, thumbPath, x, y, SaveThumbnailMode.Cut );
                }
            }


        }

    }
}

