/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Url;

using wojilu.Members.Interface;
using wojilu.Members.Sites.Service;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Members.Groups.Service;
using wojilu.Members.Groups.Domain;

using wojilu.Common.Skins;

using wojilu.Web.Controller.Common.Admin;
using wojilu.Web.Controller.Forum.Utils;
using wojilu.Common.Menus.Interface;
using wojilu.Common.MemberApp.Interface;
using wojilu.Members.Groups.Interface;
using wojilu.Web.Controller.Groups.Admin;
using wojilu.Members.Sites.Interface;

namespace wojilu.Web.Controller.Layouts {

    public partial class GroupLayoutController : ControllerBase {

        private void bindSiteInfo() {

            utils.renderPageMetaToView();
            if (ctx.utils.getIsHome()) set( "pageTitle", ctx.owner.obj.Name + " - " + lang( "group" ) );

            set( "langStr", wojilu.lang.getLangString() );

            set( "defaulttitle", ctx.owner.obj.Name );
            set( "pageKeywords", ctx.owner.obj.Name );
            set( "pageDescription", ctx.owner.obj.Name );
            set( "sitename", Site.Instance.Name );
            set( "sitelink", ctx.url.AppPath );

            set( "g.IndexLink", Link.To( Site.Instance, new Groups.MainController().Index ) );
            set( "g.FriendGroupLink", to( new Groups.FriendController().Index ) );

            set( "groupName", ctx.owner.obj.Name );

            String groupLink = Link.ToMember( ctx.owner.obj );
            if (!groupLink.StartsWith( ctx.url.SiteUrl )) groupLink = strUtil.Join( ctx.url.SiteUrl, groupLink );
            set( "groupLink", groupLink );

            set( "statsJs", config.Instance.Site.GetStatsJs() );
        }

        private void bindGroupNav( IList menus ) {

            List<IMenu> list = MenuHelper.getRootMenus( menus );

            IBlock block = getBlock( "gnavLink" );
            foreach (IMenu menu in list) {

                block.Set( "menu.CurrentClass", getCurrentClass( menu, ctx.GetItemString( "_moduleUrl" ), "current-group-menu" ) );

                IBlock subNavBlock = block.GetBlock( "subNav" );
                IBlock rootBlock = block.GetBlock( "rootNav" );

                List<IMenu> subMenus = MenuHelper.getSubMenus( menus, menu );

                if (subMenus.Count == 0) {
                    MenuHelper.bindMenuSingle( rootBlock, menu, ctx );
                }
                else {
                    IBlock subBlock = subNavBlock.GetBlock( "subMenu" );
                    MenuHelper.bindSubMenus( subBlock, subMenus, ctx );
                    MenuHelper.bindMenuSingle( subNavBlock, menu, ctx );
                }

                block.Next();

            }
        }

        public string getCurrentClass( IMenu menu, String currentModuleUrl, String currentClass ) {

            if (strUtil.IsNullOrEmpty( currentModuleUrl )) return "";

            // 论坛特殊处理：因为论坛链接不是论坛首页，而是版块首页
            if (menu.RawUrl.ToLower().IndexOf( "forum" ) < 0) return MenuHelper.getCurrentClass( menu, currentModuleUrl, currentClass );

            String forumAppUrl = getForumAppUrl( menu.RawUrl );

            if (currentModuleUrl.IndexOf( forumAppUrl ) >= 0) return currentClass;
            return "";
        }

        private string getForumAppUrl( string rawUrl ) {
            String[] arr = rawUrl.Split( wojilu.Web.Mvc.Routes.RouteTool.Separator );
            return joinUrl( joinUrl( arr[0], "Forum" ), "Index" );
        }

        private String joinUrl( String a, String b ) {
            return strUtil.Join( a, b, MvcConfig.Instance.UrlSeparator );
        }


        //----------------------------------------------------------------------------------------------------

        private void bindSiteLogoAndLoginInfo() {

            set( "site.Name", Site.Instance.Name );
            set( "site.Logo", config.Instance.Site.GetLogoHtml() );

            IBlock loginBlock = getBlock( "login" );
            if (ctx.viewer.IsLogin) return;

            loginBlock.Set( "loginLink", t2( new MainController().Login ) );
            loginBlock.Set( "regLink", t2( new RegisterController().Register ) );
            loginBlock.Next();
        }

        private void bindSkin() {
            skinService.SetSkin( new GroupSkin() );
            String skinContent = skinService.GetUserSkin( ctx.owner.obj, ctx.GetInt( "skinId" ), MvcConfig.Instance.CssVersion );
            set( "skinContent", skinContent );
        }

        private void bindSiteSkin() {
            String skinContent = siteSkinService.GetSkin();
            set( "siteSkinContent", skinContent );
        }


