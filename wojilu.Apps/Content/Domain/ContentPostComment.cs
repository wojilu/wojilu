/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.ORM;

using wojilu.Common.Comments;
using wojilu.Common.Feeds.Domain;
using wojilu.Common.Feeds.Service;
using wojilu.Common.Msg.Enum;
using wojilu.Common.Msg.Service;

using wojilu.Members.Users.Domain;

namespace wojilu.Apps.Content.Domain {

    [Serializable]
    public class ContentPostComment : ObjectBase<ContentPostComment>, IComment {

        public int AppId { get; set; }

        [Column( Length = 50 )]
        public String Author { get; set; }
        public User Member { get; set; }

        public int ParentId { get; set; }
        public int RootId { get; set; }

        [Column( Length = 30 )]
        public String Title { get; set; }
        [LongText]
        public String Content { get; set; }

        public int Replies { get; set; }

        public DateTime Created { get; set; }

        [Column( Length = 40 )]
        public String Ip { get; set; }

        public Type GetTargetType() {
            return typeof( ContentPost );
        }

        public void AddFeedInfo( String lnkTarget ) {
            Feed myfeed = new Feed();

            myfeed.Creator = this.Member;
            myfeed.DataType = this.GetTargetType().FullName;

            String tt = "{*actor*} 评论了文章 {*target*}";

            myfeed.TitleTemplate = tt;
            myfeed.TitleData = getTitleData( lnkTarget );

            myfeed.BodyGeneral = strUtil.ParseHtml( this.Content, 50 );

            myfeed.Ip = this.Ip;


            new FeedService().publishUserAction( myfeed );
        }

        private String getTitleData( String lnkPost ) {

            ContentPost data = ContentPost.findById( this.RootId );

            String target = string.Format( "<a href=\"{0}\">{1}</a>", lnkPost, data.Title );

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add( "target", target );
            return Json.ToString( dic );
        }


        public void AddNotification( String lnkTarget ) {
            ContentPost post = ContentPost.findById( this.RootId );
            if (post == null) return;

            int receiverId = post.OwnerId;

            // 自己的回复不用给自己发通知
            if (this.Member != null && (this.Member.Id == receiverId)) return;

            String msg = this.Author + " 评论了文章 <a href=\"" + lnkTarget + "\">" + post.Title + "</a>";

            NotificationService nfService = new NotificationService();
            nfService.send( receiverId, post.OwnerType, msg, NotificationType.Comment );
        }

    }

}

