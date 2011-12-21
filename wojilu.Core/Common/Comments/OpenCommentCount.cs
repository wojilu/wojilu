using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Common.Comments {

    public class OpenCommentCount : ObjectBase<OpenCommentCount> {

        public String TargetUrl { get; set; }
        public int Replies { get; set; }

        public String DataType { get; set; }
        public int DataId { get; set; }

    }

}
