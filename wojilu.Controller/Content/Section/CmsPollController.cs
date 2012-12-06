/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Common.Polls.Service;

using wojilu.Members.Users.Domain;

using wojilu.Web.Controller.Poll.Utils;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Service;
using wojilu.Apps.Content.Interface;
using wojilu.Web.Controller.Content.Caching;


namespace wojilu.Web.Controller.Content.Section {

    [App( typeof( ContentApp ) )]
    public partial class CmsPollController : ControllerBase {

        public virtual ContentPollService pollService { get; set; }
        public virtual IContentPostService topicService { get; set; }

        public CmsPollController() {
            topicService = new ContentPostService();
            pollService = new ContentPollService();
        }

        public void Detail() {

            ContentPoll p = ctx.GetItem( "poll" ) as ContentPoll;

            if (p == null) {
                set( "pollForm", "" );
                set( "pollResult", "" );
                return;
            }

            set( "topicId", p.TopicId );

            String hideCss = "display:none;";
            if (p.CheckHasVote( ctx.viewer.Id ) || (p.IsClosed() && p.IsVisible == 0)) {
                set( "formStyle", hideCss );
                set( "resultStype", "" );
            }
            else {
                set( "formStyle", "" );
                set( "resultStype", hideCss );
            }

            load( "pollForm", pollForm );
            load( "pollResult", pollResult );
        }

        public void pollForm() {

            ContentPoll p = ctx.GetItem( "poll" ) as ContentPoll;

            set( "topicId", p.TopicId );

            set( "getResultHtmlLink", to( GetPollResultHtml, p.Id ) );

            set( "poll.Id", p.Id );
            set( "poll.Title", p.Title );
            set( "poll.Question", p.Question );
            set( "poll.Voters", p.VoteCount );
            set( "poll.VoteLink", to( Vote, p.Id ) );

            IBlock opblock = getBlock( "options" );
            for (int i = 0; i < p.OptionList.Length; i++) {

                opblock.Set( "op.SelectControl", getControl( p, i, p.OptionList[i] ) );
                opblock.Set( "op.Id", (i + 1) );
                opblock.Next();
            }

            IBlock cmdBlock = getBlock( "cmdVote" ); // 投票命令
            IBlock tipBlock = getBlock( "plsVote" ); // 请先登录

            if (p.IsClosed()) {
            }
            else if (ctx.viewer.IsLogin) {
                cmdBlock.Next();
            }
            else {
                tipBlock.Next();
            }

            bindViewLink( p );

            set( "poll.ExpiryInfo", p.GetRealExpiryDate() );
        }

        private void bindViewLink( ContentPoll p ) {
            IBlock lnkView = getBlock( "lnkView" );
            IBlock lblView = getBlock( "lblView" );
            if (p.IsVisible == 0 || isAuthor( p ) || ctx.viewer.IsAdministrator() || ctx.viewer.IsOwnerAdministrator( ctx.owner.obj )) {
                lnkView.Set( "topicId", p.TopicId );
                lnkView.Next();
            }
            else {
                lblView.Next();
            }
        }

        private bool isAuthor( ContentPoll p ) {
            return ctx.viewer.Id == p.Creator.Id;
        }

        public void GetPollResultHtml( int pollId ) {
            ContentPoll p = pollService.GetById( pollId );
            ctx.SetItem( "poll", p );

            echo( loadHtml( pollResult ) );
        }

