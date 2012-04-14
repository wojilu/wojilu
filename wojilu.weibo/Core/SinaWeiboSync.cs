using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Feeds.Interface;
using wojilu.weibo.Domain;
using wojilu.weibo.Interface;
using wojilu.weibo.Service;
using wojilu.weibo.Data.Sina;
using wojilu.weibo.Data.Sina.Status;
using wojilu.weibo.Common;
using wojilu.Common.Feeds.Service;
using wojilu.Members.Users.Domain;
using wojilu.Web.Mvc;
using System.Web;
using wojilu.Common.Interface;
using wojilu.Members.Interface;

namespace wojilu.weibo.Core
{
    public class SinaWeiboSync : IMicroblogSync
    {
        ILog log = LogManager.GetLogger(typeof(SinaWeiboSync));
        public void Sync(IUser user, string text, string picUrl)
        {
            if (user == null) return;
            if (string.IsNullOrEmpty(text)||text.Length>140) return;
            IUserWeiboSettingService userWeiboSettingService = new UserWeiboSettingService();
            WeiboType type = WeiboType.GetByName("sina");
            SinaWeibo weibo = new SinaWeibo(type.AppKey, type.AppSecret);
            if (type == null) return;
            UserWeiboSetting setting = userWeiboSettingService.Find(user.Id, type.Id);
            //用户未绑定微博或用户暂时选择同步
            if (setting == null || setting.IsSync==0) return;
            //如果token过期而refreshToken不为空的话，重新得到token,这里主要是新浪oauth2.0api的refresh token需要通过新浪认证后才能得到
            if (setting.IsExpire && !string.IsNullOrEmpty(setting.RefreshToken))
            {
                try
                {
                    SinaOAuthAccessToken token = weibo.RefreshAccessToken(setting.RefreshToken);
                    setting.AccessToken = token.Token;
                    setting.RefreshToken = token.RefreshToken;
                    setting.ExpireIn = token.ExpiresIn;
                    userWeiboSettingService.Update(setting);
                }
                catch (SinaWeiboException ex)
                {
                    log.Error(ex.Message);
                }
            }
            if (!setting.IsExpire)
            {
                weibo.SetToken(setting.AccessToken);
                try
                {
                    //发送消息
                    if (string.IsNullOrEmpty(picUrl))
                    {
                        weibo.PostStatus(new UpdateStatusInfo() { Status = text });
                    }
                    else
                    {
                        weibo.PostStatusWithPic(new UpdateStatusWithPicInfo { Status = text, Pic = picUrl });
                    }
                }
                catch (SinaWeiboException ex)
                {
                    log.Error(ex.Message);
                }
            }
        }
    }
}
