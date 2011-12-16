/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.Apps.Shop.Interface;
using wojilu.Web.Mvc;
using wojilu.Apps.Shop.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Web.Controller.Users;
using wojilu.Apps.Shop.Service;
using wojilu.Web.Controller.Shop.Utils;

namespace wojilu.Web.Controller.Shop.Binder {

    public class MyInfoBinderController : ControllerBase, ISectionBinder {

        public IShopCustomTemplateService ctService { get; set; }

        public MyInfoBinderController() {
            ctService = new ShopCustomTemplateService();
        }

        public void Bind( ShopSection section, IList serviceData ) {

            TemplateUtil.loadTemplate( this, section, ctService );

            User user = ctx.owner.obj as User;
            if (user == null) return;

            bind( "user", new UserVo( user ) );
        }

    }

}
