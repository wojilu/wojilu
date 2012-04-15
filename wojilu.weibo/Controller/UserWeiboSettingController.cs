using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.weibo.Interface;
using wojilu.weibo.Service;
using wojilu.Web;
using wojilu.weibo.Domain;
using wojilu.weibo.Data.Sina;
using wojilu.Members.Interface;
using wojilu.Web.Mvc.Attr;
using wojilu.weibo.Core.QQWeibo;
using wojilu.weibo.Core;
using Newtonsoft.Json.Linq;
using wojilu.Members.Users.Service;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Common;

namespace wojilu.weibo.Controller.Weibo
{
    public class UserWeiboSettingController : ControllerBase
    {
        IUserWeiboSettingService _weiboService;
        public ILoginService loginService { get; set; }
        ILog log = LogManager.GetLogger(typeof(UserWeiboSettingController));

        public IUserService userService { get; set; }

        public UserWeiboSettingController()
        {
            userService = new UserService();
            loginService = new LoginService();
            _weiboService = new UserWeiboSettingService();
        }

        [Login]
        public void Index()
        {
            set("sinaLink", to(new WeiboRegisterController().Connect) + "?type=sina");
            set("qqweiboLink", to(new WeiboRegisterController().Connect) + "?type=qqweibo");
            IBlock block = getBlock("list");
            List<WeiboType> types = WeiboType.GetAll();
            foreach (WeiboType a in types)
            {

                UserWeiboSetting _setting = _weiboService.Find(ctx.viewer.Id, a.Id);
                IBlock unSyncBlock = block.GetBlock("unSync");
                IBlock syncBlock = block.GetBlock("sync");
                if (_setting == null)
                {

                    unSyncBlock.Set("w.Logo", a.Logo);
                    unSyncBlock.Set("w.Name", a.FriendName);
                    unSyncBlock.Set("w.ConnectUrl", to(Connect, a.Id));
                    unSyncBlock.Next();
                }
                else
                {

                    syncBlock.Set("w.Logo", a.Logo);
                    syncBlock.Set("w.Name", a.FriendName);
                    string isSync = _setting.IsSync == 1 ? "checked='selected'" : string.Empty;
                    syncBlock.Set("w.isSync", isSync);
                    syncBlock.Set("w.UnbindLink", to(Unbind, a.Id));
                    syncBlock.Set("w.SyncLink", to(Sync, a.Id));
                    syncBlock.Next();
                }
                block.Next();
            }
        }

        [Login]
        [HttpDelete]
        public void Unbind(int id)
        {
            UserWeiboSetting setting = _weiboService.Find(ctx.viewer.Id, id);
            if (setting == null)
            {
                echoRedirect("你并没绑定该微博", to(Index));
            }
            else
            {
                if (setting.delete() == 1)
                    echoRedirect("你已经解绑了该微博,操作成功!", to(Index));
                else
                    echoRedirect("操作失败,请重试", to(Index));
            }
        }

        [Login]
        [HttpPost]
        public void Sync(int id)
        {
            UserWeiboSetting setting = _weiboService.Find(ctx.viewer.Id, id);
            int isSync = ctx.PostInt("isSync");
            if (setting == null)
            {
                echoError("你并没绑定该微博");
            }
            else
            {

                if (isSync == 0)
                {
                    _weiboService.UnSync(ctx.viewer.Id, id);
                }
                else
                    _weiboService.Sync(ctx.viewer.Id, id);

                echoAjaxOk();
            }
        }

        public void Connect(int id)
        {
            WeiboType type = WeiboType.GetById(id);
            if (type == null)
            {
                echoRedirect("not found");
                return;
            }
            switch (type.Name.ToLower())
            {
                case "sina":
                    SinaWeibo w = new SinaWeibo(type.AppKey, type.AppSecret);
                    redirectUrl(w.GetAuthorizationUri(ctx.url.SiteUrl.TrimEnd('/') + to(SinaWeiboCallback)));
                    break;
                case "qqweibo":
                    processQQWeibo(type);
                    break;

                case "qqspace": break;
                case "163": break;

                default:
                    break;
            }
        }

