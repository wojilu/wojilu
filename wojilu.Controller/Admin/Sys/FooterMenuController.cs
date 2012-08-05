/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Common.Menus;
using wojilu.Members.Sites.Interface;
using wojilu.Members.Sites.Service;
using wojilu.Members.Users.Domain;
using wojilu.Web.Controller.Security;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Interface;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Admin.Sys {

    public class FooterMenuController : ControllerBase {

        public IAdminLogService<SiteLog> logService { get; set; }
        public FooterMenuController() {
            logService = new SiteLogService();
        }

        public void List() {
            List<FooterMenu> menus = FooterMenu.GetAll();
            bindList( "list", "data", menus, bindLink );
            set( "addLink", to( Add ) );
            set( "sortAction", to( SaveSort ) );
        }

        private void bindLink( IBlock tpl, String lbl, object obj ) {

            FooterMenu data = obj as FooterMenu;

            tpl.Set( "data.Link", lnkFull( data.Link ) );
            tpl.Set( "data.LinkEdit", to( Edit, data.Id ) );
            tpl.Set( "data.LinkDelete", to( Delete, data.Id ) );
        }

        [HttpPost, DbTransaction]
        public virtual void SaveSort() {

            int id = ctx.PostInt( "id" );
            String cmd = ctx.Post( "cmd" );

            FooterMenu data = cdb.findById<FooterMenu>( id );
            String condition = (ctx.app == null ? "" : "AppId=" + ctx.app.Id);

            List<FooterMenu> list = FooterMenu.GetAll();

            if (cmd == "up") {

                new SortUtil<FooterMenu>( data, list ).MoveUp();
                echoJsonOk();
            }
            else if (cmd == "down") {

                new SortUtil<FooterMenu>( data, list ).MoveDown();
                echoJsonOk();
            }
            else {
                echoError( lang( "exUnknowCmd" ) );
            }

        }


        private String lnkFull( String link ) {
            if (link.StartsWith( "http:" )) return link;
            if (link.StartsWith( ctx.url.SiteUrl )) return link;
            return strUtil.Join( ctx.url.SiteUrl, link );
        }

        public void Add() {
            target( Create );
        }

        [HttpPost, DbTransaction]
        public void Create() {

            FooterMenu data = validate( new FooterMenu() );
            if (ctx.HasErrors) {
                run( Add );
                return;
            }

            data.insert();
            log( SiteLogString.AddFooterMenu(), data );

            echoToParentPart( lang( "opok" ) );
        }

        public void Edit( int id ) {
            target( Update, id );
            FooterMenu data = FooterMenu.GetById( id );
            if (data == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }
            bind( "data", data );
        }

        [HttpPost, DbTransaction]
        public void Update( int id ) {

            FooterMenu data = FooterMenu.GetById( id );
            if (data == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }
            data = validate( data );

            if (ctx.HasErrors) {
                run( Edit, id );
                return;
            }

            data.update();
            log( SiteLogString.UpdateFooterMenu(), data );

            echoToParentPart( lang( "opok" ) );
        }


        [HttpDelete, DbTransaction]
        public void Delete( int id ) {

            FooterMenu data = FooterMenu.GetById( id );
            if (data == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            data.delete();
            log( SiteLogString.DeleteFooterMenu(), data );

            echoAjaxOk();
        }

        private FooterMenu validate( FooterMenu data ) {
            data = ctx.PostValue( data ) as FooterMenu;
            if (strUtil.IsNullOrEmpty( data.Name )) errors.Add( lang( "exName" ) );
            if (strUtil.IsNullOrEmpty( data.Link )) errors.Add( lang( "exLink" ) );
            return data;
        }


        private void log( String msg, FooterMenu data ) {
            String dataInfo = "{Id:" + data.Id + ", Name:'" + data.Name + "'}";
            logService.Add( (User)ctx.viewer.obj, msg, dataInfo, typeof( FooterMenu ).FullName, ctx.Ip );
        }

    }

}
