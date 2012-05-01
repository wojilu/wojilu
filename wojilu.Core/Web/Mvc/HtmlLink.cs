using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Web.Mvc {

    public class HtmlLink {

        public static String ToApp( IApp app ) {

            IEntity entity = app as IEntity;
            Object url = entity.get( "FriendlyUrl" );
            if (url == null) return "content"+app.Id;

            return url.ToString();
        }


        public static String ToAppData( IAppData data ) {

            DateTime n = data.Created;

            return string.Format( "/html/{0}/{1}/{2}/{3}.html", n.Year, n.Month, n.Day, data.Id );

        }
    }

}
