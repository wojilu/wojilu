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

namespace wojilu {

    /// <summary>
    /// 封装了几个常用的正则表达式
    /// </summary>
    public class RegPattern {

        //public static readonly String Email = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

        /// <summary>
        /// 网址的正则表达式
        /// </summary>
        //public static readonly String Url = @"^http\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(/\S*)?$";
        public static readonly string Url = @"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$";

        // http://msdn.microsoft.com/en-us/library/ms998267.aspx

        /// <summary>
        /// 允许的 email 长度
        /// </summary>
        public static readonly int EmailLength = 50;

        /// <summary>
        /// email 正则表达式
        /// </summary>
        public static readonly String Email = @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$";

        //public static readonly String Url = @"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$";

        /// <summary>
        /// 货币值(小数)的正则表达式
        /// </summary>
        public static readonly String Currency = @"^\d+(\.\d\d)?$"; //1.00

        /// <summary>
        /// (负数)货币值(小数)的正则表达式
        /// </summary>
        public static readonly String NegativeCurrency = @"^(-)?\d+(\.\d\d)?$"; //-1.20

        /// <summary>
        /// html 页面中图片的正则表达式，获取&lt;img src="" /&gt; 的src部分
        /// </summary>
        public static readonly String Img = "(?<=<img.+?src\\s*?=\\s*?\"?)([^\\s\"]+?)(?=[\\s\"])";

        /// <summary>
        /// 检查 input 字符串是否和指定的正则表达式匹配
        /// </summary>
        /// <param name="input">需要检查的字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <returns></returns>
        public static Boolean IsMatch( String input, String pattern ) {
            return Regex.IsMatch( input, pattern );
        }

        /// <summary>
        /// 替换html中的标签
        /// </summary>
        /// <param name="input"></param>
        /// <param name="tag">需要过滤的标签</param>
        /// <param name="stripConent">是否过滤标签的内容</param>
        /// <returns></returns>
        public static String ReplaceHtml( String input, String tag, Boolean stripConent ) {

            // self closing tag
            if (tag == "img" || tag == "br" || tag == "hr") {
                Regex r = new Regex( "<" + tag + @"\s*.*?>", RegexOptions.IgnoreCase );
                return r.Replace( input, "" );
            }

            // 过滤标签，以及标签内部的内容
            if (stripConent) {
                Regex r = new Regex( "<" + tag + @"[\s\S]+</" + tag + " *>", RegexOptions.IgnoreCase );
                return r.Replace( input, "" );
            }

            // 只过滤标签，不过滤标签的内容
            Regex rx = new Regex( "</?" + tag + "[^<]*?>", RegexOptions.IgnoreCase );
            return rx.Replace( input, "" );
        }

    }

}

