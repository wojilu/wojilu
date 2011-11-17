using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Categories;

namespace wojilu.Members.Users.Domain {

    [Serializable]
    public class FriendCategory : CategoryBase {


        public static List<FriendCategory> GetByOwner( int ownerId ) {
            return db.find<FriendCategory>( "OwnerId=" + ownerId + " order by orderId desc, Id asc" ).list();
        }

    }

}
