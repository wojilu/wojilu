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

using wojilu.Drawing;
using System.Drawing;
using System.Collections.Generic;
using System.Threading;

namespace wojilu.Web.Utils {

    /// <summary>
    /// 图片和文件上传工具
    /// </summary>
    public class Uploader {

        private static readonly ILog logger = LogManager.GetLogger( typeof( Uploader ) );

        /// <summary>
        /// 默认生成缩略图的类型
        /// </summary>
        public static ThumbnailType[] ThumbTypes = new ThumbnailType[] { ThumbnailType.Small, ThumbnailType.Medium };

        /// <summary>
        /// 判断上传文件是否是图片
        /// </summary>
        /// <param name="postedFile"></param>
        /// <returns></returns>
        public static Boolean IsImage( HttpFile postedFile ) {
            return IsImage( postedFile.ContentType, postedFile.FileName );
        }

        public static Boolean IsImage( String contentType, String fileName ) {

            if (strUtil.IsNullOrEmpty( contentType )) return false;

            if (contentType.ToLower().IndexOf( "application/octet-stream" ) >= 0) {
                String ext = Path.GetExtension( fileName );
                if (strUtil.IsNullOrEmpty( ext )) return false;
                List<String> picExts = new List<string>( new String[] { ".jpg", ".jpeg", ".gif", ".png", ".bmp" } );
                return picExts.Contains( ext.ToLower() );
            }

            return IsImage( contentType );

        }

        /// <summary>
        /// 判断上传文件是否是图片
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static Boolean IsImage( String contentType ) {
            if (strUtil.IsNullOrEmpty( contentType )) return false;
            return contentType.ToLower().IndexOf( "image" ) >= 0;
        }

        /// <summary>
        /// 保存群组 logo
        /// </summary>
        /// <param name="postedFile"></param>
        /// <param name="groupUrlName"></param>
        /// <returns></returns>
        public static Result SaveGroupLogo( HttpFile postedFile, String groupUrlName ) {
            return SaveImg( sys.Path.DiskGroupLogo, postedFile, groupUrlName, config.Instance.Group.LogoWidth, config.Instance.Group.LogoHeight );
        }

        /// <summary>
        /// 保存网站 logo
        /// </summary>
        /// <param name="postedFile"></param>
        /// <returns></returns>
        public static Result SaveSiteLogo( HttpFile postedFile ) {
            return SaveImg( sys.Path.DiskPhoto, postedFile, "logo", config.Instance.Site.LogoWidth, config.Instance.Site.LogoHeight );
        }

        /// <summary>
        /// 保存上传的文件，如果是图片，则处理缩略图
        /// </summary>
        /// <param name="postedFile"></param>
        /// <returns></returns>
        public static Result SaveFileOrImage( HttpFile postedFile ) {

            if (postedFile == null) {
                return new Result( lang.get( "exPlsUpload" ) );
            }

            if (Uploader.IsImage( postedFile ))
                return Uploader.SaveImg( postedFile );

            return Uploader.SaveFile( postedFile );
        }

