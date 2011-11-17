/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.IO;

using wojilu;
using wojilu.ORM;
using wojilu.Common.Skins;

namespace wojilu.Members.Groups.Domain {

    [Serializable]
    public class GroupSkin : ObjectBase<GroupSkin>, ISkin {

        public GroupSkin() { }

        public GroupSkin( String name, String style, String thumb ) {
            this.Name = name;
            this.ThumbUrl = thumb;
            this.StylePath = style;
        }

        public int MemberId {get;set;}

        public String Name {get;set; }
        public String Description {get;set;}
        public String ThumbUrl {get;set; }
        public String StylePath {get;set;}

        [LongText]
        public String Body { get; set; }

        public int Status { get; set; }


        public int Hits {get;set;}
        public int Replies {get;set;}
        public int MemberCount {get;set;}

        public DateTime CreateTime {get;set;}

        //-------------------------------------------------------------------

        public String GetSkinPath() {

            if (PathHelper.IsFullUrl( this.StylePath )) return this.StylePath;
            return strUtil.Join( sys.Path.GroupSkin, this.StylePath );
        }

        public String GetSkinContent() {
            if (this.MemberId > 0) return this.Body;

            String spath = strUtil.Join( sys.Path.DiskGroupSkin, this.StylePath );
            String skinPath = PathHelper.Map( spath );
            return file.Read( skinPath );
        }

        public String GetThumbPath() {

            if (PathHelper.IsFullUrl( this.ThumbUrl )) return this.ThumbUrl;
            return strUtil.Join( sys.Path.GroupSkin, this.ThumbUrl );
        }

        public String GetScreenShotPath() {
            if (PathHelper.IsFullUrl( this.ThumbUrl )) return "#";
            String fileName = Path.GetFileName( ThumbUrl );
            String extension = Path.GetExtension( ThumbUrl );
            return (sys.Path.GroupSkin + strUtil.TrimEnd( ThumbUrl, fileName ) + "screen_shot" + extension);
        }



    }
}

