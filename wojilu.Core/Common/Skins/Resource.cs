using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;

namespace wojilu.Common.Skins {

    public class ResourceType {

        public static readonly int Texture = 0;
        public static readonly int Icon = 1;
        public static readonly int Pic = 2;

        public static String GetTypeName( int typeId ) {
            if (typeId == 0) return "背景纹理";
            if (typeId == 1) return "各类图标";
            if (typeId == 2) return "主题图片";
            return "";
        }

    }

    [Serializable]
    public class Resource : ObjectBase<Resource> {

        public int TypeId { get; set; }
        public String Name { get; set; }
        public String Url { get; set; }

        [NotSave]
        public String ImgUrl {
            get { return sys.Path.GetPhotoOriginal( this.Url ); }
        }

        [NotSave]
        public String ImgMediumUrl {
            get {
                return sys.Path.GetPhotoThumb( this.Url, wojilu.Drawing.ThumbnailType.Medium );
            }
        }

        [NotSave]
        public String ImgThumbUrl {
            get { return sys.Path.GetPhotoThumb( this.Url ); }
        }

        //---------------------------------------------------------------------------

        public static DataPage<Resource> GetPage( int typeId, int pageSize ) {
            return Resource.findPage( "TypeId=" + typeId, pageSize );
        }

        public static void Delete( Resource r ) {
            r.delete();

            String picPath = strUtil.Join( sys.Path.DiskPhoto, r.Url );
            wojilu.Drawing.Img.DeleteImgAndThumb( picPath );

        }

    }

}
