/*
 * Copyright 2010 www.wojilu.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;

namespace wojilu.Web.Mvc {

    /// <summary>
    /// 页面的元信息，包括标题、关键词、描述和rss链接，主要用于 seo
    /// </summary>
    public class PageMeta {

        private String _title;
        private String _keywords;
        private String _description;
        private String _rssLink;

        //public String getTitle() { return _title; }
        //public String getKeywords() { return _keywords; }
        //public String getDescription() { return _description; }
        //public String getRssLink() { return _rssLink; }

        //public void setTitle( String title ) { _title = title; }
        //public void setKeywords( String keywords ) { _keywords = keywords; }
        //public void setDescription( String description ) { _description = description; }
        //public void setRssLink( String rssLink ) { _rssLink = rssLink; }

        /// <summary>
        /// 页面的标题
        /// </summary>
        public String Title {
            get { return _title; }
            set { _title = value; }
        }

        /// <summary>
        /// 页面的关键词
        /// </summary>
        public String Keywords {
            get { return _keywords; }
            set { _keywords = value; }
        }

        /// <summary>
        /// 页面的描述
        /// </summary>
        public String Description {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// 页面对应的 rss 链接
        /// </summary>
        public String RssLink {
            get { return _rssLink; }
            set { _rssLink = value; }
        }

    }
}

