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

namespace wojilu.Common.Resource {

    /// <summary>
    /// 各类常用基础数据列表(省份、性别、时间、身高、婚姻、血型、星座等)
    /// </summary>
    public class AppResource {

        /// <summary>
        /// 体型
        /// </summary>
        public static PropertyCollection Body = GetPropertyList( "member_body" );

        /// <summary>
        /// 其他用户联系我的条件
        /// </summary>
        public static PropertyCollection ContactCondition = GetPropertyList( "member_contactcondition" );

        /// <summary>
        /// email通知状态(启用/禁用)
        /// </summary>
        public static PropertyCollection EmailNotify = GetPropertyList( "member_emailnotify" );

        /// <summary>
        /// 性别(保密/男/女)
        /// </summary>
        public static PropertyCollection Gender = GetPropertyList( "member_gender" );

        /// <summary>
        /// 头发颜色
        /// </summary>
        public static PropertyCollection Hair = GetPropertyList( "member_hair" );

        /// <summary>
        /// 身高选项
        /// </summary>
        public static PropertyCollection Height = getHeightPropertyList();

        /// <summary>
        /// 省份
        /// </summary>
        public static PropertyCollection Province = GetPropertyList( "province" );

        /// <summary>
        /// 注册目的
        /// </summary>
        public static PropertyCollection Purpose = GetPropertyList( "member_regpurpose" );

        /// <summary>
        /// 婚姻状况
        /// </summary>
        public static PropertyCollection Relationship = GetPropertyList( "member_relationship" );

        /// <summary>
        /// 性取向
        /// </summary>
        public static PropertyCollection Sexuality = GetPropertyList( "member_sexuality" );

        /// <summary>
        /// 睡眠习惯
        /// </summary>
        public static PropertyCollection Sleeping = GetPropertyList( "member_sleeping" );

        /// <summary>
        /// 吸烟爱好
        /// </summary>
        public static PropertyCollection Smoking = GetPropertyList( "member_smoking" );

        /// <summary>
        /// 体重
        /// </summary>
        public static PropertyCollection Weight = getWeightPropertyList();

        /// <summary>
        /// 血型
        /// </summary>
        public static PropertyCollection Blood = GetPropertyList( "member_blood" );

        /// <summary>
        /// 星座
        /// </summary>
        public static PropertyCollection Zodiac = GetPropertyList( "member_zodiac" );

        /// <summary>
        /// 时间列表
        /// </summary>
        public static String[] Time = getOneDayTime();

        /// <summary>
        /// 学历
        /// </summary>
        public static PropertyCollection Degree = GetPropertyList( "degree_list" );

        private static PropertyCollection getHeightPropertyList() {
            PropertyCollection propertys = new PropertyCollection();
            propertys.Add( new PropertyItem( lang.get( "plsSelect" ), 0 ) );
            for (int i = 120; i < 221; i++) {
                propertys.Add( new PropertyItem( i + " cm", i ) );
            }
            return propertys;
        }

        /// <summary>
        /// 获取语言包里存储的键值对列表(用于自定义扩展)
        /// </summary>
        /// <param name="langItemName">语言key</param>
        /// <returns></returns>
        public static PropertyCollection GetPropertyList( String langItemName ) {

            PropertyCollection propertys = new PropertyCollection();
            String str = lang.get( langItemName );
            if (strUtil.IsNullOrEmpty( str )) return propertys;

            String[] strArray = str.Split( new char[] { '/' } );
            foreach (String item in strArray) {
                if (strUtil.IsNullOrEmpty( item )) continue;
                String[] arrPair = item.Split( new char[] { '-' } );
                if (arrPair.Length != 2) continue;
                String name = arrPair[0].Trim();
                int val = cvt.ToInt( arrPair[1] );
                propertys.Add( new PropertyItem( name, val ) );
            }

            return propertys;
        }

        /// <summary>
        /// 根据值获取省份名称
        /// </summary>
        /// <param name="provinceId"></param>
        /// <returns></returns>
        public static PropertyItem GetProvince( int provinceId ) {
            PropertyCollection province = Province;
            foreach (PropertyItem item in province) {
                if (item.Value == provinceId) return item;
            }
            return new PropertyItem( "", provinceId );
        }

        /// <summary>
        /// 根据项值，获取项的名称
        /// </summary>
        /// <param name="langKey">语言包中的key</param>
        /// <param name="itemId">项值</param>
        /// <returns>项的名称</returns>
        public static String GetItemName( String langKey, int itemId ) {
            PropertyCollection list = GetPropertyList( langKey );
            foreach (PropertyItem item in list) {
                if (item.Value == itemId) return item.Name;
            }
            return null;
        }

        private static PropertyCollection getWeightPropertyList() {
            PropertyCollection propertys = new PropertyCollection();
            propertys.Add( new PropertyItem( lang.get( "plsSelect" ), 0 ) );
            for (int i = 30; i < 131; i++) {
                propertys.Add( new PropertyItem( i + " kg", i ) );
            }
            return propertys;
        }
        /// <summary>
        /// 是否是“请选择”这个值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Boolean IsSelectTip( String name ) {
            if (name == null) return false;
            return name.Equals( lang.get( "plsSelect" ) );
        }

        private static String[] getOneDayTime() {

            String[] result = new string[48];

            result[0] = "0:00";
            result[1] = "0:30";

            result[2] = "1:00";
            result[3] = "1:30";

            result[4] = "2:00";
            result[5] = "2:30";

            result[6] = "3:00";
            result[7] = "3:30";

            result[8] = "4:00";
            result[9] = "4:30";

            result[10] = "5:00";
            result[11] = "5:30";

            result[12] = "6:00";
            result[13] = "6:30";

            result[14] = "7:00";
            result[15] = "7:30";

            result[16] = "8:00";
            result[17] = "8:30";

            result[18] = "9:00";
            result[19] = "9:30";

            result[20] = "10:00";
            result[21] = "10:30";

            result[22] = "11:00";
            result[23] = "11:30";

            result[24] = "12:00";
            result[25] = "12:30";

            result[26] = "13:00";
            result[27] = "13:30";

            result[28] = "14:00";
            result[29] = "14:30";

            result[30] = "15:00";
            result[31] = "15:30";

            result[32] = "16:00";
            result[33] = "16:30";

            result[34] = "17:00";
            result[35] = "17:30";

            result[36] = "18:00";
            result[37] = "18:30";

            result[38] = "19:00";
            result[39] = "19:30";

            result[40] = "20:00";
            result[41] = "20:30";

            result[42] = "21:00";
            result[43] = "21:30";

            result[44] = "22:00";
            result[45] = "22:30";

            result[46] = "23:00";
            result[47] = "23:30";

            return result;
        }

        /// <summary>
        /// 获取数值列表，用于下拉选项。自动在第一项前面增加“请选择”项，其值为0
        /// </summary>
        /// <param name="intFrom">起始值</param>
        /// <param name="intTo">终止值</param>
        /// <returns>数值列表</returns>
        public static PropertyCollection GetInts( int intFrom, int intTo ) {

            PropertyCollection propertys = new PropertyCollection();
            propertys.Add( new PropertyItem( lang.get( "plsSelect" ), 0 ) );

            for (int i = intFrom; i <= intTo; i++) {
                propertys.Add( new PropertyItem( i.ToString(), i ) );
            }

            return propertys;
        }

    }
}

