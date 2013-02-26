/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;

using wojilu.Common;
using wojilu.Common.Msg.Service;
using wojilu.Common.Msg.Enum;


namespace wojilu.Apps.Blog.Domain {

    [Serializable]
    public class BlogPostFeed : IShareInfo {

        private BlogPost post;

        public IEntity GetTarget() {
            return post;
        }

        public BlogPostFeed( BlogPost apost ) {
            post = apost;
        }

        public String GetShareTitleTemplate() {
            //return "{*actor*} 分享了一篇日志";
            return string.Format( alang.get( typeof( BlogApp ), "shareInfo" ), "{*actor*}" );
                
        }

        public String GetShareTitleData() {
            return "";
        }

        public String GetShareBodyTemplate() {
            return "<div><a href=\"{*postLink*}\">{*title*}</a></div><div><a href=\"{*userLink*}\">{*user*}</a></div><div class=\"note\">{*body*}</div>";
        }

        private String _shareData;
        public String GetShareBodyData( String dataLink ) {

            if (_shareData != null) return _shareData;

            Dictionary<String, object> dic = new Dictionary<String, object>();
            dic["title"] = post.Title;
            dic["postLink"] = alink.ToAppData( this.post );
            dic["user"] = post.Creator.Name;
            dic["userLink"] = Link.ToMember( post.Creator );
            dic["body"] = strUtil.ParseHtml( post.Content, 100 );

            _shareData = Json.ToString( dic );

            return _shareData;
        }

        public void addNotification( String creator, String creatorLink ) {
            int receiverId = this.post.OwnerId;

            //String msg = "<a href=\"" + creatorLink + "\">" + creator + "</a> 分享了你的日志 <a href=\"" + alink.ToAppData( post ) + "\">" + post.Title + "</a>";

            String strcreator = "<a href=\"" + creatorLink + "\">" + creator + "</a>";
            String strpost = "<a href=\"" + alink.ToAppData( post ) + "\">" + post.Title + "</a>";
            String msg = String.Format( alang.get( typeof( BlogApp ), "notiShare" ), strcreator, strpost );

            new NotificationService().send( receiverId, post.OwnerType, msg, NotificationType.Share );
        }


    }

}
