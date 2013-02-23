/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using wojilu.DI;
using wojilu.Aop;
using wojilu.Common.Microblogs.Service;
using wojilu.Common.Microblogs.Domain;
using wojilu.OAuth.Connects;

namespace wojilu.OAuth.Connects {

    public class QQWeiboSync : MethodObserver {

        private static readonly ILog logger = LogManager.GetLogger( typeof( QQWeiboSync ) );

        public override void ObserveMethods() {

            observe( typeof( MicroblogService ), "Insert" );

        }

        public override void After( object returnValue, MethodInfo method, object[] args, object target ) {

            Microblog blog = args[0] as Microblog;
            if (blog == null || blog.ParentId > 0) return;
            if (blog.User == null || blog.User.Id <= 0) return;

            if (QQWeiboJobHelper.IsQQWeiboSync( blog.Id )) return; // 是否已经同步过

            UserConnect uc = ObjectContext.Create<UserConnectService>()
                .GetConnectInfo( blog.User.Id, typeof( QQWeiboConnect ).FullName );

            // 1. 检查：用户是否绑定，是否允许同步
            if (uc == null) return; // 绑定
            if (uc.NoSync == 1) {
                logger.Info( "取消同步，因为用户明确禁止" );
                return;
            }

            // 2. 获取 access token
            AccessToken x = new AccessToken();
            x.Token = uc.AccessToken;
            x.Uid = uc.Uid;

            // 3. 同步
            QQWeiboConnect connect = AuthConnectFactory.GetConnect( typeof( QQWeiboConnect ).FullName ) as QQWeiboConnect;
            connect.Publish( x, blog.Content, PathHelper.Map( blog.PicOriginal ) );

            // 设置已经同步标记
            QQWeiboJobHelper.AddQQWeiboSyncItem( blog.Id );

        }

    }

}
