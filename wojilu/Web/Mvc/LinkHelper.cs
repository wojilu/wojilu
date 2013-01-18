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
using wojilu.Members.Interface;
using wojilu.Common;
using wojilu.Web.Mvc.Routes;

namespace wojilu.Web.Mvc {

    public class LinkHelper {

        public static String Join( String strA, String strB ) {
            if (strA.EndsWith( "/" )) return strA + strB; // 路径以/开头
            return strUtil.Join( strA, strB, MvcConfig.Instance.UrlSeparator );
        }

        public static String AppendApp( int appId, String controller, String action, int id, String result ) {

            if (controller.EndsWith( "Controller" )) controller = strUtil.TrimEnd( controller, "Controller" );

            controller = addAppId( controller, appId );
            controller = processController( controller );

            result = Join( result, controller );

            if (id > 0) result = Join( result, id.ToString() );
            result = joinAction( action, result );

            result = result + MvcConfig.Instance.UrlExt;
            return result;
        }

        public static String AppendApp( int appId, String controller, String action, String param, String result ) {

            if (controller.EndsWith( "Controller" )) controller = strUtil.TrimEnd( controller, "Controller" );

            controller = addAppId( controller, appId );
            controller = processController( controller );

            result = Join( result, controller );

            result = Join( result, param );

            result = joinAction( action, result );

            result = result + MvcConfig.Instance.UrlExt;
            return result;
        }

        private static String processController( String controller ) {
            if (MvcConfig.Instance.IsUrlToLower) return controller.ToLower();
            return controller;
        }

        private static String joinAction( String action, String result ) {
            if (action.Equals( "Show" )) return result;
            if (MvcConfig.Instance.IsUrlToLower) return Join( result, action.ToLower() );
            return Join( result, action );
        }

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
            if (memberType == null || memberUrl == null) return "";
            return getMemberPath( memberUrl, MemberPath.GetPath( strUtil.GetTypeName( memberType ) ), memberType );
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


        //-----------------------------------------------------------------------------------------

        internal static String GetRootPath() {
            return SystemInfo.RootPath;
        }

        internal static String AppPath {
            get { return SystemInfo.ApplicationPath; }
        }

        internal static String GetController( Type controllerType ) {
            return trimRootNamespace( strUtil.GetTypeFullName( controllerType ) )
                .TrimStart( '.' )
                .Replace( ".", MvcConfig.Instance.UrlSeparator );
            //return MvcConfig.Instance.IsUrlToLower ? str.ToLower() : str;
        }

        internal static String GetMemberUrl( String memberTypeFullName, String memberUrl ) {
            return getMemberPathUrl( strUtil.GetTypeName( memberTypeFullName ), memberUrl );
        }

        internal static string GetMemberUrl( String memberUrl, String ownerPath, String memberType ) {
            if (LinkHelper.IsMemberSubdomain( memberType )) {
                return sys.Url.SchemeStr + memberUrl + "." + SystemInfo.HostNoSubdomain;
            }
            else {
                return sys.Url.SchemeStr + Join( SystemInfo.Host, GetMemberUrl( memberType, memberUrl ) ) + MvcConfig.Instance.UrlExt;
            }
        }


        internal static String GetControllerName( Type type ) {
            return type.Name.Substring( 0, type.Name.Length - 10 );
        }

        //-----------------------------------------------------------------------------------------

        private static String addAppId( String controller, int appId ) {

            String[] arrItem = controller.Split( RouteTool.Separator );
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < arrItem.Length; i++) {

                if (i == 0 && appId > 0) {
                    builder.Append( arrItem[i] );
                    builder.Append( appId );
                    builder.Append( MvcConfig.Instance.UrlSeparator );
                }
                else {
                    builder.Append( arrItem[i] );
                    builder.Append( MvcConfig.Instance.UrlSeparator );
                }
            }
            return builder.ToString().TrimEnd( MvcConfig.Instance.UrlSeparator[0] );
        }

        private static String trimRootNamespace( String typeFullName ) {

            foreach (String ns in MvcConfig.Instance.RootNamespace) {
                if (typeFullName.StartsWith( ns )) return strUtil.TrimStart( typeFullName, ns );
            }

            return typeFullName;
        }

        private static string getMemberPath( String memberUrl, String ownerPath, String memberType ) {

            if (MvcConfig.Instance.CheckDomainMap()) {

                if (IsMemberSubdomain( memberType )) {
                    return sys.Url.SchemeStr + memberUrl + "." + SystemInfo.HostNoSubdomain;
                }
                else {
                    return sys.Url.SchemeStr + Join( SystemInfo.Host, GetMemberUrl( memberType, memberUrl ) );
                }
            }
            else {

                if (memberType.Equals( ConstString.SiteTypeFullName )) return GetRootPath();

                return Join( AppPath, GetMemberUrl( memberType, memberUrl ) );
            }
        }

        private static String getMemberPathUrl( String ownerTypeName, String ownerUrl ) {
            if (strUtil.GetTypeName( ConstString.SiteTypeFullName ).Equals( ownerTypeName )) return "";
            return Join( MemberPath.GetPath( ownerTypeName ), ownerUrl );
        }


    }
}
