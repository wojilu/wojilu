/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.ORM;

using wojilu.Common.Comments;
using wojilu.Common.Feeds.Domain;
using wojilu.Common.Feeds.Service;
using wojilu.Common.Msg.Service;
using wojilu.Common.Msg.Enum;

using wojilu.Members.Users.Domain;

namespace wojilu.Apps.Poll.Domain {

    [Serializable]
    public class PollComment : ObjectBase<PollComment>, IComment {

        public int AppId { get; set; }
        public int ParentId { get; set; }
        public int RootId { get; set; }
        [Column( Length = 50 )]
        public String Author { get; set; }
        public User Member { get; set; }

        [Column( Length = 20 )]
        public String Title { get; set; }
        [LongText]
        public String Content { get; set; }
        public int Replies { get; set; }
        public DateTime Created { get; set; }

        [Column( Length = 40 )]
        public String Ip { get; set; }


        public Type GetTargetType() {
            return typeof( PollData );
        }

        [NotSave]
        public String LinkShow { get; set; }


        public void AddFeedInfo( String lnkTarget ) {

            Feed myfeed = new Feed();

            myfeed.Creator = this.Member;
            myfeed.DataType = this.GetTargetType().FullName;

            //myfeed.TitleTemplate = "{*actor*} 评论了 {*target*} 的投票 {*blog*}";

            string pollcommentFeed = lang.get( "pollcommentFeed" );
            String tt = string.Format( pollcommentFeed, "{*actor*}", "{*target*}", "{*blog*}" );
            myfeed.TitleTemplate = tt;

            myfeed.TitleData = getTitleData( lnkTarget );

            myfeed.BodyGeneral = strUtil.ParseHtml( this.Content, 50 );

            new FeedService().publishUserAction( myfeed );
        }

        private String getTitleData( String lnkPost ) {

            PollData data = db.findById<PollData>( this.RootId );

            String lnkUser = wojilu.Web.Mvc.Link.ToMember( data.Creator );
            String target = string.Format( "<a href=\"{0}\">{1}</a>", lnkUser, data.Creator.Name );
            String blog = string.Format( "<a href=\"{0}\">{1}</a>", lnkPost, data.Title );

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add( "target", target );
            dic.Add( "blog", blog );

            return Json.ToString( dic );

        }

        public void AddNotification( String lnkTarget ) {


            PollData post = db.findById<PollData>( this.RootId );
            if (post == null) return;

            int receiverId = post.OwnerId;

            // 自己的回复不用给自己发通知
            if (this.Member != null && (this.Member.Id == receiverId)) return;

            //String msg = this.Author + " 评论了投票 <a href=\"" + lnkTarget + "\">" + post.Title + "</a>";

            String notificationInfo = lang.get( "pollcommentNotification" );
            String msg = String.Format( notificationInfo, this.Author, "<a href=\"" + lnkTarget + "\">" + post.Title + "</a>" );


            NotificationService nfService = new NotificationService();
            nfService.send( receiverId, msg, post.OwnerType, NotificationType.Comment );
        }
    }

}
