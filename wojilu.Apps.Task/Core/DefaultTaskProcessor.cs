using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Task.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Task.Service;
using wojilu.Web;
using wojilu.Members.Interface;
using wojilu.Common.Money.Interface;
using wojilu.Common.Money.Service;
using wojilu.Data;
using wojilu.Web.Controller;
using wojilu.Common.Msg.Interface;
using wojilu.Common.Msg.Service;
namespace wojilu.Apps.Task.Core
{
    public abstract class DefaultTaskProcessor : ITaskProcessor
    {

        ILog logger = LogManager.GetLogger(typeof(DefaultTaskProcessor));
        INotificationService notifyService;
        IUserIncomeService incomeService ;
        TaskService taskService ;

        public DefaultTaskProcessor()
        {
            notifyService = new NotificationService();
            incomeService = new UserIncomeService();
            taskService = new TaskService();
        }
        public virtual Result CanReceiveReward(IUser user)
        {
            Result result = new Result();
            UserTaskLog log = taskService.FindLogByTime(user.Id, Task.Id,Task.Day);
            if(log != null)
            {
                result.Add("此任务您已经领取过奖励了,上次领取时间: " + log.Created.ToString("yyyy-MM-dd HH:mm"));
            }
            return result;
        }

        public virtual void GetReward(ControllerBase c)
        {
            
            UserTaskLog log = taskService.FindLogByTime(c.ctx.viewer.Id, Task.Id, Task.Day);

            if(log != null)
            {
                c.ctx.errors.Add("亲，你已经领过奖励了,上次领取时间：" + log.Created.ToString("yyyy-MM-dd HH:mm"));
                return;
            }
            DbContext.beginTransactionAll();
            try
            {
                if(log != null)
                {
                    log.Created = DateTime.Now;
                    log.State = 1;
                    log.update();
                }
                else
                {
                    log = new UserTaskLog {
                        Created = DateTime.Now,
                        UserId = c.ctx.viewer.Id,
                        State = 1,
                        TaskId = Task.Id
                    };
                    log.insert();
                }
                if(Task.CurrencyId1 > 0 && Task.Reward1 > 0)
                    incomeService.AddIncome(c.ctx.viewer.obj as User, Task.CurrencyId1, Task.Reward1);
                if(Task.CurrencyId2 > 0 && Task.Reward2 > 0)
                    incomeService.AddIncome(c.ctx.viewer.obj as User, Task.CurrencyId1, Task.Reward1);
                notifyService.send(log.UserId, string.Format("恭喜您完成任务 <strong>{0}<strong> 并且与 {1} 获得{2}的奖励",Task.Name,log.Created.ToString("yyyy-MM-dd HH:mm"),Task.GetRewardHtml()));
                DbContext.commitAll();
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
                c.ctx.errors.Add(ex.Message);
                DbContext.rollbackAll();
            }
            finally
            {
                DbContext.closeConnectionAll();
            }
        }

        /// <summary>
        /// 提供一个默认的实现，用户完成任务，通知用户去任务中心领奖
        /// </summary>
        /// <param name="c"></param>
        public virtual void ProcessTask(ControllerBase c)
        {
            GetReward(c);
        }

        public virtual string ShowTaskHtml(ControllerBase controller)
        {
            UserTaskLog log = taskService.FindLogByTime(controller.ctx.viewer.Id, Task.Id,Task.Day);
            string fileName = templateFile;
            if(log != null)
                fileName = "_complete.html";
            else
            {
                Result r = CanReceiveReward(controller.ctx.viewer.obj as IUser);
                if(!r.HasErrors)
                    fileName = "_reward.html";
            }

            IUser user = controller.ctx.viewer.obj as IUser; 
            Template template = new Template(PathHelper.Map(templateDir + fileName));
            template.Set("url", Task.Url.Replace("{url}",user.Url));
            template.Set("name", Task.Name);
            template.Set("shortDesc", Task.ShortDesc);
            template.Set("middleIcon", Task.MiddleIcon);
            template.Set("goal", Task.Goal);
            template.Set("reward", Task.GetRewardHtml());
            template.Set("rewardUrl", controller.to(new TaskController().GetReward, Task.Id));
            template.Set("processUrl", controller.to(new TaskController().Process, Task.Id));
            if(log != null)
            {
                template.Set("completeTime", log.Created.ToString("yyyy-MM-dd HH:mm"));
            }
            SetVar(template,controller);
            return template.ToString();
        }

        /// <summary>
        /// 给子类提供设置模板变量方法
        /// </summary>
        /// <param name="template"></param>
        protected virtual void SetVar(Template template,ControllerBase c)
        {

        }

        protected static string templateDir
        {
            get
            {
                return SystemInfo.RootPath + "framework/views/Task/Partial/";
            }
        }

        protected virtual string templateFile
        {
            get
            {
                return "_default.html";
            }
        }
        public TaskInfo Task
        {
            get;
            set;
        }
    }
}
