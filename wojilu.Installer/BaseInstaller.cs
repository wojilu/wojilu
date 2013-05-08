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

namespace wojilu.Web.Controller {

    public class BaseInstaller {

        private static readonly ILog logger = LogManager.GetLogger( typeof( BaseInstaller ) );


        protected IMenuService menuService = new SiteMenuService();

        protected void AddMenuToHome( MvcContext ctx, String url, String name ) {

            IMenu menu = new SiteMenu();
            menu.Name = name;
            menu.Url = "default"; // 设为首页
            menu.RawUrl = UrlConverter.clearUrl( url, ctx, typeof( Site ).FullName, Site.Instance.Url );

            User creator = ctx.viewer.obj as User;
            menuService.Insert( menu, creator, Site.Instance );
        }




    }
}
