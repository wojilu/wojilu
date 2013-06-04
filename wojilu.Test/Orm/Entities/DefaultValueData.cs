using System;
using System.Collections;

using wojilu;
using wojilu.ORM;

namespace wojilu.Test.Orm.Entities {

    public class DefaultValueData : ObjectBase<DefaultValueData> {


        [Default( "匿名用户" )]
        public string Name { get; set; }

        public DateTime CreateTime { get; set; }
        public string Birthday { get; set; }

    }
}
