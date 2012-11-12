using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;

namespace wojilu.Apps.Photo.Domain {

    public class PhotoLike : ObjectBase<PhotoLike> {

        public User User { get; set; }

        public PhotoPost Post { get; set; }

        public DateTime Created { get; set; }

    }

}
