/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.ORM;
using wojilu.Members.Users.Domain;

namespace wojilu.Common.Msg.Domain {


    [Serializable]
    public class Message : ObjectBase<Message> {

        public User Receiver { get; set; }

        public MessageData MessageData { get; set; }

        public DateTime Created { get; set; }
        public DateTime Readed { get; set; }

        public int IsRead { get; set; }
        public int IsReply { get; set; }
        public int IsDelete { get; set; }

        public int SiteMsgId { get; set; }


        [NotSave]
        public String SenderName {
            get { return MessageData.SenderName; }
        }

        [NotSave]
        public String Title {
            get { return MessageData.Title; }
        }



    }
}

