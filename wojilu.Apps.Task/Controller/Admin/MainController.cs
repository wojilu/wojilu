using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Task.Domain;
using wojilu.Apps.Task.Service;
using wojilu.Web.Mvc.Attr;
using wojilu.Common.Money.Interface;
using wojilu.Common.Money.Service;
using wojilu.Common.Money.Domain;
using wojilu.Web.Controller.Task;
using wojilu.Apps.Task.Core;

namespace wojilu.Web.Controller.Task.Admin
{
    public class TaskController : ControllerBase
    {
        TaskService taskService;
        ICurrencyService currencyService;
        public TaskController() {
            taskService = new TaskService();
            currencyService = new CurrencyService();
        }

        public override void Layout()
        {
            HideLayout(typeof(TaskLayoutController));
        }

        public void Index() {
            List<TaskCatgory> catgorys = taskService.FindAllCatgorys();
            IBlock block = getBlock("catgorys");
            catgorys.ForEach((c) => {
                block.Bind(c);
                IBlock taskBlock = block.GetBlock("tasks");
                List<TaskInfo> tasks = taskService.FindTasksByCatgory(c.Id);
                tasks.ForEach((task) => {
                    taskBlock.Bind(task);
                    taskBlock.Set("editLink", to(Edit, task.Id));
                    taskBlock.Set("deleteLink", to(Delete, task.Id));
                    taskBlock.Set("taskInfo.reward", task.GetRewardHtml());
                    taskBlock.Next();
                });
                block.Next();
            });
            set("addLink", to(Add));
            set("catgoryLink", to(new CatgoryController().Index));
        }

        public void Add() {
            target(Create);
            List<TaskCatgory> catgorys = taskService.FindAllCatgorys();
            List<Currency> currencys = currencyService.GetCurrencyAll();
            dropList("taskInfo.CatgoryId", catgorys, "Name=Id", "请选择");
            dropList("taskInfo.CurrencyId1", currencys, "Name=Id","请选择");
            dropList("taskInfo.CurrencyId2", currencys, "Name=Id", "请选择");
            dropList("taskInfo.Day", TaskInfo.GetDayType(), "forever");
            dropList("taskInfo.TypeName", TaskProcessFactory.Intance.GetProcessorsClassFullNameFromAssemby(), null);
        }

        [HttpPost]
        public void Create() {
            TaskInfo task = ctx.PostValue<TaskInfo>();
            task.insert();
            echoToParent(lang("opok"), to(Index));
        }

        public void Edit(int id) {
            TaskInfo task = taskService.FindTaskById(id);
            if (task == null) {
                echoToParent("任务不存在", to(Index));
                return;
            }
            List<TaskCatgory> catgorys = taskService.FindAllCatgorys();
            List<Currency> currencys = currencyService.GetCurrencyAll();
            dropList("taskInfo.CatgoryId", catgorys, "Name=Id", task.CatgoryId);
            dropList("taskInfo.CurrencyId1", currencys, "Name=Id",task.CurrencyId1);
            dropList("taskInfo.CurrencyId2", currencys, "Name=Id",task.CurrencyId2);
            dropList("taskInfo.Day", TaskInfo.GetDayType(),task.Day);
            dropList("taskInfo.TypeName", TaskProcessFactory.Intance.GetProcessorsClassFullNameFromAssemby(), task.TypeName);
            target(Update, id);
            bind(task);
        }

        [HttpPost]
        public void Update(int id) {
            TaskInfo task = taskService.FindTaskById(id);
            if (task == null) {
                echoToParent("任务不存在", to(Index));
                return;
            }
            task = ctx.PostValue(task) as TaskInfo;
            if (task != null)
                task.update();
            echoToParent(lang("opok"), to(Index));
        }

        [HttpDelete]
        public void Delete(int id) {
            TaskInfo task = taskService.FindTaskById(id);
            if (task != null) {
                task.delete();
            }
            echoToParent(lang("opok"), to(Index));
        }
    }
}
