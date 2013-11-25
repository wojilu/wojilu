using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;

namespace wojilu.Apps.Forum.Domain {

    public class ForumRateLog : ObjectBase<ForumRateLog> {

        public User User { get; set; }

        public long PostId { get; set; }

        public long CurrencyId { get; set; }
        public int Income { get; set; }

        public String Reason { get; set; }

        public DateTime Created { get; set; }

    }

}
