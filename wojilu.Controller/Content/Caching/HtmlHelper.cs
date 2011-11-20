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

        public static String CheckPostDir( ContentPost data ) {

            String dir = GetPostDir( data );

            if (Directory.Exists( dir )) return dir;
            Directory.CreateDirectory( dir );

            return dir;
        }

        public static String GetPostDir( ContentPost data ) {
            DateTime n = data.Created;
            return PathHelper.Map( string.Format( "/html/{0}/{1}/{2}/", n.Year, n.Month, n.Day ) );
        }

        public static String GetPostPath( ContentPost post ) {
            return Path.Combine( GetPostDir( post ), post.Id + ".html" );
        }

        //----------------------------------------------------------------------------

        public static String CheckListDir( ContentPost data ) {

            String dir = GetListDir( data );

            if (Directory.Exists( dir )) return dir;
            Directory.CreateDirectory( dir );

            return dir;
        }

        public static String GetListDir( ContentPost data ) {
            return PathHelper.Map( "/html/list/" );
        }

        public static String GetListPath( ContentPost post ) {
            return Path.Combine( GetListDir( post ), post.PageSection.Id + ".html" );
        }

        public static String GetListLink( int sectionId ) {
            return string.Format( "/html/list/{0}.html", sectionId );
        }


    }

}
