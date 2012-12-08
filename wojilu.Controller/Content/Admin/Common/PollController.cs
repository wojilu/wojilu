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
using wojilu.Apps.Content.Enum;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Content.Admin.Common {

    [App( typeof( ContentApp ) )]
    public class PollController : ControllerBase {

        public virtual ContentPollService pollService { get; set; }
        public virtual IContentPostService topicService { get; set; }
        public virtual IContentSectionService sectionService { get; set; }

        public PollController() {
            topicService = new ContentPostService();
            pollService = new ContentPollService();
            sectionService = new ContentSectionService();
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

            ContentPost post = ContentValidator.SetValue( ctx );
            ContentPoll poll = new PollValidator<ContentPoll>().Validate( ctx );
            if (errors.HasErrors) {
                echoError();
                return;
            }

            pollService.CreatePoll( sectionId, poll, post );


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

        public void Edit( int pollId ) {

            ContentPoll poll = pollService.GetById( pollId );
            if (poll == null) throw new NullReferenceException( "Edit Poll, ContentPoll" );

            ContentPost post = topicService.GetById( poll.TopicId, ctx.owner.Id );
            if (post == null) throw new NullReferenceException( "Edit Poll, ContentPost" );

            target( Update, post.Id );

            bindEditInfo( post );

            List<ContentSection> sectionList = sectionService.GetInputSectionsByApp( ctx.app.Id );
            String sectionIds = sectionService.GetSectionIdsByPost( post.Id );

            checkboxList( "postSection", sectionList, "Title=Id", 0 );
            set( "sectionIds", sectionIds );
        }

        [HttpPost, DbTransaction]
        public void Update( int postId ) {

            ContentPost post = topicService.GetById( postId, ctx.owner.Id );
            if (post == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            post.Title = strUtil.CutString( ctx.Post( "Title" ), 100 );
            post.Summary = ctx.Post( "Summary" );
            post.CommentCondition = cvt.ToInt( ctx.Post( "IsCloseComment" ) );
            post.Hits = ctx.PostInt( "Hits" );
            post.Created = ctx.PostTime( "Created" );

            post.MetaKeywords = strUtil.CutString( ctx.Post( "MetaKeywords" ), 250 );
            post.MetaDescription = strUtil.CutString( ctx.Post( "MetaDescription" ), 250 );

            post.Ip = ctx.Ip;

            String sectionIds = sectionService.GetSectionIdsByPost( post.Id );
            topicService.Update( post, sectionIds, ctx.Post( "TagList" ) );

            echoToParentPart( lang( "opok" ) );
            HtmlHelper.SetCurrentPost( ctx, post );
        }


        private void bindEditInfo( ContentPost post ) {

            set( "post.DeleteUrl", to( Delete, post.Id ) );

            set( "post.Author", post.Author );
            set( "post.Title", post.Title );
            set( "post.TitleHome", strUtil.EncodeTextarea( post.TitleHome ) );

            set( "post.Width", post.Width );
            set( "post.Height", post.Height );

            editor( "Content", post.Content, "250px" );

            set( "post.Created", post.Created );
            set( "post.Hits", post.Hits );
            set( "post.OrderId", post.OrderId );

            set( "post.RedirectUrl", post.RedirectUrl );
            set( "post.MetaKeywords", post.MetaKeywords );
            set( "post.MetaDescription", post.MetaDescription );


            set( "post.Summary", post.Summary );
            set( "post.SourceLink", post.SourceLink );
            set( "post.Style", post.Style );

            set( "post.TagList", post.Tag.TextString );
            String val = AccessStatusUtil.GetRadioList( post.AccessStatus );
            set( "post.AccessStatus", val );
            set( "post.IsCloseComment", Html.CheckBox( "IsCloseComment", lang( "closeComment" ), "1", cvt.ToBool( post.CommentCondition ) ) );

            radioList( "PickStatus", PickStatus.GetPickStatus(), post.PickStatus.ToString() );

        }

    }
}

