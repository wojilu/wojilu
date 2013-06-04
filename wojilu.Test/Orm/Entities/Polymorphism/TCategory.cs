using System;
using wojilu.ORM;

namespace wojilu.Test.Orm.Entities {


    public class TCategory : ObjectBase<TCategory> {
        public string Name { get; set; }
	}
    public class TPostCategory : TCategory {
        public int Hits { get; set; }
    }
    public class TTopicCategory : TCategory {
        public int ReplyCount { get; set; }
    }



    public class TDataRoot : ObjectBase<TDataRoot>, IComparable {
        public string Title { get; set; }
        public string Body { get; set; }
        public TCategory Category { get; set; }
    }

    public class TPost : TDataRoot {
        public string Uid { get; set; }
        public int Hits { get; set; }
    }
    public class TTopic : TDataRoot {
        public string Uid { get; set; }
        public int Hits { get; set; }
        public int ReplyCount { get; set; }
    }


}
