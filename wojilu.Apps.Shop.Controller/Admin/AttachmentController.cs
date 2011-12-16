using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.Utils;

using wojilu.Members.Users.Domain;
using wojilu.Common.AppBase.Interface;

using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Domain;
using wojilu.Apps.Shop.Service;
using wojilu.Common.AppBase;
using wojilu.DI;
using wojilu.Serialization;

namespace wojilu.Web.Controller.Shop.Admin {

    [App( typeof( ShopApp ) )]
    public class AttachmentController : ControllerBase {

        public IShopItemService postService { get; set; }
        public IAttachmentService attachService { get; set; }

        public AttachmentController() {
            postService = new ShopItemService();
            attachService = new AttachmentService();
        }

        public void AdminList( int ItemId ) {
            ShopItem post = postService.GetById( ItemId, ctx.owner.Id );
            if (post == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            bindAttachments( post );
        }


        private void bindAttachments( ShopItem post ) {

            set( "addLink", to( new AttachmentController().Add, post.Id ) );
            set( "sortAction", to( new AttachmentController().SaveSort, post.Id ) );

            ShopItem topic = postService.GetById( post.Id, ctx.owner.Id );
            List<ShopItemAttachment> attachList = attachService.GetAttachmentsByPost( post.Id );

            String cmd;
            if (topic.IsAttachmentLogin == 1) {
                String lockImg = strUtil.Join( sys.Path.Img, "edit.gif" );
                cmd = alang( "currentDownloadPermission" );
                cmd += string.Format( "<img src=\"{1}\" /> <a href=\"{0}\" class=\"frmBox\">", to( new AttachmentController().SetPermission, post.Id ), lockImg );
                cmd += lang( "edit" ) + "</a>";
            }
            else {
                String lockImg = strUtil.Join( sys.Path.Img, "lock.gif" );
                cmd = string.Format( "<img src=\"{1}\" /> <a href=\"{0}\" class=\"frmBox\">" + alang( "setDownloadPermission" ) + "</a>", to( new AttachmentController().SetPermission, post.Id ), lockImg );
            }
            set( "cmd", cmd );


            bindAttachments( attachList );

        }

        private void bindAttachments( List<ShopItemAttachment> attachList ) {
            IBlock block = getBlock( "list" );

            foreach (ShopItemAttachment attachment in attachList) {

                block.Set( "a.Id", attachment.Id );

                block.Set( "a.Name", attachment.GetFileShowName() );

                String str;

                if (isImage( attachment )) {
                    str = string.Format( "<div><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" /></a></div>",
                        attachment.FileUrl, attachment.FileThumbUrl );
                }
                else {
                    str = alang( "noThumb" );
                }

                block.Set( "a.Info", str );
                block.Set( "a.Size", attachment.FileSizeKB );
                block.Set( "a.Downloads", attachment.Downloads );

                block.Set( "a.RenameLink", to( new AttachmentController().Rename, attachment.ItemId ) + "?aid=" + attachment.Id );

                block.Set( "a.UploadLink", to( new AttachmentController().Upload, attachment.ItemId ) + "?aid=" + attachment.Id );
                block.Set( "a.DeleteLink", to( new AttachmentController().Delete, attachment.ItemId ) + "?aid=" + attachment.Id );

                block.Next();
            }
        }

        private Boolean isImage( ShopItemAttachment attachment ) {
            return Uploader.IsImage( attachment.Type );
        }

        public void SetPermission( int ItemId ) {

            ShopItem post = postService.GetById( ItemId, ctx.owner.Id );

            String chk = post.IsAttachmentLogin == 1 ? "checked=\"checked\"" : "";
            set( "checked", chk );

            target( SavePermission, ItemId );
        }

        [HttpPost, DbTransaction]
        public void SavePermission( int ItemId ) {

            ShopItem post = postService.GetById( ItemId, ctx.owner.Id );
            int ischeck = ctx.PostIsCheck( "IsAttachmentLogin" );
            postService.UpdateAttachmentPermission( post, ischeck );
            echoToParent( lang( "opok" ) );
        }


        [HttpPost, DbTransaction]
        public virtual void SaveSort( int ItemId ) {

            int id = ctx.PostInt( "id" );
            String cmd = ctx.Post( "cmd" );

            ShopItemAttachment data = attachService.GetById( id );

            ShopItem post = postService.GetById( ItemId, ctx.owner.Id );
            List<ShopItemAttachment> list = attachService.GetAttachmentsByPost( ItemId );

            if (cmd == "up") {

                new SortUtil<ShopItemAttachment>( data, list ).MoveUp();
                echoRedirect( "ok" );
            }
            else if (cmd == "down") {

                new SortUtil<ShopItemAttachment>( data, list ).MoveDown();
                echoRedirect( "ok" );
            }
            else {
                echoError( lang( "exUnknowCmd" ) );
            }

        }


        public void Add( int ItemId ) {

            target( SaveAdd, ItemId );
        }


        [HttpPost, DbTransaction]
        public void SaveAdd( int ItemId ) {

            ShopItem post = postService.GetById( ItemId, ctx.owner.Id );
            HttpFile postedFile = ctx.GetFileSingle();

            Result result = Uploader.SaveFileOrImage( postedFile );

            if (result.HasErrors) {
                errors.Join( result );
                run( Add, ItemId );
                return;
            }

            ShopItemAttachment uploadFile = new ShopItemAttachment();
            uploadFile.FileSize = postedFile.ContentLength;
            uploadFile.Type = postedFile.ContentType;
            uploadFile.Name = result.Info.ToString();
            uploadFile.Description = ctx.Post( "Name" );
            uploadFile.AppId = ctx.app.Id;
            uploadFile.ItemId = ItemId;

            attachService.Create( uploadFile, (User)ctx.viewer.obj, ctx.owner.obj );

            echoToParent( lang( "opok" ) );
        }
        //--------------------------------------------------------------------------------------------------------------

        public void SaveFlashFile() {

            HttpFile postedFile = ctx.GetFileSingle();

            Result result = attachService.SaveFile( postedFile );

            Dictionary<String, String> dic = new Dictionary<String, String>();

            if (result.HasErrors) {

                dic.Add( "FileName", "" );
                dic.Add( "DeleteLink", "" );
                dic.Add( "Msg", result.ErrorsText );

                echoText( JsonString.ConvertDictionary( dic ) );
            }
            else {

                ShopItemAttachment att = result.Info as ShopItemAttachment;
                String deleteLink = to( DeleteAttachment, att.Id );

                dic.Add( "FileName", att.Description );
                dic.Add( "DeleteLink", deleteLink );
                dic.Add( "Id", att.Id.ToString() );

                echoText( JsonString.ConvertDictionary( dic ) );
            }

        }

        [HttpPost, DbTransaction]
        public void DeleteAttachment( int id ) {

            attachService.Delete( id ); // 删除文件，并且删除附件在数据库中的临时记录
            echoAjaxOk();
        }


        //--------------------------------------------------------------------------------------------------------------

        public void Rename( int ItemId ) {

            int id = ctx.GetInt( "aid" );
            set( "ActionLink", to( SaveRename, ItemId ) + "?aid=" + id );

            ShopItemAttachment attachment = attachService.GetById( id );
            if (attachment == null) {
                echoRedirect( alang( "exAttrachment" ) );
                return;
            }

            set( "name", attachment.GetFileShowName() );
        }

        [HttpPost, DbTransaction]
        public void SaveRename( int ItemId ) {

            ShopItem post = postService.GetById( ItemId, ctx.owner.Id );
            int id = ctx.GetInt( "aid" );

            String name = ctx.Post( "Name" );
            if (strUtil.IsNullOrEmpty( name )) {
                errors.Add( lang( "exName" ) );
                run( Rename, id );
                return;
            }

            ShopItemAttachment attachment = attachService.GetById( id );
            if (attachment == null) {
                echoRedirect( alang( "exAttrachment" ) );
                return;
            }

            attachService.UpdateName( attachment, name );
            echoToParent( lang( "opok" ) );
        }

        public void Upload( int ItemId ) {

            int id = ctx.GetInt( "aid" );

            ShopItemAttachment attachment = attachService.GetById( id );
            if (attachment == null) {
                echoRedirect( alang( "exAttrachment" ) );
                return;
            }

            set( "ActionLink", to( SaveUpload, ItemId ) + "?aid=" + id );
        }

        [HttpPost, DbTransaction]
        public void SaveUpload( int ItemId ) {

            ShopItem post = postService.GetById( ItemId, ctx.owner.Id );
            int id = ctx.GetInt( "aid" );

            ShopItemAttachment attachment = attachService.GetById( id );
            if (attachment == null) {
                echoRedirect( alang( "exAttrachment" ) );
                return;
            }

            HttpFile postedFile = ctx.GetFileSingle();

            Result result = Uploader.SaveFileOrImage( postedFile );

            if (result.HasErrors) {
                errors.Join( result );
                run( Upload, id );
                return;
            }

            String toDeleteFile = attachment.FileUrl;

            attachment.FileSize = postedFile.ContentLength;
            attachment.Type = postedFile.ContentType;
            attachment.Name = result.Info.ToString();

            attachService.UpdateFile( attachment, toDeleteFile );

            echoToParent( lang( "opok" ) );
        }

        [HttpDelete, DbTransaction]
        public void Delete( int ItemId ) {

            ShopItem post = postService.GetById( ItemId, ctx.owner.Id );
            int id = ctx.GetInt( "aid" );

            ShopItemAttachment attachment = attachService.GetById( id );
            if (attachment == null) {
                echoRedirect( alang( "exAttrachment" ) );
                return;
            }

            attachService.Delete( id );

            echoRedirect( lang( "opok" ) );
        }




    }
}
