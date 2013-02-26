using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Comments;
using wojilu.Members.Users.Domain;
using wojilu.ORM;
using wojilu.Common.Msg.Service;
using wojilu.Common.Msg.Enum;
using wojilu.Common.Feeds.Domain;
using wojilu.Common.Feeds.Service;
using wojilu.Serialization;

namespace wojilu.Apps.Download.Domain {

    [Table( "DownloadComment" )]
    public class FileComment : ObjectBase<FileComment>, IComment {

        public int AppId { get; set; }
        public int RootId { get; set; }
        public int ParentId { get; set; }

        public User Member { get; set; }

        [Column( Length = 50 )]
        public string Author { get; set; }
        public string Title { get; set; }

        [LongText]
        [NotNull( Lang = "exContent" )]
        public string Content { get; set; }

        [Column( Length = 40 )]
        public string Ip { get; set; }
        public int Replies { get; set; }
        public DateTime Created { get; set; }

        public Type GetTargetType() {
            return typeof( FileItem );
        }

        public void AddFeedInfo( string lnkTarget ) {
            Feed myfeed = new Feed();

            myfeed.Creator = this.Member;
            myfeed.DataType = this.GetTargetType().FullName;

            String tt = "{*actor*} 评论了文件 {*target*}";

            myfeed.TitleTemplate = tt;
            myfeed.TitleData = getTitleData( lnkTarget );

            myfeed.BodyGeneral = strUtil.ParseHtml( this.Content, 50 );

            new FeedService().publishUserAction( myfeed );
        }

        private String getTitleData( String lnkPost ) {

            FileItem data = FileItem.findById( this.RootId );

            String target = string.Format( "<a href=\"{0}\">{1}</a>", lnkPost, data.Title );

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add( "target", target );
            return Json.ToString( dic );
        }

        public void AddNotification( string lnkTarget ) {
            FileItem post = FileItem.findById( this.RootId );
            if (post == null) return;

            int receiverId = post.OwnerId;

            // 自己的回复不用给自己发通知
            if (this.Member != null && (this.Member.Id == receiverId)) return;

            String msg = this.Author + " 评论了文件 <a href=\"" + lnkTarget + "\">" + post.Title + "</a>";

            NotificationService nfService = new NotificationService();
            nfService.send( receiverId, post.OwnerType, msg, NotificationType.Comment );
        }

    }

}
