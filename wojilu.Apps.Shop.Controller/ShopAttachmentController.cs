using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Shop.Domain;
using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Service;

namespace wojilu.Web.Controller.Shop
{

    [App( typeof( ShopApp ) )]
    public class ShopAttachmentController : ControllerBase {
        public IShopItemService postService { get; set; }
        public IAttachmentService attachmentService { get; set; }

        public ShopAttachmentController() {
            postService = new ShopItemService();
            attachmentService = new AttachmentService();
        }


        public void Show( int id ) {

            String guid = ctx.Get( "id" );

            ShopItemAttachment attachment = attachmentService.GetById( id, guid );
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
