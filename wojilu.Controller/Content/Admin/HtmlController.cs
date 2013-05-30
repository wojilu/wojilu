/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;

using wojilu.Members.Sites.Domain;
using wojilu.Web.Controller.Content.Utils;
using wojilu.Web.Controller.Content.Htmls;

namespace wojilu.Web.Controller.Content.Admin {

    [App( typeof( ContentApp ) )]
    public class HtmlController : ControllerBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( HtmlController ) );

        public IContentPostService postService { get; set; }
        public IContentSectionService sectionService { get; set; }

        public HtmlController() {
            postService = new ContentPostService();
            sectionService = new ContentSectionService();
        }

        public override void CheckPermission() {
            if (ctx.owner.obj is Site == false) {
                echoError( "没有权限，目前只支持网站生成html页面" );
            }
        }

        public void Index() {

            bindHtmlSetting();

            set( "lnkMakeAll", to( MakeAll ) );
            set( "lnkMakeDetailAll", to( MakeDetailAll ) );
            set( "lnkMakeSectionAll", to( MakeSectionAll ) );
            set( "lnkMakeHome", to( MakeHomePage ) );

            IBlock sectionBlock = getBlock( "sections" );
            List<ContentSection> sections = sectionService.GetByApp( ctx.app.Id );
            foreach (ContentSection section in sections) {

                if (section.ServiceId > 0) continue;

                sectionBlock.Set( "section.Name", section.Title );
                sectionBlock.Set( "lnkMakeSection", to( MakeSection, section.Id ) );
                sectionBlock.Set( "lnkMakeDetail", to( MakeDetailBySection, section.Id ) );
                sectionBlock.Set( "lnkStaticList", clink.toSection( section.Id ) );
                sectionBlock.Next();
            }
        }

        private void bindHtmlSetting() {

            ContentApp app = ctx.app.obj as ContentApp;
            ContentSetting s = app.GetSettingsObj();

            String htmlPath = HomeMaker.GetAppPath( app.Id );
            htmlPath = htmlPath.TrimStart( '/' ).TrimEnd( '/' );

            set( "htmlPath", htmlPath );

            set( "host", ctx.url.SiteAndAppPath );
            set( "editHtmlDirLink", to( EditHtmlPath ) );

            String lnkHtmlHome = strUtil.Join( ctx.url.SiteAndAppPath, htmlPath );

            set( "lnkHtmlHome", lnkHtmlHome );
            set( "lnkOriginalHome", alink.ToApp( app ) );

            String chkAutoHtml = s.IsAutoHtml == 1 ? "checked=\"checked\"" : "";
            set( "chkAutoHtml", chkAutoHtml );
            set( "lnkEditAutoHtml", to( EditAutoHtml ) );
        }

        public void EditAutoHtml() {
            target( SaveAutoHtml );
            ContentApp app = ctx.app.obj as ContentApp;
            ContentSetting s = app.GetSettingsObj();
            String chkAutoHtml = s.IsAutoHtml == 1 ? "checked=\"checked\"" : "";
            set( "chkAutoHtml", chkAutoHtml );
        }

        [HttpPost]
        public void SaveAutoHtml() {

            ContentApp app = ctx.app.obj as ContentApp;
            ContentSetting s = app.GetSettingsObj();
            s.IsAutoHtml = ctx.PostIsCheck( "IsAutoHtml" );

            app.Settings = Json.ToString( s );
            app.update();

            echoToParentPart( lang( "opok" ) );
        }

        public void EditHtmlPath() {

            target( SaveHtmlPath );

            ContentApp app = ctx.app.obj as ContentApp;
            ContentSetting s = app.GetSettingsObj();

            String htmlPath = HomeMaker.GetAppPath( app.Id ).TrimStart( '/' );

            set( "htmlPath", htmlPath );

            set( "host", ctx.url.SiteAndAppPath );

        }

        [HttpPost]
        public void SaveHtmlPath() {

            String htmlPath = strUtil.SubString( ctx.Post( "htmlPath" ), 30 );
            if (strUtil.IsNullOrEmpty( htmlPath )) {
                echoError( "请填写内容" );
                return;
            }

            ContentApp app = ctx.app.obj as ContentApp;
            ContentSetting s = app.GetSettingsObj();

            if (htmlPath == s.StaticPath) {
                echoError( "您没有修改目录名称" );
                return;
            }

            if (HtmlHelper.IsHtmlDirError( htmlPath, ctx.errors )) {
                echoError();
                return;
            }

            s.StaticPath = htmlPath;

            app.Settings = Json.ToString( s );
            app.update();

            echoToParentPart( lang( "opok" ) );

        }

        private int htmlCount = 0;

        public void MakeAll() {
            view( "MakeDone" );

            MakeSectionAll();
            MakeDetailAll();
            MakeHomePage();
            MakeSidebar();

            echo( "生成所有静态页面成功，共 " + htmlCount + " 篇" );
        }

        private void MakeSidebar() {
            HtmlMaker.GetSidebar().Process( ctx.app.Id );
            htmlCount += 1;
        }

        public void MakeHomePage() {
            HtmlMaker.GetHome().Process( ctx.app.Id );
            echo( "生成首页成功" );
        }

        public void MakeSectionAll() {

            view( "MakeDone" );

            ContentApp app = ctx.app.obj as ContentApp;

            MakeSidebar();

            // 最近列表页
            HtmlMaker.GetRecent().ProcessAll( app.Id );
            logger.Info( "make recent html" );

            // 区块列表页
            int count = 0;
            List<ContentSection> sections = sectionService.GetByApp( ctx.app.Id );
            foreach (ContentSection section in sections) {

                int recordCount = postService.CountBySection( section.Id );

                count += HtmlMaker.GetList().ProcessAll( section.Id, recordCount );
                logger.Info( "make section html, sectionId=" + section.Id );
            }

            htmlCount += count;
            echo( "生成所有列表页成功，共 " + htmlCount + " 篇" );
        }

        public void MakeDetailAll() {

            view( "MakeDone" );

            MakeSidebar();

            List<ContentPost> list = postService.GetByApp( ctx.app.Id );
            makeDetail( list );

            htmlCount += list.Count;
        }


        public void MakeSection( int sectionId ) {

            view( "MakeDone" );

            MakeSidebar();

            ContentApp app = ctx.app.obj as ContentApp;
            int recordCount = postService.CountBySection( sectionId );

            int listCount = HtmlMaker.GetList().ProcessAll( sectionId, recordCount );
            echo( "生成列表页成功，共 " + listCount + " 篇" );

        }

        public void MakeDetailBySection( int sectionId ) {

            view( "MakeDone" );

            MakeSidebar();

            List<ContentPost> list = postService.GetAllBySection( sectionId );
            makeDetail( list );
            echo( "生成详细页成功，共 " + list.Count + " 篇" );
        }


        //-------------------------------------------------------------------------------------------

        private void makeDetail( List<ContentPost> list ) {
            foreach (ContentPost post in list) {
                HtmlMaker.GetDetail().Process( post );
                logger.Info( "make detail html, postId=" + post.Id );
            }

            echo( "生成所有详细页成功，共 " + list.Count + " 篇" );

        }


    }

}
