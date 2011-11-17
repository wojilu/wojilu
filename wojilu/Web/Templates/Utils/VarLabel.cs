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

namespace wojilu.Web.Templates {


    internal class VarLabel {

        public static readonly String CommonVar = "var";

        public String Prefix { get; set; }
        public String Postfix { get; set; }

        public VarLabel( String lable ) {

            if (strUtil.IsNullOrEmpty( lable )) return;

            _lable = lable;
            String[] arr = _lable.Split( '^' );
            if (arr.Length == 1) return;

            this.Prefix = arr[0];
            this.Postfix = arr[1];

        }

        private String _lable;

        public String Label {
            get { return _lable; }
        }

        private static Dictionary<String, VarLabel> VarList = getVarList();

        private static Dictionary<String, VarLabel> getVarList() {

            Dictionary<String, VarLabel> results = new Dictionary<String, VarLabel>();

            Dictionary<String, String> map = getMap();
            foreach (KeyValuePair<String, String> pair in map) {
                results.Add( pair.Value, new VarLabel( pair.Key ) );
            }

            return results;
        }

        private static Dictionary<String, String> getMap() {

            Dictionary<String, String> dic = new Dictionary<String, String>();

            dic.Add( "#{^}", CommonVar );
            dic.Add( "_{^}", "lang" );
            dic.Add( ":{^}", "alang" );
            dic.Add( "~^/", "path" );

            return dic;
        }

        public static VarLabelParsed GetVarLabelValue( CharSource ch ) {

            char[] charList = ch.charList;
            int index = ch.getIndex();

            foreach (KeyValuePair<String, VarLabel> v in VarList) {

                if (charList.Length < index + v.Value.Prefix.Length) continue;

                String varName = getMatchedVarName( v.Value, index, charList );
                if (varName != null) {

                    VarLabelParsed objVal = new VarLabelParsed();
                    objVal.TypeName = v.Key;
                    objVal.VarName = varName;
                    objVal.VarLabel = v.Value;

                    return objVal;

                }

            }

            return null;
        }

        private static String getMatchedVarName( VarLabel varLabel, int index, char[] charList ) {

            // 1、是否以prefix开头
            Boolean isStartWithPrefix = getIsStartWithPrefix( varLabel, charList, index );
            if (!isStartWithPrefix) return null;

            // 2、是否以postfix结尾

            String strToCheck = getStringToCheck( varLabel, index, charList );
            int postfixIndex = strToCheck.IndexOf( varLabel.Postfix );
            if (postfixIndex >= 0) {
                // 注意，这里可能返回String.Empty
                return strToCheck.Substring( 0, postfixIndex ); 
            }

            return null;
        }

        private static String getStringToCheck( VarLabel varLabel, int index, char[] charList ) {
            String strToCheck = "";
            int maxLength = getCheckMaxLength( varLabel );
            for (int i = varLabel.Prefix.Length; i < maxLength; i++) {
                if (index + i > charList.Length - 1) break;
                strToCheck += charList[index + i];
            }

            return strToCheck;
        }

        private static int getCheckMaxLength( VarLabel varLabel ) {
            return varLabel.Prefix.Length + 50;
        }

        private static Boolean getIsStartWithPrefix( VarLabel varLabel, char[] charList, int index ) {
            for (int i = 0; i < varLabel.Prefix.Length; i++) {
                if (charList[index + i] != varLabel.Prefix[i]) return false;
            }
            return true;
        }


    }

}
