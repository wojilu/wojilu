using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Utils;
using wojilu.Apps.Download.Domain;

namespace wojilu.Web.Controller.Download.Admin {

    public class UploadController : ControllerBase {

        public void PreviewPic( int id ) {
            target( SavePreviewPic, id );

            FileItem f = FileItem.findById( id );
            IBlock block = getBlock( "previewPic" );

            if (f.HasPreviewPic()) {
                block.Set( "picPath", f.PreviewPicThumb );
                block.Next();
            }
            
        }

        public void SavePreviewPic( int id ) {

            if (ctx.HasUploadFiles==false) {
                errors.Add( "请上传图片" );
                run( PreviewPic, id );
                return;
            }

            Result result = Uploader.SaveImg( ctx.GetFileSingle() );
            if (result.HasErrors) {
                errors.Join( result );
                run( PreviewPic, id );
                return;
            }

            string picPath = result.Info.ToString();

            FileItem f = FileItem.findById( id );
            f.PreviewPic = picPath;
            f.update();

            echoRedirect( "上传成功", to( PreviewPic, id ) );



        }


    }

}
