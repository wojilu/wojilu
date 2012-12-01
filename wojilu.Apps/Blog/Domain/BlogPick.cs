using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;
using wojilu.Web.Mvc;
using wojilu.Apps.Blog.Interface;
using wojilu.Common.Picks;

namespace wojilu.Apps.Blog.Domain {

    public class BlogPick : DataPickBase {

        public override Type GetImgType() {
            return typeof( BlogPickedImg );
        }
    }

}
