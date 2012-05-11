using System;
using System.Collections.Generic;
using System.Text;
using wojilu.DI;
using wojilu.Apps.Task.Domain;
using wojilu.Apps.Task.Service;

namespace wojilu.Apps.Task.Core
{
    public class TaskProcessFactory
    {
        public static TaskProcessFactory Intance = new TaskProcessFactory();

        private TaskProcessFactory()
        {
        }

        public ITaskProcessor GetProcessor(int taskId)
        {
            TaskService service = new TaskService();
            TaskInfo task = service.FindTaskById(taskId);
            if(task == null)
                return null;
            else
                return GetProcessor(task);
        }

        public ITaskProcessor GetProcessor(TaskInfo task)
        {
            ITaskProcessor p = ObjectContext.CreateObject(task.TypeName) as ITaskProcessor;
            p.Task = task;
            return p;
        }
    }
}
