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
using System.Text;

using wojilu.Web.Context;
using wojilu.Caching;

namespace wojilu.Web.Mvc.Routes {

    /// <summary>
    /// 路由解析工具
    /// </summary>
    public class RouteTool {

        private static readonly ILog logger = LogManager.GetLogger( typeof( RouteTool ) );

        public static readonly char[] Separator = getSeparator();

        private static char[] getSeparator() {
            if (MvcConfig.Instance.UrlSeparator == "/") return new char[] { '/' };
            return new char[] { '/', MvcConfig.Instance.UrlSeparator[0] };
        }

        public static Route Recognize( MvcContext context ) {
            String url = context.url.ToString();
            return RecognizePath( processCleanUrl( url, context.web.PathApplication ) );
        }

        public static Route Recognize( String fullUrlPath, String applicationPath ) {
            return RecognizePath( processCleanUrl( fullUrlPath, applicationPath ) );
        }

        /// <summary>
        /// url 必须没有后缀名
        /// </summary>
        /// <param name="cleanUrl">必须没有后缀名</param>
        /// <returns></returns>
        public static Route RecognizePath( String cleanUrl ) {

            if (strUtil.IsNullOrEmpty( cleanUrl ) || cleanUrl.ToLower().Equals( "default" )) {
                cleanUrl = "default";
            }

            logger.Info( "RecognizePath begin, clearnUrl=" + cleanUrl );

            Route x = LinkMap.Parse( cleanUrl );
            if (x != null) return x;

            if (cleanUrl.StartsWith( "/" )) cleanUrl = strUtil.TrimStart( cleanUrl, "/" );

            String[] arrPathRow = cleanUrl.Split( Separator );

            foreach (RouteSetting route in RouteTable.GetRoutes()) {

                if (!isMatched( route, arrPathRow )) continue;


                RouteParseInfo routeInfo = new RouteParseInfo( MemberPath.getOwnerInfo( arrPathRow ), arrPathRow );
                processOwnerNamespaceAppId( routeInfo, route );

                Route result = getResult( route, routeInfo );

                if (result == null) continue;

                setRouteInfo( result, routeInfo, cleanUrl );

                return result;
            }

            return null;
        }

        private static Boolean isMatched( RouteSetting setting, String[] arrPath ) {

            if (arrPath.Length == 0) return false;

            if (setting.IsNamespaceIncluded()) return true;

            if (arrPath.Length > setting.GetPathItems().Count) return false;

            for (int i = 0; i < setting.GetPathItems().Count; i++) {

                if (i > arrPath.Length - 1) break;

                PathItem item = setting.GetPathItems()[i];
                if (!item.isNamed() && !item.getName().Equals( arrPath[i] )) return false;

            }
            return true;
        }

        private static String processCleanUrl( String urlPath, String applicationPath ) {

            String path;

            if (urlPath.StartsWith( "http://" ) ||
                urlPath.StartsWith( "https://" )
                ) {
                UriBuilder builder = new UriBuilder( urlPath );
                path = builder.Path;
            }
            else {
                path = urlPath;
            }

            String srcString = strUtil.TrimEnd( path, RouteConfig.Instance.urlExt() );
            if (strUtil.IsNullOrEmpty( RouteConfig.Instance.urlExt() )) srcString = strUtil.TrimEnd( srcString, ".aspx" );
            if (srcString.ToLower().StartsWith( applicationPath.ToLower() )) {
                srcString = srcString.Substring( applicationPath.Length );
            }
            return strUtil.TrimStart( srcString, "/" );
        }


