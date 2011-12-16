/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.SOA;
using wojilu.SOA.Controls;
using wojilu.Apps.Shop.Domain;
using wojilu.Web.Controller.Shop.Section;
using System.Collections.Generic;
using wojilu.Members.Sites.Domain;
using wojilu.Web.Controller.Shop.Utils;
using wojilu.Web.Context;
using wojilu.Apps.Shop.Interface;

namespace wojilu.Web.Controller.Shop.Admin {

    public partial class ShopSectionController : ControllerBase {

        private void bindServiceInfo( int serviceId, Service service ) {
            set( "serviceId", serviceId );
            set( "service.Name", service.Name );

            // List参数用于设置默认选择项为List模板
            set( "templateType", TemplateUtil.getServiceTemplates( service.Note, "List", ctx, templateService ) );
        }

        private void bindServiceThree( int serviceId, int templateId, Service service, ShopSectionTemplate template ) {
            set( "service.Id", serviceId );
            set( "service.Name", service.Name );
            set( "template.Id", templateId );
            set( "template.Name", template.Name );

            StringBuilder builder = new StringBuilder();
            IList parms = service.GetParams();
            for (int i = 0; i < parms.Count; i++) {
                ParamControl control = parms[i] as ParamControl;

                builder.Append( "<div>" );
                builder.Append( control.Html );
                builder.Append( "</div>" );
            }
            set( "settingList", builder.ToString() );
        }



    }
}

