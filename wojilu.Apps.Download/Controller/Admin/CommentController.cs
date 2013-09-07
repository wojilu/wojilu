using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Controller.Open.Admin;
using wojilu.Apps.Download.Domain;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Controller.Download.Admin {

    [App( typeof( DownloadApp ) )]
    public class CommentController : CommentBaseController<FileItem> {
    }

}
