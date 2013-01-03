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

namespace wojilu.weibo.Core
{
   public class SinaOAuthRequestStrategy : IOAuthRequestStrategy
    {
        private static ILog log = LogManager.GetLogger(typeof(SinaOAuthRequestStrategy));

        IUserWeiboSettingService _weiboService;

        IUserService userService;

        ILoginService loginService;

        WeiboType type;

        public SinaOAuthRequestStrategy()
        {
            type = WeiboType.GetByName(SupportWeiboType.Sina);
            if (type == null)
                throw new Exception("新浪微博配置文件找不到");
            _weiboService = new UserWeiboSettingService();
            userService = new UserService();
            loginService = new LoginService();
        }

        public void RedirectToAuthorizationUri(ControllerBase controller)
        {
            SinaWeibo w = new SinaWeibo(type.AppKey, type.AppSecret);
            controller.redirectUrl(w.GetAuthorizationUri(getCallbackUrl(controller)));
        }

        public void ProcessCallback(ControllerBase controller)
        {
            string code = controller.ctx.Get("code");
            if (string.IsNullOrEmpty(code))
            {
                controller.echoRedirect("请不要直接进入此页面");
                return;
            }
            SinaWeibo w = new SinaWeibo(type.AppKey, type.AppSecret);

            SinaOAuthAccessToken token = w.GetAccessTokenByAuthorizationCode(code,getCallbackUrl(controller));

            UserWeiboSetting setting = _weiboService.Find(type.Id, token.Token, string.Empty);

            //如果用户已经微博绑定此帐户
            if (setting != null)
            {
                //这里做用户登陆处理
                User user = userService.GetById(setting.UserId);
                if (user != null)
                {
                    loginService.Login(user, LoginTime.OneMonth, controller.ctx.Ip, controller.ctx);
                    controller.redirectUrl("/");
                    return;
                }
                else
                {
                    controller.echoRedirect("发生未知错误，请重试");
                    return;
                }
            }

            w.SetToken(token.Token);
            string indexUrl = controller.to(new UserWeiboSettingController().Index);

            Data.Sina.User.UserInfo weiboUser = w.GetUserInfo(long.Parse(token.UserID));
            if (weiboUser == null)
            {
                controller.echoRedirect("很抱歉，获取失败，请重试",indexUrl);
                return;
            }
            //用户是未登陆，那就是用户通过微博注册或是通过此微博直接登陆
            if (!controller.ctx.viewer.IsLogin)
            {
                redirectSinaWeiboRegister(controller,token, weiboUser.ScreenName, weiboUser.ProfileImageUrl);
                return;
            }
            //判断用户是否已经绑定了微博，没有绑定则添加，否则更新token
            setting = _weiboService.Find(controller.ctx.viewer.Id, type.Id);
            Result result;
            if (setting == null)
            {
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
            setting.UserId = controller.ctx.viewer.Id;

            if (setting.Id == default(int))
                result = _weiboService.Bind(setting);
            else
                result = _weiboService.Update(setting);

            if (result.HasErrors)
            {
                string error = string.Empty;
                result.Errors.ForEach(c => error = error + c + System.Environment.NewLine);
                log.Error(error);
                controller.echoRedirect("很抱歉，绑定失败，请重试",indexUrl);
            }
            else
            {
                controller.echoRedirect("绑定成功");
            }
        }

        private string getCallbackUrl(ControllerBase controller)
        {
            return controller.ctx.url.SiteUrl.TrimEnd('/') + controller.to(new UserWeiboSettingController().SinaWeiboCallback);
        }

        private void redirectSinaWeiboRegister(ControllerBase c,SinaOAuthAccessToken token, string screenName, string profileImg)
        {
            UserWeiboSetting setting = new UserWeiboSetting
            {
                AccessToken = token.Token,
                ExpireIn = token.ExpiresIn,
                IsSync = 1,
                WeiboUid = token.UserID,
                RefreshToken = token.RefreshToken,
                WeiboType = type.Id,
                AppId = c.ctx.owner.Id,
                BindTime = DateTime.Now,
                WeiboName = type.Name
            };
            WeiboSession session = new WeiboSession(setting, screenName, type.FriendName, profileImg);
            c.ctx.web.SessionSet(WeiboSession.SessionName, session);
            c.redirectUrl(c.to(new WeiboRegisterController().Bind) + "?type=" + setting.WeiboName);
        }
    }
}
