using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Web.Mvc;
using wojilu.Members.Users.Service;

namespace wojilu.Common.Microblogs.Parser {


    public class MicroblogBinder : IMicroblogBinder {

        private List<User> _users = new List<User>();
        private List<String> _names = new List<String>();

        public IUserService userService { get; set; }

        public MicroblogBinder() {
            userService = new UserService();
        }

        public virtual String GetLink( String userName ) {

            if (strUtil.IsNullOrEmpty( userName )) return "@";

            User u = userService.GetByName( userName.Trim() );
            if (u == null) return "@" + userName;

            if (_names.Contains( u.Name )==false) {
                _users.Add( u );
                _names.Add( u.Name );
            }

            // TODO 此处可配置，是指向用户空间，还是用户微博
            return string.Format( "<a href=\"{1}\">@{0}</a>", userName, Link.ToMember( u ) );
        }

        public virtual String GetTagLink( String tag ) {

            String lnk = alink.ToTag( tag.Trim() );
            return string.Format( "<a href=\"{1}\" target=\"_blank\">#{0}#</a>", tag.Trim(), lnk );
        }

        public virtual String GetUrlLink( String url ) {
            return string.Format( "<a href=\"{0}\" target=\"_blank\">{0}</a>", url.Trim() );
        }

        public virtual List<User> GetValidUsers() {
            return _users;
        }

    }

}
