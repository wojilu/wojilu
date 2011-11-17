/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

namespace wojilu.Apps.Blog.Domain {


    [Serializable]
    public class BlogColumn : ObjectBase<BlogColumn> {

        public String AuthorName { get; set; }
        public String AuthorUrl { get; set; }

        public String Title { get; set; }
        public String LogoUrl { get; set; }
        public String Url { get; set; }
        public String Abstract { get; set; }

        public static List<BlogColumn> GetNew( int count ) {
            return db.find<BlogColumn>( "order by Id desc" ).list( count );
        }

    }
}

