/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;

using wojilu.Web.UI;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.SOA;
using wojilu.SOA.Controls;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;
using wojilu.Web.Controller.Content.Section;
using wojilu.Apps.Reader.Domain;
using wojilu.Apps.Reader.Interface;
using wojilu.Apps.Reader.Service;
using wojilu.Members.Sites.Domain;
using wojilu.Web.Controller.Content.Utils;
using wojilu.DI;

namespace wojilu.Web.Controller.Content.Admin {

    [App( typeof( ContentApp ) )]
    public partial class ContentSectionController : ControllerBase {

        public IContentSectionService sectionService { get; set; }
        public IContentSectionTypeService sectionTypeService { get; set; }
        public IContentSectionTemplateService templateService { get; set; }
        public IFeedSourceService feedService { get; set; }

        public ContentSectionController() {
            sectionService = new ContentSectionService();
            templateService = new ContentSectionTemplateService();
            sectionTypeService = new ContentSectionTypeService();
            feedService = new FeedSourceService();
        }

        public void Add( int columnId ) {

            target( Create, columnId );

            // 默认选中 ListSectionController 区块
            set( "sectionType", TemplateController.getSectionTypeName() );

            set( "templateTypeLink", to( new TemplateController().List ) );
            set( "layout", columnId );
        }

        [HttpPost, DbTransaction]
        public void Create( int columnId ) {
            ContentSection section = ContentValidator.SetSectionValueAndValidate( columnId, ctx );
            if (errors.HasErrors) {
                run( Add, columnId );
            }
            else {
                sectionService.Insert( section );
                echoToParentPart( lang( "opok" ) );
            }
        }

        //-------------------------------------------------------------------------

        public void AddAuto( int columnId ) {

            target( AddAutoTwo, columnId );

            IList services = ServiceContext.GetByTag( "PageSection", ctx.owner.obj.GetType().FullName );
            dropList( "serviceId", services, "Name=Id", 0 );
        }

        [HttpPost, DbTransaction]
        public void AddAutoTwo( int columnId ) {

            target( AddAutoThree, columnId );

            int serviceId = ctx.PostInt( "serviceId" );
            Service service = ServiceContext.Get( serviceId );
            bindServiceInfo( serviceId, service );
        }


        [HttpPost, DbTransaction]
        public void AddAutoThree( int columnId ) {
            target( CreateAuto, columnId );
            int serviceId = ctx.PostInt( "serviceId" );
            int templateId = ctx.PostInt( "templateId" );
            Service service = ServiceContext.Get( serviceId );
            ContentSectionTemplate template = templateService.GetById( templateId );
            template = checkJsonTemplate( template );
            bindServiceThree( serviceId, templateId, service, template );
        }

        private ContentSectionTemplate checkJsonTemplate( ContentSectionTemplate template ) {

            if (template != null) return template;
            if (ctx.owner.obj is Site == false) throw new NullReferenceException( "模板不能为空：请正确设置模板" );

            return TemplateUtil.getJsonTemplate();

        }


        [HttpPost, DbTransaction]
        public void CreateAuto( int columnId ) {
            ContentSection section = ContentValidator.SetSectionValueAndValidate( columnId, ctx );
            if (errors.HasErrors) {
                run( AddAutoThree, columnId );
            }
            else {
                sectionService.Insert( section );

                // 给参数赋值
                SectionSettingController.updateParamValues( section, sectionService, ctx );
                echoToParentPart( lang( "opok" ) );
            }
        }

        //-------------------------------------------------------------------------

        public void AddFeed( int layoutId ) {
            target( CreateFeed, layoutId );
        }

        [HttpPost, DbTransaction]
        public void CreateFeed( int layoutId ) {

            String rssUrl = ctx.Post( "Url" );
            if (strUtil.IsNullOrEmpty( rssUrl )) {
                errors.Add( "rss url can not be empty" );
                run( AddFeed, layoutId );
                return;
            }

            // 获取feed源
            FeedSource s = feedService.CreateRss( rssUrl );
            if (s == null) {
                errors.Add( "create rss error" );
                run( AddFeed, layoutId );
                return;
            }

            // 创建区块
            ContentSection section = ContentValidator.PopulateFeed( layoutId, ctx );
            String title = ctx.Post( "Title" );
            section.Title = strUtil.HasText( title ) ? title : s.Title;
            section.MoreLink = s.Link;

            // rss数据源的ID
            section.ServiceId = 18;

            // 仅使用单列列表模板
            section.TemplateId = 2;
            sectionService.Insert( section );


            // 设置参数
            int count = ctx.PostInt( "Count" );
            if (count <= 0 || count > 30) count = 5;
            updateFeedParamValues( section, rssUrl, count );

            echoToParentPart( lang( "opok" ) );
        }

        //-------------------------------------------------------------------------

        public void EditRowUI( int iRow ) {
            target( SaveRowUI, iRow );
            String name = "#row" + iRow;
            bindCssForm( name );
        }

        [HttpPost, DbTransaction]
        public void SaveRowUI( int iRow ) {
            String name = "#row" + iRow;
            Dictionary<string, string> result = CssFormUtil.getPostValues( ctx );
            updateValues( name, result );
        }

        public void EditUI( int layoutId ) {

            target( SaveUI, layoutId );

            String name = getCoulumnName( layoutId );
            bindCssForm( name );
        }

