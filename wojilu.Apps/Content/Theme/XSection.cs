using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;

namespace wojilu.Apps.Content.Domain {


    /// <summary>
    /// 导出的section
    /// </summary>
    public class XSection {

        [NotSave]
        public int Id { get; set; }

        //public int OrderId { get; set; }
        //public int RowId { get; set; }
        //public int ColumnId { get; set; }


        public String Name { get; set; }
        public String LayoutStr { get; set; }
        public String CssClass { get; set; }

        public int ListCount { get; set; }

        public String TypeFullName { get; set; }
        public int TemplateId { get; set; }
        public String TemplateCustom { get; set; } // 自定义模板内容

        public int ServiceId { get; set; }
        public String ServiceParams { get; set; }

        public String CssPath { get; set; }

        public List<XPost> PostList { get; set; }
    }


}
