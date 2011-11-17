using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;
using wojilu.Members.Users.Domain;
using wojilu.DI;

namespace wojilu.Web.Controller.Content.Admin {

    [App( typeof( ContentApp ) )]
    public class TemplateCustomController : ControllerBase {

        public IContentSectionService sectionService { get; set; }
        public IContentCustomTemplateService ctService { get; set; }
        public IContentSectionTemplateService templatelService { get; set; }

        public TemplateCustomController() {
            sectionService = new ContentSectionService();
            ctService = new ContentCustomTemplateService();
            templatelService = new ContentSectionTemplateService();
        }


        public void Edit( int sectionId ) {

            ContentSection s = sectionService.GetById( sectionId, ctx.app.Id );
            if (s == null) {
                throw new Exception( sectionNotFound( sectionId ) );
            }

            target( Save, sectionId );
            set( "resetLink", to( Reset, sectionId ) );

            String templateContent = getTemplateContent( s );
            templateContent = strUtil.EncodeTextarea( templateContent );
            set( "templateContent", templateContent );
        }

        private String getTemplateContent( ContentSection s ) {

            if (s.CustomTemplateId > 0) {
                ContentCustomTemplate ct = ctService.GetById( s.CustomTemplateId, ctx.owner.Id );
                if (ct != null) return ct.Content;
            }

            String tpath = getTemplatePath( s );
            String absPath = base.utils.getTemplatePathByFile( tpath );

            String templateContent = file.Read( absPath );
            return templateContent;
        }

        // wojilu.Web.Controller.Content.Section.TextController => Content/Section/Text/SectionShow
        private static String getTemplatePath( ContentSection s ) {
            String tpath = strUtil.TrimStart( s.SectionType, "wojilu.Web.Controller." );
            tpath = strUtil.TrimEnd( tpath, "Controller" );
            tpath = tpath.Replace( ".", "/" );

            tpath = strUtil.Join( tpath, "SectionShow" );

            return tpath;
        }

        //-------------------------------------------------------------------------------------------------------------------------
        

        public void EditBinder( int sectionId ) {

            ContentSection s = sectionService.GetById( sectionId, ctx.app.Id );
            if (s == null) {
                throw new Exception( sectionNotFound( sectionId ) );
            }

            set( "selectLink", to( new TemplateController().Select, sectionId ) );

            target( Save, sectionId );
            set( "resetLink", to( Reset, sectionId ) );

            String templateContent = getBinderTemplateContent( s );
            templateContent = strUtil.EncodeTextarea( templateContent );
            set( "templateContent", templateContent );
        }

        private string getBinderTemplateContent( ContentSection s ) {

            if (s.CustomTemplateId > 0) {
                ContentCustomTemplate ct = ctService.GetById( s.CustomTemplateId, ctx.owner.Id );
                if (ct != null) return ct.Content;
            }

            if (s.TemplateId <= 0) {
                return "";
            }

            String tpath = getBinderTemplatePath( s );
            String absPath = base.utils.getTemplatePathByFile( tpath );

            String templateContent = file.Read( absPath );
            return templateContent;

        }

        // Content/Binder/List
        private string getBinderTemplatePath( ContentSection s ) {

            ContentSectionTemplate tpl = templatelService.GetById( s.TemplateId );
            return "Content/Binder/" + tpl.TemplateName;
        }

        //-------------------------------------------------------------------------------------------------------------------------



        [HttpPost, DbTransaction]
        public void Save( int sectionId ) {

            ContentSection s = sectionService.GetById( sectionId, ctx.app.Id );
            if (s == null) {
                throw new Exception( sectionNotFound( sectionId ) );
            }

            if (s.CustomTemplateId > 0) {
                updateTemplate( s );
            }
            else {
                createAndUpdateSection( s );
            }

            echoToParentPart( lang( "opok" ) );
        }

        private void updateTemplate( ContentSection s ) {

            ContentCustomTemplate ct = ctService.GetById( s.CustomTemplateId, ctx.owner.Id );

            ct.Content = ctx.PostHtml( "Content" );

            ctService.Update( ct );

        }

        private void createAndUpdateSection( ContentSection s ) {

            // 创建一个template
            ContentCustomTemplate ct = new ContentCustomTemplate();
            ct.Creator = (User)ctx.viewer.obj;
            ct.OwnerId = ctx.owner.Id;
            ct.OwnerType = ctx.owner.obj.GetType().FullName;
            ct.OwnerUrl = ctx.owner.obj.Url;

            String name = ctx.Post( "Name" );
            if (strUtil.IsNullOrEmpty( name )) {
                name = string.Format( "section{0}({1})", s.Id, s.Title );
            }

            ct.Name = strUtil.SubString( name, 50 );
            ct.Description = strUtil.SubString( ctx.Post( "Description" ), 250 );
            ct.Content = ctx.PostHtml( "Content" );

            ctService.Insert( ct );

            // 更新section的模板
            s.CustomTemplateId = ct.Id;
            sectionService.Update( s );
        }

        [HttpPut, DbTransaction]
        public void Reset( int sectionId ) {

            ContentSection s = sectionService.GetById( sectionId, ctx.app.Id );
            if (s == null) {
                throw new Exception( sectionNotFound( sectionId ) );
            }

            s.CustomTemplateId = 0;
            sectionService.Update( s );

            echoToParentPart( lang( "opok" ) );
        }

        private String sectionNotFound( int id ) {
            return lang( "exDataNotFound" ) + ":ContentSection, id=" + id;
        }


    }

}
