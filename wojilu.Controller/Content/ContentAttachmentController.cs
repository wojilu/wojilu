using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;

namespace wojilu.Web.Controller.Content {

    [App( typeof( ContentApp ) )]
    public class ContentAttachmentController : ControllerBase {
        public IContentPostService postService { get; set; }
        public IAttachmentService attachmentService { get; set; }

        public ContentAttachmentController() {
            postService = new ContentPostService();
            attachmentService = new AttachmentService();
        }


        public void Show( int id ) {

            String guid = ctx.Get( "id" );

            ContentAttachment attachment = attachmentService.GetById( id, guid );
            if (attachment == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            attachmentService.AddHits( attachment );

            // 检查盗链
            if (isDownloadValid() == false) {
                echoRedirect( alang( "exDownload" ) );
                return;
            }

            // 转发
            redirectUrl( attachment.FileUrl );

        }

        private Boolean isDownloadValid() {

            if (ctx.web.PathReferrer == null) return false;

            return ctx.web.PathReferrerHost.Equals( ctx.url.Host );
        }

    }

}
