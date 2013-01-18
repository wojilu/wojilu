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
using System.Text;

namespace wojilu.Web.Mvc {

    /// <summary>
    /// 静态文件的路径
    /// </summary>
    public class StaticPath {

        // 必须放在第一行
        private static String StaticPathRoot = "static/"; 

        private StaticPath() {
        }

        public static StaticPath Instance = getStaticPath();

        private static StaticPath getStaticPath() {

            StaticPath p = new StaticPath();

            p.Domain = MvcConfig.Instance.StaticDomain;
            p.Static = getStatic( p.Domain );

            p.Img = pathIn( p.Domain, "img" );
            p.Js = pathIn( p.Domain, "js" );
            p.Css = pathIn( p.Domain, "css" );
            p.Skin = pathIn( p.Domain, "skin" );

            p.Upload = pathIn( p.Domain, "upload" );
            p.Avatar = p.Upload;
            p.AvatarGuest = strUtil.Join( p.Avatar, SysPath.AvatarConstString );
            p.Photo = p.Upload + "image/";
            p.GroupLogo = p.Upload + "grouplogo/";

            p.SpaceSkin = p.Skin + "space/";
            p.GroupSkin = p.Skin + "group/";
            p.SiteSkin = p.Skin + "site/";

            p.Editor = p.Js + "editor/";

            //---------------------------------------

            p.DiskStatic = getStaticDisk();

            p.DiskImg = getDiskPath( "img" );
            p.DiskJs = getDiskPath( "js" );
            p.DiskCss = getDiskPath( "css" );

            p.DiskSpaceSkin = getDiskPath("skin/space");
            p.DiskGroupSkin = getDiskPath ("skin/group");
            p.DiskSiteSkin = getDiskPath( "skin/site" );

            p.DiskUpload = getDiskPath( "upload" );
            p.DiskPhoto = p.DiskUpload + "image";
            p.DiskAvatar = p.DiskUpload + "face";
            p.DiskGroupLogo = p.DiskUpload + "grouplogo";

            return p;
        }


        public String Domain { get; set; }
        public String Static { get; set; }

        public String Upload { get; set; }

        public String Img { get; set; }
        public String Js { get; set; }
        public String Css { get; set; }
        public String Skin { get; set; }

        public String Avatar { get; set; }
        public String AvatarGuest { get; set; }
        public String Photo { get; set; }
        public String GroupLogo { get; set; }

        public String SpaceSkin { get; set; }
        public String GroupSkin { get; set; }
        public String SiteSkin { get; set; }

        public String Editor { get; set; }


        //--------------------------
        public String DiskStatic { get; set; }

        public String DiskJs { get; set; }
        public String DiskImg { get; set; }
        public String DiskCss { get; set; }

        public String DiskSpaceSkin { get; set; }
        public String DiskGroupSkin { get; set; }
        public String DiskSiteSkin { get; set; }

        public String DiskUpload { get; set; }
        public String DiskPhoto { get; set; }
        public String DiskAvatar { get; set; }
        public String DiskGroupLogo { get; set; }

        /******************************************************************/


        private static String pathIn( String sdomain, String path ) {
            if (strUtil.IsNullOrEmpty( sdomain )) return SystemInfo.RootPath + StaticPathRoot + path + "/";
            return strUtil.Join( getDomainPath( sdomain ), path ) + "/";
        }

        private static String getDomainPath( String sdomain ) {
            String hostNoWWW = strUtil.TrimStart( SystemInfo.Host, "www." );
            return sys.Url.SchemeStr + sdomain + "." + hostNoWWW;
        }


        private static String getStatic( String sdomain ) {
            if (strUtil.IsNullOrEmpty( sdomain )) return getStaticDisk();
            return strUtil.Append( getDomainPath( sdomain ), "/" );
        }

        private static String getStaticDisk() {
            return SystemInfo.RootPath + StaticPathRoot;
        }

        private static String getDiskPath( String path ) {
            return SystemInfo.RootPath + StaticPathRoot + path + "/";
        }

    }

}
