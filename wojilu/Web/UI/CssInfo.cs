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
using wojilu.Web.Mvc;

namespace wojilu.Web.UI {

    /// <summary>
    /// 常用 css 项的封装和转换
    /// </summary>
    public class CssInfo {

        //public static Dictionary<String, String> GetName() {

        //    Dictionary<String, String> cssName = new Dictionary<String, String>();
        //    cssName.Add( "width", "宽度(width)" );
        //    cssName.Add( "height", "高度(height)" );
        //    cssName.Add( "background-color", "背景色(background-color)" );
        //    cssName.Add( "background-image", "背景图片(background-image)" );

        //    cssName.Add( "background-position", "背景位置(background-position)" );
        //    cssName.Add( "background-repeat", "背景模式(background-repeat)" );

        //    cssName.Add( "margin-left", "外边距-左(margin-left)" );
        //    cssName.Add( "margin-top", "外边距-上(margin-top)" );
        //    cssName.Add( "margin-right", "外边距-右(margin-right)" );
        //    cssName.Add( "margin-bottom", "外边距-下(margin-bottom)" );

        //    cssName.Add( "padding-left", "内边距-左(padding-left)" );
        //    cssName.Add( "padding-top", "内边距-上(padding-top)" );
        //    cssName.Add( "padding-right", "内边距-右(padding-right)" );
        //    cssName.Add( "padding-bottom", "内边距-下(padding-bottom)" );

        //    cssName.Add( "border-width", "边框粗细(border-width)" );
        //    cssName.Add( "border-color", "边框颜色(border-color)" );
        //    cssName.Add( "border-style", "边框样式(border-style)" );
        //    cssName.Add( "font-size", "字体大小(font-size)" );
        //    cssName.Add( "font-family", "字体(font-family)" );
        //    cssName.Add( "font-style", "字体样式(font-style)" );
        //    cssName.Add( "font-weight", "字体加粗(font-weight)" );
        //    cssName.Add( "text-decoration", "文字装饰(text-decoration)" );
        //    cssName.Add( "text-align", "文本对齐(text-align)" );

        //    cssName.Add( "display", "是否显示(display)" );

        //    return cssName;
        //}

        public static Dictionary<String, String> GetEmptyValues() {

            Dictionary<String, String> v = new Dictionary<String, String>();
            v.Add( "width", "" );
            v.Add( "height", "" );
            v.Add( "background-color", "" );
            v.Add( "background-image", "" );
            v.Add( "background-image-value", "" );

            v.Add( "background-position", "" );
            v.Add( "background-repeat", "" );

            v.Add( "margin-left", "" );
            v.Add( "margin-top", "" );
            v.Add( "margin-right", "" );
            v.Add( "margin-bottom", "" );

            v.Add( "padding-left", "" );
            v.Add( "padding-top", "" );
            v.Add( "padding-right", "" );
            v.Add( "padding-bottom", "" );

            v.Add( "border-width", "" );
            v.Add( "border-color", "" );
            v.Add( "border-style", "" );
            v.Add( "font-size", "" );
            v.Add( "font-family", "" );
            v.Add( "font-style", "" );
            v.Add( "font-weight", "" );
            v.Add( "text-decoration", "" );
            v.Add( "text-align", "" );

            v.Add( "display", "" );

            return v;

        }

        public static Dictionary<String, CssControl> GetCssItem() {

            Dictionary<String, CssControl> css = new Dictionary<String, CssControl>();

            css.Add( "width", CssControl.String );
            css.Add( "height", CssControl.Px );
            css.Add( "background-color", CssControl.Color );
            css.Add( "background-image", CssControl.BackgroundUrl );
            css.Add( "background-position", CssControl.BackgroundPosition );
            css.Add( "background-repeat", CssControl.BackgroundRepeat );


            css.Add( "border-width", CssControl.Px );
            css.Add( "border-color", CssControl.Color );
            css.Add( "border-style", CssControl.BorderStyle );
            css.Add( "font-size", CssControl.Px );
            css.Add( "font-family", CssControl.FontFamily );
            css.Add( "font-style", CssControl.FontStyle );
            css.Add( "font-weight", CssControl.FontWeight );
            css.Add( "text-decoration", CssControl.TextDecoration );
            css.Add( "text-align", CssControl.TextAlign );

            css.Add( "margin-left", CssControl.Px );
            css.Add( "margin-top", CssControl.Px );
            css.Add( "margin-right", CssControl.Px );
            css.Add( "margin-bottom", CssControl.Px );
            css.Add( "padding-left", CssControl.Px );
            css.Add( "padding-top", CssControl.Px );
            css.Add( "padding-right", CssControl.Px );
            css.Add( "padding-bottom", CssControl.Px );

            css.Add( "display", CssControl.Display );
            return css;
        }

