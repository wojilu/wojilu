using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Web.Controller.Common {

    public class ExpirationChecker {

        public static Boolean IsExpried( String key, ICacheHelper cacher ) {

            if (key.StartsWith( "/Forum" )) {
                ForumExpirationChecker fcc = new ForumExpirationChecker( cacher );
                return fcc.IsExpried( key );
            }
            else if (key.StartsWith( "/Content" )) {
                ContentExpirationChecker fcc = new ContentExpirationChecker( cacher );
                return fcc.IsExpried( key );
            }

            else {

                return false;
            }

        }

    }

}
