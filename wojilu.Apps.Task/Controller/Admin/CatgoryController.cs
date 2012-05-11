using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Task.Domain;
using wojilu.Apps.Task.Service;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.Controller.Task;

namespace wojilu.Web.Controller.Task.Admin
{
    public class CatgoryController : ControllerBase
    {
        TaskService taskService;

        public CatgoryController() {
            taskService = new TaskService();
        }

        public override void Layout()
        {
            HideLayout(typeof(TaskLayoutController));
        }

        public void Index() {
            List<TaskCatgory> catgorys = taskService.FindAllCatgorys();
            IBlock block = getBlock("list");
            catgorys.ForEach((c) => {
                block.Bind(c);
                block.Set("editLink", to(Edit, c.Id));
                block.Set("deleteLink", to(Delete, c.Id));
                block.Next();
            });
           set("addLink", to(Add));
        }

        public void Add() {
            target(Create);
        }

        [HttpPost]
        public void Create() {
            TaskCatgory catgory = ctx.PostValue<TaskCatgory>();
            catgory.insert();
            echoRedirectPart(lang("opok"), to(Index));
        }

        public void Edit(int id) {
            TaskCatgory catgory = taskService.FindCatgoryById(id);
            if (catgory == null) {
                echoRedirectPart("任务分类不存在", to(Index));
                return;
            }
            target(Update, id);
            bind(catgory);
        }

        [HttpPost]
        public void Update(int id) {
            TaskCatgory catgory = taskService.FindCatgoryById(id);
            if (catgory == null) {
                echoRedirect("任务分类不存在", to(Index));
                return;
            }
            catgory = ctx.PostValue(catgory) as TaskCatgory;
            if (catgory != null)
                catgory.update();
            echoRedirectPart(lang("opok"), to(Index));
        }

        [HttpDelete]
        public void Delete(int id) {
            TaskCatgory catgory = taskService.FindCatgoryById(id);
            if (catgory != null) {
                catgory.delete();
            }
            echoRedirectPart(lang("opok"), to(Index));
        }
    }
}
