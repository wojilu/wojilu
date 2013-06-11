/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;
using wojilu.Web.Controller.Common.Installers;
using wojilu.Members.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Common.MemberApp.Interface;
using wojilu.Apps.Content.Domain;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller {

    public class CmsInstaller : BaseInstaller {

        public IMemberApp CreateNews( MvcContext ctx, String appName, String appUrl ) {
            String themeId = "3c715507-29ba-4436-b71d-11f6b08e12f7";
            return createContentApp( ctx, appName, appUrl, themeId );
        }

        public IMemberApp CreatePortal( MvcContext ctx, String appName, String appUrl ) {
            String themeId = "78349067-6e1f-4639-92ea-4acc142470ed";
            return createContentApp( ctx, appName, appUrl, themeId );
        }

        private IMemberApp createContentApp( MvcContext ctx, String appName, String appUrl, String themeId ) {
            IMember owner = ctx.owner.obj;

            NewsInstaller x = ObjectContext.Create<NewsInstaller>();
            IMemberApp mapp = x.Install( ctx, owner, appName, wojilu.Common.AppBase.AccessStatus.Public, themeId, appUrl );

            // 初始化权限，否则无法访问
            base.initAppPermission( mapp );

            return mapp;
        }

        //----------------------------------

    }

}
