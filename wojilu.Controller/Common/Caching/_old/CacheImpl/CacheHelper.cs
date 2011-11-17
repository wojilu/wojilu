//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.IO;
//using wojilu.Caching;
//using wojilu.DI;
//using wojilu.Web.Mvc;
//using wojilu.Web.Context;
//using wojilu.Web.Controller.Common;

//namespace wojilu.Web.Controller {




//    public class CacheHelper {

//        private IApplicationCache c {
//            get {
//                return ObjectContext.GetByType( "wojilu.Caching.MemcachedCache" ) as IApplicationCache;
//            }
//        }

//        private static CacheType getCacheType() {
//            return CacheType.InMemory;
//        }

//        public Boolean shouldCache( MvcContext ctx ) {

//            if (getCacheType() == CacheType.None) return false;

//            if (!ctx.IsGetMethod) return false; // 所有 post/put/delete 操作都不缓存
//            if (ctx.web.getHas( "rd" )) return false; // 有随机戳的不缓存

//            String url = ctx.url.PathAndQuery;
//            if (url.IndexOf( "/Edit" ) > 0) return false; // 编辑页面不缓存

//            return true;
//        }

//        //---------------------------------------------------------------------------------------------------------------------

//        public void setTimestamp( String key, DateTime t ) {

//            if (getCacheType() == CacheType.Disk) {
//                String absFile = getAbsTimestampPath( key );
//                checkCacheDir();
//                file.Write( absFile, t.ToString() );
//            }

//            if (getCacheType() == CacheType.InMemory) {
//                new ApplicationCache().Put( key, t );
//            }

//            if (getCacheType() == CacheType.Memechaed) {
//                c.Put( key, t );
//            }
//        }

//        public DateTime getTimestamp( String key ) {

//            if (getCacheType() == CacheType.Disk) {
//                String absFile = getAbsTimestampPath( key );
//                if (file.Exists( absFile ) == false) return DateTime.MinValue;
//                String str = file.Read( absFile );
//                if (strUtil.IsNullOrEmpty( str )) return DateTime.MinValue;
//                return cvt.ToTime( str, DateTime.MinValue );
//            }

//            if (getCacheType() == CacheType.InMemory) {
//                Object obj = new ApplicationCache().Get( key );
//                if (obj == null) return DateTime.MinValue;
//                return (DateTime)obj;
//            }

//            if (getCacheType() == CacheType.Memechaed) {
//                Object obj = c.Get( key );
//                if (obj == null) return DateTime.MinValue;
//                return (DateTime)obj;
//            }

//            return DateTime.MinValue;
//        }

//        //---------------------------------------------------------------------------------------------------------------------

//        public String readCache( String urlAndPath ) {

//            String key = getCacheKey( urlAndPath );

//            // 检查缓存是否过期
//            if (isForumBoard( key ) && boardExpired( key )) return null;

//            return readCachePrivate( key );
//        }

//        private String readCachePrivate( String key ) {
//            if (getCacheType() == CacheType.Disk) return getFromDisk( key );
//            if (getCacheType() == CacheType.InMemory) return getFromInMemory( key );
//            if (getCacheType() == CacheType.Memechaed) return getFromMemcached( key );

//            return null;
//        }

//        private Boolean isForumBoard( string key ) {
//            return key.IndexOf( "/Board/" ) > 0 && key.IndexOf( "Forum" ) > 0;
//        }

//        private Boolean boardExpired( string key ) {

//            // 版块最后更新时间
//            String fkey = "forumboard_" + 22;
//            DateTime ts = getTimestamp( fkey );
//            if (isTimeNull( ts )) return true;

//            // 缓存加入时间
//            DateTime created = getCacheCreated( "__ts_"+key );
//            if (isTimeNull( created )) return true;

//            if (ts >= created) return true;

//            return false;
//        }

//        private Boolean isTimeNull( DateTime created ) {
//            return created.Year < 1900;
//        }

//        private DateTime getCacheCreated( string key ) {
//            String tsStr = readCachePrivate( key );
//            if (strUtil.IsNullOrEmpty( tsStr )) return DateTime.MinValue;
//            return cvt.ToTime( tsStr );
//        }

//        public void addCache( MvcEventArgs e, String output ) {

//            if (shouldCache( e.ctx ) == false) return;

//            String key = getCacheKey( e.ctx.url.PathAndQuery );

