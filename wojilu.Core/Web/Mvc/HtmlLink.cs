using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Web.Mvc {

    public interface IStaticApp {
        String GetStaticDir();
    }

    public class HtmlLink {

        public static String ToApp( IApp app ) {

            return string.Format( "/{0}/default.html", GetStaticDir( app ) );
        }

        public static String GetStaticDir( IApp app ) {

            if (app == null) throw new ArgumentNullException( "app" );

            IStaticApp entity = app as IStaticApp;
            if (entity == null) return getDefaultStaticDir( app );

            String staticDir = entity.GetStaticDir();
            if (strUtil.IsNullOrEmpty( staticDir )) return getDefaultStaticDir( app );

            return staticDir;

        }

        private static string getDefaultStaticDir( IApp app ) {
            return "cms" + app.Id;
        }


        public static String ToAppData( IAppData data ) {

            DateTime n = data.Created;

            return string.Format( "/html/{0}/{1}/{2}/{3}.html", n.Year, n.Month, n.Day, data.Id );

        }
    }

}
