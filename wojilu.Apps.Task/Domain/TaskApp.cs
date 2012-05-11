using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;

namespace wojilu.Apps.Task.Domain {

    public class TaskApp : ObjectBase<TaskApp>, IApp, IAccessStatus
    {
        public int OwnerId { get; set; }
        public String OwnerUrl { get; set; }
        public String OwnerType { get; set; }

        public DateTime Created { get; set; }

        public int AccessStatus {
            get;
            set;
        }
    }

}
