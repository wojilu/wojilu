/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Members.Users.Domain;

using wojilu.Common.Skins;
using wojilu.Members.Sites.Domain;
using wojilu.Common.Feeds.Service;
using wojilu.Common.Pages.Service;
using wojilu.Members.Users.Interface;
using wojilu.Common.Feeds.Interface;
using wojilu.Common.Pages.Interface;
using wojilu.Web.Controller.Helpers;

namespace wojilu.Web.Controller {

    public class SiteInitController : ControllerBase {

        private SiteInitHelper siteInitHelper;

        public SiteInitController() {
            siteInitHelper = ObjectContext.Create<SiteInitHelper>();
        }

        public void Index() {

            if (isInit() == false) {
                String view = loadHtml( InitData );
                content( view );
            }
            else if (hasMember() == false) {
                String view = loadHtml( Register );
                content( view );
            }
            else {
                String view = loadHtml( Initok );
                content( view );
            }
        }

        public void SelectDb() {
            set( "link", to( Init ) );
        }

        [NonVisit]
        public void InitData() {
            set( "link", to( Init ) );
            set( "selectDbLink", to( SelectDb ) );

            String dbType = db.getDatabaseType();
            if (strUtil.IsNullOrEmpty( dbType )) {
                throw new Exception( "数据库配置错误，请参考官方配置示例。" );
            }

            set( "dbType", db.getDatabaseType() );
        }

        [NonVisit]
        public void Register() {
            set( "link", to( new RegisterController().Register ) );
        }

        [NonVisit]
        public void Initok() {

            set( "addAppLink", to( new Admin.AppController().Select ) );
            set( "addMenuLink", to( new Admin.Sys.DashboardController().Links ) );
        }

        private Boolean hasMember() {
            return User.count() > 0;
        }

        [HttpPost, DbTransaction]
        public void Init() {

            Boolean isInit = siteInitHelper.InitSite();


            if (isInit) {
                echoRedirect( "初始化成功", Index );
            }
            else {
                echoRedirect( "系统已经初始化过", Index );
            }
        }

        private Boolean isInit() {
            return siteInitHelper.HasInit();
        }


    }
}

