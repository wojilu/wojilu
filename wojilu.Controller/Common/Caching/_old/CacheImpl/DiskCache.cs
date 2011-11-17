using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using System.IO;

namespace wojilu.Web.Controller.Common {

    public class DiskCache : ICacheHelper {


        public string ReadCache( string key ) {

            String absFile = getAbsFilePath( key );
            if (file.Exists( absFile ) == false) return null;
            return file.Read( absFile );

        }

        public void AddCache( string key, string val ) {
            String absFile = getAbsFilePath( key );

            checkCacheDir();

            file.Write( absFile, val );
        }

        public void DeleteCache( string key ) {
            String absFile = getAbsFilePath( key );

            if (file.Exists( absFile )) file.Delete( absFile );
        }


        public void SetTimestamp( String key, DateTime t ) {

            String absFile = getAbsTimestampPath( key );
            checkCacheDir();
            file.Write( absFile, t.ToString() );

        }

        public DateTime GetTimestamp( String key ) {

            String absFile = getAbsTimestampPath( key );
            if (file.Exists( absFile ) == false) return DateTime.MinValue;
            String str = file.Read( absFile );
            if (strUtil.IsNullOrEmpty( str )) return DateTime.MinValue;
            return cvt.ToTime( str, DateTime.MinValue );


        }

        private static String getAbsTimestampPath( string pathAndQuery ) {

            String url = processQuery( pathAndQuery );

            String path = url.TrimStart( '/' ).Replace( "/", "_" );
            path = strUtil.TrimEnd( path, MvcConfig.Instance.UrlExt ) + ".config";
            path = "__ts_" + path;
            String absFile = Path.Combine( getAbsDir(), path );

            return absFile;
        }


        //-------------------------------------------

        private static void checkCacheDir() {
            String absDir = getAbsDir();
            if (Directory.Exists( absDir ) == false) Directory.CreateDirectory( absDir );
        }

        private static String getAbsFilePath( string pathAndQuery ) {

            String url = processQuery( pathAndQuery );

            String path = url.TrimStart( '/' ).Replace( "/", "_" ); // Content396_Content_Index
            path = strUtil.TrimEnd( path, MvcConfig.Instance.UrlExt ) + ".config"; //Content396_Content_Index.config
            String absFile = Path.Combine( getAbsDir(), path );

            return absFile;
        }

        // 修正查询字符串中的问号?为合法的路径字符_
        private static string processQuery( string pathAndQuery ) {

            int qindex = pathAndQuery.IndexOf( "?" );
            if (qindex < 0) return pathAndQuery;

            String[] arrItem = pathAndQuery.Split( '?' );

            String path = strUtil.TrimEnd( arrItem[0], MvcConfig.Instance.UrlExt );

            return path + "_" + arrItem[1];
        }

        private static String getAbsDir() {
            String dir = cfgHelper.FrameworkRoot + "cache/";
            String absDir = PathHelper.Map( dir );
            return absDir;
        }

    }

}
