/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.Web.Mvc;
using wojilu.Members.Users.Domain;
using wojilu.Members.Sites.Service;
using wojilu.Members.Interface;
using wojilu.Common.Security;

namespace wojilu.Members.Sites.Domain {


    [Serializable]
    public class Site : IMember {

        public static Site Instance = getInstance();

        private static Site getInstance() {
            Site site = new Site();
            site.Id = 0;
            site.Name = lang.get( "site" );
            site.Url = "/";
            return site;
        }

        public Site() { }

        public int Id { get; set; }
        public String Name {
            get { return config.Instance.Site.SiteName; }
            set { }
        }
        public String Url { get; set; }
        public int TemplateId { get; set; }


        public IList Menus {
            get { return new ArrayList(); }
        }

        public IList GetRoles() {
            return new SiteRoleService().GetRoleAndRank();
        }

        public IRole GetAdminRole() {
            return SiteRole.Administrator;
        }

        public IRole GetUserRole( IMember user ) {
            return ((User)user).Role;
        }

        public int Status {
            get { return 0; }
            set { }
        }


        public DateTime Created {
            get { return DateTime.Now; }
            set {
            }
        }

        public String GetUrl() {
            return "site";
        }

    }
}

