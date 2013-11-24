using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Interface;
using wojilu.Web.Mvc.Routes;

namespace wojilu.Web.Mvc {

    public class LinkMap {

        private static String separator = MvcConfig.Instance.UrlSeparator;
        private static Boolean isLinkToLow = MvcConfig.Instance.IsUrlToLower;

        private static readonly Dictionary<String, String> _map = new Dictionary<String, String>();
        private static readonly Dictionary<String, String> _shortUrlMap = new Dictionary<String, String>();

        private static Dictionary<String, String> getMap() {
            return _map;
        }

        public static void Add( String key, String val ) {
            _map.Add( key, val );
        }

        public static void ShortUrl( String key, String url ) {
            _shortUrlMap.Add( key, url );
        }

        public static void Clear() {
            _map.Clear();
            _shortUrlMap.Clear();
        }

        public static void SetLinkToLow( Boolean isToLow ) {
            isLinkToLow = isToLow;
        }

        internal static Dictionary<String, String> GetShortUrlMap() {
            return _shortUrlMap;
        }

        //-------------------------------------------------------------------------


        public static String To( IMember member, aActionWithId action, long id, long appId ) {
            String ownerPath = LinkHelper.GetMemberPathPrefix( member );
            String url = toPrivate( action, id );
            if (url == null) return null;
            url = LinkHelper.addAppId( url, appId );
            return appendExt( strUtil.Join( ownerPath, url ) );
        }

        public static string To( string memberType, string memberUrl, aActionWithId action, long id, long appId ) {
            String ownerPath = LinkHelper.GetMemberPathPrefix( memberType, memberUrl );
            String url = toPrivate( action, id );
            if (url == null) return null;
            url = LinkHelper.addAppId( url, appId );
            return appendExt( strUtil.Join( ownerPath, url ) );
        }

        public static String To( IMember member, aAction action, long appId ) {
            String ownerPath = LinkHelper.GetMemberPathPrefix( member );
            String url = toPrivate( action );
            if (url == null) return null;
            url = LinkHelper.addAppId( url, appId );
            return appendExt( strUtil.Join( ownerPath, url ) );
        }

        public static String To( IMember member, String aController, String action, long id, long appId ) {

            String ownerPath = LinkHelper.GetMemberPathPrefix( member );
            String url = toPrivate( aController, action, id );
            if (url == null) return null;
            url = LinkHelper.addAppId( url, appId );
            return appendExt( strUtil.Join( ownerPath, url ) );
        }

        public static String To( IMember member, String aController, String action, long appId ) {
            String ownerPath = LinkHelper.GetMemberPathPrefix( member );
            String url = toPrivate( aController, action );
            if (url == null) return null;
            url = LinkHelper.addAppId( url, appId );
            return appendExt( strUtil.Join( ownerPath, url ) );
        }

        public static string To( string memberType, string memberUrl, String aController, String action, long appId ) {
            String ownerPath = LinkHelper.GetMemberPathPrefix( memberType, memberUrl );
            String url = toPrivate( aController, action );
            if (url == null) return null;
            url = LinkHelper.addAppId( url, appId );
            return appendExt( strUtil.Join( ownerPath, url ) );

        }

        public static string To( string memberType, string memberUrl, String aController, String action, long id, long appId ) {
            String ownerPath = LinkHelper.GetMemberPathPrefix( memberType, memberUrl );
            String url = toPrivate( aController, action, id );
            if (url == null) return null;
            url = LinkHelper.addAppId( url, appId );
            return appendExt( strUtil.Join( ownerPath, url ) );

        }

        //-------------------------------------------------------------------------

        public static String To( String aController, String action, long id ) {
            return appendExt( toPrivate( aController, action, id ) );
        }

        private static String toPrivate( String aController, String action, long id ) {
            Dictionary<String, String> map = getMap();
            if (map.Count == 0) return null;

            String actionName = isLinkToLow ? action.ToLower() : action;

            return getLinkActionString( map, aController, actionName, id );
        }

        private static string getLinkActionString( Dictionary<String, String> map, String aController, String actionName, long id ) {
            foreach (String aNamespace in MvcConfig.Instance.RootNamespace) {

                String controller = strUtil.Join( aNamespace, aController.Replace( "/", "." ), "." );
                if (controller.EndsWith( "Controller" ) == false) controller = controller + "Controller";

                String ret = getLinkActionStr( map, controller, actionName, id );
                if (ret != null) return ret;
            }

            return null;
        }


        public static String To( aActionWithId action, long id ) {

            return appendExt( toPrivate( action, id ) );

        }

        private static String toPrivate( aActionWithId action, long id ) {
            Dictionary<String, String> map = getMap();
            if (map.Count == 0) return null;

            String controller = action.Target.GetType().FullName;
            String actionName = isLinkToLow ? action.Method.Name.ToLower() : action.Method.Name;

            return getLinkActionStr( map, controller, actionName, id );
        }

        private static String appendExt( String url ) {

            if (url == null) return null;

            if (strUtil.HasText( MvcConfig.Instance.UrlExt )) {
                url = url + MvcConfig.Instance.UrlExt;
            }

            return url;
        }

