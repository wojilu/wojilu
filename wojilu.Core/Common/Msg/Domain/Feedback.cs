/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Members.Users.Domain;
using wojilu.ORM;

namespace wojilu.Common.Msg.Domain {


    [Serializable]
    public class Feedback : ObjectBase<Feedback> {

        public static readonly int ContentLength = 2000;

        [Column( Name = "AuthorId" )]
        public User Creator { get; set; }
        public User Target { get; set; }

        public int ParentId { get; set; }

        [Column( Length = 50 )]
        public String AuthorName { get; set; }

        [LongText]
        public String Content { get; set; }

        public DateTime Created { get; set; }

        public String Email { get; set; }

        [Column( Length = 40 )]
        public String Ip { get; set; }

        public int IsPrivate { get; set; }

        public String GetContent() {
            if (this.ParentId == 0) return this.Content;
            Feedback parent = db.findById<Feedback>( this.ParentId );
            if (parent == null) return this.Content;
            String quote = "<div class='quote'><span class='qSpan'>{0} : {1}</span></div>";
            return string.Format( quote, parent.Creator.Name, strUtil.CutString( parent.Content, 50 ) ) + "<div>" + this.Content + "</div>";
        }

    }
}

