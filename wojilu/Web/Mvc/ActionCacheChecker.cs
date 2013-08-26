/*
 * Copyright 2010 www.wojilu.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;
using wojilu.Caching;

namespace wojilu.Web.Mvc {

    internal class ActionCacheChecker {

        private static readonly ILog logger = LogManager.GetLogger( typeof( ActionCacheChecker ) );

        public String CacheKey {
            get { return _cacheKey; }
        }
        public Boolean IsActionCache {
            get { return _isActionCache; }
        }

        private String _actionName;
        private String _cacheKey;
        private Boolean _isActionCache = false;


        public Object GetCache() {
            if (this.IsActionCache == false) return null;
            return CacheManager.GetApplicationCache().Get( this.CacheKey );
        }

        public void AddCache( String actionResult ) {
            CacheManager.GetApplicationCache().Put( this.CacheKey, actionResult );
            logger.Info( "add cache=" + this.CacheKey );
        }


        //----------------------------------------------------------------------

        public static ActionCacheChecker InitAction( MvcContext ctx ) {
            return initPrivate( ctx, ctx.controller, false );
        }

        public static ActionCacheChecker InitLayout( MvcContext ctx ) {
            return initPrivate( ctx, ctx.controller, true );
        }

        public static ActionCacheChecker InitLayout( MvcContext ctx, ControllerBase controller ) {
            return initPrivate( ctx, controller, true );
        }

        private static ActionCacheChecker initPrivate( MvcContext ctx, ControllerBase controller, Boolean isLayout ) {

            ActionCacheChecker x = new ActionCacheChecker();

            if (MvcConfig.Instance.IsActionCache == false) return x;

            // 模拟的context环境下，不缓存
            if (ctx.web.GetType() != typeof( WebContext )) return x;

            x._actionName = isLayout ? "Layout" : ctx.route.action;

            ActionCache actionCache = ControllerMeta.GetActionCacheAttr( controller.GetType(), x._actionName );
            x.initCacheKey( actionCache, ctx );
 

            return x;

        }

        //----------------------------------------------------------------------


        private void initCacheKey( ActionCache actionCache, MvcContext ctx ) {
            if (actionCache == null) return;

            // 只在 GET 下缓存
            if (ctx.HttpMethod.Equals( "GET" ) == false) return;

            _cacheKey = actionCache.GetCacheKey( ctx, _actionName );
            if (strUtil.HasText( _cacheKey )) {
                _isActionCache = true;
            }
        }

    }

}
