/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;

using wojilu.Common.Msg.Service;
using wojilu.Common.Msg.Enum;

using wojilu.Members.Sites.Domain;

namespace wojilu.Common.Pages.Domain {

    [Serializable]
    public class PageFeed : IShareInfo {

        private Page post;

        public PageFeed( Page p ) {
            post = p;
        }

        public IEntity GetTarget() {
            return post;
        }

        public String GetShareTitleTemplate() {
            return String.Format( lang.get( "sharePageInfo" ), "{*actor*}" );
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
            if (this.post.OwnerType == typeof( Site ).FullName) return; 
            
            String lnkCreator = "<a href=\"" + creatorLink + "\">" + creator + "</a>";
            String lnkPost = "<a href=\"" + alink.ToAppData( post ) + "\">" + post.Title + "</a>";
            String msg = String.Format( lang.get( "sharePageNotification" ), lnkCreator, lnkPost );

            new NotificationService().send( receiverId, post.OwnerType, msg, NotificationType.Share );

        }

    }

}
