using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Comments;

namespace wojilu.Apps.Forum.Domain {

    public class CommentSync : ObjectBase<CommentSync> {

        public ForumPost Post { get; set; }
        public OpenComment Comment { get; set; }
        public DateTime Created { get; set; }

    }

}
