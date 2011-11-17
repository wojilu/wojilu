using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;

namespace wojilu.Common.Spider.Domain {

    public class SpiderArticle : ObjectBase<SpiderArticle> {

        public SpiderTemplate SpiderTemplate { get; set; }

        public String Title { get; set; }

        [LongText]
        public String Abstract { get; set; }

        [HtmlText]
        [LongText]
        public String Body { get; set; }

        public String Url { get; set; }
        public String PicUrl { get; set; }

        public DateTime Created { get; set; }

        public int IsDelete { get; set; }

        public int IsPic { get; set; }

    }

}
