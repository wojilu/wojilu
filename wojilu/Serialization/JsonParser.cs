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

namespace wojilu.Serialization {

    /// <summary>
    /// json 反序列化工具
    /// </summary>
    public class JsonParser {

        /// <summary>
        /// 解析字符串，返回对象。
        /// 根据 json 的不同，可能返回整数(int)、布尔类型(bool)、字符串(string)、一般对象(Dictionary&lt;string, object&gt;)、数组(List&lt;object&gt;)等不同类型
        /// </summary>
        /// <param name="src"></param>
        /// <returns>根据 json 的不同，可能返回整数(int)、布尔类型(bool)、字符串(string)、一般对象(Dictionary&lt;string, object&gt;)、数组(List&lt;object&gt;)等不同类型</returns>
        public static Object Parse( String src ) {

            if (strUtil.IsNullOrEmpty( src )) return null;

            return new InitJsonParser( new CharSource(src)  ).getResult();
        }

    }


}
