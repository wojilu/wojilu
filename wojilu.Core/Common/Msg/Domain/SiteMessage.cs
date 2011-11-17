/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;
using wojilu.Members.Users.Domain;
using wojilu.Members.Sites.Domain;

namespace wojilu.Common.Msg.Domain {

    [Serializable]
    public class MessageSite : ObjectBase<MessageSite> {

        public MessageSite() { }
        public MessageSite( int id ) {
            this.Id = id;
        }

        public User Creator { get; set; }

        public int ReceiverRoleId { get; set; }

        [NotNull( Lang = "exTitle" )]
        public String Title { get; set; }

        [LongText, NotNull( Lang = "exContent" )]
        public String Body { get; set; }


        public DateTime Created { get; set; }

        [NotSave]
        public String Content {
            get { return strUtil.ParseHtml( this.Body, 50 ); }
        }

        [NotSave]
        public String RoleName {
            get {
                if (this.ReceiverRoleId == -1) return lang.get( "everyone" );
                SiteRole role = SiteRole.GetById( this.ReceiverRoleId );
                return role == null ? "" : role.Name;
            }
        }


    }

}
