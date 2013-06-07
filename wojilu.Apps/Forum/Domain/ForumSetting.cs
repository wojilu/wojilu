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

            if (this.ReplyInterval == 0) this.ReplyInterval = 5;
        }

        /// <summary>
        /// 主题列表页：每页数量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 主题详细页：每页回帖数量
        /// </summary>
        public int ReplySize { get; set; }

        /// <summary>
        /// 主题列表页：只有符合最近的天数，才能打上new标记
        /// </summary>
        public int NewDays { get; set; }

        /// <summary>
        /// 论坛首页：图片数量
        /// </summary>
        public int HomeImgCount { get; set; }

        /// <summary>
        /// 论坛首页：最新帖子列表的数目
        /// </summary>
        public int HomeListCount { get; set; }

        /// <summary>
        /// 论坛首页：查询热帖的天数范围
        /// </summary>
        public int HomeHotDays { get; set; }

        /// <summary>
        /// 是否隐藏全站论坛统计信息
        /// </summary>
        public int IsHideStats { get; set; }

        /// <summary>
        /// 是否隐藏最新帖子和推荐
        /// </summary>
        public int IsHideTop { get; set; }

        /// <summary>
        /// 是否隐藏友情链接
        /// </summary>
        public int IsHideLink { get; set; }

        /// <summary>
        /// 是否隐藏在线统计信息
        /// </summary>
        public int IsHideOnline { get; set; }

        public String MetaKeywords { get; set; }
        public String MetaDescription { get; set; }

        /// <summary>
        /// 回复的间隔时间(秒)，少于此时间，不允许发布
        /// </summary>
        public int ReplyInterval { get; set; }


    }

}
