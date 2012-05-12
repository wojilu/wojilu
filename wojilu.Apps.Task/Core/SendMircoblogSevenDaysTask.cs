using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Task.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Common.Microblogs.Interface;
using wojilu.Common.Microblogs.Service;
using wojilu.Common.Microblogs.Domain;
using wojilu.Apps.Task.Service;
using wojilu.Common.Msg.Interface;
using wojilu.Common.Msg.Service;
using wojilu.Data;
using wojilu.Members.Interface;
using wojilu.Web;
namespace wojilu.Apps.Task.Core
{
    /// <summary>
    /// 用户连续7天发微博任务
    /// </summary>
    [TaskName("连续7天发微博任务")]
    public class SendMircoblogSevenDaysTask : DefaultTaskProcessor
    {

        TaskService taskService;
        static readonly ILog logger = LogManager.GetLogger(typeof(SendMircoblogSevenDaysTask));

        private const int sendMicroBlogTaskId = 1;

        public SendMircoblogSevenDaysTask()
        {
            taskService = new TaskService();
        }

        protected override string templateFile
        {
            get
            {
                return "_lianxu.html";
            }
        }

        protected override void SetVar(Template template,ControllerBase c)
        {
           int days =  getContinusousDays(c.ctx.viewer.Id);
           template.Set("tip", "<span>亲，你要加油哦，你已经连续<strong>" + days + "天</strong>发送微博了！</span>");
        }

        private int getContinusousDays(int userId)
        {
            //用户连续完成任务1的天数
            int continuousDays = 0;
            List<UserTaskLog> logs = taskService.FindLogsByDays(userId, sendMicroBlogTaskId, 7);
            if(logs.Count > 0)
                continuousDays = 1;
            for(int i = 0;i < logs.Count;i++)
            {
                if((i + 1) >= logs.Count)
                    break;
                DateTime currentTime = logs[i].Created;
                DateTime nextTime = logs[i + 1].Created;
                if(currentTime.ToShortDateString() == nextTime.AddDays(1).ToShortDateString())
                    continuousDays++;
                else
                {
                    break;
                }
            }
            return continuousDays;
        }

        public override Result CanReceiveReward(IUser user)
        {
            
            Result result = new Result();
            UserTaskLog log = taskService.FindLogByTime(user.Id, Task.Id,Task.Day);
            if( log==null )
            {
                //用户连续完成任务1的天数
                int continuousDays = getContinusousDays(user.Id);
             
                if(continuousDays < 7)
                {
                    result.Add("<span>亲，你要加油哦，你已经连续<strong>"+continuousDays+"天</strong>发送微博了！</span>");
                }
            }
            else
            {
                result.Add("此任务您已经领取过奖励了,上次领取时间: " + log.Created.ToString("yyyy-MM-dd HH:mm"));
            }
            return result;
        }
    }
}
