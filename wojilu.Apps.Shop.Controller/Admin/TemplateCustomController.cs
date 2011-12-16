using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Shop.Domain;
using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Service;
using wojilu.Members.Users.Domain;
using wojilu.DI;

namespace wojilu.Web.Controller.Shop.Admin {

    [App( typeof( ShopApp ) )]
    public class TemplateCustomController : ControllerBase {

        public IShopSectionService sectionService { get; set; }
        public IShopCustomTemplateService ctService { get; set; }
        public IShopSectionTemplateService templatelService { get; set; }

        public TemplateCustomController() {
            sectionService = new ShopSectionService();
            ctService = new ShopCustomTemplateService();
            templatelService = new ShopSectionTemplateService();
        }


        public void Edit( int sectionId ) {

            ShopSection s = sectionService.GetById( sectionId, ctx.app.Id );
            if (s == null) {
                throw new Exception( sectionNotFound( sectionId ) );
            }

            target( Save, sectionId );
            set( "resetLink", to( Reset, sectionId ) );

            String templateContent = getTemplateContent( s );
            templateContent = strUtil.EncodeTextarea( templateContent );
            set( "templateContent", templateContent );
        }

        private String getTemplateContent( ShopSection s ) {

            if (s.CustomTemplateId > 0) {
                ShopCustomTemplate ct = ctService.GetById( s.CustomTemplateId, ctx.owner.Id );
                if (ct != null) return ct.Content;
            }

            String tpath = getTemplatePath( s );
            String absPath = base.utils.getTemplatePathByFile( tpath );

            String templateContent = file.Read( absPath );
            return templateContent;
        }

        // wojilu.Web.Controller.Shop.Section.TextController => Content/Section/Text/SectionShow
        private static String getTemplatePath( ShopSection s ) {
            String tpath = strUtil.TrimStart( s.SectionType, "wojilu.Web.Controller." );
            tpath = strUtil.TrimEnd( tpath, "Controller" );
            tpath = tpath.Replace( ".", "/" );

            tpath = strUtil.Join( tpath, "SectionShow" );

            return tpath;
        }

        //-------------------------------------------------------------------------------------------------------------------------
        

        public void EditBinder( int sectionId ) {

            ShopSection s = sectionService.GetById( sectionId, ctx.app.Id );
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

        private string getBinderTemplateContent( ShopSection s ) {

            if (s.CustomTemplateId > 0) {
                ShopCustomTemplate ct = ctService.GetById( s.CustomTemplateId, ctx.owner.Id );
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
        private string getBinderTemplatePath( ShopSection s ) {

            ShopSectionTemplate tpl = templatelService.GetById( s.TemplateId );
            return "Content/Binder/" + tpl.TemplateName;
        }

        //-------------------------------------------------------------------------------------------------------------------------



        [HttpPost, DbTransaction]
        public void Save( int sectionId ) {

            ShopSection s = sectionService.GetById( sectionId, ctx.app.Id );
            if (s == null) {
                throw new Exception( sectionNotFound( sectionId ) );
            }

            if (s.CustomTemplateId > 0) {
                updateTemplate( s );
            }
            else {
                createAndUpdateSection( s );
            }

            echoToParentPart(lang("opok"));
        }

        private void updateTemplate( ShopSection s ) {

            ShopCustomTemplate ct = ctService.GetById( s.CustomTemplateId, ctx.owner.Id );

            ct.Content = ctx.PostHtml("Content");

            ctService.Update( ct );

        }

        private void createAndUpdateSection( ShopSection s ) {

            // 创建一个template
            ShopCustomTemplate ct = new ShopCustomTemplate();
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
            ct.Content = ctx.PostHtml("Content");

            ctService.Insert( ct );

            // 更新section的模板
            s.CustomTemplateId = ct.Id;
            sectionService.Update( s );
        }

        [HttpPut, DbTransaction]
        public void Reset( int sectionId ) {

            ShopSection s = sectionService.GetById( sectionId, ctx.app.Id );
            if (s == null) {
                throw new Exception( sectionNotFound( sectionId ) );
            }

            s.CustomTemplateId = 0;
            sectionService.Update( s );

            echoToParentPart(lang("opok"));
        }

        private String sectionNotFound( int id ) {
            return lang( "exDataNotFound" ) + ":ShopSection, id=" + id;
        }

    }

}
