/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;
using System.Reflection;

using wojilu.Web.Mvc;
using wojilu.Web.Context;

using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Service;
using wojilu.Members.Users.Domain;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Sites.Service;
using wojilu.Common.Security;
using wojilu.Common.AppBase;
using wojilu.Common.Security.Utils;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Web.Controller.Forum.Utils {

    public class SecurityHelper {

        private static readonly ILog logger = LogManager.GetLogger( typeof( SecurityHelper ) );

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

            SecurityTool securityTool = GetSecurityTool( objSecurity, ctx );// objSecurity.SecurityTool;


            // 不需要权限管理的页面通过
            if (securityTool.IsForbiddenAction( ctx.route.getControllerAndActionPath() ) == false) return true;

            // 空页面通过
            String currentPath = ctx.url.Path;
            if (strUtil.IsNullOrEmpty( currentPath )) return true;

            // 【规则】只要系统角色，或论坛角色之一具有权限，则用户具有权限（当用户具有多重身份之时）

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


        //----------------------------------------------------------------------

        public static IList GetTopicAdminCmds( User user, ForumBoard board, MvcContext ctx ) {

            IList results = new ArrayList();

            // 1、获取用户的角色
            SecurityTool tool = GetSecurityTool( board, ctx );
            IList actions = tool.GetActionsByRole( user.Role );
            addAdminActionsToResults( actions, results );

            // 2、获取用户的等级
            if (user.RankId > 0) {
                actions = tool.GetActionsByRole( user.Rank );
                addAdminActionsToResults( actions, results );
            }

            // 3、owner的角色
            if (ctx.owner.obj.GetType() != typeof( Site )) {
                IRole roleInOwner = ctx.owner.obj.GetUserRole( user );
                actions = tool.GetActionsByRole( roleInOwner );
                addAdminActionsToResults( actions, results );
            }


            // 3、版主
            ModeratorService moderatorService = new ModeratorService();
            if (moderatorService.IsModerator( board, user )) {

                IList moderatorActions = tool.GetActionsByRole( ForumRole.Moderator );
                addAdminActionsToResults( moderatorActions, results );
            }

            return results;
        }

        private static void addAdminActionsToResults( IList actions, IList results ) {
            foreach (SecurityAction a in actions) {
                if (a.IsTopicAdmin == 1 && results.Contains( a ) == false) results.Add( a );
            }
        }

        //----------------------------------------------------------------------


        public static Boolean HasAction( User user, ISecurity objSecurity, ISecurityAction action, MvcContext ctx ) {

            SecurityTool securityTool = GetSecurityTool( objSecurity, ctx );

            Boolean hasAction = securityTool.HasAction( user.Role, action );
            if (hasAction) return true;

            hasAction = securityTool.HasAction( user.Rank, action );
            if (hasAction) return true;

            if (ctx.owner.obj.GetType() != typeof( Site )) {
                IRole roleInOwner = ctx.owner.obj.GetUserRole( user );
                hasAction = securityTool.HasAction( roleInOwner, action );
                if (hasAction) return true;
            }

            if (new ModeratorService().IsModerator( objSecurity as ForumBoard, user )) {
                hasAction = securityTool.HasAction( ForumRole.Moderator, action );
                if (hasAction) return true;
            }

            return false;
        }


        public static SecurityTool GetSecurityTool( ISecurity f, MvcContext ctx ) {

            IList forumRoles = ForumRole.GetAll();

            IList ownerRoles;
            if (ctx.owner.obj.GetType() != typeof( Site ))
                ownerRoles = ctx.owner.obj.GetRoles();
            else
                ownerRoles = new ArrayList();

            IList siteRoles = new SiteRoleService().GetRoleAndRank();

            IList allRoles = new RoleMerger()
                .Add( forumRoles )
                .Add( ownerRoles )
                .Add( siteRoles )
                .GetResults();

            SecurityTool tool = new SecurityTool( f, new SecurityAction(), allRoles );
            return tool;
        }


    }


}
