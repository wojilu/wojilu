/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.ORM;
using wojilu.Web.Mvc;

using wojilu.Members.Users.Domain;

using wojilu.Common.Comments;
using wojilu.Common.Feeds.Domain;
using wojilu.Common.Feeds.Service;
using wojilu.Common.Msg.Enum;
using wojilu.Common.Msg.Service;

namespace wojilu.Apps.Photo.Domain {

    [Serializable]
    [Table( "PhotoComment" )]
    public class PhotoPostComment : ObjectBase<PhotoPostComment>, IComment {


        public int AppId { get; set; }

        [Column( Length = 20 )]
        public String Author { get; set; }

        [LongText]
        public String Content { get; set; }

        public DateTime Created { get; set; }

        [Column( Length = 40 )]
        public String Ip { get; set; }

        public User Member { get; set; }

        public int ParentId { get; set; }

        public int Replies { get; set; }

        public int RootId { get; set; }

        [Column( Length = 30 )]
        public String Title { get; set; }

        public Type GetTargetType() {
            return typeof( PhotoPost );
        }

        public void AddFeedInfo( String lnkTarget ) {

            PhotoPost data = PhotoPost.findById( this.RootId );

            Feed myfeed = new Feed();

            myfeed.Creator = this.Member;
            myfeed.DataType = this.GetTargetType().FullName;

            //myfeed.TitleTemplate = "{*actor*} 评论了 {*target*} 的图片";
            string feedInfo = alang.get( typeof( PhotoApp ), "feedInfo" );
            String tt = string.Format( feedInfo, "{*actor*}", "{*target*}" );
            myfeed.TitleTemplate = tt;

            myfeed.TitleData = getTitleData( data );

            myfeed.BodyTemplate = "{*photo*}";
            myfeed.BodyData = getBodyData( data );

            myfeed.BodyGeneral = strUtil.ParseHtml( this.Content, 50 );

            myfeed.Ip = this.Ip;

            new FeedService().publishUserAction( myfeed );
        }

        private String getTitleData( PhotoPost data ) {

            String lnkUser = wojilu.Web.Mvc.Link.ToMember( data.Creator );
            String target = string.Format( "<a href=\"{0}\">{1}</a>", lnkUser, data.Creator.Name );

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add( "target", target );
            return Json.ToString( dic );
        }

        private String getBodyData( PhotoPost data ) {

            String photoHtml = string.Format( "<a href=\"{0}\"><img src=\"{1}\"/></a>", alink.ToAppData( data ), data.ImgThumbUrl );

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add( "photo", photoHtml );
            return Json.ToString( dic );
        }

        public void AddNotification( String lnkTarget ) {

            PhotoPost post = db.findById<PhotoPost>( this.RootId );
            if (post == null) return;

            int receiverId = post.OwnerId;
            if (this.Member != null && (this.Member.Id == receiverId)) return;

            String msg = this.Author + " " + alang.get( typeof( PhotoApp ), "commentPhoto" ) + "<br/> <a href=\"" + lnkTarget + "\"><img src='" + post.ImgThumbUrl + "' /></a>";

            NotificationService nfService = new NotificationService();
            nfService.send( receiverId, post.OwnerType, msg, NotificationType.Comment );
        }

    }
}

