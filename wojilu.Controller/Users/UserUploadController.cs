/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;

using wojilu.Apps.Photo.Domain;
using wojilu.Apps.Photo.Interface;
using wojilu.Apps.Photo.Service;
using wojilu.Common.Msg.Domain;
using wojilu.Common.Msg.Interface;
using wojilu.Common.Msg.Service;
using wojilu.Common.Upload;
using wojilu.Members.Users.Domain;
using wojilu.Net;
using wojilu.Web.Controller.Admin;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.Utils;

namespace wojilu.Web.Controller.Users {

    /// <summary>
    /// 用户上传模块。可以在网站、群组等任何地方使用。
    /// </summary>
    public class UserUploadController : ControllerBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( UserUploadController ) );

        public IPhotoPostService postService { get; set; }
        public IMessageAttachmentService attachmentService { get; set; }
        public UserFileService fileService { get; set; }

        public UserUploadController() {
            postService = new PhotoPostService();
            attachmentService = new MessageAttachmentService();
            fileService = new UserFileService();
        }

        [Login]
        public void MyPics() {

            String editorName = ctx.Get( "editor" );
            set( "editorName", editorName );

            DataPage<PhotoPost> list = postService.GetByUser( ctx.viewer.Id, 8 );
            IBlock block = getBlock( "list" );
            foreach (PhotoPost post in list.Results) {
                block.Set( "img.Thumb", post.ImgThumbUrl );
                block.Set( "img.OriginalUrl", post.ImgUrl );
                block.Set( "img.MediumUrl", post.ImgMediumUrl );
                block.Next();
            }
            set( "page", list.PageBar );
        }

        [Login]
        public void UploadForm() {

            Boolean isFlash = ("normal".Equals( ctx.Get( "type" ) ) == false);

            if (isFlash) {

                view( "FlashUpload" );
                set( "uploadLink", to( SaveEditorPic ) );

                set( "authJson", AdminSecurityUtils.GetAuthCookieJson( ctx ) );

                String editorName = ctx.Get( "editor" );
                set( "editorName", editorName );

                set( "normalLink", to( UploadForm ) + "?type=normal&editor=" + editorName );

                // swf上传跨域问题
                set( "jsPath", sys.Path.DiskJs );

            }
            else {
                target( SavePic );
                String editorName = ctx.Get( "editor" );
                set( "editorName", editorName );
            }

        }

        [Login]
        public void SavePic() {

            String editorName = ctx.Post( "editor" );
            set( "editorName", editorName );
            String uploadUrl = to( UploadForm ) + "?editor=" + editorName;
            set( "uploadUrl", uploadUrl );

            PhotoPost post = savePicPrivate();

            if (ctx.HasErrors) {
                echoRedirect( errors.ErrorsHtml, uploadUrl );
                return;
            }

            // 只允许使用中略图，原始图片可能很大，影响页面效果
            set( "picUrl", post.ImgMediumUrl );
            set( "oPicUrl", post.ImgUrl );

        }

        private PhotoPost savePicPrivate() {

            HttpFile postedFile = ctx.GetFileSingle();

            Result result = Uploader.SaveImg( postedFile );
            if (result.HasErrors) {

                errors.Join( result );
                return null;
            }

            PhotoPost post = new PhotoPost();
            post.OwnerId = ctx.viewer.Id;
            post.OwnerType = ctx.viewer.obj.GetType().FullName;
            post.OwnerUrl = ctx.viewer.obj.Url;

            post.Creator = (User)ctx.viewer.obj;
            post.CreatorUrl = ctx.viewer.obj.Url;
            post.DataUrl = result.Info.ToString();
            post.Title = strUtil.CutString( Path.GetFileNameWithoutExtension( postedFile.FileName ), 20 );
            post.Ip = ctx.Ip;
            post.PhotoAlbum = new PhotoAlbum();

            postService.CreatePostTemp( post );

            return post;
        }

        //---------------------------------------------------------------------


        [Login, DbTransaction]
        public void SaveUserFile() {

            Result result = fileService.SaveFile( ctx.GetFileSingle(), ctx.Ip, ctx.viewer.obj as User, ctx.owner.obj );

            Dictionary<String, String> dic = new Dictionary<String, String>();

            if (result.HasErrors) {

                dic.Add( "FileName", "" );
                dic.Add( "DeleteLink", "" );
                dic.Add( "Msg", result.ErrorsText );

                echoJson( dic );
            }
            else {

                UserFile att = result.Info as UserFile;

                updateDataInfo( att );

                String deleteLink = to( DeleteUserFile, att.Id );

                dic.Add( "FileName", att.Name );
                dic.Add( "DeleteLink", deleteLink );
                dic.Add( "Id", att.Id.ToString() );

                echoJson( dic );
            }
        }

        private void updateDataInfo( UserFile uFile ) {

            int dataId = ctx.PostInt( "dataId" );
            String dataType = ctx.Post( "dataType" );

            if (dataId <= 0 || strUtil.IsNullOrEmpty( dataType )) return;

            uFile.DataId = dataId;
            uFile.DataType = dataType;

            uFile.update();
            fileService.UpdateDataInfo( uFile );
        }

        [Login, DbTransaction]
        public void DeleteUserFile( int id ) {

            Result result = fileService.Delete( id );

            if (result.HasErrors) {
                echoText( result.ErrorsText );
            }
            else {
                echoAjaxOk();
            }
        }

        //----------------------------------------------------------------------------------------

        [Login]
        public void SaveMsgAttachment() {

            Result result = attachmentService.SaveFile( ctx.GetFileSingle() );

            Dictionary<String, String> dic = new Dictionary<String, String>();

            if (result.HasErrors) {

                dic.Add( "FileName", "" );
                dic.Add( "DeleteLink", "" );
                dic.Add( "Msg", result.ErrorsText );

                echoText( Json.ToString( dic ) );
            }
            else {

                MessageAttachment att = result.Info as MessageAttachment;
                String deleteLink = to( DeleteMsgAttachment, att.Id );

                dic.Add( "FileName", att.Name );
                dic.Add( "DeleteLink", deleteLink );
                dic.Add( "Id", att.Id.ToString() );

                echoText( Json.ToString( dic ) );
            }
        }

        [Login, HttpPost, DbTransaction]
        public void DeleteMsgAttachment( int id ) {

            Result result = attachmentService.Delete( id );

            if (result.HasErrors) {
                echoText( result.ErrorsText );
            }
            else {
                echoAjaxOk();
            }

        }


        //-----------------以下是编辑器上传功能-------------------------------------------------


        public void GetAuthJson() {

            ctx.web.ResponseContentType( "application/javascript" );

            String paramAuth = "window.uploadAuthParams";
            String paramFileSizeMB = "window.fileMaxMB";
            String paramPicSizeMB = "window.picMaxMB";

            String paramFileTypes = "window.fileTypes";
            String paramPicTypes = "window.picTypes";


            if (ctx.viewer.IsLogin == false) {
                echoText( string.Format( "{0} = null;{1}=0;{2}=0;{3}='';{4}='';", paramAuth, paramFileSizeMB, paramPicSizeMB, paramFileTypes, paramPicTypes ) );
            }
            else {
                String script = string.Format( "{0} = {1};{2}={3};{4}={5};{6}='{7}';{8}='{9}';",
                    paramAuth, ctx.web.GetAuthJson(),
                    paramFileSizeMB, config.Instance.Site.UploadFileMaxMB,
                    paramPicSizeMB, config.Instance.Site.UploadPicMaxMB,
                    paramFileTypes, getExtString( config.Instance.Site.UploadFileTypes ),
                    paramPicTypes, getExtString( config.Instance.Site.UploadPicTypes )
                    );
                echoText( script );
            }
        }

        /// <summary>
        /// 返回 *.7z;*.zip;*.rar
        /// </summary>
        /// <param name="arrExt"></param>
        /// <returns></returns> 
        private String getExtString( string[] arrExt ) {

            String str = "";
            foreach (String ext in arrExt) {
                if (strUtil.IsNullOrEmpty( ext )) continue;
                str += strUtil.Join( "*", ext, "." ) + ";";
            }

            return str;
        }


        [Login]
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


        [Login]
        public void MyPicJson() {

            List<String> imgs = new List<String>();
            DataPage<PhotoPost> list = postService.GetByUser( ctx.viewer.Id, 15 );
            foreach (PhotoPost post in list.Results) {
                imgs.Add( post.ImgMediumUrl );
            }

            echoJson( new { ImgList = imgs, Pager = list.PageBar } );
        }


        [Login]
        public void SaveEditorPic() {

            PhotoPost post = savePicPrivate();

            if (ctx.HasErrors) {
                echoError();
                return;
            }

            // 只允许使用中略图，原始图片可能很大，影响页面效果
            set( "picUrl", post.ImgMediumUrl );
            set( "oPicUrl", post.ImgUrl );

            Dictionary<String, String> dic = new Dictionary<String, String>();
            dic.Add( "url", post.ImgUrl ); // 图片的完整url
            dic.Add( "title", post.Title ); // 图片名称
            dic.Add( "original", ctx.GetFileSingle().FileName ); // 图片原始名称
            dic.Add( "state", "SUCCESS" ); // 上传成功

            echoJson( dic );
        }


        [Login, DbTransaction]
        public void SaveEditorFile() {

            Result result = fileService.SaveFile( ctx.GetFileSingle(), ctx.Ip, ctx.viewer.obj as User, ctx.owner.obj );

            Dictionary<String, String> dic = new Dictionary<String, String>();

            if (result.HasErrors) {

                dic.Add( "state", result.ErrorsText );

                echoJson( dic );
            }
            else {

                UserFile att = result.Info as UserFile;

                updateDataInfo( att );

                dic.Add( "state", "SUCCESS" );
                dic.Add( "url", att.PathFull );
                dic.Add( "fileType", att.Ext );
                dic.Add( "original", att.FileName );

                echoJson( dic );
            }
        }


    }

}
