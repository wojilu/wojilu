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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using wojilu.Web;
using wojilu.Web.Utils;
using System.Collections.Generic;


namespace wojilu.Drawing {

    /// <summary>
    /// 图片常用操作
    /// </summary>
    public class Img {

        private static readonly ILog logger = LogManager.GetLogger( typeof( Img ) );
        private static Random rd = new Random();

        /// <summary>
        /// 删除图片以及缩略图。如果图片不存在，则忽略
        /// </summary>
        /// <param name="srcPath">相对网址</param>
        public static void DeleteImgAndThumb( String srcPath ) {

            DeleteImgAndThumb( srcPath, ThumbConfig.GetPhotoConfig() );
        }

        /// <summary>
        /// 删除图片以及指定类型的缩略图。如果图片不存在，则忽略
        /// </summary>
        /// <param name="srcPath">相对网址</param>
        /// <param name="arrThumbType">多个缩略图类型</param>
        public static void DeleteImgAndThumb( String srcPath, Dictionary<String, ThumbInfo> arrThumbType ) {

            if (strUtil.IsNullOrEmpty( srcPath )) return;
            if (srcPath.ToLower().StartsWith( "http://" ) ||
                srcPath.ToLower().StartsWith( "https://" )
                ) return;

            String path = PathHelper.Map( srcPath );
            if (file.Exists( path )) {
                wojilu.IO.File.Delete( path );
            }

            foreach (KeyValuePair<String, ThumbInfo> kv in arrThumbType) {
                String pathThumb = PathHelper.Map( GetThumbPath( srcPath,  kv.Key ) );
                if (file.Exists( pathThumb )) {
                    wojilu.IO.File.Delete( pathThumb );
                }
            }
        }

        /// <summary>
        /// 彻底删除磁盘文件
        /// </summary>
        /// <param name="srcPath">相对路径</param>
        public static void DeleteFile( String srcPath ) {
            if (strUtil.IsNullOrEmpty( srcPath )) return;
            if (srcPath.ToLower().StartsWith( "http://" ) ||
                srcPath.ToLower().StartsWith( "https://" )
                ) return;

            String path = PathHelper.Map( srcPath );
            if (file.Exists( path )) {
                wojilu.IO.File.Delete( path );
            }
        }

        /// <summary>
        /// 获取图片的随机文件名(会添加日期文件夹和随机文件名)，比如 2009/9/28/1530703343314547.jpg
        /// </summary>
        /// <remarks>如果日期文件夹不存在，则在磁盘上自动创建文件夹</remarks>
        /// <param name="pathName">图片存储的绝对路径</param>
        /// <param name="strContentType">图片类型</param>
        /// <returns>返回图片名称，包括所在文件夹，比如 2009/9/28/1530703343314547.jpg</returns>
        public static String GetPhotoName( String absPath, String strContentType ) {
            DateTime now = DateTime.Now;
            String strDir = getDirName( now );

            String strRandom = rd.Next( 100000000, 999999999 ).ToString();

            String strFile = now.Hour.ToString() + now.Minute.ToString() + now.Second.ToString() + now.Millisecond.ToString() + strRandom;
            strFile = strUtil.Join( strFile, GetImageExt( strContentType ), "." );

            String fullDir = Path.Combine( absPath, strDir );
            if (!Directory.Exists( fullDir )) {
                Directory.CreateDirectory( fullDir );
            }
            return Path.Combine( strDir, strFile );
        }

        private static string getDirName( DateTime now ) {
            return string.Format( "{0}/{1}/{2}", now.Year, now.Month, now.Day );
        }

        /// <summary>
        /// 获取上传文件(图片或其他类型)的随机文件名。
        /// <para>返回完整绝对路径，比如 d:\www\static\upload\image\2009\9\28\1530703343314547.zip</para>
        /// </summary>
        /// <param name="ext">文件类型，比如 jpg 或者 png</param>
        /// <returns>返回完整绝对路径，比如 d:\www\static\upload\image\2009\9\28\1530703343314547.zip</returns>
        public static String GetPhotoAbsName( String ext ) {
            String targetPath = GetPhotoName( ext );
            return PathHelper.Map( strUtil.Join( sys.Path.DiskPhoto, targetPath ) );
        }

        /// <summary>
        /// 获取上传文件(图片或其他类型)的随机文件名。
        /// <para>返回 2009/9/28/1530703343314547.jpg 类似格式。</para> 
        /// <para>如果文件夹不存在，则自动创建。</para> 
        /// </summary>
        /// <remarks>如果日期文件夹不存在，则在磁盘上自动创建文件夹，存放在 sys.Path.DiskPhoto 中</remarks>
        /// <param name="ext">文件类型，比如 jpg 或者 png</param>
        /// <returns>返回文件名称，包括所在文件夹，比如 2009/9/28/1530703343314547.zip</returns>
        public static String GetPhotoName( String ext ) {
            return GetFileName( PathHelper.Map( sys.Path.DiskPhoto ), ext );
        }

