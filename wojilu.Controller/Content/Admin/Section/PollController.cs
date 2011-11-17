/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Common.AppBase.Interface;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;
using wojilu.Web.Controller.Poll.Utils;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Content.Admin.Section {

    [App( typeof( ContentApp ) )]
    public partial class PollController : ControllerBase, IPageSection {

        public IContentPostService postService { get; set; }
        public ContentPollService pollService { get; set; }

        public PollController() {
            postService = new ContentPostService();
            pollService = new ContentPollService();
        }

        public List<IPageSettingLink> GetSettingLink( int sectionId ) {

            List<IPageSettingLink> links = new List<IPageSettingLink>();

            PageSettingLink lnk = new PageSettingLink();
            lnk.Name = lang( "editSetting" );
            lnk.Url = to( new SectionSettingController().Edit, sectionId );
            links.Add( lnk );

            return links;
        }

        public void SectionShow( int sectionId ) {
        }

        public void AdminSectionShow( int sectionId ) {

            ContentPoll c = pollService.GetRecentPoll( ctx.app.Id, sectionId );
            bindPollSection( sectionId, c );
        }

        public void List( int sectionId ) {

            DataPage<ContentPost> list = postService.GetPageBySection( sectionId );
            bindPostList( list );
        }


        public void Add( int sectionId ) {
            target( Create, sectionId );
            set( "optionCount", 5 );
            editor( "Question", "", "80px" );
        }

        [HttpDelete, DbTransaction]
        public void Delete( int id ) {
            ContentPost post = postService.GetById( id, ctx.owner.Id );
            if (post != null) {
                postService.Delete( post );
            }

            echoRedirect( lang( "opok" ) );
        }

        [HttpPost, DbTransaction]
        public void Create( int sectionId ) {

            ContentPoll poll = new PollValidator<ContentPoll>().Validate( ctx );
            if (errors.HasErrors) {
                run( Add, sectionId );
                return;
            }

            Result result = pollService.CreatePoll( sectionId, poll );
            if (result.HasErrors) {
                echo( result.ErrorsHtml );
                return;
            }

            ContentPost post = postService.GetById( poll.TopicId, ctx.owner.Id );

            echoToParentPart( lang( "opok" ) );
        }

    }
}