        [HttpPost, DbTransaction]
        public void SaveUI( int layoutId ) {

            String name = getCoulumnName( layoutId );
            Dictionary<string, string> result = CssFormUtil.getPostValues( ctx );
            updateValues( name, result );
        }


        private static String getCoulumnName( int layoutId ) {
            String layoutStr = layoutId.ToString();
            int rowId = cvt.ToInt( layoutStr.Substring( 0, layoutStr.Length - 1 ) );
            int columnId = cvt.ToInt( layoutStr.Substring( layoutStr.Length - 1, 1 ) );

            String name = string.Format( "#row{0}_column{1}", rowId, columnId );
            return name;
        }


        public void EditSectionUI( int sectionId ) {

            target( SaveSectionUI, sectionId );
            String name = "#section" + sectionId;
            bindCssForm( name );

            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );

            set( "x.CssClass", section.CssClass );
            set( "cssClassSaveLink", to( SaveCssClass, sectionId ) );
        }

        public void SaveCssClass( int sectionId ) {
            String cssClass = strUtil.CutString( ctx.Post( "cssClass" ), 50 );

            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );
            section.CssClass = cssClass;
            sectionService.Update( section );

            echoRedirect( lang( "opok" ) );
        }

        [HttpPost, DbTransaction]
        public void SaveSectionUI( int sectionId ) {
            String name = "#section" + sectionId;
            Dictionary<string, string> result = CssFormUtil.getPostValues( ctx );
            updateValues( name, result );
        }

        public void EditSectionTitleUI( int sectionId ) {

            target( SaveSectionTitleUI, sectionId );
            String name = "#sectionTitle" + sectionId;
            bindCssForm( name );
        }

        [HttpPost, DbTransaction]
        public void SaveSectionTitleUI( int sectionId ) {
            String name = "#sectionTitle" + sectionId;
            Dictionary<string, string> result = CssFormUtil.getPostValues( ctx );
            updateValues( name, result );
        }

        //
        public void EditSectionContentUI( int sectionId ) {

            target( SaveSectionContentUI, sectionId );
            String name = "#sectionContent" + sectionId;
            bindCssForm( name );
        }

        [HttpPost, DbTransaction]
        public void SaveSectionContentUI( int sectionId ) {
            String name = "#sectionContent" + sectionId;
            Dictionary<string, string> result = CssFormUtil.getPostValues( ctx );
            updateValues( name, result );
        }

        private void bindCssForm( String name ) {
            ContentApp app = ctx.app.obj as ContentApp;
            Dictionary<string, Dictionary<string, string>> dic = Css.FromAndFill( app.Style );
            Dictionary<string, string> values = dic.ContainsKey( name ) ? dic[name] : CssInfo.GetEmptyValues();

            ctx.SetItem( "cssValues", values );
            load( "cssForm", CssForm );
        }

        [NonVisit]
        public void CssForm() {
            Dictionary<string, string> values = ctx.GetItem( "cssValues" ) as Dictionary<string, string>;
            bind( "v", values );
        }

        private void updateValues( String name, Dictionary<string, string> result ) {
            ContentApp app = ctx.app.obj as ContentApp;

            String newStyle = CssFormUtil.mergeStyle( app.Style, name, result );
            app.Style = newStyle;
            db.update( app, "Style" );

            echoToParentPart( lang( "opok" ) );
        }

        //-------------------------------------------------------------------------


        [HttpDelete, DbTransaction]
        public void Delete( int id ) {
            ContentSection section = sectionService.GetById( id, ctx.app.Id );
            if (section == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }
            sectionService.Delete( section );
            echoRedirectPart( lang( "opok" ), to( new ContentController().Home ), 0 );
        }

        //-------------------------------------------------------------------------

        public void Combine( int sectionId ) {
            target( SaveCombine, sectionId );
            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );
            List<ContentSection> sections = sectionService.GetForCombine( section );
            dropList( "targetSection", sections, "Title=Id", 0 );
        }

        public void SaveCombine( int sectionId ) {
            int targetSectionId = ctx.PostInt( "targetSection" );
            sectionService.CombineSections( sectionId, targetSectionId );
            echoToParentPart( lang( "opok" ) );
        }

        public void RemoveSection( int sectionId ) {
            int targetSectionId = ctx.GetInt( "targetSection" );
            sectionService.RemoveSection( targetSectionId, sectionId );
            echoToParentPart( lang( "opok" ) );
        }

        //-------------------------------------------------------------------------

        public void EditEffect( int sectionId ) {
            target( SaveEffect, sectionId );
            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );

            Dictionary<String, String> dic = new Dictionary<string, string>();
            dic.Add( "不滚动", "" );
            dic.Add( "朝上滚动", "up" );
            dic.Add( "朝左滚动", "left" );
            dic.Add( "朝下滚动", "down" );
            dic.Add( "朝右滚动", "right" );
            radioList( "marquee", dic, section.GetMarquee() );
        }

        public void SaveEffect( int sectionId ) {

            String marquee = ctx.Post( "marquee" );
            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );
            section.SetMarquee( marquee );
            echoToParentPart( lang( "opok" ) );
        }

        //-------------------------------------------------------------------------


        private void updateFeedParamValues( ContentSection section, String url, int count ) {
            String val = string.Format( "param0={0};param1={1}", url, count );
            section.ServiceParams = val;
            sectionService.Update( section );
        }


    }
}

