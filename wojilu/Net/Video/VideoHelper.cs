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

namespace wojilu.Net.Video {

    public class VideoHelper {


        public static String GetTitle( String title ) {

            String t = title;

            if (t.IndexOf( '-' ) > 0) {
                t = getItemFirst( t, '-' );
            }

            if (t.IndexOf( '_' ) > 0) {
                t = getItemFirst( t, '_' );
            }

            return t;
        }

        private static String getItemFirst( String t, char s ) {
            String[] arrItem = t.Split( s );
            if (arrItem.Length > 0) {
                return arrItem[0].Trim();
            }
            return t;
        }

    }

}
