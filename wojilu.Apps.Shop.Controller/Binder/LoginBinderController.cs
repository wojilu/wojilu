/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Common;
using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Domain;
using wojilu.Web.Controller.Users.Admin;
using wojilu.Apps.Shop.Service;
using wojilu.Web.Controller.Shop.Utils;

namespace wojilu.Web.Controller.Shop.Binder {

    public class LoginBinderController : ControllerBase, ISectionBinder{

        public IShopCustomTemplateService ctService { get; set; }

        public LoginBinderController() {
            ctService = new ShopCustomTemplateService();
        }

        public void Bind( ShopSection section, IList serviceData ) {

            TemplateUtil.loadTemplate( this, section, ctService );


            set( "loginScriptLink", t2( new MainController().LoginScript ) );
        }

    }
}
