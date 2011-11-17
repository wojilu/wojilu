/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.DI;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;
using wojilu.Web.Controller.Common;
using wojilu.SOA;
using wojilu.Common.AppBase;
using wojilu.Web.Controller.Content.Utils;
using System.Collections;
using wojilu.Serialization;

namespace wojilu.Web.Controller.Content {

    [App( typeof( ContentApp ) )]
    public class SectionDataController : ControllerBase {


        public IContentPostService postService { get; set; }
        public IContentSectionService sectionService { get; set; }
        public IContentSectionTemplateService TplService { get; set; }
        public IContentCustomTemplateService ctService { get; set; }

        public SectionDataController() {

            postService = new ContentPostService();
            sectionService = new ContentSectionService();
            TplService = new ContentSectionTemplateService();
            ctService = new ContentCustomTemplateService();
        }

        public void Show( int sectionId ) {

            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );

            actionContent( getSectionContent( section ) );

        }



        // TODO 封装
        private String getSectionContent( ContentSection section ) {
            String content;
            if (section.ServiceId <= 0)
                content = getData( section );
            else
                content = getAutoData( section );
            return content;
        }

        private String getData( ContentSection articleSection ) {
            IPageSection section = BinderUtils.GetPageSection( articleSection, ctx, "SectionShow" );
            ControllerBase sectionController = section as ControllerBase;
            section.SectionShow( articleSection.Id );
            String actionContent = sectionController.utils.getActionResult();
            return actionContent;
        }

        private String getAutoData( ContentSection section ) {

            Dictionary<string, string> presult = getDefaultValue( section );

            IList data = ServiceContext.GetData( section.ServiceId, section.GetServiceParamValues(), presult );

            if (section.TemplateId <= 0) return getJsonResult( section, data );

            ContentSectionTemplate sectionTemplate = TplService.GetById( section.TemplateId );
            Template currentView = utils.getTemplateByFileName( BinderUtils.GetBinderTemplatePath( sectionTemplate ) );
            ISectionBinder binder = BinderUtils.GetBinder( sectionTemplate, ctx, currentView );
            binder.Bind( section, data ); // custom template : SectionUtil.loadTemplate
            ControllerBase sectionController = binder as ControllerBase;
            return sectionController.utils.getActionResult();
        }

        private String getJsonResult( ContentSection section, IList data ) {

            String jsonStr = JsonString.ConvertList( data );
            String scriptData = string.Format( "	<script>var sectionData{0} = {1};</script>", section.Id, jsonStr );
            if (section.CustomTemplateId <= 0)
                return scriptData;

            ContentCustomTemplate ct = ctService.GetById( section.CustomTemplateId, ctx.owner.Id );
            if (ct == null) return scriptData;

            return scriptData + ct.Content;
        }

        private Dictionary<string, string> getDefaultValue( ContentSection section ) {
            Service service = ServiceContext.Get( section.ServiceId );

            Dictionary<string, string> pd = service.GetParamDefault();
            Dictionary<string, string> presult = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> pair in pd) {
                if (pair.Key.Equals( "ownerId" ))
                    presult.Add( pair.Key, ctx.owner.Id.ToString() );
                else if (pair.Key.Equals( "viewerId" ))
                    presult.Add( pair.Key, ctx.viewer.Id.ToString() );
                else
                    presult.Add( pair.Key, pair.Key );
            }
            return presult;
        }















    }
}
