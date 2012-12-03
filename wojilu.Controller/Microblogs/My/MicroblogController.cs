/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

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

namespace wojilu.Web.Controller.Microblogs.My {

    public partial class MicroblogController : ControllerBase {

        public IMicroblogService microblogService { get; set; }
        public IFollowerService followService { get; set; }
        public IVisitorService visitorService { get; set; }
        public MicroblogFavoriteService mfService { get; set; }
        public MicroblogCommentService commentService { get; set; }
        public MicroblogAtService matService { get; set; }

        public IVideoSpider videoSpider { get; set; }

        public MicroblogController() {
            microblogService = new MicroblogService();
            followService = new FollowerService();
            visitorService = new VisitorService();
            mfService = new MicroblogFavoriteService();
            commentService = new MicroblogCommentService();
            matService = new MicroblogAtService();
            videoSpider = new WojiluVideoSpider();
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

        public void Home() {

            Page.Title = "我的微博";


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

        public void Search() {

            view( "Home" );


            load( "publisher", Publisher );

            String q = strUtil.SqlClean( ctx.Get( "q" ), 10 );

            DataPage<Microblog> list = microblogService.GetFollowingPage( ctx.owner.obj.Id, q );
            List<MicroblogVo> volist = mfService.CheckFavorite( list.Results, ctx.viewer.Id );

            ctx.SetItem( "_microblogVoList", volist );
            ctx.SetItem( "_showUserFace", true );
            load( "blogList", new wojilu.Web.Controller.Microblogs.MicroblogController().bindBlogs );

            set( "page", list.PageBar );

        }


        public void Publisher() {

            target( new Microblogs.MicroblogSaveController().Create );

            set( "mbTotalCount", MicroblogAppSetting.Instance.MicroblogContentMax );

            set( "uploadLink", to( new Microblogs.My.MbSaveController().UploadForm ) );
            set( "getVideoUrl", to( GetVideoInfo ) );

            set( "authJson", AdminSecurityUtils.GetAuthCookieJson( ctx ) );

            set( "savPicLink", to( new My.MbSaveController().SavePic ) );

            // swf上传跨域问题
            set( "jsPath", sys.Path.DiskJs );
        }

        public void GetVideoInfo() {

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

        public void Atme() {

            Page.Title = "提到我的微博";


            load( "publisher", Publisher );

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

        [HttpDelete, DbTransaction]
        public void Delete( int id ) {

            Microblog blog = microblogService.GetById( id );
            if (blog == null) {
                throw new NullReferenceException( lang( "exDataNotFound" ) );
            }

            if (blog.User.Id != ctx.viewer.Id) {
                throw new Exception( lang( "exNoPermission" ) );
            }

            microblogService.Delete( blog );

            echoAjaxOk();

        }

    }
}
