/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;
using wojilu.Apps.Forum.Domain;
using wojilu.Web.Mvc;
using wojilu.Members.Users.Domain;
using wojilu.Members.Sites.Domain;
using wojilu.Common.AppInstall;
using wojilu.Members.Interface;
using wojilu.Common.MemberApp.Interface;

namespace wojilu.Web.Controller {

    public class ForumInstaller : BaseInstaller {

        public IAppInstallerService installService { get; set; }
        public ForumInstaller() {
            installService = new AppInstallerService();
        }

        public void Init( MvcContext ctx, String appName, String fUrl ) {

            ForumApp forum = createFirstForum( ctx, appName );

            updateSettings( forum );

            base.AddMenu( ctx, appName, alink.ToApp( forum ), fUrl );
        }

        private void updateSettings( ForumApp forum ) {

            ForumSetting s = forum.GetSettingsObj();
            // 配置：隐藏头条新帖
            s.IsHideTop = 1;

            forum.Settings = Json.ToString( s );
            forum.update();
        }

        private ForumApp createFirstForum( MvcContext ctx, String appName ) {

            IMember owner = ctx.owner.obj;
            User creator = ctx.viewer.obj as User;

            IMemberApp mapp = installService.Install( typeof( ForumApp ), owner, creator, appName );

            // 初始化权限，否则无法访问
            base.initAppPermission( mapp );
            ForumApp app = ForumApp.findById( mapp.AppOid );

            createBoardList( ctx, app );

            return app;
        }

        private void createBoardList( MvcContext ctx, ForumApp app ) {
            ForumBoard category1 = createBoard( ctx, app, null, "新闻" );
            ForumBoard category2 = createBoard( ctx, app, null, "娱乐" );

            createBoard( ctx, app, category1, "国际新闻" );
            createBoard( ctx, app, category1, "国内新闻" );
            createBoard( ctx, app, category1, "科技新闻" );

            createBoard( ctx, app, category2, "电影" );
            createBoard( ctx, app, category2, "音乐" );
            createBoard( ctx, app, category2, "游戏" );
        }

        private ForumBoard createBoard( MvcContext ctx, ForumApp app, ForumBoard board, String name ) {
            ForumBoard bd = new ForumBoard();
            bd.Name = name;
            bd.Description = name + " 的简介";

            if (board != null) {
                bd.ParentId = board.Id;
            }

            bd.AppId = app.Id;
            bd.Creator = ctx.viewer.obj as User;
            bd.CreatorUrl = bd.Creator.Url;
            setOwner( bd, app );
            bd.insert();

            return bd;
        }

        private void setOwner( ForumBoard bd, ForumApp app ) {
            bd.OwnerId = app.OwnerId;
            bd.OwnerType = app.OwnerType;
            bd.OwnerUrl = app.OwnerUrl;
        }

    }

}
