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
using System.Net;
using wojilu.Net;
using System.Collections.Generic;

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
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Result Save( HttpFile postedFile, int userId ) {
            return upload_private( sys.Path.DiskAvatar, postedFile, userId );
        }


        public static void Delete( String picPath ) {

            String fullPath = strUtil.Join( "/static/upload/", picPath );

            String oPath = Img.GetOriginalPath( fullPath );
            deleteImgPrivate( oPath, "original" );

            Dictionary<String, ThumbInfo> dicThumbConfig = ThumbConfig.GetAvatarConfig();
            foreach (KeyValuePair<String, ThumbInfo> kv in dicThumbConfig) {
                String mPath = Img.GetThumbPath( fullPath, kv.Key );
                deleteImgPrivate( mPath, kv.Key );
            }
        }

        private static Boolean deleteImgPrivate( String fullPath, String dtype ) {
            String absPath = PathHelper.Map( fullPath );
            if (file.Exists( absPath )) {
                file.Delete( absPath );
                logger.Info( "删除头像成功" + dtype + ":" + absPath );
                return true;
            }
            else {
                logger.Error( "头像不存在" + dtype + ":" + absPath );
                return false;
            }
        }


        public static Result Save( String oPicAbsPath, int userId ) {

            Result result = new Result();

            if (file.Exists( oPicAbsPath ) == false) {
                String msg = "图片不存在" + oPicAbsPath;
                logger.Error( msg );
                result.Add( msg );
                return result;
            }

            AvatarSaver aSaver = AvatarSaver.New( oPicAbsPath );

            return savePicCommon( aSaver, userId, result, sys.Path.DiskAvatar );
        }

        public static Result SaveRemote( string picUrl, int userId ) {

            logger.Info( "picUrl:" + picUrl );
            logger.Info( "userId:" + userId );

            Result result = new Result();
            AvatarSaver aSaver = new AvatarNetSaver( picUrl );
            return savePicCommon( aSaver, userId, result, sys.Path.DiskAvatar );
        }

        private static Result savePicCommon( AvatarSaver aSaver, int userId, Result result, String uploadPath ) {

            DateTime now = DateTime.Now;
            String strDir = getDirName( now );
            String fullDir = strUtil.Join( uploadPath, strDir );

            String absPath = PathHelper.Map( fullDir );
            if (!Directory.Exists( absPath )) {
                Directory.CreateDirectory( absPath );
                logger.Info( "CreateDirectory:" + absPath );
            }

            String picName = string.Format( "{0}_{1}_{2}_{3}", userId, now.Hour, now.Minute, now.Second );
            String picNameWithExt = strUtil.Join( picName, aSaver.GetExt(), "." );

            String picAbsPath = Path.Combine( absPath, picNameWithExt );
            try {

                aSaver.Save( picAbsPath );
                Boolean isValid = saveAvatarThumb( picAbsPath );
                if (!isValid) {

                    file.Delete( picAbsPath );
                    result.Add( "format error" );
                    return result;

                }
            }
            catch (Exception exception) {
                logger.Error( lang.get( "exPhotoUploadError" ) + ":" + exception.Message );
                result.Add( lang.get( "exPhotoUploadErrorTip" ) );
                return result;
            }

            // 返回的信息是缩略图
            String relPath = strUtil.Join( fullDir, picNameWithExt ).TrimStart( '/' );
            relPath = strUtil.TrimStart( relPath, "static/upload/" );
            String thumbPath = Img.GetThumbPath( relPath );

            logger.Info( "return thumbPath=" + thumbPath );

            result.Info = thumbPath;
            return result;
        }

        private static Result upload_private( String uploadPath, HttpFile postedFile, int userId ) {

            logger.Info( "uploadPath:" + uploadPath + ", userId:" + userId );

            Result result = new Result();

            checkUploadPic( postedFile, result );
            if (result.HasErrors) return result;

            AvatarSaver aSaver = AvatarSaver.New( postedFile );

            return savePicCommon( aSaver, userId, result, uploadPath );
        }

        private static void checkUploadPic( HttpFile postedFile, Result errors ) {

            if (postedFile == null) {
                errors.Add( lang.get( "exPlsUpload" ) );
                return;
            }

            // 检查文件大小
            if (postedFile.ContentLength <= 1) {
                errors.Add( lang.get( "exPlsUpload" ) );
                return;
            }

            int uploadMax = 1024 * config.Instance.Site.UploadAvatarMaxKB;
            if (postedFile.ContentLength > uploadMax) {
                errors.Add( lang.get( "exUploadMax" ) + " " + config.Instance.Site.UploadAvatarMaxKB + " KB" );
                return;
            }

            // 检查文件格式
            if (Uploader.IsAllowedPic( postedFile ) == false) {
                errors.Add( lang.get( "exUploadType" ) + ":" + postedFile.FileName + "(" + postedFile.ContentType + ")" );
            }
        }


        private static string getDirName( DateTime now ) {
            return string.Format( "{0}/{1}/{2}", now.Year, now.Month, now.Day );
        }


        private static Boolean saveAvatarThumb( String srcPath ) {

            Dictionary<String, ThumbInfo> dicThumbConfig = ThumbConfig.GetAvatarConfig();
            foreach (KeyValuePair<String, ThumbInfo> kv in dicThumbConfig) {
                SaveThumbSingle( srcPath, kv.Key, kv.Value );
            }

            return true;
        }

        public static Boolean SaveThumbSingle( String srcPath, String suffix, ThumbInfo thumbInfo ) {

            String thumbPath = Img.GetThumbPath( srcPath, suffix );
            int x = thumbInfo.Width;
            int y = thumbInfo.Height;

            try {
                using (Image img = Image.FromFile( srcPath )) {

                    if (file.Exists( thumbPath )) file.Delete( thumbPath );


                    if (img.Size.Width <= x && img.Size.Height <= y) {
                        File.Copy( srcPath, thumbPath );
                    }
                    else if (img.RawFormat.Equals( System.Drawing.Imaging.ImageFormat.Gif ) && ImageAnimator.CanAnimate( img )) {
                        File.Copy( srcPath, thumbPath );
                    }
                    else {
                        logger.Info( "save thumbnail..." + suffix + ": " + srcPath + "=>" + thumbPath );
                        Img.SaveThumbnail( srcPath, thumbPath, x, y, thumbInfo.Mode );
                    }
                    return true;
                }

            }
            catch (OutOfMemoryException ex) {
                logger.Error( "file format error: " + srcPath );
                return false;
            }


        }

    }
}

