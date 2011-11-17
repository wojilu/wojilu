/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Content.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Content.Domain;
using wojilu.Web.Controller.Common;
using wojilu.Common;
using wojilu.Apps.Content.Service;
using wojilu.Web.Controller.Content.Utils;


namespace wojilu.Web.Controller.Content.Binder {

    public class MyBlogBinderController : ControllerBase, ISectionBinder {

        public IContentCustomTemplateService ctService { get; set; }

        public MyBlogBinderController() {
            ctService = new ContentCustomTemplateService();
        }

        public void Bind( ContentSection section, IList serviceData ) {

            TemplateUtil.loadTemplate( this, section, ctService );

            IBlock block = base.getBlock( "list" );

            foreach (IBinderValue item in serviceData) {


                block.Set( "d.Created", cvt.ToTimeString(  item.Created ));
                block.Set( "d.Replies", item.Replies );

                block.Set( "d.Title", strUtil.CutString( item.Title, 20 ) );
                block.Set( "d.LinkShow", item.Link );
                block.Set( "d.Content", strUtil.ParseHtml( item.Content, 80 ) );

                block.Bind( "d", item );

                block.Next();
            }

        }

    }

}
