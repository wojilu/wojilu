using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Task.Core;
using wojilu.Apps.Task.Service;
using wojilu.Apps.Task.Domain;
using wojilu.Web;
using wojilu.Common.Money.Domain;
using wojilu.Common.Money.Interface;
using wojilu.Common.Money.Service;
using wojilu.Web.Controller.Layouts;
using wojilu.Web.Controller.Task;

namespace wojilu.Web.Controller
{
   [App(typeof(TaskController))]
    public class TaskController : ControllerBase
    {
       private static ILog log = LogManager.GetLogger(typeof(TaskController));

       private TaskService taskService;
        private ICurrencyService currencyService;

        public TaskController() {
            currencyService = new CurrencyService();
            taskService = new TaskService();
        }

        public override void CheckPermission() {
            if (!ctx.web.UserIsLogin)
                redirectLogin();
        }

        public override void Layout()
        {
            HideLayout(typeof(wojilu.Web.Controller.LayoutController));
            HideLayout(typeof(TaskLayoutController));
            run(new TaskLayoutController().AdminLayout);
        }

        public void Index() {
            IBlock block = getBlock("catgorys");
            List<TaskCatgory> catgorys = taskService.FindAllCatgorys();
            string url=string.Empty;
            catgorys.ForEach((c) => {
                block.Bind(c);
                List<TaskInfo> tasks = taskService.FindUnCompletedTasks(c.Id,ctx.viewer.Id);
                ITaskProcessor processor;
                IBlock taskBlock = block.GetBlock("tasks");
                tasks.ForEach((task) => {
                    url = t2(Show, task.Id);
                    string action = string.Empty;
                    taskBlock.Bind(task);
                    taskBlock.Set("taskInfo.reward", task.GetRewardHtml());
                    processor = TaskProcessFactory.Intance.GetProcessor(task);
                    if (processor != null) {
                        Result result = processor.CanReceiveReward(ctx.viewer.obj);
                        if (result.IsValid) {
                            action = string.Format("<a href='{0}' title='任务介绍' xwidth=500 xheight=230 class='frmBox finish'>我要领奖</a>",url);
                        }
                        else {
                            action = string.Format("<a href='{0}' title='任务介绍' class='frmBox todo'>我要参加</a>", url);
                        }
                    }
                    taskBlock.Set("taskInfo.action", action);
                    taskBlock.Next();
                });
                block.Next();
            });
            set("finishUrl", t2(Finish));
        }

        public void Finish() {
            List<TaskInfo> tasks = taskService.FindCompletedTasks(ctx.viewer.Id);
            IBlock taskBlock = getBlock("tasks");
            tasks.ForEach((task) => {
                taskBlock.Bind(task);
                taskBlock.Set("taskInfo.reward", task.GetRewardHtml());
                taskBlock.Next();
            });
            set("taskUrl", t2(Index));
        }

        [HttpPost]
        public void Process(int id) {
            ITaskProcessor processor = TaskProcessFactory.Intance.GetProcessor(id);
            if (processor == null) {
                echoError(lang("exDataNotFound"));
                return;
            } 
            processor.ProcessTask(this);
            if (errors.HasErrors) {
                echoJson(errors.ErrorsJson);
                return;
            }
            else {
                echoJson("{\"IsValid\":true}");
            }
        }

        public void Show(int id) {
            ITaskProcessor processor = TaskProcessFactory.Intance.GetProcessor(id);
            if (processor == null) {
                echoError(lang("exDataNotFound"));
                return;
            }
            string html = processor.ShowTaskHtml(this);
            viewContent(html);
        }

        [HttpPost]
        public void GetReward(int id) {
            ITaskProcessor processor = TaskProcessFactory.Intance.GetProcessor(id);
            if (processor == null) {
                echoError(lang("exDataNotFound"));
                return;
            }
            processor.GetReward(this);
            if (errors.HasErrors) {
                echoError();
                return;
            }
            else {
                echoJson("{\"IsValid\":true}");
            }
        }
    }
}
