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

namespace wojilu.Web.Mvc.Routes {

    public class Requirements {

        public Requirements() {
            _dic = getDefaultRequirement();
        }

        private Dictionary<String, String> _dic;

        public Dictionary<String, String> getDic() {
            return _dic;
        }

        public void setDic( Dictionary<String, String> dic ) {

            foreach (KeyValuePair<String, String> pair in dic) {
                _dic.Add( pair.Key, pair.Value );
            }

        }

        public Boolean match( String itemName, String val ) {

            if (this.getDic().ContainsKey( itemName ) == false) return true;

            String pattern = this.getDic()[itemName];

            // 预置规则
            if (pattern.Equals( "letter" )) return strUtil.IsLetter( val );
            if (pattern.Equals( "int" )) return cvt.IsInt( val );
            if (pattern.Equals( "page" )) return isPageNumber( val );
            if (pattern.Equals( "ownertype" )) return MemberPath.IsValidOwnerType( val );

            // 其余使用正则表达式
            return Regex.IsMatch( val, pattern );
        }

        private static Dictionary<String, String> getDefaultRequirement() {
            Dictionary<String, String> dic = new Dictionary<String, String>();
            dic.Add( "ownertype", "ownertype" );
            return dic;
        }

        private static Boolean isPageNumber( String val ) {
            return strUtil.HasText(val) && val.Length>1 &&  val.StartsWith( "p" ) && cvt.IsInt( val.Substring( 1 ) );
        }

        public static int getPageNumber( String val ) {
            return cvt.ToInt( val.Substring( 1 ) );
        }

    }

}
