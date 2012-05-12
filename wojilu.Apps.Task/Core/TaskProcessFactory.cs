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

        private Dictionary<string,string> _allProcessorsClassFullName = null;

        private static readonly object _locker = new object();

        /// <summary>
        /// 得到IOC中注册的Assembly的任务处理类的全名,已经缓存可放心调用，Key为类说明，Value为类全名
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetProcessorsClassFullNameFromAssemby()
        {
            if(_allProcessorsClassFullName == null)
            {
                lock(_locker)
                {
                    if(_allProcessorsClassFullName == null)
                    {
                        _allProcessorsClassFullName = new Dictionary<string, string>();
                        foreach(KeyValuePair<string, Type[]> kvp in ObjectContext.Instance.AssemblyTypes)
                        {
                            foreach(Type type in kvp.Value)
                            {
                                if(!type.IsAbstract && type.GetInterface("wojilu.Apps.Task.Core.ITaskProcessor", true) != null)
                                {
                                    object[] attributes = type.GetCustomAttributes(typeof(TaskNameAttribute),false);
                                    if(attributes != null && attributes.Length > 0)
                                    {
                                        TaskNameAttribute taskNameAttribute = attributes[0] as TaskNameAttribute;
                                        _allProcessorsClassFullName.Add(taskNameAttribute.Name,type.FullName);
                                    }
                                    
                                }
                            }
                        }
                    }
                }
            }
            return _allProcessorsClassFullName;
        }
    }
}
