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
        public IContentPostService postService { get; set; }
        public IContentCustomTemplateService tplService { get; set; }

        protected User user;
        protected IMember owner;
        protected String appName;
        protected String menuFUrl;

        protected MvcContext ctx;
        protected ContentApp app;

        protected String themeId;

        public CmsInstallerBase() {
            installerService = new AppInstallerService();
            appService = new UserAppService();
            menuService = new UserMenuService();
            sectionService = new ContentSectionService();
            tplService = new ContentCustomTemplateService();
            postService = new ContentPostService();
        }

        /// <summary>
        /// 带时间的简单列表
        /// </summary>
        /// <returns></returns>
        protected String getListViewWithTime() {
            return @"<ul class=""ul1"" style=""font-size:12px;"">
<!-- BEGIN list -->
<li>
<div style=""float:left;width:250px;overflow:hidden;white-space:nowrap;text-overflow:clip;""><a href=""#{post.Url}"" style=""#{post.TitleCss}"" title=""#{post.TitleFull}"" style=""#{post.TitleCss}"">#{post.Title}</a></div>
<div style=""float:right"" class=""note"">#{post.CreatedDay}</div>
</li>
<!-- END list -->
</ul>";
        }

        /// <summary>
        /// 带时间的普通列表
        /// </summary>
        /// <returns></returns>
        protected String getNormalViewWithTime() {
            return @"<div class="""">
<div><!-- BEGIN img -->
    <div class=""strong""><a href=""#{ipost.Url}"" title=""#{ipost.TitleFull}"" style=""#{ipost.TitleCss}"">#{ipost.Title}</a></div>
    <div class=""clear""></div>
    <a href=""#{ipost.Url}""><img src=""#{ipost.ImgUrl}"" style=""float:left;margin:5px 8px 8px 0px;width:#{ipost.Width}px;height:#{ipost.Height}px;"" /></a>
    <div class=""font12"">#{ipost.Content} <a href=""#{ipost.Url}"" class=""left10"">[_{detail}]</a></div>
    <!-- END img -->
    <div class=""clear""></div>
</div>   

<ul class=""ul1"" style=""margin:10px 0px 10px 0px;font-size:12px;"">
<!-- BEGIN list -->
<div style=""float:left;width:250px;overflow:hidden;white-space:nowrap;text-overflow:clip;""><a href=""#{post.Url}"" style=""#{post.TitleCss}"" title=""#{post.TitleFull}"" style=""#{post.TitleCss}"">#{post.Title}</a></div>
<div style=""float:right"" class=""note"">#{post.CreatedDay}</div><!-- END list -->
</ul>
</div>";
        }

        public IMemberApp Install( MvcContext ctx, IMember owner, String appName, AccessStatus accessStatus, String themeId, String friendlyUrl ) {


            this.ctx = ctx;
            this.appName = appName;
            this.menuFUrl = friendlyUrl;
            this.user = ctx.viewer.obj as User;
            this.owner = owner;

            this.themeId = themeId;

            setService( ctx );


            // 真正初始化过程

            initTheme();
            IMemberApp app = createPortalApp();
            createLayout();

            return app;
        }


        protected virtual void initTheme() {
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


        protected ContentPost createImgS( ContentSection s, String title, String content, String imgUrl ) {
            return createPost( s, title, content, imgUrl, "", false );
        }

        protected ContentPost createPostC( ContentSection s, String title, String cat ) {
            return createPost( s, title, "[" + cat + "] " + title, title, "", "", false );
        }

        //----------------------------------------------------------------------------
        protected ContentPost createImgHome( ContentSection s, String title, String titleHome, int width, int height, String imgUrl ) {
            return createPost( s, title, titleHome, title, imgUrl, "", 0, width, height );
        }
        //----------------------------------------------------------------------------


        protected ContentPost createImgHome( ContentSection s, String title, String titleHome, String imgUrl ) {
            return createPost( s, title, titleHome, title, imgUrl, "", true );
        }

        protected ContentPost createImgPost( ContentSection s, String title, String content, String imgUrl ) {
            return createPost( s, title, title, content, imgUrl, "", false, PostCategory.ImgPost );
        }

        protected ContentPost createPost( ContentSection s, String title, String content, String imgUrl, String videoUrl, Boolean isBigImg ) {
            return createPost( s, title, "", content, imgUrl, videoUrl, isBigImg );
        }

        protected ContentPost createPost( ContentSection s, String title, String titleHome, String content, String imgUrl, String videoUrl, Boolean isBigImg ) {
            return createPost( s, title, titleHome, content, imgUrl, videoUrl, isBigImg, 0 );
        }

        //---------------------------------

        protected ContentPost createPost( ContentSection s, String title, String titleHome, String content, String imgUrl, String videoUrl, Boolean isBigImg, int categoryId ) {
            int width = 0;
            int height = 0;

            if (strUtil.HasText( imgUrl )) {

                if (isBigImg) {
                    width = 290;
                    height = 210;
                } else {
                    width = 120;
                    height = 90;
                }
            }

            return createPost( s, title, titleHome, content, imgUrl, videoUrl, categoryId, width, height );
        }

        protected ContentPost createPost( ContentSection s, String title, String titleHome, String content, String imgUrl, String videoUrl, int categoryId, int width, int height ) {

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
            } else if (strUtil.HasText( imgUrl )) {

                if (categoryId > 0) {
                    p.CategoryId = categoryId;
                } else {
                    p.CategoryId = PostCategory.Img;
                }

                p.Width = width;
                p.Height = height;
            }

            postService.Insert( p, "" );

            return p;
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

            IMenu menu = menuService.AddMenuByApp( mapp, name, this.menuFUrl, appUrl );

            ContentApp newApp = ContentApp.findById( mapp.AppOid );
            this.app = newApp;

            return mapp;
        }

        protected ContentSection createSection( String name, String sectionType, String layoutStr ) {
            return createSection( name, sectionType, layoutStr, null );
        }

        /// <summary>
        /// 创建手动添加区块
        /// </summary>
        /// <param name="name">区块的名称</param>
        /// <param name="sectionType">区块的类型</param>
        /// <param name="layoutStr">所属行和所属列</param>
        /// <returns></returns>
        protected ContentSection createSection( String name, Type sectionType, String layoutStr ) {

            return createSection( name, sectionType, layoutStr, null );
        }

        protected ContentSection createSection( String name, Type sectionType, String layoutStr, ContentCustomTemplate ct ) {
            return createSection( name, sectionType.FullName, layoutStr, ct );
        }

        /// <summary>
        /// 创建手动添加区块
        /// </summary>
        /// <param name="name">区块的名称</param>
        /// <param name="sectionType">区块的类型</param>
        /// <param name="layoutStr">所属行和所属列</param>
        /// <param name="ct">自定义模板</param>
        /// <returns></returns>
        protected ContentSection createSection( String name, String sectionType, String layoutStr, ContentCustomTemplate ct ) {

            ContentSection section = new ContentSection();

            int rowId = cvt.ToInt( layoutStr.Substring( 0, layoutStr.Length - 1 ) );
            int columnId = cvt.ToInt( layoutStr.Substring( layoutStr.Length - 1, 1 ) );

            section.AppId = this.app.Id;
            section.RowId = rowId;
            section.ColumnId = columnId;
            section.SectionType = sectionType;
            section.Title = name;

            if (ct != null) section.CustomTemplateId = ct.Id;

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