        private static void processOwnerNamespaceAppId( RouteParseInfo routeInfo, RouteSetting route ) {

            String[] arrPathRow = routeInfo.getRowPathArray();

            int count = route.GetNamedItem().Count;
            if (count == 0) {
                routeInfo.setPathArray( arrPathRow );
                return;
            }

            String[] arrTemp;

            // 首先前面剔除掉 owner
            if (routeInfo.getOwnerInfo() != null) {
                arrTemp = new String[arrPathRow.Length - 2];
                for (int i = 2; i < arrPathRow.Length; i++) {
                    arrTemp[i - 2] = arrPathRow[i];
                }
            }
            else {
                arrTemp = arrPathRow;
            }


            if (arrTemp.Length <= count) {
                routeInfo.setPathArray( arrTemp );
                return;
            }

            // 然后末尾剔除掉 controller/action/id
            String[] arrPath = new String[count];
            int x = 0;
            for (int i = arrTemp.Length - count; i < arrTemp.Length; i++) {
                arrPath[x] = arrTemp[i];
                x++;
            }

            // 得到 ns
            StringBuilder ns = new StringBuilder();
            int end = arrTemp.Length - count;
            for (int i = 0; i < end; i++) {

                if (i > 0) ns.Append( "." );

                if (i == 0 && isEndsWithInt( arrTemp[i] )) {
                    routeInfo.setAppId( strUtil.GetEndNumber( arrTemp[i] ) );

                    ns.Append( strUtil.TrimEnd( arrTemp[i], routeInfo.getAppId().ToString() ) );

                }
                else
                    ns.Append( arrTemp[i] );
            }


            routeInfo.setPathArray( arrPath );
            routeInfo.setNamespace( ns );

        }


        private static Route getResult( RouteSetting setting, RouteParseInfo routeInfo ) {

            Route result = setting.getRouteWithDefaultValue();
            List<PathItem> items = setting.GetNamedItem();


            for (int i = 0; i < items.Count; i++) {

                String[] arrValue = routeInfo.getPathArray();

                if (i > arrValue.Length - 1) break;

                PathItem item = items[i];
                if (item.isNamed() == false) continue;

                String val = arrValue[i];
                if (strUtil.IsNullOrEmpty( val )) continue;

                // 如果条件不符合，跳过此条route
                if (setting.getRequirements().match( item.getName(), val ) == false) return null;

                result.setItem( item.getName(), val );
            }

            return result;
        }


        private static Boolean isEndsWithInt( String item ) {
            return char.IsDigit( item[item.Length - 1] );
        }

        //----------------------------------------------------------------------------------------------------------------------

        private static void setRouteInfo( Route result, RouteParseInfo routeInfo, String cleanUrl ) {

            if (strUtil.IsNullOrEmpty( result.ns ))
                result.setNs( routeInfo.getNamespace().ToString() );
            if (result.ns.Equals( "_" ))
                result.setNs( "" );

            if (routeInfo.getAppId() > 0) {
                result.setItem( "appid", routeInfo.getAppId() );
            }

            if (routeInfo.getOwnerInfo() != null) {
                result.setItem( "owner", routeInfo.getOwnerInfo().Owner );
                result.setItem( "ownertype", routeInfo.getOwnerInfo().OwnerType );
            }

            result.setCleanUrl( cleanUrl );
            setStrongTypeValue( result );
            setDefaultAction( result );
        }

        private static void setStrongTypeValue( Route result ) {

            if (result.getItem( "controller" ) != null) result.setController( result.getItem( "controller" ) );
            if (result.getItem( "action" ) != null) result.setAction( result.getItem( "action" ) );
            if (result.getItem( "query" ) != null) result.setQuery( result.getItem( "query" ) );
            if (result.getItem( "owner" ) != null) result.setOwner( result.getItem( "owner" ) );
            if (result.getItem( "ownertype" ) != null) result.setOwnerType( result.getItem( "ownertype" ) );
            if (result.getItem( "id" ) != null) result.setId( cvt.ToLong( result.getItem( "id" ) ) );
            if (result.getItem( "appid" ) != null) result.setAppId( cvt.ToLong( result.getItem( "appid" ) ) );

            int page = 1;
            if (result.getItem( "page" ) != null) {
                int routePage = Requirements.getPageNumber( result.getItem( "page" ) );
                if (routePage > 1) page = routePage;
            }

            result.setItem( "page", page );
            result.setPage( page );
            CurrentRequest.setCurrentPage( page );
        }

        private static void setDefaultAction( Route route ) {

            if (strUtil.HasText( route.action )) return;

            if (route.id > 0)
                route.setAction( "Show" );
            else
                route.setAction( "Index" );
        }


    }
}

