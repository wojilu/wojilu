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
using System.IO;

namespace wojilu.Web.Mock {

    /// <summary>
    /// response 的模拟
    /// </summary>
    public class MvcResponse {

        public MvcResponse() {
            this.Cookies = new MvcCookies();
        }

        public TextWriter Writer { get; set; }
        public void End() { }
        public MvcCookies Cookies { get; set; }
        public void Clear() { }
        public void Redirect( string url ) { }
        public void Redirect( string url, bool endResponse ) { }
        public void Write( string content ) { this.Writer.Write( content ); }
        public void Flush() {}

        public string Status { get; set; }
        public string ContentType { get; set; }
        public string Charset { get; set; }


    }

}
