using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;
using wojilu.weibo.Data.Sina;
using wojilu.Web.Mvc;
using wojilu.weibo.Controller.Weibo;
using wojilu.weibo.Domain;
using wojilu.weibo.Interface;
using wojilu.weibo.Service;
using wojilu.weibo.Common;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;
using wojilu.Common;

namespace wojilu.weibo.Core {

    public class SinaOAuthRequestStrategy : IOAuthRequestStrategy {

        private static ILog log = LogManager.GetLogger( typeof( SinaOAuthRequestStrategy ) );

        IUserWeiboSettingService _weiboService;

        IUserService userService;

        ILoginService loginService;

        WeiboType type;

        public SinaOAuthRequestStrategy() {
            type = WeiboType.GetByName( SupportWeiboType.Sina );
            if (type == null)
                throw new Exception( "新浪微博配置文件找不到" );
            _weiboService = new UserWeiboSettingService();
            userService = new UserService();
            loginService = new LoginService();
        }

        public string GetAuthorizationUri( String callbackUrl ) {
            SinaWeibo w = new SinaWeibo( type.AppKey, type.AppSecret );
            return w.GetAuthorizationUri( callbackUrl );
        }

        public Result ProcessCallback( int userId, String code, String callbackUrl ) {

            Result result = new Result();

            SinaWeibo w = new SinaWeibo( type.AppKey, type.AppSecret );

            SinaOAuthAccessToken token = w.GetAccessTokenByAuthorizationCode( code, callbackUrl );

            UserWeiboSetting setting = _weiboService.Find( type.Id, token.Token, string.Empty );

            //如果用户已经微博绑定此帐户
            if (setting != null) {
                result.Add( "对不起，已经绑定" );
                return result;
            }

            w.SetToken( token.Token );

            Data.Sina.User.UserInfo weiboUser = w.GetUserInfo( long.Parse( token.UserID ) );
            if (weiboUser == null) {
                result.Add( "很抱歉，获取失败，请重试" );
                return result;
            }

            //判断用户是否已经绑定了微博，没有绑定则添加，否则更新token
            setting = _weiboService.Find( userId, type.Id );

            if (setting == null) {
                setting = new UserWeiboSetting();
            }

            setting.WeiboUid = token.UserID;
            setting.RefreshToken = token.RefreshToken;
            setting.WeiboType = type.Id;
            setting.WeiboName = type.Name;
            setting.IsSync = 1;
            setting.AccessToken = token.Token;
            setting.RefreshToken = token.RefreshToken;
            setting.ExpireIn = token.ExpiresIn;
            setting.BindTime = DateTime.Now;
            setting.UserId = userId;

            if (setting.Id == default( int ))
                result = _weiboService.Bind( setting );
            else
                result = _weiboService.Update( setting );

            if (result.HasErrors) {
                log.Error( result.ErrorsText );
            }

            return result;
        }

    }
}
