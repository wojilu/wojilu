/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Reflection;
using System.Web.Security;
using wojilu.Aop;
using wojilu.Members.Users.Domain;
using wojilu.Web.Context;

namespace wojilu.OAuth {

    /// <summary>
    /// 检查 access token 是否过期
    /// </summary>
    public class ExpirationChecker : MethodObserver {

        private static readonly ILog logger = LogManager.GetLogger( typeof( ExpirationChecker ) );

        public IUserConnectService connectService { get; set; }

        public ExpirationChecker() {
            connectService = new UserConnectService();
        }

        public override void ObserveMethods() {

            // 监控用户初始化方法
            observe( typeof( wojilu.Web.Context.Initor.ViewerInit ), "Init" );
        }

        public override void After( object returnValue, MethodInfo method, object[] args, object target ) {

            MvcContext ctx = args[0] as MvcContext;
            if (ctx.viewer.IsLogin == false) return;

            User user = ctx.viewer.obj as User;

            if (user.LoginType > 0) {
                checkExpiration( user, ctx );
            }
        }

        // 第三方登录
        private void checkExpiration( User user, MvcContext ctx ) {

            UserConnect x = connectService.GetById( user.LoginType );

            // 已经解除第三方绑定
            if (x == null) {
                logger.Info( "UserConnect is null" );
                signOut( ctx );
                return;
            }

            // access token 过期
            if (x.IsExpired) {

                logger.Info( "access toke  is expired, UserConnect.Id=" + x.Id );
                signOut( ctx );
                return;
            }
        }

        private void signOut( MvcContext ctx ) {
            // 注销并跳转到首页
            FormsAuthentication.SignOut();
            ctx.utils.clearResource();
            ctx.web.Redirect( ctx.url.SiteAndAppPath );
        }

    }

}
