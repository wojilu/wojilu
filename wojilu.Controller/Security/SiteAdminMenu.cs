/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;

namespace wojilu.Web.Controller.Security {
    

    public class SiteAdminMenu {

        private static readonly ILog logger = LogManager.GetLogger( typeof( SiteAdminMenu ) );

        public int Id { get; set; }
        public String Name { get; set; }
        public String Url { get; set; }
        public String Tag { get; set; }
        public String Logo { get; set; }

        public SiteAdminMenu( int id, String logo, String name, aAction action, String rootNamespace ) {


            String url = SecurityUtils.getPath( action, rootNamespace );
            init( id, logo, name, url );
        }

        public SiteAdminMenu( int id, String logo, String name, String url, String tag ) {
            this.Tag = tag;
            init( id, logo, name, url );
        }

        private void init( int id, String logo, String name, String url ) {
            this.Id = id;
            this.Name = name;
            this.Url = url;
            this.Logo = logo;
        }

        //public Boolean IsApp() {
        //    return AppTag.Equals( this.Tag );
        //}

        public Boolean IsUserDataAdmin() {
            return UserDataAdminTag.Equals( this.Tag );
        }

        //public static readonly String AppTag = "apps";
        public static readonly String UserDataAdminTag = "userDataAdmin";


    }

}
