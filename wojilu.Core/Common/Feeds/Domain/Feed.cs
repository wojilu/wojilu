/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.ORM;
using wojilu.Members.Users.Domain;
using wojilu.Common.Feeds.Interface;
using wojilu.Common.Comments;
using wojilu.DI;

namespace wojilu.Common.Feeds.Domain {

    [Serializable]
    public class Feed : ObjectBase<Feed>, IFeed {

        public User Creator { get; set; }
        public String DataType { get; set; }
        public int DataId { get; set; }

        public int Replies { get; set; }

        public String TitleTemplate { get; set; }
        [LongText]
        public String TitleData { get; set; }

        public String BodyTemplate { get; set; }
        [LongText]
        public String BodyData { get; set; }

        [LongText]
        public String BodyGeneral { get; set; }

        public DateTime Created { get; set; }


        //private List<User> creatorList = new List<User>();

        //[NotSave]
        //public List<User> CreatorList {
        //    get {
        //        if (creatorList.Count == 0) creatorList.Add( this.Creator );
        //        return creatorList;
        //    }
        //    set { creatorList = value; }
        //}


    }

}
