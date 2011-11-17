/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.IO;

using wojilu.ORM;
using wojilu.Common.Skins;

namespace wojilu.Members.Users.Domain {
    
    [Table("Skin")]
    [Serializable]
    public class SpaceSkin : ObjectBase<SpaceSkin>, ISkin {

        public SpaceSkin() { }

        public SpaceSkin( String name, String stylePath, String thumbPath ) {
            this.Name = name;
            this.StylePath = stylePath;
            this.ThumbUrl = thumbPath;
        }

        public int CatId {get;set;}
        public int TypeId {get;set;}

        public int MemberId {get;set;}
        public String MemberName {get;set;}
        public String MemberUrl {get;set; }


        public String Name {get;set; }
        public String Description {get;set;}
        public String ThumbUrl {get;set; }
        public String StylePath {get;set;}

        [LongText]
        public String Body { get; set; }

        public int Status {get;set;}

        public int Hits {get;set;}
        public int MemberCount {get;set;}
        public int Replies {get;set;}
        public int Score {get;set;}

        public DateTime CreateTime {get;set;}

        //-------------------------------------------------------------------

        public String GetSkinPath() {

            if (PathHelper.IsFullUrl( this.StylePath )) return this.StylePath;
            return strUtil.Join( sys.Path.SpaceSkin, this.StylePath );
        }

        public String GetSkinContent() {
            if (this.MemberId > 0) return this.Body;

            String spath = strUtil.Join( sys.Path.DiskSpaceSkin, this.StylePath );
            String skinPath = PathHelper.Map( spath );
            return file.Read( skinPath );
        }

        public String GetThumbPath() {

            if (PathHelper.IsFullUrl( this.ThumbUrl )) return this.ThumbUrl;
            return strUtil.Join( sys.Path.SpaceSkin, this.ThumbUrl );
        }

        public String GetScreenShotPath() {
            if (PathHelper.IsFullUrl( this.ThumbUrl )) return "#";
            String fileName = Path.GetFileName( ThumbUrl );
            String extension = Path.GetExtension( ThumbUrl );
            return (sys.Path.SpaceSkin + strUtil.TrimEnd( ThumbUrl, fileName ) + "screen_shot" + extension);
        }



    }
}

