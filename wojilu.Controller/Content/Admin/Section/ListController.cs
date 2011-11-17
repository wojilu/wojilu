/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.Utils;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;

using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Content.Admin.Section {


    [App( typeof( ContentApp ) )]
    public partial class ListController : ControllerBase, IPageSection {

        public IContentPostService postService { get; set; }
        public IContentSectionService sectionService { get; set; }
        public IAttachmentService attachService { get; set; }

        public ListController() {
            postService = new ContentPostService();
            sectionService = new ContentSectionService();
            attachService = new AttachmentService();
        }

        public List<IPageSettingLink> GetSettingLink( int sectionId ) {
            List<IPageSettingLink> links = new List<IPageSettingLink>();

            PageSettingLink lnk = new PageSettingLink();
            lnk.Name = lang( "editSetting" );
            lnk.Url = to( new SectionSettingController().EditCount, sectionId );
            links.Add( lnk );

            PageSettingLink lnktmp = new PageSettingLink();
            lnktmp.Name = alang( "editTemplate" );
            lnktmp.Url = to( new TemplateCustomController().Edit, sectionId );
            links.Add( lnktmp );


            return links;
        }

        public void SectionShow( int sectionId ) {
        }

        public void AdminSectionShow( int sectionId ) {
            IList posts = postService.GetBySection( ctx.app.Id, sectionId );
            bindSectionShow( sectionId, posts );
        }


        public void AdminList( int sectionId ) {
            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );
            DataPage<ContentPost> posts = postService.GetBySectionAndCategory( section.Id, ctx.GetInt( "categoryId" ) );

            bindAdminList( section, posts );
        }

        //public void Add( int sectionId ) {

        //    ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );
        //    target( to( Create, sectionId ) + "?categoryId=" + ctx.GetInt( "categoryId" ) );

        //    set( "width", ctx.GetInt( "width" ) );
        //    set( "height", ctx.GetInt( "height" ) );

        //    bindAddInfo( sectionId, section );
        //}

        //public void AddImg( int sectionId ) {
        //    this.Add( sectionId );
        //}

        //[HttpPost, DbTransaction]
        //public void Create( int sectionId ) {
        //    ContentPost post = ContentValidator.Validate( sectionService.GetById( sectionId, ctx.app.Id ), ctx );
        //    ContentValidator.ValidateArticle( post, ctx );
        //    if (ctx.HasErrors) {
        //        echoError();
        //    }
        //    else {
        //        postService.Insert( post, ctx.Post( "TagList" ) );
        //        saveUploadedAttachments( post );

        //        echoToParentPart( lang( "opok" ) );
        //    }
        //}

        //private void saveUploadedAttachments( ContentPost post ) {
        //    String ids = ctx.PostIdList( "uploadFileIds" );
        //    int[] arrIds = cvt.ToIntArray( ids );
        //    attachService.CreateByTemp( ids, post );
        //}

        //public void Edit( int postId ) {

        //    ContentPost post = postService.GetById( postId, ctx.owner.Id );
        //    if (post == null) {
        //        echo( lang( "exDataNotFound" ) );
        //        return;
        //    }

        //    target( to( Update, postId ) + "?categoryId=" + ctx.GetInt( "categoryId" ) );

        //    bindEditInfo( post );

        //    List<ContentSection> sectionList = sectionService.GetInputSectionsByApp( ctx.app.Id );
        //    String sectionIds = sectionService.GetSectionIdsByPost( postId );

        //    checkboxList( "postSection", sectionList, "Title=Id", 0 );
        //    set( "sectionIds", sectionIds );



        //}

        //public void EditImg( int postId ) {
        //    this.Edit( postId );
        //}

        //[HttpPost, DbTransaction]
        //public void Update( int postId ) {
        //    ContentPost post = postService.GetById( postId, ctx.owner.Id );
        //    if (post == null) {
        //        echo( lang( "exDataNotFound" ) );
        //        return;
        //    }

        //    String sectionIds = ctx.PostIdList( "postSection" );

        //    ContentValidator.ValidateEdit( post, ctx );
        //    ContentValidator.ValidateArticle( post, ctx );
        //    if (errors.HasErrors) {
        //        echoError();
        //    }
        //    else {

        //        if (ctx.PostIsCheck( "saveContentPic" ) == 1) {
        //            post.Content = wojilu.Net.PageLoader.ProcessPic( post.Content, null );
        //        }

        //        postService.Update( post, sectionIds, ctx.Post( "TagList" ) );

        //        echoToParentPart( lang( "opok" ) );
        //    }
        //}

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

        ////--------------------------------------- 图片上传处理 -------------------------------------------------------------

        public void Upload( int sectionId ) {
            target( SaveUpload, sectionId );

            set( "width", ctx.GetInt( "width" ) );
            set( "height", ctx.GetInt( "height" ) );

        }

        public void SaveUpload( int sectionId ) {

            if (ctx.HasUploadFiles == false) {
                echoRedirect( alang( "exUploadEmpty" ), to( Upload, sectionId ) );
                return;
            }

            HttpFile file = ctx.GetFileSingle();
            Result result = Uploader.SaveImg( file );
            if (result.HasErrors)
                errors.Join( result );
            else {
                String imgUrl = sys.Path.GetPhotoOriginal( result.Info.ToString() );
                set( "imgUrl", imgUrl );
                set( "width", ctx.PostInt( "width" ) );
                set( "height", ctx.PostInt( "height" ) );
            }

        }

        //public void DeleteUpload( int sectionId ) {
        //    String imgUrl = ctx.Post( "imgUrl" );
        //    wojilu.Drawing.Img.DeleteImgAndThumb( imgUrl );
        //    echoAjaxOk();
        //}



    }
}

