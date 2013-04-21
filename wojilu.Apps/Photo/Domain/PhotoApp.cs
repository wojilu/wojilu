/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.ORM;
using wojilu.Members.Users.Domain;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;
using wojilu.Common.Comments;

namespace wojilu.Apps.Photo.Domain {


    [Serializable]
    public class PhotoApp : ObjectBase<PhotoApp>, IApp, IAccessStatus, ICommentApp {

        public int OwnerId { get; set; }
        public String OwnerUrl { get; set; }

        [NotSave]
        public String OwnerType {
            get { return typeof( User ).FullName; }
            set { }
        }

        public int CommentCount { get; set; }
        public int PhotoCount { get; set; }
        public int Hits { get; set; }

        [TinyInt]
        public int AccessStatus { get; set; }
        public DateTime Created { get; set; }

    }
}

