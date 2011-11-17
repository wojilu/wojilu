/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.Members.Users.Domain;
using wojilu.Common.Menus;
using wojilu.Common.Menus.Interface;

namespace wojilu.Members.Users.Service {

    public class UserMenuService : MenuServiceBase {

        public override Type GetMemberType() {
            return typeof( User );
        }

        public override IMenu New() {
            return new UserMenu();
        }

        public override Object getObj() {
            return new UserMenu();
        }


    }

}
