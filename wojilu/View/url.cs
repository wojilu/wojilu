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
using wojilu.Members.Interface;
using wojilu.Web.Context;

namespace wojilu.View {

    /// <summary>
    /// 当前网址信息
    /// </summary>
    public class url {

        /// <summary>
        /// 当前网址的完整信息，比如 http://www.abc.com:5112/main/xtest.aspx?name=zhang
        /// </summary>
        public static String text { get { return null; } }

        /// <summary>
        /// 当前网址中的路径信息，比如 /main/xtest.aspx
        /// </summary>
        public static String path { get { return null; } }

        /// <summary>
        /// 当前网址中问号后面的查询信息(包括问号)，比如 ?name=zhang
        /// </summary>
        public static String query { get { return null; } }

        /// <summary>
        /// 当前页面来源
        /// </summary>
        public static String from { get { return null; } }
    }


}
