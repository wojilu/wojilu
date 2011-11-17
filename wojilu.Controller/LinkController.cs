using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Controller {

    public class LinkController : ControllerBase {



        [Login]
        public void MyMicroblog() {
            redirectUrl( Link.T2( ctx.viewer.obj, new wojilu.Web.Controller.Microblogs.My.MicroblogController().Home ) );
        }

        [Login]
        public void MySpace() {
            redirectUrl( Link.ToMember( ctx.viewer.obj ) );
        }

        [Login]
        public void MyHome() {
            redirectUrl( Link.T2( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.FeedController().My, 0 ) );
        }

        [Login]
        public void NewMsg( int targetId ) {
            redirectUrl( Link.T2( ctx.viewer.obj, new Users.Admin.MsgController().New, targetId ) );
        }

        //-----------------------------

        [Login]
        public void MyFriends() {
            redirectUrl( Link.T2( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.Friends.FriendController().List, 0 ) );
        }

        [Login]
        public void MyMsg() {
            redirectUrl( Link.T2( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.MsgController().All ) );
        }

        [Login]
        public void MyApp() {
            redirectUrl( Link.T2( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.AppController().Index ) );
        }

        [Login]
        public void MyMenu() {
            redirectUrl( Link.T2( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.MenuController().Index ) );
        }


        [Login]
        public void MyProfile() {
            redirectUrl( Link.T2( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.UserProfileController().Profile ) );
        }


        [Login]
        public void MyPic() {
            redirectUrl( Link.T2( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.UserProfileController().Face ) );
        }


        [Login]
        public void MyPwd() {
            redirectUrl( Link.T2( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.UserProfileController().Pwd ) );
        }


        [Login]
        public void MyContact() {
            redirectUrl( Link.T2( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.UserProfileController().Contact ) );
        }


        [Login]
        public void MyInterest() {
            redirectUrl( Link.T2( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.UserProfileController().Interest ) );
        }


        [Login]
        public void MyTag() {
            redirectUrl( Link.T2( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.UserProfileController().Tag ) );
        }


        [Login]
        public void MyPrivacy() {
            redirectUrl( Link.T2( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.UserProfileController().Privacy ) );
        }


        [Login]
        public void MyCredit() {
            redirectUrl( Link.T2( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.CreditController().My ) );
        }


        [Login]
        public void MyInvite() {
            redirectUrl( Link.T2( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.InviteController().Index ) );
        }


        [Login]
        public void MyGroup() {
            redirectUrl( Link.T2( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.MyGroupController().My ) );
        }


        [Login]
        public void MyNotification() {
            redirectUrl( Link.T2( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.NotificationController().List ) );
        }


        [Login]
        public void MySkin() {
            redirectUrl( Link.T2( ctx.viewer.obj, new wojilu.Web.Controller.Users.Admin.SkinController().My ) );
        }

    }

}
