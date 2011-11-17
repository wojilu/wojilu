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

namespace wojilu.Web.Mvc.Routes {

    internal class RouteParser {

        public static List<RouteSetting> ParseConfig( String configContent ) {

            List<RouteSetting> results = new List<RouteSetting>();
            if (strUtil.IsNullOrEmpty( configContent )) return results;

            String[] arrLines = configContent.Trim().Split( '\n' );
            foreach (String line in arrLines) {

                if (strUtil.IsNullOrEmpty( line )) continue;
                if (line.Trim().StartsWith( "//" )) continue;

                RouteSetting route = getRoute( line );
                results.Add( route );
            }

            return results;
        }

        private static RouteSetting getRoute( String line ) {

            String[] arrItem = line.Trim().Split( new char[] { ';' } );

            String url = strUtil.TrimStart( arrItem[0], "url=>" ).Trim();
            RouteSetting setting = new RouteSetting();
            setting.setPath( url );
            setting.SplitPath();

            if ((arrItem.Length > 1) ) {
                addRouteInfo( arrItem, setting );
            }

            return setting;
        }

        private static void addRouteInfo( String[] arrItem, RouteSetting setting ) {

            Dictionary<String, RouteOtherInfo> dic = new Dictionary<String, RouteOtherInfo>();

            for (int i = 1; i < arrItem.Length; i++) {

                if (strUtil.IsNullOrEmpty( arrItem[i] )) continue;

                RouteOtherInfo info = RouteOtherInfo.Parse( arrItem[i] );
                if (info != null) dic.Add( info.getName(), info );

            }

            setting.setRouteOtherInfo( dic );

            if (dic.ContainsKey( "default" )) {
                setting.setDefaultValue( RouteOtherInfo.getDefaultValue( dic["default"] ) );
            }

        }


    }
}

