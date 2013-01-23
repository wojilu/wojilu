using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Common.Pages.Domain;
using wojilu.Common.Comments;
using wojilu.DI;
using wojilu.Apps.Download.Domain;

namespace wojilu.Web.Controller.Admin.Upgrade {

    public class FileCommentImportController : ControllerBase {

        public void Index() {

            set( "lnkImportFile", to( BeginImportFile ) );

        }

        public void BeginImportFile() {

            new ImportHelper<FileComment, FileItem>().Import();

            echoAjaxOk();
        }




    }

}
