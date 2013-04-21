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


namespace wojilu.Apps.Blog.Domain {

    [Serializable]
    [Table( "BlogComment" )]
    public class BlogPostComment : ObjectBase<BlogPostComment>, IComment {

        public int AppId { get; set; }
        public int ParentId { get; set; }
        public int RootId { get; set; }

        [Column( Length = 50 )]
        public String Author { get; set; }
        public User Member { get; set; }

        [Column( Length = 20 )]
        public String Title { get; set; }

        [LongText]
        [NotNull( Lang = "exContent" )]
        public String Content { get; set; }

        public int Replies { get; set; }
        public DateTime Created { get; set; }

        [Column( Length = 40 )]
        public String Ip { get; set; }

        public Type GetTargetType() {
            return typeof( BlogPost );
        }


        public void AddFeedInfo( String lnkTarget ) {

            Feed myfeed = new Feed();

            myfeed.Creator = this.Member;
            myfeed.DataType = this.GetTargetType().FullName;

            string feedInfo = alang.get( typeof( BlogApp ), "feedInfo" );
            String tt = string.Format( feedInfo, "{*actor*}", "{*target*}", "{*blog*}" );

            myfeed.TitleTemplate = tt;
            myfeed.TitleData = getTitleData( lnkTarget );

            myfeed.BodyGeneral = strUtil.ParseHtml( this.Content, 50 );

            myfeed.Ip = this.Ip;

            new FeedService().publishUserAction( myfeed );
        }

        private String getTitleData( String lnkPost ) {

            BlogPost data = db.findById<BlogPost>( this.RootId );

            String lnkUser = wojilu.Web.Mvc.Link.ToMember( data.Creator );
            String target = string.Format( "<a href=\"{0}\">{1}</a>", lnkUser, data.Creator.Name );
            String blog = string.Format( "<a href=\"{0}\">{1}</a>", lnkPost, data.Title );

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add( "target", target );
            dic.Add( "blog", blog );

            return Json.ToString( dic );
        }

        public void AddNotification( String lnkTarget ) {


            BlogPost post = db.findById<BlogPost>( this.RootId );
            if (post == null) return;

            int receiverId = post.OwnerId;

            // 自己的回复不用给自己发通知
            if (this.Member != null && (this.Member.Id == receiverId)) return;

            String notificationInfo = alang.get( typeof( BlogApp ), "notificationInfo" );

            String msg = String.Format( notificationInfo, this.Author, "<a href=\"" + lnkTarget + "\">" + post.Title + "</a>" );

            NotificationService nfService = new NotificationService();
            nfService.send( receiverId, post.OwnerType, msg, NotificationType.Comment );
        }

    }


}