        /// <summary>
        /// 获取文件的随机文件名(会添加日期文件夹和随机文件名)，比如 2009/9/28/1530703343314547.zip
        /// </summary>
        /// <remarks>如果日期文件夹不存在，则在磁盘上自动创建文件夹</remarks>
        /// <param name="absPath">存储的绝对路径，比如 PathHelper.Map( sys.Path.DiskPhoto )</param>
        /// <param name="fileExt">文件类型，比如 jpg 或者 png</param>
        /// <returns>返回文件名称，包括所在文件夹，比如 2009/9/28/1530703343314547.zip</returns>
        public static String GetFileName( String absPath, String fileExt ) {
            DateTime now = DateTime.Now;
            String strDate = getDirName( now );

            String strRandom = rd.Next( 100000000, 999999999 ).ToString();
            String strFile = now.Hour.ToString() + now.Minute.ToString() + now.Second.ToString() + now.Millisecond.ToString() + strRandom;
            strFile = strUtil.Join( strFile, fileExt, "." );

            String path = Path.Combine( absPath, strDate );
            if (!Directory.Exists( path )) {
                Directory.CreateDirectory( path );
            }
            return Path.Combine( strDate, strFile ).Replace( "\\", "/" );
        }

        /// <summary>
        /// 根据缩略图名称，获取原始图片名称
        /// </summary>
        /// <param name="thumbPath"></param>
        /// <returns></returns>
        public static String GetOriginalPath( String thumbPath ) {

            String path = thumbPath.Trim();

            String ext = Path.GetExtension( path );
            String pathWithoutExt = strUtil.TrimEnd( path, ext );

            if (pathWithoutExt.EndsWith( "_s" )) {
                return strUtil.TrimEnd( pathWithoutExt, "_s" ) + ext;
            }

            if (pathWithoutExt.EndsWith( "_m" )) {
                return strUtil.TrimEnd( pathWithoutExt, "_m" ) + ext;
            }

            if (pathWithoutExt.EndsWith( "_b" )) {
                return strUtil.TrimEnd( pathWithoutExt, "_b" ) + ext;
            }

            return path;
        }

        /// <summary>
        /// 根据原始图片名称，获取缩略图名称(最小的缩略图)
        /// </summary>
        /// <param name="srcPath"></param>
        /// <returns></returns>
        public static String GetThumbPath( Object srcPath ) {

            if (srcPath == null) return null;
            if (strUtil.IsNullOrEmpty( srcPath.ToString() )) return null;
            return GetThumbPath( srcPath, ThumbnailType.Small );
        }

        /// <summary>
        /// 根据原始图片名称和缩略图类型，获取缩略图名称
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="ttype"></param>
        /// <returns></returns>
        public static String GetThumbPath( Object srcPath, ThumbnailType ttype ) {

            if (srcPath == null) return "";
            String path = srcPath.ToString();
            if (strUtil.IsNullOrEmpty( path )) return "";

            String ext = Path.GetExtension( path );
            String pathWithoutExt = strUtil.TrimEnd( path, ext );

            if (pathWithoutExt.EndsWith( "_s" )) pathWithoutExt = strUtil.TrimEnd( pathWithoutExt, "_s" );
            if (pathWithoutExt.EndsWith( "_b" )) pathWithoutExt = strUtil.TrimEnd( pathWithoutExt, "_b" );
            if (pathWithoutExt.EndsWith( "_m" )) pathWithoutExt = strUtil.TrimEnd( pathWithoutExt, "_m" );

            String suffix = "_s";
            if (ttype == ThumbnailType.Medium) {
                suffix = "_m";
            }
            else if (ttype == ThumbnailType.Big) {
                suffix = "_b";
            }

            return pathWithoutExt + suffix + ext;
        }

        /// <summary>
        /// 根据原始图片名称和缩略图后缀(_s或_m等)，获取缩略图名称
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="suffix">请勿添加下划线，系统会自动添加</param>
        /// <returns></returns>
        public static String GetThumbPath( Object srcPath, String suffix ) {

            if (srcPath == null) return "";
            String path = srcPath.ToString();
            if (strUtil.IsNullOrEmpty( path )) return "";

            String ext = Path.GetExtension( path );
            String pathWithoutExt = strUtil.TrimEnd( path, ext );

            if (suffix.StartsWith( "_" ) == false) suffix = "_" + suffix;

            foreach (KeyValuePair<String, ThumbInfo> kv in ThumbConfig.GetPhotoConfig()) {
                pathWithoutExt = strUtil.TrimEnd( pathWithoutExt, "_" + kv.Key );
            }

            return pathWithoutExt + suffix + ext;
        }

