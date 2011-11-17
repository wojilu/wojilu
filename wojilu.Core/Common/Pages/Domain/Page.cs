/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.ORM;
using wojilu.Members.Users.Domain;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.Feeds.Interface;
using wojilu.Common.AppBase;
using wojilu.Common.Tags;

namespace wojilu.Common.Pages.Domain {

    [Serializable]
    public class Page : ObjectBase<Page>, IAppData, INode, IShareData, ISort {

        public int OwnerId { get; set; }
        public String OwnerType { get; set; }
        public String OwnerUrl { get; set; }

        public User Creator { get; set; }
        public PageCategory Category { get; set; }

        public int OrderId { get; set; }

        [NotNull( Lang = "exTitle" )]
        public String Title { get; set; }
        public String Logo { get; set; }

        [LongText]
        [NotNull( Lang = "exContent" )]
        public String Content { get; set; }

        public DateTime Created { get; set; }
        public int Hits { get; set; }
        public int Replies { get; set; }
        public int EditCount { get; set; } // 编辑次数

        public int IsAllowReply { get; set; }

        public DateTime Updated { get; set; } // 最后更新时间
        public String EditReason { get; set; } // 修改原因
        public User EditUser { get; set; } // 编辑人

        public int IsShowHistory { get; set; } // 是否公开版本历史

        public int UpdatingId { get; set; } // 正在编辑的用户
        public DateTime UpdatingTime { get; set; } // 正在编辑的最后时间


        [NotSave]
        public String CategoryStr {
            get { return Category != null && Category.Id > 0 ? "("+Category.Name+")" : ""; }
        }

        [NotSave]
        public String IsAllowReplyStr {
            get { return this.IsAllowReply == 1 ? "√" : "×"; }
        }


        // IAppData接口(用于comment)

        [NotSave]
        public int AppId { get; set; }
        [NotSave]
        public int AccessStatus { get; set; }
        [NotSave]
        public String Ip { get; set; }
        [NotSave]
        public String CreatorUrl { get; set; }

        // tree 接口
        public int ParentId { get; set; }

        [NotSave]
        public String Name {
            get { return this.Title; }
            set { }
        }
        
        public IShareInfo GetShareInfo() {
            return new PageFeed( this );
        }


        public void updateOrderId() {
            this.update();
        }

        private TagTool _tag;

        [NotSave]
        public TagTool Tag {
            get {
                if (_tag == null) {
                    _tag = new TagTool( this );
                }
                return _tag;
            }
        }


    }

}
