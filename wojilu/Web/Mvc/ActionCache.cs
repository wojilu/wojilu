using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;
using wojilu.Caching;

namespace wojilu.Web.Mvc {

    /// <summary>
    /// action 缓存基类，包括缓存key，更新操作等
    /// </summary>
    public class ActionCache : ActionObserver {

        /// <summary>
        /// 设置被缓存的 action 的缓存 key。如果返回 null ，则不会被缓存。
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public virtual string GetCacheKey( MvcContext ctx, String actionName ) {
            return null;
        }

    }
}
