using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Apps.Task.Core
{
    /// <summary>
    /// 任务名Attribute
    /// </summary>
    [AttributeUsage( AttributeTargets.Class)]
    public class TaskNameAttribute : Attribute
    {
        public string Name { get; private set; }
        public TaskNameAttribute(string name)
        {
            Name = name;
        }

    }
}
