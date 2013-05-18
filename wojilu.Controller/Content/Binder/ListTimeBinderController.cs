/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.Web.Mvc;
using wojilu.Common;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;
using wojilu.Web.Controller.Content.Utils;

namespace wojilu.Web.Controller.Content.Binder {

    public class ListTimeBinderController : ControllerBase, ISectionBinder {

        public void Bind( ContentSection section, IList serviceData ) {

            IBlock block = base.getBlock( "list" );

            foreach (IBinderValue item in serviceData) {

                block.Set( "post.Title", item.Title );
                block.Set( "post.Url", item.Link );

                block.Set( "post.Created", item.Created );
                block.Set( "post.CreatedDay", item.Created.ToShortDateString() );
                block.Set( "post.CreatedTime", item.Created.ToShortTimeString() );

                block.Set( "post.CreatorName", item.CreatorName );
                block.Set( "post.CreatorLink", item.CreatorLink );
                block.Set( "post.CreatorPic", item.CreatorPic );

                block.Set( "post.Content", item.Content );
                block.Set( "post.Summary", strUtil.CutString( item.Content, 200 ) );
                block.Set( "post.PicUrl", item.PicUrl );
                block.Set( "post.Replies", item.Replies );

                block.Bind( "post", item );

                block.Next();

            }

        }

    }
}

