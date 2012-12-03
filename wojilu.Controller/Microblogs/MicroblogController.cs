/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Common.Microblogs.Service;
using wojilu.Common.Microblogs.Domain;
using wojilu.Common.Microblogs.Interface;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;
using wojilu.Members.Users.Domain;
using wojilu.Web.Controller.Common;
using wojilu.Web.Controller.Users;
using wojilu.Common.Microblogs;

namespace wojilu.Web.Controller.Microblogs {

    public partial class MicroblogController : ControllerBase {

        public IMicroblogService microblogService { get; set; }
        public IFollowerService followService { get; set; }
        public IVisitorService visitorService { get; set; }
        public MicroblogFavoriteService mfService { get; set; }
        public MicroblogCommentService commentService { get; set; }
        public MicroblogAtService matService { get; set; }
        public UserTagService userTagService { get; set; }

        public MicroblogController() {
            microblogService = new MicroblogService();
            followService = new FollowerService();
            visitorService = new VisitorService();
            mfService = new MicroblogFavoriteService();
            commentService = new MicroblogCommentService();
            matService = new MicroblogAtService();
            userTagService = new UserTagService();
        }

        public override void Layout() {

            User user = ctx.owner.obj as User;
            ctx.SetItem( "loadHeader", false );

            bindUserInfo( user );
            bindUserTags();

            set( "cmdFollow", bindCmd( user ) );

            List<User> followers = followService.GetRecentFriendsAndFollowers( ctx.owner.Id, 18 );
            bindUsers( followers, "users" );

            List<User> visitors = visitorService.GetRecent( 9, ctx.owner.Id );
            bindUsers( visitors, "visitor" );

            set( "moreFollowers", to( new wojilu.Web.Controller.Microblogs.FriendController().FollowerList ) );
            set( "moreVisitors", to( new wojilu.Web.Controller.Users.VisitorController().Index ) );
        }


        public void List() {

            Page.Title = ctx.owner.obj.Name + "的微博_" + config.Instance.Site.SiteName;
            Page.Keywords = ctx.owner.obj.Name + "的微博";


            if (ctx.viewer.HasPrivacyPermission( ctx.owner.obj, UserPermission.Microblog.ToString() ) == false) {
                echo( lang( "exVisitNoPermission" ) );
                return;
            }

            set( "user.Name", ctx.owner.obj.Name );

            DataPage<Microblog> list = microblogService.GetPageList( ctx.owner.obj.Id, MicroblogAppSetting.Instance.MicroblogPageSize );
            List<MicroblogVo> volist = mfService.CheckFavorite( list.Results, ctx.viewer.Id );

            ctx.SetItem( "_microblogVoList", volist );
            ctx.SetItem( "_showUserFace", false );
            load( "blogList", bindBlogs );

            set( "page", list.PageBar );

        }

