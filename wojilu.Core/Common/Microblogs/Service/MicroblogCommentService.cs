using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Microblogs.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Common.Msg.Enum;
using wojilu.Common.Feeds.Interface;
using wojilu.Common.Msg.Interface;
using wojilu.Common.Feeds.Service;
using wojilu.Common.Msg.Service;

namespace wojilu.Common.Microblogs.Service {

    public class MicroblogCommentService {

        public virtual INotificationService nfService { get; set; }

        public MicroblogCommentService() {
            nfService = new NotificationService();
        }

        public virtual List<MicroblogComment> GetTop(long id, int count) {
            return MicroblogComment.find( "RootId=" + id ).list( count );
        }

        public virtual DataPage<MicroblogComment> GetComments(long id, int pageSize) {
            return MicroblogComment.findPage( "RootId=" + id, pageSize );
        }

        public virtual void InsertComment( MicroblogComment c, String microblogLink ) {

            saveComment( c );

            long receiverId = addNotificationToRoot( c, microblogLink );
            addNotificationToParent( c, microblogLink, receiverId );
        }


        private static void saveComment( MicroblogComment c ) {
            db.insert( c );
        }

        private long addNotificationToRoot( MicroblogComment c, String microblogLink ) {

            Microblog root = c.Root;

            long receiverId = root.User.Id;
            if (c.User.Id == receiverId) return 0;

            String blogTitle = strUtil.ParseHtml( root.Content, 30 );

            String msg = c.User.Name + " " + lang.get( "commentYour" ) + lang.get( "microblog" ) + " <a href=\"" + microblogLink + "\">" + blogTitle + "</a>";
            nfService.send( receiverId, typeof( User ).FullName, msg, NotificationType.Comment );
            return receiverId;
        }

        private void addNotificationToParent(MicroblogComment c, string microblogLink, long sentId) {

            if (c.ParentId == 0) return;

            MicroblogComment parent = MicroblogComment.findById( c.ParentId );

            long receiverId = parent.User.Id;
            if (c.User.Id == receiverId) return;

            if (receiverId == sentId) return;


            String commentTitle = strUtil.ParseHtml( parent.Content, 30 );

            //String msg = c.User.Name + " 回复了你的微博评论 <a href=\"" + microblogLink + "\">" + commentTitle + "</a>";
            String clink = "<a href=\"" + microblogLink + "\">" + commentTitle + "</a>";
            String msg = string.Format( lang.get( "replyMicroblog" ), c.User.Name, clink );

            nfService.send( receiverId, typeof( User ).FullName, msg, NotificationType.Comment );

        }




        public virtual MicroblogComment GetById( int id ) {
            return MicroblogComment.findById( id );
        }



        public virtual DataPage<MicroblogComment> GetPageByUser( int ownerId, int pageSize ) {

            return MicroblogComment.findPage( "UserId=" + ownerId );

        }


    }

}