        private void processQQWeibo(WeiboType type)
        {
            OauthKey key = new OauthKey(type.AppKey, type.AppSecret);
            bool success = false;
            try
            {
                string callback = ctx.url.SiteUrl.TrimEnd('/') + to(QQWeiboCallback);
                success = key.GetRequestToken(callback);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            if (success)
            {
                ctx.web.SessionSet("qqweibo", key);
                redirectUrl(key.GetOAuthUrl());
            }
            else
            {
                echoRedirect("操作失败，请重试");
            }
        }

        public void QQWeiboCallback()
        {
            OauthKey key = ctx.web.SessionGet("qqweibo") as OauthKey;
            if (key == null)
            {
                echoError("请不要直接访问此页面");
                return;
            }
            WeiboType type = WeiboType.GetByName("qqweibo");
            if (type == null)
            {
                log.Error("找不到 qq 的 WeiboType,请添加");
                return;
            }
            string verifier = ctx.Get("oauth_verifier");
            if (string.IsNullOrEmpty(verifier))
            {
                echoError("请不要直接访问此页面");
                return;
            }
            bool success = false;
            try
            {
                success = key.GetAccessToken(verifier);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            UserWeiboSetting setting = _weiboService.Find(type.Id, key.tokenKey, key.tokenSecret);

            //如果用户已经微博绑定此帐户
            if (setting != null)
            {
                //这里做用户登陆处理
                User user = userService.GetById(setting.UserId);
                if (user != null)
                {
                    loginService.Login(user, LoginTime.OneMonth, ctx.Ip, ctx);
                    redirectUrl("/");
                    return;
                }
                else
                {
                    echoRedirect("发生未知错误，请重试");
                    return;
                }
            }

            wojilu.weibo.Core.QQWeibo.user qqUser = new user(key,"json");

            JToken weiboInfo = qqUser.info();

            if (!ctx.viewer.IsLogin)
            {
                redirectQQWeiboRegister(type, key,weiboInfo["data"]["name"].ToString(),weiboInfo["data"]["head"].ToString());
                return;
            }

             setting = _weiboService.Find(ctx.viewer.Id, type.Id);
            Result result;
            if (setting == null)
            {
                result = _weiboService.Bind(new UserWeiboSetting
                {
                    AccessToken = key.tokenKey,
                    AccessSecret = key.tokenSecret,
                    IsSync = 1,
                    UserId = ctx.viewer.Id,
                    WeiboType = type.Id,
                    AppId = ctx.owner.Id,
                    BindTime = DateTime.Now,
                    WeiboName = type.Name,
                    WeiboUid = key.WeiboName
                });
            }
            else
            {
                setting.WeiboUid = key.WeiboName;
                setting.WeiboName = type.Name;
                setting.IsSync = 1;
                setting.AccessToken = key.tokenKey;
                setting.AccessSecret = key.tokenSecret;
                setting.BindTime = DateTime.Now;
                result = _weiboService.Update(setting);
            }
            if (result.HasErrors)
            {
                errors.Errors.AddRange(result.Errors);
                string error = string.Empty;
                result.Errors.ForEach(c => error = error + c + System.Environment.NewLine);
                log.Error(error);
                echoRedirect("很抱歉，绑定失败，请重试", to(Index));
            }
            else
            {
                ctx.web.SessionSet("qqweibo", null);
                echoRedirect("绑定成功", to(Index));
            }
        }

        public void SinaWeiboCallback()
        {
            string code = ctx.Get("code");
            if (string.IsNullOrEmpty(code))
            {
                return;
            }
            WeiboType type = WeiboType.GetByName("sina");
            if (type == null)
            {
                log.Error("找不到 sina 的 WeiboType,请添加");
                return;
            }
            SinaWeibo w = new SinaWeibo(type.AppKey, type.AppSecret);
            Common.SinaOAuthAccessToken token = w.GetAccessTokenByAuthorizationCode(code, ctx.url.SiteUrl.TrimEnd('/') + to(SinaWeiboCallback));

            UserWeiboSetting setting = _weiboService.Find(type.Id, token.Token,string.Empty);

            //如果用户已经微博绑定此帐户
            if (setting != null)
            {
                //这里做用户登陆处理
                User user = userService.GetById(setting.UserId);
                if (user != null)
                {
                    loginService.Login(user, LoginTime.OneMonth, ctx.Ip, ctx);
                    redirectUrl("/");
                    return;
                }
                else
                {
                    echoRedirect("发生未知错误，请重试");
                    return;
                }
            }

            w.SetToken(token.Token);

            Data.Sina.User.UserInfo weiboUser = w.GetUserInfo(long.Parse(token.UserID));
            if (weiboUser == null)
            {
                echoRedirect("很抱歉，获取失败，请重试", to(Index));
                return;
            }
            //用户是未登陆，那就是用户通过微博注册或是通过此微博直接登陆
            if (!ctx.viewer.IsLogin)
            {
                redirectSinaWeiboRegister(type, token,weiboUser.ScreenName,weiboUser.ProfileImageUrl);
                return;
            }
            //判断用户是否已经绑定了微博，没有绑定则添加，否则更新token
             setting = _weiboService.Find(ctx.viewer.Id, type.Id);
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
            setting.UserId = ctx.viewer.Id;

            if (setting.Id==default(int))
                result = _weiboService.Bind(setting);
            else
                result = _weiboService.Update(setting);

            if (result.HasErrors)
            {
                errors.Errors.AddRange(result.Errors);
                string error = string.Empty;
                result.Errors.ForEach(c => error = error + c + System.Environment.NewLine);
                log.Error(error);
                echoRedirect("很抱歉，绑定失败，请重试", to(Index));
            }
            else
            {
                echoRedirect("绑定成功");
            }

        }

        private void redirectQQWeiboRegister(WeiboType type, OauthKey key,string screenName,string profileImg)
        {
            UserWeiboSetting setting = new UserWeiboSetting
              {
                  AccessToken = key.tokenKey,
                  AccessSecret = key.tokenSecret,
                  IsSync = 1,
                  WeiboType = type.Id,
                  AppId = ctx.owner.Id,
                  BindTime = DateTime.Now,
                  WeiboName = type.Name,
                  WeiboUid = key.WeiboName
              };
            WeiboSession session = new WeiboSession(setting, screenName, type.FriendName, profileImg);
            ctx.web.SessionSet(WeiboSession.SessionName, session);
            redirectUrl(to(new WeiboRegisterController().Bind) + "?type=" + setting.WeiboName);
        }

        private void redirectSinaWeiboRegister(WeiboType type, Common.SinaOAuthAccessToken token,string screenName,string profileImg)
        {
            UserWeiboSetting setting = new UserWeiboSetting
            {
                AccessToken = token.Token,
                ExpireIn = token.ExpiresIn,
                IsSync = 1,
                WeiboUid = token.UserID,
                RefreshToken = token.RefreshToken,
                WeiboType = type.Id,
                AppId = ctx.owner.Id,
                BindTime = DateTime.Now,
                WeiboName = type.Name
            };
            WeiboSession session = new WeiboSession(setting,screenName,type.FriendName,profileImg);
            ctx.web.SessionSet(WeiboSession.SessionName,session);
            redirectUrl(to(new WeiboRegisterController().Bind) + "?type=" + setting.WeiboName);
        }
    }
}
