using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Apps.Content.Domain {

    public class ArticleListMode {
        public static int TitleOnly = 0;
        public static int Summary = 1;
    }

    [Serializable]
    public class ContentSetting {

        public ContentSetting() {

            AllowComment = 1;
            AllowAnonymousComment = 1;
            EnableSubmit = 0;

            IsAutoHtml = 0;

            SetDefaultValue();
        }

        public void SetDefaultValue() {

            if (this.ListPostPerPage == 0) this.ListPostPerPage = 15;
            if (this.ListPicPerPage == 0) this.ListPicPerPage = 15;
            if (this.ListVideoPerPage == 0) this.ListVideoPerPage = 15;

            if (this.RankPosts == 0) this.RankPosts = 8;
            if (this.RankPics == 0) this.RankPics = 6;
            if (this.RankVideos == 0) this.RankVideos = 6;

            if (this.SummaryLength == 0) this.SummaryLength = 150;

        }

        public int AllowComment { get; set; }
        public int AllowAnonymousComment { get; set; }

        public int EnableSubmit { get; set; } // 开放投递功能

        public int IsAutoHtml { get; set; } // 是否自动生成静态页面


        public int ListPostPerPage { get; set; }
        public int ListPicPerPage { get; set; }
        public int ListVideoPerPage { get; set; }

        public int RankPosts { get; set; }
        public int RankPics { get; set; }
        public int RankVideos { get; set; }
        public int CacheSeconds { get; set; }

        public int ArticleListMode { get; set; }
        public int SummaryLength { get; set; }

        public String MetaKeywords { get; set; }
        public String MetaDescription { get; set; }

        public String StaticPath { get; set; }


        public static int ListPageWidth = 3;
        public static int ListRecentPageSize = 50;

    }
}
