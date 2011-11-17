/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Common.Msg.Service {

    public class MessageEncoder {

        public static String Encode( String msgBody ) {

            return msgBody.Replace( "\n", "<br />" );
        }

        public static String Decode( String msgBody ) {
            return msgBody.Replace( "<br />", "\n" );
        }

    }

}
