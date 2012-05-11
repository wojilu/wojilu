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
    /// 用户填写标签任务
    /// </summary>
    public class WriteUserTagTask :DefaultTaskProcessor
    {
        TaskService taskService;
        IUserTagService tagService;
        public WriteUserTagTask()
        {
            taskService = new TaskService();
            tagService = new UserTagService();
        }

        public override Result CanReceiveReward(IUser user)
        {
            Result result = new Result();
            UserTaskLog log = taskService.FindLog(user.Id, Task.Id);
            if(log == null)
            {
                int count = UserTagShip.count("UserId=" + user.Id);
                if(count < 3)
                    result.Add("你的个人标签数还未达到3个以上哦");
            }
            else
                result.Add("您已经领取奖励了");
            return result;
        }
    }
}
