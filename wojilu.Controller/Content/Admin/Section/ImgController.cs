/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.Utils;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;
using wojilu.Apps.Content.Enum;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;
using wojilu.Web.Controller.Content.Caching;
using wojilu.Web.Controller.Content.Utils;
using wojilu.Web.Controller.Content.Htmls;

namespace wojilu.Web.Controller.Content.Admin.Section {

    [App( typeof( ContentApp ) )]
    public partial class ImgController : ControllerBase, IPageAdminSection {

        public virtual IContentPostService postService { get; set; }
        public virtual IContentSectionService sectionService { get; set; }
        public virtual IContentImgService imgService { get; set; }

        public ImgController() {
            postService = new ContentPostService();
            sectionService = new ContentSectionService();
            imgService = new ContentImgService();
        }

        public virtual List<IPageSettingLink> GetSettingLink( long sectionId ) {
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

        public virtual String GetEditLink( long postId ) {
            return to( AddImgList, postId );
        }

        public virtual String GetSectionIcon( long sectionId ) {
            return BinderUtils.iconPic;
        }

        public virtual void AdminSectionShow( long sectionId ) {
            List<ContentPost> posts = GetSectionPosts( sectionId );
            bindSectionShow( sectionId, posts );
        }

        public virtual List<ContentPost> GetSectionPosts( long sectionId ) {
            ContentSection s = sectionService.GetById( sectionId, ctx.app.Id );
            return postService.GetBySection( sectionId, s.ListCount );
        }

        public virtual void AdminList( long sectionId ) {
            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );
            DataPage<ContentPost> posts = postService.GetPageBySectionAndCategory( section.Id, ctx.GetLong( "categoryId" ) );

            bindAdminList( sectionId, section, posts );
        }

        //--------------------------------------------------------------------------------------------------------------------------

        public virtual void AddListInfo( long sectionId ) {
            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );
            target( to( CreateListInfo, sectionId ) + "?categoryId=" + ctx.GetLong( "categoryId" ) );
            set( "section.Name", section.Title );
            set( "App.ImagesPath", sys.Path.Img );
        }

        [HttpPost, DbTransaction]
        public virtual void CreateListInfo( long sectionId ) {
            ContentPost post = ContentValidator.SetValueBySection( sectionService.GetById( sectionId, ctx.app.Id ), ctx );
            if (strUtil.IsNullOrEmpty( post.Title )) {
                errors.Add( lang( "exTitle" ) );
                run( AddListInfo, sectionId );
            }
            else {
                post.CategoryId = PostCategory.Img;
                post.TypeName = typeof( ContentImg ).FullName;
                post.HasImgList = 1;

                postService.Insert( post, null );

                redirect( AddImgList, post.Id );
                HtmlHelper.SetPostToContext( ctx, post );
            }
        }

        //-------------------------------------------------------------

