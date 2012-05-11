using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;
using wojilu.ORM;

namespace wojilu.Apps.Task.Domain {
    public class UserTaskLog : ObjectBase<UserTaskLog> {
        private int _state;

        public int UserId { get; set; }

        public int  TaskId { get; set; }

        public DateTime Created {
            get;
            set;
        }

        /// <summary>
        /// 任务状态 0表示可以领奖了 1表示已经领取奖励
        /// </summary>
        public int State {
            get { return _state; }
            set { _state = value; }
        }

        /// <summary>
        /// 任务是否已经领取过奖励了
        /// </summary>
        [NotSave]
        public bool HasComplete {
            get {
                return Id > 0 && State == 1;
            }
        }
    }
}
