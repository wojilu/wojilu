using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.cms.Domain;
using wojilu.Web;
using System.Web;
using wojilu.Web.Utils;

namespace wojilu.cms.Controller.Admin {

    public class FileController : ControllerBase {

        public void Index() {

            set( "fileSaveLink", to( SaveFile ) );
            set( "picSaveLink", to( SavePic ) );

            DataPage<UploadFile> files = UploadFile.findPage( "" );
            IBlock block = getBlock( "list" );
            foreach (UploadFile f in files.Results) {
                block.Set( "f.Id", f.Id );
                block.Set( "f.Name", f.Name );
                block.Set( "f.Path", f.Path );
                block.Set( "f.Created", f.Created );
                block.Set( "f.Info", getFileInfo( f ) );
                block.Set( "f.DeleteLink", to( Delete, f.Id ) );
                block.Next();
            }
            set( "page", files.PageBar );
        }

        private string getFileInfo( UploadFile f ) {
            if (f.FileType == (int)FileType.Pic)
                return string.Format( "<a href='{0}'><img src='{1}' /></a>", f.FullPath, f.FullThumbPath );
            return string.Format( "<a href='{0}'>{0}</a>", f.FullPath );
        }

        public void SaveFile() {

            if (ctx.HasUploadFiles == false) {
                echoRedirect( "不能上传空文件", Index );
                return;
            }

            HttpFile file = ctx.GetFileSingle();
            Result result = Uploader.SaveFile( file );
            string savedPath = result.Info.ToString();

            UploadFile f = new UploadFile();
            f.Name = file.FileName;
            f.Path = savedPath;
            f.FileType = (int)FileType.File;
            f.insert();

            redirect( Index );
        }

        public void SavePic() {

            if (ctx.HasUploadFiles == false) {
                echoRedirect( "不能上传空图片", Index );
                return;
            }

            HttpFile file = ctx.GetFileSingle();
            Result result = Uploader.SaveImg( file );
            string savedPath = result.Info.ToString();

            UploadFile f = new UploadFile();
            f.Name = file.FileName;
            f.Path = savedPath;
            f.FileType = (int)FileType.Pic;
            f.insert();

            redirect( Index );
        }

        public void Delete( int id ) {

            UploadFile f = UploadFile.findById( id );
            if (f == null) {
                echoRedirect( "文件不存在" );
                return;
            }

            if (f.FileType == (int)FileType.Pic)
                wojilu.Drawing.Img.DeleteImgAndThumb( PathHelper.Map( f.FullPath ) );
            else
                wojilu.IO.File.Delete( PathHelper.Map( f.FullPath ) );

            f.delete();

            redirect( Index );
        }

    }

}
