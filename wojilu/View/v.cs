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
    /// 模板视图的核心方法
    /// </summary>
    public class v {

        /// <summary>
        /// 获取视图对象
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static Object data( String item ) { return null; }

        /// <summary>
        /// 在当前模板中，显示到当前代码处为止；当前模板的剩余部分不再显示
        /// </summary>
        public static void end() { }

        /// <summary>
        /// 加载其他 action 内容，到当前html代码处
        /// </summary>
        /// <param name="action"></param>
        public static void load( aAction action ) { }

        /// <summary>
        /// 加载其他 action 内容，到当前html代码处
        /// </summary>
        /// <param name="action"></param>
        /// <param name="id"></param>
        public static void load( aActionWithId action, int id ) { }

        /// <summary>
        /// 加载其他 action 内容，到当前html代码处
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        public static void load( String controller, String action ) { }

        /// <summary>
        /// 加载其他 action 内容，到当前html代码处
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <param name="id"></param>
        public static void load( String controller, String action, int id ) { }

    }


}
