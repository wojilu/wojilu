/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Members.Users.Domain;
using wojilu.Common.Feeds.Interface;

namespace wojilu.Web.Controller.Common.Feeds {

    [Serializable]
    public class FeedView : IFeed {

        public int Id { get; set; }

        public User Creator { get; set; }
        public String DataType { get; set; }
        public int DataId { get; set; }

        public int Replies { get; set; }

        public String TitleTemplate { get; set; }
        public String TitleData { get; set; }

        public String BodyTemplate { get; set; }
        public String BodyData { get; set; }
        public String BodyGeneral { get; set; }

        public DateTime Created { get; set; }

        private Boolean isMerged = false;

        public Boolean IsMerged {
            get { return isMerged; }
            set { isMerged = value; }
        }

        private List<User> creatorList = new List<User>();

        public List<User> CreatorList {
            get {
                if (creatorList.Count == 0) creatorList.Add( this.Creator );
                return creatorList;
            }
            set { creatorList = value; }
        }

        public Boolean CanComment {
            get {

                if (this.DataId <= 0) return false;

                return true;
            }
        }


        public string Ip { get; set; }
    }

}
