using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web;
using wojilu.Drawing;
using wojilu.Web.Utils;
using wojilu.DI;
using wojilu.Members.Interface;

namespace wojilu.Members {

    /// <summary>
    /// 继承自IMember对象的帮助方法，用于logo的保存，缩略图路径、皮肤路径等的获取
    /// </summary>
    public class MemberHelper {

        private static Dictionary<Type, String> Types = getMemberTypes();

        private static Dictionary<Type, string> getMemberTypes() {

            Dictionary<Type, String> map = new Dictionary<Type, string>();

            foreach (KeyValuePair<String, Type> kv in ObjectContext.Instance.TypeList) {

                if (rft.IsInterface( kv.Value, typeof( IMember ) ) == false) continue;

                IMember obj = rft.GetInstance( kv.Value ) as IMember;

                map.Add( kv.Value, obj.GetUrl() );

            }

            return map;
        }

        public static Dictionary<Type, String> GetMemberTypes() {
            return MemberHelper.Types;
        }

        private int _logoWidth = 120;
        private int _logoHeight = 120;
        private String _picExt = ".jpg";

        /// <summary>
        /// logo的宽度，默认是120px
        /// </summary>
        public int LogoWidth {
            get { return _logoWidth; }
            set { _logoWidth = value; }
        }

        /// <summary>
        /// logo的高度，默认是120px
        /// </summary>
        public int LogoHeight {
            get { return _logoHeight; }
            set { _logoHeight = value; }
        }

        /// <summary>
        /// logo图片的后缀名，包括点号，默认是.jpg
        /// </summary>
        public String PicExt {
            get { return _picExt; }
            set { _picExt = value; }
        }

        protected String memberPath { get; set; }

        public String GetLogoOriginal( String relativeUrl ) {
            relativeUrl = processEmptyLogo( relativeUrl );
            return wojilu.Drawing.Img.GetOriginalPath( LogoPath + relativeUrl );
        }

        public String GetLogoThumb( String relativeUrl ) {
            return GetLogoThumb( relativeUrl, ThumbnailType.Small );
        }

        public String GetLogoThumb( String relativeUrl, ThumbnailType ttype ) {
            String original = GetLogoOriginal( relativeUrl );
            return wojilu.Drawing.Img.GetThumbPath( original, ttype );
        }



        private String LogoPath {
            get { return strUtil.Join( sys.Path.Upload, memberPath + "/" ); }
        }

        private String LogoDiskPath {
            get { return strUtil.Join( sys.Path.DiskUpload, memberPath + "/" ); }
        }
        
        public String SkinDiskPath {
            get { return strUtil.Join( sys.Path.DiskStatic, "skin/" + memberPath + "/" ); }
        }

        public Result SaveLogo( HttpFile afile, String url ) {
            return Uploader.SaveImg( this.LogoDiskPath, afile, url, this.LogoWidth, this.LogoHeight, SaveThumbnailMode.Cut );
        }

        //------------------------------------------------------

        private String processEmptyLogo( String relativeUrl ) {
            if (strUtil.IsNullOrEmpty( relativeUrl )) return logoPicName;
            return relativeUrl;
        }

        private String logoPicName {
            get { return memberPath + this.PicExt; }
        }

        public String SkinPath {
            get { return strUtil.Join( sys.Path.Skin, memberPath + "/" ); }
        }

    }




}
