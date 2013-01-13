using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Common.Microblogs.Service;
using wojilu.Common.Microblogs.Domain;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Controller.Admin.Mb {

    public class TrashController : ControllerBase {

        public SysMicroblogService sysMicroblogService { get; set; }

        public TrashController() {
            sysMicroblogService = new SysMicroblogService();
        }

        public void Index() {

            set( "OperationUrl", to( Admin ) );

            DataPage<Microblog> list = sysMicroblogService.GetSysTrashPage( 30 );

            list.Results.ForEach( x => {
                x.data["CreatorLink"] = alink.ToUserMicroblog( x.User );
                x.data.show = alink.ToAppData( x );
                x.data["PicIcon"] = x.IsPic ? string.Format( "<a href=\"{0}\" target=\"_blank\" title=\"点击查看原始图片\"><img src=\"{1}img.gif\" /></a>", x.PicOriginal, sys.Path.Img ) : "";
            } );

            bindList( "list", "x", list.Results );

            set( "page", list.PageBar );
        }

        [HttpPost, DbTransaction]
        public void Admin() {

            String ids = ctx.PostIdList( "choice" );
            String cmd = ctx.Post( "action" );

            if ("deleteTrue".Equals( cmd )) {
                sysMicroblogService.DeleteTrueBatch( ids );
                echoAjaxOk();
            }
            else if ("restore".Equals( cmd )) {
                sysMicroblogService.RestoreSysBatch( ids );
                echoAjaxOk();
            }
            else {
                echoError( "errorCmd" );
            }
        }


    }

}
