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
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Mvc {

    /// <summary>
    /// 通用链接生成工具
    /// </summary>
    [MvcLink]
    public class Link {

        public static String To( aAction action ) {
            return To( null, action, -1 );
        }

        public static String To( aActionWithId action, int id ) {
            return To( null, action, id, -1 );
        }

        public static String To( String controller, String action ) {
            return To( null, controller, action, -1 );
        }

        public static String To( String controller, String action, int id ) {
            return To( null, controller, action, id );
        }

        //-------------------------------------------------------------

        public static String To( IMember member, String controller, String action, int id ) {
            return To( member, controller, action, id, -1 );
        }

        public static String To( IMember member, String controller, String action, int id, int appId ) {
            String ownerPath = LinkHelper.GetMemberPathPrefix( member );
            return LinkHelper.AppendApp( appId, controller, action, id, ownerPath );
        }

        public static String To( IMember member, aAction action ) {
            return To( member, LinkHelper.GetController( action.Target.GetType() ), action.Method.Name, -1, -1 );
        }

        public static String To( IMember member, aAction action, int appId ) {
            return To( member, LinkHelper.GetController( action.Target.GetType() ), action.Method.Name, -1, appId );
        }

        public static String To( IMember member, aActionWithId action, int id ) {
            String ownerPath = LinkHelper.GetMemberPathPrefix( member );
            return LinkHelper.AppendApp( -1, LinkHelper.GetController( action.Target.GetType() ), action.Method.Name, id, ownerPath );
        }

        public static String To( IMember member, aActionWithId action, int id, int appId ) {
            String ownerPath = LinkHelper.GetMemberPathPrefix( member );
            return LinkHelper.AppendApp( appId, LinkHelper.GetController( action.Target.GetType() ), action.Method.Name, id, ownerPath );
        }

        public static String To( String memberType, String memberUrl, aActionWithId action, int id, int appId ) {
            String ownerPath = LinkHelper.GetMemberPathPrefix( memberType, memberUrl );
            return LinkHelper.AppendApp( appId, LinkHelper.GetController( action.Target.GetType() ), action.Method.Name, id, ownerPath );
        }

        //-------------------------------------------------------------

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

        /// <summary>
        /// 获取某个 member 的网址
        /// </summary>
        /// <param name="memberType">member 的类型完整名</param>
        /// <param name="memberUrl">member 的个性网址</param>
        /// <returns></returns>
        public static String ToMember( String memberType, String memberUrl ) {

            if (MvcConfig.Instance.CheckDomainMap()) {
                String ownerPath = MemberPath.GetPath( strUtil.GetTypeName( memberType ) );
                return LinkHelper.GetMemberUrl( memberUrl, ownerPath, memberType );
            }
            else {
                if (memberType.Equals( ConstString.SiteTypeFullName )) return LinkHelper.GetRootPath();
                if (memberType.Equals( ConstString.UserTypeFullName )) return ToUser( memberUrl );
                String ownerPath = MemberPath.GetPath( strUtil.GetTypeName( memberType ) );
                return strUtil.Append( LinkHelper.Join( LinkHelper.Join( LinkHelper.AppPath, ownerPath ), memberUrl ), MvcConfig.Instance.UrlExt );
            }
        }

        /// <summary>
        /// 获取某个注册用户的网址
        /// </summary>
        /// <param name="friendUrl">用户的个性网址</param>
        /// <returns></returns>
        public static String ToUser( String friendUrl ) {
            if (LinkHelper.IsMemberSubdomain( ConstString.UserTypeFullName )) {
                return sys.Url.SchemeStr + friendUrl + "." + SystemInfo.HostNoSubdomain;
            }
            else {
                return strUtil.Append( LinkHelper.Join( LinkHelper.AppPath, friendUrl ), MvcConfig.Instance.UrlExt );
            }
        }



    }

}