        public void pollResult() {

            ContentPoll p = ctx.GetItem( "poll" ) as ContentPoll;

            set( "topicId", p.TopicId );

            set( "poll.Title", p.Title );
            set( "poll.Question", p.Question );
            set( "poll.Voters", p.VoteCount );

            set( "lnkVoter", getVoterLink( p ) );

            int colorCount = 6;
            int iColor = 1;

            IBlock opblock = getBlock( "options" );
            for (int i = 0; i < p.OptionList.Length; i++) {

                PollHelper or = new PollHelper( p, p.OptionList.Length, i );

                opblock.Set( "op.Text", p.OptionList[i] );
                opblock.Set( "op.Id", (i + 1) );
                opblock.Set( "op.ImgWidth", or.ImgWidth * 1 );
                opblock.Set( "op.Percent", or.VotesAndPercent );
                opblock.Set( "op.ColorIndex", iColor );
                opblock.Next();

                iColor++;
                if (iColor > 6) iColor = 1;
            }

            set( "poll.ExpiryInfo", p.GetRealExpiryDate() );

            IBlock btnVote = getBlock( "btnVote" );
            IBlock lblVoted = getBlock( "lblVoted" );
            if (p.CheckHasVote( ctx.viewer.Id )) {
                lblVoted.Next();
            }
            else if (p.IsClosed() == false) {
                btnVote.Next();
            }
        }

        private String getVoterLink( ContentPoll p ) {

            if (p.IsOpenVoter == 0) {
                String url = to( Voter, p.Id );
                return string.Format( "<a href=\"{0}\" class=\"frmBox left10 right10\" target=\"_blank\">投票人数: {1}</a>", url, p.VoteCount );
            }

            return "<span class=\"poll-voter-count\">投票人数: " + p.VoteCount + "</span>";
        }

        private String getControl( ContentPoll poll, int optionIndex, String optionText ) {
            Html html = new Html();
            if (poll.Type == 1) {
                html.CheckBox( "pollOption", Convert.ToString( (optionIndex + 1) ), optionText );
            }
            else {
                html.Radio( "pollOption", Convert.ToString( (optionIndex + 1) ), optionText );
            }
            return html.ToString();
        }

        //----------------------------------------------------------------------------

        [Login]
        public void Add( int sectionId ) {

            set( "ActionLink", to( Create, sectionId ) );
            bindAddForm( sectionId );
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

        [Login, HttpPost, DbTransaction]
        public void Vote( int id ) {

            ContentPoll poll = pollService.GetById( id );
            if (poll == null) {
                echoError( lang( "exPollItemNotFound" ) );
                return;
            }

            if (poll.CheckHasVote( ctx.viewer.Id )) {
                echoError( lang( "exVoted" ) );
                return;
            }

            String choice = ctx.Post( "pollOption" );
            if (strUtil.IsNullOrEmpty( choice )) {
                echoError( lang( "pollSelectRequire" ) );
                return;
            }

            ContentPollResult pollResult = new ContentPollResult();
            pollResult.User = (User)ctx.viewer.obj;
            pollResult.PollId = poll.Id;
            pollResult.Answer = choice;
            pollResult.Ip = ctx.Ip;

            String lnkPost = to( new Forum.TopicController().Show, poll.TopicId );
            pollService.CreateResult( pollResult, lnkPost );

            echoAjaxOk();
        }

        public void Voter( int id ) {

            ContentPoll poll = pollService.GetById( id );

            if (poll == null) {
                echoText( lang( "exPollItemNotFound" ) );
                return;
            }

            DataPage<ContentPollResult> voterList = pollService.GetVoterList( id );

            bindVoter( poll, voterList );
        }

        //----------------------------------------------------------------------------------------
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

        private void bindVoter( ContentPoll poll, DataPage<ContentPollResult> voterList ) {

            IBlock block = getBlock( "list" );
            foreach (ContentPollResult result in voterList.Results) {
                block.Set( "user.Name", result.User.Name );
                block.Set( "user.Choice", result.Answer );
                block.Set( "user.Created", result.Created );
                block.Next();
            }

            set( "page", voterList.PageBar );
        }


        //------------------------------------------------------------------

        public void List( int sectionId ) {

            DataPage<ContentPost> list = topicService.GetPageBySection( sectionId );

            // TODO
            //bindPostList( list );
        }


        [Login, HttpDelete, DbTransaction]
        public void Delete( int id ) {
            ContentPost post = topicService.GetById( id, ctx.owner.Id );
            if (post != null) {
                topicService.Delete( post );
            }

            echoRedirect( lang( "opok" ) );
            HtmlHelper.SetCurrentPost( ctx, post );
        }

    }
}

