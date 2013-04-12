using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Utils;
using wojilu.Drawing;

namespace wojilu.Web.Controller.Demo {

    public class UploadController : ControllerBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( UploadController ) );

        public void Index() {
        }

        public void CommonSave() {
        }

        public void FrameUpload() {
            set( "uploadLink", to( FrameSave ) ); // 接受上传的网址
        }

        public void FrameSave() {

            logger.Info( "iframe upload" );

            Result result = Uploader.SaveImg( ctx.GetFileSingle() );
            if (result.HasErrors) {
                return;
            }
            String picName = result.Info.ToString(); // 获取图片名称
            String picUrl = strUtil.Join( sys.Path.Photo, picName ); // 获取图片完整路径
            picUrl = Img.GetThumbPath( picUrl, ThumbnailType.Medium );// 获取中等缩略图

            // 这里的内容返回给 iframe
            StringBuilder sb = new StringBuilder();
            sb.Append( "<script>" );
            // 这段内容在iframe中，所以通过 parent 来调用主页面的方法
            sb.Append( "parent.showPic('" + picUrl + "')" ); 
            sb.Append( "</script>" );

            echoText( sb.ToString() );
        }

        //----------------------------------------------------------------------------------

        public void FlashUpload() {
            set( "uploadLink", to( FlashSave ) ); // 接受上传的网址
            set( "authJson", ctx.web.GetAuthJson() );

            // 如果有其他安全cookie，请指定。否则没有权限
            //set( "authJson", ctx.web.GetAuthJson( "_cookieName" ) );
        }

        public void FlashSave() {

            Result result = Uploader.SaveImg( ctx.GetFileSingle() );
            if (result.HasErrors) {
                echoError( result ); // 返回错误信息
                return;
            }

            String picName = result.Info.ToString(); // 获取图片名称
            String picUrl = strUtil.Join( sys.Path.Photo, picName ); // 获取图片完整路径
            picUrl = Img.GetThumbPath( picUrl, ThumbnailType.Medium );// 获取中等缩略图

            echoJsonMsg( "ok", true, picUrl ); // 将图片网址返回给客户端
        }

    }

}
