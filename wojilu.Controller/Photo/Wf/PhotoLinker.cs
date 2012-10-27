using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Photo.Wf {

    public class PhotoLinker {

        public static String getUserLink( User u ) {
            return string.Format( "/photo/{0}{1}", u.Url, MvcConfig.Instance.UrlExt );
        }

        public static String getCategoryLink( String userFurl, int albumId ) {
            return string.Format( "/photo/category/{0}/{1}{2}", userFurl, albumId, MvcConfig.Instance.UrlExt );
        }

        public static String getUserLikeLink( User u ) {
            return string.Format( "/photo/like/{0}{1}", u.Url, MvcConfig.Instance.UrlExt );
        }

        public static String getUserAlbumLink( User u ) {
            return string.Format( "/photo/album/{0}{1}", u.Url, MvcConfig.Instance.UrlExt );
        }

        public static String getUserFollowerLink( User u ) {
            return string.Format( "/photo/follower/{0}{1}", u.Url, MvcConfig.Instance.UrlExt );
        }

    }

}
