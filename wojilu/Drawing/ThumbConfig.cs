using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Drawing {

    public class ThumbConfig {

        /// <summary>
        /// 头像缩略图的配置。默认值 s=width:48|height:48|mode:cut, m=width:100|height:100|mode:cut, b=width:200|height:200|mode:cut
        /// <para>mode含义依次是：auto(自动)，cut(裁切)，x(根据宽度)，y(根据高度)，xy(根据宽高)</para>
        /// </summary>
        /// <returns></returns>
        public static Dictionary<String, ThumbInfo> GetAvatarConfig() {
            return config.Instance.Site.GetAvatarThumbConfig();
        }

        public static ThumbInfo GetAvatar( String key ) {
            ThumbInfo x;
            GetAvatarConfig().TryGetValue( key, out x );
            return x;
        }

        /// <summary>
        /// 上传图片的缩略图配置信息。默认值 s=width:170|height:170|mode:cut, m=width:600|height:600|mode:auto, b=width:1024|height:1024|mode:auto
        /// </summary>
        /// <returns></returns>
        public static Dictionary<String, ThumbInfo> GetPhotoConfig() {
            return config.Instance.Site.GetPhotoThumbConfig(); 
        }

        public static ThumbInfo GetPhoto( String key ) {
            ThumbInfo x;
            GetPhotoConfig().TryGetValue( key, out x );
            return x;
        }

        /// <summary>
        /// 从配置字符串中获取缩略图的配置信息。
        /// </summary>
        /// <example>
        /// <code>s=width:75|height:75|mode:auto, sx=width:200|height:200|mode:auto, m=width:600|height:600|mode:auto, b=width:1024|height:1024|mode:auto</code>
        /// </example>
        /// <param name="cfgString"></param>
        /// <returns></returns>
        public static Dictionary<String, ThumbInfo> ReadString( String cfgString ) {

            Dictionary<String, ThumbInfo> ret = new Dictionary<String, ThumbInfo>();

            if (strUtil.IsNullOrEmpty( cfgString )) return ret;

            String[] arr = cfgString.Split( ',' );

            foreach (String item in arr) {

                if (strUtil.IsNullOrEmpty( item )) continue;

                String name = getConfigName( item );
                if (name == null) continue;

                ThumbInfo thumbInfo = getThumbInfo( item );
                if (thumbInfo == null) continue;


                ret.Add( name.ToLower(), thumbInfo );
            }

            return ret;
        }

        private static string getConfigName( string item ) {
            if (strUtil.IsNullOrEmpty( item )) return null;
            String[] arr = item.Split( '=' );
            if (arr.Length != 2) return null;
            return arr[0].Trim();
        }

        private static ThumbInfo getThumbInfo( string item ) {

            if (strUtil.IsNullOrEmpty( item )) return null;
            String[] arr = item.Split( '=' );
            if (arr.Length != 2) return null;

            return getThumbInfoPrivate( arr[1].Trim() );
        }

        //width:75|height:75|mode:auto
        private static ThumbInfo getThumbInfoPrivate( string str ) {

            ThumbInfo x = new ThumbInfo();

            String[] arr = str.Split( '|' );
            foreach (String rItem in arr) {

                if (strUtil.IsNullOrEmpty( rItem )) continue;

                String[] arrValue = rItem.Trim().Split( ':' );
                if (arrValue.Length != 2) continue;

                String key = arrValue[0].Trim();
                String val = arrValue[1].Trim();

                if (strUtil.IsNullOrEmpty( key )) continue;

                if (strUtil.EqualsIgnoreCase( key, "width" )) {
                    x.Width = getWidth( val );
                    continue;
                }

                if (strUtil.EqualsIgnoreCase( key, "height" )) {
                    x.Height = getHeight( val );
                    continue;
                }

                if (strUtil.EqualsIgnoreCase( key, "mode" )) {
                    x.Mode = getMode( val );
                    continue;
                }

            }

            return x;
        }

        private static int getWidth( string val ) {
            return cvt.ToInt( val.Trim() );
        }

        private static int getHeight( string val ) {
            return cvt.ToInt( val.Trim() );
        }

        private static SaveThumbnailMode getMode( string mode ) {
            if (strUtil.IsNullOrEmpty( mode )) return SaveThumbnailMode.Auto;
            if (strUtil.EqualsIgnoreCase( mode, "cut" )) return SaveThumbnailMode.Cut;
            if (strUtil.EqualsIgnoreCase( mode, "x" )) return SaveThumbnailMode.ByWidth;
            if (strUtil.EqualsIgnoreCase( mode, "y" )) return SaveThumbnailMode.ByHeight;
            if (strUtil.EqualsIgnoreCase( mode, "xy" )) return SaveThumbnailMode.ByWidthHeight;
            return SaveThumbnailMode.Auto;
        }


    }
}
