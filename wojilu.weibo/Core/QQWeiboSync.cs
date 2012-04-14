using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Interface;
using wojilu.Members.Interface;
using wojilu.weibo.Interface;
using wojilu.weibo.Domain;
using wojilu.weibo.Service;
using wojilu.weibo.Core.QQWeibo;

namespace wojilu.weibo.Core
{
   public class QQWeiboSync : IMicroblogSync
    {
        ILog log = LogManager.GetLogger(typeof(SinaWeiboSync));
        public void Sync(IUser user, string text, string picUrl)
        {
            if (user == null) return;
            if (string.IsNullOrEmpty(text) || text.Length > 140) return;
            IUserWeiboSettingService userWeiboSettingService = new UserWeiboSettingService();
            WeiboType type = WeiboType.GetByName("qqweibo");
            if (type == null) return;
           
            UserWeiboSetting setting = userWeiboSettingService.Find(user.Id, type.Id);
            //用户未绑定微博或用户暂时选择同步
            if (setting == null || setting.IsSync == 0) return;

            if (string.IsNullOrEmpty(setting.AccessToken) || string.IsNullOrEmpty(setting.AccessSecrct))
                return;
             QQWeibo.OauthKey oauthKey = new QQWeibo.OauthKey(type.AppKey,type.AppSecret,setting.AccessToken,setting.AccessSecrct);

             t twit = new t(oauthKey, "json");
            //暂时用127.0.0.1处理，因为这里得没办法得到ip值
             string ip = "127.0.0.1";
             string result;
             try
             {
             
                 if (string.IsNullOrEmpty(picUrl))
                 {
                    result= twit.add(text, ip, "", "");
                 }
                 else
                 {
                     result =  twit.add_pic(text, ip, "", "", picUrl);
                 }
             }
             catch (Exception ex)
             {
                 log.Error(ex.Message);
             }
             log.Info("同步成功");
        }
    }
}
