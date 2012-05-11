using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Task.Service;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;
using wojilu.Members.Interface;
using wojilu.Apps.Task.Domain;
using wojilu.Members.Users.Domain;

namespace wojilu.Apps.Task.Core
{
    /// <summary>
    /// 用户等级达到3级送积分
    /// </summary>
    public class SendScore3Task :DefaultTaskProcessor
    {
        TaskService taskService;
        IUserTagService tagService;
        public SendScore3Task()
        {
            taskService = new TaskService();
            tagService = new UserTagService();
        }

        public override Result CanReceiveReward(IUser user)
        {
            Result result = new Result();
            UserTaskLog log = taskService.FindLogByTime(user.Id, Task.Id,Task.Day);
            if(log == null)
            {
                User u = user as User;
                if(u.RankId < 3)
                    result.Add("亲，您的等级还不够哦");
            }
            else
                result.Add("您已经领取奖励了");
            return result;
        }
    }
}