        private static string getLinkActionStr( Dictionary<String, String> map, String controller, String actionName, long id ) {

            String strControllerAndAction = controller + "." + actionName;

            // 1)
            String path = getPathFromMapWidthAction( map, strControllerAndAction );
            if (path != null) {
                // append id
                if (id > 0) {
                    return addSlash( path ) + separator + id;
                }
                else {
                    return addSlash( path );
                }
            }

            // 2)
            path = getPathFromMap( map, controller );
            if (path != null) {
                // append id
                if (id > 0) {
                    return addSlash( path ) + separator + actionName + separator + id;
                }
                else {
                    return addSlash( path ) + separator + actionName;
                }
            }

            // 3)
            return null;
        }



        public static String To( String aController, String action ) {

            return appendExt( toPrivate( aController, action ) );
        }


        private static String toPrivate( String aController, String action ) {

            Dictionary<String, String> map = getMap();
            if (map.Count == 0) return null;

            String actionName = isLinkToLow ? action.ToLower() : action;

            return getLinkActionString( map, aController, actionName );
        }

        private static string getLinkActionString( Dictionary<String, String> map, String aController, String actionName ) {
            foreach (String aNamespace in MvcConfig.Instance.RootNamespace) {

                String controller = strUtil.Join( aNamespace, aController.Replace( "/", "." ), "." );
                if (controller.EndsWith( "Controller" ) == false) controller = controller + "Controller";

                String ret = getLinkActionStr( map, controller, actionName );
                if (ret != null) return ret;
            }

            return null;
        }

        public static String To( aAction action ) {

            return appendExt( toPrivate( action ) );
        }

        private static String toPrivate( aAction action ) {

            Dictionary<String, String> map = getMap();
            if (map.Count == 0) return null;

            String actionName = isLinkToLow ? action.Method.Name.ToLower() : action.Method.Name;

            String controller = action.Target.GetType().FullName;

            return getLinkActionStr( map, controller, actionName );
        }

        private static string getLinkActionStr( Dictionary<String, String> map, String controller, String actionName ) {
            String strControllerAndAction = controller + "." + actionName;

            // 1)
            String path = getPathFromMapWidthAction( map, strControllerAndAction );
            if (path != null) return addSlash( path );

            // 2)
            path = getPathFromMap( map, controller );
            if (path != null) return addSlash( path ) + separator + actionName;

            // 3)
            return null;
        }

        private static String addSlash( String path ) {
            if (path.StartsWith( "/" )) return path;
            return "/" + path;
        }

        private static String getPathFromMap( Dictionary<String, String> map, String controller ) {
            foreach (KeyValuePair<String, String> kv in map) {
                if (kv.Value.Equals( controller )) return kv.Key;
            }
            return null;
        }

        private static String getPathFromMapWidthAction( Dictionary<String, String> map, String controllerAndAction ) {
            foreach (KeyValuePair<String, String> kv in map) {
                if (isLinkToLow) {
                    if (strUtil.EqualsIgnoreCase( kv.Value, controllerAndAction )) return kv.Key;
                }
                else {
                    if (kv.Value.Equals( controllerAndAction )) return kv.Key;
                }
            }
            return null;
        }

        public static Route Parse( String cleanUrl ) {

            Dictionary<String, String> map = getMap();
            if (map.Count == 0) return null;

            cleanUrl = cleanUrl.Trim().TrimStart( separator[0] );

            String[] arrPathRow = cleanUrl.Split( RouteTool.Separator );
            OwnerInfo owner = MemberPath.getOwnerInfo( arrPathRow );

            if (owner == null) {
                return processController( parseRoute( cleanUrl ), cleanUrl );
            }
            else {

                String urlWithoutOwnerInfo = getUrlWithoutOwnerInfo( arrPathRow );
                Route x = parseRoute( urlWithoutOwnerInfo );
                if (x == null) return null;

                x.setOwner( owner.Owner );
                x.setOwnerType( owner.OwnerType );
                return processController( x, cleanUrl );
            }

        }

        private static Route processController( Route route, String cleanUrl ) {

            if (route == null || route.controller == null) return route;

            route.setCleanUrl( cleanUrl );

            if (route.owner == null) {

                route.setOwner( "site" );
                route.setOwnerType( "site" );
            }

            foreach (String aNamespace in MvcConfig.Instance.RootNamespace) {

                if (route.controller.StartsWith( aNamespace )) {

                    String cleanNs = strUtil.TrimStart( route.controller, aNamespace ).TrimStart( '.' );

                    String[] arrItem = cleanNs.Split( '.' );
                    String controller = arrItem[arrItem.Length - 1];
                    String ns = strUtil.TrimEnd( cleanNs, controller ).TrimEnd( '.' );

                    controller = strUtil.TrimEnd( controller, "Controller" );

                    route.setController( controller );
                    route.setNs( ns );
                    route.setRootNamespace( aNamespace );

                    return route;

                }

            }

            return route;
        }

        private static String getUrlWithoutOwnerInfo( String[] arrPathRow ) {
            StringBuilder sb = new StringBuilder();
            for (int i = 2; i < arrPathRow.Length; i++) {
                sb.Append( arrPathRow[i] );
                sb.Append( separator );
            }
            return sb.ToString().TrimEnd( separator[0] );
        }

