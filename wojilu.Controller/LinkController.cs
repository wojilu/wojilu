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
        public virtual void MyMicroblog() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Microblogs.My.MicroblogController().Home ) );
        }

        [Login]
        public virtual void MySpace() {
            redirectDirect( toUser( ctx.viewer.obj ) );
        }

        [Login]
        public virtual void MyHome() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.HomeController().Index, 0 ) );
        }

        [Login]
        public virtual void NewMsg( long targetId ) {
            redirectDirect( Link.To( ctx.viewer.obj, new Users.Admin.MsgController().New, targetId ) );
        }

        //-----------------------------

        [Login]
        public virtual void MyFriends() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.Friends.FriendController().List, 0 ) );
        }

        [Login]
        public virtual void MyMsg() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.MsgController().Index ) );
        }

        [Login]
        public virtual void MyApp() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.AppController().Index ) );
        }

        [Login]
        public virtual void MyMenu() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.MenuController().Index ) );
        }


        [Login]
        public virtual void MyProfile() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.UserProfileController().Profile ) );
        }


        [Login]
        public virtual void MyPic() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.UserProfileController().Face ) );
        }


        [Login]
        public virtual void MyPwd() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.UserProfileController().Pwd ) );
        }


        [Login]
        public virtual void MyContact() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.UserProfileController().Contact ) );
        }


        [Login]
        public virtual void MyInterest() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.UserProfileController().Interest ) );
        }


        [Login]
        public virtual void MyTag() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.UserProfileController().Tag ) );
        }


        [Login]
        public virtual void MyPrivacy() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.UserProfileController().Privacy ) );
        }


        [Login]
        public virtual void MyCredit() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.CreditController().My ) );
        }


        [Login]
        public virtual void MyInvite() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.InviteController().Index ) );
        }


        [Login]
        public virtual void MyGroup() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.MyGroupController().My ) );
        }


        [Login]
        public virtual void MyNotification() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.NotificationController().List ) );
        }


        [Login]
        public virtual void MySkin() {
            redirectDirect( Link.To( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.SkinController().My ) );
        }

    }

}
