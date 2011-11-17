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
using System.Text;

using wojilu.Web.Mvc.Routes;
using wojilu.Web.Context;

using wojilu.Members.Interface;
using wojilu.Common;

namespace wojilu.Web.Mvc {


    /// <summary>
    /// 通用链接生成工具
    /// </summary>
    public class Link {

        private int _appId;
        private IMember _owner;
        private MvcContext _ctx;

        public Link( MvcContext ctx ) {
            _ctx = ctx;
            _appId = ctx.route.appId;
            if (ctx.owner != null) _owner = ctx.owner.obj;
        }


        public String To( aAction action ) {
            return To( _owner, getController( action.Target.GetType() ), action.Method.Name );
        }

        public String To( aActionWithId action, int id ) {
            return To( _owner, getController( action.Target.GetType() ), action.Method.Name, id );
        }

        //public string To( aActionWithQuery action, string param ) {
        //    return To( _owner, getController( action.Target.GetType() ), action.Method.Name, param );
        //}

        public String To( IMember member, aAction action ) {
            return To( member, getController( action.Target.GetType() ), action.Method.Name, -1 );
        }

        public String To( IMember member, aActionWithId action, int id ) {
            String ownerPath = GetMemberPathPrefix( member );
            return AppendApp( _appId, getController( action.Target.GetType() ), action.Method.Name, id, ownerPath );
        }

        public String To( String memberType, String memberUrl, aActionWithId action, int id, int appId ) {
            String ownerPath = GetMemberPathPrefix( memberType, memberUrl );
            return AppendApp( appId, getController( action.Target.GetType() ), action.Method.Name, id, ownerPath );
        }


        //--------------------------------------------------


        public String T2( aAction action ) {
            return To( _owner, getController( action.Target.GetType() ), action.Method.Name, 0, 0 );
        }

        public String T2( aActionWithId action, int id ) {
            return To( _owner, getController( action.Target.GetType() ), action.Method.Name, id, 0 );
        }

        //public String T2( aActionWithQuery action, String param ) {
        //    return To( _owner, getController( action.Target.GetType() ), action.Method.Name, param, 0 );
        //}

        public String T2( IMember member, aAction action ) {
            return To( member, getController( action.Target.GetType() ), action.Method.Name, -1, 0 );
        }

        public String T2( IMember member, aActionWithId action, int id ) {
            String ownerPath = GetMemberPathPrefix( member );
            return AppendApp( 0, getController( action.Target.GetType() ), action.Method.Name, id, ownerPath );
        }

        private String getController( Type controllerType ) {
            return trimRootNamespace( strUtil.GetTypeFullName( controllerType ) )
                .TrimStart( '.' )
                .Replace( ".", "/" );
        }

        private String trimRootNamespace( String typeFullName ) {

            foreach (String ns in MvcConfig.Instance.RootNamespace) {
                if (typeFullName.StartsWith( ns )) return strUtil.TrimStart( typeFullName, ns );
            }

            return typeFullName;
        }


        //---------------------------------------

        public String To( IMember member, Type controller, String action, int id ) {
            return To( member, getController( controller ), action, id, _appId );
        }

        public String To( IMember member, String controller, String action, int id ) {
            return To( member, controller, action, id, _appId );
        }

        public String To( IMember member, String controller, String action, int id, int appId ) {
            String ownerPath = GetMemberPathPrefix( member );
            return AppendApp( appId, controller, action, id, ownerPath );
        }

        public string To( IMember member, String controller, String action, string param ) {
            return To( member, controller, action, param, _appId );
        }

        public string To( IMember member, String controller, String action, string param, int appId ) {
            String ownerPath = GetMemberPathPrefix( member );
            return AppendApp( appId, controller, action, param, ownerPath );
        }


        //---------------------------------------

        internal String To( String controller, String action, int id ) {
            return To( _owner, controller, action, id );
        }

        internal String To( Type type, String action, int id ) {
            return To( _owner, getControllerNameByType( type ), action, id );
        }

        internal String To( IMember member, String controller, String action ) {
            return To( member, controller, action, -1 );
        }


        //----------------------------------------------------------------------------------------------

