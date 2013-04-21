/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.ORM;
using wojilu.Common;
using wojilu.Common.AppBase;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.Comments;


namespace wojilu.Apps.Blog.Domain {


    [Serializable]
    public class BlogApp : ObjectBase<BlogApp>, IApp, IAccessStatus, IHits, ICommentApp {

        public int OwnerId { get; set; }
        public String OwnerUrl { get; set; }
        public String OwnerType { get; set; }

        public int BlogCount { get; set; }
        public int CommentCount { get; set; }
        public int Hits { get; set; }

        [TinyInt]
        public int AccessStatus { get; set; }
        public DateTime Created { get; set; }

        public String Settings { get; set; }

        public BlogSetting GetSettingsObj() {
            if (strUtil.IsNullOrEmpty( this.Settings )) return new BlogSetting();
            BlogSetting s = Json.Deserialize<BlogSetting>( this.Settings );
            s.SetDefaultValue();
            return s;
        }

    }



}

