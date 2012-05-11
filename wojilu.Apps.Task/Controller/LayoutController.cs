using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Controller.Layouts;
using wojilu.Common.Skins;
using wojilu.Common.MemberApp.Interface;
using wojilu.Members.Users.Interface;
using wojilu.Common.Menus.Interface;
using wojilu.Members.Sites.Interface;
using wojilu.Members.Users.Service;
using wojilu.Members.Sites.Service;
using wojilu.Members.Users.Domain;
using wojilu.Web.Mvc.Attr;
using wojilu.Members.Sites.Domain;
using System.Collections;
using wojilu.Web.Controller.Users.Admin;
using wojilu.Common;
using wojilu.Web.Controller.Common;

namespace wojilu.Web.Controller.Task
{
    public partial class TaskLayoutController : ControllerBase
    {
        public SkinService skinService { get; set; }
        public IMemberAppService userAppService { get; set; }
        public IVisitorService visitorService { get; set; }
        public IMenuService menuService { get; set; }
        public ISiteSkinService siteSkinService { get; set; }

        public TaskLayoutController()
        {
            skinService = new SkinService();
            userAppService = new UserAppService();
            visitorService = new VisitorService();
            menuService = new UserMenuService();
            siteSkinService = new SiteSkinService();
        }

        private void bindSpaceName() {
            User user = ctx.viewer.obj as User;
            String spaceName = strUtil.IsNullOrEmpty(user.Title) ? user.Name + " 的空间" : user.Title;
            set("spaceName","全部任务-个人中心");
        }

        private void loadHeader()
        {
            object loadHeader = ctx.GetItem("loadHeader");
            if(loadHeader != null && (Boolean)loadHeader == false)
                set("header", "");
            else
                load("header", Header);
        }

        [NonVisit]
        public void Header()
        {
            bindSpaceName();
            set("spaceUrl", getSpaceUrl());
            List<IMenu> list = menuService.GetList(ctx.owner.obj);
            bindNavList(list);
        }

        public void AdminLayout()
        {

            set("lostPage", Link.T2(Site.Instance, new MainController().lost));
            bindCommon();
            object isLoadAdminSidebar = ctx.GetItem("_loadAdminSidebar");
            if(isLoadAdminSidebar != null && (Boolean)isLoadAdminSidebar == false)
                set("adminMain", "#{layout_content}");
            else
                load("adminMain", AdminSidebar);

           // load("topNav", new TopNavController().Index);
            load("header", new TopNavController().Header);
            set("statsJs", config.Instance.Site.GetStatsJs());

            set("adFooter", AdItem.GetAdById(AdCategory.Footer));

            set("adLoadLink", to(new AdLoaderController().Index));
        }

        [NonVisit]
        public void AdminSidebar()
        {

            User owner = ctx.viewer.obj as User;

            set("owner.Name", owner.Name);
            set("owner.Pic", owner.PicSmall);

            set("owner.EditProfile", Link.T2(owner, new UserProfileController().Profile));
            set("owner.EditContact", Link.T2(owner, new UserProfileController().Contact));
            set("owner.EditInterest", Link.T2(owner, new UserProfileController().Interest));
            set("owner.EditPic", Link.T2(owner, new UserProfileController().Face));
            set("owner.EditPwd", Link.T2(owner, new UserProfileController().Pwd));


            IList userAppList = userAppService.GetByMember(ctx.viewer.Id);
            Boolean isFrm = true;
            bindUserAppList(userAppList, isFrm);

            set("shareLink", Link.T2(ctx.viewer.obj, new Users.Admin.ShareController().Index, -1));

            Boolean isUserAppAdminClose = Component.IsClose(typeof(UserAppAdmin));
            if(isUserAppAdminClose)
            {
                set("appAdminStyle", "display:none");
            }
            else
            {
                set("appAdminUrl", Link.T2(ctx.viewer.obj, new AppController().Index));
                set("appAdminStyle", "");
            }

            Boolean isUserMenuAdminClose = Component.IsClose(typeof(UserMenuAdmin));
            if(isUserMenuAdminClose)
            {
                set("menuAdminStyle", "display:none");
            }
            else
            {
                set("menuAdminUrl", Link.T2(ctx.viewer.obj, new MenuController().Index));
                set("menuAdminStyle", "");
            }

            Boolean isUserLinksClose = Component.IsClose(typeof(UserLinks));
            if(isUserLinksClose)
            {
                set("myUrlStyle", "display:none");
            }
            else
            {
                set("myUrlList", Link.T2(ctx.viewer.obj, new MyLinkController().Index));
                set("myUrlStyle", "");
            }

            if(Component.IsEnableGroup())
            {
                set("groupLink", Link.T2(ctx.viewer.obj, new MyGroupController().My));
                set("groupLinkStyle", "");
            }
            else
            {
                set("groupLinkStyle", "display:none");
            }

        }
    }

    /*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */
    public partial class TaskLayoutController : ControllerBase
    {
        private void bindCommon() {

            utils.renderPageMetaToView();

            String skinContent = siteSkinService.GetSkin();
            set( "siteSkinContent", skinContent );

            if (ctx.utils.getIsHome()) {
                String str = lang( "userSpace" );
                set("pageTitle", string.Format(str, ctx.viewer.obj.Name));
            }

            set( "langStr", wojilu.lang.getLangString() );
            load( "topNav", new TopNavController().Index );

            set( "statsJs", config.Instance.Site.GetStatsJs() );
        }



        private String getSpaceUrl() {
            String spaceUrl = Link.ToMember(ctx.viewer.obj);
            if (spaceUrl.StartsWith( "http" )) return spaceUrl; // 二级域名下会有完整域名
            if (!spaceUrl.StartsWith( ctx.url.SiteUrl )) spaceUrl = strUtil.Join( ctx.url.SiteUrl, spaceUrl );
            return spaceUrl;
        }

        private void bindNavList( List<IMenu> menus ) {

            List<IMenu> list = MenuHelper.getRootMenus( menus );

            IBlock block = getBlock( "navLink" );

            foreach (IMenu menu in list) {

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


        //----------------------------------------------------------------------------------------------------

        private void bindUserAppList( IList userAppList, Boolean isFrm ) {
            IBlock block = getBlock( "apps" );
            foreach (IMemberApp app in userAppList) {

                if (app.AppInfo.IsInstanceClose( ctx.owner.obj.GetType() )) continue;

                block.Set( "app.NameAndUrl", getNameAndUrl( app, isFrm ) );
                block.Next();
            }
        }

        private String getNameAndUrl( IMemberApp app, Boolean isFrm ) {

            String iconPath = strUtil.Join( sys.Path.Img, "app/m/" );
            iconPath = strUtil.Join( iconPath, app.AppInfo.TypeFullName ) + ".png";
            String icon = string.Format( "<img src=\"{0}\" />", iconPath );

            if (app.IsStop == 1) return ("<span class='stop'>" + icon + " " + app.Name + "</span>");

            //return string.Format( "<a href='{1}'>{0}</a> <a href='{2}' style='color:#666'>发表</a>", app.Name, Link.ToAppAdmin( ctx.owner.obj, app ), Link.ToAppAdminAdd( ctx.owner.obj, app ) );

            String frmStr = isFrm ? "class=\"frmLink\" loadTo=\"uaMain\" nolayout=1" : "";
            return string.Format( "<a href=\"{1}\" {3}>{2} {0}</a>", app.Name, alink.ToAppAdmin( ctx.owner.obj, app ), icon, frmStr );


        }

    }

}

