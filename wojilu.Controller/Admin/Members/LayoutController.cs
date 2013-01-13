/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Admin.Members {

    public class LayoutController : ControllerBase {

        public override void Layout() {

            set( "confirmTemplateLink", to( new EmailConfirmController().EditTemplate ) );

            set( "userListLink", to( new UserController().Index ) );
            set( "siteMsgLink", to( new SiteMsgController().Index ) );

            set( "importLink", to( new ImportController().Index ) );

            set( "regLink", to( new UserRegController().Index ) );
            set( "settingLink", to( new UserSettingController().Index ) );

        }
    }

}
