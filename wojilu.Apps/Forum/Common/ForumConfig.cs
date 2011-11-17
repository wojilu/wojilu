/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

namespace wojilu.Apps.Forum.Domain {


    [Serializable]
    public class ForumConfig {

        public static ForumConfig Instance = new ForumConfig();


        public String BanMsgTitle {
            get { return alang.get( typeof( ForumApp ), "exBanPostTitle" ); }
        }

        public String BanMsgBody {
            get { return alang.get( typeof( ForumApp ), "exBanPostBody" ) + "：" + DateTime.Now; }
        }

        public int QuestionExpiryDay {
            get { return 10; }
        }

        public int QuestionRewardCount {
            get { return 10; }
        }

        public int ShowEditInfoTime {
            get { return 60; }
        }

    }

}

