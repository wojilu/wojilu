using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Common.Comments;
using wojilu.Common.Microblogs.Domain;
using wojilu.Common.Microblogs.Interface;
using wojilu.Common.Microblogs.Service;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;
using wojilu.Common.Microblogs;
using wojilu.Apps.Photo.Interface;
using wojilu.Apps.Photo.Service;
using wojilu.Apps.Photo.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Common.Msg.Domain;
using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Interface;
using wojilu.Apps.Blog.Service;

namespace wojilu.Web.Controller.Users {

    public class HomeController : ControllerBase {

        public virtual IMicroblogService microblogService { get; set; }
        public virtual IFollowerService followService { get; set; }
        public virtual IVisitorService visitorService { get; set; }
        public virtual MicroblogFavoriteService mfService { get; set; }
        public virtual IOpenCommentService commentService { get; set; }
        public virtual IFriendService friendService { get; set; }

        public virtual IPhotoPostService photoPostService { get; set; }

        public virtual IBlogService blogAppService { get; set; }
        public virtual IPhotoService photoAppService { get; set; }

        public HomeController() {
            microblogService = new MicroblogService();
            followService = new FollowerService();
            visitorService = new VisitorService();
            mfService = new MicroblogFavoriteService();
            commentService = new OpenCommentService();
            photoPostService = new PhotoPostService();
            friendService = new FriendService();
            blogAppService = new BlogService();
            photoAppService = new PhotoService();
        }

        public override void Layout() {
            load( "userMenu", new ProfileController().UserMenu );
            bindProfile();
            bindUserLinkList();
            bindVisitorList();
            bindFriendList();
            bindFollowingList();
            set( "lnkProfile", to( new ProfileController().Main ) );
        }

        public virtual void Index() {

            set( "user.Name", ctx.owner.obj.Name );
            bindUserLinkList();

            bindFeedList();

            bindFeedback();
            bindProfile();
            bindPicList();
        }

        private void bindFeedList() {

            DataPage<Microblog> list = microblogService.GetPageList( ctx.owner.obj.Id, MicroblogAppSetting.Instance.MicroblogPageSize );
            List<MicroblogVo> volist = mfService.CheckFavorite( list.Results, ctx.viewer.Id );

            if (list.PageCount <= 1) {
                set( "lnkFeed", to( Feed ) );
                set( "feedMoreStyle", "display:none;" );
            }
            else {
                set( "lnkFeed", PageHelper.AppendNo( to( Feed ), 2 ) );
                set( "feedMoreStyle", "" );
            }

            ctx.SetItem( "_microblogVoList", volist );
            ctx.SetItem( "_showUserFace", false );
            load( "blogList", new Microblogs.MicroblogController().bindBlogs );

            if (list.RecordCount == 0) {
                set( "page1", list.PageBar );
                set( "page2", "对不起，还动态消息" );
            }
            else if (list.PageCount > 1) {
                set( "page1", list.PageBar );
                set( "page2", list.PageBar );
            }
            else {
                set( "page1", list.PageBar );
                set( "page2", "" );
            }
        }

        public virtual void Feed() {
            bindFeedList();
        }

        public virtual void Info( long id ) {
            set( "lnkList", to( Feed ) );
            load( "blogBody", new Microblogs.MicroblogController().Show, id );
        }

        public virtual void SendMsg() {

            HideLayout( typeof( HomeController ) );

            String lnk = Link.To( ctx.viewer.obj, new Users.Admin.MsgController().CreateOk );
            set( "ActionLink", lnk );
            set( "receiver", ctx.owner.obj.Name );
            set( "returnUrl", Link.ToMember( ctx.owner.obj ) );
        }

        private void bindProfile() {

            User user = ctx.owner.obj as User;
            UserVo uservo = new UserVo( user );
            bind( "user", uservo );

            int microblogCount = microblogService.CountByUser( user.Id );
            set( "user.MicroblogCount", microblogCount );

        }

        public virtual void Blog() {
            BlogApp app = blogAppService.GetFirstByUser( ctx.owner.Id );
            if (app == null) {
                echoError( "app不存在" );
                return;
            }
            String lnkApp = Link.To( ctx.owner.obj, new Blog.BlogController().Index, app.Id );
            redirectDirect( lnkApp );
        }

