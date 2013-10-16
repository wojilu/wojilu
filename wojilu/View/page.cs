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
    /// 对整体页面的操作
    /// </summary>
    public class page {

        /// <summary>
        /// 页面跳转
        /// </summary>
        /// <param name="url"></param>
        public static void go( String url ) { }

        /// <summary>
        /// 添加页面的 header 值
        /// </summary>
        /// <param name="headerName"></param>
        /// <param name="headerValue"></param>
        public static void header( String headerName, String headerValue ) { }

        /// <summary>
        /// 设置页面的 status ，比如 "404 Not Found"
        /// </summary>
        /// <param name="statusStr"></param>
        public static void status( String statusStr ) { }

        /// <summary>
        /// 设置页面的 title。在其他Layout中，通过 #{pageTitle} 获取
        /// </summary>
        /// <param name="titleMsg"></param>
        public static void title( String titleMsg ) { }

        /// <summary>
        /// 设置页面的 description。在其他Layout中，通过 #{pageDescription} 获取
        /// </summary>
        /// <param name="descriptionMsg"></param>
        public static void description( String descriptionMsg ) { }

        /// <summary>
        /// 设置页面的 keywords。在其他Layout中，通过 #{pageKeywords} 获取
        /// </summary>
        /// <param name="keywordsMsg"></param>
        public static void keywords( String keywordsMsg ) { }

        /// <summary>
        /// 整个页面退出，只显示当前 msg 信息(根据 msg 提示模板显示)
        /// </summary>
        /// <param name="msg"></param>
        public static void exit( String msg ) { }

        /// <summary>
        /// 整个页面退出，只显示纯文本提示
        /// </summary>
        /// <param name="msg"></param>
        public static void exitText( String msg ) { }

        /// <summary>
        /// 不再显示html内容，整个页面只显示当前的 json 字符串
        /// </summary>
        /// <param name="jsonStr"></param>
        public static void json( Object jsonStr ) { }

    }


}
