using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc.Attr;
using wojilu.Web.Controller.Open.Admin;
using wojilu.Apps.Content.Domain;

namespace wojilu.Web.Controller.Content.Admin {

    [App( typeof( ContentApp ) )]
    public class CommentController : CommentBaseController<ContentPost> {



    }

}
