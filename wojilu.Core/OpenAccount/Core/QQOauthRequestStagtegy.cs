using System;
using System.Collections.Generic;
using System.Text;
using wojilu.weibo.Interface;
using wojilu.Members.Users.Interface;
using wojilu.weibo.Domain;
using wojilu.weibo.Service;
using wojilu.Members.Users.Service;
using wojilu.Web.Mvc;
using wojilu.weibo.Core.QQWeibo;
using wojilu.weibo.Controller.Weibo;
using wojilu.Members.Users.Domain;
using wojilu.Common;
using Newtonsoft.Json.Linq;
using wojilu.Web.Context;

namespace wojilu.weibo.Core {

    public class QQOAuthRequestStagtegy : IOAuthRequestStrategy {

        private static ILog log = LogManager.GetLogger( typeof( QQOAuthRequestStagtegy ) );

        IUserWeiboSettingService _weiboService;

        IUserService userService;

        ILoginService loginService;

        WeiboType type;

        public QQOAuthRequestStagtegy() {
            type = WeiboType.GetByName( SupportWeiboType.QQWeibo );
            if (type == null)
                throw new Exception( "腾讯微博配置文件找不到" );
            _weiboService = new UserWeiboSettingService();
            userService = new UserService();
            loginService = new LoginService();
        }

        public void RedirectToAuthorizationUri( MvcContext ctx, String indexUrl, String callbackUrl ) {
            OauthKey key = new OauthKey( type.AppKey, type.AppSecret );
            bool success = false;
            try {
                success = key.GetRequestToken( getCallbackUrl( ctx, callbackUrl ) );
            }
            catch (Exception ex) {
                log.Error( ex.Message );
            }
            if (success) {
                ctx.web.SessionSet( "qqweibo", key );
                ctx.web.Redirect( key.GetOAuthUrl() );
            }
            else {
                //c.echoRedirect( "操作失败，请重试" );
            }
        }

        public void ProcessCallback( MvcContext ctx, String indexUrl, String callbackUrl ) {
            OauthKey key = ctx.web.SessionGet( "qqweibo" ) as OauthKey;
            string verifier = ctx.Get( "oauth_verifier" );
            if (key == null || string.IsNullOrEmpty( verifier )) {
                //c.echoRedirect( "请不要直接访问此页面", "/" );
                return;
            }
            bool success = false;
            try {
                success = key.GetAccessToken( verifier );
            }
            catch (Exception ex) {
                log.Error( ex.Message );
            }

            if (!success) {
                //c.echoRedirect( "绑定失败，请重试！", indexUrl );
                return;
            }
            UserWeiboSetting setting = _weiboService.Find( type.Id, key.tokenKey, key.tokenSecret );

            //如果用户已经微博绑定此帐户
            if (setting != null) {
                //这里做用户登陆处理
                User user = userService.GetById( setting.UserId );
                if (user != null) {
                    loginService.Login( user, LoginTime.OneMonth, ctx.Ip, ctx );
                    ctx.web.Redirect( "/" );
                    return;
                }
                else {
                    //c.echoRedirect( "发生未知错误，请重试", indexUrl );
                    return;
                }
            }

            wojilu.weibo.Core.QQWeibo.user qqUser = new user( key, "json" );

            JToken weiboInfo = qqUser.info();

            if (!ctx.viewer.IsLogin) {
                //redirectToRegister( c, key, weiboInfo["data"]["name"].ToString(), weiboInfo["data"]["head"].ToString() );
                return;
            }

            setting = _weiboService.Find( ctx.viewer.Id, type.Id );
            Result result;
            if (setting == null) {
                result = _weiboService.Bind( new UserWeiboSetting {
                    AccessToken = key.tokenKey,
                    AccessSecret = key.tokenSecret,
                    IsSync = 1,
                    UserId = ctx.viewer.Id,
                    WeiboType = type.Id,
                    AppId = ctx.owner.Id,
                    BindTime = DateTime.Now,
                    WeiboName = type.Name,
                    WeiboUid = key.WeiboName
                } );
            }
            else {
                setting.WeiboUid = key.WeiboName;
                setting.WeiboName = type.Name;
                setting.IsSync = 1;
                setting.AccessToken = key.tokenKey;
                setting.AccessSecret = key.tokenSecret;
                setting.BindTime = DateTime.Now;
                result = _weiboService.Update( setting );
            }
            if (result.HasErrors) {
                string error = string.Empty;
                result.Errors.ForEach( e => error = error + e + System.Environment.NewLine );
                log.Error( error );
                //c.echoRedirect( "很抱歉，绑定失败，请重试", indexUrl );
            }
            else {
                ctx.web.SessionSet( "qqweibo", null );
                //c.echoRedirect( "绑定成功", indexUrl );
            }
        }

        private string getCallbackUrl( MvcContext ctx, String lnkCallback ) {
            return strUtil.Join( sys.Url.SiteUrl, lnkCallback );
        }

        private void redirectToRegister( MvcContext ctx, OauthKey key, string screenName, string profileImg ) {
            UserWeiboSetting setting = new UserWeiboSetting {
                AccessToken = key.tokenKey,
                AccessSecret = key.tokenSecret,
                IsSync = 1,
                WeiboType = type.Id,
                AppId = ctx.owner.Id,
                BindTime = DateTime.Now,
                WeiboName = type.Name,
                WeiboUid = key.WeiboName
            };
            WeiboSession session = new WeiboSession( setting, screenName, type.FriendName, profileImg );
            ctx.web.SessionSet( WeiboSession.SessionName, session );
            //ctx.web.Redirect( c.to( new WeiboRegisterController().Bind ) + "?type=" + setting.WeiboName );
        }
    }
}
