/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.IO;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.Utils;
using wojilu.Members.Users.Domain;
using wojilu.Common.Microblogs.Service;
using wojilu.Common.Microblogs.Domain;
using wojilu.Common.Microblogs.Interface;
using wojilu.Apps.Photo.Domain;
using wojilu.Apps.Photo.Interface;
using wojilu.Apps.Photo.Service;
using wojilu.Web.Controller.Users.Admin;
using System.Collections.Generic;
using wojilu.Serialization;
using wojilu.Common;

namespace wojilu.Web.Controller.Microblogs {

    public class MicroblogSaveController : ControllerBase {

        public IMicroblogService microblogService { get; set; }
        public IPhotoPostService postService { get; set; }

        public MicroblogSaveController() {
            microblogService = new MicroblogService();
            postService = new PhotoPostService();
        }


        [Login, HttpPost, DbTransaction]
        public void Create() {

            if (Component.IsClose( typeof( MicroblogApp ) )) {
                actionContent( "对不起，微博功能暂停运行" );
                return;
            }

            String content = ctx.Post( "Content" );
            if (strUtil.IsNullOrEmpty( content )) {
                actionContent( lang( "exContent" ) );
                return;
            }

            String picUrl = ctx.Post( "PicUrl" );

            User user = ctx.viewer.obj as User;

            Microblog blog = new Microblog();
            blog.Content = content;
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

            echoJson( JsonString.Convert( dic ) );
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

            echoJson( JsonString.Convert( dic ) );
        }


        private String getOneBlogHtml( Microblog blog ) {
            List<MicroblogVo> volist = new List<MicroblogVo>();
            MicroblogVo mbvo = new MicroblogVo();
            mbvo.Microblog = blog;
            mbvo.IsFavorite = false;
            volist.Add( mbvo );

            ctx.SetItem( "_microblogVoList", volist );
            ctx.SetItem( "_showUserFace", true );

            return loadHtml( new Microblogs.MicroblogController().bindBlogs );
        }

        private void setVideoInfo( Microblog blog ) {

            int videoId = ctx.PostInt( "videoId" );

            if (videoId <= 0) return;

            MicroblogVideoTemp mvt = MicroblogVideoTemp.findById( videoId );
            if (mvt == null) return;

            blog.FlashUrl = mvt.FlashUrl;
            blog.PageUrl = mvt.PageUrl;
            blog.PicUrl = mvt.PicUrl;
        }



    }
}
