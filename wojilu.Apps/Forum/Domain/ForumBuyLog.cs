using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Apps.Forum.Domain {

    public class ForumBuyLog : ObjectBase<ForumBuyLog> {

        public long UserId { get; set; }

        public long TopicId { get; set; }

        public DateTime Created { get; set; }


    }
}
