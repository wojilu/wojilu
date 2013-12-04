using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Net.Video;
using wojilu.Web.Controller.Common;
using wojilu.Web.Controller.Users;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;
using wojilu.Members.Users.Domain;
using wojilu.Common.Microblogs.Service;
using wojilu.Common.Microblogs.Domain;
using wojilu.Common.Microblogs.Interface;
using wojilu.Web.Controller.Admin;
using wojilu.Common;
using wojilu.Common.Microblogs;
using wojilu.Common.Comments;

namespace wojilu.Web.Controller.Users.Admin {

    public class HomeController : ControllerBase {


        public virtual IMicroblogService microblogService { get; set; }
        public virtual IFollowerService followService { get; set; }
        public virtual IVisitorService visitorService { get; set; }
        public virtual MicroblogFavoriteService mfService { get; set; }
        public virtual MicroblogAtService matService { get; set; }
        public virtual MicroblogFavoriteService favoriteService { get; set; }
        public virtual IOpenCommentService commentService { get; set; }

        public IVideoSpider videoSpider { get; set; }


        public HomeController() {
            favoriteService = new MicroblogFavoriteService();
            microblogService = new MicroblogService();
            followService = new FollowerService();
            visitorService = new VisitorService();
            mfService = new MicroblogFavoriteService();
            matService = new MicroblogAtService();
            videoSpider = new WojiluVideoSpider();
            commentService = new OpenCommentService();
        }

        public override void Layout() {

            User user = ctx.owner.obj as User;
            ctx.SetItem( "loadHeader", false );

            bindUserInfo( user );

            set( "cmdFollow", bindCmd( user ) );

            List<User> followers = followService.GetRecentFriendsAndFollowers( ctx.owner.Id, 18 );
            bindUsers( followers, "users" );

            List<User> visitors = visitorService.GetRecent( 9, ctx.owner.Id );
            bindUsers( visitors, "visitor" );

            set( "moreFollowers", to( new wojilu.Web.Controller.Users.FriendController().FollowerList ) );
            set( "moreVisitors", to( new wojilu.Web.Controller.Users.VisitorController().Index ) );
        }

        public virtual void Index( long userId ) {

            load( "publisher", Publisher );

            User user = (User)ctx.owner.obj;

            set( "user.Link", toUser( user ) );
            set( "user.Face", user.PicSmall );
            set( "user.Name", user.Name );

            DataPage<Microblog> list = microblogService.GetFollowingPage( ctx.owner.obj.Id, MicroblogAppSetting.Instance.MicroblogPageSize );
            List<MicroblogVo> volist = mfService.CheckFavorite( list.Results, ctx.viewer.Id );

            ctx.SetItem( "_microblogVoList", volist );
            ctx.SetItem( "_showUserFace", true );
            load( "blogList", new wojilu.Web.Controller.Microblogs.MicroblogController().bindBlogs );

            set( "page", list.PageBar );
        }


        public virtual void Publisher() {

            target( new Microblogs.MicroblogSaveController().Create );

            set( "mbTotalCount", MicroblogAppSetting.Instance.MicroblogContentMax );

            set( "uploadLink", to( new Microblogs.My.MbSaveController().UploadForm ) );
            set( "getVideoUrl", to( GetVideoInfo ) );

            set( "authJson", AdminSecurityUtils.GetAuthCookieJson( ctx ) );

            set( "savPicLink", to( new wojilu.Web.Controller.Microblogs.My.MbSaveController().SavePic ) );

            // swf上传跨域问题
            set( "jsPath", sys.Path.DiskJs );
        }

        public virtual void GetVideoInfo() {

            String videoUrl = ctx.Post( "videoUrl" );
            if (strUtil.IsNullOrEmpty( videoUrl )) {
                echoError( "请填写网址" );
                return;
            }

            if (videoUrl.StartsWith( "http://" ) == false) videoUrl = "http://" + videoUrl;
            VideoInfo vi = videoSpider.GetInfo( videoUrl );

            MicroblogVideoTemp mvt = new MicroblogVideoTemp( vi );
            mvt.insert();

            echoJsonMsg( "", true, mvt.Id.ToString() );
        }