        public void Show( int id ) {

            Microblog blog = microblogService.GetById( id );
            if (blog == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            // 详细内容：使用通用视图文件
            loadCommonView( blog );

            // 评论列表
            DataPage<MicroblogComment> comments = commentService.GetComments( id, 20 );
            IBlock cblock = getBlock( "comments" );
            bindComments( cblock, comments.Results );
            String pager = comments.PageCount > 1 ? comments.PageBar : "";
            set( "page", pager );

            // 评论表单
            target( new MicroblogCommentsController().SaveReply );
            set( "c.RootId", id );
            set( "c.ParentId", 0 );
            set( "viewer.PicSmall", ctx.viewer.obj.PicSmall );
        }



        //------------------------------------------------------------------------------------------------


        [NonVisit]
        public void bindBlogs() {

            List<MicroblogVo> list = (List<MicroblogVo>)ctx.GetItem( "_microblogVoList" );
            Boolean showUserFace = (Boolean)ctx.GetItem( "_showUserFace" );

            IBlock block = getBlock( "list" );
            foreach (MicroblogVo blog in list) {

                bindOne( block, blog.Microblog, blog.IsFavorite, showUserFace );

                block.Next();

            }
        }

        //------------------------------------------------------------------------------------------------

        [Login]
        public void Forward( int id ) {

            Microblog blog = microblogService.GetById( id );
            if (blog == null) {
                errors.Add( lang( "exDataNotFound" ) );
                echoError();
                return;
            }

            IBlock pblock = getBlock( "parent" );

            if (blog.ParentId <= 0) {
                set( "blog.User", blog.User.Name );
                set( "blog.Content", blog.Content );
                set( "commentBody", "" );
            }

            else {

                Microblog parent = microblogService.GetById( blog.ParentId );

                set( "blog.User", blog.User.Name );
                set( "blog.Content", parent.Content );
                set( "commentBody", "//@" + blog.User.Name + ":" + blog.Content );

                pblock.Set( "blog.ParentUser", parent.User.Name );
                pblock.Next();

            }

            target( Save, id );

        }

        [HttpPost, DbTransaction]
        public void Save( int id ) {

            Microblog tblog = microblogService.GetById( id );

            int isComment = ctx.PostIsCheck( "isComment" );
            int isCommentParent = ctx.PostIsCheck( "isComment" );

            String content = ctx.Post( "content" );
            if (strUtil.IsNullOrEmpty( content )) content = "转发微博";

            Microblog blog = new Microblog();
            blog.Content = content;
            blog.ParentId = getParentId( tblog ); // 转发微博
            blog.User = ctx.viewer.obj as User;
            blog.Ip = ctx.Ip;

            microblogService.Insert( blog );

            // 给对方评论
            if (isComment == 1) {
                saveComment( tblog, content );
            }

            // 给原始微博评论
            if (isCommentParent == 1) {
                saveCommentParent( tblog, content );
            }

            echoToParent( "转发成功" );
        }


        private int getParentId( Microblog tblog ) {
            if (tblog.ParentId <= 0) return tblog.Id;
            return tblog.ParentId;
        }

        private void saveCommentParent( Microblog tblog, string content ) {

            if (tblog.ParentId <= 0) return;

            Microblog parent = microblogService.GetById( tblog.ParentId );
            saveComment( parent, content );
        }


        private void saveComment( Microblog tblog, String content ) {
            MicroblogComment c = new MicroblogComment();
            c.Root = tblog;
            c.Content = content;
            c.User = ctx.viewer.obj as User;
            c.Ip = ctx.Ip;

            commentService.InsertComment( c, to( new MicroblogController().Show, tblog.Id ) );
        }

        [HttpPost, DbTransaction]
        public void SaveFavorite( int id ) {

            if (ctx.viewer.IsLogin == false) {
                echoJsonMsg( "请先登录", false, "" );
                return;
            }

            Microblog blog = microblogService.GetById( id );
            if (blog == null) {
                errors.Add( lang( "exDataNotFound" ) );
                echoError();
                return;
            }


            mfService.SaveFavorite( ctx.viewer.Id, blog );

            echoJsonMsg( "ok", true, null );

        }

        [HttpPost, DbTransaction]
        public void CancelFavorite( int id ) {

            Microblog blog = microblogService.GetById( id );
            if (blog == null) {
                errors.Add( lang( "exDataNotFound" ) );
                echoError();
                return;
            }


            mfService.CancelFavorite( ctx.viewer.Id, blog );

            echoJsonMsg( "ok", true, null );

        }

        //------------------------------------------------------------------------------------------------

        [HttpPost, DbTransaction]
        public void Follow() {

            if (ctx.viewer.IsLogin == false) errors.Add( "请先登录" );
            if (ctx.viewer.IsFollowing( ctx.owner.Id )) errors.Add( "已经关注" );
            if (ctx.HasErrors) {
                echoText( errors.ErrorsText );
                return;
            }

            followService.Follow( ctx.viewer.Id, ctx.owner.Id );
            echoAjaxOk();
        }

        [HttpPost, DbTransaction]
        public void CancelFollow() {

            if (ctx.viewer.IsLogin == false) errors.Add( "请先登录" );
            if (ctx.viewer.IsFollowing( ctx.owner.Id ) == false) errors.Add( "尚未关注，不可取消" );
            if (ctx.HasErrors) {
                echoText( errors.ErrorsText );
                return;
            }

            followService.DeleteFollow( ctx.viewer.Id, ctx.owner.Id );
            echoAjaxOk();
        }


    }
}
