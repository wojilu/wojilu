/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Polls.Domain;

namespace wojilu.Apps.Poll.Views {

    public class PollUtil {

        public static String GetRealExpiryDate( PollBase poll ) {

            DateTime expiryDate = poll.ExpiryDate;
            DateTime created = poll.Created;

            if (cvt.IsDayEqual( expiryDate, created )) return "";

            TimeSpan span = expiryDate.Date.Subtract( DateTime.Now.Date );
            int leavingDays = span.Days + 1;

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
