/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Members.Users.Domain;
using wojilu.Common.Polls.Domain;

namespace wojilu.Apps.Poll.Views {


    public class PollViewFactory {

        private PollBase poll;
        private User user;
        private String postLink;
        private String lnkVoter;

        public PollViewFactory( User viewer, PollBase poll, String postLink, String lnkVoter ) {
            this.user = viewer;
            this.poll = poll;
            this.postLink = postLink;
            this.lnkVoter = lnkVoter;
        }

        private Boolean checkHasVote() {
            return poll.CheckHasVote( user.Id );
        }

        private Boolean isModerator() {
            return false;
        }

        public virtual PollView GetPollView() {

            PollView view = new PollView();
            view.Question = poll.Question;
            view.Submit = getSubmit( poll );
            view.PollMsg = getMsg( poll );
            view.hasVote = checkHasVote();
            view.isVisible = poll.IsVisible;
            if (strUtil.IsNullOrEmpty( poll.Answer )) {
                view.Items = new List<PollViewItem>();
                return view;
            }
            view.Items = GetPollViewItems( poll );
            return view;

        }

        private String getSubmit( PollBase poll ) {


            String voteLink = postLink;

            Boolean hasVote = checkHasVote();

            Html html = new Html();
            if (!user.IsRegisterUser) {
                html.Code( "<input type=button disabled=disabled value=\"" + lang.get( "pollLoginRequire" ) + "\" />" );
            }
            else if (hasVote) {
                html.Code( "<input type=button disabled=disabled value=\"" + lang.get( "pollVoted" ) + "\" />" );
            }
            else {

                html.Code( "<input id=\"btnPoll\" class=\"btn\" type=button value=\" " + lang.get( "pollVote" ) + " \" />" );

            }
            if (!(hasVote || (poll.IsVisible != 1))) {
                html.Code( " <span class='red font12'>[" + lang.get( "pollViewTip" ) + "]</span>" );
            }

            String js = getAjaxJs( voteLink, lang.get( "pollSelectRequire" ), lang.get( "pollVoted" ) );
            html.Code( js );

            return html.ToString();
        }

        private String getAjaxJs( String voteLink, String plsSelect, String clickedValue ) {
            String js = @"
<script>

_run( function() {

    $('#btnPoll').click( function() {

        var btnPoll = $(this);

        var voteLink = '" + voteLink + @"'.toAjax();

        var selectedControl = $( 'input[name=pollOption][@checked]' );
        var pValue = selectedControl.serialize();
        if( wojilu.str.hasText( pValue )==false ) {
            alert( '" + plsSelect + @"' );
            return false;
        }

        btnPoll.attr( 'disabled', 'disabled' );
        btnPoll.val( '" + clickedValue + @"' );
        var url = voteLink+'&'+pValue;

        $.post( url, function(data) {
            btnPoll.attr( 'disabled', '' );
            //$('#pollContainer').html( data );
            logger.info( data );

            var opr = eval( '('+data+')' );
            $('#voterCount').text( opr.total );
            for( var i=0;i<opr.results.length;i++ ) {

                var opone = opr.results[i];
                var opimg = $('img', $('#onePollResult'+i));
                opimg.width( opone.ImgWidth );

                var opnum = $('span', $('#onePollResult'+i)); 
                var opInfo = opone.Votes+'('+opone.Percent+'%)';
                opnum.text( opInfo );
            }

        });

    });

});

</script>

";
            return js;
        }

        private String getMsg( PollBase poll ) {

            String expiryDateInfo = PollUtil.GetRealExpiryDate( poll );

            //ForumBoard fb = ctx.getItems( "forumBoard" ) as ForumBoard;
            //Boolean isModerator = fb == null ? true : moderatorService.IsModerator( fb, (User)ctx.viewer.obj );

            Boolean isOpen = false;
            if ((poll.IsOpenVoter == 0) || ((poll.IsOpenVoter == 1) && isModerator()))
                isOpen = true;


            String voteInfo = string.Empty;
            String vstr = lang.get( "pollAllVoter" );
            if (!isOpen) {
                voteInfo = vstr.Replace( "{0}", "<span id='voterCount'>" + poll.VoteCount + "</span>" );
            }
            else {
                String lnkView = string.Format( "<a href=\"{1}\" id=\"pollVoterList\" class=\"frmBox\"><span id=\"voterCount\">{0}</span></a>", poll.VoteCount, lnkVoter );
                voteInfo = vstr.Replace( "{0}", lnkView );

            }

            return (expiryDateInfo + voteInfo);
        }





        public static List<PollViewItem> GetPollViewItems( PollBase poll ) {
            string[] optionList = poll.OptionList;
            List<PollViewItem> list = new List<PollViewItem>();
            for (int i = 0; i < optionList.Length; i++) {
                PollViewItem item = new PollViewItem();
                item.SetPoll( poll );
                item.SetOptionString( optionList[i] );
                item.SetOptionIndex( i );
                item.SetOptionCount( optionList.Length );
                item.Init();
                list.Add( item );
            }
            return list;
        }

        public virtual String GetJsonResult() {

            int total = poll.VoteCount;

            //{total:6, results:[{ "ImgWidth":1, "Votes":0, "Percent":"0" }, { "ImgWidth":166, "Votes":5, "Percent":"83" }, { "ImgWidth":34, "Votes":1, "Percent":"17" }, { "ImgWidth":1, "Votes":0, "Percent":"0" }, { "ImgWidth":1, "Votes":0, "Percent":"0" }]};

            String json = "{total:";
            json += poll.VoteCount;
            json += ", results:[";

            string[] optionList = poll.OptionList;
            for (int i = 0; i < optionList.Length; i++) {


                OptionResult opr = new OptionResult( poll, optionList.Length, i );
                json += Json.ToString( opr );
                if (i != optionList.Length - 1) json += ", ";

            }
            json += "]}";

            return json;
        }


    }

}
