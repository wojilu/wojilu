using System;
using System.Collections.Generic;
using System.Text;
using wojilu.weibo.Interface;
using wojilu.Members.Users.Interface;
using wojilu.weibo.Domain;
using wojilu.weibo.Service;
using wojilu.Members.Users.Service;
using wojilu.Web.Mvc;

using wojilu.weibo.Controller.Weibo;
using wojilu.Members.Users.Domain;
using wojilu.Common;
using Newtonsoft.Json.Linq;
using wojilu.Web.Context;
using wojilu.weibo.Core.QQWeibo;

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

        public string GetAuthorizationUri( String callbackUrl ) {
            OauthKey key = new OauthKey( type.AppKey, type.AppSecret );
            bool success = false;
            try {
                success = key.GetRequestToken( callbackUrl );
            }
            catch (Exception ex) {
                log.Error( ex.Message );
            }
            if (success) {

                return key.GetOAuthUrl();

            }
            else {
                return null;
            }
        }

        public Result ProcessCallback( int userId, String verifier, String callbackUrl ) {

            Result result = new Result();

            OauthKey key = new OauthKey( type.AppKey, type.AppSecret );

            Boolean success = false;
            try {
                success = key.GetAccessToken( callbackUrl, verifier );
            }
            catch (Exception ex) {
                log.Error( ex.Message );
                result.Add( ex.Message );
                return result;
            }

            if (!success) {
                result.Add( "绑定错误" );
                return result;
            }

            UserWeiboSetting setting = _weiboService.Find( type.Id, key.tokenKey, key.tokenSecret );

            //如果用户已经微博绑定此帐户
            if (setting != null) {
                result.Add( "对不起，已经绑定" );
                return result;
            }

            user qqUser = new user( key, "json" );

            JToken weiboInfo = qqUser.info();

            setting = _weiboService.Find( userId, type.Id );

            if (setting == null) {
                result = _weiboService.Bind( new UserWeiboSetting {
                    AccessToken = key.tokenKey,
                    AccessSecret = key.tokenSecret,
                    IsSync = 1,
                    UserId = userId,
                    WeiboType = type.Id,
                    AppId = 0,
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
                log.Error( result.ErrorsText );
            }

            return result;
        }
    }
}
