using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Data;
using wojilu.Web.Jobs;
using wojilu.Apps.Content.Domain;

namespace wojilu.Web.Controller.Content.Caching {

    public class HtmlJobItem : CacheObject {

        public String Method { get; set; }

        public int PostId { get; set; }
    }


}
