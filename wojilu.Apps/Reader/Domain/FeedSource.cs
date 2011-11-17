/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;
using wojilu.Web;
using wojilu.ORM;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;

namespace wojilu.Apps.Reader.Domain {

    [Serializable]
    public class FeedSource : ObjectBase<FeedSource>, ISort {


        public int OrderId { get; set; }
        public String Title { get; set; }
        public String FeedLink { get; set; }
        public String Link { get; set; }

        // 自定义名称
        public String Name { get; set; } 
        public FeedSysCategory Category { get; set; }

        [LongText]
        public String Description { get; set; }

        public String BlogLanguage { get; set; }
        public DateTime PubDate { get; set; }
        public DateTime LastBuildDate { get; set; }
        public String Generator { get; set; }
        public int Ttl { get; set; }

        // 收藏时间
        public DateTime Created { get; set; }

        // 最后更新时间
        public DateTime LastRefreshTime { get; set; }

        // 博客文章数目
        public int EntryCount { get; set; } 
        public int UserCount { get; set; }

        [NotSave]
        public RssChannel RssChannel { get; set; }


        public void updateOrderId() {
            base.update( "OrderId" );
        }

    }
}
