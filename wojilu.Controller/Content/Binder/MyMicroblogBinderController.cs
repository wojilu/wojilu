/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Domain;
using wojilu.Web.Controller.Users;
using wojilu.Common.Microblogs.Domain;
using wojilu.Apps.Content.Service;
using wojilu.Web.Controller.Content.Utils;

namespace wojilu.Web.Controller.Content.Binder {

    public class MyMicroblogBinderController : ControllerBase, ISectionBinder {

        public void Bind( ContentSection section, IList serviceData ) {

            load( "publisher", new Microblogs.My.MicroblogController().Publisher );


            set( "user.Name", ctx.owner.obj.Name );
            set( "blog.MoreLink", alink.ToUserMicroblog(ctx.owner.obj) );

            IBlock block = getBlock( "list" );
            foreach (Microblog blog in serviceData) {

                block.Set( "blog.Content", blog.Content );
                block.Set( "blog.Created", cvt.ToTimeString( blog.Created ) );

                block.Set( "commentInfo", "" );

                block.Bind( "blog", blog );

                block.Next();

            }

        }


    }

}
