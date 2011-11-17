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

namespace wojilu.Web.Mvc.Routes {

    public class RouteOtherInfo {

        private String _name;
        public String getName() { return _name; }
        public void setName( String name ) { _name = name; }

        private Dictionary<String, String> _items;
        public Dictionary<String, String> getItems() { return _items; }
        public void setItems( Dictionary<String, String> dic ) { _items = dic; }

        public static DefaultRouteValue getDefaultValue( RouteOtherInfo info ) {


            Dictionary<String, String> values = info.getItems();

            DefaultRouteValue result = new DefaultRouteValue();

            if (values.ContainsKey( "ownertype" )) result.setOwnerType( values["ownertype"] );
            if (values.ContainsKey( "owner" )) result.setOwner( values["owner"] );
            if (values.ContainsKey( "ns" )) result.setNs( values["ns"] );
            if (values.ContainsKey( "controller" )) result.setController( values["controller"] );
            if (values.ContainsKey( "action" )) result.setAction( values["action"] );
            if (values.ContainsKey( "query" )) result.setQuery( values["query"] );
            if (values.ContainsKey( "id" )) result.setId( cvt.ToInt( values["id"] ) );
            if (values.ContainsKey( "appId" )) result.setAppId( cvt.ToInt( values["appId"] ) );
            if (values.ContainsKey( "page" )) result.setPage( cvt.ToInt( values["page"] ) );

            return result;
        }

        public static RouteOtherInfo Parse( String str ) {

            if (strUtil.IsNullOrEmpty( str )) return null;

            String[] arrItem = str.Split( ':' );
            if (arrItem.Length != 2) return null;

            String name = arrItem[0].Trim();
            String settingStr = arrItem[1].Trim();
            if (strUtil.IsNullOrEmpty( name ) || strUtil.IsNullOrEmpty( settingStr )) return null;
            settingStr = settingStr.TrimStart( '{' ).TrimEnd( '}' ).Trim();
            if (strUtil.IsNullOrEmpty( settingStr )) return null;

            //-----------------------------

            RouteOtherInfo result = new RouteOtherInfo();
            result.setName( name );

            String[] arrSettings = settingStr.Split( ',' );

            Dictionary<String, String> dic = new Dictionary<String, String>();
            foreach (String item in arrSettings) {

                if (strUtil.IsNullOrEmpty( item )) continue;

                String[] arrPair = item.Trim().Split( '=' );
                if (arrPair.Length != 2) continue;

                String key = arrPair[0].Trim();
                String val = arrPair[1].Trim();

                if (strUtil.IsNullOrEmpty( key ) ) continue;
                if (strUtil.IsNullOrEmpty( val )) val = "";
                dic.Add( key, val );

            }

            if (dic.Count == 0) return null;

            result.setItems( dic );

            return result;
        }

    }

}

