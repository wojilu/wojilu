/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Interface;
using wojilu.Web.Context;
using wojilu.Web.Context.Initor;

namespace wojilu.Installer {

    public class InstallerFilter : IMvcFilter {

        public void Process( wojilu.Web.Mvc.MvcEventPublisher publisher ) {

            publisher.Begin_ParseRoute += new EventHandler<MvcEventArgs>( publisher_Begin_ParseRoute );
            publisher.Begin_InitContext += new EventHandler<MvcEventArgs>( publisher_Begin_InitContext );

        }

        void publisher_Begin_ParseRoute( object sender, MvcEventArgs e ) {

            if (config.Instance.Site.IsInstall) return;

            MvcContext ctx = e.ctx;

            if (ctx.url.Path.ToLower().IndexOf( "/installer" ) <0) {

                String installerUrl = "/Installer/Index" + MvcConfig.Instance.UrlExt;
                ctx.setUrl( strUtil.Join( ctx.url.SiteAndAppPath, installerUrl ) );

            }
        }

        void publisher_Begin_InitContext( object sender, MvcEventArgs e ) {

            if (config.Instance.Site.IsInstall) return;

            MvcContext ctx = e.ctx;
            if (ctx.utils.isSkipCurrentProcessor()) return;
            ContextInitDefault init = new ContextInitDefault();
            init.InitController( ctx );
            ctx.utils.skipCurrentProcessor( true ); // 跳过下面正常的init
        }



    }

}
