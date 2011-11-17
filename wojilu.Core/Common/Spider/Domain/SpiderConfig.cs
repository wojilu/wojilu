using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Common.Spider.Domain {

    public class SpiderConfig {

        public static readonly String UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";

        public static readonly String ListLinkPattern = @"href=[""']([^""]+?)[""'][^\>]*?\>([^\<]+?)\</a\>";

        public static readonly int SuspendFrom = 2000;
        public static readonly int SuspendTo = 6000;


    }

}
