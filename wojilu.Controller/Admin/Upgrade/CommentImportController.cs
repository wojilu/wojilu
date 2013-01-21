using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Common.Pages.Domain;
using wojilu.Common.Comments;
using wojilu.DI;
using wojilu.Apps.Photo.Domain;
using wojilu.Apps.Blog.Domain;

namespace wojilu.Web.Controller.Admin.Upgrade {

    public class CommentImportController : ControllerBase {

        public void Index() {

            set( "lnkImportPage", to( BeginImportPage ) );
            set( "lnkImportBlog", to( BeginImportBlog ) );
            set( "lnkImportPhoto", to( BeginImportPhoto ) );

        }

        public void BeginImportPage() {

            new ImportHelper<PageComment, Page>().Import();

            echoAjaxOk();
        }

        public void BeginImportPhoto() {
            new ImportHelper<PhotoPostComment, PhotoPost>().Import();
            echoAjaxOk();
        }

        public void BeginImportBlog() {
            new ImportHelper<BlogPostComment, BlogPost>().Import();
            echoAjaxOk();
        }



    }

}
