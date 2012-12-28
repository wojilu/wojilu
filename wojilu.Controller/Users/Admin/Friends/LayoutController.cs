using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Users.Admin.Friends {


    public class LayoutController : ControllerBase{

        public override void Layout() {

            set( "f.ListLink", to( new FriendController().List, 0 ) );
            set( "f.FollowingLink", to( new FriendController().FollowingList ) );
            set( "f.MoreLink", to( new FriendController().More ) );
            set( "f.SearchLink", Link.To( Site.Instance, new wojilu.Web.Controller.Users.MainController().Search ) );
            set( "f.InviteLink", to( new InviteController().Index ) );
            set( "f.Rank", Link.To( Site.Instance, new wojilu.Web.Controller.Users.MainController().Rank ) );
        }

    }

}
