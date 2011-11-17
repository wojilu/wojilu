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

namespace wojilu.Web.Controller.Content.Admin.Section {


    [App( typeof( ContentApp ) )]
    public partial class VideoController : ControllerBase, IPageSection {

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

        public void SectionShow( int sectionId ) {
        }

        public void AdminSectionShow( int sectionId ) {
            List<ContentPost> posts = postService.GetBySection( ctx.app.Id, sectionId );
            bindSectionShow( sectionId, posts );
        }


        public void AdminList( int sectionId ) {
            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );
            DataPage<ContentPost> posts = postService.GetBySectionAndCategory( section.Id, ctx.GetInt( "categoryId" ) );
            bindAdminList( sectionId, section, posts );
        }


        public void Add( int sectionId ) {
            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );
            target( to( Create, sectionId ) + "?categoryId=" + ctx.GetInt( "categoryId" ) );
            bindAddInfo( section );

            set( "videoServiceUrl", to( new Common.Service.VideoController().PlayUrl ) );
        }


        [HttpPost, DbTransaction]
        public void Create( int sectionId ) {
            ContentPost post = ContentValidator.Validate( sectionService.GetById( sectionId, ctx.app.Id ), ctx );
            ContentValidator.ValidateVideo( post, ctx );
            if (ctx.HasErrors) {
                run( Add, sectionId );
            }
            else {
                
                postService.Insert( post, null );
                
                echoToParentPart( lang( "opok" ) );

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

            ContentValidator.ValidateEdit( post, ctx );
            ContentValidator.ValidateVideo( post, ctx );
            if (ctx.HasErrors) {
                run( Edit, postId );
            }
            else {
                postService.Update( post, null );

                echoToParentPart( lang( "opok" ) );
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

        }

    }
}

