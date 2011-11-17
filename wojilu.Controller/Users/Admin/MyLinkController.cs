/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Common.MemberApp.Interface;
using wojilu.Members.Users.Service;
using wojilu.Common;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Users.Admin {


    public class MyLinkController : ControllerBase {

        public IMemberAppService userAppService { get; set; }

        public MyLinkController() {
            userAppService = new UserAppService();
        }

        public override void CheckPermission() {

            Boolean isUserLinksClose = Component.IsClose( typeof( UserLinks ) );
            if (isUserLinksClose) {
                echo( "对不起，本功能已经停用" );
            }
        }

        public void Index() {

            set( "addMenu", to( new MenuController().AddMenu ) );

            set( "aboutMe", lnkFull( to( new Users.ProfileController().Main ) ) );
            set( "microBlog", lnkFull( alink.ToUserMicroblog( ctx.owner.obj ) ) );
            set( "recentVisitor", lnkFull( to( new Users.VisitorController().Index ) ) );
            set( "friends", lnkFull( to( new Users.FriendController().FriendList ) ) );
            set( "feedback", lnkFull( to( new Users.FeedbackController().List ) ) );
            set( "shareLink", lnkFull( to( new Users.ShareController().Index ) ) );

            set( "forumTopic", lnkFull( to( new Users.ForumController().Topic ) ) );
            set( "forumPost", lnkFull( to( new Users.ForumController().Post ) ) );


            IList apps = userAppService.GetByMember( ctx.owner.obj.Id );
            IBlock block = getBlock( "list" );
            foreach (IMemberApp app in apps) {
                block.Set( "app.Name", app.Name );
                String lnk = lnkFull( alink.ToUserAppFull( app ) );
                block.Set( "app.Link", lnk );
                block.Next();
            }

        }

        private String lnkFull( String link ) {
            if (link.StartsWith( "http" )) return link; // 开启二级域名支持情况下，link是完整url
            return strUtil.Join( ctx.url.SiteUrl, link );
        }

    }

}
