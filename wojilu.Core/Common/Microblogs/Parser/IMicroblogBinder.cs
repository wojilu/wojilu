using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Common.Microblogs.Parser {

    public interface IMicroblogBinder {
        String GetLink( String userName );
        String GetTagLink( String tag );
        String GetUrlLink( String url );
    }

}
