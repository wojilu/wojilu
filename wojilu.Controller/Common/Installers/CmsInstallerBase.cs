/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Url;
using wojilu.Web.Context;

using wojilu.Common.AppBase;
using wojilu.Common.AppInstall;
using wojilu.Common.MemberApp.Interface;
using wojilu.Common.Menus.Interface;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Service;

using wojilu.Members.Interface;
using wojilu.Members.Sites.Service;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Service;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Groups.Domain;
using wojilu.Members.Groups.Service;
using wojilu.Apps.Content.Enum;
using wojilu.Apps.Content.Interface;
using wojilu.Common;

namespace wojilu.Web.Controller.Common.Installers {

    public abstract class CmsInstallerBase {

        public AppInstallerService installerService { get; set; }
        public IMemberAppService appService { get; set; }
        public IMenuService menuService { get; set; }
        public IContentSectionService sectionService { get; set; }
        public IContentCustomTemplateService tplService { get; set; }

        protected User user;
        protected IMember owner;
        protected String appName;
        protected MvcContext ctx;
        protected ContentApp app;

        public CmsInstallerBase() {
            installerService = new AppInstallerService();
            appService = new UserAppService();
            menuService = new UserMenuService();
            sectionService = new ContentSectionService();
            tplService = new ContentCustomTemplateService();
        }

        public IMemberApp Install( MvcContext ctx, IMember owner, String appName, AccessStatus accessStatus ) {


            this.ctx = ctx;
            this.appName = appName;
            this.user = ctx.viewer.obj as User;
            this.owner = owner;

            setService( ctx );

            IMemberApp app = createPortalApp();
            createLayout();

            return app;
        }

        protected abstract IMemberApp createPortalApp();
        protected abstract void createLayout();

        private void setService( MvcContext ctx ) {

            this.appService = ServiceMap.GetUserAppService( this.owner.GetType() );
            this.menuService = ServiceMap.GetMenuService( this.owner.GetType() );

        }

        protected void createPost( ContentSection s, String title ) {
            createPost( s, title, "", "", false );
        }

        protected void createText( ContentSection s, String content ) {
            createPost( s, s.Title + "_文本", content, "", "", false );

        }

        protected void createImgS( ContentSection s, String title, String imgUrl ) {
            createPost( s, title, imgUrl, "", false );
        }


        protected void createImgBig( ContentSection s, String title, String imgUrl ) {
            createPost( s, title, imgUrl, "", true );
        }

        protected void createVideo( ContentSection s, String title, String imgUrl, String videoUrl ) {
            createPost( s, title, imgUrl, videoUrl, true );
        }

        protected void createPost( ContentSection s, String title, String imgUrl, String videoUrl, Boolean isBigImg ) {
            createPost( s, title, title + "——内容", imgUrl, videoUrl, isBigImg );
        }

        protected void createPoll( ContentSection s, String title, List<String> options ) {

            ContentPoll p = new ContentPoll();
            p.Title = title;
            p.Question = "";
            p.Answer = getAnswerString( options );

            IMember o = this.owner;
            User u = this.user;

            p.OwnerId = o.Id;
            p.OwnerType = o.GetType().FullName;
            p.OwnerUrl = o.Url;

            p.Creator = u;
            p.CreatorUrl = u.Url;

            p.AppId = s.AppId;



            new ContentPollService().CreatePoll( s.Id, p );

        }

        protected string getAnswerString( List<String> options ) {
            String str = "";
            foreach (String s in options) str += s + Environment.NewLine;
            return str.Trim();
        }


        protected void createImgS( ContentSection s, String title, String content, String imgUrl ) {
            createPost( s, title, content, imgUrl, "", false );
        }

        protected void createPostC( ContentSection s, String title, String cat ) {

            createPost( s, title, "[" + cat + "] " + title, title, "", "", false );
        }

        //----------------------------------------------------------------------------
        protected void createImgHome( ContentSection s, String title, String titleHome, int width, int height, String imgUrl ) {

            createPost( s, title, titleHome, title, imgUrl, "", 0, width, height );
        }
        //----------------------------------------------------------------------------


        protected void createImgHome( ContentSection s, String title, String titleHome, String imgUrl ) {

            createPost( s, title, titleHome, title, imgUrl, "", true );
        }

        protected void createImgPost( ContentSection s, String title, String content, String imgUrl ) {

            createPost( s, title, title, content, imgUrl, "", false, PostCategory.ImgPost );
        }