        /// <summary>
        /// 保存缩略图到磁盘(可指定宽度，默认自动缩放)
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="destPath"></param>
        /// <param name="width"></param>
        public static void SaveThumbnail( String srcPath, String destPath, int width ) {
            SaveThumbnail( srcPath, destPath, width, width, SaveThumbnailMode.Auto );
        }

        /// <summary>
        /// 保存缩略图到磁盘(可指定宽高，默认自动缩放)
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="destPath"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void SaveThumbnail( String srcPath, String destPath, int width, int height ) {
            SaveThumbnail( srcPath, destPath, width, height, SaveThumbnailMode.Auto );
        }

        /// <summary>
        /// 保存缩略图到磁盘(可指定宽高，可指定缩放模式)
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="destPath"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="mode"></param>
        public static void SaveThumbnail( String srcPath, String destPath, int width, int height, SaveThumbnailMode mode ) {

            try {
                using (Image srcImg = Image.FromFile( srcPath )) {

                    ThumbSize t = getTargetSize( width, height, mode, srcImg );
                    using (Bitmap newImg = new Bitmap( t.New.Width, t.New.Height )) {

                        using (Graphics g = Graphics.FromImage( newImg )) {
                            g.InterpolationMode = InterpolationMode.High;
                            g.SmoothingMode = SmoothingMode.HighQuality;
                            g.Clear( Color.White );
                            g.DrawImage( srcImg, t.getNewRect(), t.getRect(), GraphicsUnit.Pixel );

                            try {
                                newImg.Save( destPath, ImageFormat.Jpeg );
                            }
                            catch (Exception e) {
                                throw e;
                            }

                        }
                    }

                }
            }
            catch (OutOfMemoryException ex) {
                logger.Error( "file format error: " + srcPath );
            }
        }

        /// <summary>
        /// 根据图片绝对路径，获取图片的大小
        /// </summary>
        /// <param name="absPhotoPath"></param>
        /// <returns></returns>
        public static Size GetPhotoSize( String absPhotoPath ) {
            using (Image img = Image.FromFile( absPhotoPath )) {
                return img.Size;
            }
        }

        /// <summary>
        /// 获取缩略图尺寸
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="mode"></param>
        /// <param name="src"></param>
        /// <returns></returns>
        public static ThumbSize getTargetSize( int width, int height, SaveThumbnailMode mode, Image src ) {

            ThumbSize t = new ThumbSize();
            t.Src = new Size( src.Width, src.Height );
            t.Point = new Point( 0, 0 );

            if (mode == SaveThumbnailMode.ByWidth) {
                int newHeight = src.Height * width / src.Width;
                t.New = new Size( width, newHeight );
                return t;
            }
            else if (mode == SaveThumbnailMode.ByHeight) {
                int newWidth = src.Width * height / src.Height;
                t.New = new Size( newWidth, height );
                return t;
            }
            else if (mode == SaveThumbnailMode.ByWidthHeight) {
                t.New = new Size( width, height );
                return t;
            }
            else if (mode == SaveThumbnailMode.Cut) {
                return getCutSize( width, height, mode, src );
            }

            return getAutoSize( width, height, mode, src );
        }

        private static ThumbSize getAutoSize( int width, int height, SaveThumbnailMode mode, Image src ) {
            ThumbSize t = new ThumbSize();
            t.Src = new Size( src.Width, src.Height );
            t.Point = new Point( 0, 0 );

            int newWidth = width;
            int newHeight = height;
            if ((double)src.Width / (double)src.Height > (double)width / (double)height)
                newHeight = src.Height * width / src.Width;
            else
                newWidth = src.Width * height / src.Height;
            t.New = new Size( newWidth, newHeight );

            return t;

        }

        private static ThumbSize getCutSize( int width, int height, SaveThumbnailMode mode, Image src ) {

            ThumbSize t = new ThumbSize();

            if ((double)src.Width / (double)src.Height > (double)width / (double)height) {

                int newWidth = src.Height * width / height;
                t.Src = new Size( newWidth, src.Height );
                t.New = new Size( width, height );
                t.Point = new Point( (src.Width - newWidth) / 2, 0 );
            }
            else {

                int newHeight = src.Width * height / width;
                t.Src = new Size( src.Width, newHeight );
                t.New = new Size( width, height );
                t.Point = new Point( 0, (src.Height - newHeight) / 2 );

            }

            return t;
        }


