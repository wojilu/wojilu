using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Data;
using wojilu.Common.AppBase;

namespace wojilu.Apps.Task.Domain
{
    public class TaskCatgory : CacheObject,ISort,IComparable
    {
        public void updateOrderId() {
            this.update();
        }

        public int CompareTo(object obj) {
            TaskCatgory t = obj as TaskCatgory;
            if (this.OrderId > t.OrderId) return -1;
            if (this.OrderId < t.OrderId) return 1;
            if (this.Id > t.Id) return 1;
            if (this.Id < t.Id) return -1;
            return 0;
        }

        public int OrderId {
            get;
            set;
        }
    }
}
