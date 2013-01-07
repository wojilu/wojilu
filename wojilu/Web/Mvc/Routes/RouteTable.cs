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
using System.Web;
using System.Collections.Generic;

using wojilu.IO;
using System.Text;

namespace wojilu.Web.Mvc.Routes {

    /// <summary>
    /// 路由表
    /// </summary>
    public sealed class RouteTable {

        private static volatile List<RouteSetting> routeTable;
        private static Object _syncRoot = new object();

        private static String _routeTableString;

        public static List<RouteSetting> GetRoutes() {
            if (routeTable == null) {
                lock (_syncRoot) {
                    if (routeTable == null) routeTable = loadRouteTable();
                }
            }
            return routeTable;

        }

        public static void Init( String configContent ) {
            routeTable = RouteParser.ParseConfig( configContent );
        }

        public static void UpdateFriendUrl( String furl ) {
            if (_routeTableString == null) throw new Exception( "_routeTableString not initialized" );

            String furlSetting = getFriendUrlSetting( furl );

            String[] arrLines = _routeTableString.Trim().Split( '\n' );
            int insertLineNo = 0;
            for (int i = 0; i < arrLines.Length; i++) {
                String line = arrLines[i];

                if (strUtil.IsNullOrEmpty( line )) continue;
                if (line.Trim().StartsWith( "//" )) continue;

                if (line.Trim().Equals( furlSetting )) {
                    return; // 配置已经存在，直接返回
                }

                if (line.Trim().StartsWith( "{owner}" )) {
                    insertLineNo = i;
                }
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < arrLines.Length; i++) {
                if (i == insertLineNo) {
                    sb.Append( furlSetting );
                    sb.AppendLine();
                }
                sb.Append( arrLines[i].Trim() );
                sb.AppendLine();
            }
            _routeTableString = sb.ToString();

            file.Write( RouteConfig.Instance.getConfigPath(), _routeTableString );
            routeTable = RouteParser.ParseConfig( _routeTableString );
        }

        private static String getFriendUrlSetting( String furl ) {
            return furl + ";default:{ownertype=site,owner=site}";
        }

        private static List<RouteSetting> loadRouteTable() {
            _routeTableString = loadConfig();
            return RouteParser.ParseConfig( _routeTableString );
        }

        private static String loadConfig() {

            String configString = "";

            String cfgAbsPath = RouteConfig.Instance.getConfigPath();
            if (file.Exists( cfgAbsPath )) {
                configString = File.Read( cfgAbsPath );
            }

            if (strUtil.IsNullOrEmpty( configString )) configString = getDefaultRouteConfig();

            return configString;
        }

        private static String getDefaultRouteConfig() {

            return @"
~/{controller}/{id};requirements:{id=int}
~/{controller}/{action};requirements:{controller=letter,action=letter}
~/{controller}/{id}/{action};requirements:{controller=letter,id=int,action=letter}
~/{controller}/{action}/{page};requirements:{controller=letter,action=letter,page=page}
~/{controller}/{id}/{page};requirements:{controller=letter,id=int,page=page}
~/{controller}/{id}/{action}/{page};requirements:{controller=letter,id=int,action=letter,page=page}
";

        }

        private RouteTable() { }

        public static void Reset() {
            routeTable = null;
        }

    }
}

