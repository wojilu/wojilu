/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.ORM;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Forum.Interface;
using wojilu.Common.Tags;
using wojilu.Common;
using wojilu.Common.AppBase;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Apps.Forum.Domain {

    [Serializable]
    public class ForumTopic : ObjectBase<ForumTopic>, IPost, IAppData, IShareData, ISort, IHits {

        public User Creator { get; set; }
        public String CreatorUrl { get; set; }

        public int OwnerId { get; set; }
        public String OwnerType { get; set; }
        public String OwnerUrl { get; set; }

        public int AppId { get; set; }
        public ForumBoard ForumBoard { get; set; }
        public ForumCategory Category { get; set; }

        //主要用于主题显示的顺序
        public int OrderId { get; set; } 

        [NotNull( Lang = "exTitle" ), Column( Length = 50 )]
        public String Title { get; set; }

        public String TitleStyle { get; set; }

        public String TypeName { get; set; } // type full name

        // 附件数量
        public int Attachments { get; set; } 

        [TinyInt]
        public int IsTop { get; set; }

        // 不再可以跟帖
        [TinyInt]
        public int IsLocked { get; set; } 
        [TinyInt]
        public int IsPicked { get; set; }

        // 查看、下载附件是否需要登录
        [TinyInt]
        public int IsAttachmentLogin { get; set; }

        // 出售的价格
        public int Price { get; set; }

        // 悬赏、售价
        public int Reward { get; set; } 
        public int RewardAvailable { get; set; }

        // 阅读权限
        public int ReadPermission { get; set; }

        //总共获得的评分
        public int Rate { get; set; } 

        public DateTime Replied { get; set; }
        public int Replies { get; set; }
        public int LastReById { get; set; }

        // 原始存储的值
        public String RepliedUserFriendUrl { get; set; } 
        public String RepliedUserName { get; set; }

        public int AccessStatus { get; set; }

        // 置顶、删除、待审核 enum TopicStatus
        public int Status { get; set; } 

        public DateTime Created { get; set; }

        [Column( Length = 40 )]
        public String Ip { get; set; }

        public int Hits { get; set; }

        private String _content;

        [NotSave]
        public String Content {
            get {
                if (_content == null) {
                    ForumPost post = ForumPost.find( "TopicId=" + this.Id + " and ParentId=0" ).first();
                    if (post != null) _content = post.Content;
                }
                return _content;
            }
            set { _content = value; }
        }

        private TagTool _tag;

        [NotSave]
        public TagTool Tag {
            get {
                if (_tag == null) _tag = new TagTool( this );
                return _tag;
            }
        }

        [NotSave]
        public String TagRawString { get; set; }


        public IShareInfo GetShareInfo() {
            return new ForumTopicFeed( this );
        }

        [NotSave]
        public Boolean IsGlobalSticky { get; set; }


        public void updateOrderId() {
            db.update( this, "OrderId" );
        }

    }
}

