/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Caching;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;
using wojilu.Web.Controller.Common;
using wojilu.Web.Controller.Content.Caching;

namespace wojilu.Web.Controller.Content {

    [App( typeof( ContentApp ) )]
    public partial class ContentController : ControllerBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( ContentController ) );

        public IContentSectionService SectionService { get; set; }
        public IContentSectionTemplateService TplService { get; set; }
        public IContentCustomTemplateService ctService { get; set; }

        public ContentController() {
            SectionService = new ContentSectionService();
            TplService = new ContentSectionTemplateService();
            ctService = new ContentCustomTemplateService();
        }

        [CachePage( typeof( ContentIndexPageCache ) )]
        [CacheAction( typeof( ContentIndexCache ) )]
        public void Index() {

            ContentApp app = ctx.app.obj as ContentApp;
            ContentSetting setting = app.GetSettingsObj();
            if (setting.CacheSeconds > 0) {
                String content = loadFromCache();
                if (strUtil.IsNullOrEmpty( content )) {
                    content = loadHtml( IndexPage );
                    SysCache.Put( getKey(), content, setting.CacheSeconds );
                }
                actionContent( content );
            }
            else {
                run( IndexPage );
            }
        }

        private string getKey() {
            return typeof( ContentApp ).FullName + "_" + ctx.app.Id;
        }

        private string loadFromCache() {
            Object objCache = SysCache.Get( getKey() );
            if (objCache == null) return null;
            return objCache.ToString();
        }

        [NonVisit]
        public void IndexPage() {

            WebUtils.pageTitle( this, ctx.app.Name );

            ContentApp app = ctx.app.obj as ContentApp;

            set( "app.Style", app.Style );
            set( "app.SkinStyle", app.SkinStyle );

            List<ContentSection> sections = SectionService.GetByApp( ctx.app.Id );
            bindRows( app, sections );

        }


    }
}

