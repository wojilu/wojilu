using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Apps.Shop.Enum {

    public class PickStatus {
        public static readonly int Normal = 0;
        public static readonly int Picked = 1; // 推荐
        public static readonly int Focus = 2; // 热点

        private static Dictionary<String, String> pickStatusList = getPickStatus();

        private static Dictionary<String, String> getPickStatus() {
            Dictionary<String, String> map = new Dictionary<String, String>();
            map.Add("普通", "0");
            map.Add("推荐", "1");
            map.Add("热点", "2");
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
