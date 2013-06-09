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
        public IContentCustomTemplateService ctService { get; set; }

        public ContentController() {
            SectionService = new ContentSectionService();
            ctService = new ContentCustomTemplateService();
        }

        [CachePage( typeof( ContentIndexPageCache ) )]
        [CacheAction( typeof( ContentIndexCache ) )]
        public void Index() {

            ContentApp app = ctx.app.obj as ContentApp;
            ContentSetting setting = app.GetSettingsObj();

            ctx.Page.Title = ctx.app.Name;
            ctx.Page.Description = setting.MetaDescription;

            if (strUtil.HasText( setting.MetaKeywords )) {
                this.Page.Keywords = setting.MetaKeywords;
            }
            else {
                this.Page.Keywords = ctx.app.Name;
            }

            set( "app.Style", app.Style );
            set( "app.SkinStyle", app.SkinStyle );
            set( "lnkSendPost", to( new Submit.PostController().Index ) );

            List<ContentSection> sections = SectionService.GetByApp( ctx.app.Id );
            bindRows( app, sections );

        }

    }
}

