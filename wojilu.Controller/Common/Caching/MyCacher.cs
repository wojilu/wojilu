using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Common {

    public class MyCacher {

        private static CacheType getCacheType() {
            return CacheType.None;
        }


        private ICacheHelper cacher;

        public MyCacher() {

            if (getCacheType() == CacheType.Disk) {
                this.cacher = new DiskCache();
            }
            else if (getCacheType() == CacheType.InMemory) {
                this.cacher = new InMemoryCache();
            }
            else if (getCacheType() == CacheType.Memechaed) {
                this.cacher = new MemcachedCache();
            }
            else
                this.cacher = new NonCache();

        }

        public Boolean shouldCache( MvcContext ctx ) {

            if (isAllowedApp( ctx ) == false) return false;

            if (getCacheType() == CacheType.None) return false;

            if (!ctx.IsGetMethod) return false; // 所有 post/put/delete 操作都不缓存
            if (ctx.web.getHas( "rd" )) return false; // 有随机戳的不缓存

            String url = ctx.url.PathAndQuery;
            if (url.IndexOf( "/Edit" ) > 0) return false; // 编辑页面不缓存

            return true;
        }

        private bool isAllowedApp( MvcContext ctx ) {

            if (ctx.url.Path.IndexOf( "/Admin" ) >= 0) return false; // 管理界面不缓存

            if (ctx.url.Path.ToLower().StartsWith( "/default" )) return true; // 开启首页缓存
            if (ctx.url.Path.StartsWith( "/Forum" )) return true; // 开启论坛缓存
            if (ctx.url.Path.StartsWith( "/Content" )) return true; // 开启cms缓存

            return false;
        }

        public string ReadCache( string urlAndPath, MvcContext ctx ) {

            String key = getCacheKey( urlAndPath );

            // 检查论坛缓存是否过期
            if (ExpirationChecker.IsExpried( key, cacher )) return null;

            return this.cacher.ReadCache( key );
        }

        public void AddCache( string urlAndPath, string val ) {
            String key = getCacheKey( urlAndPath );

            this.cacher.AddCache( key, val );
            this.cacher.SetTimestamp( key, DateTime.Now );
        }

        public void DeleteCache( string key ) {
            this.cacher.DeleteCache( key );
        }

        public void SetTimestamp( String key, DateTime t ) {
            this.cacher.SetTimestamp( key, t );
        }

        //-----------------------------------------------------------------------------


        //--------------------------------------------------------------------------------------

        // 首页 default 转化为小写形式
        private static String getCacheKey( String pathAndQuery ) {
            String key = pathAndQuery;
            // 首页以小写形式缓存，因为menu中保存的也是小写形式
            String pkey = strUtil.TrimEnd( pathAndQuery.TrimStart( '/' ), MvcConfig.Instance.UrlExt );
            if (strUtil.EqualsIgnoreCase( "default", pkey )) {
                key = key.Replace( pkey, "default" );
            }
            return key;
        }

    }

}
