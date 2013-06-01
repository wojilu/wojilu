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
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using wojilu.Web.Context;

namespace wojilu.Web.Mvc.Utils {

    internal class PostValueProcessor {

        public static String ProcessPostValue( String result, MvcContext ctx ) {
            if (result == null) return null;
            if (ctx.HttpMethod.Equals( "POST" )) {
                result = showErrorInfo( result, ctx );
            }
            return result;
        }

        private static String setPostValue( String result, MvcContext ctx ) {
            NameValueCollection post = ctx.web.postValueAll();
            for (int i = 0; i < post.Count; i++) {
                String pattern = "(<input.+?name\\s*=\\s*\"" + post.GetKey( i ) + "\".+?type=\"(text|password)\".*?)(value=\".*?\")*.*?>";
                String replacement = "$1 value=\"" + post[i] + "\">";
                if (Regex.IsMatch( result, pattern )) {
                    result = Regex.Replace( result, pattern, replacement, RegexOptions.IgnoreCase );
                }
                else {
                    String toReplace = "#{" + post[i] + "}";
                    String pattern2 = "(<textarea.+?name\\s*=\\s*\"" + post.GetKey( i ) + "\".*?>)(.*?)</textarea>";
                    String replacement2 = "$1" + toReplace + "</textarea>";
                    result = Regex.Replace( result, pattern2, replacement2, RegexOptions.IgnoreCase );
                    result = result.Replace( toReplace, post[i] );
                }
            }
            return result;
        }

        private static String showErrorInfo( String result, MvcContext ctx ) {
            if (ctx.errors.HasErrors && ctx.errors.AutoShow) {
                result = Regex.Replace( result, "(<form.+?>)", "$1 " + ctx.errors.ErrorsHtml, RegexOptions.IgnoreCase );
            }
            return result;
        }

    }
}
