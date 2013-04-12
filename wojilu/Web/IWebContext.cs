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
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.SessionState;
using wojilu.Common;

namespace wojilu.Web {

    /// <summary>
    /// web 原始数据和方法的接口
    /// </summary>
    public interface IWebContext {

        Uri Url { get; }

        String PathRoot { get; }
        String PathAbsolute { get; }
        String PathApplication { get; }
        String PathRawUrl { get; }
        String PathHost { get; }
        String PathInfo { get; }
        String PathReferrer { get; }
        String PathReferrerHost { get; }

        String ClientHttpMethod { get; }
        String ClientIp { get; }
        String ClientAgent { get; }
        String[] ClientLanguages { get; }
        String ClientVar( String key );

        void CompleteRequest();
        void ResponseWrite( String content );
        void ResponseStatus( String status );
        void ResponseFlush( );
        void ResponseEnd();
        void ResponseContentType( String contentType );

        String UrlDecode( String path );
        String UrlEncode( String path );
        void RenderJson( String content );
        void RenderXml( String content );

        String post( String key );
        Boolean postHas( String key );
        NameValueCollection postValueAll();
        String[] postValuesByKey( String name );

        String get( String key );
        Boolean getHas( String key );

        String param( String key );
        HttpFileCollection getUploadFiles();

        void Redirect( String url );
        void Redirect( String url, bool endResponse );

        /// <summary>
        /// 获取当前登录用户的 userId，如果没有登录，则为-1
        /// </summary>
        /// <returns></returns>
        int UserId();

        /// <summary>
        /// 获取使用自定义的cookie名称登录的 userId
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        int UserId( String cookieName );

        /// <summary>
        /// 判断用户是否登录(默认cookie验证)
        /// </summary>
        bool UserIsLogin { get; }

        /// <summary>
        /// 使用默认的cookie登录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="expiration">过期的时间，比如10分钟过期： DateTime.Now.AddMinutes( 10 )</param>
        void UserLogin( int userId, String userName, DateTime expiration );

        /// <summary>
        /// 使用默认的cookie登录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="expiration">使用 LoginTime 枚举确定过期时间</param>
        void UserLogin( int userId, String userName, LoginTime expiration );

        /// <summary>
        /// 使用自定义的cookie名称登录，允许多个验证授权，互不影响
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="expiration">过期的时间，比如10分钟过期： DateTime.Now.AddMinutes( 10 )</param>
        void UserLogin( String cookieName, int userId, String userName, DateTime expiration );

        /// <summary>
        /// 使用自定义的cookie名称登录，允许多个验证授权，互不影响
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="expiration">使用 LoginTime 枚举确定过期时间</param>
        void UserLogin( String cookieName, int userId, String userName, LoginTime expiration );

        /// <summary>
        /// 用户注销
        /// </summary>
        void UserLogout();

        /// <summary>
        /// 在自定义cookie验证模式下，注销登录
        /// </summary>
        /// <param name="cookieName"></param>
        void UserLogout( String cookieName );

        /// <summary>
        /// 默认的登录验证的cookie名称=FormsAuthentication.FormsCookieName
        /// </summary>
        /// <returns></returns>
        String CookieAuthName();

        /// <summary>
        /// 默认的FormsAuthentication.FormsCookieName的cookie值，未经解密
        /// </summary>
        /// <returns></returns>
        String CookieAuthValue();

        /// <summary>
        /// 获取自定义的auth cookie的值，尚未解密
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        String CookieAuthValue( String cookieName );

        /// <summary>
        /// 清除所有wojiluCookie
        /// </summary>
        void CookieClear();

        /// <summary>
        /// 获取wojiluCookie中某项的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        String CookieGet( String key );

        /// <summary>
        /// 在wojiluCookie中删除一项
        /// </summary>
        /// <param name="key"></param>
        void CookieRemove( String key );

        /// <summary>
        /// 在wojiluCookie中增加一项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        void CookieSet( String key, String val );
        void CookieSetLang( String val );

        object SessionGet( String key );
        void SessionSet( String key, object val );
        String SessionId { get; }

        String GetAuthJson();
        String GetAuthJson( String cookieName );
        String GetAuthJson( String[] arrCookieName );

        object Session { get; }
        object Context { get; }

    }

}