        public virtual void AddImgList( long postId ) {

            ContentPost post = postService.GetById( postId, ctx.owner.Id );
            if (post == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            target( CreateImgList, postId );
            List<ContentImg> imgList = imgService.GetImgList( postId );

            bindAddList( postId, post, imgList );
            bindLinkList();
        }

        private void bindLinkList() {
            int lnkCounts = 3;
            IBlock lnkBlock = getBlock( "linkList" );
            for (int i = 1; i < (lnkCounts + 1); i++) {
                lnkBlock.Set( "photoIndex", i );
                lnkBlock.Next();
            }
        }


        [HttpPost, DbTransaction]
        public virtual void CreateImgList( long postId ) {

            ContentPost post = postService.GetById( postId, ctx.owner.Id );
            if (post == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            String dataType = ctx.Post( "dataType" );
            if (dataType == "upload") {
                saveUploadPic( postId, post );
            }
            else if (dataType == "link") {
                saveLinkPic( post );
            }
        }

        private void saveLinkPic( ContentPost post ) {

            int lnkCounts = 3;
            Boolean hasPic = false;
            for (int i = 1; i < (lnkCounts + 1); i++) {

                String picUrl = ctx.Post( "Pic"+i );
                String picDesc = ctx.Post( "Desc" + i );

                if (strUtil.IsNullOrEmpty( picUrl )) continue;

                saveLinkPicSingle( picUrl, picDesc, post, i );

                hasPic = true;

            }

            if (hasPic == false) {
                echoError( "请填写图片网址" );
            }
            else {
                redirect( AddImgList, post.Id );
            }

        }

        private void saveLinkPicSingle( string picUrl, string picDesc, ContentPost post, int i ) {

            ContentImg img = new ContentImg();
            img.Post = post;
            img.ImgUrl = picUrl;
            img.Description = picDesc;
            imgService.CreateImg( img );
            if ((i == 1) && post.HasImg() == false) {
                post.ImgLink = img.ImgUrl;
                imgService.UpdateImgLogo( post );
            }
        }

        private void saveUploadPic( long postId, ContentPost post ) {

            if (ctx.GetFiles().Count <= 0) {
                errors.Add( alang( "plsUpImg" ) );
                run( AddImgList, postId );
                return;
            }

            for (int i = 0; i < ctx.GetFiles().Count; i++) {
                Result result = Uploader.SaveImg( ctx.GetFiles()[i] );
                if (result.HasErrors) {
                    errors.Join( result );
                }
                else {
                    ContentImg img = new ContentImg();
                    img.Post = post;
                    img.ImgUrl = result.Info.ToString();
                    img.Description = ctx.Post( "Text" + (i + 1) );
                    imgService.CreateImg( img );
                    if ((i == 0) && post.HasImg() == false) {
                        post.ImgLink = img.ImgUrl;
                        imgService.UpdateImgLogo( post );
                    }
                }
            }

            if (errors.Errors.Count >= ctx.GetFiles().Count) {
                errors.Errors.Clear();
                errors.Add( alang( "plsUpImg" ) );
                run( AddImgList, postId );
            }
            else {
                redirect( AddImgList, postId );
            }
        }


        [HttpPut, DbTransaction]
        public virtual void SetLogo( long postId ) {

            ContentPost post = postService.GetById( postId, ctx.owner.Id );
            if (post == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            long imgId = ctx.GetLong( "imgId" );

            ContentImg img = imgService.GetImgById( imgId );
            if (img == null) {
                echoRedirect( alang( "exImgFound" ) );
                return;
            }

            post.ImgLink = img.ImgUrl;
            imgService.UpdateImgLogo( post );

            echoRedirect( lang( "opok" ) );
        }

        //--------------------------------------------------------------------------------------------------------------------------

        public virtual void EditListInfo( long postId ) {

            view( "EditListInfo" );
            ContentPost post = postService.GetById( postId, ctx.owner.Id );
            if (post == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            target( UpdateListInfo, postId );
            bindListEdit( postId, post );
        }


        [HttpPost, DbTransaction]
        public virtual void UpdateListInfo( long postId ) {
            ContentPost post = postService.GetById( postId, ctx.owner.Id );
            if (post == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            post.Title = ctx.Post( "Title" );
            post.TitleHome = ctx.Post( "TitleHome" );
            post.Content = ctx.PostHtml( "Content" );
            post.OrderId = ctx.PostInt( "OrderId" );
            post.CommentCondition = cvt.ToInt( ctx.Post( "IsCloseComment" ) );
            post.HasImgList = 1;

            post.MetaKeywords = ctx.Post( "MetaKeywords" );
            post.MetaDescription = strUtil.SubString( ctx.Post( "MetaDescription" ), 250 );

            if (strUtil.IsNullOrEmpty( post.Title )) {
                errors.Add( lang( "exTitle" ) );
                run( EditListInfo, postId );
            }
            else {
                postService.Update( post, null );

                echoRedirect( lang( "opok" ), to( EditListInfo, post.Id ) );
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------

        [HttpDelete, DbTransaction]
        public virtual void Delete( long postId ) {
            ContentPost post = postService.GetById( postId, ctx.owner.Id );
            if (post == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            postService.Delete( post ); // 删除回收站
            echoRedirect( lang( "opok" ) );
            HtmlHelper.SetPostToContext( ctx, post );
        }

        [HttpDelete, DbTransaction]
        public virtual void DeleteImg( long postId ) {

            ContentPost post = postService.GetById( postId, ctx.owner.Id );
            if (post == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            long imgId = ctx.GetLong( "imgId" );
            ContentImg img = imgService.GetImgById( imgId );
            if (img == null) {
                echoRedirect( alang( "exImgFound" ) );
                return;
            }

            imgService.DeleteImgOne( img );

            echoRedirect( lang( "opok" ) );
            HtmlHelper.SetPostToContext( ctx, post );
        }


    }
}

