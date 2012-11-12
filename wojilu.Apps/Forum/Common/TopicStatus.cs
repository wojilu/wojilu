/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

namespace wojilu.Apps.Forum.Domain {

    public class TopicStatus {
        public static readonly int Normal = 0;
        public static readonly int Sticky = 1;
        public static readonly int Approve = 2;
        public static readonly int Delete = 3;

        public static String GetShowCondition() {
            return "Status<=1";
        }
    }

}

