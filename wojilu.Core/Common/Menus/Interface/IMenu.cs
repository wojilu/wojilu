/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Members.Users.Domain;
using wojilu.Common.AppBase;


namespace wojilu.Common.Menus.Interface {

    public interface IMenu : ISort, INode {

        new int Id { get; set; }

        String Style { get; set; }
        int OpenNewWindow { get; set; }

        int OwnerId { get; set; }
        String OwnerUrl { get; set; }
        String OwnerType { get; set; }

        User Creator { get; set; }

        /// <summary>
        /// 存储的原始网址，已经处理过，不包括后缀名和开始的斜杠，比如是 Forum1/Forum/Index
        /// </summary>
        String RawUrl { get; set; }

        /// <summary>
        /// 友好别名，比如 bbs，比如 default 等
        /// </summary>
        String Url { get; set; }

        DateTime Created { get; set; }



    }
}

