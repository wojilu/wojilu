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

namespace wojilu.Net {

    /// <summary>
    /// 与 ip 地址相关的工具类
    /// </summary>
    public class IpUtil {

        public static readonly String UnknowIp = "unknow";

        /// <summary>
        /// 判断 ip 地址是否在允许的范围内
        /// </summary>
        /// <param name="ip">需要判断的ip地址</param>
        /// <param name="list">禁止访问的ip规则</param>
        /// <returns></returns>
        /// <example>
        /// 使用说明。ip规则分4种：1)直接指出ip地址; 2)使用通配符; 3)用横杠指定范围; 4)用unknow禁止非法的ip地址
        /// <code>
        /// List&lt;String&gt; list = new List&lt;String&gt;();
        /// 
        /// list.Add( "88.88.99.99" );
        /// list.Add( "66.66.66.*" );
        /// list.Add( "55.55.*.*" );
        /// list.Add( "44.*.*.*" );
        /// list.Add( "192.168.3.2-192.168.3.250" );
        /// list.Add( "33.168.10.*-33.168.90.*" );
        /// list.Add( "unknow" );
        /// 
        /// Assert.IsFalse( IpUtil.IsAllowedIp( "88.88.99.99", list ) );
        /// Assert.IsTrue( IpUtil.IsAllowedIp( "88.88.99.100", list ) );
        /// 
        /// Assert.IsFalse( IpUtil.IsAllowedIp( "192.168.3.2", list ) );
        /// Assert.IsFalse( IpUtil.IsAllowedIp( "192.168.3.250", list ) );
        /// Assert.IsTrue( IpUtil.IsAllowedIp( "192.168.3.1", list ) );
        /// Assert.IsTrue( IpUtil.IsAllowedIp( "192.168.3.251", list ) );
        /// 
        /// Assert.IsFalse( IpUtil.IsAllowedIp( "66.66.66.1", list ) );
        /// Assert.IsFalse( IpUtil.IsAllowedIp( "66.66.66.2", list ) );
        /// Assert.IsFalse( IpUtil.IsAllowedIp( "66.66.66.254", list ) );
        /// Assert.IsFalse( IpUtil.IsAllowedIp( "66.66.66.255", list ) );
        /// 
        /// Assert.IsFalse( IpUtil.IsAllowedIp( "55.55.33.33", list ) );
        /// Assert.IsFalse( IpUtil.IsAllowedIp( "55.55.99.99", list ) );
        /// Assert.IsTrue( IpUtil.IsAllowedIp( "55.56.33.33", list ) );
        /// 
        /// Assert.IsFalse( IpUtil.IsAllowedIp( "44.55.33.33", list ) );
        /// Assert.IsFalse( IpUtil.IsAllowedIp( "44.99.33.33", list ) );
        /// Assert.IsTrue( IpUtil.IsAllowedIp( "45.99.33.33", list ) );
        /// 
        /// Assert.IsFalse( IpUtil.IsAllowedIp( "33.168.10.1", list ) );
        /// Assert.IsFalse( IpUtil.IsAllowedIp( "33.168.10.2", list ) );
        /// Assert.IsFalse( IpUtil.IsAllowedIp( "33.168.10.255", list ) );
        /// Assert.IsFalse( IpUtil.IsAllowedIp( "33.168.90.1", list ) );
        /// Assert.IsFalse( IpUtil.IsAllowedIp( "33.168.90.2", list ) );
        /// Assert.IsFalse( IpUtil.IsAllowedIp( "33.168.90.255", list ) );
        /// 
        /// Assert.IsFalse( IpUtil.IsAllowedIp( "", list ) );
        /// Assert.IsFalse( IpUtil.IsAllowedIp( "unknow", list ) );
        /// Assert.IsFalse( IpUtil.IsAllowedIp( "abc", list ) );
        /// Assert.IsFalse( IpUtil.IsAllowedIp( "-", list ) );
        /// 
        /// Assert.IsTrue( IpUtil.IsAllowedIp( "68.81.101.87", list ) );
        /// Assert.IsTrue( IpUtil.IsAllowedIp( "71.85.125.152", list ) );
        /// Assert.IsTrue( IpUtil.IsAllowedIp( "85.114.137.152", list ) );
        /// </code>
        /// </example>
        public static Boolean IsAllowedIp( String ip, List<String> list ) {

            if (isIpError( ip )) {
                return !list.Contains( UnknowIp );
            }

            foreach (String setting in list) {

                if (setting.Equals( UnknowIp )) continue;

                if (isAllowedSingle( ip, setting ) == false) return false;
            }

            return true;
        }

