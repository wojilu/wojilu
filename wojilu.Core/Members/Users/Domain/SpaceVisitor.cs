/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

namespace wojilu.Members.Users.Domain {

    /// <summary>
    /// 保存了所有用户的访问记录
    /// TODO 自动删除三个月前的访问
    /// </summary>
    [Serializable]
    public class SpaceVisitor : ObjectBase<SpaceVisitor> {

        public User Visitor { get; set; }
        public User Target { get; set; }

        public DateTime Created { get; set; }

    }

}
