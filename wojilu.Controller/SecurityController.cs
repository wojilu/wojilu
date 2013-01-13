/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;

using wojilu.Web.Mvc;
using wojilu.Web.Context;

using wojilu.Members.Groups.Domain;
using wojilu.Members.Groups.Service;
using wojilu.Members.Groups.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Service;
using wojilu.Members.Sites.Domain;

using wojilu.Common.AppBase;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.MemberApp.Interface;
using wojilu.Web.Controller.Admin;
using wojilu.Web.Controller.Security;
using wojilu.Web.Controller.Users;
using wojilu.Net;
using wojilu.DI;

namespace wojilu.Web.Controller {

    public class SecurityController : ControllerBase {

        private IMemberGroupService mgrService = new MemberGroupService();

        public override void CheckPermission() {

            if (config.Instance.Site.NeedLogin && ctx.viewer.IsLogin == false) {
                echoRedirect( lang( "exPlsLogin" ), t2( new MainController().Login ) + "?returnUrl=" + ctx.url.EncodeUrl );
                return;
            }

            if (config.Instance.Site.BannedIp.Length > 0) {
                if (IpUtil.IsAllowedIp( ctx.Ip, config.Instance.Site.BannedIp ) == false) {
                    echo( config.Instance.Site.BannedIpInfo );
                    return;
                }
            }

            if (ctx.owner.obj is Site)
                checkSitePermission();

            else if (ctx.owner.obj is User)
                checkSpacePermission();

            else if (ctx.owner.obj is Group)
                checkGroupPermission();
        }

        public void CanAppAdmin( int appId ) {

            if (ctx.viewer.IsLogin == false) {
                echoText( "no" );
                return;
            }

            if (ctx.viewer.IsAdministrator()) {
                echoAjaxOk();
                return;
            }

            if (ctx.viewer.IsOwnerAdministrator( ctx.owner.obj )) {
                echoAjaxOk();
                return;
            }

            String appType = ctx.Get( "appType" );

            if (ObjectContext.Instance.TypeList.ContainsKey( appType ) == false) {
                echoText( "no" );
                return;
            }

            Type t = ObjectContext.Instance.TypeList[appType];

            if (AppAdminRole.CanAppAdmin( ctx.viewer.obj, ctx.owner.obj, t, appId )) {
                echoAjaxOk();
            }
            else {
                echoText( "no" );
            }

        }


        //--------------------------------------------------------- 网站 -------------------------------------------------------------------------

        private void checkSitePermission() {

            if (!checkSiteAppPermission()) return;

            if (ctx.route.isAdmin == false) return;

            //-------------------------------------------------------------------

            if (ctx.viewer.IsLogin == false) {
                redirectUrl( t2( new MainController().Login ) );
                return;
            }

            if (AdminSecurityUtils.HasSession( ctx ) == false) {
                redirectUrl( t2( new Admin.MainController().Login ) + "?returnUrl=" + ctx.url.EncodeUrl );

                return;
            }

            if (!checkSiteAdminPermission()) return;

            if (!checkSiteAppAdminPermission()) return;

            checkUserDataAdminPermission();
        }

        // 前台app权限
        private Boolean checkSiteAppPermission() {

            if (ctx.route.isAdmin) return true;

            if (ctx.app == null) return true;
            if (ctx.app.obj == null) return true;
            IMemberApp app = ((AppContext)ctx.app).UserApp;
            if (app == null) return true;

            Boolean hasPermission = AppRole.IsRoleInApp( ((User)ctx.viewer.obj).RoleId, typeof( SiteRole ).FullName, app.Id );
            if (!hasPermission) {
                echo( lang( "exNoPermission" ) );
                return false;
            }

            return true;
        }

        // 1) 禁止链接检查
        private Boolean checkSiteAdminPermission() {
            if (SiteAdminService.HasAdminPermission( ctx.viewer.obj, ctx.url.Path )) {
                return true;
            }
            else {
                echo( lang( "exNoAdminPermission" ) );
                return false;
            }
        }

        // 2) 后台app检查
        private Boolean checkSiteAppAdminPermission() {

            if (isSiteAppAdmin() == false) return true;

            if (ctx.app == null) return true;
            if (ctx.app.obj == null) return true;
            IMemberApp app = ((AppContext)ctx.app).UserApp;
            if (app == null) return true;

            Boolean hasPermission = AppAdminRole.IsRoleInApp( ((User)ctx.viewer.obj).RoleId, app.Id );
            if (!hasPermission) {
                echo( lang( "exNoAppAdminPermission" ) );
                return false;
            }
            return true;
        }

