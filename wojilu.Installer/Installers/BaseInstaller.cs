/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Menus.Interface;
using wojilu.Members.Sites.Service;
using wojilu.Web.Context;
using wojilu.Web.Url;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Drawing;
using wojilu.Common.MemberApp.Interface;
using wojilu.Web.Controller.Security;

namespace wojilu.Web.Controller {

    public class BaseInstaller {

        private static readonly ILog logger = LogManager.GetLogger( typeof( BaseInstaller ) );


        protected IMenuService menuService = new SiteMenuService();

        protected void AddMenu( MvcContext ctx, String name, String url, String fUrl ) {

            IMenu menu = new SiteMenu();
            menu.Name = name;
            menu.Url = fUrl;
            menu.RawUrl = UrlConverter.clearUrl( url, ctx, typeof( Site ).FullName, Site.Instance.Url );

            User creator = ctx.viewer.obj as User;
            menuService.Insert( menu, creator, Site.Instance );
        }

        protected String lnkFull( MvcContext ctx, String link ) {
            if (link.StartsWith( "http" )) return link;
            return strUtil.Join( ctx.url.SiteAndAppPath, link );
        }

        protected void initAppPermission( IMemberApp app ) {
            AppRole.InitSiteFront( app.Id );
            AppAdminRole.InitSiteAdmin( app.Id );
        }

    }
}
