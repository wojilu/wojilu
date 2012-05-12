using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Task.Service;
using wojilu.Members.Interface;
using wojilu.Apps.Task.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;

namespace wojilu.Apps.Task.Core
{
    /// <summary>
    /// 用户上传头像任务
    /// </summary>
    [TaskName("头像任务")]
    public class UploadFaceTask : DefaultTaskProcessor
    {

        TaskService taskService;
        IUserService userService;
        public UploadFaceTask()
        {
            taskService = new TaskService();
            userService = new UserService();
        }

        public override Result CanReceiveReward(IUser user)
        {
            Result result = new Result();
            taskService = new TaskService();
            UserTaskLog log = taskService.FindLog(user.Id, Task.Id);
            if(log == null)
            {
                User u = user as User;
                if(u.Pic == UserFactory.Guest.Pic)
                {
                    result.Add("您目前还没有上传头像");
                }
            }
            else
                result.Add("您已经领取奖励了");
            return result;
        }
    }
}
