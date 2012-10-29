/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Interface;
using wojilu.Common.AppBase;
using wojilu.Common.Menus.Interface;
using wojilu.Web.Mvc.Routes;
using wojilu.Web.Url;
using wojilu.Members.Users.Domain;
using wojilu.Common;
using wojilu.Members.Groups.Domain;
using wojilu.Web.Mvc;

namespace wojilu.Web.Context.Initor {

    public class OwnerInit : IContextInit {


        public void Init( MvcContext ctx ) {

            if (ctx.utils.isEnd()) return;

            if (strUtil.IsNullOrEmpty( ctx.route.ownerType )) return;

            IMember owner = InitHelperFactory.GetHelper( ctx ).getOwnerByUrl( ctx );

            if (owner == null || owner.Status == MemberStatus.Deleted || owner.Status == MemberStatus.Approving) {

                ctx.utils.endMsg( "owner not found (" + lang.get( "exUser" ) + " )", HttpStatus.NotFound_404 );
                return;
            }

            // 检查空间是否禁用
            if (spaceStopped( ctx, owner )) {
                ctx.utils.endMsg( "对不起，用户空间已停用", HttpStatus.NotFound_404 );
                return;
            }

            // 检查群组是否禁用
            if (owner.GetType() == typeof( Group ) &&
                Component.IsEnableGroup() == false) {

                ctx.utils.endMsg( "对不起，群组功能已停用", HttpStatus.NotFound_404 );
                return;
            }

            OwnerContext context = new OwnerContext();
            context.Id = owner.Id;
            context.obj = owner;

            ctx.utils.setOwnerContext( context );

            this.updateRoute_ByOwnerMenus( ctx, owner );

            initEditorUploadPath( ctx ); // 如果是登录用户，则设置编辑的上传路径

        }

        private void initEditorUploadPath( MvcContext ctx ) {

            if (ctx.viewer.IsLogin) {
                // 此处使用onwer，避免二级域名下的跨域问题
                ctx.SetItem( "editorUploadUrl", Link.To( ctx.owner.obj, "Users/UserUpload", "UploadForm", -1, -1 ) );
                ctx.SetItem( "editorMyPicsUrl", Link.To( ctx.owner.obj, "Users/UserUpload", "MyPics", -1, -1 ) );
            }
        }


        private bool spaceStopped( MvcContext ctx, IMember owner ) {

            if (owner.GetType() != typeof( User )) return false;
            if (Component.IsEnableUserSpace()) return false;

            // 三种例外
            if (ctx.route.isAdmin) return false;
            if (ctx.route.isInPath( "Microblogs" )) return false;
            if (ctx.url.PathAndQuery.IndexOf( "Layouts/TopNav/Nav" ) >= 0) return false;

            return true;
        }

        private void updateRoute_ByOwnerMenus( MvcContext ctx, IMember owner ) {


            List<IMenu> list = InitHelperFactory.GetHelper( ctx ).GetMenus( ctx.owner.obj );

            String cleanUrlWithoutOwner = ctx.route.getCleanUrlWithoutOwner( ctx );
            if (cleanUrlWithoutOwner == string.Empty || strUtil.EqualsIgnoreCase( cleanUrlWithoutOwner, "default" )) {

                updateRoute_Menu( ctx, list, "default" );
                ctx.utils.setIsHome( true );
            }
            else {
                updateRoute_Menu( ctx, list, cleanUrlWithoutOwner );
            }
        }

        private void updateRoute_Menu( MvcContext ctx, List<IMenu> list, String cleanUrlWithoutOwner ) {
            foreach (IMenu menu in list) {
                if (cleanUrlWithoutOwner.Equals( menu.Url )) { // 如果友好网址相同

                    // 获取实际的网址
                    String fullUrl = UrlConverter.getMenuFullPath( ctx, menu );
                    Route.setRoutePath( fullUrl );

                    Route newRoute = RouteTool.Recognize( fullUrl, ctx.web.PathApplication );
                    if (newRoute == null) break;

                    refreshRouteAndOwner( ctx, newRoute );

                    break;
                }
            }
        }

        private void refreshRouteAndOwner( MvcContext ctx, Route newRoute ) {


            if (newRoute.owner != ctx.route.owner || newRoute.ownerType != ctx.route.ownerType) {
                ctx.utils.setRoute( newRoute );
                Init( ctx ); // 当前Owner已经变换，所以需要重新更新owner
            }
            else {
                ctx.utils.setRoute( newRoute );

            }
        }


    }

}
