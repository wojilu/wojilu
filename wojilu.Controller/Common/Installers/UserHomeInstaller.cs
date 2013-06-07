/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Url;
using wojilu.Web.Context;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Service;

using wojilu.Common.AppBase;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppInstall;
using wojilu.Common.MemberApp.Interface;
using wojilu.Common.Menus.Interface;

using wojilu.Members.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Service;


namespace wojilu.Web.Controller.Common.Installers {
    
    public class UserHomeInstaller : IAppInstaller {

        private ContentSectionService sectionService { get; set; }
        private AppInstallerService appinfoService { get; set; }
        private UserAppService appService { get; set; }
        private UserMenuService menuService { get; set; }

        private MvcContext ctx;
        private LanguageSetting lbl;
        private User user;
        private ContentApp app;

        public UserHomeInstaller() {
            sectionService = new ContentSectionService();
            appService = new UserAppService();
            menuService = new UserMenuService();
            appinfoService = new AppInstallerService();

            lbl = lang.getByApp( typeof( ContentApp ) );
        }

        public IMemberApp Install( MvcContext ctx, IMember owner, String appName, AccessStatus accessStatus, String themeId, String friendlyUrl ) {

            this.ctx = ctx;

            User user = ctx.owner.obj as User;
            IMemberApp memberApp = createPortal( user );

            return memberApp;
        }

        public IMemberApp Install( MvcContext ctx, User user, String appName, AccessStatus accessStatus ) {

            this.ctx = ctx;

            IMemberApp memberApp = createPortal( user );

            return memberApp;
        }

        private String alang( String key ) {
            return lbl.get( key );
        }

        private IMemberApp createPortal( User user ) {

            this.user = user;

            IMemberApp memberApp =  createApp();
            createLayout();

            this.createSection( alang( "myface" ), 12, 14, "11" );
            this.createSection( alang( "myprofile" ), 5, 10, "11" );
            this.createSection( alang( "recentVisitor" ), 8, 13, "11" );


            this.createSection( alang( "mymicroblog" ), 13, 15, "12" );
            this.createSection( alang( "myfeed" ), 7, 12, "12" );
            this.createSection( alang( "myblog" ), 11, 17, "12" );
            this.createSection( alang( "feedback" ), 14, 16, "12" );

            this.createSection( alang( "myfriends" ), 9, 13, "13" );
            this.createSection( alang( "myphoto" ), 10, 3, "13" );

            return memberApp;
        }

        private IMemberApp createApp() {

            int appInfoId = 4;
            AppInstaller info = appinfoService.GetById( appInfoId );

            IMember owner = this.user;
            User creator = this.user;
            String name = lang.get( "homepage" );

            IMemberApp myuserApp = appService.Add( creator, owner, name, info.Id, AccessStatus.Public );
            //String appUrl = UrlConverter.clearUrl( myuserApp, ctx );
            String appUrl = UrlConverter.clearUrl( myuserApp, ctx, owner );

            IMenu menu = menuService.AddMenuByApp( myuserApp, name, "", appUrl );
            menu.Url = "default";
            menu.OrderId = 99;
            menuService.Update( menu );

            this.app = ContentApp.findById( myuserApp.AppOid );

            return myuserApp;
        }

        private void createLayout() {

            String style = "#row1_column1 {width:25%;margin-right:8px;margin-left:8px;}" + Environment.NewLine
                + "#row1_column2 {width:50%;}" + Environment.NewLine
                + "#row1_column3 {width:21%;margin-left:8px;margin-right:8px;}";

            app.Style = style;
            app.Layout = "3";
            db.update( app );

        }

        private void createSection( String title, int serviceId, int templateId, String layoutStr ) {

            ContentSection section = new ContentSection();

            int rowId = cvt.ToInt( layoutStr.Substring( 0, layoutStr.Length - 1 ) );
            int columnId = cvt.ToInt( layoutStr.Substring( layoutStr.Length - 1, 1 ) );

            section.AppId = this.app.Id;
            section.RowId = rowId;
            section.ColumnId = columnId;
            section.ServiceId = serviceId;
            section.TemplateId = templateId;
            section.Title = title;
            section.MoreLink = "";

            sectionService.Insert( section );

        }

    }

}
