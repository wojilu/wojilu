using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;

namespace wojilu.Apps.Content.Domain {


    /// <summary>
    /// 导出的post
    /// </summary>
    public class XPost {
        public String Title { get; set; }
        public String TitleHome { get; set; } 
        public String Content { get; set; }
        public String Summary { get; set; }

        public int CategoryId { get; set; }

        public String Pic { get; set; }
        public String SourceLink { get; set; }

        public int PickStatus { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

    }

}
