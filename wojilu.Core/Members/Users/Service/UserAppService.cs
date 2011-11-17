/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */


using wojilu.Members.Users.Domain;
using wojilu.Common.MemberApp.Interface;
using System;
using wojilu.Common.MemberApp;

namespace wojilu.Members.Users.Service {

    public class UserAppService : MemberAppServiceBase {

        public UserAppService() {
            menuService = new UserMenuService();
        }

        public override Type GetMemberType() {
            return typeof( User );
        }

        public override IMemberApp New() {
            return new UserApp();
        }

        public override Object getObj() {
            return new UserApp();
        }

    }

}
