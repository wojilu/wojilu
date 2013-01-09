using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.weibo.Interface;
using wojilu.weibo.Service;
using wojilu.Web;
using wojilu.weibo.Domain;

namespace wojilu.weibo.Controller.Weibo.Admin
{
    public class UserWeiboSettingController : ControllerBase
    {
        IUserWeiboSettingService _weiboService;

        public UserWeiboSettingController()
        {
            _weiboService = new UserWeiboSettingService();
        }

        public void Index()
        {
            IBlock block = getBlock("list");
            int current = ctx.GetInt("page");
            if (current == 0)
            {
                current++;
            }
            DataPage<UserWeiboSetting> page = _weiboService.FindByPage(string.Empty, current, 20);
            foreach (UserWeiboSetting setting in page.Results)
            {
                block.Set("s.id", setting.Id);
                block.Set("s.userId", setting.UserId);
                block.Set("s.weiboName", setting.WeiboName);
                block.Next();
            }
            string tip = string.Empty;
            if (page.RecordCount == 0)
            {
                tip = "暂时没有用户进行微博绑定";
            }
            set("tip", tip);
            set("totalCount", page.RecordCount);
            set("pageBar", page.PageBar);
        }
    }
}
