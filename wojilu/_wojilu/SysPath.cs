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
using System.Collections.Generic;
using System.Web;

using wojilu.Config;
using wojilu.Drawing;
using wojilu.Web;
using wojilu.Web.Mvc;

namespace wojilu {

    /// <summary>
    /// 系统路径(路径末尾有斜杠"/")
    /// </summary>
    /// <remarks>所有的路径末尾都有斜杠"/"，以区别于没有斜杠作后缀的文件名</remarks>
    public class SysPath {

        private SysPath() { }

        /// <summary>
        /// 系统路径信息(全局缓存)
        /// </summary>
        public static SysPath Instance = new SysPath();

        public String Root { get { return SystemInfo.RootPath; } }

        //------------------------------------ 内部请求 ---------------------------------------------------      
        public String DiskStatic { get { return StaticPath.Instance.DiskStatic; } }

        public String DiskSpaceSkin { get { return StaticPath.Instance.DiskSpaceSkin; } }
        public String DiskGroupSkin { get { return StaticPath.Instance.DiskGroupSkin; } }
        public String DiskSiteSkin { get { return StaticPath.Instance.DiskSiteSkin; } }

        public String DiskJs { get { return StaticPath.Instance.DiskJs; } }
        public String DiskImg { get { return StaticPath.Instance.DiskImg; } }
        public String DiskCss { get { return StaticPath.Instance.DiskCss; } }

        public String DiskUpload { get { return StaticPath.Instance.DiskUpload; } }
        public String DiskPhoto { get { return StaticPath.Instance.DiskPhoto; } }
        public String DiskAvatar { get { return StaticPath.Instance.DiskAvatar; ; } }
        public String DiskGroupLogo { get { return StaticPath.Instance.DiskGroupLogo; } }

        //------------------------------------ 外部请求(静态化) ---------------------------------------------------

        public String Static { get { return StaticPath.Instance.Static; } }
        public String Upload { get { return StaticPath.Instance.Upload; } }

        public String Avatar { get { return StaticPath.Instance.Avatar; } }
        public String AvatarGuest { get { return StaticPath.Instance.AvatarGuest; } }

        public String Photo { get { return StaticPath.Instance.Photo; } }
        public String GroupLogo { get { return StaticPath.Instance.GroupLogo; } }

        public String SpaceSkin { get { return StaticPath.Instance.SpaceSkin; } }
        public String GroupSkin { get { return StaticPath.Instance.GroupSkin; } }
        public String SiteSkin { get { return StaticPath.Instance.SiteSkin; } }

        public String Editor { get { return StaticPath.Instance.Editor; } }
        public String Img { get { return StaticPath.Instance.Img; } }
        public String ImgStar { get { return strUtil.Join( Img, "Star/" ); } }
        public String Js { get { return StaticPath.Instance.Js; } }
        public String Skin { get { return StaticPath.Instance.Skin; } }
        public String Css { get { return StaticPath.Instance.Css; } }


        /************************************* 方法 *********************************************/

        public String GetPhotoOriginal( String relativeUrl ) {

            if (strUtil.IsNullOrEmpty( relativeUrl )) return null;
            if (relativeUrl.ToLower().StartsWith( "http://" )) return relativeUrl;
            if (relativeUrl.StartsWith( "/" )) return relativeUrl;

            if (strUtil.IsNullOrEmpty( relativeUrl )) return "";
            if (relativeUrl.StartsWith( sys.Path.Photo )) return relativeUrl;
            return strUtil.Join( sys.Path.Photo, relativeUrl );
        }

        public String GetPhotoThumb( String relativeUrl ) {
            return GetPhotoThumb( relativeUrl, ThumbnailType.Small );
        }

        public String GetPhotoThumb( String relativeUrl, ThumbnailType ttype ) {

            if (strUtil.IsNullOrEmpty( relativeUrl )) return null;
            if (relativeUrl.ToLower().StartsWith( "http://" )) return relativeUrl;
            if (relativeUrl.StartsWith( "/" )) return relativeUrl;

            return wojilu.Drawing.Img.GetThumbPath( GetPhotoOriginal( relativeUrl ), ttype );
        }

        /// <summary>
        /// 获取图片的缩略图
        /// </summary>
        /// <param name="relativeUrl"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public String GetPhotoThumb( String relativeUrl, String suffix ) {

            if (strUtil.IsNullOrEmpty( relativeUrl )) return null;
            if (relativeUrl.ToLower().StartsWith( "http://" )) return relativeUrl;
            if (relativeUrl.StartsWith( "/" )) return relativeUrl;

            return wojilu.Drawing.Img.GetThumbPath( GetPhotoOriginal( relativeUrl ), suffix );
        }

        public String GetPhotoRelative( String originalUrl ) {
            if (strUtil.IsNullOrEmpty( originalUrl )) return "";
            if (originalUrl.StartsWith( sys.Path.Photo )) {
                return strUtil.TrimStart( originalUrl, sys.Path.Photo ).TrimStart( '/' );
            }
            return originalUrl;
        }

        public String GetAvatarOriginal( String relativeUrl ) {
            relativeUrl = processEmptyAvatar( relativeUrl );
            if (relativeUrl.StartsWith( sys.Path.Avatar )) {
                return wojilu.Drawing.Img.GetOriginalPath( relativeUrl );
            }
            else {
                return wojilu.Drawing.Img.GetOriginalPath( sys.Path.Avatar + relativeUrl );
            }
        }

        public String GetAvatarThumb( String relativeUrl ) {
            return GetAvatarThumb( relativeUrl, ThumbnailType.Small );
        }

        public String GetAvatarThumb( String relativeUrl, String suffix ) {

            if (strUtil.IsNullOrEmpty( relativeUrl )) return null;
            if (relativeUrl.ToLower().StartsWith( "http://" )) return relativeUrl;
            if (relativeUrl.StartsWith( "/" )) return relativeUrl;

            String originalAvatar = GetAvatarOriginal( relativeUrl );
            return wojilu.Drawing.Img.GetThumbPath( originalAvatar, suffix );
        }

        public String GetAvatarThumb( String relativeUrl, ThumbnailType ttype ) {
            String originalAvatar = GetAvatarOriginal( relativeUrl );
            return wojilu.Drawing.Img.GetThumbPath( originalAvatar, ttype );
        }

        private String processEmptyAvatar( String relativeUrl ) {
            if (strUtil.IsNullOrEmpty( relativeUrl )) return AvatarConstString;
            return relativeUrl;
        }

        public String GetGroupLogoOriginal( String relativeUrl ) {
            relativeUrl = processEmptyGroupLogo( relativeUrl );
            if (relativeUrl.StartsWith( sys.Path.GroupLogo )) {
                return wojilu.Drawing.Img.GetOriginalPath( relativeUrl );
            }
            else {
                return wojilu.Drawing.Img.GetOriginalPath( sys.Path.GroupLogo + relativeUrl );
            }
        }

        public String GetGroupLogoThumb( String relativeUrl ) {
            return GetGroupLogoThumb( relativeUrl, ThumbnailType.Small );
        }

        public String GetGroupLogoThumb( String relativeUrl, ThumbnailType ttype ) {
            String originalGroup = GetGroupLogoOriginal( relativeUrl );
            return wojilu.Drawing.Img.GetThumbPath( originalGroup, ttype );
        }

        private String processEmptyGroupLogo( String relativeUrl ) {
            if (strUtil.IsNullOrEmpty( relativeUrl )) return GroupLogoConstString;
            return relativeUrl;
        }

        public static readonly String AvatarConstString = "face/guest.jpg";
        public static readonly String GroupLogoConstString = "group.jpg";
    }

}