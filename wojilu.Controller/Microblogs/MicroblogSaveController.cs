/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Apps.Photo.Interface;
using wojilu.Apps.Photo.Service;

using wojilu.Common;
using wojilu.Common.Microblogs.Domain;
using wojilu.Common.Microblogs.Interface;
using wojilu.Common.Microblogs.Service;

using wojilu.Members.Users.Domain;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Controller.Microblogs {

    public class MicroblogSaveController : ControllerBase {

        public virtual IMicroblogService microblogService { get; set; }
        public virtual IPhotoPostService postService { get; set; }

        public MicroblogSaveController() {
            microblogService = new MicroblogService();
            postService = new PhotoPostService();
        }


        [Login, HttpPost, DbTransaction]
        public virtual void Create() {

            //if (Component.IsClose( typeof( MicroblogApp ) )) {
            //    content( "对不起，微博功能暂停运行" );
            //    return;
            //}

            String blogContent = ctx.Post( "Content" );
            if (strUtil.IsNullOrEmpty( blogContent )) {
                content( lang( "exContent" ) );
                return;
            }

            String picUrl = ctx.Post( "PicUrl" );

            User user = ctx.viewer.obj as User;

            Microblog blog = new Microblog();
            blog.Content = blogContent;
            blog.Ip = ctx.Ip;
            blog.User = user;
            blog.Pic = picUrl;

            setVideoInfo( blog );

            microblogService.Insert( blog );

            String srcType = ctx.Post( "srcType" );

            if ("shareBox".Equals( srcType )) {
                returnCloseBox();
            }
            else if ("mbHome".Equals( srcType )) {
                returnOneBlogHtml( blog );
            }
            else {
                echoRedirect( "ok", to( new My.MicroblogController().Home ) );
            }

        }

        private void returnCloseBox() {
            Dictionary<String, String> dic = new Dictionary<string, string>();
            dic.Add( "IsValid", "true" );
            dic.Add( "SrcType", "shareBox" );
            dic.Add( "ForwardUrl", "" );

            echoJson( dic );
        }

        private void returnOneBlogHtml( Microblog blog ) {

            // 加载最新微博html片段
            Dictionary<String, Object> dic = new Dictionary<String, Object>();
            dic.Add( "IsValid", true );
            dic.Add( "SrcType", "mbHome" );
            dic.Add( "ForwardUrl", "" );
            dic.Add( "BlogId", blog.Id );

            String html = getOneBlogHtml( blog );
            dic.Add( "Info", html );

            echoJson( dic );
        }


        private String getOneBlogHtml( Microblog blog ) {
            List<MicroblogVo> volist = new List<MicroblogVo>();
            MicroblogVo mbvo = new MicroblogVo();
            mbvo.Microblog = blog;
            mbvo.IsFavorite = false;
            volist.Add( mbvo );

            ctx.SetItem( "_microblogVoList", volist );
            ctx.SetItem( "_showUserFace", true );

            if (ctx.Post( "fromPage" ) == "microblogPage") {
                ctx.SetItem( "_showType", "microblog" );
            }

            return loadHtml( new Microblogs.MicroblogController().bindBlogs );
        }

        private void setVideoInfo( Microblog blog ) {

            long videoId = ctx.PostLong( "videoId" );

            if (videoId <= 0) return;

            MicroblogVideoTemp mvt = MicroblogVideoTemp.findById( videoId );
            if (mvt == null) return;

            blog.FlashUrl = mvt.FlashUrl;
            blog.PageUrl = mvt.PageUrl;
            blog.PicUrl = mvt.PicUrl;
        }


        [HttpDelete, DbTransaction]
        public virtual void Delete( long id ) {

            Microblog blog = microblogService.GetById( id );
            if (blog == null) {
                throw new NullReferenceException( lang( "exDataNotFound" ) );
            }

            if (hasPermission( blog ) == false) {
                echoError( lang( "exNoPermission" ) );
                return;
            }

            microblogService.Delete( blog );

            echoAjaxOk();
        }

        private bool hasPermission( Microblog blog ) {

            if (ctx.viewer.IsLogin == false) return false;

            if (blog.User.Id == ctx.viewer.Id) return true;
            if (ctx.viewer.IsAdministrator()) return true;
            return false;
        }


    }
}
