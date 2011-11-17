using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;

namespace wojilu.Web.Caching {

    public interface IActionCache {

        /// <summary>
        /// action缓存的key
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        String GetCacheKey( String actionName );

        /// <summary>
        /// 关联action。一旦关联action被操作，则缓存会失效
        /// </summary>
        /// <returns></returns>
        Dictionary<String, String> GetRelatedActions();

        /// <summary>
        /// 关联action操作之后，需要清除缓存或者重建缓存的具体操作
        /// </summary>
        /// <param name="ctx"></param>
        void UpdateCache( MvcContext ctx );


    }

}
