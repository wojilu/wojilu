using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Apps.Forum.Domain {

    public class TopicTagShip : ObjectBase<TopicTagShip> {

        public UserTopicTag Tag { get; set; }
        public ForumTopic Topic { get; set; }

    }

}
