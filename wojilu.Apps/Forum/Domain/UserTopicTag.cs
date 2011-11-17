using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;
using wojilu.Common.AppBase;

namespace wojilu.Apps.Forum.Domain {

    public class UserTopicTag : ObjectBase<UserTopicTag>, ISort {

        public User User { get; set; }
        public String Name { get; set; }
        public DateTime Created { get; set; }

        public int DataCount { get; set; }

        public int OrderId { get; set; }

        public void updateOrderId() {
            this.update();
        }



    }

}
