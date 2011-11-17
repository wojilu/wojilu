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
using System.Web;

namespace wojilu.Web.Mock {

    // TODO
    /// <summary>
    /// cookie 的模拟
    /// </summary>
    public class MvcCookies {

        public void Add( HttpCookie cookie ) { }

        public HttpCookie this[int index] { get { return new HttpCookie( "key", "val" ); } }
        public HttpCookie this[string name] { get { return new HttpCookie( "key", "val" ); } }

        public HttpCookie Get( int index ) { return new HttpCookie( "key", "val" ); }
        public HttpCookie Get( string name ) { return new HttpCookie( "key", "val" ); }
    }

}
