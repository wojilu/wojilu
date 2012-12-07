/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Common.Polls.Service;

using wojilu.Members.Users.Domain;

using wojilu.Web.Controller.Poll.Utils;
using wojilu.Web.Controller.Content.Caching;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Service;
using wojilu.Apps.Content.Interface;


namespace wojilu.Web.Controller.Content.Admin.Section {

    [App( typeof( ContentApp ) )]
    public class CmsPollController : ControllerBase {

        public virtual ContentPollService pollService { get; set; }
        public virtual IContentPostService topicService { get; set; }

        public CmsPollController() {
            topicService = new ContentPostService();
            pollService = new ContentPollService();
        }

        //------------------ admin ----------------------------------------------------------

        [Login]
        public void Add( int sectionId ) {

            set( "ActionLink", to( Create, sectionId ) );
            bindAddForm( sectionId );
        }

        private void bindAddForm( int sectionId ) {

            set( "optionCount", 5 );
            set( "appId", ctx.app.Id );
            editor( "Question", "", "80px" );

            if ((ctx.Post( "PollType" ) == null) || (ctx.Post( "PollType" ) == "0")) {
                set( "singleCheck", " checked=\"checked\"" );
                set( "multiCheck", "" );
            }
            else if (ctx.Post( "PollType" ) == "1") {
                set( "singleCheck", "" );
                set( "multiCheck", " checked=\"checked\"" );
            }
        }

        [Login, HttpPost, DbTransaction]
        public void Create( int sectionId ) {

            ContentPoll poll = new PollValidator<ContentPoll>().Validate( ctx );
            if (errors.HasErrors) {
                echoError();
                return;
            }

            pollService.CreatePoll( sectionId, poll );


            echoToParentPart( lang( "opok" ) );
        }

        [Login]
        public void AdminList( int sectionId ) {

            DataPage<ContentPost> list = topicService.GetPageBySection( sectionId, 10 );
            list.Results.ForEach( x => {
                x.data.delete = to( Delete, x.Id );
                x.data.show = alink.ToAppData( x );
            } );

            bindList( "list", "x", list.Results );
            set( "page", list.PageBar );
        }

        [Login, HttpDelete, DbTransaction]
        public void Delete( int id ) {
            ContentPost post = topicService.GetById( id, ctx.owner.Id );
            if (post != null) {
                topicService.Delete( post );
            }

            // TODO
            // delete poll

            echoRedirect( lang( "opok" ) );
            HtmlHelper.SetCurrentPost( ctx, post );
        }

    }
}

