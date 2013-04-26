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
using wojilu.Web.Context;
using wojilu.Common;

namespace wojilu.Web.Mvc {

    /// <summary>
    /// 页面的元信息，包括标题、关键词、描述和 rss 链接，主要用于 SEO
    /// </summary>
    public class PageMeta {

        public MvcContext ctx { get; set; }

        private static String titleSeparator = "_";

        private String _title;
        private String _keywords;
        private String _description;
        private String _rssLink;

        public PageMeta( MvcContext ctx ) {
            this.ctx = ctx;
        }

        /// <summary>
        /// 页面的标题。设值的时候，默认会加上网站名称的后缀。
        /// 如果不需要后缀，请使用 SetTitleOnly() 方法
        /// </summary>
        public String Title {
            get { return _title; }
            set {
                _title = value + titleSeparator + getTitlePostfix( this.ctx );
            }
        }

        /// <summary>
        /// 页面的关键词。如果值为空，则返回 site 的默认配置
        /// </summary>
        public String Keywords {
            get {

                if (strUtil.HasText( _keywords )) {
                    return _keywords;
                }
                else {
                    return config.Instance.Site.Keywords;
                }

            }
            set { _keywords = value; }
        }

        /// <summary>
        /// 页面的描述。如果值为空，则返回 site 的默认配置
        /// </summary>
        public String Description {
            get {

                if (strUtil.HasText( _description )) {
                    return _description;
                }
                else {
                    return config.Instance.Site.Description;
                }
            }
            set { _description = value; }
        }

        /// <summary>
        /// 页面对应的 rss 链接
        /// </summary>
        public String RssLink {
            get { return _rssLink; }
            set { _rssLink = value; }
        }


        /// <summary>
        /// 给 title 赋值，不添加网站名称后缀
        /// </summary>
        /// <param name="title"></param>
        public void SetTitleOnly( String title ) {
            _title = title;
        }

        /// <summary>
        /// 将多个值放到 title 中
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        public void SetTitle( String item1, String item2 ) {
            _title = item1 + titleSeparator + item2 + titleSeparator + getTitlePostfix( this.ctx );
        }

        private static String getTitlePostfix( MvcContext ctx ) {
            if ( ctx.owner != null && ctx.owner.obj != null &&  ctx.owner.obj.GetType().FullName.Equals( ConstString.SiteTypeFullName ) == false) {
                return ctx.owner.obj.Name + titleSeparator + config.Instance.Site.SiteName;
            }
            else {
                return config.Instance.Site.SiteName;
            }
        }



    }
}

