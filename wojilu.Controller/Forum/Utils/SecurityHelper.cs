using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;
using wojilu.Common.Security;
using wojilu.Web.Mvc;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Forum.Service;
using wojilu.Common.Security.Utils;
using System.Collections;
using wojilu.Apps.Forum.Domain;

namespace wojilu.Web.Controller.Forum.Utils {

    public class SecurityHelper {

        public static Boolean Check( ControllerBase controller, ISecurity objSecurity ) {

            MvcContext ctx = controller.ctx;

            if (!hasAction( objSecurity, ctx )) {
                controller.echo( lang.get( "exNoPermission" ) );
                return false;
            }

            return true;
        }


        private static Boolean hasAction( ISecurity objSecurity, MvcContext ctx ) {

            // 未提供权限配置的页面通过
            if (objSecurity == null) return true;

            SecurityTool securityTool = ForumSecurityService.GetSecurityTool( objSecurity, ctx );// objSecurity.SecurityTool;


            // 不需要权限管理的页面通过
            if (securityTool.IsForbiddenAction( ctx.route.getControllerAndActionPath() ) == false) return true;

            // 空页面——通过
            String currentPath = ctx.url.Path;
            if (strUtil.IsNullOrEmpty( currentPath )) return true;

            // 编辑权限例外：用户可以编辑自己的帖子

            // 只要系统角色，或论坛角色之一具有权限，则用户具有权限（当用户具有多重身份之时）

            // 1、获取用户的角色

            //系统角色
            SiteRole role = ((User)ctx.viewer.obj).Role;
            IList actions = securityTool.GetActionsByRole( role );
            if (hasAction_private( actions, ctx )) return true;

            // 2、获取用户在特定owner中的角色
            if (ctx.owner.obj.GetType() != typeof( Site )) {
                IRole roleInOwner = ctx.owner.obj.GetUserRole( ctx.viewer.obj );
                IList ownerRoleActions = securityTool.GetActionsByRole( roleInOwner );
                if (hasAction_private( ownerRoleActions, ctx )) return true;
            }

            // 3、获取用户的等级
            SiteRank rank = ((User)ctx.viewer.obj).Rank;
            if (rank.Id > 0) {
                actions = securityTool.GetActionsByRole( rank );
                if (hasAction_private( actions, ctx )) return true;
            }

            // 4、是否在论坛担任角色

            if (objSecurity is ForumBoard) {

                ModeratorService moderatorService = new ModeratorService();

                if (moderatorService.IsModerator( objSecurity as ForumBoard, (User)ctx.viewer.obj )) {
                    IList moderatorActions = securityTool.GetActionsByRole( ForumRole.Moderator );
                    if (hasAction_private( moderatorActions, ctx )) return true;
                }

            }

            return false;
        }

        private static Boolean hasAction_private( IList actions, MvcContext ctx ) {

            foreach (ISecurityAction action in actions) {

                if (action == null) continue;
                if (action.Url.IndexOf( ctx.route.getControllerAndActionPath() ) >= 0) return true;

            }
            return false;
        }

    }

}
