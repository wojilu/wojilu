using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;
using wojilu.Web.Controller.Common;

namespace wojilu.Web.Controller.Users.Admin.Friends {

    public class FriendCategoryController : CategoryBaseController<FriendCategory> {

        public FriendCategoryController() {
            LayoutControllerType = typeof( FriendController );
        }

    }

}
