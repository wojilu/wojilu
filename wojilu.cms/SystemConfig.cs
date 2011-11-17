using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.cms {

    public class SystemConfig {
                
        public int PageSize { get; set; } // 文章列表中，每页文章数

        public string Contact { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public static SystemConfig Instance = getConfig();

        private static SystemConfig getConfig() {
            return cfgHelper.Read<SystemConfig>();
        }

        public static void Save() {
            cfgHelper.Write( Instance );
        }
    }

}
