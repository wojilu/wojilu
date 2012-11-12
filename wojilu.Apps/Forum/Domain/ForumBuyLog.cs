using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Apps.Forum.Domain {

    public class ForumBuyLog : ObjectBase<ForumBuyLog> {

        public int UserId { get; set; }

        public int TopicId { get; set; }

        public DateTime Created { get; set; }


    }
}
