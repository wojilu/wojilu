/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Polls.Domain;

namespace wojilu.Apps.Poll.Views {

    public class OptionResult {

        public int ImgWidth { get; set; }
        public int Votes { get; set; }
        public String Percent { get; set; }

        public String VotesAndPercent { get; set; }

        public OptionResult() {
        }

        public OptionResult( PollBase poll, int optionCount, int optionIndex ) {

            string[] arrGuestResults = getResults( poll.AnonymousResult, optionCount );
            string[] arrMemberResults = getResults( poll.MemberResult, optionCount );

            // 100%时候的宽度
            int imgWidth = 200;

            int currentItemVotes = cvt.ToInt( arrGuestResults[optionIndex] ) + cvt.ToInt( arrMemberResults[optionIndex] );

            double percent = ((double)currentItemVotes) / ((double)poll.GetTotalVotes());

            percent = Math.Round( percent, 2 );
            imgWidth = (int)(imgWidth * percent);

            if (imgWidth == 0) imgWidth = 1;

            this.ImgWidth = imgWidth;
            this.Votes = currentItemVotes;
            this.Percent = Convert.ToString( (double)(percent * 100.0) );
            this.VotesAndPercent = Votes + "(" + Percent + "%)";
        }


        private static string[] getResults( String voteResult, int optionLength ) {

            if (strUtil.HasText( voteResult ))
                return voteResult.Split( '/' );


            string[] arrGuestResults = new string[optionLength];
            for (int i = 0; i < optionLength; i++) {
                arrGuestResults[i] = "0";
            }
            return arrGuestResults;
        }

    }

}
