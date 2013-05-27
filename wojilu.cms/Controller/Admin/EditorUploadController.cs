using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;

using wojilu.Net;
using wojilu.Web;
using wojilu.Web.Mvc;
using wojilu.Web.Utils;

using wojilu.cms.Domain;


namespace wojilu.cms.Controller.Admin {

    /// <summary>
    /// 给编辑器使用的：图片和文件上传
    /// </summary>
    public class EditorUploadController : ControllerBase {

        public void SavePic() {

            if (ctx.HasUploadFiles == false) {
                echoError( "不能上传空图片" );
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

            Dictionary<String, String> dic = new Dictionary<String, String>();
            dic.Add( "url", f.PicO ); // 图片的完整url
            dic.Add( "title", f.Name ); // 图片名称
            dic.Add( "original", ctx.GetFileSingle().FileName ); // 图片原始名称
            dic.Add( "state", "SUCCESS" ); // 上传成功

            echoJson( dic );
        }

        public void PicList() {

            List<String> imgs = new List<String>();
            DataPage<UploadFile> list = UploadFile.findPage( "FileType=" + (int)FileType.Pic );
            foreach (UploadFile post in list.Results) {
                imgs.Add( post.PicO );
            }

            echoJson( new { ImgList = imgs, Pager = list.PageBar } );
        }

        public void SaveFile() {

            Dictionary<String, String> dic = new Dictionary<String, String>();
            Result result = null;
            HttpFile file = null;

            if (ctx.HasUploadFiles == false) {
                errors.Add( "不能上传空图片" );
            }
            else {
                file = ctx.GetFileSingle();
                result = Uploader.SaveFile( file );
                if (result.HasErrors) {
                    errors.Add( result.ErrorsText );
                }
            }

            if (ctx.HasErrors) {
                dic.Add( "state", errors.ErrorsText );
                echoJson( dic );
                return;
            }

            string savedPath = result.Info.ToString();
            UploadFile f = new UploadFile();
            f.Name = file.FileName;
            f.Path = savedPath;
            f.FileType = (int)FileType.File;
            f.insert();

            dic.Add( "state", "SUCCESS" );
            dic.Add( "url", f.PicO );
            dic.Add( "fileType", f.Ext );
            dic.Add( "original", f.Name );

            echoJson( dic );
        }

        public void GetAuthJson() {

            ctx.web.ResponseContentType( "application/javascript" );

            String paramName = "window.uploadAuthParams";

            if (ctx.web.UserIsLogin == false) {
                echoText( string.Format( "{0} = null;", paramName ) );
            } else {
                String script = string.Format( "{0} = {1};", paramName, ctx.web.GetAuthJson() );
                echoText( script );
            }
        }

        public void GetRemotePic() {

            string uri = ctx.Post( "upfile" );
            uri = uri.Replace( "&amp;", "&" );
            string[] imgUrls = strUtil.Split( uri, "ue_separate_ue" );

            string[] filetype = { ".gif", ".png", ".jpg", ".jpeg", ".bmp" };             //文件允许格式
            int fileSize = 3000;                                                        //文件大小限制，单位kb

            ArrayList tmpNames = new ArrayList();
            WebClient wc = new WebClient();
            HttpWebResponse res;
            String tmpName = String.Empty;
            String imgUrl = String.Empty;
            String currentType = String.Empty;

            try {
                for (int i = 0, len = imgUrls.Length; i < len; i++) {
                    imgUrl = imgUrls[i];

                    if (imgUrl.Substring( 0, 7 ) != "http://") {
                        tmpNames.Add( "error!" );
                        continue;
                    }

                    //格式验证
                    int temp = imgUrl.LastIndexOf( '.' );
                    currentType = imgUrl.Substring( temp ).ToLower();
                    if (Array.IndexOf( filetype, currentType ) == -1) {
                        tmpNames.Add( "error!" );
                        continue;
                    }

                    String imgPath = PageLoader.DownloadPic( imgUrl );
                    tmpNames.Add( imgPath );
                }
            }
            catch (Exception) {
                tmpNames.Add( "error!" );
            }
            finally {
                wc.Dispose();
            }

            echoJson( "{url:'" + converToString( tmpNames ) + "',tip:'远程图片抓取成功！',srcUrl:'" + uri + "'}" );
        }

        //集合转换字符串
        private string converToString( ArrayList tmpNames ) {
            String str = String.Empty;
            for (int i = 0, len = tmpNames.Count; i < len; i++) {
                str += tmpNames[i] + "ue_separate_ue";
                if (i == tmpNames.Count - 1)
                    str += tmpNames[i];
            }
            return str;
        }



    }
}
