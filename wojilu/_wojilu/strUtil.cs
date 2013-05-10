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
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Configuration;
using System.Web.Security;
using System.Text;

namespace wojilu {

    /// <summary>
    /// 字符串工具类，封装了常见字符串操作
    /// </summary>
    public class strUtil {

        private static Regex htmlReg = new Regex( "<[^>]*>" );

        /// <summary>
        /// 检查字符串是否是 null 或者空白字符。不同于.net自带的string.IsNullOrEmpty，多个空格在这里也返回true。
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Boolean IsNullOrEmpty( String target ) {
            if (target != null) {
                return target.Trim().Length == 0;
            }
            return true;
        }

        /// <summary>
        /// 检查是否包含有效字符(空格等空白字符不算)
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Boolean HasText( String target ) {
            return !IsNullOrEmpty( target );
        }

        /// <summary>
        /// 比较两个字符串是否相等
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static Boolean Equals( String s1, String s2 ) {
            if (s1 == null && s2 == null) return true;
            if (s1 == null || s2 == null) return false;
            if (s2.Length != s1.Length) return false;
            return string.Compare( s1, 0, s2, 0, s2.Length ) == 0;
        }

        /// <summary>
        /// 比较两个字符串是否相等(不区分大小写)
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static Boolean EqualsIgnoreCase( String s1, String s2 ) {

            if (s1 == null && s2 == null) return true;
            if (s1 == null || s2 == null) return false;

            if (s2.Length != s1.Length) return false;

            return string.Compare( s1, 0, s2, 0, s2.Length, StringComparison.OrdinalIgnoreCase ) == 0;
        }

        /// <summary>
        /// 将 endString 附加到 srcString末尾，如果 srcString 末尾已包含 endString，则不再附加。
        /// </summary>
        /// <param name="srcString"></param>
        /// <param name="endString"></param>
        /// <returns></returns>
        public static String Append( String srcString, String endString ) {
            if (strUtil.IsNullOrEmpty( srcString )) return endString;
            if (strUtil.IsNullOrEmpty( endString )) return srcString;
            if (srcString.EndsWith( endString )) return srcString;
            return srcString + endString;
        }

        /// <summary>
        /// 将对象转为字符串，如果对象为 null，则转为空字符串(string.Empty)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String ConverToNotNull( Object str ) {
            if (str == null) return "";
            return str.ToString();
        }

        /// <summary>
        /// 从字符串中截取指定长度的一段，如果源字符串被截取了，则结果末尾出现省略号...
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="length">需要截取的长度</param>
        /// <returns></returns>
        public static String CutString( Object str, int length ) {
            return CutString( ConverToNotNull( str ), length );
        }

        /// <summary>
        /// 从字符串中截取指定长度的一段，如果源字符串被截取了，则结果末尾出现省略号...
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="length">需要截取的长度</param>
        /// <returns></returns>
        public static String CutString( String str, int length ) {
            if (str == null) return null;
            if (str.Length > length) return String.Format( "{0}...", str.Substring( 0, length ) );
            return str;
        }

        /// <summary>
        /// 对双引号进行编码
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static String EncodeQuote( String src ) {
            return src.Replace( "\"", "&quot;" );
        }

        /// <summary>
        /// 让 html 在 textarea 中正常显示。替换尖括号和字符&amp;lt;与&amp;gt;
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static String EncodeTextarea( String html ) {
            if (html == null) return null;
            return html.Replace( "&lt;", "&amp;lt;" ).Replace( "&gt;", "&amp;gt;" ).Replace( "<", "&lt;" ).Replace( ">", "&gt;" );
        }

        ///// <summary>
        ///// 对双引号进行编码，并替换掉换行符
        ///// </summary>
        ///// <param name="src"></param>
        ///// <returns></returns>
        //public static String EncodeQuoteAndClearLine( String src ) {
        //    return src.Replace( "\"", "\\\"" ).Replace( "\r\n", "" ).Replace( "\n", "" ).Replace( "\r", "" ).Replace( "\r\n", "" );
        //}

        /// <summary>
        /// 截取字符串末尾的整数
        /// </summary>
        /// <param name="rawString"></param>
        /// <returns></returns>
        public static int GetEndNumber( String rawString ) {

            if (IsNullOrEmpty( rawString )) return 0;

            char[] chArray = rawString.ToCharArray();
            int startIndex = -1;
            for (int i = chArray.Length - 1; i >= 0; i--) {
                if (!char.IsDigit( chArray[i] )) break;
                startIndex = i;
            }
            if (startIndex == -1) return 0;

            return cvt.ToInt( rawString.Substring( startIndex ) );
        }

        /// <summary>
        /// 获取 html 文档的标题内容
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public String getHtmlTitle( String html ) {
            Match match = Regex.Match( html, "<title>(.*)</title>" );
            if (match.Groups.Count == 2) return match.Groups[1].Value;
            return "(unknown)";
        }

