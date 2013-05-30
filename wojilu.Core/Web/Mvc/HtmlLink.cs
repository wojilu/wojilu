using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Web.Mvc {

    public interface IStaticApp {
        String GetStaticPath();
    }

    public class HtmlLink {

        public static String ToApp( IApp app ) {

            IStaticApp entity = app as IStaticApp;
            String path = entity.GetStaticPath();

            if (isDir( path )) {

                String dir = GetStaticDir( app );
                if (dir == "/") {
                    return string.Format( "/default.html" );
                }
                else {
                    return string.Format( "/{0}/default.html", dir );
                }
            }
            else {

                if (path.StartsWith( "/" ) == false) {
                    return "/" + path;
                }

                return path;
            }
        }

        private static bool isDir( string staticPath ) {
            if (strUtil.IsNullOrEmpty( staticPath )) return true;

            if (staticPath.EndsWith( ".htm" ) || staticPath.EndsWith( ".html" )) {
                return false;
            }

            return true;
        }

        public static String GetStaticDir( IApp app ) {

            if (app == null) throw new ArgumentNullException( "app" );

            IStaticApp entity = app as IStaticApp;
            if (entity == null) return getDefaultStaticDir( app );

            String staticDir = getDirFromPath( entity.GetStaticPath() );
            if (strUtil.IsNullOrEmpty( staticDir )) return getDefaultStaticDir( app );

            return staticDir;
        }

        private static string getDirFromPath( string staticPath ) {

            // /cms88/default.html
            if (strUtil.IsNullOrEmpty( staticPath )) return null;

            // 1) /cms88/default.html
            staticPath = staticPath.Trim().TrimStart( '/' ).TrimEnd( '/' );

            // 2) cms88/default.html
            String[] arr = staticPath.Split( '/' );
            if (arr.Length == 1) {

                if (staticPath.EndsWith( ".htm" ) || staticPath.EndsWith( ".html" )) {
                    // somepage.html，返回根目录
                    return "/";
                }
                else {
                    // someDir
                    return staticPath;
                }
            }
            else {
                return strUtil.TrimEnd( staticPath, arr[arr.Length - 1] ).TrimEnd( '/' );
            }
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
