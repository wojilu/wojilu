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
using wojilu.Web.Mvc;

namespace wojilu.Web.Utils.Tags {

    internal class TagFilter {

        private static readonly Regex scriptReg = new Regex( @"<script[\s\S]+</script *>", RegexOptions.IgnoreCase );
        private static readonly Regex iframeReg = new Regex( @"<iframe[\s\S]+</iframe *>", RegexOptions.IgnoreCase );
        private static readonly Regex framesetReg = new Regex( @"<frameset[\s\S]+</frameset *>", RegexOptions.IgnoreCase );
        private static readonly Regex styleReg = new Regex( @"<style[\s\S]+</style *>", RegexOptions.IgnoreCase );


        public static String Clear( String input ) {
            if (TagWhitelist.IsAllowAllHtml()) return input;
            return Clear( input, TagWhitelist.GetInstance(), true );
        }

        public static String Clear( String input, String tags ) {
            return Clear( input, TagWhitelist.GetTags( tags ), true );
        }

        public static String ClearWithAllowedTags( String input, String tags ) {
            return Clear( input, TagWhitelist.AppendTags( tags ), true );
        }

        /// <summary>
        /// 根据白名单过滤
        /// </summary>
        /// <param name="input">需要过滤的字符串</param>
        /// <param name="whitelist">白名单</param>
        /// <param name="isAddBaseAttr">是否允许 id/class/style 这三个基础属性</param>
        /// <returns></returns>
        public static String Clear( String input, Dictionary<String, String> whitelist, Boolean isAddBaseAttr ) {

            input = clearScriptContent( input, whitelist );

            MatchCollection ms = getMatchs( input );

            StringBuilder sb = new StringBuilder();

            int lastIndex = 0;

            List<String> tobeClearTags = new List<String>();

            foreach (Match m in ms) {

                TagValid validResult = checkTagValid( m, tobeClearTags, whitelist );
                sb.Append( input.Substring( lastIndex, m.Index - lastIndex ) );

                if (validResult.IsValid) {
                    if (validResult.IsStartTag) {

                        addTagAndAttribute( input, sb, m, whitelist, isAddBaseAttr );
                    }
                    else {
                        addTagString( input, sb, m );
                    }
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

        // 对 script/iframe/frameset/style 的特殊处理：一律清空标签内容
        private static String clearScriptContent( String input, Dictionary<String, String> whitelist ) {

            if (whitelist.ContainsKey( "script" ) == false) {
                input = scriptReg.Replace( input, "" );
            }

            if (whitelist.ContainsKey( "iframe" ) == false) {
                input = iframeReg.Replace( input, "" );
            }

            if (whitelist.ContainsKey( "frameset" ) == false) {
                input = framesetReg.Replace( input, "" );
            }

            if (whitelist.ContainsKey( "style" ) == false) {
                input = styleReg.Replace( input, "" );
            }

            return input;
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

            String tag = m.Value;

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

            result.IsValid = whitelist.ContainsKey( tagName.ToLower() );
            return result;
        }

        private static TagValid checkStartTag( String tag, Dictionary<String, String> whitelist ) {

            int nameEndIndex = tag.IndexOf( ' ' );
            if (nameEndIndex == -1) nameEndIndex = tag.Length - 1;

            String tagName = tag.Substring( 1, nameEndIndex - 1 );
            tagName = tagName.TrimEnd( '/' );

            if (whitelist.ContainsKey( tagName.ToLower() ) == false) {
                return new ErrorTagValid( tagName );
            }

            return new TagValid( tagName );
        }


        private static void addTagString( string input, StringBuilder sb, Match m ) {
            sb.Append( input.Substring( m.Index, m.Length ) );
        }

        private static void addTagAndAttribute( String input, StringBuilder sb, Match m, Dictionary<String, String> whitelist, Boolean isAddBaseAttr ) {

            String tag = m.Value.Trim();

            int nameEndIndex = tag.IndexOf( ' ' );
            if (nameEndIndex == -1) nameEndIndex = tag.Length - 1;

            String tagName = tag.Substring( 1, nameEndIndex - 1 );

            int attrStartIndex = tagName.Length;
            String attributes = tag.Substring( attrStartIndex + 1, (tag.Length - (attrStartIndex + 2)) );

            if (strUtil.IsNullOrEmpty( attributes )) {
                sb.Append( input.Substring( m.Index, m.Length ) );
                return;
            }

            attributes = attributes.Trim().TrimEnd( '/' ).Trim();
            String[] allowedAttr = getAllowedAttr( tagName, whitelist, isAddBaseAttr );

            sb.Append( "<" + tagName + "" );
            addAttribute( attributes, allowedAttr, sb );
            sb.Append( getTagClose( tag ) );
        }

        private static String getTagClose( String tag ) {
            StringBuilder sb = new StringBuilder();
            for (int i = tag.Length - 1; i >= 0; i--) {
                if (tag[i] == '>' || tag[i] == ' ' || tag[i] == '/') {
                    sb.Append( tag[i] );
                }
                else {
                    break;
                }
            }
            return reverseString( sb );
        }

        private static string reverseString( StringBuilder sb ) {
            String str = "";
            for (int i = sb.Length - 1; i >= 0; i--) {
                str += sb[i];
            }
            return str;
        }

        private static Boolean addAttribute( String attributes, String[] allowedAttr, StringBuilder sb ) {

            Dictionary<String, String> dic = TagHelper.getAttributes( attributes );

            foreach (KeyValuePair<String, String> kv in dic) {

                if (isContainsAttr( allowedAttr, kv.Key ) == false) continue;

                if (kv.Key == "href") {
                    if (isUrlValid( kv.Value )) {
                        sb.Append( " " + kv.Key + "=" + "\"" + kv.Value + "\"" );
                    }
                }
                else {
                    if (isAttrValueValid( kv.Value.ToLower() )) {
                        sb.Append( " " + kv.Key + "=" + "\"" + kv.Value + "\"" );
                    }
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
                if (attrValue.IndexOf( str ) >= 0) return false;
            }
            return true;
        }

        private static Boolean isContainsAttr( String[] allowedAttr, String attrName ) {
            if (strUtil.IsNullOrEmpty( attrName )) return false;
            foreach (String a in allowedAttr) {
                if (strUtil.EqualsIgnoreCase( a, attrName )) return true;
            }
            return false;
        }

        private static String[] getAllowedAttr( String tagName, Dictionary<String, String> whitelist, Boolean isAddBaseAttr ) {

            String al = whitelist[tagName.ToLower()];

            String[] allowedAttr = strUtil.HasText( al ) ? al.Split( ',' ) : new String[] { };
            String[] result = new string[allowedAttr.Length + 3];
            for (int i = 0; i < allowedAttr.Length; i++) {
                result[i] = allowedAttr[i];
            }

            if (isAddBaseAttr) {
                result[allowedAttr.Length] = "id";
                result[allowedAttr.Length + 1] = "class";
                result[allowedAttr.Length + 2] = "style";
            }

            String ss = "";
            foreach (String str in result) {
                ss += str + ",";
            }

            return result;
        }


    }

}
