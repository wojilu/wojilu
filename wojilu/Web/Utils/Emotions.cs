using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Serialization;

namespace wojilu.Web.Utils {

    internal class Emotions {

        internal static readonly String emJson = getEmotionJson();

        private static string getEmotionJson() {

            String filePath = PathHelper.Map( strUtil.Join( sys.Path.DiskJs, "wojilu.core.ems.js" ) );
            if (file.Exists( filePath ) == false) return null;

            String emstr = file.Read( filePath );
            if (strUtil.IsNullOrEmpty( emstr )) return null;

            int idxEms = emstr.IndexOf( "ems" );
            emstr = emstr.Substring( idxEms ).Trim();
            emstr = strUtil.TrimStart( emstr, "ems" ).Trim();
            emstr = strUtil.TrimStart( emstr, ":" ).Trim();
            emstr = strUtil.TrimEnd( emstr, "});" ).Trim();
            emstr = strUtil.TrimEnd( emstr, "};" ).Trim();

            return emstr;
        }

        internal static Dictionary<string, string> GetEmotions() {

            Dictionary<string, string> dic = new Dictionary<string, string>();

            Dictionary<string, object> map = getEmotionMap();
            foreach (KeyValuePair<String, object> kv in map) {

                String picPath = getPicPath( kv.Key );

                dic.Add( kv.Value.ToString(), picPath );

            }

            return dic;
        }

        private static Dictionary<string, object> getEmotionMap() {

            Dictionary<string, object> map = JsonParser.Parse( Emotions.emJson ) as Dictionary<string, object>;

            return map;
        }


        private static string getPicPath( string key ) {
            key = key.TrimStart( '$' );
            return strUtil.Join( sys.Path.Js, "editor/skin/em/" + key + ".gif" );
        }


    }

}
