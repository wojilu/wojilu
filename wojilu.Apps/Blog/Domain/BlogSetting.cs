using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Apps.Blog.Domain {

    [Serializable]
    public class BlogSetting {

        public void SetDefaultValue() {
            if (this.PerPageBlogs == 0) this.PerPageBlogs = 5;
            if (this.StickyCount == 0) this.StickyCount = 3;
            if (this.NewBlogCount == 0) this.NewBlogCount = 7;
            if (this.NewCommentCount == 0) this.NewCommentCount = 7;
            if (this.RssCount == 0) this.RssCount = 10;
            if (this.ListAbstractLength == 0) this.ListAbstractLength = 300; // 默认列表显示300字
        }

        public BlogSetting() {

            AllowComment = 1;
            AllowAnonymousComment = 1;

            ListMode = BlogListMode.Abstract; // 0表示摘要，1表示全文

            SetDefaultValue();

            IsShowStats = 1;

        }

        public int AllowComment { get; set; }
        public int AllowAnonymousComment { get; set; }
        public int IsShowStats { get; set; }

        public int PerPageBlogs { get; set; }

        public int ListMode { get; set; }
        public int ListAbstractLength { get; set; }

        public int StickyCount { get; set; }
        public int NewBlogCount { get; set; }
        public int NewCommentCount { get; set; }

        public int RssCount { get; set; }
    }

    public class BlogListMode {
        public static readonly int Full = 1;
        public static readonly int Abstract = 0;
    }

}
