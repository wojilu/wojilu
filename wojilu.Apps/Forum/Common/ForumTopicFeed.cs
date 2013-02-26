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
    public class ForumTopicFeed : IShareInfo {

        private ForumTopic topic;

        public IEntity GetTarget() {
            return topic;
        }

        public ForumTopicFeed( ForumTopic forumTopic ) {
            topic = forumTopic;
        }


        public String GetShareTitleTemplate() {
            //return "{*actor*} 分享了一个论坛主题";
            return String.Format( alang.get( typeof( ForumApp ), "shareTopic" ), "{*actor*}" );
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

            dic["title"] = topic.Title;
            dic["postLink"] = alink.ToAppData( topic );
            dic["user"] = topic.Creator.Name;
            dic["userLink"] = Link.ToMember( topic.Creator );
            dic["body"] = strUtil.ParseHtml( topic.Content, 100 );

            _shareData = Json.ToString( dic );

            return _shareData;
        }

        public void addNotification( String creator, String creatorLink ) {
            int receiverId = this.topic.Creator.Id;

            //String msg = "<a href=\"" + creatorLink + "\">" + creator + "</a> 分享了你的帖子 <a href=\"" + alink.ToAppData( topic ) + "\">" + topic.Title + "</a>";
            String shareYourInfo = alang.get( typeof( ForumApp ), "shareYourInfo" );
            String strCreator = "<a href=\"" + creatorLink + "\">" + creator + "</a>";
            String strPost = "<a href=\"" + alink.ToAppData( topic ) + "\">" + topic.Title + "</a>";
            String msg = string.Format( shareYourInfo, strCreator, strPost );


            new NotificationService().send( receiverId, msg, NotificationType.Share );
        }

    }
}
