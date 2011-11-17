using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wojilu.Test.Orm.Entities {

    public class TNews : ObjectBase<TNews> {

        public String Title { get; set; }
        public TAuthor Author { get; set; }
    }

    public class TAuthor : ObjectBase<TAuthor> {
        public String Name { get; set; }
        public TAuthorCategory Category { get; set; }
    }

    public class TAuthorCategory : ObjectBase<TAuthorCategory> {
        public String Name { get; set; }
    }



}