        protected void createPost( ContentSection s, String title, String content, String imgUrl, String videoUrl, Boolean isBigImg ) {
            createPost( s, title, "", content, imgUrl, videoUrl, isBigImg );
        }

        protected void createPost( ContentSection s, String title, String titleHome, String content, String imgUrl, String videoUrl, Boolean isBigImg ) {
            createPost( s, title, titleHome, content, imgUrl, videoUrl, isBigImg, 0 );

        }

        //---------------------------------

        protected void createPost( ContentSection s, String title, String titleHome, String content, String imgUrl, String videoUrl, Boolean isBigImg, int categoryId ) {
            int width=0;
            int height=0;

            if (strUtil.HasText( imgUrl )) {

                if (isBigImg) {
                    width = 290;
                    height = 210;
                }
                else {
                    width = 120;
                    height = 90;
                }
            }

            createPost( s, title, titleHome, content, imgUrl, videoUrl, categoryId, width, height );

        }

        protected void createPost( ContentSection s, String title, String titleHome, String content, String imgUrl, String videoUrl, int categoryId, int width, int height ) {

            IMember o = this.owner;
            User u = this.user;

            ContentPost p = new ContentPost();

            p.OwnerId = o.Id;
            p.OwnerType = o.GetType().FullName;
            p.OwnerUrl = o.Url;

            p.Creator = u;
            p.CreatorUrl = u.Url;

            p.AppId = s.AppId;
            p.PageSection = s;

            p.Author = "作者";

            p.Title = title;
            p.Content = content;
            p.TitleHome = titleHome;

            p.ImgLink = imgUrl;
            p.SourceLink = videoUrl;


            if (strUtil.HasText( videoUrl )) {
                p.CategoryId = PostCategory.Video;
                p.TypeName = typeof( ContentVideo ).FullName;
                p.Width = 290;
                p.Height = 235;
            }
            else if (strUtil.HasText( imgUrl )) {

                if (categoryId > 0)
                    p.CategoryId = categoryId;
                else
                    p.CategoryId = PostCategory.Img;

                //if (isBigImg) {
                //    p.Width = 290;
                //    p.Height = 210;
                //}
                //else {
                //    p.Width = 120;
                //    p.Height = 90;
                //}

                p.Width = width;
                p.Height = height;
            }

            p.insert();
        }

        //---------------------------------


        /// <summary>
        /// 安装app
        /// </summary>
        /// <returns></returns>
        protected IMemberApp createApp() {

            int installerId = 4;
            AppInstaller installer = installerService.GetById( installerId );

            IMember owner = this.owner;
            User creator = this.user;
            String name = appName;

            IMemberApp mapp = appService.Add( creator, owner, name, installer.Id, AccessStatus.Public );
            String appUrl = UrlConverter.clearUrl( mapp, ctx, this.owner );

            IMenu menu = menuService.AddMenuByApp( mapp, name, "", appUrl );

            ContentApp newApp = ContentApp.findById( mapp.AppOid );
            this.app = newApp;

            return mapp;
        }

        /// <summary>
        /// 创建手动添加区块
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sectionType"></param>
        /// <param name="layoutStr"></param>
        /// <returns></returns>
        protected ContentSection createSection( String name, Type sectionType, String layoutStr ) {

            ContentSection section = new ContentSection();

            int rowId = cvt.ToInt( layoutStr.Substring( 0, layoutStr.Length - 1 ) );
            int columnId = cvt.ToInt( layoutStr.Substring( layoutStr.Length - 1, 1 ) );

            section.AppId = this.app.Id;
            section.RowId = rowId;
            section.ColumnId = columnId;
            section.SectionType = sectionType.FullName;
            section.Title = name;

            sectionService.Insert( section );

            return section;
        }

        /// <summary>
        /// 创建聚合区块
        /// </summary>
        /// <param name="title"></param>
        /// <param name="serviceId"></param>
        /// <param name="templateId"></param>
        /// <param name="layoutStr"></param>
        /// <returns></returns>
        protected ContentSection createSectionMashup( String title, int serviceId, int templateId, String layoutStr ) {

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
            return section;
        }

        protected ContentCustomTemplate createTemplate( String templateBody ) {

            ContentCustomTemplate cs = new ContentCustomTemplate();
            cs.Content = templateBody;
            cs.OwnerId = this.app.OwnerId;
            cs.OwnerType = this.app.OwnerType;
            cs.OwnerUrl = this.app.OwnerUrl;
            cs.Name = "" + DateTime.Now.ToShortDateString();
            cs.Creator = this.user;
            tplService.Insert( cs );

            return cs;

        }



    }
}