        /// <summary>
        /// 保存上传的非图片型文件
        /// </summary>
        /// <param name="postedFile"></param>
        /// <returns></returns>
        public static Result SaveFile( HttpFile postedFile ) {

            Result errors = new Result();

            checkUploadFile( postedFile, errors );
            if (errors.HasErrors) {
                logger.Info( errors.ErrorsText );
                return errors;
            }

            String fileExt = Path.GetExtension( postedFile.FileName );

            String pathName = PathHelper.Map( sys.Path.DiskPhoto );
            String fileName = Img.GetFileName( pathName, fileExt );
            String filenameWithPath = Path.Combine( pathName, fileName );
            try {
                postedFile.SaveAs( filenameWithPath );
            }
            catch (Exception exception) {
                logger.Error( lang.get( "exPhotoUploadError" ) + ":" + exception.Message );
                errors.Add( lang.get( "exPhotoUploadErrorTip" ) );
                return errors;
            }
            errors.Info = fileName.Replace( @"\", "/" );
            return errors;
        }


        /// <summary>
        /// 保存上传的图片
        /// </summary>
        /// <param name="postedFile"></param>
        /// <returns></returns>
        public static Result SaveImg( HttpFile postedFile ) {

            return SaveImg( postedFile, ThumbTypes );
        }

        /// <summary>
        /// 保存上传的图片
        /// </summary>
        /// <param name="postedFile"></param>
        /// <param name="arrThumbType"></param>
        /// <returns></returns>
        public static Result SaveImg( HttpFile postedFile, ThumbnailType[] arrThumbType ) {

            Result result = new Result();

            checkUploadPic( postedFile, result );
            if (result.HasErrors) {
                logger.Info( result.ErrorsText );
                return result;
            }

            String pathName = PathHelper.Map( sys.Path.DiskPhoto );
            String photoName = Img.GetPhotoName( pathName, postedFile.ContentType );
            String filename = Path.Combine( pathName, photoName );

            try {
                postedFile.SaveAs( filename );

                foreach (ThumbnailType ttype in arrThumbType) {
                    saveThumbSmall( filename, ttype );
                }

            }
            catch (Exception exception) {
                logger.Error( lang.get( "exPhotoUploadError" ) + ":" + exception.Message );
                result.Add( lang.get( "exPhotoUploadErrorTip" ) );
                return result;
            }
            result.Info = photoName.Replace( @"\", "/" );
            return result;
        }

        private static void saveThumbSmall( String filename, ThumbnailType ttype ) {

            int x = 0;
            int y = 0;
            SaveThumbnailMode sm = SaveThumbnailMode.Auto;

            if (ttype == ThumbnailType.Small) {
                x = config.Instance.Site.PhotoThumbWidth;
                y = config.Instance.Site.PhotoThumbHeight;
                sm = SaveThumbnailMode.Cut;
            }
            else if (ttype == ThumbnailType.Medium) {
                x = config.Instance.Site.PhotoThumbWidthMedium;
                y = config.Instance.Site.PhotoThumbHeightMedium;
            }
            else if (ttype == ThumbnailType.Big) {
                x = config.Instance.Site.PhotoThumbWidthBig;
                y = config.Instance.Site.PhotoThumbHeightBig;
            }


            using (Image img = Image.FromFile( filename )) {
                if (img.Size.Width <= x && img.Size.Height <= y) {
                    File.Copy( filename, Img.GetThumbPath( filename, ttype ) );
                }
                else {
                    Img.SaveThumbnail( filename, Img.GetThumbPath( filename, ttype ), x, y, sm );
                }
            }
        }


        private static Boolean shouldMakeThumb( String ofile ) {
            using (Image img = Image.FromFile( ofile )) {
                if (img.Size.Width <= 500 && img.Size.Height <= 500) return false;
            }
            return true;
        }


        /// <summary>
        /// 上传图片(自定义保存路径)
        /// </summary>
        /// <param name="uploadPath">保存路径(相对路径)</param>
        /// <param name="postedFile">HttpFile</param>
        /// <param name="picName">图片名称</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns></returns>
        public static Result SaveImg( String uploadPath, HttpFile postedFile, String picName, int width, int height ) {
            logger.Info( "uploadPath : " + uploadPath );
            logger.Info( "picName : " + picName );
            Result result = new Result();

            checkUploadPic( postedFile, result );
            if (result.HasErrors) return result;

            String str = PathHelper.Map( uploadPath );
            String str2 = picName + "." + Img.GetImageExt( postedFile.ContentType );
            String filename = Path.Combine( str, str2 );
            try {
                postedFile.SaveAs( filename );
                Img.SaveThumbnail( filename, Img.GetThumbPath( filename ), width, height, SaveThumbnailMode.Cut );
            }
            catch (Exception exception) {
                logger.Error( lang.get( "exPhotoUploadError" ) + ":" + exception.Message );
                result.Add( lang.get( "exPhotoUploadErrorTip" ) );
                return result;
            }
            result.Info = Path.GetFileName( Img.GetThumbPath( filename ) );
            return result;
        }


        private static void checkUploadFile( HttpFile postedFile, Result errors ) {

            if (postedFile == null) {
                errors.Add( lang.get( "exPlsUpload" ) );
                return;
            }

            // 检查文件大小
            if (postedFile.ContentLength <= 1) {
                errors.Add( lang.get( "exPlsUpload" ) );
                return;
            }

            int uploadMax = 1024 * 1024 * config.Instance.Site.UploadFileMaxMB;
            if (postedFile.ContentLength > uploadMax) {
                errors.Add( "文件不能超过" + uploadMax );
                return;
            }

            // 检查文件格式
            if (Uploader.isAllowedFile( postedFile ) == false) {
                errors.Add( lang.get( "exUploadType" ) + ":" + postedFile.FileName + "(" + postedFile.ContentType + ")" );
            }

        }

        /// <summary>
        /// 检查上传的图片是否合法
        /// </summary>
        /// <param name="postedFile"></param>
        /// <param name="errors"></param>
        public static void checkUploadPic( HttpFile postedFile, Result errors ) {

            if (postedFile == null) {
                errors.Add( lang.get( "exPlsUpload" ) );
                return;
            }

            // 检查文件大小
            if (postedFile.ContentLength <= 1) {
                errors.Add( lang.get( "exPlsUpload" ) );
                return;
            }

            int uploadMax = 1024 * 1024 * config.Instance.Site.UploadPicMaxMB;
            if (postedFile.ContentLength > uploadMax) {
                errors.Add( lang.get( "exUploadMax" ) + " " + uploadMax );
                return;
            }

            // TODO: (flash upload) application/octet-stream 
            //if (postedFile.ContentType.ToLower().IndexOf( "image" ) < 0) {
            //    errors.Add( lang.get( "exPhotoFormatTip" ) );
            //    return;
            //}

            // 检查文件格式
            if (Uploader.isAllowedPic( postedFile ) == false) {
                errors.Add( lang.get( "exUploadType" ) + ":" + postedFile.FileName + "(" + postedFile.ContentType + ")" );
            }
        }

        private static Boolean isAllowedFile( HttpFile pfile ) {

            String[] types = { "zip", "7z", "rar" };
            String[] cfgTypes = config.Instance.Site.UploadFileTypes;
            if (cfgTypes != null && cfgTypes.Length > 0) types = cfgTypes;

            foreach (String ext in types) {
                if (strUtil.IsNullOrEmpty( ext )) continue;
                String extWithDot = ext.StartsWith( "." ) ? ext : "." + ext;
                if (strUtil.EqualsIgnoreCase( Path.GetExtension( pfile.FileName ), extWithDot )) return true;
            }

            return false;
        }

        private static Boolean isAllowedPic( HttpFile pfile ) {

            String[] types = { "jpg", "gif", "bmp", "png", "jpeg" };
            String[] cfgTypes = config.Instance.Site.UploadPicTypes;
            if (cfgTypes != null && cfgTypes.Length > 0) types = cfgTypes;

            foreach (String ext in types) {
                if (strUtil.IsNullOrEmpty( ext )) continue;
                String extWithDot = ext.StartsWith( "." ) ? ext : "." + ext;
                if (strUtil.EqualsIgnoreCase( Path.GetExtension( pfile.FileName ), extWithDot )) return true;
            }

            return false;
        }

    }
}