        /// <summary>
        /// 获取成员根路径，比如 /space/zhangsan, /group/moon 等；如果是二级域名，返回 http://zhangsan.abc.com
        /// </summary>
        public static String GetMemberPathPrefix( IMember member ) {
            if (member == null) return "";
            return getMemberPath( member.Url, MemberPath.GetPath( member.GetType().Name ), member.GetType().FullName );
        }

        /// <summary>
        /// 获取成员根路径，比如 /space/zhangsan, /group/moon 等；如果是二级域名，返回 http://zhangsan.abc.com
        /// </summary>
        public static String GetMemberPathPrefix( String memberType, String memberUrl ) {
            return getMemberPath( memberUrl, MemberPath.GetPath( strUtil.GetTypeName( memberType ) ), memberType );
        }

        private static string getMemberPath( String memberUrl, String ownerPath, String memberType ) {


            if (MvcConfig.Instance.CheckDomainMap()) {

                if (IsMemberSubdomain( memberType )) {
                    return "http://" + memberUrl + "." + SystemInfo.HostNoSubdomain;
                }
                else {
                    return "http://" + strUtil.Join( SystemInfo.Host, getMemberUrl( memberType, memberUrl ) );
                }
            }

            else {

                if (memberType.Equals( ConstString.SiteTypeFullName )) return getRootPath();

                return strUtil.Join( _app, getMemberUrl( memberType, memberUrl ) );
            }
        }


        private static String getMemberUrl( String memberTypeFullName, String memberUrl ) {
            return getMemberPathUrl( strUtil.GetTypeName( memberTypeFullName ), memberUrl );
        }

        private static String getMemberPathUrl( String ownerTypeName, String ownerUrl ) {
            if (strUtil.GetTypeName( ConstString.SiteTypeFullName ).Equals( ownerTypeName )) return "";
            return strUtil.Join( MemberPath.GetPath( ownerTypeName ), ownerUrl );
        }

        private String getControllerNameByType( Type type ) {
            return type.Name.Substring( 0, type.Name.Length - 10 );
        }

        public static String AppendApp( int appId, String controller, String action, String param, String result ) {

            if (controller.EndsWith( "Controller" )) controller = strUtil.TrimEnd( controller, "Controller" );

            controller = addAppId( controller, appId );
            result = strUtil.Join( result, controller );

            result = strUtil.Join( result, param );
            if (!action.Equals( "Show" )) result = strUtil.Join( result, action );

            result = result + MvcConfig.Instance.UrlExt;
            return result;
        }

        public static String AppendApp( int appId, String controller, String action, int id, String result ) {

            if (controller.EndsWith( "Controller" )) controller = strUtil.TrimEnd( controller, "Controller" );

            controller = addAppId( controller, appId );
            result = strUtil.Join( result, controller );

            if (id > 0) result = strUtil.Join( result, id.ToString() );
            if (!action.Equals( "Show" )) result = strUtil.Join( result, action );

            result = result + MvcConfig.Instance.UrlExt;
            return result;
        }

        private static String addAppId( String controller, int appId ) {

            String[] arrItem = controller.Split( '/' );
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < arrItem.Length; i++) {

                if (i == 0 && appId > 0) {
                    builder.Append( arrItem[i] );
                    builder.Append( appId );
                    builder.Append( "/" );
                }
                else {
                    builder.Append( arrItem[i] );
                    builder.Append( "/" );
                }
            }
            return builder.ToString().TrimEnd( '/' );
        }


        //--------------------------------------------- 静态方法 -------------------------------------------------

        /// <summary>
        /// 获取某个 member 的网址
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static String ToMember( IMember member ) {

            if (member == null) return "";
            if (strUtil.IsNullOrEmpty( member.Url ) || member.Url == "#") return "javascript:void(0)";

            return ToMember( member.GetType().FullName, member.Url );
        }

        private static readonly ILog logger = LogManager.GetLogger( typeof( Link ) );

