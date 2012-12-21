/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Caching;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Users.Caching {

    public class UserMainLayoutCache : ActionCache {

        public override string GetCacheKey( wojilu.Web.Context.MvcContext ctx, string actionName ) {
            return "__action_user_main_layout";
        }

        public override void ObserveActions() {
            observe( new wojilu.Web.Controller.Admin.Members.UserController().Operation );
        }


        public override void AfterAction( wojilu.Web.Context.MvcContext ctx ) {
            CacheManager.GetApplicationCache().Remove( GetCacheKey( null, null ) );
        }

    }
}
