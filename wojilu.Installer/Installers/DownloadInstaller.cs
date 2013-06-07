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
using wojilu.Apps.Download.Domain;
using wojilu.Common.MemberApp.Interface;

namespace wojilu.Web.Controller {

    public class DownloadInstaller : BaseInstaller {

        public IAppInstallerService installService { get; set; }
        public DownloadInstaller() {
            installService = new AppInstallerService();
        }

        public void Init( MvcContext ctx, String appName, String fUrl ) {
            DownloadApp downloadApp = createApp( ctx, appName );
            base.AddMenu( ctx, appName, alink.ToApp( downloadApp ), fUrl );
        }


        private DownloadApp createApp( MvcContext ctx, String appName ) {

            IMember owner = ctx.owner.obj;
            User creator = ctx.viewer.obj as User;

            IMemberApp app = installService.Install( typeof( DownloadApp ), owner, creator, appName ) as IMemberApp;

            // 初始化权限，否则无法访问
            base.initAppPermission( app );

            DownloadApp obj = DownloadApp.findById( app.AppOid );

            return obj;
        }


    }

}
