/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Common.Spider.Domain;
using wojilu.Common.Spider.Service;

namespace wojilu.Web.Controller.Admin.Spiders {

    public class LayoutController : ControllerBase{

        public SpiderTemplateService templateService { get; set; }

        public LayoutController() {
            templateService = new SpiderTemplateService();
        }

        public override void Layout() {

            set( "templateList", to( new TemplateController().List ) );
            set( "dataAll", to( new ArticleController().List, 0 ) );
            set( "logUrl", to( new LogController().List ) );
            set( "importList", to( new ImportController().List ) );

            List<SpiderTemplate> templateList = templateService.GetAll();

            IBlock block = getBlock( "list" );
            foreach (SpiderTemplate s in templateList) {
                block.Set( "template.Title", s.SiteName );
                block.Set( "template.Url", to( new ArticleController().List, s.Id ) );
                block.Next();
            }

        }
    }

}
