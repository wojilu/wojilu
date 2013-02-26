/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;

using wojilu.Common;
using wojilu.Common.Msg.Service;
using wojilu.Common.Msg.Enum;


namespace wojilu.Apps.Forum.Domain {

    [Serializable]
    public class ForumPostFeed : IShareInfo {

        private ForumPost post;

        public IEntity GetTarget() {
            return post;
        }

        public ForumPostFeed( ForumPost forumPost ) {
            post = forumPost;
        }

        public String GetShareTitleTemplate() {
            //return "{*actor*} 分享了一个帖子";
            return String.Format( alang.get( typeof( ForumApp ), "shareInfo" ), "{*actor*}" );
        }

        public String GetShareTitleData() {
            return "";
        }

        public String GetShareBodyTemplate() {
            return "<div><a href=\"{*postLink*}\">{*title*}</a></div><div><a href=\"{*userLink*}\">{*user*}</a></div><div class=\"note\">{*body*}</div>";
        }

        private String _shareData = null;
        public String GetShareBodyData( String dataLink ) {

            if (_shareData != null) return _shareData;

            Dictionary<string, object> dic = new Dictionary<string, object>();

            dic["title"] = post.Title;
            dic["postLink"] = alink.ToAppData( post );
            dic["user"] = post.Creator.Name;
            dic["userLink"] = Link.ToMember( post.Creator );
            dic["body"] = strUtil.ParseHtml( post.Content, 100 );

            _shareData = Json.ToString( dic );

            return _shareData;
        }

        public void addNotification( String creator, String creatorLink ) {
            int receiverId = this.post.Creator.Id;

            String shareYourInfo = alang.get( typeof( ForumApp ), "shareYourInfo" );
            String strCreator = "<a href=\"" + creatorLink + "\">" + creator + "</a>";
            String strPost = "<a href=\"" + alink.ToAppData( post ) + "\">" + post.Title + "</a>";
            String msg = string.Format( shareYourInfo, strCreator, strPost );
            //String msg = "<a href=\"" + creatorLink + "\">" + creator + "</a> 分享了你的帖子 <a href=\"" + alink.ToAppData( post ) + "\">" + post.Title + "</a>";

            new NotificationService().send( receiverId, post.Creator.GetType().FullName, msg, NotificationType.Share );
        }

    }
}
