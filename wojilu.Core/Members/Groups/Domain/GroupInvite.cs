using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;
using wojilu.ORM;

namespace wojilu.Members.Groups.Domain {

    public class GroupInvite : ObjectBase<GroupInvite> {

        public int OwnerId { get; set; }

        public User Inviter { get; set; }

        public User Receiver { get; set; }

        [LongText, HtmlText]
        public String Msg { get; set; }

        public DateTime Created { get; set; }

        public int Status { get; set; }

        public String Code { get; set; }

        [NotSave]
        public String StatusStr {
            get {

                if (this.Status == 0) return "尚未接受";
                if (this.Status == 1) return "已接受邀请";
                return "";
            }
        }



    }

}
