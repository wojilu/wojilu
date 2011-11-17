using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Download.Domain;
using wojilu.Web.Controller.Common;

namespace wojilu.Web.Controller.Download {

    public class FileCommentController : CommentController<FileComment> {

        protected override string getTargetLink( FileComment post ) {

            return to( new FileController().Show, post.RootId );
        }

    }

}
