using System;
using System.Collections;
using System.Text;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;
using wojilu.SOA;
using wojilu.SOA.Controls;
using wojilu.Web.Context;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.DI;

namespace wojilu.Web.Controller.Content.Admin {

    [App( typeof( ContentApp ) )]
    public class SectionSettingController : ControllerBase {

        public IContentSectionService sectionService { get; set; }
        public IContentCustomTemplateService ctService { get; set; }
        public IContentSectionTemplateService templatelService { get; set; }

        public SectionSettingController() {
            sectionService = new ContentSectionService();
            ctService = new ContentCustomTemplateService();
            templatelService = new ContentSectionTemplateService();
        }

        public void Edit( int sectionId ) {

            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );
            if (section == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            target( Update, sectionId );

            set( "section.Title", section.Title );
            set( "section.MoreLink", section.MoreLink );

            set( "section.MetaKeywords", section.MetaKeywords );
            set( "section.MetaDescription", section.MetaDescription );
        }

        [HttpPost, DbTransaction]
        public void Update( int id ) {
            ContentSection section = sectionService.GetById( id, ctx.app.Id );
            if (section == null) {
                echoToParentPart( lang( "exDataNotFound" ) );
                return;
            }

            section.Title = ctx.Post( "Title" );
            section.MoreLink = strUtil.CutString( ctx.PostHtml( "MoreLink" ), 250 );
            if (strUtil.IsNullOrEmpty( section.Title )) {
                errors.Add( lang( "exName" ) );
                run( Edit, id );
                return;
            }
            else {
                section.Title = strUtil.SubString( section.Title, 50 );
            }

            section.MetaKeywords = ctx.Post( "MetaKeywords" );
            section.MetaDescription = strUtil.CutString( ctx.Post( "MetaDescription" ), 250 );


            sectionService.Update( section );

            echoToParentPart( lang( "opok" ) );
        }

        //-----------------------------------------------------------------------------------------------------


        public void EditCount( int sectionId ) {

            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );
            if (section == null) {
                echoRedirect( sectionNotFound( sectionId )  );
                return;
            }

            target( SaveCount, sectionId );

            set( "count", section.ListCount );

            set( "section.Title", section.Title );
            set( "section.MoreLink", section.MoreLink );

            set( "section.MetaKeywords", section.MetaKeywords );
            set( "section.MetaDescription", section.MetaDescription );

        }

        [HttpPost, DbTransaction]
        public void SaveCount( int sectionId ) {

            ContentSection s = sectionService.GetById( sectionId, ctx.app.Id );
            if (s == null) {
                throw new Exception( sectionNotFound( sectionId ) );
            }

            s.Title = ctx.Post( "Title" );
            s.MoreLink = strUtil.CutString( ctx.PostHtml( "MoreLink" ), 250 );
            s.MetaKeywords = ctx.Post( "MetaKeywords" );
            s.MetaDescription = strUtil.CutString( ctx.Post( "MetaDescription" ), 250 );

            if (strUtil.IsNullOrEmpty( s.Title )) {
                errors.Add( lang( "exName" ) );
                run( EditCount, sectionId );
                return;
            }
            else {
                s.Title = strUtil.SubString( s.Title, 50 );
            }

            int count = ctx.PostInt( "Count" );
            if (count > 2000) count = 0;
            s.ListCount = count;

            sectionService.Update( s );

            echoToParentPart( lang( "opok" ) );
        }

        //-----------------------------------------------------------------------------------------------------

        public void EditBinder( int id ) {

            ContentSection section = sectionService.GetById( id, ctx.app.Id );
            if (section == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            target( UpdateBinder, id );

            Service service = ServiceContext.Get( section.ServiceId );
            bindSettingEdit( section, service );


            set( "section.Title", section.Title );
            set( "section.MoreLink", section.MoreLink );

            set( "section.MetaKeywords", section.MetaKeywords );
            set( "section.MetaDescription", section.MetaDescription );

        }

        private void bindSettingEdit( ContentSection section, Service service ) {
            StringBuilder builder = new StringBuilder();
            IList parms = service.GetParams();
            for (int i = 0; i < parms.Count; i++) {
                ParamControl control = parms[i] as ParamControl;
                control.Value = control.ChangeType( section.GetServiceParamValue( "param" + i ) ).ToString();

                builder.Append( "<div>" );
                builder.Append( control.Html );
                builder.Append( "</div>" );
            }
            set( "settingList", builder.ToString() );
        }


        [HttpPost, DbTransaction]
        public void UpdateBinder( int id ) {
            ContentSection section = sectionService.GetById( id, ctx.app.Id );
            if (section == null) {
                echoToParentPart( lang( "exDataNotFound" ) );
                return;
            }

            int tplId = section.TemplateId;
            int cTplId = section.CustomTemplateId;

            section = ContentValidator.ValidateSectionEdit( section, ctx );
            if (errors.HasErrors) {
                run( EditBinder, id );
                return;
            }

            // 此处不修改模板
            section.TemplateId = tplId;
            section.CustomTemplateId = cTplId;

            section.MetaKeywords = ctx.Post( "MetaKeywords" );
            section.MetaDescription = strUtil.CutString( ctx.Post( "MetaDescription" ), 250 );


            if (section.ServiceId > 0) {
                sectionService.Update( section );
                updateParamValues( section, sectionService, ctx );// 修改数据源参数
            }

            echoToParentPart( lang( "opok" ) );
        }

        public static void updateParamValues( ContentSection section, IContentSectionService sectionService, MvcContext ctx ) {
            IList parms = ServiceContext.Get( section.ServiceId ).GetParams();
            int count = parms.Count;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < count; i++) {
                ParamControl control = parms[i] as ParamControl;
                builder.Append( "param" );
                builder.Append( i );
                builder.Append( "=" );
                String strVal = ctx.Post( "param" + i );
                object val = control.ChangeType( strVal );
                builder.Append( val );
                if (i < (count - 1)) {
                    builder.Append( ";" );
                }
            }
            section.ServiceParams = builder.ToString();
            sectionService.Update( section );
        }

        private String sectionNotFound( int id ) {
            return lang( "exDataNotFound" ) + ":ContentSection, id=" + id;
        }


    }






}
