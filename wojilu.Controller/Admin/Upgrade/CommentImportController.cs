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

        public virtual void Index() {

            set( "lnkImportPage", to( BeginImportPage ) );
            set( "lnkImportBlog", to( BeginImportBlog ) );
            set( "lnkImportPhoto", to( BeginImportPhoto ) );

            set( "lnkImportDownload", to( BeginImportDownload ) );

        }

        public virtual void BeginImportPage() {

            //new ImportHelper<PageComment, Page>().Import( ctx.PostInt( "startId" ), ctx.PostInt( "endId" ) );
            echoAjaxOk();
        }

        public virtual void BeginImportPhoto() {
            //new ImportHelper<PhotoPostComment, PhotoPost>().Import( ctx.PostInt( "startId" ), ctx.PostInt( "endId" ) );
            echoAjaxOk();
        }

        public virtual void BeginImportBlog() {
            //new ImportHelper<BlogPostComment, BlogPost>().Import( ctx.PostInt( "startId" ), ctx.PostInt( "endId" ) );
            echoAjaxOk();
        }

        public virtual void BeginImportDownload() {

            //Type dCommentType = ObjectContext.GetType( "wojilu.Apps.Download.Domain.FileComment" );
            //Type dTargetType = ObjectContext.GetType( "wojilu.Apps.Download.Domain.FileItem" );

            //new ImportRawHelper().Import( dCommentType, dTargetType, ctx.PostInt( "startId" ), ctx.PostInt( "endId" ) );
            echoAjaxOk();
        }



    }

}
