using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Data;

namespace wojilu.Web.Mvc {

    /// <summary>
    /// 特定域名到url的映射配置，Name中存储域名，Url中存储指向的网址
    /// </summary>
    public class DomainMap : CacheObject {

        public String Url { get; set; }

    }

}
