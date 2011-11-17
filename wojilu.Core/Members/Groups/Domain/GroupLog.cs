/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;
using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;

namespace wojilu.Members.Groups.Domain {

    [Serializable]
    public class GroupLog : ObjectBase<GroupLog>, IAdminLog {

        public User User { get; set; }
        public DateTime Created { get; set; }

        public int CategoryId { get; set; }
        public String Message { get; set; }

        // 对象的序列化字符串
        public String DataInfo { get; set; }

        // TypeFullName
        public String DataType { get; set; } 

        [Column( Length = 40 )]
        public String Ip { get; set; }

    }

}
