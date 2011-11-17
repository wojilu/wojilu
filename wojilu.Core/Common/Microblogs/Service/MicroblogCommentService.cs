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

        //        void InsertComment( MicroblogComment c, String miniblogLink );

        //------------------------------------------------------------------------------------------------------------------

        public virtual IFeedService feedService { get; set; }
        public virtual INotificationService nfService { get; set; }

        public MicroblogCommentService() {
            feedService = new FeedService();
            nfService = new NotificationService();
        }

        public List<MicroblogComment> GetTop( int id, int count ) {
            return MicroblogComment.find( "RootId=" + id ).list( count );
        }

        public virtual DataPage<MicroblogComment> GetComments( int id, int pageSize ) {
            return MicroblogComment.findPage( "RootId=" + id, pageSize );
        }

        public virtual void InsertComment( MicroblogComment c, String microblogLink ) {

            saveComment( c );
            copyCommentCountToFeed( c );

            int receiverId = addNotificationToRoot( c, microblogLink );
            addNotificationToParent( c, microblogLink, receiverId );
        }


        private static void saveComment( MicroblogComment c ) {
            db.insert( c );
        }

        private void copyCommentCountToFeed( MicroblogComment c ) {
            feedService.SetCommentCount( c.Root );
        }

        private int addNotificationToRoot( MicroblogComment c, String microblogLink ) {

            Microblog root = c.Root;

            int receiverId = root.User.Id;
            if (c.User.Id == receiverId) return 0;

            String blogTitle = strUtil.ParseHtml( root.Content, 30 );

            String msg = c.User.Name + " " + lang.get( "commentYour" ) + lang.get( "microblog" ) + " <a href=\"" + microblogLink + "\">" + blogTitle + "</a>";
            nfService.send( receiverId, typeof( User ).FullName, msg, NotificationType.Comment );
            return receiverId;
        }

        private void addNotificationToParent( MicroblogComment c, String microblogLink, int sentId ) {

            if (c.ParentId == 0) return;

            MicroblogComment parent = MicroblogComment.findById( c.ParentId );

            int receiverId = parent.User.Id;
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



        public DataPage<MicroblogComment> GetPageByUser( int ownerId, int pageSize ) {

            return MicroblogComment.findPage( "UserId=" + ownerId );

        }


    }

}
