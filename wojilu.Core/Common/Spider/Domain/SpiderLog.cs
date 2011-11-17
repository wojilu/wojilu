using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;

namespace wojilu.Common.Spider.Domain {

    public class SpiderLog : ObjectBase<SpiderLog> {
        
        [LongText]
        public String Msg { get; set; }


        public DateTime Created { get; set; }

    }

}
