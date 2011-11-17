using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wojilu.ORM;

namespace wojilu.Test.Orm.Entities {

    //[Database( "db2" )]
    public class TOtherDB : ObjectBase<TOtherDB> {
        public string Name { get; set; }
        public int Age { get; set; }
    }


}