//            if (getCacheType() == CacheType.Disk) addDisk( e, key, output );
//            if (getCacheType() == CacheType.InMemory) addInMemory( e, key, output );
//            if (getCacheType() == CacheType.Memechaed) addMemcached( e, key, output );
//        }


//        public void deleteCache( String url ) {

//            if (getCacheType() == CacheType.Disk) removeDiskCache( url );
//            if (getCacheType() == CacheType.InMemory) removeInMemory( url );
//            if (getCacheType() == CacheType.Memechaed) removeMemcached( url );
//        }

//        //----------------------------------------- read cache ------------------------------------------------------------



//        // 1) 测试缓存
//        private String getFromInMemory( String urlAndPath ) {
//            Object obj = new ApplicationCache().Get( urlAndPath );
//            if (obj == null) return null;
//            return obj.ToString();
//        }

//        // 2) 扩展缓存
//        private String getFromMemcached( String urlAndPath ) {
//            Object obj = c.Get( urlAndPath );
//            if (obj == null) return null;

//            return obj.ToString();
//        }

//        // 3) 磁盘缓存
//        private String getFromDisk( String urlAndPath ) {
//            String absFile = getAbsFilePath( urlAndPath );
//            if (file.Exists( absFile ) == false) return null;
//            return file.Read( absFile );
//        }

//        //--------------------------------------------------- add cache --------------------------------------------------

//        private void addInMemory( MvcEventArgs e, String key, string output ) {
//            new ApplicationCache().Put( key, output );
//        }

//        private void addMemcached( MvcEventArgs e, String key, string output ) {
//            c.Put( key, output );
//        }

//        private static void addDisk( MvcEventArgs e, String key, String output ) {

//            String absFile = getAbsFilePath( key );

//            checkCacheDir();

//            file.Write( absFile, output );
//        }

//        private static void checkCacheDir() {
//            String absDir = getAbsDir();
//            if (Directory.Exists( absDir ) == false) Directory.CreateDirectory( absDir );
//        }

//        private static String getAbsFilePath( string pathAndQuery ) {

//            String url = processQuery( pathAndQuery );

//            String path = url.TrimStart( '/' ).Replace( "/", "_" ); // Content396_Content_Index
//            path = strUtil.TrimEnd( path, MvcConfig.Instance.UrlExt ) + ".config"; //Content396_Content_Index.config
//            String absFile = Path.Combine( getAbsDir(), path );

//            return absFile;
//        }

//        private static String getAbsTimestampPath( string pathAndQuery ) {

//            String url = processQuery( pathAndQuery );

//            String path = url.TrimStart( '/' ).Replace( "/", "_" ); 
//            path = strUtil.TrimEnd( path, MvcConfig.Instance.UrlExt ) + ".config";
//            path = "__ts_" + path;
//            String absFile = Path.Combine( getAbsDir(), path );

//            return absFile;
//        }

//        // 修正查询字符串中的问号?为合法的路径字符_
//        private static string processQuery( string pathAndQuery ) {

//            int qindex = pathAndQuery.IndexOf( "?" );
//            if (qindex < 0) return pathAndQuery;

//            String[] arrItem = pathAndQuery.Split( '?' );

//            String path = strUtil.TrimEnd( arrItem[0], MvcConfig.Instance.UrlExt );

//            return path + "_" + arrItem[1];
//        }

//        private static String getAbsDir() {
//            String dir = cfgHelper.FrameworkRoot + "cache/";
//            String absDir = PathHelper.Map( dir );
//            return absDir;
//        }

//        //------------------------------------------------ delete cache -----------------------------------------------------


//        public void removeDiskCache( String url ) {

//            String absFile = getAbsFilePath( url );

//            if (file.Exists( absFile )) file.Delete( absFile );
//        }

//        private void removeInMemory( String url ) {
//            new ApplicationCache().Remove( url );
//        }

//        private void removeMemcached( String url ) {
//            c.Remove( url );
//        }

//        //-----------------------------------------------------------------------------------

//        // 首页 default 转化为小写形式
//        private static String getCacheKey( String pathAndQuery ) {
//            String key = pathAndQuery;
//            // 首页以小写形式缓存，因为menu中保存的也是小写形式
//            String pkey = strUtil.TrimEnd( pathAndQuery.TrimStart( '/' ), MvcConfig.Instance.UrlExt );
//            if (strUtil.EqualsIgnoreCase( "default", pkey )) {
//                key = key.ToLower();
//            }
//            return key;
//        }


//    }

//}
