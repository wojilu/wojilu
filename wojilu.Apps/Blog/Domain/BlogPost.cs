/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.ORM;
using wojilu.Common.Feeds.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Common.Tags;
using wojilu.Common.Jobs;
using wojilu.Common.AppBase.Interface;
using wojilu.Common;
using wojilu.Common.Comments;

namespace wojilu.Apps.Blog.Domain {

    [Serializable]
    public class BlogPost : ObjectBase<BlogPost>, IAppData, IShareData, IHits, ICommentTarget {

        public IShareInfo GetShareInfo() {
            return new BlogPostFeed( this );
        }

        public int OwnerId { get; set; }
        public String OwnerType { get; set; }
        [Column( Length = 50 )]
        public String OwnerUrl { get; set; }

        public int AppId { get; set; }
        public int SysCategoryId { get; set; }

        public User Creator { get; set; }
        [Column( Length = 50 )]
        public String CreatorUrl { get; set; }

        public BlogCategory Category { get; set; }

        [Column( Length = 250 )]
        [NotNull( Lang = "exTitle" )]
        public String Title { get; set; }

        [LongText, HtmlText]
        [NotNull( Lang = "exContent" )]
        public String Content { get; set; }

        [LongText]
        public String Abstract { get; set; }
        public String Pic { get; set; }

        [TinyInt]
        public int CommentCondition { get; set; }
        public int Hits { get; set; }

        [TinyInt]
        public int IsPic { get; set; }
        [TinyInt]
        public int IsPick { get; set; }
        [TinyInt]
        public int IsTop { get; set; }

        public int Replies { get; set; }

        [TinyInt]
        public int SaveStatus { get; set; }
        public int AccessStatus { get; set; }

        [Column( Length = 40 )]
        public String Ip { get; set; }

        public DateTime Created { get; set; }

        public int AttachmentCount { get; set; }

        private TagTool tag;
        [NotSave]
        public TagTool Tag {
            get {
                if (tag == null) tag = new TagTool( this );
                return tag;
            }
        }

        [NotSave]
        public String Tags { get; set; }

        public void UpdateComments() {

            BlogApp app = BlogApp.findById( this.AppId );
            if (app == null) return;
            app.CommentCount = OpenComment.count( "AppId=" + this.AppId + " and TargetDataType='" + this.GetType().FullName + "'" );
            app.update();

        }

        public Type GetAppType() {
            return typeof( BlogApp );
        }
    }
}

