/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Context.Initor {

    public class ControllerInit : IContextInit {

        public void Init( MvcContext ctx ) {

            if (ctx.utils.isEnd()) return;

            ControllerBase controller = ControllerFactory.InitController( ctx );

            if (controller == null) {
                String typeName = ctx.route.getControllerNameWithoutRootNamespace();
                String msg = lang.get( "exControllerNotExist" ) + ": " + typeName;

                ctx.utils.endMsg( msg, HttpStatus.NotFound_404 );
                return;

            }

            ctx.utils.setController( controller );


            checkPublishTime( ctx );

        }

        private void checkPublishTime( MvcContext ctx ) {

            if (config.Instance.Site.PublishTimeAfterReg <= 0) return;
            if (ctx.web.ClientHttpMethod == null || ctx.web.ClientHttpMethod == "GET") return;
            if (ctx.viewer.IsAdministrator()) return;

            User user = ctx.viewer.obj as User;
            if (DateTime.Now.Subtract( user.Created ).Hours >= config.Instance.Site.PublishTimeAfterReg) return;

            // 可以登录、注销；阅读通知、接受好友；可以修改用户资料
            String[] skipController = new String[] {
                "wojilu.Web.Controller.MainController",
                "wojilu.Web.Controller.RegisterController",
                "wojilu.Web.Controller.Users.Admin.NotificationController",
                "wojilu.Web.Controller.Users.Admin.UserProfileController"
            };

            if (isSkipController( ctx.route.getControllerFullName(), skipController )) {
                return;
            }

            String msg = string.Format( "对不起，您必须在注册 {0} 小时之后才能操作。请先逛一逛吧，谢谢支持。", config.Instance.Site.PublishTimeAfterReg );
            ctx.utils.endMsg( msg, null );
        }

        private bool isSkipController( String currentController, String[] skipControllerList ) {

            foreach (String controller in skipControllerList) {
                if (currentController == controller) return true;
            }

            return false;
        }



    }

}
