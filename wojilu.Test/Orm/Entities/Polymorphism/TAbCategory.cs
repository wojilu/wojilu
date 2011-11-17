using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wojilu.Test.Orm.Entities {

    public abstract class TAbCategory : ObjectBase<TAbCategory> {
        public string Name { get; set; }
    }

    public class TAbNewCategory : TAbCategory {


        public string Title { get; set; }
        public int Hits { get; set; }
    }

    public class TAbNewCategory2 : TAbCategory {
        public string Title2 { get; set; }
    }

}
