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
using System.Text.RegularExpressions;
using wojilu.Web.Utils.Tags;

namespace wojilu.Web.Utils {

    /// <summary>
    /// html 过滤器：根据白名单，过滤掉不安全的字符
    /// </summary>
    public class HtmlFilter {

        /// <summary>
        /// 根据白名单，过滤掉不安全的字符
        /// </summary>
        /// <param name="srcString"></param>
        /// <returns></returns>
        public static String Filter( String srcString ) {

            if (srcString == null) return null;

            String s = TagFilter.Clear( srcString );

            return s;
        }

        /// <summary>
        /// 只有允许的标签才能出现
        /// </summary>
        /// <param name="srcString"></param>
        /// <param name="allowedTags">用英文逗号或斜杠分隔;不区分大小写</param>
        /// <returns></returns>
        public static String Filter( String srcString, String allowedTags ) {

            if (srcString == null) return null;

            String s = TagFilter.Clear( srcString, allowedTags );
            return s;
        }

        /// <summary>
        /// 只有允许的标签才能出现
        /// </summary>
        /// <param name="srcString"></param>
        /// <param name="allowedTags">允许的tag，包括属性列表</param>
        /// <returns></returns>
        public static String Filter( String srcString, Dictionary<String, String> allowedTags ) {

            if (srcString == null) return null;

            String s = TagFilter.Clear( srcString, allowedTags, false );

            return s;
        }

        /// <summary>
        /// 在默认白名单的基础上，增加允许的tag
        /// </summary>
        /// <param name="srcString"></param>
        /// <param name="allowedTags"></param>
        /// <returns></returns>
        public static String FilterAppendTags( String srcString, String allowedTags ) {
            
            if (srcString == null) return null;

            String s = TagFilter.ClearWithAllowedTags( srcString, allowedTags );

            return s;
        }

    }
}
