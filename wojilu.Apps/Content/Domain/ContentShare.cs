/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Common;

namespace wojilu.Apps.Content.Domain {

    [Serializable]
    public class ContentShare : IShareInfo {

        private ContentPost post;

        public ContentShare( ContentPost cpost ) {
            this.post = cpost;
        }

        public IEntity GetTarget() {
            return this.post;
        }

        public string GetShareTitleTemplate() {
            return "{*actor*} 分享了一篇文章";
        }

        public string GetShareTitleData() {
            return "";
        }

        public string GetShareBodyTemplate() {
            return "<div><a href=\"{*postLink*}\">{*title*}</a></div>" +
                "<div class=\"note\">{*body*}</div>";
        }

        private String _shareData;

        public string GetShareBodyData( string dataLink ) {
            if (_shareData != null) return _shareData;

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["title"] = post.Title;
            dic["postLink"] = dataLink;
            dic["body"] = strUtil.ParseHtml( post.Content, 100 );

            _shareData = Json.ToString( dic );
            return _shareData;
        }

        public void addNotification( string creator, string creatorLink ) {
        }

    }
}
