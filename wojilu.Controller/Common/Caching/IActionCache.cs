using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;

namespace wojilu.Web.Controller.Common.Caching {

    public interface IActionCache {

        String GetCacheKey( String actionName );

        Dictionary<String, String> GetRelatedActions();

        void Update( MvcContext ctx );

    }

}
