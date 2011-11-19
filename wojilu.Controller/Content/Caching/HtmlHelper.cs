using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Content.Domain;
using System.IO;
using wojilu.Web.Context;

namespace wojilu.Web.Controller.Content.Caching {

    public class HtmlHelper {

        public static void SetCurrentPost( MvcContext ctx, ContentPost post ) {
            ctx.SetItem( "_currentContentPost", post );
        }

        public static String CheckDir( ContentPost data ) {

            String dir = GetDir( data );

            if (Directory.Exists( dir )) return dir;
            Directory.CreateDirectory( dir );

            return dir;
        }

        public static String GetDir( ContentPost data ) {
            DateTime n = data.Created;
            return PathHelper.Map( string.Format( "/html/{0}/{1}/{2}/", n.Year, n.Month, n.Day ) );
        }

        public static String GetPath( ContentPost post ) {
            return Path.Combine( GetDir( post ), post.Id + ".html" );
        }

    }

}
