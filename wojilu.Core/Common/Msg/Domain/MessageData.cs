/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.ORM;
using wojilu.Members.Users.Domain;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Interface;

namespace wojilu.Common.Msg.Domain {

    [Serializable]
    public class MessageData : ObjectBase<MessageData> {

        public MessageData() { }
        public MessageData( int id ) {
            this.Id = id;
        }

        public User Sender { get; set; }
        public String SenderName { get; set; }
        public String ToName { get; set; }

        public String Title { get; set; }
        [LongText]
        public String Body { get; set; }


        public DateTime Created { get; set; }
        public int IsDelete { get; set; }

        public IMember GetSender() {
            if ( this.Sender==null || this.Sender.Id < 0) {
                return Site.Instance;
            }
            return db.findById<User>( this.Sender.Id );
        }


        public int AttachmentCount { get; set; }

    }
}