        public virtual void Photo() {
            PhotoApp app = photoAppService.GetByUser( ctx.owner.Id );
            if (app == null) {
                echoError( "app不存在" );
                return;
            } String lnkApp = Link.To( ctx.owner.obj, new Photo.PhotoController().Index, app.Id );
            redirectDirect( lnkApp );
        }

        private void bindUserLinkList() {

            set( "lnkHomepage", Link.ToMember( ctx.owner.obj ) );
            set( "lnkMicroblog", alink.ToUserMicroblog( ctx.owner.obj ) );
            set( "lnkBlog", to( Blog ) );
            set( "lnkPhoto", to( Photo ) );

            set( "lnkForumPost", to( new ForumController().Topic ) );

            set( "lnkFeedback", to( new FeedbackController().List ) );
            set( "lnkFriend", to( new FriendController().FriendList ) );
            set( "lnkVisitor", to( new VisitorController().Index ) );
            set( "lnkProfile", to( new ProfileController().Main ) );

            set( "lnkMsg", to( SendMsg ) );

            set( "lnkFollower", to( new FriendController().FollowerList ) );
            set( "lnkFollowing", to( new FriendController().FollowingList ) );

            set( "lnkFollow", to( new FriendController().Follow, ctx.owner.Id ) );
            set( "lnkUnFollow", to( new FriendController().DeleteFollow, ctx.owner.Id ) );


            if (ctx.viewer.IsFollowing( ctx.owner.Id )) {
                set( "followDisplay", "display:none" );
                set( "followedDisplay", "" );
            }
            else {
                set( "followDisplay", "" );
                set( "followedDisplay", "display:none" );
            }

        }

        private void bindFeedback() {

            set( "ActionLink", t2( new FeedbackController().Create ) );
            set( "f.ListLink", t2( new FeedbackController().List ) );

            String pwTip = string.Format( lang( "pwTip" ), Feedback.ContentLength );
            set( "pwTip", pwTip );
        }

        private void bindPicList() {

            List<PhotoPost> list = photoPostService.GetNew( ctx.owner.Id, 3 );
            IBlock block = getBlock( "picList" );
            foreach (PhotoPost x in list) {
                block.Set( "x.Link", alink.ToAppData( x ) );
                block.Set( "x.Pic", x.ImgThumbUrl );
                block.Next();
            }
        }

        private void bindVisitorList() {

            IBlock block = getBlock( "visitors" );
            List<User> visitorList = visitorService.GetRecent( 9, ctx.owner.obj.Id );
            foreach (User user in visitorList) {
                block.Set( "user.Name", user.Name );
                block.Set( "user.Face", user.PicSmall );
                block.Set( "user.Link", toUser( user ) );
                block.Next();
            }
        }

        private void bindFriendList() {
            List<User> friends = friendService.GetRecentActive( 9, ctx.owner.obj.Id );
            IBlock block = getBlock( "myfriendList" );
            foreach (User user in friends) {
                block.Set( "user.Name", user.Name );
                block.Set( "user.Face", user.PicSmall );
                block.Set( "user.Link", toUser( user ) );
                block.Set( "user.LastActiveTime", getLastActiveTime( user ) );
                block.Next();
            }
        }

        private void bindFollowingList() {
            List<User> friends = followService.GetRecentFollowing( ctx.owner.obj.Id, 9 );
            IBlock block = getBlock( "following" );
            foreach (User user in friends) {
                block.Set( "user.Name", user.Name );
                block.Set( "user.Face", user.PicSmall );
                block.Set( "user.Link", toUser( user ) );
                block.Set( "user.LastActiveTime", getLastActiveTime( user ) );
                block.Next();
            }
        }

        // TODO 得到好友最近活动时间
        private String getLastActiveTime( User user ) {
            if (cvt.IsDayEqual( user.LastUpdateTime, DateTime.Now )) {
                return user.LastUpdateTime.ToShortTimeString();
            }
            return user.LastUpdateTime.ToShortDateString();
        }

    }

}
