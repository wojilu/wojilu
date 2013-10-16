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
    /// 在模板中嵌入动态 html 代码
    /// </summary>
    public class html {

        /// <summary>
        /// 在当前代码处，显示 html
        /// </summary>
        /// <param name="str"></param>
        public static void show( String str ) { }

        /// <summary>
        /// 在当前代码处，显示 html ，并加上换行符(\r\n)
        /// </summary>
        /// <param name="str"></param>
        public static void showLine( String str ) { }

        /// <summary>
        /// 对要在浏览器中显示的字符串进行编码
        /// </summary>
        /// <param name="str"></param>
        public static void encode( String str ) { }
    }


}
