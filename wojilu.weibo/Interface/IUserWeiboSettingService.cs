using System;
using System.Collections.Generic;
using System.Text;
using wojilu.weibo.Domain;

namespace wojilu.weibo.Interface
{
    public interface IUserWeiboSettingService
    {
        Result Bind(UserWeiboSetting setting);

        Result Update(UserWeiboSetting setting);

        int Unbind(int userId, int weiboTypeId);

        void Sync(int userId, int weiboTypeId);

        void UnSync(int userId, int weiboTypeId);

        bool ExistBy(int userId, int weiboTypeId);

        UserWeiboSetting Find(int weiboTypeId, string tokenKey, string tokenSecret);

        IList<UserWeiboSetting> FindByUserId(int userId);

        UserWeiboSetting Find(int userId, int weiboType);

        int Count();

        DataPage<UserWeiboSetting> FindByPage(string condition, int current, int pageSize);
    }
}
