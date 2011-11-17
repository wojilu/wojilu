using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;

namespace wojilu.cms.Domain {

    [Table( "Users" )]
    public class User : ObjectBase<User> {

        [NotNull( "必须填写用户名" )]
        public string Name { get; set; }

        [NotNull( "必须填写密码" )]
        public string Pwd { get; set; }
    }

}
