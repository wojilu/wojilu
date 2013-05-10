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

            if (ctx != null && ctx.IsMock && ctx.GetItem( "_makeHtml" ) != null) return HtmlLink.ToAppData( data );

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

            return appName + MvcConfig.Instance.UrlSeparator + controllerName;
        }

        private static String To( IAppData data, String controller, String action, int id ) {
            String ownerPath = LinkHelper.GetMemberPathPrefix( data.OwnerType, data.OwnerUrl );
            return LinkHelper.AppendApp( data.AppId, controller, action, id, ownerPath );
        }

        private static String _app {
            get { return sys.Path.Root; }
        }

        //----------------------------------------------------------------------------------------------------------------

        public static String ToUserAppFull( IMemberApp app ) {

            String strApp = strUtil.TrimEnd( app.AppInfo.TypeName, "App" );

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

            if (ctx != null && ctx.IsMock && ctx.GetItem( "_makeHtml" ) != null) return HtmlLink.ToApp( app );

            String appName = strUtil.TrimEnd( app.GetType().Name, "App" );
            String ret = getAppLink( app.OwnerType, app.OwnerUrl, appName, app.Id );
            if (MvcConfig.Instance.IsUrlToLower) {
                return ret.ToLower();
            }
            return ret;
        }

        private static String getAppLink( String ownerTypeFull, String ownerUrl, String appName, int appId ) {

            String result = LinkHelper.GetMemberPathPrefix( ownerTypeFull, ownerUrl );

            result = LinkHelper.Join( result, appName );
            if (appId > 0) result = result + appId;

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
