/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;

using wojilu.Members.Groups.Domain;
using wojilu.Common.MemberApp;
using wojilu.Common.MemberApp.Interface;

namespace wojilu.Members.Groups.Service {

    public class GroupAppService : MemberAppServiceBase {

        public GroupAppService() {
            menuService = new GroupMenuService();
        }

        public override Type GetMemberType() {
            return typeof( Group );
        }

        public override IMemberApp New() {
            return new GroupApp();
        }

        public override Object getObj() {
            return new GroupApp();
        }

    }

}
