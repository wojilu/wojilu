using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;
using wojilu.Common;
using wojilu.Web.Mvc;

namespace wojilu.Members.Users.Service {

    public class SysUserService {


        public virtual List<User> GetRanked( int count ) {
            if (count <= 0) count = 10;
            return db.find<User>( "order by Credit desc, Hits desc, Id desc" ).list( count );
        }

        public virtual List<IBinderValue> GetNewListWithAvatar( int count ) {
            if (count <= 0) count = 10;
            List<User> list = GetRecentUsersWithAvatar( count );

            return populateUser( list );

        }

        private static List<IBinderValue> populateUser( List<User> list ) {
            List<IBinderValue> results = new List<IBinderValue>();
            foreach (User user in list) {
                IBinderValue vo = new ItemValue();
                vo.Title = user.Name;
                vo.PicUrl = user.PicSmall;
                vo.Link = Link.ToUser( user.Url );
                results.Add( vo );
            }
            return results;
        }

        public List<User> GetRecentUsersWithAvatar( int count ) {
            if (count <= 0) count = 10;
            return User.find( "Pic is not null and Pic<>'" + SysPath.AvatarConstString + "'  order by Id desc" ).list( count );
        }


    }

}
