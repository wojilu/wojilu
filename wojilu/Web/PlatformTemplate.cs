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
using wojilu.Serialization;

namespace wojilu.Web {

    /// <summary>
    /// 综合系统中 feed 消息的模板处理
    /// </summary>
    public class PlatformTemplate {

        /// <summary>
        /// 获取模板中变量的值，以 json 格式返回
        /// </summary>
        /// <param name="template"></param>
        /// <param name="jsonData">输入的 json 数据，其中有模板不需要用到的多余数据</param>
        /// <returns></returns>
        public static String GetVarData( String template, String jsonData ) {

            StringBuilder result = new StringBuilder( "{" );
            List<String> vars = GetVars( template );
            Dictionary<String, object> dic = JSON.ToDictionary( jsonData );
            foreach (String key in vars) {

                if (dic.ContainsKey( key ) == false) continue;
                result.Append( key );
                result.Append( ":" );
                result.Append( "\"" );
                result.Append( dic[key] );
                result.Append( "\", " );
            }
            return result.ToString().Trim().TrimEnd( ',' ) + "}";
        }

        /// <summary>
        /// 获取模板中出现的所有变量
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public static List<String> GetVars( String template ) {


            List<String> results = new List<String>();
            char[] chars = template.ToCharArray();

            StringBuilder tempVar = new StringBuilder();
            Boolean isVarBegin = false;
            for (int i = 0; i < chars.Length; i++) {

                if (i == 0) continue;

                if (chars[i - 1] == '{' && chars[i] == '*') {
                    tempVar.Remove( 0, tempVar.Length );
                    isVarBegin = true;
                    continue;
                }

                if (chars[i] == '*' && chars[i + 1] == '}') {
                    results.Add( tempVar.ToString() );
                    isVarBegin = false;
                    continue;
                }

                if (isVarBegin) {
                    tempVar.Append( chars[i] );
                }

            }

            return results;
        }

    }

}
