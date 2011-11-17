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

namespace wojilu.ORM.Utils {

    internal class SqlUtil {


        public static List<String[]> getEntityProperties( String condition ) {

            List<String[]> result = new List<String[]>();

            String[] arrItem = condition.Split( ' ' );
            foreach (String str in arrItem) {
                if (str.IndexOf( '.' ) < 0) continue;

                String[] arr = getPropertyItems( str );
                result.Add( arr );

            }


            return result;
        }


        public static String[] getPropertyItems( String str ) {

            String[] arr = new string[2];

            String key = "";
            String val = "";
            Boolean valBegin = false;
            char[] arrChar = str.ToCharArray();
            foreach (char ch in arrChar) {

                if( ch == ' ' ) continue;

                if (ch == '.') {
                    valBegin = true;
                    continue;
                }

                if (valBegin == false) {

                    if (isAbcAndNumber( ch ) == false) {
                        key = "";
                        continue;
                    }
                    key += ch;

                }
                else {

                    if (isAbcAndNumber( ch ) == false) {
                        break;
                    }

                    val += ch;

                }

            }

            arr[0] = key;
            arr[1] = val;



            return arr;
        }


        private static Boolean isAbcAndNumber( char ch ) {
            return "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".IndexOf( ch ) >= 0;
        }

    }
}
