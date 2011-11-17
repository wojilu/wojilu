/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;

using wojilu.ORM;
using wojilu.Common.Security;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Forum.Domain.Security;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;

namespace wojilu.Apps.Forum.Domain {

    [Serializable]
    public class ForumBoard : ObjectBase<ForumBoard>, IAppData, ISecurity, INode, ISort {

        public ForumBoard() {
            this.Security = ForumPermission.GetDefaultPermission();
        }

        public ForumBoard( int id ) {
            Id = id;
            this.Security = ForumPermission.GetDefaultPermission();
        }

        public int pkid { get; set; }

        public int OwnerId { get; set; }
        public String OwnerType { get; set; }
        public String OwnerUrl { get; set; }

        public User Creator { get; set; }
        public String CreatorUrl { get; set; }

        public int AppId { get; set; }
        public int ParentId { get; set; }
        public int OrderId { get; set; }

        [NotNull( Lang = "exName" )]
        public String Name { get; set; }

        [LongText]
        public String Notice { get; set; }
        public String Description { get; set; }

        public String Logo { get; set; }
        public String Moderator { get; set; }


        public int Topics { get; set; }
        public int Posts { get; set; }
        public int TodayPosts { get; set; }
        public int TodayTopics { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public int AccessStatus { get; set; }

        // 是否论坛分类？如果是分类，则指允许出现子版块，不允许在这个版块下面发帖
        public int IsCategory { get; set; } 

        [LongText]
        public String Security { get; set; }

        [Column( Length = 40 )]
        public String Ip { get; set; }

        public String UpdateInfo { get; set; }

        public int ViewId { get; set; }


        [NotSave]
        public LastUpdateInfo LastUpdateInfo {
            get { return new LastUpdateInfo( this.UpdateInfo ); }
            set { this.UpdateInfo = value.ToSavedString(); }
        }

        [NotSave]
        public String Title {
            get { return this.Name; }
            set { this.Name = value; }
        }

        public void updateOrderId() {
            db.update( this, "OrderId" );
        }

        public new Result update() {
            return db.update( this );
        }

    }



}

