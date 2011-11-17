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
using System.Text;
using System.Text.RegularExpressions;
using wojilu.Web.Utils.Tags;

namespace wojilu.Web.Utils {

    /// <summary>
    /// html 过滤器：根据白名单，过滤掉不安全的字符
    /// </summary>
    public class HtmlFilter {

        private static readonly Regex scriptReg = new Regex( @"<script[\s\S]+</script *>", RegexOptions.IgnoreCase );
        private static readonly Regex iframeReg = new Regex( @"<iframe[\s\S]+</iframe *>", RegexOptions.IgnoreCase );
        private static readonly Regex framesetReg = new Regex( @"<frameset[\s\S]+</frameset *>", RegexOptions.IgnoreCase );
        
        /// <summary>
        /// 默认是不允许 script/iframe/frameset 标签的
        /// </summary>
        /// <param name="srcString"></param>
        /// <returns></returns>
        public static String Filter( String srcString ) {

            if (srcString == null) return null;

            String s = scriptReg.Replace( srcString, "" );
            s = iframeReg.Replace( s, "" );
            s = framesetReg.Replace( s, "" );

            s = TagFilter.Clear( s );

            return s;
        }

        /// <summary>
        /// 前提：在不允许 script/iframe/frameset 标签的基础上，只有允许的标签才能出现
        /// </summary>
        /// <param name="srcString"></param>
        /// <param name="allowedTags">用英文逗号或斜杠分隔;不区分大小写</param>
        /// <returns></returns>
        public static String Filter( String srcString, String allowedTags ) {

            if (srcString == null) return null;

            String s = scriptReg.Replace( srcString, "" );
            s = iframeReg.Replace( s, "" );
            s = framesetReg.Replace( s, "" );

            s = TagFilter.Clear( s, allowedTags );

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

            String s = scriptReg.Replace( srcString, "" );
            s = iframeReg.Replace( s, "" );
            s = framesetReg.Replace( s, "" );

            s = TagFilter.ClearWithAllowedTags( s, allowedTags );

            return s;
        }

    }
}
