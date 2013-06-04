/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;

using wojilu.Web.Controller.Common;
using wojilu.Web.Controller.Content.Utils;
using wojilu.Web.Controller.Admin;
using wojilu.Web.Controller.Content.Caching;

using wojilu.Apps.Content.Enum;

using wojilu.Common.Upload;
using wojilu.Common.AppBase;
using wojilu.Web.Controller.Content.Htmls;
using wojilu.Members.Sites.Domain;
using wojilu.Common.MemberApp.Interface;
using wojilu.Members.Sites.Service;

namespace wojilu.Web.Controller.Content.Admin.Common {

    [App( typeof( ContentApp ) )]
    public class PostController : ControllerBase {

        public virtual IContentPostService postService { get; set; }
        public virtual IContentSectionService sectionService { get; set; }
        public virtual IAttachmentService attachService { get; set; }
        public virtual IMemberAppService appService { get; set; }

        public PostController() {
            postService = new ContentPostService();
            sectionService = new ContentSectionService();
            attachService = new AttachmentService();
            appService = new SiteAppService();

            HideLayout( typeof( wojilu.Web.Controller.Content.LayoutController ) );
        }

        public void Add( int sectionId ) {

            target( to( Create ) + "?categoryId=" + ctx.GetInt( "categoryId" ) + "&fromList=" + ctx.GetInt( "fromList" ) );

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
                echoText( Json.ToString( dic ) );
            }
            else {

                TempUploadFile tfile = result.Info as TempUploadFile;

                dic.Add( "PicUrl", tfile.FileUrl );
                dic.Add( "PicThumbUrl", tfile.FileThumbUrl );
                dic.Add( "DeleteLink", to( DeleteTempPic, tfile.Id ) );

                echoText( Json.ToString( dic ) );
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

            ContentPost post = ContentValidator.SetValue( ctx );
            ContentValidator.ValidateTitleBody( post, ctx );

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
                echoRedirectPart( lang( "opok" ), to( List, 0 ), 1 );
            }
            else {
                echoToParentPart( lang( "opok" ) );
            }

            HtmlHelper.SetPostToContext( ctx, post );
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

            set( "post.DeleteUrl", to( Delete, post.Id ) );

            set( "post.Author", post.Author );
            set( "post.Title", post.Title );
            set( "post.TitleHome", strUtil.EncodeTextarea( post.TitleHome ) );

            set( "post.Width", post.Width );
            set( "post.Height", post.Height );

            set( "Content", post.Content );

            set( "post.Created", post.Created );
            set( "post.Hits", post.Hits );
            set( "post.OrderId", post.OrderId );

            set( "post.RedirectUrl", post.RedirectUrl );
            set( "post.MetaKeywords", post.MetaKeywords );
            set( "post.MetaDescription", post.MetaDescription );


            set( "post.Summary", post.Summary );
            set( "post.SourceLink", post.SourceLink );
            set( "post.Style", post.Style );

            set( "post.ImgLink", post.GetImgOriginal() );
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

            wojilu.Drawing.Img.DeleteImgAndThumb( post.GetImgOriginal() );
            echoAjaxOk();
        }


