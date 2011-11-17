using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Web.Controller.Common {

    public class ContentExpirationChecker {


        private ICacheHelper cacher;

        public ContentExpirationChecker( ICacheHelper cacher ) {
            this.cacher = cacher;
        }



        public bool IsExpried( string key ) {
            return false;
        }
    }

}
