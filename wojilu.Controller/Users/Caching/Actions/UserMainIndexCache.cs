/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Caching;
using wojilu.Web.Mvc;
using wojilu.Web.Controller.Common.Caching;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Users.Caching {



    public class UserMainIndexCache : ActionCache {

        public override string GetCacheKey( wojilu.Web.Context.MvcContext ctx, string actionName ) {
            return "__action_user_main_index";
        }

        public override void ObserveActions() {

            observe( new RegisterController().SaveReg );
            observe( new wojilu.Web.Controller.Admin.Members.UserController().Operation );

        }

        public override void AfterAction( wojilu.Web.Context.MvcContext ctx ) {
            CacheManager.GetApplicationCache().Remove( GetCacheKey( null, null ) );
        }

    }



}
