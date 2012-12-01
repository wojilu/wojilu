/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;
using wojilu.Members.Users.Domain;

namespace wojilu.Common.Picks {

    public abstract class ImgPickBase : ObjectBase<ImgPickBase> {

        public int AppId { get; set; }
        public User Creator { get; set; }

        [NotNull( "请填写标题" )]
        public String Title { get; set; }

        [NotNull( "请填写图片地址" )]
        public String ImgUrl { get; set; }

        [NotNull( "请填写网址" )]
        public String Url { get; set; }
        public DateTime Created { get; set; }

    }

}
