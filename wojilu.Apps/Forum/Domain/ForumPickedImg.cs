using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;
using wojilu.ORM;

namespace wojilu.Apps.Forum.Domain {

    [Serializable]
    public class ForumPickedImg : ObjectBase<ForumPickedImg> {

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
