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

        public void Bind( ContentSection section, IList serviceData ) {

            IBlock block = base.getBlock( "list" );

            foreach (IBinderValue item in serviceData) {

                block.Bind( "d", item );

                IBlock tcBlock = block.GetBlock( "titleAndContent" );
                IBlock onlyBlock = block.GetBlock( "onlyTitle" );


                if (strUtil.HasText( item.Content )) {
                    tcBlock.Set( "d.Title", strUtil.CutString( item.Title, 20 ) );
                    tcBlock.Set( "d.Content", strUtil.ParseHtml( item.Content, 150 ) );
                    tcBlock.Set( "d.Created", item.Created );
                    tcBlock.Bind( "c", item );
                    tcBlock.Next();
                }
                else {
                    onlyBlock.Set( "d.Title", item.Title );
                    onlyBlock.Bind( "c", item );
                    onlyBlock.Set( "d.Created", item.Created );
                    onlyBlock.Next();
                }

                block.Next();
            }

        }


    }
}
