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
using wojilu.Apps.Content.Service;

namespace wojilu.Web.Controller.Content.Binder {

    public class ImgBinderController : ControllerBase, ISectionBinder {

        public IContentCustomTemplateService ctService { get; set; }
        public ImgBinderController() {
            ctService = new ContentCustomTemplateService();
        }

        public void Bind( ContentSection section, IList serviceData ) {

            IBlock block = base.getBlock( "list" );

            foreach (IBinderValue item in serviceData) {

                block.Set( "post.Title", strUtil.SubString( item.Title, 10 ) );
                block.Set( "post.Url", item.Link );
                block.Set( "post.PicUrl", item.PicUrl );

                block.Bind( "post", item );

                block.Next();
            }

        }

    }

}
