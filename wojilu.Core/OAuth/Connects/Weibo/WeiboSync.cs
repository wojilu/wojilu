/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using wojilu.Aop;
using wojilu.Common.Microblogs.Service;
using wojilu.Common.Microblogs.Domain;
using wojilu.OAuth.Connects;
using wojilu.Members.Users.Domain;
using wojilu.DI;

namespace wojilu.OAuth.Connects {


    public class WeiboSync : MethodObserver {

        private static readonly ILog logger = LogManager.GetLogger( typeof( WeiboSync ) );

        public override void ObserveMethods() {

            observe( typeof( MicroblogService ), "Insert" );

        }

        public override void After( object returnValue, MethodInfo method, object[] args, object target ) {

            Microblog blog = args[0] as Microblog;
            if (blog == null || blog.ParentId > 0) return;
            if (blog.User == null || blog.User.Id <= 0) return;

            UserConnect uc = ObjectContext.Create<UserConnectService>()
                .GetConnectInfo( blog.User.Id, typeof( WeiboConnect ).FullName );

            // 1. 检查：用户是否绑定，是否允许同步
            if (uc == null) return; // 绑定
            if (uc.NoSync == 1) {
                logger.Info( "取消同步，因为用户明确禁止" );
                return;
            }

            // 2. 同步
            WeiboConnect connect = AuthConnectFactory.GetConnect( typeof( WeiboConnect ).FullName ) as WeiboConnect;
            connect.Publish( uc.AccessToken, blog.Content, PathHelper.Map( blog.PicOriginal ) );
        }


    }

}
