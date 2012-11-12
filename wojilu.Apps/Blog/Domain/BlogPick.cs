using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;
using wojilu.Web.Mvc;
using wojilu.Apps.Blog.Interface;

namespace wojilu.Apps.Blog.Domain {

    public class MergedPost {

        public BlogPost Topic { get; set; }

        public String Title { get; set; } // 支持 html

        public String Link { get; set; } // 自定义 link

        public String Summary { get; set; } // 支持 html

        // 是否固定
        public Boolean IsPin {
            get { return this.Topic == null; }
        }

        public int IsEdit { get; set; } // 是否编辑过

    }

    public class BlogPick : ObjectBase<BlogPick> {

        public int AppId { get; set; }

        [LongText, NotNull]
        public String Title { get; set; } // 支持 html

        public String Link { get; set; } // 自定义 link

        [LongText]
        public String Summary { get; set; } // 支持 html

        //-------------------------------------------------------------------

        public int IsEdit { get; set; } // 是否编辑过
        public int EditId { get; set; } // 被编辑的帖子，就不再重复显示了

        public int IsDelete { get; set; } // 是否被删除
        public int DeleteId { get; set; } // 被删除的Id

        public int IsPin { get; set; } // 是否固定
        public int PinIndex { get; set; } // 固定的位置
        public int PinTopicId { get; set; } // 被固定的topic

    }

}
