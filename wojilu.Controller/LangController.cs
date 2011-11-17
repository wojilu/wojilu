/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Controller {

    public class LangController : ControllerBase {

        [HttpPut, DbTransaction]
        public void Switch() {

            String langStr = ctx.Get( "lang" );
            ctx.web.CookieSetLang( langStr );

            redirectUrl( ctx.web.PathReferrer.ToString() );
        }

    }

}
