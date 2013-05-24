/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.Web.Mvc;

using wojilu.Common.AppInstall;
using wojilu.Common.AppBase;
using wojilu.Common.MemberApp.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Members.Sites.Domain;
using wojilu.Apps.Forum.Domain;
using System.Collections.Generic;

namespace wojilu.Web.Controller.Common.Admin {


    public partial class AppBaseController : ControllerBase {


        private void bindAppList( IList apps ) {

            IBlock block = getBlock( "list" );
            foreach (IMemberApp app in apps) {

                if (app.AppInfo.IsInstanceClose( ctx.owner.obj.GetType() )) continue;

                block.Set( "app.Id", app.Id );
                block.Set( "app.Name", app.Name );
                block.Set( "app.Status", app.Status );
                block.Set( "app.Permission", getPermission( app ) );
                block.Set( "app.StatusStyle", getStatusStyle( app ) );
                block.Set( "app.OrderId", app.OrderId );
                block.Set( "app.TypeName", app.AppInfo.Name );
                block.Set( "app.EditUrl", to( Edit, app.Id ) );
                block.Set( "app.StateStr", getAppState( app ) );

                block.Set( "app.StateAdmin", getAppStateAdmin( app ) );
                block.Set( "app.DeleteUrl", to( Delete, app.Id ) );
                block.Set( "app.ViewUrl", to( ViewUrl, app.Id ) );

                block.Next();
            }
        }

        private String getPermission( IMemberApp app ) {
            if (app.AccessStatus == (int)AccessStatus.Public)
                return lang( "statusPublic" );
            if (app.AccessStatus == (int)AccessStatus.Friend)
                return lang( "statusFriend" );
            if (app.AccessStatus == (int)AccessStatus.Private)
                return lang( "statusPrivate" );
            return lang( "statusPublic" );
        }



        private String getAppState( IMemberApp app ) {
            if (app.IsStop == 1) return lang( "stopped" );
            return lang( "running" );
        }

        private object getAppStateAdmin( IMemberApp app ) {

            if (app.IsStop == 1)
                return string.Format(
                    "<span href='{0}' class=\"putCmd cmd\">" + lang( "startApp" ) + "</span>", to( Start, app.Id ) );

            return
                string.Format(
                    "<span href='{0}' class=\"putCmd cmd\">" + lang( "stopApp" ) + "</span>", to( Stop, app.Id ) );
        }

        private void bindHomePage() {

            set( "devUrl", "www.wojilu.net" );
            set( "devName", "wojilu" );

            //IBlock homeBlock = getBlock( "homeApp" );
            //if (ctx.owner.obj is User) {
            //    homeBlock.Set( "userHomeLink", to( CreateUserHome ) );
            //    homeBlock.Next();
            //}
        }

        private void bindAppEdit( IMemberApp app ) {
            set( "appInfo.Type", app.AppInfo.Name );
            set( "appInfo.Description", app.AppInfo.Description );
            set( "app.Name", app.Name );
            set( "app.OrderId", app.OrderId );
            //set( "app.AccessStatus", AccessStatusUtil.GetRadioList( app.AccessStatus ) );
            //set( "viewer.MenuAdmin", t2( new MenuAdminController().Index ) );
        }

        private object getStatusStyle( IMemberApp app ) {
            if (app.IsStop == 1) {
                return "stop";
            }
            return "";
        }


        private void bindAppInfo( AppInstaller info ) {
            set( "app.Name", info.Name );
            set( "app.Description", info.Description );
            set( "app.Id", info.Id );
        }


        private void bindAppSelectList( IList apps ) {

            IBlock listBlock = getBlock( "list" );

            foreach (AppInstaller app in apps) {

                if (app.ParentId > 0) continue;

                listBlock.Set( "app.Name", app.Name );

                String appLogo = app.Logo.Replace( "~img/", sys.Path.Img );
                listBlock.Set( "app.Logo", appLogo );

                listBlock.Set( "app.Creator", app.Creator );
                listBlock.Set( "app.Description", app.Description );

                if (isInstalled( app )) {
                    String addInfo = "<span class=\"note\">(" + lang( "appInstalled" ) + ")</span>";
                    listBlock.Set( "app.AddInfo", addInfo );
                }
                else {

                    bindAppAdder( apps, listBlock, app );
                }

                listBlock.Next();
            }
        }

        private void bindAppAdder( IList apps, IBlock listBlock, AppInstaller app ) {

            String addGif = strUtil.Join( sys.Path.Img, "add.gif" );
            String downGif = strUtil.Join( sys.Path.Img, "downWhite.gif" );

            List<AppInstaller> childApps = getChildApp( app, apps );
            if (childApps.Count <= 0) {

                String lnk = to( NewApp, app.Id );
                String addInfo = string.Format( "<a href=\"{0}\" class=\"cmd frmBox\" title=\"{1}\"><img src=\"{2}\"> {3}</a>",
                    lnk, lang( "appAddTip" ), addGif, lang( "appadd" ) );

                listBlock.Set( "app.AddInfo", addInfo );
            }
            else {
                String addInfo = getSubApps( app, childApps );
                listBlock.Set( "app.AddInfo", addInfo );
            }
        }

        private String getSubApps( AppInstaller app,  List<AppInstaller> childApps ) {

            String addGif = strUtil.Join( sys.Path.Img, "add.gif" );
            String downGif = strUtil.Join( sys.Path.Img, "down.gif" );

            String addInfo = "<span class=\"cmd menuMore\" list=\"subApps" + app.Id + "\"><img src=\"" + addGif + "\"> " + lang( "appadd" ) + " <img src=\"" + downGif + "\"/>";
            addInfo += "<ul class=\"menuItems\" style=\"width:250px;text-align:left\" id=\"subApps" + app.Id + "\">";

            String lnk = string.Format( "<a href=\"{0}\" class=\"frmBox\" title=\"{1}\"><img src=\"" + addGif + "\"> ", to( NewApp, app.Id ), lang( "appAddTip" ) );

            addInfo += "<li><div><strong>" + lnk + app.Name + "</a></strong><span class=\"note\">(»ù±¾ÐÍ)</span></div></li>";

            foreach (AppInstaller a in childApps) {

                String alnk = string.Format( "<a href=\"{0}\" class=\"frmBox\" title=\"{1}\"><img src=\"" + addGif + "\"> ", to( NewApp, a.Id ), lang( "appAddTip" ) );

                addInfo += "<li><div><strong>" + alnk + a.Name + "</a></strong><span class=\"note\">(" + a.Description + ")</span></div></li>";
            }

            addInfo += "</ul></span>";
            return addInfo;
        }

        private List<AppInstaller> getChildApp( AppInstaller app, IList apps ) {
            List<AppInstaller> list = new List<AppInstaller>();
            foreach (AppInstaller a in apps) {
                if (a.ParentId == app.Id) list.Add( a );
            }
            return list;
        }

        private Boolean isInstalled( AppInstaller app ) {

            if (app.Singleton == false) return false;
            if (ctx.owner.obj.GetType() == typeof( Site ) && app.TypeFullName == typeof( ForumApp ).FullName) return false;

            if (userAppService.HasInstall( ctx.owner.Id, app.Id )) return true;

            return false;


        }

    }
}

