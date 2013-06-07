/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Controller {

    public class LinkController : ControllerBase {

        [Login]
        public void MyMicroblog() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Microblogs.My.MicroblogController().Home ) );
        }

        [Login]
        public void MySpace() {
            redirectDirect( toUser( ctx.viewer.obj ) );
        }

        [Login]
        public void MyHome() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.FeedController().My, 0 ) );
        }

        [Login]
        public void NewMsg( int targetId ) {
            redirectDirect( Link.To( ctx.viewer.obj, new Users.Admin.MsgController().New, targetId ) );
        }

        //-----------------------------

        [Login]
        public void MyFriends() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.Friends.FriendController().List, 0 ) );
        }

        [Login]
        public void MyMsg() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.MsgController().Index ) );
        }

        [Login]
        public void MyApp() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.AppController().Index ) );
        }

        [Login]
        public void MyMenu() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.MenuController().Index ) );
        }


        [Login]
        public void MyProfile() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.UserProfileController().Profile ) );
        }


        [Login]
        public void MyPic() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.UserProfileController().Face ) );
        }


        [Login]
        public void MyPwd() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.UserProfileController().Pwd ) );
        }


        [Login]
        public void MyContact() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.UserProfileController().Contact ) );
        }


        [Login]
        public void MyInterest() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.UserProfileController().Interest ) );
        }


        [Login]
        public void MyTag() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.UserProfileController().Tag ) );
        }


        [Login]
        public void MyPrivacy() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.UserProfileController().Privacy ) );
        }


        [Login]
        public void MyCredit() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.CreditController().My ) );
        }


        [Login]
        public void MyInvite() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.InviteController().Index ) );
        }


        [Login]
        public void MyGroup() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.MyGroupController().My ) );
        }


        [Login]
        public void MyNotification() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.NotificationController().List ) );
        }


        [Login]
        public void MySkin() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.SkinController().My ) );
        }

    }

}
