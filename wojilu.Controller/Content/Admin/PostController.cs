using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;
using wojilu.Web.Controller.Content.Admin.Section;
using wojilu.Web.Controller.Common;
using wojilu.Web.Controller.Content.Utils;
using wojilu.DI;
using wojilu.Apps.Content.Enum;
using System.Web;
using wojilu.Web.Controller.Users.Admin;
using wojilu.Web.Utils;
using wojilu.Serialization;
using wojilu.Drawing;
using wojilu.Common.Upload;
using wojilu.Common.AppBase;
using wojilu.Web.Controller.Admin;

namespace wojilu.Web.Controller.Content.Admin {

    [App( typeof( ContentApp ) )]
    public class PostController : ControllerBase {

        public IContentPostService postService { get; set; }
        public IContentSectionService sectionService { get; set; }
        public IAttachmentService attachService { get; set; }

        public PostController() {
            postService = new ContentPostService();
            sectionService = new ContentSectionService();
            attachService = new AttachmentService();

            HideLayout( typeof( wojilu.Web.Controller.Content.LayoutController ) );
        }

        public void Add( int sectionId ) {

            target( to( Create ) + "?categoryId=" + ctx.GetInt( "categoryId" ) + "&fromList=" + ctx.GetInt( "fromList" ) );
            editor( "Content", "", "280px" );

            List<ContentSection> sections = sectionService.GetInputSectionsByApp( ctx.app.Id );
            checkboxList( "postSection", sections, "Title=Id", 0 );

            set( "created", DateTime.Now );

            set( "sectionId", sectionId );

            set( "width", ctx.GetInt( "width" ) );
            set( "height", ctx.GetInt( "height" ) );

            set( "uploadLink", to( new AttachmentController().SaveFlashFile ) ); // 接受上传的网址
            set( "imgUploadLink", to( SavePic ) ); //图片上传

            set( "authJson", AdminSecurityUtils.GetAuthCookieJson( ctx ) );


            radioList( "PickStatus", PickStatus.GetPickStatus(), "0" );
        }

        //--------------------------------------------------------------------------------------------------------

        public void SavePic() {

            Dictionary<String, String> dic = new Dictionary<String, String>();

            HttpFile postedFile = ctx.GetFileSingle();

            TempUploadFileService uploadService = new TempUploadFileService();
            Result result = uploadService.Upload( postedFile );

            if (result.HasErrors) {

                dic.Add( "PicUrl", "" );
                dic.Add( "Msg", result.ErrorsText );
                echoText( JsonString.ConvertDictionary( dic ) );
            }
            else {

                TempUploadFile tfile = result.Info as TempUploadFile;

                dic.Add( "PicUrl", tfile.FileUrl );
                dic.Add( "PicThumbUrl", tfile.FileThumbUrl );
                dic.Add( "DeleteLink", to( DeleteTempPic, tfile.Id ) );

                echoText( JsonString.ConvertDictionary( dic ) );
            }

        }

        public void DeleteTempPic( int id ) {

            TempUploadFileService uploadService = new TempUploadFileService();
            Result result = uploadService.DeleteTempFile( id );

            if (result.HasErrors) {
                echoText( result.ErrorsText );
            }
            else {
                echoAjaxOk();
            }

        }


        //--------------------------------------------------------------------------------------------------------


        public void Create() {

            ContentPost post = ContentValidator.Validate( ctx );
            ContentValidator.ValidateArticle( post, ctx );

            String sectionIds = ctx.PostIdList( "postSection" );
            if (strUtil.IsNullOrEmpty( sectionIds )) errors.Add( "请选择区块" );

            int[] arrAttachmentIds = cvt.ToIntArray( ctx.PostIdList( "attachmentIds" ) );

            // 图片默认值处理
            if (strUtil.HasText( post.ImgLink )) {

                if (post.Width <= 0) {
                    post.Width = 100;
                    post.Height = 85;
                }
            }

            if (ctx.HasErrors) {
                echoError();
                return;
            }


            // 处理远程图片
            if (ctx.PostIsCheck( "isDowloadPic" ) == 1) {
                post.Content = wojilu.Net.PageLoader.ProcessPic( post.Content, "" );
            }

            postService.Insert( post, sectionIds, ctx.Post( "TagList" ) );
            attachService.UpdateAtachments( arrAttachmentIds, post );

            if (ctx.GetInt( "fromList" ) > 0) {
                echoRedirectPart( lang( "opok" ), to( List ), 1 );
            }
            else {
                echoToParentPart( lang( "opok" ) );
            }


        }

