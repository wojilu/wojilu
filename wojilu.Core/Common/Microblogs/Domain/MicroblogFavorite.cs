using System;
using System.Collections.Generic;
using System.Text;


namespace wojilu.Common.Microblogs.Domain {

    public class MicroblogFavorite : ObjectBase<MicroblogFavorite> {

        public int UserId { get; set; }
        public Microblog Microblog { get; set; }

        public DateTime Created { get; set; }

    }

}