        private String getJoinCmd( Group group ) {

            String lnkJoin = to( new Groups.JoinController().Index );
            String lnkQuit = to( new Groups.JoinController().Quit );


            String joinStr = string.Format( "<span href='{0}' class='frmBox btn btn-mini'><i class='icon-plus'></i> {1}</span>", lnkJoin, lang( "joinGroup" ) );
            String quitStr = string.Format( "<span href='{0}' class='frmBox btn btn-mini'><i class='icon-off'></i> {1}</span>", lnkQuit, lang( "quitGroup" ) );

            int status = mgrService.MemberStatus( (User)ctx.viewer.obj, group.Id );

            String cmd = "";
            if (status == GroupRole.Member.Id || status == GroupRole.Administrator.Id)
                cmd = quitStr;
            else if (status == GroupRole.Approving.Id)
                cmd = lang( "waitApprove" ) + "...";
            else if (status == GroupRole.NotFound) {

                if (group.IsCloseJoinCmd == 1) {
                    cmd = "";
                }
                else {
                    cmd = joinStr;
                }

            }
            else if (status == GroupRole.Blacklist.Id)
                cmd = "";

            return cmd;
        }


        //public void bindGroupNav( IList list ) {

        //    IBlock block = getBlock( "gnavLink" );

        //    foreach (IMenu menu in list) {
        //        block.Set( "gmenu.Name", menu.Name );
        //        block.Set( "gmenu.Link", UrlConverter.toMenu( menu, ctx ) );
        //        block.Next();
        //    }
        //}


        private void bindFriends( IList friends ) {
            IBlock gfBlock = getBlock( "friendGroupList" );
            foreach (Group g in friends) {
                gfBlock.Set( "g.Name", g.Name );
                gfBlock.Set( "g.Logo", g.LogoSmall );
                gfBlock.Set( "g.Url", Link.ToMember( g ) );
                gfBlock.Next();
            }
        }

        private void bindNewMemberList( IList newMember ) {
            IBlock block = getBlock( "newMember" );
            foreach (User member in newMember) {
                if (member == null) continue;
                block.Set( "m.Name", member.Name );
                block.Set( "m.Face", member.PicSmall );
                block.Set( "m.Url", Link.ToMember( member ) );
                block.Next();
            }
        }

        private void bindGroupStats( Group g ) {

            set( "g.VisitCount", g.Hits );
            set( "g.Created", g.Created.ToShortDateString() );

            String groupStats = string.Format( lang( "groupStats" ), g.MemberCount, g.Hits, g.Created.ToShortDateString() );
            set( "groupStats", groupStats );

            Boolean isOffice = false;
            List<User> officerList = mgrService.GetOfficer( g.Id );
            StringBuilder sb = getOfficerStr( officerList, out isOffice );
            set( "g.OfficerList", sb );

            String adminLink = getAdminLink( isOffice );
            set( "adminHomeLink", adminLink );
        }

        private String getAdminLink( Boolean isOffice ) {
            String adminLink = "";
            if (isOffice || ctx.viewer.IsAdministrator()) {
                String imgTools = strUtil.Join( sys.Path.Img, "tools.gif" );
                adminLink = string.Format( "<a href='{0}' class='strong'><img src='{1}'/> {2}</a>",
                    Link.To( ctx.owner.obj, new Groups.Admin.MainController().Index ),
                    imgTools,
                    lang( "groupAdm" )
                    );
            }
            return adminLink;
        }

        private StringBuilder getOfficerStr( List<User> officerList, out Boolean isOffice ) {
            StringBuilder sb = new StringBuilder();
            isOffice = false;
            foreach (User user in officerList) {
                sb.Append( "<a href=\"" );
                sb.Append( toUser( user ) );
                sb.Append( "\">" );
                sb.Append( user.Name );
                sb.Append( "</a>" );
                sb.Append( " " );

                if (user.Id == ctx.viewer.Id) isOffice = true;
            }
            return sb;
        }


        private void bindAppList( IList apps ) {
            IBlock block = getBlock( "apps" );
            foreach (IMemberApp app in apps) {
                block.Set( "app.NameAndUrl", getNameAndUrl( app, ctx.owner.obj ) );
                block.Next();
            }
        }

        private String getNameAndUrl( IMemberApp app, IMember owner ) {
            if (app.IsStop == 1) return ("<span class='stop'>" + app.Name + "</span>");
            return string.Format( "<a href='{1}'>{0}</a>", app.Name, alink.ToAppAdmin( owner, app ) );
        }

        private String getLoginInfo() {

            if (ctx.viewer.IsLogin == false) return "";

            String loginLink = Link.To( Site.Instance, new MainController().Login );
            String regLink = Link.To( Site.Instance, new RegisterController().Register );
            String loginInfo = string.Format( "<a href='{0}'>{1}</a> <a href='{2}' class='left10'>{3}</a>", loginLink, lang( "login" ), regLink, lang( "register" ) );
            return loginInfo;



        }


    }

}
