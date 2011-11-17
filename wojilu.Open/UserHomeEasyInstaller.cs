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


namespace wojilu.Open {
    
    public class UserHomeEasyInstaller  {

        private ContentApp app;


        public IMemberApp Install( User user, AccessStatus accessStatus ) {
            return createPortal( user );
        }


        private IMemberApp createPortal( User user ) {


            IMemberApp memberApp =  createApp( user );
            createLayout();

            this.createSection( "我的头像", 12, 14, "11" );
            this.createSection( "我的信息", 5, 10, "11" );
            this.createSection( "最近访客", 8, 13, "11" );


            this.createSection( "我的状态", 13, 15, "12" );
            this.createSection( "我的动态", 7, 12, "12" );
            this.createSection( "我的博客", 11, 17, "12" );
            this.createSection( "留言板", 14, 16, "12" );

            this.createSection( "我的好友", 9, 13, "13" );
            this.createSection( "我的图片", 10, 3, "13" );

            return memberApp;
        }

        private IMemberApp createApp( User user ) {


            UserApp ua = AppService.addApp( user, "主页", 4, "wojilu.Apps.Content.Domain.ContentApp", true );

            this.app = ContentApp.findById( ua.AppOid );

            return ua;
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

            section.insert();
        }


    }

}
