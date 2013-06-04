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
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;
using wojilu.Web.Controller.Content.Caching;
using wojilu.Web.Controller.Content.Utils;
using wojilu.Web.Controller.Content.Htmls;

namespace wojilu.Web.Controller.Content.Admin.Section {


    [App( typeof( ContentApp ) )]
    public partial class VideoController : ControllerBase, IPageAdminSection {

        public IContentPostService postService { get; set; }
        public IContentSectionService sectionService { get; set; }

        public VideoController() {
            postService = new ContentPostService();
            sectionService = new ContentSectionService();
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
            return to( Edit, postId );
        }

        public String GetSectionIcon( int sectionId ) {
            return BinderUtils.iconVideo;
        }

        public void AdminSectionShow( int sectionId ) {
            List<ContentPost> posts = GetSectionPosts( sectionId );
            bindSectionShow( sectionId, posts );
        }

        public List<ContentPost> GetSectionPosts( int sectionId ) {
            ContentSection s = sectionService.GetById( sectionId, ctx.app.Id );
            return postService.GetBySection( sectionId, s.ListCount );
        }

        public void AdminList( int sectionId ) {
            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );
            DataPage<ContentPost> posts = postService.GetPageBySectionAndCategory( section.Id, ctx.GetInt( "categoryId" ) );
            bindAdminList( sectionId, section, posts );
        }


        public void Add( int sectionId ) {
            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );
            target( to( Create, sectionId ) + "?categoryId=" + ctx.GetInt( "categoryId" ) );
            bindAddInfo( section );

            set( "videoServiceUrl", to( new wojilu.Web.Controller.Open.VideoController().PlayUrl ) );
        }


        [HttpPost, DbTransaction]
        public void Create( int sectionId ) {
            ContentPost post = ContentValidator.SetValueBySection( sectionService.GetById( sectionId, ctx.app.Id ), ctx );
            ContentValidator.ValidateVideo( post, ctx );
            if (ctx.HasErrors) {
                run( Add, sectionId );
            }
            else {

                postService.Insert( post, ctx.Post( "TagList" ) );

                echoToParentPart( lang( "opok" ) );
                HtmlHelper.SetPostToContext( ctx, post );

            }
        }

        public void Edit( int postId ) {
            ContentPost post = postService.GetById( postId, ctx.owner.Id );
            if (post == null) {
                echoToParentPart( lang( "exDataNotFound" ) );
                return;
            }

            target( to( Update, postId ) + "?categoryId=" + ctx.GetInt( "categoryId" ) );
            bindEditInfo( postId, post );
        }

        [HttpPost, DbTransaction]
        public void Update( int postId ) {
            ContentPost post = postService.GetById( postId, ctx.owner.Id );
            if (post == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            ContentValidator.SetPostValue( post, ctx );
            ContentValidator.ValidateVideo( post, ctx );
            if (ctx.HasErrors) {
                run( Edit, postId );
            }
            else {
                postService.Update( post, ctx.Post( "TagList" ) );

                echoToParentPart( lang( "opok" ) );
                HtmlHelper.SetPostToContext( ctx, post );
            }
        }

        [HttpDelete, DbTransaction]
        public void Delete( int postId ) {
            ContentPost post = postService.GetById( postId, ctx.owner.Id );
            if (post == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            postService.Delete( post );
            echoRedirect( lang( "opok" ) );
            HtmlHelper.SetPostToContext( ctx, post );
        }

    }
}

