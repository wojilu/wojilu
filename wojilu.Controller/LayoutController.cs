/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Members.Users.Domain;
using wojilu.Members.Groups.Domain;
using wojilu.Web.Controller.Layouts;
using wojilu.Members.Sites.Domain;
using wojilu.Web.Controller.Common.Caching;

namespace wojilu.Web.Controller {

    public class LayoutController : ControllerBase {

        [CacheAction( typeof( SiteLayoutCache ) )]
        public override void Layout() {


            if (ctx.owner.obj is Site) {

                if (ctx.route.isAdmin || ctx.route.isUserDataAdmin) {
                    run( new SiteLayoutController().AdminLayout );
                }
                else
                    run( new SiteLayoutController().Layout );
            }
            else if (ctx.owner.obj is User) {

                if (ctx.route.isAdmin) 
                    run( new SpaceLayoutController().AdminLayout );
                else
                    run( new SpaceLayoutController().Layout );

            }
            else if (ctx.owner.obj is Group) {

                if (ctx.route.isAdmin)
                    run( new GroupLayoutController().AdminLayout );
                else {
                    run( new GroupLayoutController().Layout );
                }

            }
        }


    }

}
