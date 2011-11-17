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
using System.Text.RegularExpressions;
using wojilu.Web.Utils;

namespace wojilu.Web.Utils.Tags {

    internal class TagFilter {

        public static String Clear( String input ) {
            return Clear( input, TagWhitelist.GetInstance() );
        }

        public static String Clear( String input, String tags ) {
            return Clear( input, TagWhitelist.GetTags( tags ) );
        }

        public static String ClearWithAllowedTags( String input, String tags ) {
            return Clear( input, TagWhitelist.AppendTags( tags ) );
        }

        private static String Clear( String input, Dictionary<String, String> whitelist ) {

            MatchCollection ms = getMatchs( input );

            StringBuilder sb = new StringBuilder();

            int lastIndex = 0;

            List<String> tobeClearTags = new List<String>();

            foreach (Match m in ms) {

                TagValid validResult = checkTagValid( m, tobeClearTags, whitelist );
                sb.Append( input.Substring( lastIndex, m.Index - lastIndex ) );

                if (validResult.IsValid) {
                    sb.Append( input.Substring( m.Index, m.Length ) );
                }
                else {
                    if (validResult.IsStartTag) {
                        tobeClearTags.Add( validResult.Name );
                    }
                }

                lastIndex = m.Index + m.Length;

            }
            sb.Append( input.Substring( lastIndex, input.Length - lastIndex ) );

            return sb.ToString();
        }


        private static MatchCollection getMatchs( String input ) {
            String pattern = "<[^<>]+>?";
            return getMatchsPrivate( input, pattern );
        }

        private static MatchCollection getMatchsPrivate( String input, String pattern ) {
            MatchCollection ms = Regex.Matches( input, pattern, RegexOptions.IgnoreCase );
            return ms;
        }

        private static TagValid checkTagValid( Match m, List<String> tobeClearTags, Dictionary<String, String> whitelist ) {

            String tag = m.Value.ToLower();

            TagValid isComment = checkHtmlComment( tag );
            if (isComment.IsValid) return isComment;

            String endTagName = getEndTagName( tag );
            if (strUtil.HasText( endTagName )) {
                TagValid v = checkEndTag( endTagName, tobeClearTags, whitelist );
                v.IsStartTag = false;
                return v;
            }

            TagValid result = checkStartTag( tag, whitelist );
            result.IsStartTag = true;
            return result;
        }

        private static TagValid checkHtmlComment( string tag ) {
            TagValid v = new TagValid();
            v.IsValid = true;
            if (strUtil.IsNullOrEmpty( tag )) return v;
            if (tag.StartsWith( "<!--" ) && tag.EndsWith( "-->" )) return v;

            v.IsValid = false;
            return v;
        }

        private static String getEndTagName( String tag ) {

            if (strUtil.IsNullOrEmpty( tag )) return null;
            if (tag.StartsWith( "<" ) == false) return null;

            Boolean isSecondSlash = false;
            String str = "";
            for (int i = 1; i < tag.Length - 1; i++) {
                if (tag[i] == ' ') {
                    continue;
                }

                if (isSecondSlash) {
                    str += tag[i];
                    continue;
                }

                if (tag[i] != '/') {
                    return null;
                }

                if (tag[i] == '/') {
                    isSecondSlash = true;
                    continue;
                }
            }
            return str.Trim();
        }

        private static TagValid checkEndTag( String tagName, List<String> tobeClearTags, Dictionary<String, String> whitelist ) {
            TagValid result = new TagValid( tagName );

            foreach (String tobeTag in tobeClearTags) {
                if (tobeTag.Equals( tagName )) {
                    tobeClearTags.Remove( tobeTag );
                    result.IsValid = false;
                    return result;
                }
            }

            result.IsValid = whitelist.ContainsKey( tagName );
            return result;
        }

        private static TagValid checkStartTag( String tag, Dictionary<String, String> whitelist ) {

            int nameEndIndex = tag.IndexOf( ' ' );
            if (nameEndIndex == -1) nameEndIndex = tag.Length - 1;

            String tagName = tag.Substring( 1, nameEndIndex - 1 );

            if (whitelist.ContainsKey( tagName ) == false) {
                return new ErrorTagValid( tagName );
            }

            int attrStartIndex = tagName.Length;
            String attributes = tag.Substring( attrStartIndex + 1, (tag.Length - (attrStartIndex + 2)) );
            if (strUtil.IsNullOrEmpty( attributes )) return new TagValid( tagName );

            attributes = attributes.Trim().TrimEnd( '/' ).Trim();
            String[] allowedAttr = getAllowedAttr( tagName, whitelist );

            Boolean isAlloweda = isAllowedAttr( attributes, allowedAttr );
            return new TagValid( tagName, isAlloweda );

        }

        private static String[] getAllowedAttr( String tagName, Dictionary<String, String> whitelist ) {

            String al = whitelist[tagName];

            String[] allowedAttr = strUtil.HasText( al ) ? al.Split( ',' ) : new String[] { };
            String[] result = new string[allowedAttr.Length + 3];
            for (int i = 0; i < allowedAttr.Length; i++) {
                result[i] = allowedAttr[i];
            }
            result[allowedAttr.Length] = "class";
            result[allowedAttr.Length + 1] = "style";
            result[allowedAttr.Length + 2] = "id";

            String ss = "";
            foreach (String str in result) {
                ss += str + ",";
            }

            return result;
        }

        private static Boolean isAllowedAttr( String attributes, String[] allowedAttr ) {

            Dictionary<String, String> dic = TagHelper.getAttributes( attributes );

            foreach (KeyValuePair<String, String> kv in dic) {
                if (isContainsAttr( allowedAttr, kv.Key ) == false) return false;
                if (kv.Key == "href") {
                    if (isUrlValid( kv.Value ) == false) return false;
                }
                else {
                    if (isAttrValueValid( kv.Value ) == false) return false;
                }
            }

            return true;
        }

        private static Boolean isUrlValid( String attrValue ) {
            if (attrValue.StartsWith( "javascript" )) return false;
            return true;
        }

        private static Boolean isAttrValueValid( String attrValue ) {
            
            String[] list = "javascript|script|eval|behaviour|expression".Split( '|' );
            foreach (String str in list) {
                if (attrValue.IndexOf( str )>=0) return false;
            }
            return true;
        }

        private static Boolean isContainsAttr( String[] allowedAttr, String attrName ) {
            if (strUtil.IsNullOrEmpty( attrName )) return false;
            foreach (String a in allowedAttr) {
                if (a == attrName) return true;
            }
            return false;
        }


    }

}
