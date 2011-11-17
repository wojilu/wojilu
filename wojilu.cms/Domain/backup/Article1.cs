using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;

namespace wojilu.cms.Domain {

    public class Article : ObjectBase<Article> {

        public Category Category { get; set; }

        [NotNull( "请输入标题" ), Column( Length = 10 )]
        public string Title { get; set; }

        [LongText, NotNull( "请输入内容" )]
        public string Content { get; set; }

        public DateTime Created { get; set; }

    }

}
