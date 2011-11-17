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

namespace wojilu.Web.UI {

    /// <summary>
    /// css ×ª»»Æ÷
    /// </summary>
    public class Css {

        public static Dictionary<String, Dictionary<String, String>> FromAndFill( String str ) {
            return fromPrivate( str, true );
        }

        public static Dictionary<String, Dictionary<String, String>> From( String str ) {
            return fromPrivate( str, false );

        }

        private static Dictionary<String, Dictionary<String, String>> fromPrivate( String str, Boolean isFill ) {
            Dictionary<String, Dictionary<String, String>> result = new Dictionary<String, Dictionary<String, String>>();
            if (strUtil.IsNullOrEmpty( str )) return result;

            str = str.Replace( "  ", "" );
            String[] arr = str.Split( '}' );
            foreach (String one in arr) {
                if (strUtil.IsNullOrEmpty( one )) continue;

                String[] arrPair = one.Split( '{' );
                String name = arrPair[0].Trim();
                String val = arrPair[1];

                if( isFill )
                    result.Add( name, FromItemByFillEmptyValue( val ) );
                else
                    result.Add( name, FromItem( val ) );

            }

            return result;
        }


        public static String To( Dictionary<String, Dictionary<String, String>> items ) {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<String, Dictionary<String, String>> kv in items) {

                String itemValue = ToItem( kv.Value );
                if (itemValue.Equals( "{ }" )) continue;

                sb.Append( kv.Key );
                sb.Append( " " );
                sb.Append( itemValue );
                sb.AppendLine();
            }

            return sb.ToString();
        }

        //----------------------------------------------------------------------------------------------

        public static String ToItem( Dictionary<String, String> items ) {
            StringBuilder sb = new StringBuilder();
            sb.Append( "{ " );
            foreach (KeyValuePair<String, String> p in items) {

                if (strUtil.IsNullOrEmpty( p.Value )) continue;

                sb.Append( p.Key );
                sb.Append( ":" );
                sb.Append( p.Value );
                sb.Append( "; " );
            }
            sb.Append( "}" );

            return sb.ToString();
        }


        public static Dictionary<string, string> FromItem( String cssString ) {


            if (strUtil.IsNullOrEmpty( cssString )) return new Dictionary<string, string>();

            string items = cssString.Trim().TrimStart( '{' ).TrimEnd( '}' ).Trim().TrimEnd( ';' );

            string[] arrItem = items.Split( ';' );

            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (string item in arrItem) {

                if (strUtil.IsNullOrEmpty( item )) continue;

                string[] arrPair = item.Split( new char[] { ':' }, 2 );
                if (arrPair.Length < 2) continue;

                String name = arrPair[0].Trim();
                String val = arrPair[1].Trim();

                if (strUtil.IsNullOrEmpty( name )) continue;

                dic.Add( name, val );

            }

            return dic;
        }

        private static Dictionary<string, string> FromItemByFillEmptyValue( String cssString ) {

            Dictionary<String, String> css = CssInfo.GetEmptyValues();

            Dictionary<String, String> items = FromItem( cssString );

            foreach (KeyValuePair<String, String> kv in items) {
                css[kv.Key] = kv.Value;
            }

            processBackgroundImage( css );

            return css;
        }

        private static void processBackgroundImage( Dictionary<String, String> css ) {
            String backgroundImage = css["background-image"];
            if (strUtil.HasText( backgroundImage )) {
                backgroundImage = backgroundImage.Trim();
                backgroundImage = strUtil.TrimStart( backgroundImage, "url(" )
                    .Trim().TrimStart( '"' ).TrimStart( '\'' ).TrimEnd( ')' ).Trim();
            }
            css["background-image-value"] = backgroundImage;
        }


    }


}
