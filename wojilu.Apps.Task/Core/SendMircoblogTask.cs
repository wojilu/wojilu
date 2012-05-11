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
namespace wojilu.Apps.Task.Core
{
    /// <summary>
    /// 用户每日发微博任务
    /// </summary>
    public class SendMircoblogTask : DefaultTaskProcessor
    {
        INotificationService notifyService;

        TaskService taskService;
        IMicroblogService mblogService;
        static readonly ILog logger = LogManager.GetLogger(typeof(SendMircoblogTask));

        public SendMircoblogTask()
        {
            notifyService = new NotificationService();
            taskService = new TaskService();
            mblogService = new MicroblogService();
        }

        public override Result CanReceiveReward(IUser user)
        {
            Result result = new Result();
            UserTaskLog log = taskService.FindLogByTime(user.Id, Task.Id,Task.Day);
            if( log==null )
            {
                int count = mblogService.CountByUserTime(user.Id,"today");
                if(count == 0)
                {
                    result.Add("亲,您今天还没有发微博哦。");
                }
            }
            else
            {
                result.Add("此任务您已经领取过奖励了,上次领取时间: " + log.Created.ToString("yyyy-MM-dd HH:mm"));
            }
            return result;
        }

        //删除采用事件订阅的方法进行处理
        //public override void Subscribe(IEventAggregator eventAggregator)
        //{
        //    eventAggregator.GetEvent<MicroblogCreatedEvent>().Subscribe(microblog_Created);
        //}

        //private void microblog_Created(Microblog blog)
        //{
        //    TaskService tService = new TaskService();
        //    IMicroblogService mService = new MicroblogService();

        //    TaskInfo task = tService.FindTaskById(TaskId);
        //    if(task == null || task.Enable == 0)
        //        return;

        //    if(mService.CountByUser(blog.Creator.Id) > 0)
        //    {

        //        UserTaskLog log = tService.FindLog(blog.Creator.Id, TaskId);
        //        if(log == null)
        //        {
        //            log = new UserTaskLog {
        //                Created = DateTime.Now,
        //                UserId = blog.Creator.Id,
        //                State = 1,
        //                TaskId = TaskId
        //            };
        //            Result result = log.insert();
        //            if(result.IsValid)
        //                notifyService.send(log.UserId, string.Format("恭喜完成任务——{0}，我记录为您准备了 {1} 奖励，快去{2}领奖嘀哒！", task.Name, task.GetRewardHtml(), "<a href='/Task/Index.aspx'>任务中心</a>"));
        //        }
        //    }
        //}
    }
}
