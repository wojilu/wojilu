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
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Mvc {

    [MvcLink]
    public class alink {

        public static String ToAppAdmin( IMember user, IMemberApp app ) {
            String ownerPath = LinkHelper.GetMemberPathPrefix( user );
            String appName = strUtil.TrimEnd( app.AppInfo.TypeName, "App" );
            String controller = appName + MvcConfig.Instance.UrlSeparator + "Admin" + MvcConfig.Instance.UrlSeparator + appName;
            return LinkHelper.AppendApp( app.AppOid, controller, "Index", -1, ownerPath );
        }

        public static String ToAppData( IAppData data ) {
            return ToAppData( data, null );
        }

        public static String ToAppData( IAppData data, MvcContext ctx ) {

            if (data == null) return "";

            // 1) html
            if (ctx != null && ctx.IsMock && ctx.GetItem( "_makeHtml" ) != null) return HtmlLink.ToAppData( data );

            String controllerPath = getAppDataController( data.GetType().FullName, data.AppId );

            // 2) link map
            String x = LinkMap.To( data.OwnerType, data.OwnerUrl, controllerPath, "Show", data.Id, data.AppId );
            if (x != null) return x;


            // 3)
            return To( data, controllerPath, "Show", data.Id );
        }

        private static String getAppDataController( String typeFullName, long appId ) {

            String typeName = strUtil.GetTypeName( typeFullName );

            string[] arrItem = typeFullName.Split( '.' );
            if (arrItem.Length < 3) throw new Exception( "domain namespace is contrary to convention" ); // app.domain.model => 1.app 2.domain 3.model

            String appNamespace = arrItem[arrItem.Length - 3];
            String appName = strUtil.TrimEnd( appNamespace, "App" );
            String controllerName = strUtil.TrimStart( typeName, appName );

            return appName + MvcConfig.Instance.UrlSeparator + controllerName;
        }

        private static string To(IAppData data, string controller, string action, long id) {
            String ownerPath = LinkHelper.GetMemberPathPrefix( data.OwnerType, data.OwnerUrl );
            return LinkHelper.AppendApp( data.AppId, controller, action, id, ownerPath );
        }

        private static String _app {
            get { return sys.Path.Root; }
        }

        //-----------------------------------------------------------------------------------------

        public static String ToUserAppFull( IMemberApp app ) {

            String strApp = strUtil.TrimEnd( app.AppInfo.TypeName, "App" );

            // 1) link map
            String controller = strApp + "/" + strApp;
            String x = LinkMap.To( app.OwnerType, app.OwnerUrl, controller, "Index", app.Id );
            if (x != null) return x;


            // 2) 
            if (MvcConfig.Instance.IsUrlToLower) {
                strApp = strApp.ToLower();
            }

            return getAppLink( app.OwnerType, app.OwnerUrl, strApp, app.AppOid );
        }

        /// <summary>
        /// 获取App的网址。最后的效果包括后缀名，比如 /Forum1/Forum/Index.aspx
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static String ToApp( IApp app ) {
            return ToApp( app, null );
        }

        /// <summary>
        /// 获取App的网址。最后的效果包括后缀名，比如 /Forum1/Forum/Index.aspx
        /// </summary>
        /// <param name="app"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static String ToApp( IApp app, MvcContext ctx ) {


            // 1) html
            if (ctx != null && ctx.IsMock && ctx.GetItem( "_makeHtml" ) != null) return HtmlLink.ToApp( app );

            String appName = strUtil.TrimEnd( app.GetType().Name, "App" );

            // 2) link map
            String controller = appName + "/" + appName;
            String x = LinkMap.To( app.OwnerType, app.OwnerUrl, controller, "Index", app.Id );
            if (x != null) return x;

            // 3)
            String ret = getAppLink( app.OwnerType, app.OwnerUrl, appName, app.Id );
            if (MvcConfig.Instance.IsUrlToLower) {
                return ret.ToLower();
            }
            return ret;
        }

        private static string getAppLink(string ownerTypeFull, string ownerUrl, string appName, long appId) {

            String result = LinkHelper.GetMemberPathPrefix( ownerTypeFull, ownerUrl );

            result = LinkHelper.Join( result, appName );
            if (appId > 1) result = result + appId;

            result = LinkHelper.Join( result, appName );
            if (MvcConfig.Instance.IsUrlToLower) {
                return LinkHelper.Join( result, "index" ) + MvcConfig.Instance.UrlExt;
            }
            else {
                return LinkHelper.Join( result, "Index" ) + MvcConfig.Instance.UrlExt;
            }
        }

        //----------------------------------------------------------------------------------------------------------------

        public static String ToUserMicroblog( IMember user ) {

            if (LinkHelper.IsMemberSubdomain( typeof( User ).FullName )) {
                return sys.Url.SchemeStr + user.Url + "." + SystemInfo.HostNoSubdomain + MvcConfig.Instance.UrlSeparator + "t" + MvcConfig.Instance.UrlExt;
            }
            else {
                return LinkHelper.Join( _app, "t" ) + MvcConfig.Instance.UrlSeparator + user.Url + MvcConfig.Instance.UrlExt;
            }
        }

        public static string ToMicroblog() {
            return LinkHelper.Join( _app, "t" ) + MvcConfig.Instance.UrlExt;
        }

        public static String ToTag( String tagName ) {
            return LinkHelper.Join( _app, "tag" ) + MvcConfig.Instance.UrlSeparator + tagName + MvcConfig.Instance.UrlExt;
        }



    }

}