        public virtual void Atme() {

            Page.Title = "提到我的微博";


            load( "publisher", Publisher );
            set( "homeLink", to( Index, ctx.owner.Id ) );

            set( "user.Name", ctx.owner.obj.Name );

            // 使用通用视图文件
            DataPage<Microblog> list = matService.GetByUser( ctx.owner.Id, 20 );
            List<MicroblogVo> volist = mfService.CheckFavorite( list.Results, ctx.viewer.Id );
            ctx.SetItem( "_microblogVoList", volist );
            ctx.SetItem( "_showUserFace", true );
            load( "blogList", new wojilu.Web.Controller.Microblogs.MicroblogController().bindBlogs );

            // 标记为已读
            User owner = ctx.owner.obj as User;
            if (owner.MicroblogAtUnread > 0 && ctx.viewer.Id == owner.Id) {
                owner.MicroblogAtUnread = 0;
                owner.update( "MicroblogAtUnread" );
            }

            set( "page", list.PageBar );
        }

        public virtual void Favorite() {

            load( "publisher", Publisher );
            set( "homeLink", to( Index, ctx.owner.Id ) );

            DataPage<Microblog> list = favoriteService.GetBlogPage( ctx.owner.Id, 25 );


            List<MicroblogVo> volist = favoriteService.CheckFavorite( list.Results, ctx.viewer.Id );
            ctx.SetItem( "_microblogVoList", volist );
            ctx.SetItem( "_showUserFace", true );
            load( "flist", new wojilu.Web.Controller.Microblogs.MicroblogController().bindBlogs );

            set( "page", list.PageBar );
        }

        public virtual void Comment() {

            load( "publisher", Publisher );
            set( "homeLink", to( Index, ctx.owner.Id ) );

            DataPage<OpenComment> list = commentService.GetByMicroblogOwnerId( ctx.owner.Id );

            IBlock block = getBlock( "list" );

            foreach (OpenComment c in list.Results) {

                block.Set( "c.UserName", c.Member.Name );
                block.Set( "c.UserFace", c.Member.PicSmall );
                block.Set( "c.UserLink", toUser( c.Member ) );
                block.Set( "c.UserName", c.Member.Name );

                block.Set( "c.Created", c.Created );
                block.Set( "c.Content", c.Content );

                Microblog blog = microblogService.GetById( c.FeedId );
                if (blog == null) {
                    block.Set( "c.Microblog", "--" );
                    block.Set( "c.MicroblogLink", "#" );
                }
                else {

                    block.Set( "c.Microblog", blog.Content );
                    block.Set( "c.MicroblogLink", wojilu.Web.Controller.Microblogs.MbLink.ToShowFeed( blog.User, c.FeedId ) );
                }

                block.Next();

            }

            set( "page", list.PageBar );
        }


        private void bindUserInfo( User user ) {
            set( "siteName", config.Instance.Site.SiteName );
            set( "microblogHomeLink", getFullUrl( alink.ToMicroblog() ) );
            set( "homeLink", to( Index, user.Id ) );
            set( "favoriteLink", to( Favorite ) );
            set( "atmeLink", to( Atme ) );
            set( "myCommentLink", to( Comment ) );

            set( "user.Name", user.Name );
            set( "user.Pic", user.PicSX );
            set( "user.PicSmall", user.PicSmall );

            set( "user.PicBig", user.PicM );
            set( "user.Link", getFullUrl( toUser( user ) ) );
            set( "user.MLink", to( Index, user.Id ) );
            set( "user.Signature", user.Signature );
            set( "user.Description", user.Profile.Description );
        }

        private String getFullUrl( String url ) {
            if (url == null) return "";
            if (url.StartsWith( "http" )) return url;
            return strUtil.Join( ctx.url.SiteAndAppPath, url );
        }

        private String bindCmd( User user ) {

            set( "followUrl", to( new wojilu.Web.Controller.Microblogs.MicroblogController().Follow ) );
            set( "cancelUrl", to( new wojilu.Web.Controller.Microblogs.MicroblogController().CancelFollow ) );

            if (ctx.viewer.IsLogin == false) return "<div id=\"lblFollow\"><span>加关注</span></div>";
            if (ctx.viewer.Id == user.Id) return "";
            if (ctx.viewer.IsFriend( user.Id ))
                return "<div id=\"cmdCancelFollow\"><span id=\"followed\">已是好友</span></div>";

            if (ctx.viewer.IsFollowing( user.Id ))
                return "<div id=\"cmdCancelFollow\"><span id=\"followed\">已关注</span><span id=\"cancelFollow\">取消关注</span></div>";

            return "<div id=\"cmdFollow\"><span>加关注</span></div>";
        }



        private void bindUsers( List<User> users, String blockName ) {

            IBlock block = getBlock( blockName );
            foreach (User user in users) {
                block.Set( "user.Name", user.Name );
                block.Set( "user.Face", user.PicSmall );
                block.Set( "user.Link", alink.ToUserMicroblog( user ) );
                block.Next();
            }
        }


    }

}
