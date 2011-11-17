using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Apps.Forum.Domain {

    [Serializable]
    public class ForumSetting {

        public ForumSetting() {

            SetDefaultValue();
        }

        public void SetDefaultValue() {
            if (this.PageSize == 0) this.PageSize = 25;
            if (this.ReplySize == 0) this.ReplySize = 10;
            if (this.NewDays == 0) this.NewDays = 1;
            if (this.HomeImgCount == 0) this.HomeImgCount = 5;
            if (this.HomeListCount == 0) this.HomeListCount = 10;
            if (this.HomeHotDays == 0) this.HomeHotDays = 30;
        }

        public int PageSize { get; set; } // 主题列表页：每页数量
        public int ReplySize { get; set; } // 主题详细页：每页回帖数量
        public int NewDays { get; set; } // 主题列表页：只有符合最近的天数，才能打上new标记

        public int HomeImgCount { get; set; } // 论坛首页：图片数量
        public int HomeListCount { get; set; } // 论坛首页：最新帖子列表的数目
        public int HomeHotDays { get; set; } // 论坛首页：查询热帖的天数范围

        public int IsHideStats { get; set; } // 是否隐藏全站论坛统计信息
        public int IsHideTop { get; set; } // 是否隐藏最新帖子和推荐
        public int IsHideLink { get; set; } // 是否隐藏友情链接
        public int IsHideOnline { get; set; } // 是否隐藏在线统计信息

    }

}
