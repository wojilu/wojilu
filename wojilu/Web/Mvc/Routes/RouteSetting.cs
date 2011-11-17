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

    /// <summary>
    /// 所有路由数据
    /// </summary>
    public class RouteSetting {

        private List<PathItem> _itemsAll = new List<PathItem>();
        private DefaultRouteValue _default = new DefaultRouteValue();
        private Dictionary<String, RouteOtherInfo> _otherInfos = new Dictionary<String, RouteOtherInfo>();
        private List<PathItem> _itemsNamed;

        private String _path;
        public String getPath() { return _path; }
        public void setPath( String path ) { _path = path; }

        public Boolean IsNamespaceIncluded() {
            return this._path.StartsWith( "~/" );
        }

        public void setRouteOtherInfo( Dictionary<String, RouteOtherInfo> info ) {
            _otherInfos = info;
        }

        public Requirements getRequirements() {
            if (_otherInfos.ContainsKey( "requirements" )) {
                RouteOtherInfo info = _otherInfos["requirements"];
                Requirements result = new Requirements();
                result.setDic( info.getItems() );
                return result;
            }
            return new Requirements();
        }


        public List<PathItem> GetPathItems() {
            return _itemsAll;
        }

        public DefaultRouteValue getDefaultValue() { return _default; }
        public void setDefaultValue( DefaultRouteValue dvalue ) { _default = dvalue; }

        public List<PathItem> GetNamedItem() {
            return _itemsNamed;
        }

        public Boolean OnlyHasNamedItems() {
            return (this.GetPathItems().Count == this.GetNamedItem().Count);
        }

        public RouteSetting SplitPath() {

            String pathWithoutNs = strUtil.TrimStart( this._path, "~/" );

            String[] strArray = pathWithoutNs.Split( RouteTool.Separator );
            foreach (String str in strArray) {

                if (strUtil.IsNullOrEmpty( str )) continue;

                PathItem item = new PathItem();
                if ((str[0] == NamePrefix) && (str[str.Length - 1] == NamePostfix)) {
                    item.setName( str.Substring( 1, str.Length - 2 ) );
                    item.isNamed( true );
                }
                else {
                    item.setName( str );
                    item.isNamed( false );
                }
                _itemsAll.Add( item );
            }

            initItemsNamed();

            return this;
        }

        public Route getRouteWithDefaultValue() {

            Route route = new Route();

            route.setOwnerType ( _default.getOwnerType());
            route.setOwner ( _default.getOwner());
            route.setNs( _default.getNs() );
            route.setController ( _default.getController());
            route.setAction ( _default.getAction());
            route.setQuery ( _default.getQuery());
            route.setId( _default.getId() );
            route.setAppId( _default.getAppId() );
            route.setPage( _default.getPage() );

            route.setItem( "ownertype", route.ownerType );
            route.setItem( "owner", route.owner );
            route.setItem( "namespace", route.ns );
            route.setItem( "controller", route.controller );
            route.setItem( "action", route.action );
            route.setItem( "query", route.query );
            route.setItem( "id", route.id );
            route.setItem( "appId", route.appId );
            route.setItem( "page", route.page );

            return route;
        }

        private void initItemsNamed() {
            _itemsNamed = new List<PathItem>();
            foreach (PathItem item in this.GetPathItems()) {
                if (item.isNamed()) _itemsNamed.Add( item );
            }
        }

        public static readonly char NamePrefix = '{';
        public static readonly char NamePostfix = '}';




    }
}