        /// <summary>
        /// 获取图片的后缀名(不包括点号)
        /// </summary>
        /// <param name="strContentType"></param>
        /// <returns></returns>
        public static String GetImageExt( String strContentType ) {

            if (strUtil.IsNullOrEmpty( strContentType )) return null;

            String t = strContentType.ToLower();

            logger.Info( "strContentType=>" + t );

            if (t.Equals( ".gif" )) return "gif";
            if (t.Equals( ".bmp" )) return "bmp";
            if (t.Equals( ".png" )) return "png";
            if (t.Equals( ".jpg" )) return "jpg";
            if (t.Equals( ".jpeg" )) return "jpeg";

            if (t.Equals( "gif" )) return "gif";
            if (t.Equals( "bmp" )) return "bmp";
            if (t.Equals( "png" )) return "png";
            if (t.Equals( "jpg" )) return "jpg";
            if (t.Equals( "jpeg" )) return "jpeg";

            if (t.Equals( "image/gif" )) return "gif";
            if (t.Equals( "image/bmp" )) return "bmp";
            if (t.Equals( "image/tiff" )) return "tiff";
            if (t.Equals( "image/x-icon" )) return "icon";
            if (t.Equals( "image/x-png" )) return "png"; // ie
            if (t.Equals( "image/png" )) return "png"; // firefox, google

            if (t.Equals( "image/x-emf" )) return "emf";
            if (t.Equals( "image/x-exif" )) return "exif";
            if (t.Equals( "image/x-wmf" )) return "wmf";
            if (t.Equals( "image/pjpeg" )) return "jpg"; // ie6
            if (t.Equals( "image/jpg" )) return "jpg"; // ie8
            if (t.Equals( "image/jpeg" )) return "jpg"; // firefox, google

            return "jpg";
        }

        /// <summary>
        /// 获取图片的类型
        /// </summary>
        /// <param name="strContentType"></param>
        /// <returns></returns>
        public static ImageFormat GetImageType( String strContentType ) {

            if (strUtil.IsNullOrEmpty( strContentType )) return null;

            String t = strContentType.ToLower();

            if (t.Equals( ".gif" )) return ImageFormat.Gif;
            if (t.Equals( ".bmp" )) return ImageFormat.Bmp;
            if (t.Equals( ".png" )) return ImageFormat.Png;
            if (t.Equals( ".jpg" )) return ImageFormat.Jpeg;
            if (t.Equals( ".jpeg" )) return ImageFormat.Jpeg;

            if (t.Equals( "gif" )) return ImageFormat.Gif;
            if (t.Equals( "bmp" )) return ImageFormat.Bmp;
            if (t.Equals( "png" )) return ImageFormat.Png;
            if (t.Equals( "jpg" )) return ImageFormat.Jpeg;
            if (t.Equals( "jpeg" )) return ImageFormat.Jpeg;

            if (t.Equals( "image/pjpeg" )) return ImageFormat.Jpeg;
            if (t.Equals( "image/gif" )) return ImageFormat.Gif;
            if (t.Equals( "image/bmp" )) return ImageFormat.Bmp;
            if (t.Equals( "image/tiff" )) return ImageFormat.Tiff;
            if (t.Equals( "image/x-icon" )) return ImageFormat.Icon;
            if (t.Equals( "image/x-png" )) return ImageFormat.Png;
            if (t.Equals( "image/png" )) return ImageFormat.Png;
            if (t.Equals( "image/x-emf" )) return ImageFormat.Emf;
            if (t.Equals( "image/x-exif" )) return ImageFormat.Exif;
            if (t.Equals( "image/x-wmf" )) return ImageFormat.Wmf;

            return ImageFormat.MemoryBmp;
        }

        /// <summary>
        /// 将图片拷贝到上传目录中，并生成中、小缩略图
        /// </summary>
        /// <param name="oPath">原始相对路径</param>
        /// <returns>返回图片名称，包括所在文件夹，比如 2009/9/28/1530703343314547.jpg</returns>
        public static string CopyToUploadPath( String oPath ) {
            oPath = PathHelper.Map( oPath );

            // 需要保存的路径
            String shortPath = Img.GetPhotoName( Path.GetExtension( oPath ) );
            String targetPath = PathHelper.Map( strUtil.Join( sys.Path.DiskPhoto, shortPath ) );
            logger.Info( "copy pic. oPath=" + oPath + ", target=" + targetPath );

            // 1) 复制原图
            file.Copy( oPath, targetPath );

            Dictionary<String, ThumbInfo> thumbConfigMap = ThumbConfig.GetPhotoConfig();
            foreach (KeyValuePair<String, ThumbInfo> kv in thumbConfigMap) {

                String sPath = Img.GetThumbPath( targetPath, kv.Key );
                Img.SaveThumbnail( oPath, sPath, kv.Value.Width, kv.Value.Height, kv.Value.Mode );

            }

            return shortPath;
        }

    }
}