        /// <summary>
        /// 判断 ip 地址是否在允许的范围内
        /// </summary>
        /// <param name="ip">需要判断的ip地址</param>
        /// <param name="settings">禁止访问的ip规则</param>
        /// <returns></returns>
        public static Boolean IsAllowedIp( String ip, String[] settings ) {
            List<String> list = new List<String>( settings );
            return IsAllowedIp( ip, list );
        }

        /// <summary>
        /// 将 ip 转化为通配符形式，隐藏后几位信息
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="trimEnd">需要隐藏的位数，从末尾往前倒数</param>
        /// <returns></returns>
        public static String GetIpWild( String ip, int hideLength ) {

            if (isIpError( ip )) return "ip error";

            String[] arrItem = ip.Split( '.' );

            if (hideLength == 1) {
                return arrItem[0] +"."+ arrItem[1]+"." + arrItem[2] + ".*";
            }
            else if (hideLength == 2) {
                return arrItem[0] + "." + arrItem[1]  + ".*.*";
            }
            else if (hideLength == 3) {
                return arrItem[0] + ".*.*.*";
            }
            else if (hideLength == 4) {
                return "*.*.*.*";
            }
            else {
                return ip;
            }

        }

        private static bool isAllowedSingle( String ip, String settingItem ) {

            if (settingItem.IndexOf( "-" ) > 0) return isAllowedScaleIp( settingItem, ip );
            if (settingItem.IndexOf( "*" ) >= 0) return isAllowedWildcards( settingItem, ip );

            if (isIpError( settingItem )) return true;// 配置无效

            return !ip.Equals( settingItem );
        }


        // 192.168.35.*
        private static bool isAllowedWildcards( String settingItem, String ip ) {

            if (isIpError( settingItem )) return true; // 配置无效

            String[] arr = settingItem.Split( '.' );
            String[] arrIp = ip.Split( '.' );
            for (int i = 0; i < arr.Length; i++) {
                if (arr[i].Equals( "*" )) continue;
                if (arr[i].Equals( arrIp[i] ) == false) return true;
            }

            return false;
        }

        // 192.168.3.12-192.168.3.250
        private static bool isAllowedScaleIp( String scaleSetting, String ip ) {

            String[] settingItems = scaleSetting.Split( '-' );

            String itemA = settingItems[0];
            String itemB = settingItems[1];

            if (isIpError( itemA )) return true; // 配置无效
            if (isIpError( itemB )) return true; // 配置无效

            Boolean inScale = ipGreaterOrEqual( ip, itemA ) && ipLessOrEqual( ip, itemB );
            return !inScale;
        }

        // ip>=itemA
        private static bool ipGreaterOrEqual( String ip, String itemA ) {

            String[] arrIp = ip.Split( '.' );
            String[] arrSetting = itemA.Split( '.' );

            for (int i = 0; i < arrSetting.Length; i++) {
                if (arrSetting[i].Equals( "*" )) continue;
                if (cvt.ToInt( arrIp[i] ) < cvt.ToInt( arrSetting[i] )) return false;
            }

            return true;
        }

        // ip<=itemB
        private static bool ipLessOrEqual( String ip, String itemA ) {

            String[] arrIp = ip.Split( '.' );
            String[] arrSetting = itemA.Split( '.' );

            for (int i = 0; i < arrSetting.Length; i++) {
                if (arrSetting[i].Equals( "*" )) continue;
                if (cvt.ToInt( arrIp[i] ) > cvt.ToInt( arrSetting[i] )) return false;
            }

            return true;
        }


        // 配置格式是否正确
        private static bool isIpError( string ip ) {

            Boolean error = true;

            if (strUtil.IsNullOrEmpty( ip )) return error;
            String[] arrItem = ip.Split( '.' );
            if (arrItem.Length != 4) return error;

            foreach (String item in arrItem) {
                if (item.Equals( "*" )) continue;
                if (cvt.IsInt( item ) == false) return error;
                int itemValue = cvt.ToInt( item );
                if (itemValue < 0 || itemValue > 255) return error;
            }

            return false;
        }

    }

}
