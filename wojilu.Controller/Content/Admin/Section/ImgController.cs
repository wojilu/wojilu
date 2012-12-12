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

namespace wojilu.Web.Controller.Content.Admin.Section {

    [App( typeof( ContentApp ) )]
    public partial class ImgController : ControllerBase, IPageAdminSection {

        public IContentPostService postService { get; set; }
        public IContentSectionService sectionService { get; set; }
        public IContentImgService imgService { get; set; }

        public ImgController() {
            postService = new ContentPostService();
            sectionService = new ContentSectionService();
            imgService = new ContentImgService();
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

        public String GetEditLink( int postId ) {
            return to( AddImgList, postId );
        }

        public String GetSectionIcon( int sectionId ) {
            return BinderUtils.iconPic;
        }

        public void AdminSectionShow( int sectionId ) {
            List<ContentPost> posts = postService.GetBySection( sectionId );
            bindSectionShow( sectionId, posts );
        }

        public void AdminList( int sectionId ) {
            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );
            DataPage<ContentPost> posts = postService.GetPageBySectionAndCategory( section.Id, ctx.GetInt( "categoryId" ) );

            bindAdminList( sectionId, section, posts );
        }

        //--------------------------------------------------------------------------------------------------------------------------

        public void AddImgList( int postId ) {

            ContentPost post = postService.GetById( postId, ctx.owner.Id );
            if (post == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            target( CreateImgList, postId  );
            List<ContentImg> imgList = imgService.GetImgList( postId );

            bindAddList( postId, post, imgList );
        }


        public void AddListInfo( int sectionId ) {
            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );
            target( to( CreateListInfo, sectionId ) + "?categoryId=" + ctx.GetInt( "categoryId" ) );
            set( "section.Name", section.Title );
            set( "App.ImagesPath", sys.Path.Img );

            editor( "Content", "", "190px" );
        }
        
        [HttpPost, DbTransaction]
        public void CreateListInfo( int sectionId ) {
            ContentPost post = ContentValidator.SetValueBySection( sectionService.GetById( sectionId, ctx.app.Id ), ctx );
            if (strUtil.IsNullOrEmpty( post.Title )) {
                errors.Add( lang( "exTitle" ) );
                run(AddListInfo, sectionId );
            }
            else {
                post.CategoryId = PostCategory.Img;
                post.TypeName = typeof( ContentImg ).FullName;
                post.HasImgList = 1;

                postService.Insert( post, null );

                redirect( AddImgList, post.Id );
                HtmlHelper.SetCurrentPost( ctx, post );
            }
        }

        //-------------------------------------------------------------

        [HttpPost, DbTransaction]
        public void CreateImgList( int postId ) {

            ContentPost post = postService.GetById( postId, ctx.owner.Id );
            if (post == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            if (ctx.GetFiles().Count <= 0) {
                errors.Add( alang( "plsUpImg" ) );
                run(AddImgList, postId );
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
                    if ((i == 0) && post.HasImg()==false) {
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
                HtmlHelper.SetCurrentPost( ctx, post );
            }
        }


        [HttpPut, DbTransaction]
        public void SetLogo( int imgId ) {

            ContentImg img = imgService.GetImgById( imgId );
            if (img == null) {
                echoRedirect( alang( "exImgFound" ) );
                return;
            }

            ContentPost post = img.Post;
            post.ImgLink = img.ImgUrl;
            imgService.UpdateImgLogo( post );

            echoRedirect( lang( "opok" ) );
            HtmlHelper.SetCurrentPost( ctx, post );
        }

        //--------------------------------------------------------------------------------------------------------------------------
        
        public void EditListInfo( int postId ) {

            view( "EditListInfo" );
            ContentPost post = postService.GetById( postId, ctx.owner.Id );
            if (post == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }
            
            target( UpdateListInfo, postId  );
            bindListEdit( postId, post );
        }


        [HttpPost, DbTransaction]
        public void UpdateListInfo( int postId ) {
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
                run(EditListInfo, postId );
            }
            else {
                postService.Update( post, null );

                echoRedirect( lang( "opok" ), to( EditListInfo, post.Id ) );
                HtmlHelper.SetCurrentPost( ctx, post );
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------

        [HttpDelete, DbTransaction]
        public void Delete( int postId ) {
            ContentPost post = postService.GetById( postId, ctx.owner.Id );
            if (post == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            postService.Delete( post ); // …æ≥˝ªÿ ’’æ
            echoRedirect( lang( "opok" ) );
            HtmlHelper.SetCurrentPost( ctx, post );
        }

        [HttpDelete, DbTransaction]
        public void DeleteImg( int imgId ) {
            ContentImg img = imgService.GetImgById( imgId );
            if (img == null) {
                echoRedirect( alang( "exImgFound" ) );
                return;
            }

            imgService.DeleteImgOne( img );
            
            echoRedirect( lang( "opok" ) );
            HtmlHelper.SetCurrentPost( ctx, img.Post );

        }


    }
}

