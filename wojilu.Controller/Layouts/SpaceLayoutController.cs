/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.Web.Mvc;

using wojilu.Common.Skins;
using wojilu.Common.Menus.Interface;
using wojilu.Common.MemberApp.Interface;

using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;

using wojilu.Web.Controller.Users.Admin;
using wojilu.Members.Sites.Interface;
using wojilu.Members.Sites.Service;
using wojilu.Members.Sites.Domain;
using wojilu.Web.Mvc.Attr;
using System.Collections.Generic;
using wojilu.Common;

namespace wojilu.Web.Controller.Layouts {

    public partial class SpaceLayoutController : ControllerBase {

        public SkinService skinService { get; set; }
        public IMemberAppService userAppService { get; set; }
        public IVisitorService visitorService { get; set; }
        public IMenuService menuService { get; set; }
        public ISiteSkinService siteSkinService { get; set; }

        public SpaceLayoutController() {
            skinService = new SkinService();
            userAppService = new UserAppService();
            visitorService = new VisitorService();
            menuService = new UserMenuService();
            siteSkinService = new SiteSkinService();
        }

        public override void Layout() {

            visitSpace();

            Page.Keywords = ctx.owner.obj.Name;
            User user = ctx.owner.obj as User;
            Page.Description = ctx.owner.obj.Name + "的空间 " + user.Title;

            bindCommon();

            loadHeader();

            bindSpaceName();
            set( "spaceUrl", getSpaceUrl() );
            set( "sitename", config.Instance.Site.SiteName );
            set( "sitelink", ctx.url.SiteAndAppPath );

            skinService.SetSkin( new SpaceSkin() );
            String skinContent = skinService.GetUserSkin( ctx.owner.obj, ctx.GetInt( "skinId" ), MvcConfig.Instance.CssVersion );
            set( "skinContent", skinContent );

            set( "customSkinLink", to( new Users.Admin.SkinController().CustomBg ) );
        }

        private void bindSpaceName() {
            User user = ctx.owner.obj as User;
            String spaceName = strUtil.IsNullOrEmpty( user.Title ) ? user.Name + " 的空间" : user.Title;

            set( "spaceName", spaceName );
        }

        private void loadHeader() {
            object loadHeader = ctx.GetItem( "loadHeader" );
            if (loadHeader != null && (Boolean)loadHeader == false)
                set( "header", "" );
            else
                load( "header", Header );
        }

        [NonVisit]
        public void Header() {

            bindSpaceName();
            set( "spaceUrl", getSpaceUrl() );
            List<IMenu> list = menuService.GetList( ctx.owner.obj );
            bindNavList( list );
        }

        public void AdminLayout() {

            set( "lostPage", Link.To( Site.Instance, new MainController().lost ) );
            bindCommon();

            object isLoadAdminSidebar = ctx.GetItem( "_loadAdminSidebar" );
            if (isLoadAdminSidebar != null && (Boolean)isLoadAdminSidebar == false)
                set( "adminMain", "#{layout_content}" );
            else
                load( "adminMain", AdminSidebar );
        }

        [NonVisit]
        public void AdminSidebar() {

            User owner = ctx.owner.obj as User;

            set( "owner.Name", owner.Name );
            set( "owner.Pic", owner.PicSmall );

            set( "owner.EditProfile", Link.To( owner, new UserProfileController().Profile ) );
            set( "owner.EditContact", Link.To( owner, new UserProfileController().Contact ) );
            set( "owner.EditInterest", Link.To( owner, new UserProfileController().Interest ) );
            set( "owner.EditPic", Link.To( owner, new UserProfileController().Face ) );
            set( "owner.EditPwd", Link.To( owner, new UserProfileController().Pwd ) );


            IList userAppList = userAppService.GetByMember( ctx.owner.Id );
            Boolean isFrm = true;
            bindUserAppList( userAppList, isFrm );

            set( "shareLink", Link.To( ctx.owner.obj, new Users.Admin.ShareController().Index, -1 ) );

            Boolean isUserAppAdminClose = Component.IsClose( typeof( UserAppAdmin ) );
            if (isUserAppAdminClose) {
                set( "appAdminStyle", "display:none" );
            }
            else {
                set( "appAdminUrl", Link.To( ctx.owner.obj, new AppController().Index ) );
                set( "appAdminStyle", "" );
            }

            Boolean isUserMenuAdminClose = Component.IsClose( typeof( UserMenuAdmin ) );
            if (isUserMenuAdminClose) {
                set( "menuAdminStyle", "display:none" );
            }
            else {
                set( "menuAdminUrl", Link.To( ctx.owner.obj, new MenuController().Index ) );
                set( "menuAdminStyle", "" );
            }

            Boolean isUserLinksClose = Component.IsClose( typeof( UserLinks ) );
            if (isUserLinksClose) {
                set( "myUrlStyle", "display:none" );
            }
            else {
                set( "myUrlList", Link.To( ctx.owner.obj, new MyLinkController().Index ) );
                set( "myUrlStyle", "" );
            }

            if (Component.IsEnableGroup()) {
                set( "groupLink", Link.To( ctx.owner.obj, new MyGroupController().My ) );
                set( "groupLinkStyle", "" );
            }
            else {
                set( "groupLinkStyle", "display:none" );
            }


        }

        private void visitSpace() {

            if (ctx.owner.obj.GetType() == typeof( User ) && ctx.viewer.IsLogin) {
                visitorService.Visit( ctx.viewer.Id, (User)ctx.owner.obj );
            }

        }


    }

}