        // 3) 用户管理数据检查
        private Boolean checkUserDataAdminPermission() {

            if (isUserDataAdmin() == false) return true;
            String ns = ctx.route.ns;
            string[] arrItem = strUtil.TrimStart( ns, "Admin.Apps." ).Split( '.' );
            String app = arrItem[0];

            Boolean hasPermission = UserDataRole.IsRoleInApp( ((User)ctx.viewer.obj).RoleId, app );
            if (!hasPermission) {
                echo( lang( "exNoUserDataAdminPermission" ) );
                return false;
            }
            return true;
        }

        private Boolean isSiteAppAdmin() {
            if (!(ctx.owner.obj is Site)) return false;
            if (ctx.route.ns.StartsWith( "Admin." ) || "Admin".Equals( ctx.route.ns )) return false;
            if (ctx.route.isAdmin) return true;
            return false;
        }

        private Boolean isUserDataAdmin() {
            if (ctx.route.ns.StartsWith( "Admin.Apps." )) return true;
            return false;
        }

        //--------------------------------------------------------- 空间 -------------------------------------------------------------------------

        public void checkSpacePermission() {

            User owner = ctx.owner.obj as User;
            if (owner == null) {
                echoRedirect( lang( "exUser" ) );
                return;
            }

            if (ctx.route.isAdmin && owner.Id != ctx.viewer.Id) {
                if (ctx.viewer.IsLogin)
                    echo( lang( "exAccessViolation" ) );
                else
                    echo( lang( "exAccessTimeout" ) );
                return;
            }

            if (ctx.viewer.HasPrivacyPermission( owner, UserPermission.SpaceVisit.ToString() ) == false) {

                if (canPass() == false) {

                    String html = loadHtml( new ForbiddenController().User );
                    echo( html );
                    return;
                }
            }

            String userAppPermission = checkUserApp();
            if (userAppPermission != appOk) echo( userAppPermission );
        }

        private Boolean canPass() {
            FriendController c = new FriendController();
            return passAction( c.AddFriend ) || passAction( c.SaveFriend );
        }

        private Boolean passAction( aActionWithId action ) {
            String fullName = strUtil.Join( action.Method.DeclaringType.FullName, action.Method.Name, "." );
            return ctx.route.getControllerAndActionFullName().Equals( fullName );
        }


        private static readonly String appOk = "";

        private String checkUserApp() {

            if (ctx.app == null) return appOk;

            IAccessStatus securityApp = ctx.app.obj as IAccessStatus;
            if (securityApp == null) return appOk;

            if (ctx.owner.Id == ctx.viewer.Id) return appOk;

            // 基于好友关系验证app权限
            if (securityApp.AccessStatus == (int)AccessStatus.Public)
                return appOk;

            if (securityApp.AccessStatus == (int)AccessStatus.Friend) {
                FriendService friendService = new FriendService();
                if (friendService.IsFriend( ctx.viewer.Id, ctx.owner.Id ) == false) return lang( "exFriendVisitOnly" );
            }
            else if (securityApp.AccessStatus == (int)AccessStatus.Private) {
                if (ctx.viewer.Id != ctx.owner.obj.Id) return lang( "exVisitForbidden" );
            }

            return appOk;
        }

        //--------------------------------------------------------- 群组 -------------------------------------------------------------------------

        private void checkGroupPermission() {

            Group group = ctx.owner.obj as Group;
            if (group == null) return;

            if (ctx.viewer.IsAdministrator()) return;

            // 是否锁定
            if (group.IsLock == 1 && ctx.HttpMethod.Equals( "GET" ) == false) {
                echo( lang( "exGroupLockTip" ) );
                return;
            }

            // 管理功能
            if (ctx.route.isAdmin) {
                checkGroupAdmin();
                return;
            }

            // 秘密群组
            if (group.IsSecret()) {
                checkSecretGroup();
                return;
            }

            checkGroupCreateAndUpdateAction();
        }

        private void checkGroupAdmin() {
            if (ctx.viewer.IsAdministrator()) return;
            if (mgrService.IsGroupOfficer( ctx.viewer.Id, ctx.owner.Id ) == false) echo( lang( "exNoPermission" ) );
        }

        private void checkSecretGroup() {
            if (mgrService.IsGroupMember( ctx.viewer.Id, ctx.owner.Id )) return;
            String html = loadHtml( new Groups.ForbiddenController().Group );
            echo( html );
        }

        private void checkGroupCreateAndUpdateAction() {

            // 提交数据操作
            if (ctx.HttpMethod.Equals( "GET" ) == false) {

                if (ctx.app.Id > 0 && mgrService.IsGroupMember( ctx.viewer.Id, ctx.owner.Id ) == false) {
                    echo( lang( "exGroupMemberOnly" ) );
                }
            }
        }

    }

}
