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
using wojilu.Web.Context;

namespace wojilu.Web.UI {

    /// <summary>
    /// css 表单工具
    /// </summary>
    public class CssFormUtil {


        public static Dictionary<String, String> getPostValues( MvcContext ctx ) {

            Dictionary<String, String> result = new Dictionary<String, String>();

            Dictionary<String, CssControl> cssItems = CssInfo.GetCssItem();
            foreach (KeyValuePair<String, CssControl> kv in cssItems) {

                String val = ctx.Post( kv.Key );
                if (kv.Value == CssControl.Color) {

                    val = getColorValue( val );
                }

                else if (kv.Value == CssControl.Px) {
                    if (strUtil.IsNullOrEmpty( val )) {
                        val = null;
                    }
                    else {

                        val = strUtil.TrimEnd( val, "px" ).Trim();

                        int intVal = cvt.ToInt( val );
                        val = intVal + "px";
                    }

                }
                else if (kv.Value == CssControl.BackgroundUrl) {
                    if (strUtil.HasText( val )) {
                        if (val.StartsWith( "http://" ) == false && val.StartsWith( "/" ) == false) {
                            val = "http://" + val;
                        }
                        val = String.Format( "url({0})", val );
                    }
                }

                result.Add( kv.Key, val );
            }
            return result;
        }

        private static String getColorValue( String val ) {

            if (strUtil.IsNullOrEmpty( val )) return null;
            if (val == "#") return null;

            val = strUtil.TrimStart( val, "#" );
            if (strUtil.IsColorValue( val )) return "#" + val;

            return null;
        }


        //-----------------------------------------------------------------------------------------------------------------------

        public static String mergeStyle( String strStyle, String name, Dictionary<String, String> result ) {
            Dictionary<String, Dictionary<String, String>> dic = mergeDic( strStyle, name, result );
            return Css.To( dic );
        }

        private static Dictionary<String, Dictionary<String, String>> mergeDic
            ( String strStyle, String name, Dictionary<String, String> result ) {
            Dictionary<String, Dictionary<String, String>> dic = Css.FromAndFill( strStyle );
            if (dic.ContainsKey( name ))
                dic[name] = combine( dic[name], result );
            else
                dic[name] = result;

            return dic;
        }

        private static Dictionary<String, String> combine( Dictionary<String, String> dic, Dictionary<String, String> result ) {
            foreach (KeyValuePair<String, String> kv in dic) {
                if (result.ContainsKey( kv.Key ) == false) {
                    result.Add( kv.Key, kv.Value );
                }

            }
            return result;
        }

        //-----------------------------------------------------------------------------------------------------------------------

        public static String mergeStyle( String oStyle, String newStyle ) {

            Dictionary<String, Dictionary<String, String>> oDic = Css.FromAndFill( oStyle );
            Dictionary<String, Dictionary<String, String>> nDic = Css.FromAndFill( newStyle );

            foreach (KeyValuePair<String, Dictionary<String, String>> kv in nDic) {
                oDic[kv.Key] = kv.Value;
            }

            return Css.To( oDic );
        }

        public static String MergeStyle( String oStyle, String newStyle ) {

            Dictionary<String, Dictionary<String, String>> oDic = Css.From( oStyle );
            Dictionary<String, Dictionary<String, String>> nDic = Css.From( newStyle );

            foreach (KeyValuePair<String, Dictionary<String, String>> kv in nDic) {

                if (oDic.ContainsKey( kv.Key )) {

                    Dictionary<String, String> oCssValues = oDic[kv.Key];
                    Dictionary<String, String> nCssValues = kv.Value;

                    Dictionary<String, String> cssValues = mergetCssValues( oCssValues, nCssValues );

                    oDic[kv.Key] = resetOrder( cssValues );

                }
                else {
                    oDic[kv.Key] = resetOrder( kv.Value );
                }

            }

            return Css.To( oDic );

        }

        // background-repeat 必须在 background 后面，否则页面不起作用
        private static Dictionary<String, String> resetOrder( Dictionary<String, String> cssValues ) {

            Dictionary<String, String> dic = new Dictionary<String, String>();
            if (cssValues.ContainsKey( "background" )) {
                dic.Add( "background", cssValues["background"] );
                cssValues.Remove( "background" );
            }

            foreach (KeyValuePair<String, String> kv in cssValues) {
                dic.Add( kv.Key, kv.Value );
            }

            return dic;
        }

        private static Dictionary<String, String> mergetCssValues( Dictionary<String, String> oCssValues, Dictionary<String, String> nCssValues ) {

            foreach (KeyValuePair<String, String> kv in nCssValues) {
                oCssValues[kv.Key] = kv.Value;
            }

            return oCssValues;
        }

    }

}
