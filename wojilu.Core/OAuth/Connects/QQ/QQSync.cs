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

    public class QQSync : MethodObserver {

        private static readonly ILog logger = LogManager.GetLogger( typeof( QQSync ) );

        public IUserConnectService connectService { get; set; }

        public QQSync() {
            connectService = new UserConnectService();
        }

        public override void ObserveMethods() {

            observe( typeof( MicroblogService ), "Insert" );

        }

        private String _blogContent = null;

        public override void Before( MethodInfo method, object[] args, object target ) {
            Microblog blog = args[0] as Microblog;
            _blogContent = blog.Content; // 获得原始的微博内容（没有经过 #tag# 和链接处理）
        }

        public override void After( object returnValue, MethodInfo method, object[] args, object target ) {

            Microblog blog = args[0] as Microblog;

            if (blog == null || blog.ParentId > 0) return;
            if (blog.User == null || blog.User.Id <= 0) return;

            if (QQWeiboJobHelper.IsQQWeiboSync( blog.Id )) return; // 是否已经同步过

            UserConnect uc = connectService.GetConnectInfo( blog.User.Id, typeof( QQConnect ).FullName );

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
            QQConnect connect = AuthConnectFactory.GetConnect( typeof( QQConnect ).FullName ) as QQConnect;
            connect.Publish( x, _blogContent, getPicDiskPath( blog.Pic ) );

            // 设置已经同步标记
            QQWeiboJobHelper.AddQQWeiboSyncItem( blog.Id );

        }

        private String getPicDiskPath( String pic ) {
            if (strUtil.IsNullOrEmpty( pic )) return null;
            return PathHelper.Map( strUtil.Join( sys.Path.DiskPhoto, pic ) );
        }

    }

}
