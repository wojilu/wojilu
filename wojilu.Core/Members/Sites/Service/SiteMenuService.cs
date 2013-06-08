/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;


using wojilu.Members.Interface;

using wojilu.Members.Users.Domain;
using wojilu.Members.Sites.Domain;
using wojilu.Common.Menus.Interface;
using wojilu.Common.MemberApp.Interface;
using wojilu.Data;
using wojilu.Web.Mvc.Routes;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Members.Sites.Service {

    public class SiteMenuService : IMenuService {

        public IMenu New() {
            return new SiteMenu();
        }

        public Type GetMemberType() {
            return typeof( Site );
        }

        public Result Delete( IMenu menu ) {

            if (hasSubMenus( menu )) {
                return new Result( lang.get( "deleteSubMenuFirst" ) );
            }


            ((SiteMenu)menu).delete();
            updateDefaultMenu( menu );
            return new Result();
        }

        private bool hasSubMenus( IMenu menu ) {

            List<IMenu> subMenus = GetByParent( menu );
            return subMenus.Count > 0;
        }

        public List<IMenu> GetByParent( IMenu m ) {
            List<IMenu> list = new List<IMenu>();
            List<SiteMenu> menus = cdb.findAll<SiteMenu>();
            foreach (SiteMenu menu in menus) {
                if (menu.ParentId == m.Id) list.Add( menu );
            }
            return list;
        }

        public IMenu FindById( int ownerId, int menuId ) {
            List<SiteMenu> menus = cdb.findAll<SiteMenu>();
            foreach (SiteMenu menu in menus) {
                if (menu.OwnerId == ownerId && menu.Id == menuId)
                    return menu;
            }
            return null;
        }

        public List<IMenu> GetRootList( IMember owner ) {

            List<IMenu> results = new List<IMenu>();
            List<SiteMenu> menus = cdb.findAll<SiteMenu>();
            foreach (SiteMenu menu in menus) {
                if (menu.OwnerId == owner.Id && menu.ParentId==0)
                    results.Add( menu );
            }
            results.Sort();
            return results;
        }

        public List<IMenu> GetList( IMember owner ) {

            List<IMenu> results = new List<IMenu>();
            List<SiteMenu> menus = cdb.findAll<SiteMenu>();
            foreach (SiteMenu menu in menus) {
                if (menu.OwnerId == owner.Id )
                    results.Add( menu );
            }
            results.Sort();
            return results;
        }

        public Result Insert( IMenu menu, User creator, IMember owner ) {

            menu.OwnerId = owner.Id;
            menu.Creator = creator;
            menu.Created = DateTime.Now;

            clearDefaultMenu( menu );

            updateRoute( menu );

            return Insert( menu );
        }

        private static void updateRoute( IMenu menu ) {
            if (strUtil.HasText( menu.Url ) && strUtil.EqualsIgnoreCase( "default", menu.Url ) == false) {
                RouteTable.UpdateFriendUrl( menu.Url );
            }
        }


        private Result Insert( IMenu menu ) {
            ((SiteMenu)menu).insert();
            return new Result();
        }

        public Result Update( IMenu menu ) {

            ((SiteMenu)menu).update();

            clearDefaultMenu( menu );

            if (strUtil.HasText( menu.Url ) && strUtil.EqualsIgnoreCase( "default", menu.Url ) == false) {
                RouteTable.UpdateFriendUrl( menu.Url );
            }

            return new Result();

        }

        private void clearDefaultMenu( IMenu menu ) {

            if ("default".Equals( menu.Url ) == false) return;

            List<SiteMenu> list = cdb.findAll<SiteMenu>();
            foreach (SiteMenu smenu in list) {
                if ((smenu.Id != menu.Id) && "default".Equals( smenu.Url )) {
                    smenu.Url = "";
                    smenu.update();
                    break;
                }
            }
        }

        //----------------------------------------------------------------------

        public IMenu FindByApp( String rawAppUrl ) {

            List<SiteMenu> menus = cdb.findAll<SiteMenu>();
            foreach (SiteMenu menu in menus) {
                if (strUtil.IsNullOrEmpty( menu.RawUrl )) continue;
                if (PathHelper.CompareUrlWithoutExt( menu.RawUrl, rawAppUrl )) return menu;
            }
            return null;

        }

        public IMenu AddMenuByApp( IMemberApp app, String name, String friendUrl, String rawAppUrl ) {

            Boolean isFirst = this.GetList(Site.Instance).Count == 0;

            IMenu menu = new SiteMenu();
            menu.OwnerId = app.OwnerId;
            menu.OwnerUrl = app.OwnerUrl;
            menu.OwnerType = app.OwnerType;
            menu.Creator = app.Creator;
            menu.Name = name;

            menu.Url = friendUrl;
            if (isFirst) menu.Url = "default";

            menu.RawUrl = rawAppUrl;
            menu.Created = DateTime.Now;

            Insert( menu );

            updateRoute( menu );

            return menu;
        }


        public void RemoveMenuByApp( IMemberApp app, String rawAppUrl ) {
            IMenu menu = FindByApp( rawAppUrl );
            if (menu != null) {
                ((SiteMenu)menu).delete();
                updateDefaultMenu( menu );

            }
        }

        public void UpdateMenuByApp( IMemberApp app, String rawAppUrl ) {
            IMenu menu = FindByApp( rawAppUrl );
            if (menu != null) {
                UpdateName( menu, app.Name );
            }
        }

        private void UpdateName( IMenu menu, String appName ) {
            menu.Name = appName;
            ((SiteMenu)menu).update();
        }

        private void updateDefaultMenu( IMenu menu ) {
            if (strUtil.EqualsIgnoreCase( menu.Url, "default" )) {
                List<IMenu> menus = this.GetList( Site.Instance );
                if (menus.Count > 0) {
                    IMenu defaultMenu = menus[0];
                    defaultMenu.Url = "default";

                    cdb.update( (SiteMenu)defaultMenu );

                }
            }
        }

    }

}
