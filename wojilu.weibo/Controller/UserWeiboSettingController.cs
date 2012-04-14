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

namespace wojilu.weibo.Controller.Weibo
{
    public class UserWeiboSettingController : ControllerBase
    {
        IUserWeiboSettingService _weiboService;
        ILog log = LogManager.GetLogger(typeof(UserWeiboSettingController));
        
        public UserWeiboSettingController()
        {
            _weiboService = new UserWeiboSettingService();
        }

        //bug？ ctx.viewer=null
        public override void CheckPermission()
        {
           
        }

        public void Index()
        {
            IBlock block = getBlock("list");
            List<WeiboType> types = WeiboType.GetAll();
            foreach (WeiboType a in types)
            {
                block.Set("w.Name", a.Name);
                block.Set("w.Id", a.Id);
                block.Set("w.Logo", a.Logo);
                block.Set("w.AuthUrl", a.AuthUrl);
                block.Set("w.CallbackUrl", a.CallbackUrl);
                block.Set("w.ConnectUrl", to(Connect, a.Id));
                block.Next();
            }
            
        }

        public void Connect(int id)
        {
            WeiboType type = WeiboType.GetById(id);
            if (type==null)
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
                    
                case "qqspace":break;
                case "163weibo":break;
                 
                default:
                    break;
            }
        }

        private void processQQWeibo(WeiboType type)
        {
            if(!ctx.web.UserIsLogin){
                redirectLogin();
                return;
            }
            OauthKey key = new OauthKey(type.AppKey,type.AppSecret);
            bool success= false;
            try{
                string callback = ctx.url.SiteUrl.TrimEnd('/') + to(QQWeiboCallback);
             success =  key.GetRequestToken(callback);
            }
            catch(Exception ex){
                log.Error(ex.Message);
            }
            if (success) {
                ctx.web.SessionSet("qqweibo",key);
                redirectUrl(key.GetOAuthUrl());
            }
            else{
                echoRedirect("绑定失败，请重试");
            }
        }

        public void QQWeiboCallback()
        {
            OauthKey key = ctx.web.SessionGet("qqweibo") as OauthKey;
            if (key==null) {
               echoError("请不要直接访问此页面");
            }
            WeiboType type = WeiboType.GetByName("qqweibo");
            if (type==null)
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
                success =  key.GetAccessToken( verifier);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            if (key==null)
            {
                echoRedirect("绑定失败，请重试",to(Index));
                return;
            }
            UserWeiboSetting setting = _weiboService.Find(ctx.viewer.Id, type.Id);
            Result result;
            if (setting == null)
            {
                result = _weiboService.Bind(new UserWeiboSetting
                {
                     AccessToken = key.tokenKey,
                     AccessSecrct = key.tokenSecret,
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
                setting.AccessSecrct = key.tokenSecret;
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
                echoRedirect("绑定成功",to(Index));
            }
        }

        [Login]
        public void SinaWeiboCallback()
        {
            string code = ctx.Get("code");
            if (string.IsNullOrEmpty(code))
            {
                return;
            }
            WeiboType type = WeiboType.GetByName("sina");
            if (type==null)
            {
                log.Error("找不到 sina 的 WeiboType,请添加");
                return;
            }
            SinaWeibo w = new SinaWeibo(type.AppKey, type.AppSecret);
            Common.SinaOAuthAccessToken token= w.GetAccessTokenByAuthorizationCode(code, ctx.url.SiteUrl.TrimEnd('/')+ to(SinaWeiboCallback));
            w.SetToken(token.Token);
            //测试一下
            Data.Sina.User.UserInfo weiboUser = w.GetUserInfo(long.Parse(token.UserID));
           if (weiboUser != null)
           {
               //判断用户是否已经绑定了微博，没有绑定则添加，否则更新token
               UserWeiboSetting setting = _weiboService.Find(ctx.viewer.Id, type.Id);
               Result result;
               if (setting == null)
               {
                   result = _weiboService.Bind(new UserWeiboSetting
                   {
                       AccessToken = token.Token,
                       ExpireIn = token.ExpiresIn,
                       IsSync = 1,
                       UserId = ctx.viewer.Id,
                       WeiboUid = token.UserID,
                       RefreshToken = token.RefreshToken,
                       WeiboType = type.Id,
                       AppId = ctx.owner.Id,
                       BindTime = DateTime.Now,
                       WeiboName = type.Name
                   });
               }
               else
               {
                   setting.WeiboName = type.Name;
                   setting.IsSync = 1;
                   setting.AccessToken = token.Token;
                   setting.RefreshToken = token.RefreshToken;
                   setting.ExpireIn = token.ExpiresIn;
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
                   echoRedirect("绑定成功");
               }
           }
           else
           {
               echoRedirect("很抱歉，绑定失败，请重试", to(Index));
           }
        }
    }
}
