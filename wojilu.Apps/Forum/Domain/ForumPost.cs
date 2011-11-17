/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.ORM;
using wojilu.Common.Feeds.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Forum.Interface;
using wojilu.Common.Tags;
using wojilu.Common;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Apps.Forum.Domain {

    [Serializable]
    public class ForumPost : ObjectBase<ForumPost>, IPost, IAppData, IShareData, IHits {

        public int AppId { get; set; }
        public int ForumBoardId { get; set; }

        public int TopicId { get; set; }
        public int ParentId { get; set; }

        public User Creator { get; set; }
        public String CreatorUrl { get; set; }

        public int OwnerId { get; set; }
        public String OwnerType { get; set; }
        public String OwnerUrl { get; set; }

        public int AccessStatus { get; set; }

        [NotNull( Lang = "exTitle" ), Column( Length = 50 )]
        public String Title { get; set; }

        [LongText]
        public String Content { get; set; }

        public int Attachments { get; set; }


        public int IsEdited { get; set; }
        public int EditCount { get; set; }
        public int EditMemberId { get; set; }
        public String EditReason { get; set; }
        public DateTime EditTime { get; set; }

        public int Rate { get; set; }
        public int Reward { get; set; }

        public int Status { get; set; }
        public int Hits { get; set; }

        [Column( Length = 40 )]
        public String Ip { get; set; }

        public DateTime Created { get; set; }

        [NotSave]
        public ForumBoard Board { get; set; }

        [NotSave]
        public ForumTopic Topic { get; set; }

        private TagTool _tag;

        [NotSave]
        public TagTool Tag {
            get {
                if (_tag == null) _tag = new TagTool( this );
                return _tag;
            }
        }

        public IShareInfo GetShareInfo() {
            return new ForumPostFeed( this );
        }

    }
}

