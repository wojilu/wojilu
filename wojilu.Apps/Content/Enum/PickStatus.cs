using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Apps.Content.Enum {

    public class PickStatus {

        /// <summary>
        /// 普通
        /// </summary>
        public static readonly int Normal = 0;

        /// <summary>
        /// 精选要闻
        /// </summary>
        public static readonly int Picked = 1;

        /// <summary>
        /// 头条要闻
        /// </summary>
        public static readonly int Focus = 2;

        private static Dictionary<String, String> pickStatusList = getPickStatus();

        private static Dictionary<String, String> getPickStatus() {
            Dictionary<String, String> map = new Dictionary<String, String>();
            map.Add( "普通", "0" );
            map.Add( "要闻", "1" );
            map.Add( "焦点要闻(头条) ", "2" );
            return map;
        }

        public static Dictionary<String, String> GetPickStatus() {
            return pickStatusList;
        }

        public static String GetPickStatusStr( int p ) {
            foreach (KeyValuePair<String, String> kv in pickStatusList) {
                if (kv.Value == p.ToString()) return kv.Key;
            }
            return "";
        }
    }


}
