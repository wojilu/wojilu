/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Utils;
using wojilu.Apps.Photo.Domain;
using wojilu.Apps.Photo.Service;
using wojilu.Apps.Photo.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Web.Mvc.Attr;
using wojilu.Serialization;
using wojilu.Common.Msg.Domain;
using wojilu.Common.Msg.Service;
using wojilu.Common.Msg.Interface;
using wojilu.Web.Controller.Admin;

namespace wojilu.Web.Controller.Users {

    /// <summary>
    /// 用户上传模块。可以在网站、群组等任何地方使用。
    /// </summary>
    public class UserUploadController : ControllerBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( UserUploadController ) );

        public IPhotoPostService postService { get; set; }
        public IMessageAttachmentService attachmentService { get; set; }

        public UserUploadController() {
            postService = new PhotoPostService();
            attachmentService = new MessageAttachmentService();
        }

        public override void CheckPermission() {
            if (ctx.viewer.IsLogin == false) {
                redirectLogin();
            }
        }

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


        public void UploadForm() {

            Boolean isFlash = ("normal".Equals( ctx.Get( "type" ) ) == false);

            if (isFlash) {

                view( "FlashUpload" );
                set( "uploadLink", to( SaveFlash ) );

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

        public void SaveFlash() {

            PhotoPost post = savePicPrivate();

            if (ctx.HasErrors) {
                echoError();
                return;
            }

            // 只允许使用中略图，原始图片可能很大，影响页面效果
            set( "picUrl", post.ImgMediumUrl );
            set( "oPicUrl", post.ImgUrl );

            String json = "{PicUrl:'" + post.ImgMediumUrl + "', OpicUrl:'" + post.ImgUrl + "'}";

            echoText( json );

        }

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

        public void SaveMsgAttachment() {

            Result result = attachmentService.SaveFile( ctx.GetFileSingle() );
            
            Dictionary<String, String> dic = new Dictionary<String, String>();            

            if (result.HasErrors) {

                dic.Add( "FileName", "" );
                dic.Add( "DeleteLink", "" );
                dic.Add( "Msg", result.ErrorsText );

                echoText( JsonString.ConvertDictionary( dic ) );
            }
            else {

                MessageAttachment att = result.Info as MessageAttachment;
                String deleteLink = to( DeleteMsgAttachment, att.Id );

                dic.Add( "FileName", att.Name );
                dic.Add( "DeleteLink", deleteLink );
                dic.Add( "Id", att.Id.ToString() );

                echoText( JsonString.ConvertDictionary( dic ) );
            }
        }

        [HttpPost, DbTransaction]
        public void DeleteMsgAttachment( int id ) {

            Result result = attachmentService.Delete( id );

            if (result.HasErrors) {
                echoText( result.ErrorsText );
            }
            else {
                echoAjaxOk();
            }

        }

    }

}
