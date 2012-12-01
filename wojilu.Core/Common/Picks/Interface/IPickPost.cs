/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

namespace wojilu.Common.Picks {

    public interface IPickPost {

        int AppId { get; set; }

        string Title { get; set; }// 支持 html
        string Summary { get; set; } // 支持 html
        string Link { get; set; }// 自定义 link

        int IsDelete { get; set; }  // 是否被删除
        int DeleteId { get; set; }  // 被删除的Id

        int IsEdit { get; set; } // 是否编辑过
        int EditId { get; set; } // 被编辑的帖子，就不再重复显示了

        int IsPin { get; set; } // 是否固定
        int PinIndex { get; set; } // 固定的位置
        int PinTopicId { get; set; } // 被固定的topic

    }

}
