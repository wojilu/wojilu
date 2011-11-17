using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;
using wojilu.ORM;

namespace wojilu.Common.Pages.Domain {

    public class PageHistory : ObjectBase<PageHistory> {

        public int PageId { get; set; }
        public DateTime Updated { get; set; }
        public String EditReason { get; set; }
        public User EditUser { get; set; } // 编辑人

        [LongText]
        public String Content { get; set; }

    }

}