        [HttpPost, DbTransaction]
        public void Update( int postId ) {
            ContentPost post = postService.GetById( postId, ctx.owner.Id );
            if (post == null) {
                echoError( lang( "exDataNotFound" ) );
                return;
            }

            String sectionIds = ctx.PostIdList( "postSection" );
            if (strUtil.IsNullOrEmpty( sectionIds )) {
                echoError( "请选择区块" );
                return;
            }

            ContentValidator.SetPostValue( post, ctx );
            ContentValidator.ValidateTitleBody( post, ctx );
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

        public void List( int sectionId ) {

            set( "addUrl", to( Add, 0 ) );
            set( "OperationUrl", to( SaveAdmin ) );
            set( "lnkTrans", to( Trans ) );

            ContentApp app = ctx.app.obj as ContentApp;
            set( "app.Name", ctx.app.Name );
            set( "app.Link", to( new ContentController().Index ) );
            set( "searchKey", "" );

            set( "tagAction", to( SaveTag ) );

            DataPage<ContentPost> posts;
            if (sectionId <= 0) {
                posts = postService.GetByApp( ctx.app.Id, 50 );
            }
            else {
                posts = postService.GetPageBySection( sectionId, 50 );
            }

            bool isTrash = false;
            bindAdminList( posts, isTrash );

            bindCategories( app );

            target( Search );
        }

        public void Trans() {

            target( TransSave );

            String ids = ctx.GetIdList( "ids" );
            set( "ids", ids );

            List<ContentApp> apps = ContentApp.find( "OwnerType=:otype order by Id" ).set( "otype", typeof( Site ).FullName ).list();
            IBlock block = getBlock( "apps" );
            List<Dictionary<String, String>> xlist = new List<Dictionary<String, String>>();
            foreach (ContentApp app in apps) {
                block.Set( "app.Id", app.Id );

                IMemberApp ma = appService.GetByApp( app );
                if (ma == null) continue;

                block.Set( "app.Name", ma.Name );
                List<ContentSection> sections = ContentSection.find( "AppId=" + app.Id + " order by Id" ).list();
                if (sections.Count == 0) continue;

                Dictionary<String, String> obj = new Dictionary<String, String>();
                obj["Id"] = app.Id.ToString();
                obj["Name"] = ma.Name;
                xlist.Add( obj );

                block.Set( "dataTarget", Html.CheckBoxList( sections, "dataTarget", "Title", "Id", null ) );
                block.Next();
            }

            bindList( "xlist", "x", xlist );
        }

        public void TransSave() {

            String postIds = ctx.PostIdList( "srcIds" );
            String targetSectionIds = ctx.PostIdList( "dataTarget" );

            if (strUtil.IsNullOrEmpty( postIds )) {
                echoError( "postIds is empty" );
                return;
            }

            if (strUtil.IsNullOrEmpty( targetSectionIds )) {
                echoError( "targetSectionIds" );
                return;
            }

            postService.Trans( postIds, targetSectionIds );

            echoToParentPart( lang( "opok" ) );
        }

        private void bindCategories( ContentApp app ) {
            List<ContentSection> sections = sectionService.GetInputSectionsByApp( app.Id );
            sections.ForEach( x => x.data["SectionLink"] = to( List, x.Id ) );
            bindList( "cats", "x", sections );
        }

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
            HtmlHelper.SetPostToContext( ctx, post );
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
                redirect( List, 0 );
                return;
            }

            String cmd = ctx.Post( "action" );

            if ("delete" == cmd) {
                postService.DeleteBatch( ids );
            }
            else if ("deletetrue" == cmd) {
                postService.DeleteTrueBatch( ids );
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

                IPageAdminSection sectionController = BinderUtils.GetPageSectionAdmin( post, ctx, "AdminSectionShow" );

                String typeIcon = BinderUtils.getTypeIcon( sectionController, post );
                String pickIcon = BinderUtils.getPickedIcon( post );
                String attIcon = post.Attachments > 0 ? BinderUtils.iconAttachment : "";

                block.Set( "post.ImgIcon", typeIcon );
                block.Set( "post.PickIcon", pickIcon );
                block.Set( "post.AttachmentIcon", attIcon );

                block.Set( "post.Title", strUtil.SubString( post.GetTitle(), 50 ) );
                block.Set( "post.TitleCss", post.Style );
                block.Set( "post.TitleFull", post.Title );

                block.Set( "post.SectionName", post.SectionName );

                block.Set( "post.Url", post.SourceLink );
                block.Set( "post.Link", alink.ToAppData( post ) );
                block.Set( "post.PubDate", post.Created );

                if (post.Creator != null) {
                    block.Set( "post.Submitter", string.Format( "<a href=\"{0}\" target=\"_blank\">{1}</a>", toUser( post.Creator ), post.Creator.Name ) );
                }
                else {
                    block.Set( "post.Submitter", "无" );
                }

                block.Bind( "post", post );

                String lnkEdit = isTrash ? "#" : sectionController.GetEditLink( post.Id );

                String lnkDelete = to( Delete, post.Id );
                if (isTrash) lnkDelete = to( DeleteSys, post.Id );


                block.Set( "post.HtmlUrl", clink.toAppData( post ) );


                block.Set( "post.EditUrl", lnkEdit );
                block.Set( "post.DeleteUrl", lnkDelete );

                block.Set( "post.RestoreUrl", to( Restore, post.Id ) );
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
            HtmlHelper.SetPostToContext( ctx, post );
        }

        [HttpPut, DbTransaction]
        public void Restore( int id ) {

            postService.Restore( id );
            ContentPost post = postService.GetById( id, ctx.owner.Id );

            echoRedirectPart( lang( "opok" ) );
        }

        [HttpDelete, DbTransaction]
        public void DeleteSys( int postId ) {
            postService.DeleteSys( postId );


            echoRedirectPart( lang( "opok" ) );
        }




    }

}
