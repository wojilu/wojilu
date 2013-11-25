/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.ORM;
using wojilu.Members.Users.Domain;

namespace wojilu.Apps.Forum.Domain {

    [Serializable]
    public class ForumLog : ObjectBase<ForumLog> {

        public long UserId { get; set; }

        public String UserName { get; set; }

        public String UserUrl { get; set; }

        public long ActionId { get; set; }

        public long AppId { get; set; }

        public String Msg { get; set; }

        public long PostId { get; set; }

        public long TopicId { get; set; }

        public DateTime Created { get; set; }

        public int Expiration { get; set; }

        [Column( Length = 40 )]
        public String Ip { get; set; }

        [NotSave]
        public String MsgInfo {
            get {

                String msg = "N/A";

                if (this.ActionId > 0) {

                    if (this.TopicId > 0) {
                        msg = "forum topic " + " " + lang.get( "op" ) + ":" + ForumLogAction.GetLable( this.ActionId ) + " " + this.TopicId ;
                        if( strUtil.HasText( this.Msg ) )
                            msg += "(" + this.Msg + ")";
                    }
                    else if (this.PostId > 0) {
                        msg = "forum post " + " " + lang.get( "op" ) + ":" + ForumLogAction.GetLable( this.ActionId ) + " " + this.PostId;
                        if (strUtil.HasText( this.Msg ))
                            msg += "(" + this.Msg + ")";
                    }
                }
                else if (strUtil.HasText( this.Msg ))
                    msg = this.Msg;
                
                return msg;

            }
        }


    }
}

