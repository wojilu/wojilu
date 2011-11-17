using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;
using wojilu.Common.Menus.Interface;
using wojilu.Caching;
using wojilu.Members.Interface;

namespace wojilu.Web.Mvc {

    public class PageCache : IPageCache {

        private static readonly ILog logger = LogManager.GetLogger( typeof( PageCache ) );

        protected List<Type> list = new List<Type>();

        public virtual Boolean IsCache( MvcContext ctx ) {
            return true;
        }

        public virtual List<Type> GetRelatedActions() {

            if (list.Count == 0) {
                this.ObserveActionCaches();
            }
            return this.list;
        }

        public virtual void ObserveActionCaches() {

        }

        protected void observe( Type t ) {
            if (list.Contains( t ) == false) list.Add( t );
        }

        public virtual void UpdateCache( MvcContext ctx ) {
        }

        public virtual void AfterCachePage( MvcContext ctx ) {
        }

        /// <summary>
        /// 除了原始网址，还要更新相关的友好网址( friendly url )
        /// </summary>
        /// <param name="url"></param>
        /// <param name="ctx"></param>
        protected virtual void updateAllUrl( String url, MvcContext ctx ) {
            updateAllUrl( url, ctx, ctx.owner.obj );
        }

        /// <summary>
        /// 除了原始网址，还要更新相关的友好网址( friendly url )
        /// </summary>
        /// <param name="url"></param>
        /// <param name="ctx"></param>
        protected virtual void updateAllUrl( String url, MvcContext ctx, IMember owner ) {

            CacheManager.GetApplicationCache().Remove( url );
            logger.Info( "remove page cache, key=" + url );

            List<String> friendUrls = getFriendlyUrls( ctx, owner, url );
            foreach (String furl in friendUrls) {
                CacheManager.GetApplicationCache().Remove( furl );
                logger.Info( "remove page cache, key=" + furl );
            }
        }

        protected virtual List<String> getFriendlyUrls( MvcContext ctx, IMember owner, String rawUrl ) {

            List<String> furls = new List<String>();

            List<IMenu> list = InitHelperFactory.GetHelper( owner.GetType(), ctx ).GetMenus( owner );

            foreach (IMenu menu in list) {

                if (getFullUrl( menu.RawUrl ).Equals( rawUrl )) {

                    String urlKey = "/" + menu.Url + MvcConfig.Instance.UrlExt;
                    furls.Add( urlKey );

                    if (menu.Url == "default") { // 在静态页面的时候，有多种结果：/和/Default.aspx
                        furls.Add( "/" );
                        furls.Add( "/Default" + MvcConfig.Instance.UrlExt );
                    }

                }

            }

            return furls;
        }

        private String getFullUrl( string rawUrl ) {
            return "/" + rawUrl + MvcConfig.Instance.UrlExt;
        }

    }

}
