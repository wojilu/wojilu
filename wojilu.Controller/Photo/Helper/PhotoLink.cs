using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;
using wojilu.Web.Mvc;
using wojilu.Apps.Photo.Domain;
using wojilu.Web.Context;

namespace wojilu.Web.Controller.Photo {

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

        public static String ToUser( User u ) {
            return string.Format( "/photo/{0}{1}", u.Url, ext );
        }

        public static String ToLike( User u ) {
            return string.Format( "/photo/like/{0}{1}", u.Url, ext );
        }

        public static String ToAlbumOne( String userFriendlyUrl, int albumId ) {
            return string.Format( "/photo/category/{0}/{1}{2}", userFriendlyUrl, albumId, ext );
        }

        public static String ToAlbumList( User u ) {
            return string.Format( "/photo/album/{0}{1}", u.Url, ext );
        }

        public static String ToFollower( User u ) {
            return string.Format( "/photo/follower/{0}{1}", u.Url, ext );
        }

        //public static String ToAdminPost( User u, MvcContext ctx ) {

        //    PhotoApp app = PhotoApp.find( "OwnerId="+ u.Id ).first();
        //    if (app == null) return;


        //}

        //public static String ToAdminAlbum( User u ) {
        //}


    }

}
