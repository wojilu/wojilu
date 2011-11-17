/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Url;

using wojilu.Members.Groups.Domain;
using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Service;
using wojilu.Members.Groups.Service;
using wojilu.Common.MemberApp;
using wojilu.Common.Menus;
using wojilu.Web.Context;
using wojilu.Common.AppBase;
using wojilu.Apps.Forum.Domain.Security;
using wojilu.Common.Menus.Interface;
using wojilu.Common.MemberApp.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Forum.Interface;


namespace wojilu.Web.Controller.Groups {

    public class GroupUtil {

        public IMemberAppService appService { get; set; }
        public IMenuService menuService { get; set; }
        public IForumService forumService { get; set; }

        public GroupUtil() {
            appService = new GroupAppService();
            menuService = new GroupMenuService();
            forumService = new ForumService();
        }

        public void CreateAppAndMenu( Group group, MvcContext ctx ) {
            
            // 添加程序
            IMemberApp forumApp = appService.Add( (User)ctx.viewer.obj, group, lang.get( "groupBoard" ), 1, AccessStatus.Public );

            // 论坛
            ForumApp app = forumService.GetById( forumApp.AppOid );
            ForumPermission.AddOwnerAdminPermission( app );

            // 添加一个论坛板块
            ForumBoard board = new ForumBoard();
            board.Name = lang.get( "groupBoard" );
            board.ParentId = 0;
            board.AppId = forumApp.AppOid;

            board.Creator = (User)ctx.viewer.obj;
            board.CreatorUrl = ctx.viewer.obj.Url;

            board.OwnerId = group.Id;
            board.OwnerUrl = group.Url;
            board.OwnerType = typeof( Group ).FullName;
            board.Ip = ctx.Ip;

            board.Security = app.Security;

            db.insert( board );

            // 添加menuUrl
            String forumUrl = UrlConverter.clearUrl( board, ctx );
            menuService.AddMenuByApp( forumApp, forumApp.Name, "default", forumUrl );

        }

    }
}