        private static Boolean isPageNumber( String val ) {
            return strUtil.HasText( val ) && val.Length > 1 && val.StartsWith( "p" ) && cvt.IsInt( val.Substring( 1 ) );
        }

        private static Route parseRoute( String apath ) {

            if (apath == null) return new Route();

            Route x = new Route();

            apath = processPageIndex( apath, x );

            PathAppId xPath = processAppId( apath.TrimStart( '/' ) );
            String path = xPath.Path;

            x.setAppId( xPath.AppId );

            Dictionary<String, String> map = getMap();

            foreach (KeyValuePair<String, String> kv in map) {

                if (isLinkToLow) {

                    if (strUtil.EqualsIgnoreCase( path, kv.Key )) {
                        // path=category
                        // map.Add( "category", "wojilu.Test.Web.Mvc.TestPostController.List" ); 
                        return getEqualMap( kv, x );
                    }

                    if (path.ToLower().StartsWith( kv.Key.ToLower() + separator )) {
                        return getParseResult( path, kv.Value, x );
                    }

                }
                else {

                    if (path.Equals( kv.Key )) {
                        // path=category
                        // map.Add( "category", "wojilu.Test.Web.Mvc.TestPostController.List" ); 
                        return getEqualMap( kv, x );
                    }

                    if (path.StartsWith( kv.Key + separator )) {
                        return getParseResult( path, kv.Value, x );
                    }


                }


            }

            return null;
        }

        private static String processPageIndex( String apath, Route x ) {
            String[] arrP = apath.Split( separator[0] );
            String lastItem = arrP[arrP.Length - 1];
            if (isPageNumber( lastItem )) {

                int pageIndex = cvt.ToInt( lastItem.TrimStart( 'p' ) );
                x.setPage( pageIndex );
                CurrentRequest.setCurrentPage( pageIndex );

                apath = strUtil.TrimEnd( apath, lastItem ).TrimEnd( separator[0] );

            }
            return apath;
        }

        private static PathAppId processAppId( String path ) {

            PathAppId x = new PathAppId();

            String[] arr = path.Split( separator[0] );
            String firstItem = arr[0];

            if (isEndsWithInt( firstItem )) {
                x.AppId = strUtil.GetEndNumber( firstItem );

                String firstWithoutAppId = strUtil.TrimEnd( firstItem, x.AppId.ToString() );
                x.Path = firstWithoutAppId + strUtil.TrimStart( path, firstItem );
            }
            else {
                x.Path = path;
            }

            return x;
        }

        private static Boolean isEndsWithInt( String item ) {
            return char.IsDigit( item[item.Length - 1] );
        }


        private static Route getParseResult( String path, String mapController, Route x ) {

            String[] arr = path.Split( separator[0] );
            if (arr.Length == 1) {
                return null;
            }
            else if (arr.Length == 2) {

                Boolean isMapController = mapController.EndsWith( "Controller" );
                if (isMapController) {
                    // path=post/Index
                    // map.Add( "post", "wojilu.Test.Web.Mvc.TestPostController" );                        
                    return parseControllerAndAction( arr, mapController, x );
                }
                else {
                    // path=product/99
                    // map.Add( "product", "wojilu.Test.Web.Mvc.TestPostController.Product" );
                    return parseCAndAId( arr, mapController, x );
                }

            }
            else if (arr.Length == 3) {
                // path=post/Show/88
                // map.Add( "post", "wojilu.Test.Web.Mvc.TestPostController" ); 
                return parseActionId( arr, mapController, x );
            }
            else {
                //throw new Exception( "path过长，请勿超过3项" );
                return null;
            }
        }

        private static Route parseCAndAId( String[] arrPath, String mapController, Route x ) {

            String[] arrValue = mapController.Split( '.' );
            String action = arrValue[arrValue.Length - 1];
            String controller = strUtil.TrimEnd( mapController, action ).TrimEnd( '.' );
            x.setController( controller );
            x.setAction( action );
            x.setId( cvt.ToInt( arrPath[1] ) );

            return x;
        }

        private static Route parseControllerAndAction( String[] arrPath, String mapController, Route x ) {

            String action = arrPath[1];
            x.setController( mapController );
            x.setAction( action );

            return x;

        }


        private static Route parseActionId( String[] arrPath, String mapController, Route x ) {

            String id = arrPath[2];
            if (cvt.IsInt( id ) == false) return null;

            String action = arrPath[1];
            x.setController( mapController );
            x.setAction( action );
            x.setId( cvt.ToInt( arrPath[2] ) );

            return x;
        }

        private static Route getEqualMap( KeyValuePair<String, String> kv, Route x ) {
            String[] arr = kv.Value.Split( '.' );
            String action = arr[arr.Length - 1];
            String controller = strUtil.TrimEnd( kv.Value, action ).TrimEnd( '.' );
            x.setController( controller );
            x.setAction( action );
            return x;
        }




    }

    public class PathAppId {
        public String Path;
        public int AppId;
    }


}
