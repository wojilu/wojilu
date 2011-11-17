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
using System.Collections;
using System.Web;

namespace wojilu.Web.Mock {

    /// <summary>
    /// HttpContext 的模拟
    /// </summary>
    public class MvcHttpContext {

        public MvcHttpContext() {
            this.Session = new MvcSession();
        }

        public MvcSession Session { get; set; }
        public MvcResponse Response { get; set; }
        public MvcRequest Request { get; set; }
        public IDictionary Items { get; set; }
        public static MvcHttpContext Current { get; set; }
        public HttpServerUtility Server { get; set; }
    }

}
