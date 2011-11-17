/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Common.Polls.Domain;

namespace wojilu.Apps.Poll.Views {


    public class PollViewItem {

        private int optionCount;
        private int optionIndex;

        private PollBase poll;
        private String strOption;

        public String CheckBox { get; set; }
        public String ImgAndCountInfo { get; set; }
        public String OptionString { get; set; }
        public int OptionValue {
            get { return optionIndex + 1; }
        }

        public void Init() {
            CheckBox = getBox();
            OptionString = getOptionString();
            ImgAndCountInfo = GetImgAndCountInfo();
        }

        public void SetOptionCount( int optCount ) {
            optionCount = optCount;
        }

        public void SetOptionIndex( int optIndex ) {
            optionIndex = optIndex;
        }

        public void SetOptionString( String optString ) {
            strOption = optString;
        }

        public void SetPoll( PollBase mypoll ) {
            poll = mypoll;
        }


        private String getBox() {
            Html html = new Html();
            if (poll.Type == 1) {
                html.CheckBox( "pollOption", Convert.ToString( (optionIndex + 1) ), "" );
            }
            else {
                html.Radio( "pollOption", Convert.ToString( (optionIndex + 1) ), "" );
            }
            return html.ToString();
        }

        private String getOptionString() {
            return strOption;
        }

        private String GetImgAndCountInfo() {

            if (poll.VoteCount <= 0)
                return string.Empty;

            OptionResult opr = new OptionResult( poll, optionCount, optionIndex );

            StringBuilder builder = new StringBuilder();

            String imgV = strUtil.Join( sys.Path.Skin, "apps/forum/vote.gif" );
            builder.AppendFormat( "<img src='{0}' style='width:{1}px;height:9px;' />", imgV, opr.ImgWidth );


            builder.Append( "<span class='pollOptionVotes'>" );
            builder.Append( opr.Votes );
            builder.Append( "(" );
            builder.Append( opr.Percent );
            builder.Append( "%)</span>" );
            return builder.ToString();
        }


    }
}

