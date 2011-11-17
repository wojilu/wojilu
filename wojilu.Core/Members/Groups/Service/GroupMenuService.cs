/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Members.Groups.Domain;
using wojilu.Common.Menus;
using wojilu.Common.Menus.Interface;

namespace wojilu.Members.Groups.Service {

    public class GroupMenuService : MenuServiceBase {

        public override IMenu New() {
            return new GroupMenu();
        }

        public override Type GetMemberType() {
            return typeof( Group );
        }

        public override Object getObj() {
            return new GroupMenu();
        }

    }

}
