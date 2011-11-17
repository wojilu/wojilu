/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;


namespace wojilu.Common.AppBase.Interface {

    public interface IAppStats {


        DateTime TodayTime { get; set; }
        int TodayTopicCount { get; set; }
        int TodayPostCount { get; set; }
        int TodayVisitCount { get; set; }

        int PeakPostCount { get; set; }

        int VisitCount { get; set; }

    }

}

