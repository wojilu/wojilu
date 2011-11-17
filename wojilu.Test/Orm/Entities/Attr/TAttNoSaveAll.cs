using System;
using System.Collections;
using wojilu.ORM;

namespace wojilu.Test.Orm.Attr {

    public class TAttNoSaveAll : ObjectBase<TAttNoSaveAll> {


        [NotSave]
        public string Name { get; set; }
        public string Title { get; set; }
        [NotSave]
        public int ReadCount { get; set; }

    }


    public class TAttWithSaveAll : ObjectBase<TAttWithSaveAll> {

        public string Name { get; set; }
        public string Title { get; set; }
        public int ReadCount { get; set; }

    }

}
