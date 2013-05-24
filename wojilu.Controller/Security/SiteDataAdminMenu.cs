/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Web.Controller.Security {

    public class SiteDataAdminMenu {

        public int Id { get; set; }
        public String Name { get; set; }
        public String Url { get; set; }
        public String Logo { get; set; }

        public SiteDataAdminMenu( int id, String logo, String name, aAction action, String rootNamespace ) {

            String url = SecurityUtils.getPath( action, rootNamespace );
            init( id, logo, name, url );
        }

        public SiteDataAdminMenu( aAction action, String rootNamespace ) {

            String url = SecurityUtils.getPath( action, rootNamespace );
            init( 0, "", "", url );
        }

        private void init( int id, String logo, String name, String url ) {
            this.Id = id;
            this.Name = name;
            this.Url = url;
            this.Logo = logo;
        }

        public Boolean CanShow( List<SiteAdminOperation> userActions ) {
            foreach (SiteAdminOperation action in userActions) {

                List<string> urls = action.GetUrlList();
                if (urls.Contains( this.Url )) return true;

            }
            return false;
        }

    }


}
