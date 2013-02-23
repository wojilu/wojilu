/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.OAuth.Connects {

    // 避免腾讯微博和QQ都绑定状态下的重复发布
    public class QQWeiboJobHelper {

        public static void AddQQWeiboSyncItem( int blogId ) {
            String key = getItemKey( blogId );
            Caching.CacheManager.GetApplicationCache().Put( key, blogId );
        }

        public static Boolean IsQQWeiboSync( int blogId ) {
            String key = getItemKey( blogId );
            Object syncItem = Caching.CacheManager.GetApplicationCache().Get( key );
            if (syncItem == null) return false;

            Caching.CacheManager.GetApplicationCache().Remove( key );
            return true;
        }

        private static String getItemKey( int blogId ) {
            return "__microblog_sync_qq_t_" + blogId;
        }

    }
}
