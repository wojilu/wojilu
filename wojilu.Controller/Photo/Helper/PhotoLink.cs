using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;
using wojilu.Web.Mvc;
using wojilu.Apps.Photo.Domain;
using wojilu.Web.Context;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Controller.Photo {

    [MvcLink]
    public class PhotoLink {

        private static String ext {
            get { return MvcConfig.Instance.UrlExt; }
        }

        public static String ToHome() {
            return string.Format( "/photo/home{0}", ext );
        }

        public static String ToPost( int postId ) {
            return string.Format( "/photo/post/{0}{1}", postId, ext );
        }

        public static String ToHot() {
            return string.Format( "/photo/hot{0}", ext );
        }

        public static String ToPick() {
            return string.Format( "/photo/pick{0}", ext );
        }

        public static String ToCategory( int categoryId ) {
            return string.Format( "/photo/category/{0}{1}", categoryId, ext );
        }

        //-----------------------------------------------------------

        public static String ToUser( User u ) {
            return string.Format( "/photo/{0}{1}", u.Url, ext );
        }

        public static String ToLike( User u ) {
            return string.Format( "/photo/like/{0}{1}", u.Url, ext );
        }

        public static String ToAlbumOne( String userFriendlyUrl, int albumId ) {
            return string.Format( "/photo/album/{0}/{1}{2}", userFriendlyUrl, albumId, ext );
        }

        public static String ToAlbumList( User u ) {
            return string.Format( "/photo/albums/{0}{1}", u.Url, ext );
        }

        public static String ToFollower( User u ) {
            return string.Format( "/photo/follower/{0}{1}", u.Url, ext );
        }

        public static String ToAdminPost( User u ) {

            PhotoApp app = PhotoApp.find( "OwnerId=" + u.Id ).first();
            if (app == null) return "";

            return Link.To( u, new Photo.Admin.MyController().My, app.Id );
        }

        public static String ToAdminAlbum( User u ) {
            PhotoApp app = PhotoApp.find( "OwnerId=" + u.Id ).first();
            if (app == null) return "";

            return Link.To( u, new Photo.Admin.AlbumController().List, app.Id );
        }

    }

}
