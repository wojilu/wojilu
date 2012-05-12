using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Task.Domain;
using wojilu.Apps.Task.Service;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;
using wojilu.Common.Money.Interface;
using wojilu.Common.Money.Service;
using wojilu.Data;
using wojilu.Web;

namespace wojilu.Apps.Task.Core
{
    /// <summary>
    /// 问答任务,同类型的任务都可以采用此类
    /// </summary>
    [TaskName("问答任务")]
    public class AnswerTask : DefaultTaskProcessor
    {
        ILog logger = LogManager.GetLogger(typeof(AnswerTask));
        TaskService taskService ;
        public AnswerTask()
        {
            taskService = new TaskService();
        }

        public override Result CanReceiveReward(Members.Interface.IUser user)
        {
            Result result = new Result();
            UserTaskLog log = taskService.FindLogByTime(user.Id, Task.Id, Task.Day);
            if(log != null)
            {
                result.Add("此任务您已经领取过奖励了,上次领取时间: " + log.Created.ToString("yyyy-MM-dd HH:mm"));
            }
            else
            {
                result.Add("请先完成此任务!");
            }
            return result;
        }

        public override void ProcessTask(ControllerBase c)
        {
            string answer = c.ctx.Post("answer");
            if(string.IsNullOrEmpty(answer))
            {
                c.ctx.errors.Add("你的答案不正确");
                return;
            }
            if(Task.Enable == 0)
            {
                c.ctx.errors.Add("该任务尚未开启");
                return;
            }
            if(answer != Task.Answer)
            {
                c.ctx.errors.Add("你的答案不正确");
                return;
            }
            if(c.ctx.HasErrors)
                return;
            else
                base.ProcessTask(c);
        }

        protected override string templateFile
        {
            get
            {
                return "_defaultWithInput.html";
            }
        }
    }
}
