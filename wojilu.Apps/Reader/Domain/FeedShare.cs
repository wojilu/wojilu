/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Common;

namespace wojilu.Apps.Reader.Domain {

    [Serializable]
    public class FeedShare : IShareInfo {

        private FeedEntry post;

        public FeedShare( FeedEntry entry ) {
            post = entry;
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
            return "<div><a href=\"{*postLink*}\">{*title*}</a></div><div>{*user*}</div><div class=\"note\">{*body*}</div>";
        }

        private String _shareData;
        public String GetShareBodyData( String dataLink ) {
            if (_shareData != null) return _shareData;
            Dictionary<String, object> dic = new Dictionary<String, object>();
            dic["title"] = post.Title;
            dic["postLink"] = dataLink;
            dic["user"] = post.Author;
            dic["body"] = strUtil.ParseHtml( post.Abstract, 100 );
            _shareData = Json.ToString( dic );
            return _shareData;
        }

        public void addNotification( String creator, String creatorLink ) {
        }
    }

}
