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
    /// 生成链接的工具
    /// </summary>
    public class link {

        /// <summary>
        /// 生成到某个action的链接
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static String to( aAction action ) { return ""; }

        /// <summary>
        /// 生成到某个action的链接
        /// </summary>
        /// <param name="action"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static String to( aActionWithId action, long id ) { return ""; }

        /// <summary>
        /// 生成到某个用户的链接
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static String user( Object name ) { return ""; }

        /// <summary>
        /// 生成到某个app的链接(此app必须实现IApp接口)
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static String app( Object app ) { return ""; }

        /// <summary>
        /// 生成到某条数据的链接(此数据必须实现IAppData接口)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static String data( Object data ) { return ""; }

    }

}
