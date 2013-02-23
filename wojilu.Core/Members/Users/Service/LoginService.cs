/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.ORM;
using wojilu.Web;
using wojilu.Web.Context;
using wojilu.Common;
using wojilu.Common.Msg.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Common.Onlines;

namespace wojilu.Members.Users.Service {

    public class LoginService : ILoginService {

        private static readonly ILog logger = LogManager.GetLogger( typeof( LoginService ) );

        // 用户登录区块：只起占位作用，没有具体意义
        public virtual IList GetLoginInfo() {
            return new ArrayList();
        }

        // 直接本站登录(不是从第三方登录)
        public virtual void Login( User user, LoginTime expiration, String ip, MvcContext ctx ) {
            Login( user, 0, expiration, ip, ctx );
        }

        // 从第三方带骨
        public virtual void Login( User user, int userConnectId, LoginTime expiration, String ip, MvcContext ctx ) {

            logger.Info( "userConnectId=" + userConnectId );

            user.LoginType = userConnectId;
            updateLastLogin( user, ip );
            ctx.web.UserLogin( user.Id, user.Name, expiration );
            OnlineStats.Instance.AddMemberCount();
        }

        // 直接登录
        private void updateLastLogin( User user, String ip ) {
            if (user.LastLoginTime.ToShortDateString() != DateTime.Now.ToShortDateString()) {
                user.LoginDay++;
            }
            user.LoginCount++;
            user.LastLoginTime = DateTime.Now;

            user.LastLoginIp = ip;
            user.update();
        }

        // 每次页面加载都要检查
        public virtual void UpdateLastLogin( User user, String ip ) {


            if (user.LastLoginTime.ToShortDateString() != DateTime.Now.ToShortDateString()) {
                user.LoginDay++;
                user.LoginCount++;
                user.LastLoginTime = DateTime.Now;
                user.LastLoginIp = ip;
                user.update();
            }

        }


    }
}
