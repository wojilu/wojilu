/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;

namespace wojilu.Web.Context.Initor {

    public class ControllerInit : IContextInit {

        public void Init( MvcContext ctx ) {

            if (ctx.utils.isEnd()) return;

            ControllerBase controller = ControllerFactory.InitController( ctx );

            if (controller == null) {
                String typeName = ctx.route.getControllerNameWithoutRootNamespace();
                String msg = lang.get( "exControllerNotExist" ) + ": " + typeName;

                ctx.utils.endMsg( msg, HttpStatus.NotFound_404 );
                return;

            }

            ctx.utils.setController( controller );
        }


    }

}
