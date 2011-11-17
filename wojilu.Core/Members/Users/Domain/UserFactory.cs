using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Interface;
using wojilu.Members.Sites.Domain;

namespace wojilu.Members.Users.Domain {

    [Serializable]
    public class UserFactory : IUserFactory {

        public IUser New() {
            return new User();
        }

        public IUser New( int id ) {
            return new User( id );
        }

        public IUser GuestUser() {
            User user = new User( SiteRole.GuestId );
            user.Pic = SysPath.AvatarConstString;
            user.RoleId = SiteRole.GuestId;
            user.Url = "#";
            user.Name = user.Role.Name;
            return user;
        }

        public IUser NullUser() {
            User user = new User( SiteRole.DeletedUserId );
            user.Pic = SysPath.AvatarConstString;
            user.RoleId = SiteRole.DeletedUserId;
            user.Url = "#";
            user.Name = user.Role.Name;
            return user;
        }

        public static User Guest {
            get {
                return new UserFactory().GuestUser() as User;
            }
        }

    }

}
