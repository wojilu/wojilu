/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Collections;

using wojilu.Web.Mvc;

using wojilu.Common.MemberApp.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Service;
using wojilu.Members.Users.Interface;

using wojilu.Members.Sites.Service;
using wojilu.Members.Sites.Domain;
using wojilu.Web.Controller.Photo;

namespace wojilu.Web.Controller.Admin.Sys {

    public class DashboardController : ControllerBase {

        public IUserService userService { get; set; }
        public IMemberAppService siteAppService { get; set; }

        public DashboardController() {
            userService = new UserService();
            siteAppService = new SiteAppService();
        }

        public void Links() {

            set( "addMenu", to( new Admin.MenuController().AddMenu ) );
            set( "addFooterMenuLink", to( new FooterMenuController().Add ) );

            set( "groupLink", lnkFull( to( new Web.Controller.Groups.MainController().Index ) ) );
            set( "userLink", lnkFull( to( new Users.MainController().Index ) ) );
            set( "blogLink", lnkFull( to( new Blog.MainController().Index ) ) );
            set( "photoLink", lnkFull( to( new Photo.MainController().Index ) ) );

            set( "photoWfLink", lnkFull( PhotoLink.ToHome() ) );

            set( "microblogLink", lnkFull( alink.ToMicroblog() ) );
            set( "tagLink", lnkFull( to( new TagController().Index ) ) );


            set( "myHomeLink", lnkFull( to( new LinkController().MyHome ) ) );
            set( "mySpaceLink", lnkFull( to( new LinkController().MySpace ) ) );
            set( "myMicroblogLink", lnkFull( to( new LinkController().MyMicroblog ) ) );
            set( "myFriendsLink", lnkFull( to( new LinkController().MyFriends ) ) );
            set( "myMsgLink", lnkFull( to( new LinkController().MyMsg ) ) );
            set( "myAppLink", lnkFull( to( new LinkController().MyApp ) ) );
            set( "myMenuLink", lnkFull( to( new LinkController().MyMenu ) ) );

            set( "myProfileEditLink", lnkFull( to( new LinkController().MyProfile ) ) );
            set( "myPicLink", lnkFull( to( new LinkController().MyPic ) ) );
            set( "myPwdLink", lnkFull( to( new LinkController().MyPwd ) ) );
            set( "myContactLink", lnkFull( to( new LinkController().MyContact ) ) );
            set( "myInterestLink", lnkFull( to( new LinkController().MyInterest ) ) );
            set( "myTagLink", lnkFull( to( new LinkController().MyTag ) ) );

            set( "myPrivacyLink", lnkFull( to( new LinkController().MyPrivacy ) ) );
            set( "myCreditLink", lnkFull( to( new LinkController().MyCredit ) ) );
            set( "myInviteLink", lnkFull( to( new LinkController().MyInvite ) ) );

            set( "myGroupLink", lnkFull( to( new LinkController().MyGroup ) ) );
            set( "myNotificationLink", lnkFull( to( new LinkController().MyNotification ) ) );
            set( "mySkinLink", lnkFull( to( new LinkController().MySkin ) ) );


            IList apps = siteAppService.GetByMember( Site.Instance.Id );
            IBlock block = getBlock( "list" );
            bindAppList( apps, block );
        }

        private void bindAppList( IList apps, IBlock block ) {
            foreach (IMemberApp app in apps) {
                block.Set( "app.Name", app.Name );
                String lnk = lnkFull( alink.ToUserAppFull( app ) );
                block.Set( "app.Link", lnk );
                block.Next();
            }
        }

        private String lnkFull( String link ) {
            if (link.StartsWith( "http" )) return link;
            return strUtil.Join( ctx.url.SiteAndAppPath, link );
        }

        public void Index() {
            Home( -1 );
        }

        public void Home( int id ) {

            view( "Home" );

            bindUsers();

            set( "feedList", loadHtml( new FeedAdminController().Home, id ) );
        }

        //------------------------------------------------------------------------

        private void bindUsers() {
            List<User> newUsers = userService.GetNewList( 8 );
            bindUserList( "newUsers", newUsers );

            List<User> newLoginUsers = userService.GetNewLoginList( 8 );
            bindUserList( "visitors", newLoginUsers );
        }

        private void bindUserList( String blockName, List<User> users ) {

            IBlock block = getBlock( blockName );
            foreach (User user in users) {
                block.Set( "user.Name", user.Name );
                block.Set( "user.Face", user.PicSmall );
                block.Set( "user.Created", cvt.ToTimeString( user.Created ) );
                block.Set( "user.LastLoginTime", cvt.ToTimeString( user.LastLoginTime ) );
                block.Set( "user.Link", toUser( user ) );
                block.Next();
            }
        }
    }

}
