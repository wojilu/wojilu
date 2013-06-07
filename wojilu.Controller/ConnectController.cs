/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.OAuth;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;
using wojilu.Common;
using wojilu.Web.Utils;
using wojilu.Net;
using wojilu.DI;
using wojilu.Web.Controller.Helpers;

namespace wojilu.Web.Controller {

    public class ConnectController : ControllerBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( ConnectController ) );

        public IUserService userService { get; set; }
        public ILoginService loginService { get; set; }
        public IUserConnectService connectService { get; set; }

        public ConnectController() {
            userService = new UserService();
            loginService = new LoginService();
            connectService = new UserConnectService();
        }

        [Login]
        public void Bind() {

            String connectType = ctx.Get( "connectType" );

            AuthConnect connect = AuthConnectFactory.GetConnect( connectType );
            if (connect == null) {
                echoError( "此连接类型不存在:" + connectType );
                return;
            }

            // 检查是否绑定过
            if (connectService.HasBind( ctx.viewer.Id, connect.GetType().FullName )) {
                echoError( "对不起，已经绑定" );
                return;
            }

            ctx.web.SessionSet( "__connectType", connect.GetType().FullName );
            ctx.web.SessionSet( "__connectFrom", "bind" );

            redirectUrl( connect.GetAuthorizationFullUrl() );
        }

        public void Login() {

            if (ctx.viewer.IsLogin) {
                echoError( "对不起，您已经登录" );
                return;
            }

            // 1) 登录的第三方类型
            String connectType = ctx.Get( "connectType" );

            AuthConnect connect = AuthConnectFactory.GetConnect( connectType );
            if (connect == null) {
                echoError( "连接类型不存在:" + connectType );
                return;
            }

            // 2) 根据类型，redirect到第三方授权。回调网址callback 是 ProcessLogin()
            ctx.web.SessionSet( "__connectType", connect.GetType().FullName );
            ctx.web.SessionSet( "__connectFrom", "login" );

            redirectUrl( connect.GetAuthorizationFullUrl() );
        }

        public void Callback() {

            string code = ctx.Get( "code" );
            if (strUtil.IsNullOrEmpty( code )) {
                echoError( "code无效" );
                return;
            }

            Object connectType = ctx.web.SessionGet( "__connectType" );
            if (connectType == null) {
                echoError( "无效的 connect type" );
                return;
            }

            AuthConnect connect = AuthConnectFactory.GetConnect( connectType.ToString() );
            if (connect == null) {
                echoError( "此连接类型不存在:" + connectType );
                return;
            }

            Object connectFrom = ctx.web.SessionGet( "__connectFrom" );
            if (connectFrom == null) {
                echoError( "进入本页方式错误" );
            }
            else if (connectFrom.ToString() == "bind") {
                processBind( connect, code );
            }
            else if (connectFrom.ToString() == "login") {
                processLogin( connect, code );
            }
            else {
                echoError( "进入本页方式错误" );
            }
        }

        private void processBind( AuthConnect connect, String code ) {

            if (ctx.viewer.IsLogin == false) {
                echoError( "对不起，请先登录" );
                return;
            }

            // 检查是否绑定过
            if (connectService.HasBind( ctx.viewer.Id, connect.GetType().FullName )) {
                echoError( "对不起，已经绑定" );
                return;
            }

            // 获取用户 uid
            AccessToken x = OAuthClient.New().GetAccessToken( connect, code, connect.HttpMethod_AccessToken );
            x.Uid = connect.GetUid( x );

            // 获取用户名称
            OAuthUserProfile userProfile = null;

            try {
                userProfile = connect.GetUserProfile( x );
            }
            catch (HttpClientException ex) {

                if (ex.Message.IndexOf( "applications over the unaudited use restrictions" ) > 0) {
                    echo( getTestRestrictionsMsg( "绑定" ) );
                    return;
                }
                else {
                    throw ex;
                }
            }

            x.Name = userProfile.Name;

            Result result = connectService.Create( ctx.viewer.obj as User, connect.GetType().FullName, x );

            // 日志
            logger.Info( "accessToken=" + x.Token );
            logger.Info( "uid=" + x.Uid );
            logger.Info( "refresh_token=" + x.RefreshToken );
            logger.Info( "expires_in=" + x.ExpiresIn );
            logger.Info( "scope=" + x.Scope );

            if (result.HasErrors) {
                echo( result.ErrorsHtml );
            }
            else {
                echoRedirect( lang( "opok" ), "/" );
            }
        }

        private String getTestRestrictionsMsg( String opName ) {
            return string.Format( "对不起，因为目前属于测试期，您的帐号没有加入测试名单，所以暂时无法{0}。<br/>您可以将帐号提交给管理员，申请测试。", opName );
        }

        private void processLogin( AuthConnect connect, String code ) {

            if (ctx.viewer.IsLogin) {
                echoError( "对不起，您已经登录" );
                return;
            }

            AccessToken accessToken = OAuthClient.New().GetAccessToken( connect, code, connect.HttpMethod_AccessToken );
            logger.Info( "accessToken=" + accessToken.Token );
            logger.Info( "uid=" + accessToken.Uid );
            logger.Info( "refresh_token=" + accessToken.RefreshToken );
            logger.Info( "expires_in=" + accessToken.ExpiresIn );
            logger.Info( "scope=" + accessToken.Scope );

            String uid = connect.GetUid( accessToken );

            // 1) 检查网站中是否有此用户
            UserConnect x = connectService.GetConnectInfo( uid, connect.GetType().FullName );

            // 第一次登录
            if (x == null) {
                try {
                    loadUserProfile( connect, accessToken );

                }
                catch (HttpClientException ex) {

                    if (ex.Message.IndexOf( "applications over the unaudited use restrictions" ) > 0) {
                        echo( getTestRestrictionsMsg( "登录" ) );
                    }
                    else {
                        throw ex;
                    }

                }
            }
            // 其他：获取用户信息，然后登录
            else {

                checkAccessToken( x, accessToken );

                LoginTime expiration = LoginTime.OneWeek;
                loginService.Login( x.User, x.Id, expiration, ctx.Ip, ctx );

                echoRedirect( "登录成功", "/" );
            }
        }

        private void checkAccessToken( UserConnect x, AccessToken token ) {

            x.AccessToken = token.Token;
            x.RefreshToken = token.RefreshToken;
            x.ExpiresIn = token.ExpiresIn;
            x.Scope = token.Scope;

            // 重新获取access token之后，服务器会自动延续授权时间
            x.Updated = DateTime.Now;

            x.update();
        }

        private void loadUserProfile( AuthConnect connect, AccessToken accessToken ) {

            OAuthUserProfile user = connect.GetUserProfile( accessToken );

            ctx.SetItem( "__currentOAuthUser", user );
            ctx.SetItem( "__currentAccessToken", accessToken );

            content( loadHtml( confirmUserInfo ) );
        }

        [NonVisit]
        public void confirmUserInfo() {

            target( SaveFirstLogin );

            OAuthUserProfile user = ctx.GetItem( "__currentOAuthUser" ) as OAuthUserProfile;
            AccessToken accessToken = ctx.GetItem( "__currentAccessToken" ) as AccessToken;

            String userName = getUserName( user.Name );
            set( "userName", userName );
            set( "userUrl", getUserUrl( userName, user.FriendlyUrl ) );
            set( "userPic", user.PicUrlSmall );

            set( "userUrlPrefix", sys.Url.SiteUrl );
            set( "urlExt", MvcConfig.Instance.UrlExt );

            // hidden
            set( "uid", accessToken.Uid );
            set( "accessToken", accessToken.Token );
            set( "refreshToken", accessToken.RefreshToken );
            set( "expiresIn", accessToken.ExpiresIn );
            set( "scope", accessToken.Scope );
        }

        [HttpPost, DbTransaction]
        public void SaveFirstLogin() {

            if (ctx.viewer.IsLogin) {
                echoError( "对不起，您已经登录" );
                return;
            }

            Object connectType = ctx.web.SessionGet( "__connectType" );
            if (connectType == null) {
                echoError( "无效的 connect type" );
                return;
            }

            AuthConnect connect = AuthConnectFactory.GetConnect( connectType.ToString() );
            if (connect == null) {
                echoError( "此连接类型不存在:" + connectType );
                return;
            }

            AccessToken accessToken = getAccessToken();

            OAuthUserProfile userProfile = connect.GetUserProfile( accessToken );
            if (userProfile == null) {
                echoError( "无法获取正常 user profile" );
                return;
            }

            accessToken.Name = userProfile.Name;

            // 注册用户
            User user = new User();
            user.Name = ctx.Post( "userName" );
            user.Url = ctx.Post( "userUrl" );

            Result result = userService.RegisterNoPwd( user );
            if (result.HasErrors) {
                echoError( result );
                return;
            }

            result = AvatarUploader.SaveRemote( userProfile.PicUrlBig, user.Id );
            if (result.IsValid) {
                user.Pic = result.Info.ToString();
                user.update();
            }
            else {
                echoError( result );
                return;
            }

            // 是否开启空间
            RegHelper.CheckUserSpace( user, ctx );

            // 绑定用户
            Result saveResult = connectService.Create( user, connect.GetType().FullName, accessToken );

            if (saveResult.IsValid) {

                UserConnect userConnect = saveResult.Info as UserConnect;
                loginService.Login( user, userConnect.Id, LoginTime.OneWeek, ctx.Ip, ctx ); // 登录
                echoRedirect( "登录成功", "/" );
            }
            else {
                echoError( saveResult );
            }
        }

        private AccessToken getAccessToken() {

            AccessToken x = new AccessToken();

            String uid = ctx.Post( "uid" );
            String accessToken = ctx.Post( "accessToken" );
            String refreshToken = ctx.Post( "refreshToken" );
            int expiresIn = ctx.PostInt( "expiresIn" );
            String scope = ctx.Post( "scope" );

            x.Token = accessToken;
            x.RefreshToken = refreshToken;
            x.Uid = uid;
            x.ExpiresIn = expiresIn;
            x.Scope = scope;

            return x;
        }

        private String getUserName( string userName ) {

            if (userService.IsExist( userName ) == null) return userName;

            for (int i = 1; i <= 10; i++) {
                String newName = userName + i;
                if (userService.IsExist( newName ) == null) return newName;
            }
            return userName + "_a1";
        }

        // TODO 检查中文名
        private object getUserUrl( string name, string domain ) {

            // 如果有现成的个性网址
            if (strUtil.HasText( domain )) {

                if (userService.IsExistUrl( domain ) == null) {
                    return domain;
                }
                else {
                    return getNewByOldName( domain );
                }
            }

            // 如果用户名符合要求
            if (strUtil.IsUrlItem( name )) {

                if (userService.IsExistUrl( name ) == null) {
                    return name;
                }
                else {
                    return getNewByOldName( name );
                }
            }

            // 旧的个性网址 & 用户名，都不符合要求，只能自定义随机
            DateTime x = DateTime.Now;
            String str = "u" + x.Year + x.Month + x.Day;

            return getNewByOldName( str );
        }

        private String getNewByOldName( String name ) {

            for (int i = 1; i <= 10; i++) {
                String newUrl = name + i;
                if (userService.IsExistUrl( newUrl ) == null) return newUrl;
            }
            return name + "_a1";
        }

        //----------------------------------------------------------------


        [Login, HttpDelete, DbTransaction]
        public void UnBind() {
            Result result = connectService.UnBind( ctx.viewer.Id, ctx.Get( "connectType" ) );
            if (result.HasErrors) {
                echo( result.ErrorsText );
            }
            else {
                echoRedirectPart( lang( "opok" ) );
            }

        }


        [Login, HttpPost, DbTransaction]
        public void Sync() {
            Result result = connectService.Sync( ctx.viewer.Id, ctx.Get( "connectType" ), ctx.PostInt( "isSync" ) );
            echoResult( result );
        }

    }
}
