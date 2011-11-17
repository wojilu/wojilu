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
//using wojilu.Net;

//namespace wojilu.Web.Caching {

//    public class MemoryPageCacher : PageCacher {
//        private static void addToCache( String pageName, String newPageContent ) {
//            CachePage page = new CachePage();
//            page.Name = pageName;
//            page.Body = newPageContent;
//            page.insert();
//        }

//        private static String getFromCache( IList result ) {
//            CachePage page = result[0] as CachePage;
//            if (page == null) {
//                throw new Exception( "cache object's type must be CachePage" );
//            }
//            return page.Body;
//        }

//        private static String getFromUrl( String pageName ) {
//            String newPageContent = PageLoader.Download( pageName, "NoCache", "utf-8" );
//            addToCache( pageName, newPageContent );
//            return newPageContent;
//        }

//        public override String GetPage( String pageName ) {
//            IList result = new CachePage().findByName( pageName );
//            if (result.Count > 0) {
//                return getFromCache( result );
//            }
//            return getFromUrl( pageName );
//        }
//    }
//}

