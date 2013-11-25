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
using wojilu.Apps.Content.Enum;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;
using wojilu.Web.Controller.Content.Caching;
using wojilu.Web.Controller.Content.Utils;
using wojilu.Web.Controller.Content.Htmls;

namespace wojilu.Web.Controller.Content.Admin.Section {


    [App( typeof( ContentApp ) )]
    public partial class TextController : ControllerBase, IPageAdminSection {

        public IContentPostService postService { get; set; }
        public IContentSectionService sectionService { get; set; }

        public TextController() {
            postService = new ContentPostService();
            sectionService = new ContentSectionService();
        }

        public List<IPageSettingLink> GetSettingLink( long sectionId ) {
            List<IPageSettingLink> links = new List<IPageSettingLink>();

            PageSettingLink lnk = new PageSettingLink();
            lnk.Name = lang( "editSetting" );
            lnk.Url = to( new SectionSettingController().Edit, sectionId );
            links.Add( lnk );

            PageSettingLink lnktmp = new PageSettingLink();
            lnktmp.Name = alang( "editTemplate" );
            lnktmp.Url = to( new TemplateCustomController().Edit, sectionId );
            links.Add( lnktmp );

            return links;
        }

        public void AdminSectionShow( long sectionId ) {
            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );
            ContentPost textPost = postService.GetFirstPost( ctx.app.Id, sectionId );

            bindSectionShow( sectionId, textPost );
        }

        public List<ContentPost> GetSectionPosts( long sectionId ) {
            ContentPost textPost = postService.GetFirstPost( ctx.app.Id, sectionId );
            List<ContentPost> list = new List<ContentPost>();
            list.Add( textPost );
            return list;
        }

        public void AdminList( long sectionId ) {
            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );
            DataPage<ContentPost> posts = postService.GetPageBySectionAndCategory( section.Id, ctx.GetLong( "categoryId" ) );
            bindAdminList( section, posts );
        }



        public String GetEditLink( long postId ) {
            return to( Edit, postId );
        }

        public String GetSectionIcon( long sectionId ) {
            return BinderUtils.iconText;
        }

        public void Add( long sectionId ) {
            view( "Add" );
            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );
            target( Create, sectionId );
            set( "section.Title", section.Title );
        }

        [HttpPost, DbTransaction]
        public void Create( long sectionId ) {

            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );

            ContentPost post = ContentValidator.SetValueBySection( sectionService.GetById( sectionId, ctx.app.Id ), ctx );
            if (strUtil.IsNullOrEmpty( post.Title )) {
                post.Title = section.Title + " " + DateTime.Now.ToShortDateString();
            }

            if (strUtil.IsNullOrEmpty( post.Content )) {
                errors.Add( lang( "exContent" ) );
                run( Add, sectionId );
            }
            else {
                post.CategoryId = PostCategory.Notice;

                postService.Insert( post, null );
               
                echoToParentPart( lang( "opok" ) );
                HtmlHelper.SetPostToContext( ctx, post );
            }
        }

        [HttpDelete, DbTransaction]
        public void Delete( long postId ) {
            ContentPost post = postService.GetById( postId, ctx.owner.Id );
            if (post == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            postService.Delete( post );
            echoRedirectPart( lang( "opok" ) );
            HtmlHelper.SetPostToContext( ctx, post );
        }

        public void Edit( long postId ) {
            view( "Edit" );
            ContentPost post = postService.GetById( postId, ctx.owner.Id );
            if (post == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            target( Update, postId );
            set( "section.Title", post.SectionName );
            set( "Content", post.Content );
        }

        [HttpPost, DbTransaction]
        public void Update( long postId ) {
            ContentPost post = postService.GetById( postId, ctx.owner.Id );
            if (post == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            ContentValidator.SetPostValue( post, ctx );
            if (strUtil.IsNullOrEmpty( post.Content )) {
                errors.Add( lang( "exContent" ) );
                run( Edit, postId );
            }
            else {
                postService.Update( post, null );

                echoToParentPart( lang( "opok" ) );
                HtmlHelper.SetPostToContext( ctx, post );
            }
        }


    }
}

