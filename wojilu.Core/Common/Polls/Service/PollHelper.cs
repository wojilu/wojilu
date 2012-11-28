using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Polls.Domain;

namespace wojilu.Common.Polls.Service {

    public class PollHelper {

        public int ImgWidth { get; set; }
        public int Votes { get; set; }
        public String Percent { get; set; }

        public String VotesAndPercent { get; set; }


        public PollHelper( PollBase poll, int optionCount, int optionIndex ) {

            string[] arrGuestResults = getResults( poll.AnonymousResult, optionCount );
            string[] arrMemberResults = getResults( poll.MemberResult, optionCount );

            // 100%时候的宽度
            int imgWidth = 200;

            int currentItemVotes = cvt.ToInt( arrGuestResults[optionIndex] ) + cvt.ToInt( arrMemberResults[optionIndex] );

            double percent = poll.GetMemberVotes() == 0 ? 0 : ((double)currentItemVotes) / ((double)poll.GetMemberVotes());

            percent = Math.Round( percent, 2 );
            imgWidth = (int)(imgWidth * percent);

            this.ImgWidth = imgWidth;
            this.Votes = currentItemVotes;
            this.Percent = Convert.ToString( (double)(percent * 100.0) );
            this.VotesAndPercent = Votes + "<span class=\"poll-option-percent\">(" + Percent + "%)</span>";
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

        //----------------------------------------------------------------------

        public static String GetRealExpiryDate( PollBase poll ) {

            DateTime expiryDate = poll.ExpiryDate;
            DateTime created = poll.Created;

            if (cvt.IsDayEqual( expiryDate, created )) return "";

            TimeSpan span = expiryDate.Date.Subtract( DateTime.Now.Date );
            int leavingDays = span.Days;

            if (leavingDays <= 0) return lang.get( "pollEnd" );
            if (leavingDays == 1) return lang.get( "pollEndToday" );
            if (leavingDays == 2) return lang.get( "pollEndTomorrow" );

            return string.Format( lang.get( "pollEndDays" ), expiryDate.ToShortDateString(), leavingDays );
        }

        public static Boolean IsClosed( PollBase poll ) {
            DateTime expiryDate = poll.ExpiryDate;
            DateTime created = poll.Created;

            if (cvt.IsDayEqual( expiryDate, created )) return false;

            TimeSpan span = expiryDate.Date.Subtract( DateTime.Now.Date );
            int leavingDays = span.Days + 1;

            if (leavingDays <= 0) return true;
            return false;
        }


    }
}