        //--------------------------------------------------------------------------------------------------------

        public void EditImg( int postId ) {
            view( "Edit" );
            this.Edit( postId );
        }

        public void Edit( int postId ) {

            ContentPost post = postService.GetById( postId, ctx.owner.Id );
            if (post == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            target( to( Update, postId ) + "?categoryId=" + ctx.GetInt( "categoryId" ) );

            bindEditInfo( post );

            List<ContentSection> sectionList = sectionService.GetInputSectionsByApp( ctx.app.Id );
            String sectionIds = sectionService.GetSectionIdsByPost( postId );

            checkboxList( "postSection", sectionList, "Title=Id", 0 );
            set( "sectionIds", sectionIds );

            //--------------------------上传信息----------------------------------

            //set( "uploadLink", to( new AttachmentController().SaveFlashFile ) ); // 接受上传的网址
            set( "attachmentLink", to( new AttachmentController().AdminList, postId ) );

            set( "imgUploadLink", to( SavePic ) ); //图片上传
            set( "authJson", AdminSecurityUtils.GetAuthCookieJson( ctx ) );


        }

        private void bindEditInfo( ContentPost post ) {

            if (post.PageSection == null) return;

            set( "post.DeleteUrl", to( Delete, post.Id ) );

            set( "post.Author", post.Author );
            set( "post.Title", post.Title );
            set( "post.TitleHome", strUtil.EncodeTextarea( post.TitleHome ) );

            set( "post.Width", post.Width );
            set( "post.Height", post.Height );

            editor( "Content", strUtil.Edit( post.Content ), "250px" );

            set( "post.Created", post.Created );
            set( "post.Hits", post.Hits );
            set( "post.OrderId", post.OrderId );

            set( "post.RedirectUrl", post.RedirectUrl );
            set( "post.MetaKeywords", post.MetaKeywords );
            set( "post.MetaDescription", post.MetaDescription );


            set( "post.Summary", post.Summary );
            set( "post.SourceLink", post.SourceLink );
            set( "post.Style", post.Style );

            set( "post.ImgLink", post.GetImgUrl() );
            set( "post.ImgThumbLink", post.GetImgThumb() );
            set( "post.ImgDeleteLink", to( DeletePostPic, post.Id ) );


            set( "post.TagList", post.Tag.TextString );
            String val = AccessStatusUtil.GetRadioList( post.AccessStatus );
            set( "post.AccessStatus", val );
            set( "post.IsCloseComment", Html.CheckBox( "IsCloseComment", lang( "closeComment" ), "1", cvt.ToBool( post.CommentCondition ) ) );

            radioList( "PickStatus", PickStatus.GetPickStatus(), post.PickStatus.ToString() );
            

            set( "attachmentAdminLink", to( new AttachmentController().AdminList, post.Id ) );
        }

        public void DeletePostPic( int id ) {

            ContentPost post = postService.GetById( id, ctx.owner.Id );
            if (post == null) {
                echoText( "data not found" );
                return;
            }


            wojilu.Drawing.Img.DeleteImgAndThumb( post.GetImgUrl() );
            echoAjaxOk();
        }


        [HttpPost, DbTransaction]
        public void Update( int postId ) {
            ContentPost post = postService.GetById( postId, ctx.owner.Id );
            if (post == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            String sectionIds = ctx.PostIdList( "postSection" );

            ContentValidator.ValidateEdit( post, ctx );
            ContentValidator.ValidateArticle( post, ctx );
            if (errors.HasErrors) {
                run( Edit, postId );
            }
            else {

                if (ctx.PostIsCheck( "saveContentPic" ) == 1) {
                    post.Content = wojilu.Net.PageLoader.ProcessPic( post.Content, null );
                }

                postService.Update( post, sectionIds, ctx.Post( "TagList" ) );

                echoToParentPart( lang( "opok" ) );
            }
        }


        //--------------------------------------------------------------------------------------------------------

        public void List() {

            set( "addUrl", to( Add, 0 ) );

            set( "OperationUrl", to( SaveAdmin ) );
            ContentApp app = ctx.app.obj as ContentApp;
            set( "app.Name", ctx.app.Name );
            set( "app.Link", to( new ContentController().Index ) );
            set( "searchKey", "" );

            set( "tagAction", to( SaveTag ) );

            DataPage<ContentPost> posts = postService.GetByApp( ctx.app.Id, 50 );

            bool isTrash = false;
            bindAdminList( posts, isTrash );

            //bindCategories(app);

            target( Search );
        }

        //private void bindCategories( ContentApp app ) {
        //    List<ContentSection> sections = sectionService.GetInputSectionsByApp( app.Id );
        //    bindList( "cats", "category", sections );
        //}

        public void SaveTag() {

            int postId = ctx.PostInt( "postId" );
            String tagValue = ctx.Post( "tagValue" );

            if (strUtil.IsNullOrEmpty( tagValue )) {
                echoText( "请输入内容" );
                return;
            }

            ContentPost post = postService.GetById( postId, ctx.owner.Id );
            if (post == null) { echo( lang( "exDataNotFound" ) ); return; }

            post.Tag.Save( tagValue );

            echoAjaxOk();
        }

        public void EditSection( int id ) {
            target( UpdateSection, id );

            ContentPost post = postService.GetById( id, ctx.owner.Id );
            if (post == null) { echo( lang( "exDataNotFound" ) ); return; }
            if (post.PageSection == null) { echo( lang( "exDataNotFound" ) + ":post property PageSection" ); return; }
            List<ContentSection> sections = sectionService.GetInputSectionsByApp( ctx.app.Id );

            dropList( "SectionId", sections, "Title=Id", post.PageSection.Id );
        }

        public void UpdateSection( int id ) {

            int sectionId = ctx.PostInt( "SectionId" );
            ContentPost post = postService.GetById( id, ctx.owner.Id );
            if (post == null) { echo( lang( "exDataNotFound" ) ); return; }
            postService.UpdateSection( post, sectionId );

            echoToParentPart( lang( "opok" ) );
        }

        public void EditTitleStyle( int id ) {
            target( UpdateTitleStyle, id );
            load( "TextStyle", new FormController().TextStyle );
        }

        public void UpdateTitleStyle( int id ) {

            String titleStyle = strUtil.SqlClean( FormController.GetTitleStyle( ctx ), 100 );

            ContentPost post = postService.GetById( id, ctx.owner.Id );
            if (post == null) { echo( lang( "exDataNotFound" ) ); return; }
            postService.UpdateTitleStyle( post, titleStyle );
            echoToParentPart( lang( "opok" ) );
        }

        public void Trash() {

            ContentApp app = ctx.app.obj as ContentApp;
            set( "app.Name", ctx.app.Name );
            set( "app.Link", to( new ContentController().Index ) );

            DataPage<ContentPost> posts = postService.GetTrashByApp( ctx.app.Id, 50 );

            bool isTrash = true;
            bindAdminList( posts, isTrash );

            target( Search );
        }

        public void Search() {

            view( "List" );

            ContentApp app = ctx.app.obj as ContentApp;
            set( "app.Name", ctx.app.Name );
            set( "app.Link", to( new ContentController().Index ) );


            String key = strUtil.SqlClean( ctx.Get( "q" ), 10 );
            set( "searchKey", key );

            target( Search );

            DataPage<ContentPost> posts = postService.GetBySearch( ctx.app.Id, key, 50 );
            bool isTrash = false;
            bindAdminList( posts, isTrash );

        }

        [HttpPost, DbTransaction]
        public void SaveAdmin() {

            String ids = ctx.PostIdList( "choice" );

            if (strUtil.IsNullOrEmpty( ids )) {
                redirect( List );
                return;
            }

            String cmd = ctx.Post( "action" );


            if ("category" == cmd) {
                int sectionId = ctx.PostInt( "categoryId" );
                postService.UpdateSection( ids, sectionId );
            }
            else if ("deletetrue" == cmd) {
                postService.DeleteBatch( ids );
            }
            else if ("status_pick" == cmd) {
                postService.SetStatus_Pick( ids );
            }
            else if ("status_normal" == cmd) {
                postService.SetStatus_Normal( ids );
            }
            else if ("status_focus" == cmd) {
                postService.SetStatus_Focus( ids );
            }

            echoAjaxOk();
        }



        private void bindAdminList( DataPage<ContentPost> posts, bool isTrash ) {

            IBlock block = getBlock( "list" );


            foreach (ContentPost post in posts.Results) {


                String typeIcon = BinderUtils.getTypeIcon( post );
                String pickIcon = BinderUtils.getPickedIcon( post );
                String attIcon = post.Attachments > 0 ? BinderUtils.iconAttachment : "";

                block.Set( "post.ImgIcon", typeIcon );
                block.Set( "post.PickIcon", pickIcon );
                block.Set( "post.AttachmentIcon", attIcon );

                block.Set( "post.Title", strUtil.SubString( post.GetTitle(), 50 ) );
                block.Set( "post.TitleCss", post.Style );
                block.Set( "post.TitleFull", post.Title );

                //String sectionName = post.PageSection == null ? "" : post.PageSection.Title;
                //block.Set( "post.SectionName", sectionName );

                block.Set( "post.Url", post.SourceLink );
                block.Set( "post.Link", alink.ToAppData( post ) );
                block.Set( "post.PubDate", post.Created );

                if (post.Creator != null) {
                    block.Set( "post.Submitter", string.Format( "<a href=\"{0}\" target=\"_blank\">{1}</a>", Link.ToMember( post.Creator ), post.Creator.Name ) );
                }
                else {
                    block.Set( "post.Submitter", "无" );
                }



                block.Bind( "post", post );

                String lnkEdit = "";
                if (isTrash) {
                    lnkEdit = "#";
                }
                else if (post.HasImg()) {
                    lnkEdit = to( new PostController().EditImg, post.Id );
                }
                else {
                    lnkEdit = to( new PostController().Edit, post.Id );
                }

                String lnkDelete = to( Delete, post.Id );
                if (isTrash) lnkDelete = to( DeleteTrue, post.Id );

                block.Set( "post.EditUrl", lnkEdit );
                block.Set( "post.DeleteUrl", lnkDelete );

                block.Set( "post.RestoreUrl", to( Restore, post.Id ) );
                block.Set( "post.EditSectionUrl", to( EditSection, post.Id ) );
                block.Set( "post.EditTitleStyleUrl", to( EditTitleStyle, post.Id ) );

                block.Set( "post.AttachmentLink", to( new AttachmentController().AdminList, post.Id ) );


                block.Next();
            }
            set( "page", posts.PageBar );
        }



        [HttpDelete, DbTransaction]
        public void Delete( int postId ) {
            ContentPost post = postService.GetById( postId, ctx.owner.Id );
            if (post == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            postService.Delete( post );

            echoRedirectPart( lang( "opok" ) );
        }

        [HttpPut, DbTransaction]
        public void Restore( int id ) {

            postService.Restore( id );
            ContentPost post = postService.GetById( id, ctx.owner.Id );

            echoRedirectPart( lang( "opok" ) );
        }

        [HttpDelete, DbTransaction]
        public void DeleteTrue( int postId ) {
            postService.DeleteTrue( postId );


            echoRedirectPart( lang( "opok" ) );
        }




    }

}
