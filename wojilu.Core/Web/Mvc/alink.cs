/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Context;
using wojilu.Members.Interface;
using wojilu.Members.Sites.Domain;
using wojilu.Common.MemberApp.Interface;
using wojilu.Common.AppBase.Interface;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Mvc {

    public class alink {

        public static String ToAppAdmin( IMember user, IMemberApp app ) {
            String ownerPath = Link.GetMemberPathPrefix( user );
            String appName = strUtil.TrimEnd( app.AppInfo.TypeName, "App" );
            String controller = appName + "/Admin/" + appName;
            return Link.AppendApp( app.AppOid, controller, "Index", -1, ownerPath );
        }

        public static String ToAppData( IAppData data ) {

            String controllerPath = getAppDataController( data.GetType().FullName, data.AppId );

            return To( data, controllerPath, "Show", data.Id );
        }

        private static String getAppDataController( String typeFullName, int appId ) {

            String typeName = strUtil.GetTypeName( typeFullName );

            string[] arrItem = typeFullName.Split( '.' );
            if (arrItem.Length < 3) throw new Exception( "domain namespace is contrary to convention" ); // app.domain.model => 1.app 2.domain 3.model

            String appNamespace = arrItem[arrItem.Length - 3];
            String appName = strUtil.TrimEnd( appNamespace, "App" );
            String controllerName = strUtil.TrimStart( typeName, appName ) + "Controller";

            return appName + "/" + controllerName;
        }

        private static String To( IAppData data, String controller, String action, int id ) {
            String ownerPath = Link.GetMemberPathPrefix( data.OwnerType, data.OwnerUrl );
            return Link.AppendApp( data.AppId, controller, action, id, ownerPath );
        }

        private static String _app {
            get { return sys.Path.Root; }
        }

        //----------------------------------------------------------------------------------------------------------------

        public static String ToUserAppFull( IMemberApp app ) {

            String strApp = strUtil.TrimEnd( app.AppInfo.TypeName, "App" );
            return getAppLink( app.OwnerType, app.OwnerUrl, strApp, app.AppOid );
        }

        /// <summary>
        /// 最后的效果包括后缀名，比如 /Forum1/Forum/Index.aspx
        /// </summary>
        /// <param name="app"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static String ToApp( IApp app, MvcContext ctx ) {

            String appName = strUtil.TrimEnd( app.GetType().Name, "App" );
            return getAppLink( app.OwnerType, app.OwnerUrl, appName, app.Id );
        }

        private static String getAppLink( String ownerTypeFull, String ownerUrl, String appName, int appId ) {

            String result = Link.GetMemberPathPrefix( ownerTypeFull, ownerUrl );

            result = strUtil.Join( result, appName );
            if (appId > 0) result = result + appId;

            result = strUtil.Join( result, appName );
            return strUtil.Join( result, "Index" ) + MvcConfig.Instance.UrlExt;
        }

        //private static String getOwnerUrl( String ownerTypeFull, String ownerUrl ) {

        //    if (ownerTypeFull.Equals( typeof( Site ).FullName )) return ownerUrl;

        //    String result = strUtil.GetTypeName( ownerTypeFull ).ToLower();
        //    result = MemberPath.GetPath( result );
        //    return strUtil.Join( result, ownerUrl );
        //}

        //----------------------------------------------------------------------------------------------------------------

        public static String ToUserMicroblog( IMember user ) {

            if (Link.IsMemberSubdomain( typeof( User ).FullName )) {
                return "http://" + user.Url + "." + SystemInfo.HostNoSubdomain + "/t" + MvcConfig.Instance.UrlExt;
            }
            else {
                return strUtil.Join( _app, "t/" ) + user.Url + MvcConfig.Instance.UrlExt;
            }
        }




        public static string ToMicroblog() {
            return strUtil.Join( _app, "t" ) + MvcConfig.Instance.UrlExt;
        }

        public static String ToTag( String tagName ) {
            return strUtil.Join( _app, "tag/" ) + tagName + MvcConfig.Instance.UrlExt;
        }



    }

}
