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
                     redirectUrl(w.GetAuthorizationUri(type.CallbackUrl));
                    break;
                case "qqweibo":break;
                case "qqspace":break;
                case "163weibo":break;
                 
                default:
                    break;
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
