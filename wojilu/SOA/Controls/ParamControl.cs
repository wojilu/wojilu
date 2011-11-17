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
using wojilu;

namespace wojilu.SOA.Controls {

    /// <summary>
    /// 参数在 html 表单中对应的控件
    /// </summary>
    public abstract class ParamControl {

        /// <summary>
        /// web界面中显示用的字符
        /// </summary>
        public String Label { get; set; }

        /// <summary>
        /// 参数名称，从0开始：param0, param1, param2, .... paramN
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 参数的值，用于传递
        /// </summary>
        public String Value { get; set; } 

        /// <summary>
        /// 控件的 Html
        /// </summary>
        public abstract String Html { get; }

        /// <summary>
        /// 参数的类型
        /// </summary>
        public abstract Type Type { get; }

        protected ParamControl() { }

        /// <summary>
        /// 将参数值转换为正确的参数类型
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public Object ChangeType( Object val ) {

            if (val == null) return getNullValue( val );

            try {
                return Convert.ChangeType( val, this.Type );
            }
            catch {
                return getNullValue( val );
            }
        }

        private Object getNullValue( Object val ) {
            if (this.Type == typeof( int )) return 0;
            if (this.Type == typeof( DateTime )) return DateTime.Now;
            return "";
        }

        /// <summary>
        /// 获取参数对应的控件
        /// </summary>
        /// <param name="param"></param>
        /// <param name="paramIndex"></param>
        /// <returns></returns>
        public static ParamControl GetControl( String param, int paramIndex ) {
            return GetControl( param, paramIndex, "" );
        }

        /// <summary>
        /// 获取参数对应的控件
        /// </summary>
        /// <param name="param"></param>
        /// <param name="paramIndex"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ParamControl GetControl( String param, int paramIndex, String value ) {

            if (strUtil.IsNullOrEmpty( param )) return null;

            String[] strArray = param.Split( new char[] { '=' } );
            String itemName = strArray[0];
            String itemValue = strArray[1];

            if (itemValue.StartsWith( "IntTextbox" )) {
                IntTextbox intBox = new IntTextbox();
                init( intBox, itemName, paramIndex, value );
                return intBox;
            }
            if (itemValue.StartsWith( "IntDroplist" )) {
                IntDroplist droplist = new IntDroplist();
                initList( droplist, itemName, paramIndex, value, itemValue );
                return droplist;
            }
            if (itemValue.StartsWith( "StringTextbox" )) {
                StringTextbox strBox = new StringTextbox();
                init( strBox, itemName, paramIndex, value );
                return strBox;
            }
            if (itemValue.StartsWith( "StringDroplist" )) {
                StringDroplist droplist = new StringDroplist();
                initList( droplist, itemName, paramIndex, value, itemValue );
                return droplist;
            }
            if (itemValue.StartsWith( "StringRadio" )) {
                StringRadio radio = new StringRadio();
                initList( radio, itemName, paramIndex, value, itemValue );
                return radio;
            }
            if (itemValue.StartsWith( "StringCheckbox" )) {
                StringCheckbox checkbox = new StringCheckbox();
                initList( checkbox, itemName, paramIndex, value, itemValue );
                return checkbox;
            }
            if (itemValue.StartsWith( "LongTextbox" )) {
                LongTextbox longBox = new LongTextbox();
                init( longBox, itemName, paramIndex, value );
                return longBox;
            }

            return null;
        }

        private static void init( ParamControl ctl, String itemName, int paramIndex, String value ) {
            ctl.Label = itemName;

            // 参数从0开始：param0, param1, param2, .... paramN
            ctl.Name = "param" + paramIndex; 
            ctl.Value = value;
        }

        private static void initList( ParamControl ctl, String itemName, int paramIndex, String value, String itemValue ) {
            ctl.Label = itemName;
            ctl.Name = "param" + paramIndex;
            ctl.Value = value;
            ((IListControl)ctl).Options = getOptions( itemValue );
        }

        private static String[] getOptions( String itemValue ) {
            String str = itemValue.TrimEnd( ')' ).Split( '(' )[1];
            return str.Split( '/' );
        }


    }
}

