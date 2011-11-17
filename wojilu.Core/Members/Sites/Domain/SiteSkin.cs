/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.IO;

using wojilu;
using wojilu.ORM;
using wojilu.Common.Skins;
using wojilu.Data;

namespace wojilu.Members.Sites.Domain {

    [Serializable]
    public class SiteSkin : CacheObject {

        public String Description {get;set;}
        public String ThumbUrl {get;set; }
        public String StylePath {get;set;}

        public DateTime Created {get;set;}

        public String Body { get; set; }

        //-------------------------------------------------------------------

        public String GetSkinPath() {

            if (PathHelper.IsFullUrl( this.StylePath )) return this.StylePath;
            return strUtil.Join( sys.Path.SiteSkin, this.StylePath );
        }

        //public String GetSkinContent() {
        //    String spath = strUtil.Join( sys.Path.DiskSiteSkin, this.StylePath );
        //    String skinPath = PathHelper.Map( spath );
        //    return file.Read( skinPath );
        //}

        public String GetThumbPath() {

            if (PathHelper.IsFullUrl( this.ThumbUrl )) return this.ThumbUrl;
            return strUtil.Join( sys.Path.SiteSkin, this.ThumbUrl );
        }

        public String GetScreenShotPath() {
            if (PathHelper.IsFullUrl( this.ThumbUrl )) return "#";
            String fileName = Path.GetFileName( ThumbUrl );
            String extension = Path.GetExtension( ThumbUrl );
            return (sys.Path.SiteSkin + strUtil.TrimEnd( ThumbUrl, fileName ) + "screen_shot" + extension);
        }



    }
}

