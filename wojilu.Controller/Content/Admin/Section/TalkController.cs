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
    public partial class TalkController : ControllerBase, IPageAdminSection {

        public IContentPostService postService { get; set; }
        public IContentSectionService sectionService { get; set; }

        public TalkController() {
            postService = new ContentPostService();
            sectionService = new ContentSectionService();
        }

        public List<IPageSettingLink> GetSettingLink( int sectionId ) {
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

        public String GetEditLink( int postId ) {
            return to( Edit, postId );
        }

        public String GetSectionIcon( int sectionId ) {
            return BinderUtils.iconTalk;
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
            DataPage<ContentPost> posts = postService.GetPageBySectionAndCategory( section.Id, 0 );
            bindAdminList( section, posts );
        }

        public void Add( int sectionId ) {
            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );
            target( Create, sectionId );
            bindAddInfo( section );
        }

        [HttpPost, DbTransaction]
        public void Create( int sectionId ) {
            ContentPost post = ContentValidator.SetValueBySection( sectionService.GetById( sectionId, ctx.app.Id ), ctx );
            ContentValidator.ValidateTalk( post, ctx );
            if (errors.HasErrors) {
                run( Add, sectionId );
            }
            else {
                post.CategoryId = PostCategory.Talk;
                post.Title = strUtil.SubString( post.Content, 20 );

                postService.Insert( post, ctx.Post( "TagList" ) );

                echoToParentPart( lang( "opok" ) );
                HtmlHelper.SetPostToContext( ctx, post );
            }
        }

        public void Edit( int postId ) {
            view( "Edit" );
            ContentPost post = postService.GetById( postId, ctx.owner.Id );
            if (post == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            target( Update, postId );
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
            ContentValidator.ValidateTalk( post, ctx );
            if (errors.HasErrors) {
                Edit( postId );
            }
            else {
                post.Title = strUtil.SubString( post.Content, 20 );
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

            echoToParentPart( lang( "opok" ) );
            HtmlHelper.SetPostToContext( ctx, post );
        }


    }
}

