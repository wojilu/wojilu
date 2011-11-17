using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;
using wojilu.Caching;

namespace wojilu.Web.Mvc {

    internal class CacheInfo {

        private static readonly ILog logger = LogManager.GetLogger( typeof( CacheInfo ) );

        public String CacheKey {
            get { return _cacheKey; }
        }
        public Boolean IsActionCache {
            get { return _isActionCache; }
        }

        private String _actionName;
        private String _cacheKey;
        private Boolean _isActionCache = false;


        private void initCacheKey( IActionCache actionCache, MvcContext ctx ) {
            if (actionCache == null) return ;
            if (ctx.HttpMethod.Equals( "GET" ) == false) return ;

            _cacheKey = actionCache.GetCacheKey( ctx, _actionName );
            if (strUtil.HasText( _cacheKey )) {
                _isActionCache = true;
            }
        }

        public Object CheckCache() {
            if (this.IsActionCache == false) return null;
            return CacheManager.GetApplicationCache().Get( this.CacheKey );
        }

        public void AddContentToCache( String actionResult ) {
            CacheManager.GetApplicationCache().Put( this.CacheKey, actionResult );
            logger.Info( "add cache=" + this.CacheKey );
        }


        //----------------------------------------------------------------------

        public static CacheInfo Init( MvcContext ctx ) {
            return InitLayout( ctx, ctx.controller, false );
        }

        public static CacheInfo InitLayout( MvcContext ctx ) {
            return InitLayout( ctx, ctx.controller, true );
        }

        public static CacheInfo InitLayout( MvcContext ctx, ControllerBase controller ) {
            return InitLayout( ctx, controller, true );
        }

        private static CacheInfo InitLayout( MvcContext ctx, ControllerBase controller, Boolean isLayout ) {

            CacheInfo ci = new CacheInfo();

            if (MvcConfig.Instance.IsActionCache == false) return ci;

            // 模拟的context环境下，不缓存
            if (ctx.web.GetType() != typeof( WebContext )) return ci;

            ci._actionName = isLayout ? "Layout" : ctx.route.action;

            IActionCache actionCache = ControllerMeta.GetActionCache( controller.GetType(), ci._actionName );
            ci.initCacheKey( actionCache, ctx );
 

            return ci;

        }



    }

}
