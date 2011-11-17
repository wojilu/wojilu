using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Caching;
using wojilu.Members.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Web.Context;
using wojilu.Web.Mvc.Utils;
using wojilu.Web.Controller.Common.Caching;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Forum.Domain;

namespace wojilu.Web.Controller.Forum.Caching {
    
    // TODO
    // 最新注册用户
    // 当前在线：通过ajax从ctx中获取，不再显示列表，只显示数值统计
    //（重新设计在线功能：访问的时候，只增加不删除。删除定时进行：每隔1分钟删除过期的用户）

    // 缓存列表页前5页（相应也要更新前5页）
    // 缓存最新帖子、排名等
    // 所有置顶、精华等操作也要更新缓存
    // 所有后台操作也要更新缓存
    public class IndexViewCacher {

        public static void Update( IMember owner, int appId ) {


            String key = GetCacheKey( owner, appId );

            String content = getIndexCache( appId, owner );

            CacheManager.GetApplicationCache().Put( key, content );

        }

        public static String GetCacheKey( IMember owner, int appId ) {
            return owner.GetType().FullName + "_" + owner.Url.Replace( "/", "" ) + "_" + typeof( wojilu.Web.Controller.Forum.ForumController ).FullName + "_" + appId;
        }

        private static string getIndexCache( int appId, IMember owner ) {
            MvcContext ctx = MockContext.GetOne( owner, typeof( ForumApp ), appId );
            String content = ControllerRunner.Run( ctx, new wojilu.Web.Controller.Forum.ForumController().Index );

            return content;
        }

    }
}
