using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Photo.Domain;

namespace wojilu.Apps.Photo.Helper {

    public class PhotoInfoHelper {

        public virtual Dictionary<String, PhotoInfo> GetInfo( String str ) {

            Dictionary<String, PhotoInfo> dic = new Dictionary<String, PhotoInfo>();
            if (strUtil.IsNullOrEmpty( str )) return dic;

            String[] arr = str.Split( ',' );
            foreach (String item in arr) {

                String key = getKey( item );
                PhotoInfo size = getSize( item );

                if (strUtil.IsNullOrEmpty( key )) continue;
                if (size == null) continue;

                dic.Add( key, size );
            }
            return dic;
        }

        //s=68/68
        private String getKey( String item ) {
            if (strUtil.IsNullOrEmpty( item )) return null;
            String[] arr = item.Split( '=' );
            if (arr.Length != 2) return null;
            return arr[0].Trim();
        }

        private PhotoInfo getSize( String item ) {
            if (strUtil.IsNullOrEmpty( item )) return null;
            String[] arr = item.Split( '=' );
            if (arr.Length != 2) return null;

            String[] wh = arr[1].Trim().Split( '/' );
            if (wh.Length != 2) return null;
            PhotoInfo x = new PhotoInfo();
            x.Width = cvt.ToInt( wh[0] );
            x.Height = cvt.ToInt( wh[1] );

            if (x.Width <= 0 || x.Height <= 0) return null;

            return x;
        }

        public virtual String ConvertString( Dictionary<String, PhotoInfo> dic ) {
            String str = "";
            foreach (KeyValuePair<String, PhotoInfo> kv in dic) {
                str += kv.Key + "=";
                str += kv.Value.Width + "/";
                str += kv.Value.Height + ",";
            }
            return str.TrimEnd( ',' );
        }
    }
}
