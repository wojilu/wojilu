/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;

using wojilu.Common;
using wojilu.Common.Msg.Service;
using wojilu.Common.Msg.Enum;


namespace wojilu.Members.Users.Domain {

    public class MemberFeed : IShareInfo {


        private User user;

        public IEntity GetTarget() {
            return user;
        }

        public MemberFeed( User member ) {
            user = member;
        }

        public String GetShareTitleTemplate() {
            String shareUserFeed = lang.get( "shareUserFeed" );
            return string.Format( shareUserFeed, "{*actor*}", "<a href=\"{*titlelink*}\">{*title*}</a>" );
            //return "{*actor*} 分享了用户 <a href=\"{*titlelink*}\">{*title*}</a>";
        }

        public String GetShareTitleData() {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["title"] = user.Name;
            dic["titlelink"] = Link.ToMember( user );
            return Json.ToString( dic );
        }

        public String GetShareBodyTemplate() {
            return "<div><a href=\"{*imgLink*}\"><img src=\"{*imgSrc*}\" style='float:left;margin-right:10px;'/></a><div><a href='{*userLink*}'>{*user*}</a></div><div class=\"clear\"></div></div>";
        }

        private String _shareData = null;

        public String GetShareBodyData( String dataLink ) {

            if (_shareData != null) return _shareData;

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["imgSrc"] = user.PicSmall;
            dic["imgLink"] = Link.ToMember( user );
            dic["user"] = user.Name;
            dic["userLink"] = Link.ToMember( user );

            _shareData = Json.ToString( dic );

            return _shareData;
        }

        public void addNotification( String creator, String creatorLink ) {
            int receiverId = this.user.Id;

            String msg = string.Format( lang.get( "shareNotification" ), "<a href='" + creatorLink + "'>" + creator + "</a>" );
            //String msg = "<a href='" + creatorLink + "'>" + creator + "</a> 将你分享给了好友";

            new NotificationService().send( receiverId, typeof( User ).FullName, msg, NotificationType.Share );
        }

    }

}
