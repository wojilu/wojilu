///*
// * Copyright 2010 www.wojilu.com
// * 
// * Licensed under the Apache License, Version 2.0 (the "License");
// * you may not use this file except in compliance with the License.
// * You may obtain a copy of the License at
// * 
// *      http://www.apache.org/licenses/LICENSE-2.0
// * 
// * Unless required by applicable law or agreed to in writing, software
// * distributed under the License is distributed on an "AS IS" BASIS,
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// * See the License for the specific language governing permissions and
// * limitations under the License.
// */

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Text;

//using wojilu.Data;
//using wojilu.Web.Mvc.Interface;
//using wojilu.Web.Context;
//using wojilu.Caching;

//namespace wojilu.Web.Mvc.Attr {



//    /// <summary>
//    /// 网页缓存批注
//    /// </summary>
//    /// <remarks>
//    /// 1) 本批注不能直接用在action中，必须通过 wojilu.Web.Mvc.CacheUrl.config 进行配置才能使用
//    /// 2) 本批注只能用在被请求的主action方法，layout方法，load的方法都无法使用
//    /// </remarks>
//    [Serializable, AttributeUsage( AttributeTargets.Interface )]
//    public class CacheUrlAttribute : Attribute, IActionFilter {

//        private static readonly ILog logger = LogManager.GetLogger( typeof( CacheUrlAttribute ) );

//        private int _cacheSeconds;

//        public int Second {
//            set { _cacheSeconds = value; }
//            get { return _cacheSeconds; }
//        }

//        public int Minute {
//            set { _cacheSeconds = value * 60; }
//            get { return _cacheSeconds / 60; }
//        }


//        public void BeforeAction( ControllerBase controller ) {

//            if (enableCache( controller.ctx ) == false) return;

//            String key = getCacheKey( controller.ctx );
//            String actionContent = getFromCache( key );
//            if (strUtil.HasText( actionContent )) {
//                controller.actionContent( actionContent );
//                controller.utils.IsRunAction( false );
//                logger.Info( "getActionFromCache=>" + key );
//            }
//        }

//        public void AfterAction( ControllerBase controller ) {

//            if (enableCache( controller.ctx ) == false) return;

//            Boolean shouldAddCache = controller.utils.IsRunAction();
//            if (shouldAddCache) {
//                String actionContent = controller.utils.getActionResult();
//                String key = getCacheKey( controller.ctx );
//                int cacheTime = getCacheTime( controller.ctx );

//                addCache( key, actionContent, getCacheTime( controller.ctx ) );
//            }
//        }

//        //--------------------------------------------------------------------

//        // 如果配置了时间，则使用配置；如果没有，则 _cacheSeconds 就是默认缓存时间
//        private int getCacheTime( MvcContext ctx ) {

//            CacheUrl setting = getUrlCached( ctx );
//            return setting == null ? _cacheSeconds : setting.CacheSeconds;
//        }

//        private Boolean enableCache( MvcContext ctx ) {
//            CacheUrl setting = getUrlCached( ctx );
//            if (setting != null && setting.CacheSeconds > 0) return true;
//            return this.Second > 0;
//        }

//        // 根据网址和controller判断是否有配置
//        private CacheUrl getUrlCached( MvcContext ctx ) {

//            List<CacheUrl> list = cdb.findByName<CacheUrl>( ctx.url.PathAndQueryWithouApp );
//            if (list.Count == 0) return null;
//            return list[0];
//        }

//        private String getFromCache( String key ) {
//            Object result = SysCache.Get( key );
//            return result == null ? "" : result.ToString();
//        }

//        private static void addCache( String key, String actionContent, int _cacheSeconds ) {
//            SysCache.Put( key, actionContent, _cacheSeconds );
//        }

//        // 应用根目录之后的路径（不包括虚拟目录）
//        private String getCacheKey( MvcContext ctx ) {
//            return ctx.url.PathAndQueryWithouApp;

//        }

//        public int Order { get; set; }


//    }

//}
