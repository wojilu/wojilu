/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Url;

using wojilu.Common.Menus.Interface;
using wojilu.Common.MemberApp.Interface;

using wojilu.Members.Sites.Domain;


namespace wojilu.Web.Controller.Layouts {

    public partial class SpaceLayoutController : ControllerBase {

        private void bindCommon() {

            utils.renderPageMetaToView();

            String skinContent = siteSkinService.GetSkin();
            set( "siteSkinContent", skinContent );

            if (ctx.utils.getIsHome()) {
                String str = lang( "userSpace" );
                set( "pageTitle", string.Format( str, ctx.owner.obj.Name ) );
            }

            set( "langStr", wojilu.lang.getLangString() );
            load( "topNav", new TopNavController().Index );

            set( "statsJs", config.Instance.Site.GetStatsJs() );
        }



        private String getSpaceUrl() {
            String spaceUrl = Link.ToMember( ctx.owner.obj );
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
