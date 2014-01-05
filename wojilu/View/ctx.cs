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
    /// 当前上下文对象，包括当前登录用户、当前被访问用户、当前用户ip等
    /// </summary>
    public class ctx {

        /// <summary>
        /// 将数据放到 ctx 中，便于不同视图之间传递数据
        /// </summary>
        /// <param name="obj"></param>
        public static void setItem( String itemName, Object itemValue ) { }

        /// <summary>
        /// 获取上下文 ctx 中暂存的数据
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static Object getItem( String item ) { return null; }

        /// <summary>
        /// 当前被访问用户
        /// </summary>
        public static IOwnerContext owner { get { return null; } }

        /// <summary>
        /// 当前访问者(可以是登录用户，也可以是游客；通过 IsLogin 判断)
        /// </summary>
        public static IViewerContext viewer { get { return null; } }

        /// <summary>
        /// 当前app
        /// </summary>
        public static IAppContext app { get { return null; } }

        /// <summary>
        /// 当前通过 GET 方式提交的查询字符串
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static String get( String item ) { return ""; }

        /// <summary>
        /// 当前通过 GET 方式提交的查询字符串(返回整数类型)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static String getInt( String item ) { return ""; }

        /// <summary>
        /// 当前通过 GET 方式提交的查询字符串(返回long类型)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static String getLong( String item ) { return ""; }

        /// <summary>
        /// 当前通过 POST 方式提交的查询字符串
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static String post( String item ) { return ""; }

        /// <summary>
        /// 当前通过 POST 方式提交的查询字符串(返回整数类型)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static String postInt( String item ) { return ""; }

        /// <summary>
        /// 当前通过 POST 方式提交的查询字符串(返回long类型)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static String postLong( String item ) { return ""; }

        /// <summary>
        /// 当前提交的 HttpMethod，比如 GET/POST/PUT/DELETE 等
        /// </summary>
        public static String method { get { return null; } }

        /// <summary>
        /// 当前访客的 IP 地址
        /// </summary>
        public static String ip { get { return null; } }

        /// <summary>
        /// 当前访客的浏览器信息
        /// </summary>
        public static String agent { get { return null; } }
    }


}
