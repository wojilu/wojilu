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
//using System.IO;
//using System.Web;
//using wojilu;
//using wojilu.IO;
//using wojilu.Net;
//using wojilu.Web.Mvc;

//namespace wojilu.Web.Caching {

//    public class FilePageCacher : PageCacher {
//        private static void checkCachePathExists() {
//            if (!Directory.Exists( cachePath )) {
//                Directory.CreateDirectory( cachePath );
//            }
//        }

//        private static String getFileName( String fullUrl ) {
//            String str = fullUrl.Replace( "http://", "" ).Replace( "/", "_slash_" ).Replace( ".", "_dot_" ).Replace( "?", "_query_" ) + ".html";
//            return Path.Combine( cachePath, str );
//        }

//        private static String getFromCache( String fullUrl ) {
//            return wojilu.IO.File.Read( getFileName( fullUrl ) );
//        }

//        private static String getFromUrl( String fullUrl ) {
//            String fileContent = PageLoader.Download( fullUrl, "NoCache", "utf-8" );
//            checkCachePathExists();
//            wojilu.IO.File.Write( getFileName( fullUrl ), fileContent );
//            return fileContent;
//        }

//        public override String GetPage( String fullUrl ) {
//            if (isFromCache( fullUrl )) {
//                return getFromCache( fullUrl );
//            }
//            return getFromUrl( fullUrl );
//        }

//        private static Boolean isFromCache( String fullUrl ) {
//            String path = getFileName( fullUrl );
//            if (System.IO.File.Exists( path )) {
//                DateTime lastWriteTime = System.IO.File.GetLastWriteTime( path );
//                if (DateTime.Now.Subtract( lastWriteTime ).Minutes < 30) {
//                    return true;
//                }
//            }
//            return false;
//        }

//        private static String cachePath {
//            get {
//                String path = strUtil.Join( SystemInfo.ApplicationPath, "cache/" );
//                return PathHelper.Map( path );
//            }
//        }
//    }
//}

