/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Common;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Domain;
using wojilu.Web.Controller.Content.Utils;
using wojilu.Apps.Content.Service;

namespace wojilu.Web.Controller.Content.Binder {

    public class ExcerptBinderController : ControllerBase, ISectionBinder {

        public IContentCustomTemplateService ctService { get; set; }

        public ExcerptBinderController() {
            ctService = new ContentCustomTemplateService();
        }

        public void Bind( ContentSection section, IList serviceData ) {


            TemplateUtil.loadTemplate( this, section, ctService );

            IBlock block = base.getBlock( "list" );

            foreach (IBinderValue item in serviceData) {


                block.Set( "d.CreatorName", item.CreatorName );
                block.Set( "d.CreatorLink", item.CreatorLink );
                block.Set( "d.CreatorFace", item.CreatorPic );

                block.Set( "d.Title", strUtil.CutString( item.Title, 20 ) );
                block.Set( "d.LinkShow", item.Link );
                block.Set( "d.Content", strUtil.ParseHtml( item.Content, 150 ) );

                block.Bind( "d", item );

                block.Next();
            }

        }


    }
}
