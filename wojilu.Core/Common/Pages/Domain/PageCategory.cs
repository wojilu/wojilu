/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;

using wojilu.Members.Users.Domain;
using System.Collections.Generic;
using wojilu.Common.AppBase;

namespace wojilu.Common.Pages.Domain {

    public class OpenStatus {

        /// <summary>
        /// 关闭编辑
        /// </summary>
        public static readonly int Close = 0;

        /// <summary>
        /// 完全开放，任何注册用户都可以编辑
        /// </summary>
        public static readonly int Open = 1;

        /// <summary>
        /// 只有被挑选的人(编辑)可以编辑
        /// </summary>
        public static readonly int Editor = 2;
    }

    [Serializable]
    public class PageCategory : ObjectBase<PageCategory>, ISort {

        public int OwnerId { get; set; }
        public String OwnerType { get; set; }
        public String OwnerUrl { get; set; }

        public User Creator { get; set; }

        public int OrderId { get; set; }
        public int ParentId { get; set; }

        public String Name { get; set; }
        public String Description { get; set; }
        public String Logo { get; set; }
        public DateTime Created { get; set; }
        public int DataCount { get; set; }

        public int IsShowWiki { get; set; } // 是否显示wiki词条统计信息
        public int OpenStatus { get; set; } // 开放状态，见OpenStatus
        public String EditorIds { get; set; } // 允许编辑的用户名单，当 OpenStatus==2 的时候起作用


        public void updateOrderId() {
            this.update();
        }

    }

}
