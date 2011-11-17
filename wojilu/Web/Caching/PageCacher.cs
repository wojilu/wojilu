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

//namespace wojilu.Web.Caching {

//    public abstract class PageCacher {

//        protected PageCacher() {
//        }

//        public static PageCacher Get() {
//            return new MemoryPageCacher();
//        }

//        public static PageCacher Get( CacheMode cacheMode ) {
//            if (cacheMode == CacheMode.File) {
//                return new FilePageCacher();
//            }
//            return new MemoryPageCacher();
//        }

//        public abstract String GetPage( String pageName );
//    }
//}

