using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Controller.Common;
using wojilu.Apps.Content.Domain;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Blog.Domain;

namespace wojilu.Web.Controller.Content.Admin {

    [App( typeof( ContentApp ) )]
    public class CommentController : CommentController<ContentPostComment> {
    }

}
