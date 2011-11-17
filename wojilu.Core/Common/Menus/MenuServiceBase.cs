/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;
using wojilu.Common.MemberApp;
using wojilu.Common.Menus.Interface;
using wojilu.Common.MemberApp.Interface;
using wojilu.Common.AppBase.Interface;
using wojilu.Data;
using wojilu.ORM;

namespace wojilu.Common.Menus {

    public abstract class MenuServiceBase : IMenuService {

        private static readonly ILog logger = LogManager.GetLogger( typeof( MenuServiceBase ) );

        public abstract IMenu New();
        public abstract Object getObj();
        public abstract Type GetMemberType();

        private Type t { get { return getObj().GetType(); } }

        public Result Delete( IMenu menu ) {

            if (hasSubMenus( menu )) {
                return new Result( lang.get( "deleteSubMenuFirst" ) );
            }

            db.delete( (IEntity)menu );
            updateDefaultMenu( menu );
            return new Result();
        }

        private bool hasSubMenus( IMenu menu ) {

            List<IMenu> subMenus = GetByParent( menu );
            return subMenus.Count > 0;
        }

        public List<IMenu> GetByParent( IMenu m ) {
            IList list = ndb.find( t, "OwnerId=" + m.OwnerId + " and ParentId="+m.Id+" order by OrderId desc, Id asc" ).list();
            return populateMenu( list );
        }

        private static List<IMenu> populateMenu( IList list ) {
            List<IMenu> results = new List<IMenu>();
            foreach (IMenu menu in list) results.Add( menu );
            return results;
        }

        public IMenu FindById( int ownerId, int menuId ) {
            return ndb.find( t, "OwnerId=" + ownerId + " and Id=" + menuId ).first() as IMenu;
        }

        public List<IMenu> GetRootList( IMember owner ) {
            if (owner.Id < 0) return new List<IMenu>();
            IList list = ndb.find( t, "OwnerId=" + owner.Id + " and ParentId=0 order by OrderId desc, Id asc" ).list();
            return populateMenu( list );
        }

        public List<IMenu> GetList( IMember owner ) {

            IList list = GetList( owner.Id );
            return populateMenu( list );
        }

        private IList GetList( int ownerId ) {
            if (ownerId < 0) return new ArrayList();
            return ndb.find( t, "OwnerId=" + ownerId + " order by OrderId desc, Id asc" ).list();
        }

        public Result Insert( IMenu menu, User creator, IMember owner ) {

            menu.OwnerId = owner.Id;
            menu.OwnerUrl = owner.Url;

            menu.Creator = creator;
            menu.Created = DateTime.Now;
            Result result = db.insert( menu );

            checkDefaultMenu( menu );
            return result;
        }

        public Result Update( IMenu menu ) {

            Result result = db.update( menu );

            checkDefaultMenu( menu );
            return result;
        }

        private void checkDefaultMenu( IMenu menu ) {

            logger.Info( "menuUrl=" + menu.Url );

            if ("default".Equals( menu.Url ) == false) return;

            String condition = string.Format( "OwnerId={0} and Id<>{1} and Url='default'", menu.OwnerId, menu.Id );
            IMenu dMenu = ndb.find( t, condition ).first() as IMenu;

            if (dMenu != null) {

                dMenu.Url = "";
                db.update( dMenu );
            }

        }

        //----------------------------------------------------------------------

        public IMenu FindByApp( String rawAppUrl ) {

            return ndb.find( t, "RawUrl=:url" )
                .set( "url", rawAppUrl )
                .first() as IMenu;
        }

        public IMenu AddMenuByApp( IMemberApp app, String name, String friendUrl, String rawAppUrl ) {

            Boolean isFirst = this.GetList( app.OwnerId ).Count == 0;

            IMenu menu = New();

            menu.OwnerId = app.OwnerId;
            menu.OwnerUrl = app.OwnerUrl;
            menu.OwnerType = app.OwnerType;
            menu.Creator = app.Creator;
            menu.Name = name;

            menu.Url = friendUrl;
            if (isFirst) menu.Url = "default";

            menu.RawUrl = rawAppUrl;
            menu.Created = DateTime.Now;

            db.insert( menu );

            return menu;
        }

        public void RemoveMenuByApp( IMemberApp app, String rawAppUrl ) {

            IMenu menu = FindByApp( rawAppUrl );
            if (menu != null) {

                // 删除一个菜单
                Delete( menu );

                updateDefaultMenu( menu );

            }
        }

        private void updateDefaultMenu( IMenu menu ) {
            if (strUtil.EqualsIgnoreCase( menu.Url, "default" )) {
                wojilu.ORM.Caching.ContextCache.Clear();
                IList menus = this.GetList( menu.OwnerId );
                if (menus.Count > 0) {
                    IMenu defaultMenu = menus[0] as IMenu;
                    defaultMenu.Url = "default";
                    db.update( defaultMenu, "Url" );
                }
            }
        }

        public void UpdateMenuByApp( IMemberApp app, String rawAppUrl ) {
            IMenu menu = FindByApp( rawAppUrl );
            if (menu != null) {
                menu.Name = app.Name;
                db.update( menu, "Name" );
            }
        }


    }

}