        /// <summary>
        /// 获取某个 member 的网址
        /// </summary>
        /// <param name="memberType">member 的类型完整名</param>
        /// <param name="memberUrl">member 的个性网址</param>
        /// <returns></returns>
        public static String ToMember( String memberType, String memberUrl ) {


            if (MvcConfig.Instance.CheckDomainMap()) {

                String ownerPath = MemberPath.GetPath( strUtil.GetTypeName( memberType ) );
                return getMemberUrl( memberUrl, ownerPath, memberType );

            }
            else {

                if (memberType.Equals( ConstString.SiteTypeFullName )) return getRootPath();
                if (memberType.Equals( ConstString.UserTypeFullName )) return ToUser( memberUrl );


                String ownerPath = MemberPath.GetPath( strUtil.GetTypeName( memberType ) );
                return strUtil.Append( strUtil.Join( strUtil.Join( _app, ownerPath ), memberUrl ), MvcConfig.Instance.UrlExt );

            }

        }

        private static string getMemberUrl( String memberUrl, String ownerPath, String memberType ) {

            if (IsMemberSubdomain( memberType )) {
                return "http://" + memberUrl + "." + SystemInfo.HostNoSubdomain;
            }
            else {
                return "http://" + strUtil.Join( SystemInfo.Host, getMemberUrl( memberType, memberUrl ) ) + MvcConfig.Instance.UrlExt;
            }

        }

        /// <summary>
        /// 某member是否被配置为二级域名
        /// </summary>
        /// <param name="memberType"></param>
        /// <returns></returns>
        public static bool IsMemberSubdomain( String memberType ) {

            if (MvcConfig.Instance.CheckDomainMap() == false) return false;

            return memberType.Equals( MvcConfig.Instance.SubdomainWildcardType );
        }

        /// <summary>
        /// 获取某个注册用户的网址
        /// </summary>
        /// <param name="friendUrl">用户的个性网址</param>
        /// <returns></returns>
        public static String ToUser( String friendUrl ) {
            if (IsMemberSubdomain( ConstString.UserTypeFullName )) {
                return "http://" + friendUrl + "." + SystemInfo.HostNoSubdomain;
            }
            else {
                return strUtil.Append( strUtil.Join( _app, friendUrl ), MvcConfig.Instance.UrlExt );
            }
        }

        private static String getRootPath() {
            return SystemInfo.RootPath;
        }

        private static String _app {
            get { return SystemInfo.ApplicationPath; }
        }


        //----------------------------------------------------------------------------------------

        /// <summary>
        /// 计算分页的页码
        /// </summary>
        /// <param name="count">数据量</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns>总计多少页</returns>
        public static int GetPageIndex( int count, int pageSize ) {

            if (count == 0) return 1;

            int mod = count % pageSize;
            if (mod == 0) return count / pageSize;

            return count / pageSize + 1;
        }

        /// <summary>
        /// 在已有网址url后加上页码 Post/List.aspx=>Post/List/p6.aspx
        /// </summary>
        /// <param name="srcUrl">原始网址</param>
        /// <param name="pageNumber">页码</param>
        /// <returns></returns>
        public static String AppendPage( String srcUrl, int pageNumber ) {

            String url = srcUrl;

            // 查询字符串
            int qIndex = url.IndexOf( "?" );
            String query = "";
            if (qIndex > 0) {
                url = srcUrl.Substring( 0, qIndex );
                query = srcUrl.Substring( qIndex, (srcUrl.Length - qIndex) );
            }

            String ext = getExt( url );

            // 有扩展名
            if (ext.Length > 0) {
                url = strUtil.TrimEnd( url, ext );
            }

            String originalPage = getPageNumberLabel( url );
            if (originalPage.Length > 0) url = strUtil.TrimEnd( url, originalPage );

            if (pageNumber <= 1)
                return url + ext + query;
            else
                return url + "/p" + pageNumber + ext + query;
        }

        private static String getPageNumberLabel( String url ) {

            if (strUtil.IsNullOrEmpty( url )) return "";

            char separator = '/';
            String[] arr = url.Split( separator );
            if (arr.Length < 2) return "";

            String end = arr[arr.Length - 1];

            if (end.StartsWith( "p" ) && cvt.IsInt( end.Substring( 1 ) )) {
                return separator + end;
            }

            return "";
        }

        private static String getExt( String url ) {
            int dotIndex = url.LastIndexOf( "." );
            int slashIndex = url.LastIndexOf( "/" );
            if (dotIndex < 0) return "";
            if (dotIndex < slashIndex) return "";
            return url.Substring( dotIndex, (url.Length - dotIndex) );
        }




    }
}

