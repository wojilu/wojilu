/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Url;

using wojilu.Members.Users.Domain;
using wojilu.Members.Sites.Domain;
using wojilu.Common.Menus;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.Context;
using wojilu.Members.Sites.Service;
using wojilu.Members.Users.Service;
using wojilu.Members.Groups.Service;
using wojilu.Members.Interface;
using wojilu.Common.AppBase;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.Menus.Interface;
using wojilu.Members.Groups.Domain;
using wojilu.Web.Controller.Security;
using wojilu.Web.UI;

namespace wojilu.Web.Controller.Common.Admin {


    public partial class MenuBaseController : ControllerBase {

        public virtual IMenuService menuService { get; set; }

        public virtual void log( String msg, IMenu menu ) {
            // 由子类继承去实现
        }

        private Tree<IMenu> _tree;

        private Tree<IMenu> getTree() {
            if (_tree == null) _tree = new Tree<IMenu>( menuService.GetList( ctx.owner.obj ) );
            return _tree;
        }

        public virtual String GetCommonLink() {
            return null;
        }

        //-----------------------------------------------------------------------------------------------------

        public override void Layout() {
            set( "addLink", to( AddMenu ) );
            set( "listLink", to( Index ) );
        }

        public void Index() {
        //    set( "addLink", to( AddMenu ) );
        //    set( "listLink", to( List ) );
        //    load( "list", List );
        //}

        //public void List() {
            set( "sortAction", to( SortMenu ) );
            List<IMenu> menus = getTree().GetAllOrdered();
            bindMenus( menus );
        }

        [HttpPost, DbTransaction]
        public void SortMenu() {

            int id = ctx.PostInt( "id" );
            String cmd = ctx.Post( "cmd" );

            IMenu menu = menuService.FindById( ctx.owner.Id, id );
            IList results = menuService.GetList( ctx.owner.obj );

            List<IMenu> list = new List<IMenu>();
            foreach (IMenu c in results) list.Add( c );

            if (cmd == "up") {

                new SortUtil<IMenu>( menu, list ).MoveUp();
                echoJsonOk();
            }
            else if (cmd == "down") {

                new SortUtil<IMenu>( menu, list ).MoveDown();
                echoJsonOk();
            }
            else {
                echoError( lang( "exUnknowCmd" ) );
            }

        }

        public virtual void AddMenu() {

            target( Create );

            String url = ctx.Get( "url" );
            if (isUrlValid( url ) == false) {
                url = "http://";
            }

            set( "url", url );
            set( "name", ctx.Get( "name" ) );
            set( "furl", ctx.Get( "furl" ) );

            String commonLink = this.GetCommonLink();
            IBlock lnkBlock = getBlock( "commonLink" );
            if (strUtil.HasText( commonLink )) {
                lnkBlock.Set( "lnkAddLink", commonLink );
                lnkBlock.Next();
            }

        }


        public void AddSubMenu( int id ) {

            target( SaveSubMenu, id );

            IMenu menu = menuService.FindById( ctx.owner.Id, id );
            set( "parentMenuName", menu.Name );
        }


        private Boolean isUrlValid( String url ) {
            if (strUtil.IsNullOrEmpty( url )) return false;
            return true;
        }

        [HttpPost, DbTransaction]
        public void Create() {

            IMenu menu = validateMenu( menuService.New() );

            if (ctx.HasErrors) {
                echoError();
                return;
            }

            Result result = menuService.Insert( menu, (User)ctx.viewer.obj, ctx.owner.obj );

            if (result.HasErrors) {
                echoError( result );
                return;
            }

            log( SiteLogString.AddMenu(), menu );

            echoRedirect( lang( "opok" ), to( Index ) );

        }


        [HttpPost, DbTransaction]
        public void SaveSubMenu( int id ) {

            IMenu menu = validateMenu( menuService.New() );

            if (ctx.HasErrors) {
                echoError();
                return;
            }

            menu.ParentId = id;

            Result result = menuService.Insert( menu, (User)ctx.viewer.obj, ctx.owner.obj );

            if (result.HasErrors) {
                echoError( result );
                return;
            }

            log( SiteLogString.AddMenu(), menu );

            echoToParentPart( lang( "opok" ) );

        }


        public void Edit( int id ) {

            IMenu menu = menuService.FindById( ctx.owner.Id, id );
            if (menu == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            set( "m.UrlFull", UrlConverter.getMenuFullUrl( menu, ctx ) );
            bind( "m", menu );

            String chkBold = "";
            String menuColor = "";
            Dictionary<string,string> dic = Css.FromItem( menu.Style );
            if (dic.ContainsKey( "font-weight" )) chkBold = " checked=\"checked\" ";
            if (dic.ContainsKey( "color" )) menuColor = dic["color"];
                
            set( "chkBold", chkBold );
            set( "menuColor", menuColor );

            String chkBlank = menu.OpenNewWindow == 1 ? " checked=\"checked\" " : "";
            set( "chkBlank", chkBlank );

            target( Update, id );
        }

        [HttpPost, DbTransaction]
        public virtual void Update( int id ) {

            IMenu menu = menuService.FindById( ctx.owner.Id, id );
            if (menu == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            menu = validateMenu( menu );

            if (ctx.HasErrors) {
                run( Edit, id );
                return;
            }

            Result result = menuService.Update( menu );

            if (result.HasErrors) {
                errors.Join( result );
                run( Edit, id );
                return;
            }

            log( SiteLogString.UpdateMenu(), menu );

            ctx.SetItem( "currentMenu", menu );

            echoToParentPart( lang( "opok" ) );
        }

        [HttpDelete, DbTransaction]
        public void Delete( int id ) {
            IMenu menu = menuService.FindById( ctx.owner.Id, id );
            if (menu == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }
            Result result = menuService.Delete( menu );
            if (result.HasErrors) {
                echoRedirect( result.ErrorsHtml );
                return;
            }

            log( SiteLogString.DeleteMenu(), menu );


            redirect();
        }

    }
}

