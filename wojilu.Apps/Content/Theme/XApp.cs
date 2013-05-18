using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;

namespace wojilu.Apps.Content.Domain {

    /// <summary>
    /// 导出的app
    /// </summary>
    public class XApp {
        public String Layout { get; set; }
        public String Style { get; set; }
        public String SkinStyle { get; set; }
        public List<XSection> SectionList { get; set; }
    }

}