        //public static String GetCssForm( Dictionary<String, String> values ) {

        //    Dictionary<String, String> cname = GetName();

        //    Dictionary<String, CssControl> cssItems = GetCssItem();

        //    StringBuilder sb = new StringBuilder();
        //    sb.Append( "<table style=\"width:100%\" cellpadding=\"0\" cellspacing=\"0\">" );

        //    foreach (KeyValuePair<String, CssControl> kv in cssItems) {

        //        sb.AppendFormat( "<tr><td style=\"width:200px;\">{0}</td>", cname[kv.Key] );

        //        String ctl = getCssControlHtml( kv.Key, kv.Value, values );
        //        sb.AppendFormat( "<td>{0}</td>", ctl );
        //        sb.Append( "</tr>" );

        //    }

        //    sb.Append( "<tr><td>&nbsp;</td><td>" );
        //    sb.AppendFormat( "<input type=\"submit\" value=\"{0}\" class=\"btn\" />", lang.get( "submit" ) );
        //    sb.AppendFormat( "<input type=\"button\" value=\"{0}\" class=\"btnCancel\" />", lang.get( "cancel" ) );
        //    sb.Append( "</td></tr></table>" );

        //    return sb.ToString();
        //}


        private static String getCssControlHtml( String name, CssControl ctl, Dictionary<String, String> values ) {

            String val;
            values.TryGetValue( name, out val );

            if (ctl == CssControl.Px) {
                String intVal = strUtil.HasText( val ) ? strUtil.TrimEnd( val, "px" ) : "";
                return String.Format( "<input name=\"{0}\" type=\"text\" style=\"width:30px;\" value=\"{1}\" />px", name, intVal );
            }
            else if (ctl == CssControl.Percent) {
                String intVal = strUtil.HasText( val ) ? strUtil.TrimEnd( val, "%" ) : "";
                return String.Format( "<input name=\"{0}\" type=\"text\" style=\"width:30px;\" value=\"{1}\" />%", name, intVal );
            }
            else if (ctl == CssControl.BackgroundUrl) {
                String urlVal = "";
                if (strUtil.HasText( val )) {
                    urlVal = strUtil.TrimStart( val, "url(" ).TrimEnd( ')' );
                }
                return String.Format( "<input name=\"{0}\" type=\"text\" style=\"width:400px;\" value=\"{1}\" />", name, urlVal );
            }
            else if (ctl == CssControl.Color) {
                String colValue = strUtil.HasText( val ) ? val.TrimStart( '#' ) : val;
                return String.Format( "#<input name=\"{0}\" type=\"text\" style=\"width:80px;\" value=\"{1}\" />", name, colValue );
            }
            else if (ctl == CssControl.BackgroundPosition) {
                return Html.DropList( CssStyle.List[ctl], name, val );
            }
            else if (ctl == CssControl.BackgroundRepeat) {
                return Html.DropList( CssStyle.List[ctl], name, val );
            }
            else if (ctl == CssControl.BorderStyle) {
                return Html.DropList( CssStyle.List[ctl], name, val );
            }
            else if (ctl == CssControl.Display) {
                return Html.RadioList( CssStyle.List[ctl], name, val );
            }

            else if (ctl == CssControl.FontFamily) {
                return Html.DropList( CssStyle.List[ctl], name, val );
            }
            else if (ctl == CssControl.FontStyle) {
                return Html.RadioList( CssStyle.List[ctl], name, val );
            }
            else if (ctl == CssControl.FontWeight) {
                return Html.RadioList( CssStyle.List[ctl], name, val );
            }
            else if (ctl == CssControl.TextDecoration) {
                return Html.DropList( CssStyle.List[ctl], name, val );
            }
            else if (ctl == CssControl.TextAlign) {
                return Html.DropList( CssStyle.List[ctl], name, val );
            }


            return null;
        }
    }

}
