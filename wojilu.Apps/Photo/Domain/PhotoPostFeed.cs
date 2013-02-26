/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;

using wojilu.Common;
using wojilu.Common.Msg.Service;
using wojilu.Common.Msg.Enum;


namespace wojilu.Apps.Photo.Domain {

    [Serializable]
    public class PhotoPostFeed : IShareInfo {

        private PhotoPost post;

        public IEntity GetTarget() {
            return post;
        }

        public PhotoPostFeed( PhotoPost apost ) {
            post = apost;
        }

        public String GetPublishTitleTemplate() {
            //return "{*actor*} 上传了{*photoCount*}张新照片";
            return String.Format( alang.get( typeof( PhotoApp ), "uploadInfo" ), "{*actor*}", "{*photoCount*}" );
        }

        public String GetPublishBodyTemplate() {
            return "{*photos*}";
        }

        public String GetShareTitleTemplate() {
            //return "{*actor*} 分享了一张图片";
            return String.Format( alang.get( typeof( PhotoApp ), "sharePhoto" ), "{*actor*}" );
        }

        public String GetShareTitleData() {
            return "";
        }

        public String GetShareBodyTemplate() {
            return "<div><a href=\"{*imgLink*}\"><img src=\"{*imgSrc*}\" style=\"float:left;margin-right:10px;\"/></a><div>{*imgName*}<br/><a href=\"{*userLink*}\">{*user*}</a></div><div class=\"clear\"></div></div>";
        }

        private String _shareData = null;

        public String GetShareBodyData( String dataLink ) {

            if (_shareData != null) return _shareData;

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["imgSrc"] = post.ImgThumbUrl;
            dic["imgLink"] = alink.ToAppData( post );
            dic["user"] = post.Creator.Name;
            dic["userLink"] = Link.ToMember( post.Creator );
            dic["imgName"] = post.Title;

            _shareData = Json.ToString( dic );

            return _shareData;
        }

        public void addNotification( String creator, String creatorLink ) {
            int receiverId = this.post.OwnerId;
            String msg = "<a href=\"" + creatorLink + "\">" + creator + "</a> " + alang.get( typeof(PhotoApp), "shareYourPhoto" ) + " <a href=\"" + alink.ToAppData( post ) + "\">" + post.Title + "</a>";

            new NotificationService().send( receiverId, post.OwnerType, msg, NotificationType.Share );
        }
    }

}