        /// <summary>
        /// 将整数按照指定的长度转换为字符串，比如33转换为6位就是"000033"
        /// </summary>
        /// <param name="intValue"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static String GetIntString( int intValue, int length ) {
            return String.Format( "{0:D" + length + "}", intValue );
        }

        /// <summary>
        /// 得到字符串的 TitleCase 格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String GetTitleCase( String str ) {
            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase( str );
        }

        /// <summary>
        /// 得到字符串的 CamelCase 格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String GetCamelCase( String str ) {
            if (IsNullOrEmpty( str )) return str;
            return str[0].ToString().ToLower() + str.Substring( 1 );
        }

        /// <summary>
        /// 根据对象(IEntity)列表，获取所有对象的ids字符串
        /// </summary>
        /// <param name="objList">对象必须是IEntity接口</param>
        /// <returns>比如 2,5,8 等</returns>
        public static String GetIds( IList objList ) {
            if (objList == null || objList.Count == 0) return "";
            String ids = "";
            foreach (IEntity obj in objList) {
                if (obj == null || obj.Id == 0) continue;
                ids += obj.Id + ",";
            }
            return ids.TrimEnd( ',' );
        }

        /// <summary>
        /// 获取所有整数 int 的字符串
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public static String GetIds( List<int> idList ) {
            if (idList == null || idList.Count == 0) return "";
            return GetIds( idList.ToArray() );
        }

        /// <summary>
        /// 获取所有整数 int 的字符串
        /// </summary>
        /// <param name="arrIds"></param>
        /// <returns></returns>
        public static String GetIds( int[] arrIds ) {
            if (arrIds == null || arrIds.Length == 0) return "";
            String ids = "";
            foreach (int x in arrIds) {
                ids += x + ",";
            }
            return ids.TrimEnd( ',' );
        }

        /// <summary>
        /// 从类型的全名中获取类型名称(不包括命名空间)
        /// </summary>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        public static String GetTypeName( String typeFullName ) {
            String[] strArray = typeFullName.Split( new char[] { '.' } );
            return strArray[strArray.Length - 1];
        }

        /// <summary>
        /// 获取类型名称(主要针对泛型做特殊处理)。如果要获取内部元素信息，请使用t.GetGenericArguments
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static String GetTypeName( Type t ) {
            if (t.IsGenericType == false) return t.Name;
            return t.Name.Split( '`' )[0];
        }

        /// <summary>
        /// 获取类型全名(主要针对泛型做特殊处理)，比如List&lt;String&gt;返回System.Collections.Generic.List。如果要获取内部元素信息，请使用t.GetGenericArguments
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static String GetTypeFullName( Type t ) {
            if (t.IsGenericType == false) return t.FullName;
            return t.FullName.Split( '`' )[0];
        }

        /// <summary>
        /// 返回泛型的类型全名，包括元素名，比如System.Collections.Generic.List&lt;System.String&gt;
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static String GetGenericTypeWithArgs( Type t ) {

            if (t.IsGenericType == false) return t.FullName;

            //System.Collections.Generic.Dictionary`2[System.Int32,System.String]
            String[] arr = t.ToString().Split( '`' );

            String[] arrArgs = arr[1].Split( '[' );
            String args = "<" + arrArgs[1].TrimEnd( ']' ) + ">";

            return arr[0] + args;
        }

        /// <summary>
        /// 是否是英文字符和下划线
        /// </summary>
        /// <param name="rawString"></param>
        /// <returns></returns>
        public static Boolean IsLetter( String rawString ) {
            if (IsNullOrEmpty( rawString )) return false;

            char[] arrChar = rawString.ToCharArray();
            foreach (char c in arrChar) {

                if ("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_".IndexOf( c ) < 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 是否是英文、数字和下划线，但不能以下划线开头
        /// </summary>
        /// <param name="rawString"></param>
        /// <returns></returns>
        public static Boolean IsUrlItem( String rawString ) {
            if (IsNullOrEmpty( rawString )) return false;

            char[] arrChar = rawString.ToCharArray();
            if (arrChar[0] == '_') return false;

            foreach (char c in arrChar) {
                if ("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_1234567890".IndexOf( c ) < 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 是否全部都是中文字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean IsChineseLetter( String str ) {
            if (strUtil.IsNullOrEmpty( str )) return false;
            char[] arr = str.ToCharArray();
            for (int i = 0; i < arr.Length; i++) {
                if (IsChineseLetter( str, i ) == false) return false;
            }
            return true;
        }

        /// <summary>
        /// 只能以英文或中文开头，允许英文、数字、下划线和中文；
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean IsAbcNumberAndChineseLetter( String str ) {

            if (strUtil.IsNullOrEmpty( str )) return false;

            char[] arr = str.ToCharArray();
            if (isAbcAndChinese( arr[0] ) == false) return false;

            for (int i = 0; i < arr.Length; i++) {
                if ("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_1234567890".IndexOf( arr[i] ) >= 0) continue;
                if (IsChineseLetter( str, i ) == false) return false;
            }
            return true;
        }

        private static Boolean isAbcAndChinese( char c ) {
            if ("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf( c ) >= 0) return true;
            if (IsChineseLetter( c.ToString(), 0 ) == true) return true;
            return false;
        }

        private static Boolean IsChineseLetter( String input, int index ) {

            int chineseCharBegin = Convert.ToInt32( 0x4e00 );
            int chineseCharEnd = Convert.ToInt32( 0x9fff );
            int code = Char.ConvertToUtf32( input, index );
            return (code >= chineseCharBegin && code <= chineseCharEnd);
        }

        /// <summary>
        /// 是否是有效的颜色值(3位或6位，全部由英文字符或数字组成)
        /// </summary>
        /// <param name="aColor"></param>
        /// <returns></returns>
        public static Boolean IsColorValue( String aColor ) {
            if (strUtil.IsNullOrEmpty( aColor )) return false;
            String color = aColor.Trim().TrimStart( '#' ).Trim();
            if (color.Length != 3 && color.Length != 6) return false;

            char[] arr = color.ToCharArray();
            foreach (char c in arr) {
                if ("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".IndexOf( c ) < 0) return false;
            }

            return true;
        }

        /// <summary>
        /// 用斜杠/拼接两个字符串
        /// </summary>
        /// <param name="strA"></param>
        /// <param name="strB"></param>
        /// <returns></returns>
        public static String Join( String strA, String strB ) {
            return Join( strA, strB, "/" );
        }

        /// <summary>
        /// 根据制定的分隔符拼接两个字符串
        /// </summary>
        /// <param name="strA"></param>
        /// <param name="strB"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static String Join( String strA, String strB, String separator ) {
            return (Append( strA, separator ) + TrimStart( strB, separator ));
        }

        /// <summary>
        /// 剔除 html 中的 tag
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static String ParseHtml( Object html ) {
            if (html == null) return String.Empty;
            return htmlReg.Replace( html.ToString(), "" ).Replace( " ", " " );
        }

        /// <summary>
        /// 剔除 html 中的 tag，并返回指定长度的字符串
        /// </summary>
        /// <param name="html"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static String ParseHtml( Object html, int count ) {
            return CutString( ParseHtml( html ), count ).Replace( "　", "" );
        }

        /// <summary>
        /// 从 html 中截取指定长度的一段，并关闭未结束的 html 标签
        /// </summary>
        /// <param name="html"></param>
        /// <param name="count">需要截取的长度(小于20个字符按20个字符计算)</param>
        /// <returns></returns>
        public static String CutHtmlAndColse( String html, int count ) {
            if (html == null) return "";
            html = html.Trim();
            if (count <= 0) return "";
            if (count < 20) count = 20;
            String unclosedHtml = html.Length <= count ? html : html.Trim().Substring( 0, count );
            return CloseHtml( unclosedHtml );
        }

        /// <summary>
        /// 关闭未结束的 html 标签
        /// (TODO 本方法临时使用，待重写)
        /// </summary>
        /// <param name="unClosedHtml"></param>
        /// <returns></returns>
        public static String CloseHtml( String unClosedHtml ) {
            if (unClosedHtml == null) return "";
            String[] arrTags = new String[] { "strong", "b", "i", "u", "em", "font", "span", "label", "pre", "td", "th", "tr", "tbody", "table", "li", "ul", "ol", "h1", "h2", "h3", "h4", "h5", "h6", "p", "div" };

            for (int i = 0; i < arrTags.Length; i++) {

                Regex re = new Regex( "<" + arrTags[i] + "[^>]*>", RegexOptions.IgnoreCase );
                int openCount = re.Matches( unClosedHtml ).Count;
                if (openCount == 0) continue;

                re = new Regex( "</" + arrTags[i] + ">", RegexOptions.IgnoreCase );
                int closeCount = re.Matches( unClosedHtml ).Count;

                int unClosedCount = openCount - closeCount;

                for (int k = 0; k < unClosedCount; k++) {
                    unClosedHtml += "</" + arrTags[i] + ">";
                }
            }

            return unClosedHtml;
        }

        /// <summary>
        /// 将字符串分割成数组
        /// </summary>
        /// <param name="srcString"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static String[] Split( String srcString, String separator ) {
            if (srcString == null) return null;
            if (separator == null) throw new ArgumentNullException();
            return srcString.Split( new String[] { separator }, StringSplitOptions.None );
        }

        /// <summary>
        /// 过滤掉 sql 语句中的单引号，并返回指定长度的结果
        /// </summary>
        /// <param name="rawSql"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static String SqlClean( String rawSql, int number ) {
            if (IsNullOrEmpty( rawSql )) return rawSql;
            return SubString( rawSql, number ).Replace( "'", "''" );
        }

        /// <summary>
        /// 从字符串中截取指定长度的一段，结果末尾没有省略号
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static String SubString( String str, int length ) {
            if (str == null) return null;
            if (str.Length > length) return str.Substring( 0, length );
            return str;
        }

        /// <summary>
        /// 将纯文本中的换行符转换成html中换行符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String Text2Html( String str ) {
            return str.Replace( "\n", "<br/>" );
        }

        /// <summary>
        /// 从 srcString 的末尾剔除掉 trimString
        /// </summary>
        /// <param name="srcString"></param>
        /// <param name="trimString"></param>
        /// <returns></returns>
        public static String TrimEnd( String srcString, String trimString ) {
            if (strUtil.IsNullOrEmpty( trimString )) return srcString;
            if (srcString.EndsWith( trimString ) == false) return srcString;
            if (srcString.Equals( trimString )) return "";
            return srcString.Substring( 0, srcString.Length - trimString.Length );
        }

        /// <summary>
        /// 从 srcString 的开头剔除掉 trimString
        /// </summary>
        /// <param name="srcString"></param>
        /// <param name="trimString"></param>
        /// <returns></returns>
        public static String TrimStart( String srcString, String trimString ) {
            if (srcString == null) return null;
            if (trimString == null) return srcString;
            if (strUtil.IsNullOrEmpty( srcString )) return String.Empty;
            if (srcString.StartsWith( trimString ) == false) return srcString;
            return srcString.Substring( trimString.Length );
        }

        /// <summary>
        /// 将 html 中的脚本从各个部位，全部挪到页脚，以提高网页加载速度
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static String ResetScript( String html ) {

            Regex reg = new Regex( "<script.*?</script>", RegexOptions.Singleline );

            MatchCollection mlist = reg.Matches( html );
            StringBuilder sb = new StringBuilder();
            sb.Append( reg.Replace( html, "" ) );

            for (int i = 0; i < mlist.Count; i++) {
                sb.Append( mlist[i].Value );
            }
            return sb.ToString();
        }

        /// <summary>
        /// 将字符串分割成平均的n等份，每份长度为count
        /// </summary>
        /// <param name="str"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<String> SplitByNum( String str, int count ) {

            List<String> list = new List<String>();

            if (str == null) return list;
            if (str.Length == 0) {
                list.Add( str );
                return list;
            }

            if (count <= 0) {
                list.Add( str );
                return list;
            }

            int k = 0;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str.Length; i++) {

                if (k == count) {
                    list.Add( sb.ToString() );
                    k = 0;
                    sb = new StringBuilder();
                }

                sb.Append( str[i] );

                k++;
            }

            if (sb.Length > 0) list.Add( sb.ToString() );

            return list;
        }

        /// <summary>
        /// 统计字符出现的次数
        /// </summary>
        /// <param name="input">输入的字符</param>
        /// <param name="pattern">需要统计的字符</param>
        /// <returns></returns>
        public static int CountString( String input, String pattern ) {

            if (input == null) return 0;

            int count = 0;
            int i = 0;
            while ((i = input.IndexOf( pattern, i )) != -1) {
                i += pattern.Length;
                count++;
            }
            return count;
        }

        /// <summary>
        /// 将 html 中空白字符和空白标记(&amp;nbsp;)剔除掉
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static String TrimHtml( String val ) {

            if (val == null) return null;
            val = val.Trim();

            String text = ParseHtml( val );
            text = trimHtmlBlank( text );
            if (strUtil.IsNullOrEmpty( text ) && hasNotImg( val ) && hasNotFlash( val )) return "";

            val = trimHtmlBlank( val );
            return val;
        }

        private static String trimHtmlBlank( String text ) {

            if (text == null) return null;
            text = text.Trim();

            if (text.StartsWith( htmlBlank ) || text.EndsWith( htmlBlank )) {
                while (true) {
                    text = strUtil.TrimStart( text, htmlBlank ).Trim();
                    text = strUtil.TrimEnd( text, htmlBlank ).Trim();
                    if (!text.StartsWith( htmlBlank ) && !text.EndsWith( htmlBlank )) break;
                }
            }

            return text;
        }

        private static Boolean hasNotImg( String val ) {
            if (val.ToLower().IndexOf( "<img " ) >= 0) return false;
            return true;
        }

        private static Boolean hasNotFlash( String val ) {
            if (val.ToLower().IndexOf( "x-shockwave-flash" ) >= 0) return false;
            return true;
        }

        private static readonly String htmlBlank = "&nbsp;";



    }
}

