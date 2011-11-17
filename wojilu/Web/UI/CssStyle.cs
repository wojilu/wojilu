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

namespace wojilu.Web.UI {

    /// <summary>
    /// 常见 css 选项列表：比如边框样式、字体、背景方式等
    /// </summary>
    public class CssStyle {

        public static Dictionary<CssControl, Dictionary<String, String>> List = getStyleAll();

        private static Dictionary<CssControl, Dictionary<String, String>> getStyleAll() {
            Dictionary<CssControl, Dictionary<String, String>> dic = new Dictionary<CssControl, Dictionary<String, String>>();

            dic[CssControl.BorderStyle] = getBoardStyle();
            dic[CssControl.Display] = getDisplay();
            dic[CssControl.FontFamily] = getFontFamily();
            dic[CssControl.FontStyle] = getFontStyle();
            dic[CssControl.FontWeight] = getFontWeight();
            dic[CssControl.TextDecoration] = getTextDecoration();
            dic[CssControl.TextAlign] = getTextAlign();

            dic[CssControl.BackgroundPosition] = getBackgroundPosition();
            dic[CssControl.BackgroundRepeat] = getBackgroundRepeat();

            return dic;
        }

        private static Dictionary<String, String> getBoardStyle() {

            Dictionary<String, String> dic = new Dictionary<String, String>();

            dic.Add( "默认", "" );
            dic.Add( "实线/solid", "solid" );
            dic.Add( "虚线/dashed", "dashed" );
            dic.Add( "点状/dotted", "dotted" );
            dic.Add( "双线/double", "double" );
            dic.Add( "槽状/groove", "groove" );
            dic.Add( "脊状/ridge", "ridge" );
            dic.Add( "凹陷/inset", "inset" );
            dic.Add( "凸出/outset", "outset" );
            dic.Add( "无边框/none", "none" );

            return dic;
        }

        private static Dictionary<String, String> getBackgroundPosition() {

            Dictionary<String, String> dic = new Dictionary<String, String>();

            dic.Add( "默认", "" );

            dic.Add( "上部左侧/top left", "top left" );
            dic.Add( "上部中间/top center", "top center" );
            dic.Add( "上部右侧/top right", "top right" );

            dic.Add( "中间左侧/center left", "center left" );
            dic.Add( "正中间/center center", "center center" );
            dic.Add( "中间右侧/center right", "center right" );

            dic.Add( "底部左侧/bottom left", "bottom left" );
            dic.Add( "底部中间/bottom center", "bottom center" );
            dic.Add( "底部右侧/bottom right", "bottom right" );

            return dic;
        }

        private static Dictionary<String, String> getBackgroundRepeat() {

            Dictionary<String, String> dic = new Dictionary<String, String>();

            dic.Add( "默认", "" );
            dic.Add( "平铺/repeat", "repeat" );
            dic.Add( "不平铺/no-repeat", "no-repeat" );
            dic.Add( "横向平铺/repeat-x", "repeat-x" );
            dic.Add( "纵向平铺/repeat-y", "repeat-y" );

            return dic;
        }

        private static Dictionary<String, String> getDisplay() {

            Dictionary<String, String> dic = new Dictionary<String, String>();

            dic.Add( "显示", "" );
            dic.Add( "隐藏", "none" );

            return dic;
        }

        private static Dictionary<String, String> getFontFamily() {

            Dictionary<String, String> dic = new Dictionary<String, String>();

            dic.Add( "默认", "" );
            dic.Add( "宋体", "宋体" );
            dic.Add( "楷体", "楷体_GB2312" );
            dic.Add( "黑体", "黑体" );
            dic.Add( "微软雅黑", "微软雅黑" );
            dic.Add( "隶书", "隶书" );
            dic.Add( "仿宋", "仿宋_GB2312" );
            dic.Add( "Arial", "arial, helvetica, sans-serif" );
            dic.Add( "Arial Black", "Arial Black" );
            dic.Add( "Verdana", "Verdana, Arial, Helvetica, sans-serif" );
            dic.Add( "Courier New", "courier new, courier, mono" );
            dic.Add( "Times New Roman", "times new roman, times, serif" );

            return dic;
        }

        private static Dictionary<String, String> getFontStyle() {

            Dictionary<String, String> dic = new Dictionary<String, String>();

            dic.Add( "默认", "" );
            dic.Add( "斜体", "italic" );
            dic.Add( "无样式", "normal" );

            return dic;
        }

        private static Dictionary<String, String> getFontWeight() {

            Dictionary<String, String> dic = new Dictionary<String, String>();

            dic.Add( "默认", "" );
            dic.Add( "粗体", "bold" );
            dic.Add( "不加粗", "normal" );

            return dic;
        }

        private static Dictionary<String, String> getTextDecoration() {

            Dictionary<String, String> dic = new Dictionary<String, String>();

            dic.Add( "默认", "" );
            dic.Add( "下划线/underline", "underline" );
            dic.Add( "删除线/line-through", "line-through" );
            dic.Add( "上划线/overline", "overline" );
            dic.Add( "闪烁/blink", "blink" );
            dic.Add( "无修饰/none", "none" );

            return dic;
        }

        private static Dictionary<String, String> getTextAlign() {

            Dictionary<String, String> dic = new Dictionary<String, String>();

            dic.Add( "默认", "" );
            dic.Add( "左对齐/left", "left" );
            dic.Add( "中间对齐/center", "center" );
            dic.Add( "右对齐/right", "right" );

            return dic;
        }


    }
}
