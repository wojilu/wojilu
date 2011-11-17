/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Web.Mvc.Interface;
using wojilu.Web.Context.Initor;

namespace wojilu.Web.Context {

    public class ContextInit : IMvcFilter {

        public void Process( wojilu.Web.Mvc.MvcEventPublisher publisher ) {

            publisher.Begin_InitContext += new EventHandler<wojilu.Web.Mvc.MvcEventArgs>( publisher_Begin_InitContext );
        }

        void publisher_Begin_InitContext( object sender, wojilu.Web.Mvc.MvcEventArgs e ) {

            MvcContext ctx = e.ctx;

            if (ctx.utils.isSkipCurrentProcessor()) return;

            InitFactory.GetViewer().Init( ctx );
            InitFactory.GetOwner().Init( ctx );
            InitFactory.GetController().Init( ctx );
            InitFactory.GetApp().Init( ctx );
            InitFactory.GetOnlineUser().Init( ctx );


            ctx.utils.skipCurrentProcessor( true );


        }


    }
}
