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

        // 用户登录区块：只起占位作用，没有具体意义
        public virtual IList GetLoginInfo() {
            return new ArrayList();
        }

        public virtual void Login( User user, LoginTime expiration, String ip, MvcContext ctx ) {
            updateLastLogin( user, ip );
            ctx.web.UserLogin( user.Id, user.Name, expiration );
            OnlineStats.Instance.AddMemberCount();
        }

        private void checkMsg( User user ) {



        }


        //private void logForAdmin() {
        //    AdminLog log = new AdminLog();
        //    log.Member = _m;
        //    log.MemberName = _m.Name;
        //    log.MemberUrl = _m.Url;
        //    log.AdminAction = lang.get( "login_site_admin" );
        //    //log.Ip = util.GetIp();
        //    log.insert();
        //}

        // 直接登录
        private void updateLastLogin( User user, String ip ) {
            if (user.LastLoginTime.ToShortDateString() != DateTime.Now.ToShortDateString()) {
                user.LoginDay++;
            }
            user.LoginCount++;
            user.LastLoginTime = DateTime.Now;

            user.LastLoginIp = ip;
            string[] arrPropertyName = new string[] { "LoginDay", "LoginCount", "LastLoginTime", "LastLoginIp" };
            db.update( user, arrPropertyName );
        }

        // 每次页面加载都要检查
        public virtual void UpdateLastLogin( User user, String ip ) {


            if (user.LastLoginTime.ToShortDateString() != DateTime.Now.ToShortDateString()) {
                user.LoginDay++;
                user.LoginCount++;
                user.LastLoginTime = DateTime.Now;
                user.LastLoginIp = ip;
                string[] arrPropertyName = new string[] { "LoginDay", "LoginCount", "LastLoginTime", "LastLoginIp" };
                db.update( user, arrPropertyName );
            }

        }


    }
}
