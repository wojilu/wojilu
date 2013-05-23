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
using System.Collections.Generic;
using System.Collections;
using wojilu.Data;

namespace wojilu {

    /// <summary>
    /// 不同类型之间数值转换
    /// </summary>
    public class cvt {

        /// <summary>
        /// 判断字符串是否是小数或整数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean IsDecimal( String str ) {

            if (strUtil.IsNullOrEmpty( str )) {
                return false;
            }

            if (str.StartsWith( "-" )) {
                return isDecimal_private( str.TrimStart( '-' ) );
            }
            else
                return isDecimal_private( str );

        }

        private static Boolean isDecimal_private( String str ) {
            char[] arrChar = str.ToCharArray();
            foreach (char ch in arrChar) {
                if (!(char.IsDigit( ch ) || (ch == '.'))) {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 判断字符串是否是多个整数的列表，整数之间必须通过英文逗号分隔
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static Boolean IsIdListValid( String ids ) {

            if (strUtil.IsNullOrEmpty( ids )) {
                return false;
            }
            String[] strArray = ids.Split( new char[] { ',' } );
            foreach (String str in strArray) {
                if (!IsInt( str )) {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 判断字符串是否是整数或负整数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean IsInt( String str ) {

            if (strUtil.IsNullOrEmpty( str )) {
                return false;
            }

            if (str.StartsWith( "-" )) {
                str = str.Substring( 1, str.Length - 1 );
            }

            if (str.Length > 10) {
                return false;
            }

            char[] chArray = str.ToCharArray();
            foreach (char ch in chArray) {
                if (!char.IsDigit( ch )) {
                    return false;
                }
            }
            if (chArray.Length == 10) {

                int charInt;
                Int32.TryParse( chArray[0].ToString(), out charInt );
                if (charInt > 2) {
                    return false;
                }

                int charInt2;
                Int32.TryParse( chArray[1].ToString(), out charInt2 );

                if ((charInt == 2) && (charInt2 > 0)) {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// 判断字符串是否是"true"或"false"(不区分大小写)
        /// </summary>
        /// <param name="str"></param>
        /// <returns>只有字符串是"true"或"false"(不区分大小写)时，才返回true</returns>
        public static Boolean IsBool( String str ) {
            if (str == null) return false;
            if (strUtil.EqualsIgnoreCase( str, "true" ) || strUtil.EqualsIgnoreCase( str, "false" )) return true;

            return false;
        }

        /// <summary>
        /// 将对象转换成目标类型
        /// </summary>
        /// <param name="val"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public static Object To( Object val, Type destinationType ) {
            return Convert.ChangeType( val, destinationType );
        }

        /// <summary>
        /// 将整数转换成 Boolean 类型。只有参数等于1时，才返回true
        /// </summary>
        /// <param name="integer"></param>
        /// <returns>只有参数等于1时，才返回true</returns>
        public static Boolean ToBool( int integer ) {

            return (integer == 1);
        }

        /// <summary>
        /// 将对象转换成 Boolean 类型。只有对象的字符串形式等于1或者true(不区分大小写)时，才返回true
        /// </summary>
        /// <param name="objBool"></param>
        /// <returns>只有对象的字符串形式等于1或者true(不区分大小写)时，才返回true</returns>
        public static Boolean ToBool( Object objBool ) {

            if (objBool == null) {
                return false;
            }
            String str = objBool.ToString();
            return (str.Equals( "1" ) || str.ToUpper().Equals( "TRUE" ));
        }

        /// <summary>
        /// 将字符串(不区分大小写)转换成 Boolean 类型。只有字符串等于1或者true时，才返回true
        /// </summary>
        /// <param name="str"></param>
        /// <returns>只有字符串等于1或者true时，才返回true</returns>
        public static Boolean ToBool( String str ) {

            if (str == null) {
                return false;
            }
            if (str.ToUpper().Equals( "TRUE" )) {
                return true;
            }
            if (str.ToUpper().Equals( "FALSE" )) {
                return false;
            }
            return (str.Equals( "1" ) || str.ToUpper().Equals( "TRUE" ));
        }

        /// <summary>
        /// 将字符串转换成 System.Decimal 类型。如果str不是整数或小数，返回0
        /// </summary>
        /// <param name="str"></param>
        /// <returns>如果str不是整数或小数，返回0</returns>
        public static decimal ToDecimal( String str ) {

            if (!IsDecimal( str )) {
                return 0;
            }
            return Convert.ToDecimal( str );
        }

        /// <summary>
        /// 将字符串转换成 System.Double 类型。如果str不是整数或小数，返回0
        /// </summary>
        /// <param name="str"></param>
        /// <returns>如果str不是整数或小数，返回0</returns>
        public static Double ToDouble( String str ) {

            if (!IsDecimal( str )) {
                return 0;
            }
            return Convert.ToDouble( str );
        }

        /// <summary>
        /// 将字符串转换成 System.Decimal 类型。如果str不是整数或小数，返回参数 defaultValue 指定的值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal ToDecimal( String str, decimal defaultValue ) {

            if (!IsDecimal( str )) {
                return defaultValue;
            }
            return Convert.ToDecimal( str );
        }

        /// <summary>
        /// 将对象转换成整数；如果不是整数，则返回0
        /// </summary>
        /// <param name="objInt"></param>
        /// <returns>如果不是整数，则返回0</returns>
        public static int ToInt( Object objInt ) {

            if ((objInt != null) && IsInt( objInt.ToString() )) {
                int result;
                Int32.TryParse( objInt.ToString(), out result );
                return result;
            }
            return 0;
        }

        /// <summary>
        /// 将 decimal 转换成整数
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int ToInt( decimal number ) {
            return Convert.ToInt32( number );
        }

        /// <summary>
        /// 将对象转换成非Null形式，如果传入的参数是 null，则返回空字符串(即""，也即string.Empty)
        /// </summary>
        /// <param name="str"></param>
        /// <returns>如果为null，则返回空字符串(即""，也即string.Empty)</returns>
        public static String ToNotNull( Object str ) {

            if (str == null) {
                return "";
            }
            return str.ToString();
        }

        /// <summary>
        /// 将对象转换成 DateTime 形式，如果不符合格式，则返回当前时间
        /// </summary>
        /// <param name="objTime"></param>
        /// <returns>如果不符合格式，则返回当前时间</returns>
        public static DateTime ToTime( Object objTime ) {

            return ToTime( objTime, DateTime.Now );
        }

        /// <summary>
        /// 将对象转换成 DateTime 形式，如果不符合格式，则返回第二个参数指定的时间
        /// </summary>
        /// <param name="objTime"></param>
        /// <param name="targetTime"></param>
        /// <returns></returns>
        public static DateTime ToTime( Object objTime, DateTime targetTime ) {

            if (objTime == null) {
                return targetTime;
            }
            try {
                return Convert.ToDateTime( objTime );
            }
            catch {
                return targetTime;
            }
        }

        /// <summary>
        /// 判断两个时间的日期是否相同(要求同年同月同日)
        /// </summary>
        /// <param name="day1"></param>
        /// <param name="day2"></param>
        /// <returns></returns>
        public static Boolean IsDayEqual( DateTime day1, DateTime day2 ) {
            return (day1.Year == day2.Year && day1.Month == day2.Month && day1.Day == day2.Day);
        }

        /// <summary>
        /// 获取日期的日常表达形式，要求最近三天依次用 {今天，昨天，前天} 表示
        /// </summary>
        /// <param name="day"></param>
        /// <returns>要求最近三天依次用 {今天、昨天、前天} 表示</returns>
        public static String ToDayString( DateTime day ) {

            DateTime today = DateTime.Now;

            if (IsDayEqual( day, today )) return lang.get( "today" );
            if (IsDayEqual( day, today.AddDays( -1 ) )) return lang.get( "yesterday" );
            if (IsDayEqual( day, today.AddDays( -2 ) )) return lang.get( "thedaybeforeyesterday" );

            return day.ToShortDateString();
        }

        /// <summary>
        /// 获取时间的日常表达形式，格式为 {**小时前，**分钟前，**秒前}，以及 {昨天，前天}
        /// </summary>
        /// <param name="t"></param>
        /// <returns>格式为 {**小时前，**分钟前，**秒前}，以及 {昨天，前天}</returns>
        public static String ToTimeString( DateTime t ) {

            DateTime now = DateTime.Now;
            TimeSpan span = now.Subtract( t );

            if (cvt.IsDayEqual( t, now )) {

                if (span.Hours > 0)
                    return span.Hours + lang.get( "houresAgo" );
                else {
                    if (span.Minutes == 0)
                        if (span.Seconds == 0)
                            return lang.get( "justNow" );
                        else
                            return span.Seconds + lang.get( "secondAgo" );
                    else
                        return span.Minutes + lang.get( "minuteAgo" );
                }
            }

            if (cvt.IsDayEqual( t, now.AddDays( -1 ) )) return lang.get( "yesterday" );
            if (cvt.IsDayEqual( t, now.AddDays( -2 ) )) return lang.get( "thedaybeforeyesterday" );

            return t.ToShortDateString();
        }

        /// <summary>
        /// 将对象序列化为 xml (内部调用 .net 框架自带的 XmlSerializer)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static String ToXML( Object obj ) {
            return EasyDB.SaveToString( obj );
        }

        /// <summary>
        /// 将整数转换成字符串形式，多个整数之间用英文逗号分隔
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static String ToString( int[] ids ) {

            if (ids == null || ids.Length == 0) return "";
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < ids.Length; i++) {
                builder.Append( ids[i] );
                if (i < ids.Length - 1) builder.Append( ',' );
            }
            return builder.ToString();
        }

        /// <summary>
        /// 将字符串形式的 id 列表转换成整型数组
        /// </summary>
        /// <param name="myids"></param>
        /// <returns></returns>
        public static int[] ToIntArray( String myids ) {

            if (strUtil.IsNullOrEmpty( myids )) return new int[] { };

            String[] arrIds = myids.Split( ',' );
            int[] Ids = new int[arrIds.Length];
            for (int i = 0; i < arrIds.Length; i++) {
                int oneID = ToInt( arrIds[i].Trim() );
                Ids[i] = oneID;
            }

            return Ids;
        }

        /// <summary>
        /// 将字符串转换成以井号开头的表达形式；如果不是有效的颜色值，则返回null
        /// </summary>
        /// <param name="val"></param>
        /// <returns>将字符串转换成以井号开头的表达形式；如果不是有效的颜色值，则返回null</returns>
        public static String ToColorValue( String val ) {
            if (strUtil.IsColorValue( val ) == false) return null;
            if (val.StartsWith( "#" )) return val;
            return "#" + val;
        }

        /// <summary>
        /// 将10进制整数转换为62进制
        /// </summary>
        /// <param name="inputNum"></param>
        /// <returns>62进制数</returns>
        public static String ToBase62( Int64 inputNum ) {
            String chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return ToBase( inputNum, chars );
        }

        /// <summary>
        /// 将10进制整数转换为n进制
        /// </summary>
        /// <param name="inputNum">10进制整数</param>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static String ToBase( Int64 inputNum, String chars ) {
            int cbase = chars.Length;
            int imod;
            String result = "";

            while (inputNum >= cbase) {
                imod = (int)(inputNum % cbase);
                result = chars[imod] + result;
                inputNum = inputNum / cbase;
            }

            return chars[(int)inputNum] + result;
        }

        /// <summary>
        /// 将62进制转换为10进制整数
        /// </summary>
        /// <param name="str">62进制数</param>
        /// <returns>10进制整数</returns>
        public static Int64 DeBase62( String str ) {

            String chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return DeBase( str, chars );
        }

        /// <summary>
        /// 将n进制转换为10进制整数
        /// </summary>
        /// <param name="str">需要转换的n进制数</param>
        /// <param name="chars"></param>
        /// <returns>10进制整数</returns>
        public static Int64 DeBase( String str, String chars ) {
            int cbase = chars.Length;

            Int64 result = 0;
            for (int i = 0; i < str.Length; i++) {

                int index = chars.IndexOf( str[i] );
                result += index * (Int64)Math.Pow( cbase, (str.Length - i - 1) );
            }

            return result;
        }

        public static List<T> ToList<T>( IList list ) {
            List<T> results = new List<T>();
            foreach (T obj in list) {
                results.Add( obj );
            }
            return results;
        }


    }
}

